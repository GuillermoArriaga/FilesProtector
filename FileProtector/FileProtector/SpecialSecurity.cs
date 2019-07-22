using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Security.Cryptography;

namespace FileProtector
{
    public static class SpecialSecurity
    {
        public static void EncryptFile(string fileName, string key)
        {
            string subfile = Application.StartupPath + "\\fileProtected.zip";
            ZipAFile(subfile, key, fileName);

            byte[] fileBytes = File.ReadAllBytes(subfile);
            BytesInterchange(ref fileBytes, key);
            int parts = 10;
            byte[][] fileParts = FileSplitter(fileBytes, parts);

            string folder = Application.StartupPath + "\\tmpFolder\\";
            Directory.CreateDirectory(folder);

            for (int i = 0; i < parts; i++)
            {
                File.WriteAllBytes(folder + "part" + i + ".bin", fileParts[i]);
            }

            ZipAFolder(fileName + ".zip", "", folder);
            SpecialDelete(folder, true);
            SpecialDelete(subfile, false);

            Process.Start(GetFileRoute(fileName));
        }

        public static void DecryptFile(string fileName, string key)
        {
            string folder = Application.StartupPath + "\\tmpFolder\\";
            Directory.CreateDirectory(folder);
            UncompressAZip(fileName, "", folder);

            string[] filePartsNames = Directory.GetFiles(folder);
            byte[][] fileParts = new byte[filePartsNames.Length][];
            for (int i = 0; i < filePartsNames.Length; i++)
            {
                fileParts[i] = File.ReadAllBytes(filePartsNames[i]);
            }

            byte[] file = FileJoinner(fileParts);
            BytesInterchange(ref file, key, true);

            string subfile = Application.StartupPath + "\\fileProtected.zip";
            File.WriteAllBytes(subfile, file);
            SpecialDelete(folder, true);

            folder = GetFileRoute(fileName);
            UncompressAZip(subfile, key, folder);
            SpecialDelete(subfile, false);

            Process.Start(GetFileRoute(fileName));
        }

        public static void SpecialDelete(string route, bool isFolder = false)
        {
            // Tryes to avoid using trash can.
            if (isFolder)
            {
                if (Directory.Exists(route))
                {
                    try
                    {
                        DirectoryInfo dir = new DirectoryInfo(route);
                        dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
                        dir.Delete(true);
                    }
                    catch
                    {
                        File.Delete(route);
                    }
                }
            }
            else
            {
                if (File.Exists(route))
                {
                    try
                    {
                        FileInfo arc = new FileInfo(route);
                        arc.Attributes = arc.Attributes & ~FileAttributes.ReadOnly;
                        arc.Delete();
                    }
                    catch
                    {
                        File.Delete(route);
                    }
                }
            }
        }

        public static void BytesInterchange(ref byte[] fileBytes, string key, bool decrypt = false)
        {
            int m = fileBytes.Length;
            int n = m - 1;
            byte swap;
            char[] operations = new char[key.Length];

            for (int k = 0; k < key.Length; k++)
            {
                // The key to encrypt readed in reverse mode is de decrypt key.
                // Each operation is self reversible:
                if (decrypt)
                {
                    operations[key.Length - 1 - k] = key[k];
                }
                else
                {
                    operations[k] = key[k];
                }
            }

            // Getting special numbers that customizes the operations from first three characters of the key
            int special1, special2, special3, value1, value2;
            if (key.Length > 3)
            {
                special1 = ((int)key[0] % 11 + (int)key[1] % 13 + (int)key[2] % 17) % 10;
                special2 = ((int)key[0] % 2 + (int)key[1] % 3 + (int)key[2] % 5) % 10;
                special3 = ((int)key[0] % 4 + (int)key[1] % 6 + (int)key[2] % 12) % 10;
            }
            else
            {
                special1 = 3;
                special2 = 5;
                special3 = 7;
            }

            for (int j = 0; j < key.Length; j++)
            {
                value1 = (((int)operations[j]) * special1 + special2 * special3) % 12;
                value2 = ((int)operations[j]) % 12 + 5;

                switch (value1)
                {
                    case 0:    // Upside-down
                        for (int i = 0; i < m / 2; i++)
                        {
                            swap = fileBytes[i];
                            fileBytes[i] = fileBytes[n - i];
                            fileBytes[n - i] = swap;
                        }
                        break;

                    case 1:    // Halfs interchange
                        for (int i = 0; i < m / 2; i++)
                        {
                            swap = fileBytes[i];
                            fileBytes[i] = fileBytes[n / 2 + 1 + i];
                            fileBytes[n / 2 + 1 + i] = swap;
                        }
                        break;

                    default:    // Interchange of two bytes inside a group of them
                        for (int i = 0; i < (m - value2); i = i + 1 + value2)
                        {
                            swap = fileBytes[i];
                            fileBytes[i] = fileBytes[i + value2];
                            fileBytes[i + value2] = swap;
                        }
                        break;
                }
            }
        }

        public static byte[][] FileSplitter(byte[] fileBytes, int amountParts = 10)
        {
            byte[][] parts = new byte[amountParts][];
            int partLength = fileBytes.Length / amountParts;
            int residuous = fileBytes.Length % amountParts;

            if (residuous == 0)
            {
                for (int i = 0; i < amountParts; i++)
                {
                    parts[i] = new byte[partLength];
                }
            }
            else
            {
                // All parts grow, except the last one
                partLength++;

                for (int i = 0; i < amountParts - 1; i++)
                {
                    parts[i] = new byte[partLength];
                }

                parts[amountParts - 1] = new byte[fileBytes.Length % partLength];

            }

            for (int i = 0, j = 0, k = 0; i < fileBytes.Length; i++, k++)
            {
                if (k == partLength)
                {
                    k = 0;
                    j++;
                }
                parts[j][k] = fileBytes[i];
            }

            return parts;
        }

