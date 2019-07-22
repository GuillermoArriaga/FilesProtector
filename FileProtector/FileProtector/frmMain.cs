using System;
using System.Windows.Forms;

namespace FileProtector
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ofd.Filter = "All files|*.*";
            ofd.Title = "Select a File to Encrypt";

            if (DialogResult.OK == ofd.ShowDialog())
            {
                if (ofd.FileName.Length > 0)
                {
                    try
                    {
                        SpecialSecurity.EncryptFile(ofd.FileName, txbKey.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ofd.Filter = "All files|*.*";
            ofd.Title = "Select a File to Decrypt";

            if (DialogResult.OK == ofd.ShowDialog())
            {
                if (ofd.FileName.Length > 0)
                {
                    try
                    {
                        SpecialSecurity.DecryptFile(ofd.FileName, txbKey.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void ckbHide_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbHide.Checked)
            {
                txbKey.PasswordChar = '*';
            }
            else
            {
                txbKey.PasswordChar = '\0';
            }
        }
    }
}
