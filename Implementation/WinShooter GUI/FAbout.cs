// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FAbout.cs" company="John Allberg">
//   Copyright ©2001-2016 John Allberg
//   
//   This program is free software; you can redistribute it and/or
//   modify it under the terms of the GNU General Public License
//   as published by the Free Software Foundation; either version 2
//   of the License, or (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
//   GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License
//   along with this program; if not, write to the Free Software
//   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
// <summary>
//   Summary description for FAbout.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using Allberg.Shooter.Windows.Forms;

    /// <summary>
    /// Summary description for FAbout.
    /// </summary>
    internal sealed class FAbout : Form
    {
        private Forms.SafeButton _btnClose;
        private LinkLabel _linkClickAllbergSeSkytte;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components;
        internal SafeLabel LblVersion;
        private RichTextBox _richTextBox1;


                
        //public delegate void EnableMainEventHandler();
        public event MethodInvoker EnableMain;
        //public event EnableMainEventHandler EnableMain;
        internal bool DisposeNow;

        public FAbout()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            MinimumSize = Size;
            MaximumSize = Size;
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            LblVersion.Text = "v." + ver.Major + "." +
                ver.Minor + "." +
                ver.Build;
            if (ver.Revision != 0)
                LblVersion.Text += "." +
                    ver.Revision;

            const string licencePath = "licensvillkor.rtf";
#if DEBUG
            LblVersion.Text += "B";
#endif
            if (!File.Exists(licencePath)) 
                return;

            var licenseText = GetLicenceText(licencePath);
            _richTextBox1.Rtf = licenseText;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FAbout: Dispose(" + disposing + ")" +
                "from thread \"" + Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                DateTime.Now.ToLongTimeString());

            Visible = false;
            try
            {
                if (!DisposeNow)
                    EnableMain();
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FAbout exception while disposing:" + exc);
                throw;
            }

            if(!DisposeNow)
                return;
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
            Trace.WriteLine("FAbout: Dispose(" + disposing + ")" +
                "ended." +
                DateTime.Now.ToLongTimeString());
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAbout));
            this._btnClose = new SafeButton();
            this._linkClickAllbergSeSkytte = new System.Windows.Forms.LinkLabel();
            this.LblVersion = new SafeLabel();
            this._richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this._btnClose.BackColor = System.Drawing.SystemColors.Control;
            this._btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnClose.Location = new System.Drawing.Point(248, 192);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(75, 23);
            this._btnClose.TabIndex = 4;
            this._btnClose.Text = "Stäng";
            this._btnClose.UseVisualStyleBackColor = false;
            this._btnClose.Click += new System.EventHandler(this.BtnCloseClick);
            // 
            // linkClickAllbergSeSkytte
            // 
            this._linkClickAllbergSeSkytte.Location = new System.Drawing.Point(8, 32);
            this._linkClickAllbergSeSkytte.Name = "_linkClickAllbergSeSkytte";
            this._linkClickAllbergSeSkytte.Size = new System.Drawing.Size(176, 16);
            this._linkClickAllbergSeSkytte.TabIndex = 5;
            this._linkClickAllbergSeSkytte.TabStop = true;
            this._linkClickAllbergSeSkytte.Text = "http://www.winshooter.se";
            this._linkClickAllbergSeSkytte.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._linkClickAllbergSeSkytte.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClickAllbergSeSkytteLinkClicked);
            // 
            // lblVersion
            // 
            this.LblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblVersion.Location = new System.Drawing.Point(216, 88);
            this.LblVersion.Name = "LblVersion";
            this.LblVersion.Size = new System.Drawing.Size(104, 32);
            this.LblVersion.TabIndex = 6;
            this.LblVersion.Text = "v1.0.0";
            this.LblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextBox1
            // 
            this._richTextBox1.BackColor = System.Drawing.Color.White;
            this._richTextBox1.Location = new System.Drawing.Point(0, 220);
            this._richTextBox1.Name = "_richTextBox1";
            this._richTextBox1.ReadOnly = true;
            this._richTextBox1.Size = new System.Drawing.Size(328, 220);
            this._richTextBox1.TabIndex = 7;
            this._richTextBox1.Text = "Winshooter licensieras under GPL, Gnu Public License.\n\nFilen licensvillkort.rtf h" +
                "ittades inte.";
            // 
            // FAbout
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Allberg.Shooter.Windows.Properties.Resources.Splash;
            this.ClientSize = new System.Drawing.Size(328, 440);
            this.Controls.Add(this._richTextBox1);
            this.Controls.Add(this.LblVersion);
            this.Controls.Add(this._linkClickAllbergSeSkytte);
            this.Controls.Add(this._btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Om";
            this.ResumeLayout(false);

        }
        #endregion

        private void LinkClickAllbergSeSkytteLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Start IE and point to http://www.allberg.se/WinShooter
            Process.Start("IExplore"," http://www.winshooter.se");
            TopMost = false;
        }

        internal void EnableMe()
        {
            Visible = true;
            Focus();
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Visible = false;
            EnableMain();
        }

        private static string GetLicenceText(string licencePath)
        {
            var reader = new StreamReader(licencePath);
            var toReturn = reader.ReadToEnd();
            reader.Close();

            return toReturn;
        }
    }
}