        public static byte[] FileJoinner(byte[][] FileParts)
        {
            int fullLength = 0;
            for (int i = 0; i < FileParts.Length; i++)
            {
                fullLength += FileParts[i].Length;
            }

            byte[] file = new byte[fullLength];

            for (int i = 0, j = 0, k = 0; i < fullLength; i++, j++)
            {
                if (j == FileParts[k].Length)
                {
                    j = 0;
                    k++;
                }
                file[i] = FileParts[k][j];
            }

            return file;
        }

        public static byte[] HashFromFile(byte[] archivo)
        {
            byte[] hash = (SHA512.Create()).ComputeHash(archivo);
            return hash;
        }

        public static string StringHashFromFile(byte[] archivo)
        {
            byte[] hash = (SHA512.Create()).ComputeHash(archivo);

            StringBuilder hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        public static void ZipAFolder(string zipFileName, string password, string folderRoute)
        {
            ZipOutputStream outZipStream = new ZipOutputStream(File.Create(zipFileName));

            outZipStream.SetLevel(9);          // 0-9, 9 max compression
            outZipStream.Password = password;  // Optional. It could be null.

            int specialEnding = 0;
            if (!folderRoute.EndsWith("\\"))
            {
                specialEnding = 1;
            }
            FolderCompression(folderRoute, outZipStream, folderRoute.Length + specialEnding);

            outZipStream.IsStreamOwner = true;
            outZipStream.Close();
        }

        public static void ZipAFile(string zipFileName, string password, string fileName)
        {
            ZipOutputStream outZipStream = new ZipOutputStream(File.Create(zipFileName));

            outZipStream.SetLevel(9);          // 0-9, 9 max compression
            outZipStream.Password = password;  // Optional. It could be null.

            FileInfo fi = new FileInfo(fileName);

            string name = GetFileName(fileName);
            name = ZipEntry.CleanName(name);
            ZipEntry newZipElement = new ZipEntry(name);
            newZipElement.DateTime = fi.LastWriteTime;
            newZipElement.Size = fi.Length;
            outZipStream.PutNextEntry(newZipElement);

            byte[] buffer = new byte[1024 * 4];    // 4K is a recommendable buffer
            using (FileStream streamReader = File.OpenRead(fileName))
            {
                StreamUtils.Copy(streamReader, outZipStream, buffer);
            }
            outZipStream.CloseEntry();
            outZipStream.IsStreamOwner = true;
            outZipStream.Close();
        }

        private static void FolderCompression(string route, ZipOutputStream zipOutStream, int routeLength)
        {
            string[] files = Directory.GetFiles(route);

            foreach (string fileName in files)
            {
                FileInfo fi = new FileInfo(fileName);

                string name = fileName.Substring(routeLength);
                name = ZipEntry.CleanName(name);
                ZipEntry newZipElement = new ZipEntry(name);
                newZipElement.DateTime = fi.LastWriteTime;
                newZipElement.Size = fi.Length;
                zipOutStream.PutNextEntry(newZipElement);

                byte[] buffer = new byte[1024 * 4];    // 4K is a recommendable buffer
                using (FileStream streamReader = File.OpenRead(fileName))
                {
                    StreamUtils.Copy(streamReader, zipOutStream, buffer);
                }
                zipOutStream.CloseEntry();
            }

            string[] folders = Directory.GetDirectories(route);

            foreach (string folder in folders)
            {
                FolderCompression(folder, zipOutStream, routeLength);
            }
        }

        public static void UncompressAZip(string zipName, string password, string folder)
        {
            ZipFile zipFile = null;
            try
            {
                FileStream fs = File.OpenRead(zipName);
                zipFile = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zipFile.Password = password;
                }
                foreach (ZipEntry fileInZip in zipFile)
                {
                    if (!fileInZip.IsFile)
                    {
                        continue;
                    }

                    String fileZippedName = fileInZip.Name;
                    byte[] buffer = new byte[1024 * 4];    // 4K is a recommendable buffer
                    Stream zipStream = zipFile.GetInputStream(fileInZip);
                    String zipRoute = Path.Combine(folder, fileZippedName);
                    string folder1 = Path.GetDirectoryName(zipRoute);

                    if (folder1.Length > 0)
                    {
                        Directory.CreateDirectory(folder1);
                    }

                    using (FileStream streamWriter = File.Create(zipRoute))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zipFile != null)
                {
                    zipFile.IsStreamOwner = true;
                    zipFile.Close();
                }
            }
        }

        public static string GetFileName(string nameWithRoute)
        {
            for (int i = nameWithRoute.Length - 1; i > 0; i--)
            {
                if (nameWithRoute[i] == '\\')
                {
                    return (nameWithRoute.Substring(i + 1));
                }
            }
            return null;
        }

        public static string GetFileRoute(string nameWithRoute)
        {
            for (int i = nameWithRoute.Length - 1; i > 0; i--)
            {
                if (nameWithRoute[i] == '\\')
                {
                    return (nameWithRoute.Substring(0, i + 1));
                }
            }
            return null;
        }
    }
}
