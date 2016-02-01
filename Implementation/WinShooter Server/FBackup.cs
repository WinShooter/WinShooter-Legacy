// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FBackup.cs" company="John Allberg">
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
//   Defines the FBackup type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServer
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;

    public partial class FBackup : Form
    {
        readonly ClientInterface _myInterface;
        public FBackup(ClientInterface newInterface)
        {
            InitializeComponent();

            _myInterface = newInterface;
            saveFileDialog1.InitialDirectory = 
                Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDocuments);
        }

        internal bool DisposeNow;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Trace.WriteLine("FClubs: Dispose(" + disposing + ")" +
                "from thread \"" + Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                DateTime.Now.ToLongTimeString());

            Visible = false;
            try
            {
                if (!DisposeNow)
                    EnableMain();
            }
            catch (Exception exc)
            {
                Trace.WriteLine("FClubs: exception while enabling Main:" + exc);
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public delegate void EnableMainHandler();
        public event EnableMainHandler EnableMain;

        internal void EnableMe()
        {
            Visible = true;
            Enabled = true;
            Focus();
        }

        private DateTime _lastBackup = DateTime.Now;
        private void ChkEnableAutoBackupCheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableAutoBackup.Checked)
            {
                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    chkEnableAutoBackup.Checked = false;
                    return;
                }
                txtFilename.Text = saveFileDialog1.FileName;
                _lastBackup = DateTime.Now;
            }

            txtTimeLeft.Visible = chkEnableAutoBackup.Checked;
            lblTimeLeft.Visible = chkEnableAutoBackup.Checked;
            timerBackup.Enabled = chkEnableAutoBackup.Checked;
        }

        private void TimerBackupTick(object sender, EventArgs e)
        {
            var span = DateTime.Now - _lastBackup.AddMinutes((int)numericUpDown1.Value);
            txtTimeLeft.Text = (-span).Minutes + ":";
            if ((-span).Seconds.ToString().Length < 2)
            {
                txtTimeLeft.Text += "0";
            }
            txtTimeLeft.Text += (-span).Seconds.ToString();

            if (span.TotalSeconds < 0) 
                return;

            Trace.WriteLine("Server.FBackup: Starting Backup from timer.");
            RunBackup(txtFilename.Text);
            Trace.WriteLine("Server.FBackup: Backup from timer done.");
            _lastBackup = DateTime.Now;
        }

 

        private void BtnBackupNowClick(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                chkEnableAutoBackup.Checked = false;
                return;
            }

            Trace.WriteLine("Server.FBackup: Starting manual backup.");
            RunBackup(saveFileDialog1.FileName);
            Trace.WriteLine("Server.FBackup: Manual backup done.");
            _lastBackup = DateTime.Now;
        }

        private void RunBackup(string filename)
        {
            _myInterface.Backup(filename);
        }
    }
}