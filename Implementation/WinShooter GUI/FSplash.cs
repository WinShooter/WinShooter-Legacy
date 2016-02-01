// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FSplash.cs" company="John Allberg">
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
//   Summary description for FSplash.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#define VisualStudio

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;
    using Allberg.Shooter.Windows.Forms;

    /// <summary>
    /// Summary description for FSplash.
    /// </summary>
    public class FSplash : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components;
        private FSplash()
        {
            Trace.WriteLine("FSplash: creating on thread " +
                System.Threading.Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) Sleeping...");
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.Size = this.BackgroundImage.Size;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            try
            {
                Trace.WriteLine("FSplash: Dispose(" + disposing.ToString() + ")" +
                    "from thread \"" + System.Threading.Thread.CurrentThread.Name + "\" " +
                    " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                    DateTime.Now.ToLongTimeString());
            }
            catch (Exception)
            {
            }

            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );

            try
            {
                Trace.WriteLine("FSplash: Dispose(" + disposing.ToString() + ")" +
                    "ended " +
                    DateTime.Now.ToLongTimeString());
            }
            catch (Exception)
            {
            }
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSplash));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.safeLabel1 = new SafeLabel();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // safeLabel1
            // 
            this.safeLabel1.AutoSize = true;
            this.safeLabel1.BackColor = System.Drawing.Color.White;
            this.safeLabel1.Location = new System.Drawing.Point(16, 178);
            this.safeLabel1.Name = "safeLabel1";
            this.safeLabel1.Size = new System.Drawing.Size(283, 13);
            this.safeLabel1.TabIndex = 0;
            this.safeLabel1.Text = "WinShooter är gratis under GPL. Mer info under Hjälp->Om";
            // 
            // FSplash
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(329, 220);
            this.Controls.Add(this.safeLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FSplash";
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Should not be seen";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Timer timer1;
        private double opacityChange = .025;

        public bool ShowForm = true;
        private SafeLabel safeLabel1;
        //private System.Windows.Forms.Label lblRegisteredTo;

        static Thread ms_oThread;
        public static void ShowSplash()
        {
            Trace.WriteLine("FSplash: ShowSplash started on thread \"" +
                System.Threading.Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            ms_oThread = new Thread( new ThreadStart(FSplash.TheInstance));
            ms_oThread.IsBackground = true;
            //ms_oThread.SetApartmentState(ApartmentState.STA);
            ms_oThread.Name = "FSplash Thread";
            ms_oThread.Start();
        }
        public static void RemoveSplash()
        {
            Trace.WriteLine("FSplash: RemoveSplash started on thread \"" +
                System.Threading.Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            if (theInstance != null)
            {
                theInstance.ShowForm = false;
            }
            else
                Trace.WriteLine("FSplash: Instance was null. Aborting");
        }

        static internal FSplash theInstance = null;

        public static void TheInstance()
        {
            Trace.WriteLine("FSplash: TheInstance called.");
            if (theInstance == null)
                theInstance = new FSplash();

            Application.Run(theInstance);
        }

        int viewedTime = 0;

        /*internal void SetRegisteredTo(string RegisteredTo)
        {
            if (RegisteredTo != null)
                this.lblRegisteredTo.Text = RegisteredTo;
        }*/

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            //Trace.WriteLine("FSplash: viewedTime = " + this.viewedTime.ToString());
            viewedTime += this.timer1.Interval;
            try
            {
                if (ShowForm)
                {
                    this.Visible = true;
                    if (this.Opacity + this.opacityChange >= 100)
                    {
                        this.Opacity = 100;
                    }
                    else
                    {
                        this.Opacity += this.opacityChange;
                    }
                }
                else
                {
                    if (this.Opacity - opacityChange <= 0)
                    {
                        Trace.WriteLine("FSplash: Will dispose splash");
                        
                        this.Opacity = 0;
                        this.Visible = false;
                        this.timer1.Stop();
                        this.timer1.Enabled = false;
                        try
                        {
                            Trace.WriteLine("FSplash: Activating FMain");
                            //EnableMain();
                        }
                        catch(Exception exc)
                        {
                            Trace.WriteLine("FSplash: Error occured when activating FMain: " + exc.ToString());
                        }

                        //theInstance = null;
                        //this.Dispose(true);
                    }
                    else
                    {
                        if (viewedTime > 1000)
                        {
                            this.Opacity -= this.opacityChange;
                        }
                        else
                        {
                            this.Opacity += this.opacityChange;
                        }
                    }
                }
            }
            catch(System.Threading.ThreadAbortException)
            {
            }
        }
    }
}
