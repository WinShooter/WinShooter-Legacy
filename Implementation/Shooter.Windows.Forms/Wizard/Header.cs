// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Header.cs" company="John Allberg">
//   Copyright ©2001-2016 John Allberg
//   //   This program is free software; you can redistribute it and/or
//   //   modify it under the terms of the GNU General Public License
//   //   as published by the Free Software Foundation; either version 2
//   //   of the License, or (at your option) any later version.
//   //   This program is distributed in the hope that it will be useful,
//   //   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   //   MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
//   //   GNU General Public License for more details.
//   //   You should have received a copy of the GNU General Public License
//   //   along with this program; if not, write to the Free Software
//   //   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allberg.Shooter.Windows.Forms.Wizard
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    /// <summary>
    /// Summary description for WizardHeader.
    /// </summary>
    [Designer(typeof(HeaderDesigner))]
    public class Header : UserControl
    {
        /// <summary>
        /// The pnl dock padding.
        /// </summary>
        private Panel pnlDockPadding;

        /// <summary>
        /// The lbl description.
        /// </summary>
        private Label lblDescription;

        /// <summary>
        /// The lbl title.
        /// </summary>
        private Label lblTitle;

        /// <summary>
        /// The pic icon.
        /// </summary>
        private PictureBox picIcon;

        /// <summary>
        /// The pnl 3 d dark.
        /// </summary>
        private Panel pnl3dDark;

        /// <summary>
        /// The pnl 3 d bright.
        /// </summary>
        private Panel pnl3dBright;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class. 
        /// Constructor for Header
        /// </summary>
        public Header()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            setPicIconImage();
        }

        /// <summary>
        /// The set pic icon image.
        /// </summary>
        private void setPicIconImage()
        {
            try
            {
                /*System.Windows.Forms.MessageBox.Show("Test1");
                Trace.WriteLine("Test1");
                // get stream to resource
                System.IO.StreamReader reader = Common.GetResourceReader("Wizard.picIcon.bmp");
                System.Windows.Forms.MessageBox.Show("Test2");
                Trace.WriteLine("Test2");
                //this.picIcon.Image = ((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
                if (reader == null || reader.BaseStream == null)
                {
                    throw new ApplicationException("reader or basestream is null");
                }
                Trace.WriteLine("Reader= " + reader.ToString());
                Trace.WriteLine("Basestream=" + reader.BaseStream.ToString());
                Trace.WriteLine("Test3");
                System.Windows.Forms.MessageBox.Show("Test3");
                //this.picIcon.Image = System.Drawing.Bitmap.FromStream(reader.BaseStream, true, true);*/


                // this.picIcon.Image = new System.Drawing.Bitmap(54,54);
                this.picIcon.Image = Common.GetResourceBitmap("Wizard.picIcon.bmp");
            }
            catch(Exception exc)
            {
                MessageBox.Show("Test" + exc.ToString());
                Trace.WriteLine("Exception: " + exc.ToString());

                // throw;
            }
        }

        /// <summary>
        /// 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager resources = new System.Resources.ResourceManager(typeof(Header));
            this.pnlDockPadding = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.pnl3dDark = new System.Windows.Forms.Panel();
            this.pnl3dBright = new System.Windows.Forms.Panel();
            this.pnlDockPadding.SuspendLayout();
            this.SuspendLayout();

            // pnlDockPadding
            this.pnlDockPadding.BackColor = System.Drawing.SystemColors.Window;
            this.pnlDockPadding.Controls.Add(this.lblDescription);
            this.pnlDockPadding.Controls.Add(this.lblTitle);
            this.pnlDockPadding.Controls.Add(this.picIcon);
            this.pnlDockPadding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDockPadding.DockPadding.Bottom = 4;
            this.pnlDockPadding.DockPadding.Left = 8;
            this.pnlDockPadding.DockPadding.Right = 4;
            this.pnlDockPadding.DockPadding.Top = 6;
            this.pnlDockPadding.Font = new System.Drawing.Font(
                "Tahoma", 
                8.25F, 
                System.Drawing.FontStyle.Regular, 
                System.Drawing.GraphicsUnit.Point, 
                (System.Byte)(0));
            this.pnlDockPadding.Location = new System.Drawing.Point(0, 0);
            this.pnlDockPadding.Name = "pnlDockPadding";
            this.pnlDockPadding.Size = new System.Drawing.Size(324, 64);
            this.pnlDockPadding.TabIndex = 6;

            // lblDescription
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblDescription.Location = new System.Drawing.Point(8, 22);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(260, 38);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description";

            // lblTitle
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblTitle.Font = new System.Drawing.Font(
                "Tahoma", 
                8.25F, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point, 
                (System.Byte)(0));
            this.lblTitle.Location = new System.Drawing.Point(8, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(260, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Title";

            // picIcon
            this.picIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.picIcon.Image = (System.Drawing.Image)(resources.GetObject("picIcon.Image"));
            this.picIcon.Location = new System.Drawing.Point(268, 6);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(52, 54);
            this.picIcon.TabIndex = 3;
            this.picIcon.TabStop = false;

            // pnl3dDark
            this.pnl3dDark.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnl3dDark.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl3dDark.Location = new System.Drawing.Point(0, 62);
            this.pnl3dDark.Name = "pnl3dDark";
            this.pnl3dDark.Size = new System.Drawing.Size(324, 1);
            this.pnl3dDark.TabIndex = 7;

            // pnl3dBright
            this.pnl3dBright.BackColor = System.Drawing.Color.White;
            this.pnl3dBright.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl3dBright.Location = new System.Drawing.Point(0, 63);
            this.pnl3dBright.Name = "pnl3dBright";
            this.pnl3dBright.Size = new System.Drawing.Size(324, 1);
            this.pnl3dBright.TabIndex = 8;

            // Header
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CausesValidation = false;
            this.Controls.Add(this.pnl3dDark);
            this.Controls.Add(this.pnl3dBright);
            this.Controls.Add(this.pnlDockPadding);
            this.Name = "Header";
            this.Size = new System.Drawing.Size(324, 64);
            this.SizeChanged += new System.EventHandler(this.Header_SizeChanged);
            this.pnlDockPadding.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// The resize image and text.
        /// </summary>
        private void ResizeImageAndText()
        {
            try
            {
                if (picIcon.Image == null)
                {
                    setPicIconImage();
                }

                // Resize image 
                picIcon.Size = picIcon.Image.Size;

                // Relocate image according to its size
                picIcon.Top = (this.Height - picIcon.Height) / 2;
                picIcon.Left = this.Width - picIcon.Width - 8;

                // Fit text around picture
                lblTitle.Width = picIcon.Left - lblTitle.Left;
                lblDescription.Width = picIcon.Left - lblDescription.Left;
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }
        }

        /// <summary>
        /// The header_ size changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Header_SizeChanged(object sender, EventArgs e)
        {
            ResizeImageAndText();
        }

        /// <summary>
        /// Get/Set the title for the wizard page
        /// </summary>
        [Category("Appearance")]
        public string Title
        {
            get
            {
                return lblTitle.Text;
            }

            set
            {
                lblTitle.Text = value;
            }
        }

        /// <summary>
        /// Gets/Sets the
        /// </summary>
        [Category("Appearance")]
        public string Description
        {
            get
            {
                return lblDescription.Text;
            }

            set
            {
                lblDescription.Text = value;
            }
        }

        /// <summary>
        /// Gets/Sets the Icon
        /// </summary>
        [Category("Appearance")]
        public Image Image
        {
            get
            {
                return picIcon.Image;
            }

            set
            {
                picIcon.Image = value;
                ResizeImageAndText();
            }
        }
    }
}
