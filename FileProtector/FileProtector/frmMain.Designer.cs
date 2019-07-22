namespace FileProtector
{
    partial class frmMain
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblKey = new System.Windows.Forms.Label();
            this.txbKey = new System.Windows.Forms.TextBox();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.ckbHide = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Location = new System.Drawing.Point(34, 20);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(39, 20);
            this.lblKey.TabIndex = 7;
            this.lblKey.Text = "Key:";
            // 
            // txbKey
            // 
            this.txbKey.Location = new System.Drawing.Point(80, 17);
            this.txbKey.Name = "txbKey";
            this.txbKey.PasswordChar = '*';
            this.txbKey.Size = new System.Drawing.Size(220, 26);
            this.txbKey.TabIndex = 0;
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(190, 65);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(145, 65);
            this.btnDecrypt.TabIndex = 2;
            this.btnDecrypt.Text = "Decrypt file";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(35, 65);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(145, 65);
            this.btnEncrypt.TabIndex = 1;
            this.btnEncrypt.Text = "Encrypt file";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // ckbHide
            // 
            this.ckbHide.AutoSize = true;
            this.ckbHide.Checked = true;
            this.ckbHide.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbHide.Location = new System.Drawing.Point(313, 19);
            this.ckbHide.Name = "ckbHide";
            this.ckbHide.Size = new System.Drawing.Size(22, 21);
            this.ckbHide.TabIndex = 3;
            this.ckbHide.UseVisualStyleBackColor = true;
            this.ckbHide.CheckedChanged += new System.EventHandler(this.ckbHide_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 144);
            this.Controls.Add(this.ckbHide);
            this.Controls.Add(this.lblKey);
            this.Controls.Add(this.txbKey);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnEncrypt);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Files Protector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.TextBox txbKey;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.CheckBox ckbHide;
    }
}

