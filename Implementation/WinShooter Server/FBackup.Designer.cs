namespace Allberg.Shooter.WinShooterServer
{
    partial class FBackup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FBackup));
            this.btnBackupNow = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTimeLeft = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.lblTimeLeft = new Allberg.Shooter.Windows.Forms.SafeLabel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.safeLabel3 = new Allberg.Shooter.Windows.Forms.SafeLabel();
            this.safeLabel2 = new Allberg.Shooter.Windows.Forms.SafeLabel();
            this.txtFilename = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.safeLabel1 = new Allberg.Shooter.Windows.Forms.SafeLabel();
            this.chkEnableAutoBackup = new System.Windows.Forms.CheckBox();
            this.timerBackup = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBackupNow
            // 
            this.btnBackupNow.Location = new System.Drawing.Point(12, 12);
            this.btnBackupNow.Name = "btnBackupNow";
            this.btnBackupNow.Size = new System.Drawing.Size(75, 23);
            this.btnBackupNow.TabIndex = 0;
            this.btnBackupNow.Text = "Backup nu!";
            this.btnBackupNow.UseVisualStyleBackColor = true;
            this.btnBackupNow.Click += new System.EventHandler(this.BtnBackupNowClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtTimeLeft);
            this.groupBox1.Controls.Add(this.lblTimeLeft);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.safeLabel3);
            this.groupBox1.Controls.Add(this.safeLabel2);
            this.groupBox1.Controls.Add(this.txtFilename);
            this.groupBox1.Controls.Add(this.safeLabel1);
            this.groupBox1.Controls.Add(this.chkEnableAutoBackup);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 128);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Schemalagd backup";
            // 
            // txtTimeLeft
            // 
            this.txtTimeLeft.Enabled = false;
            this.txtTimeLeft.Location = new System.Drawing.Point(117, 94);
            this.txtTimeLeft.Name = "txtTimeLeft";
            this.txtTimeLeft.Size = new System.Drawing.Size(100, 20);
            this.txtTimeLeft.TabIndex = 9;
            this.txtTimeLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTimeLeft.Visible = false;
            // 
            // lblTimeLeft
            // 
            this.lblTimeLeft.AutoSize = true;
            this.lblTimeLeft.Location = new System.Drawing.Point(6, 97);
            this.lblTimeLeft.Name = "lblTimeLeft";
            this.lblTimeLeft.Size = new System.Drawing.Size(105, 13);
            this.lblTimeLeft.TabIndex = 8;
            this.lblTimeLeft.Text = "Tid till nästa backup:";
            this.lblTimeLeft.Visible = false;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(93, 65);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(45, 20);
            this.numericUpDown1.TabIndex = 5;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // safeLabel3
            // 
            this.safeLabel3.AutoSize = true;
            this.safeLabel3.Location = new System.Drawing.Point(136, 67);
            this.safeLabel3.Name = "safeLabel3";
            this.safeLabel3.Size = new System.Drawing.Size(44, 13);
            this.safeLabel3.TabIndex = 7;
            this.safeLabel3.Text = ":e minut";
            // 
            // safeLabel2
            // 
            this.safeLabel2.AutoSize = true;
            this.safeLabel2.Location = new System.Drawing.Point(6, 67);
            this.safeLabel2.Name = "safeLabel2";
            this.safeLabel2.Size = new System.Drawing.Size(81, 13);
            this.safeLabel2.TabIndex = 6;
            this.safeLabel2.Text = "Gör backup var";
            // 
            // txtFilename
            // 
            this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename.Enabled = false;
            this.txtFilename.Location = new System.Drawing.Point(55, 39);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(328, 20);
            this.txtFilename.TabIndex = 4;
            // 
            // safeLabel1
            // 
            this.safeLabel1.AutoSize = true;
            this.safeLabel1.Location = new System.Drawing.Point(6, 42);
            this.safeLabel1.Name = "safeLabel1";
            this.safeLabel1.Size = new System.Drawing.Size(43, 13);
            this.safeLabel1.TabIndex = 3;
            this.safeLabel1.Text = "Filnamn";
            // 
            // chkEnableAutoBackup
            // 
            this.chkEnableAutoBackup.AutoSize = true;
            this.chkEnableAutoBackup.Location = new System.Drawing.Point(6, 19);
            this.chkEnableAutoBackup.Name = "chkEnableAutoBackup";
            this.chkEnableAutoBackup.Size = new System.Drawing.Size(158, 17);
            this.chkEnableAutoBackup.TabIndex = 0;
            this.chkEnableAutoBackup.Text = "Aktivera automatisk backup";
            this.chkEnableAutoBackup.UseVisualStyleBackColor = true;
            this.chkEnableAutoBackup.CheckedChanged += new System.EventHandler(this.ChkEnableAutoBackupCheckedChanged);
            // 
            // timerBackup
            // 
            this.timerBackup.Interval = 1000;
            this.timerBackup.Tick += new System.EventHandler(this.TimerBackupTick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "mdb";
            this.saveFileDialog1.Filter = "MS Access Files|*.mdb";
            // 
            // FBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 181);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBackupNow);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FBackup";
            this.Text = "Backup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBackupNow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkEnableAutoBackup;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtFilename;
        private Allberg.Shooter.Windows.Forms.SafeLabel safeLabel1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private Allberg.Shooter.Windows.Forms.SafeLabel safeLabel3;
        private Allberg.Shooter.Windows.Forms.SafeLabel safeLabel2;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtTimeLeft;
        private Allberg.Shooter.Windows.Forms.SafeLabel lblTimeLeft;
        private System.Windows.Forms.Timer timerBackup;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}