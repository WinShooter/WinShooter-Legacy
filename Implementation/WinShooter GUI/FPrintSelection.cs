// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FPrintSelection.cs" company="John Allberg">
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
//   Summary description for FPrintSelection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;
    using Allberg.Shooter.Windows.Forms;

    /// <summary>
    /// Summary description for FPrintSelection.
    /// </summary>
    public class FPrintSelection : System.Windows.Forms.Form
    {
        internal SafeLabel lblSelection;
        internal System.Windows.Forms.CheckBox chkPrintAll;
        internal Allberg.Shooter.Windows.Forms.SafeComboBox DdSelection;
        private SafeButton btnOk;
        private SafeButton btnCancel;
        internal NumericUpDown numericUpDown1;
        internal Label lblNumericUpDown1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public FPrintSelection()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            TablePrintSelection =  
                new System.Data.DataTable();
            TablePrintSelection.Columns.Add("Id", ("x").GetType());
            TablePrintSelection.Columns.Add("Name", ("x").GetType());

            this.DdSelection.DataSource = this.TablePrintSelection;
            this.DdSelection.DisplayMember = "Name";
            this.DdSelection.ValueMember = "Id";
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FPrintSelection: Dispose(" + disposing.ToString() + ")" +
                "from thread \"" + Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
            Trace.WriteLine("FPrintSelection: Dispose(" + disposing.ToString() + ")" +
                "ended " +
                DateTime.Now.ToLongTimeString());
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPrintSelection));
            this.lblSelection = new SafeLabel();
            this.chkPrintAll = new System.Windows.Forms.CheckBox();
            this.DdSelection = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.btnOk = new SafeButton();
            this.btnCancel = new SafeButton();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lblNumericUpDown1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSelection
            // 
            this.lblSelection.Location = new System.Drawing.Point(8, 8);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(100, 23);
            this.lblSelection.TabIndex = 0;
            this.lblSelection.Text = "Selection";
            // 
            // chkPrintAll
            // 
            this.chkPrintAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPrintAll.Checked = true;
            this.chkPrintAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrintAll.Location = new System.Drawing.Point(112, 32);
            this.chkPrintAll.Name = "chkPrintAll";
            this.chkPrintAll.Size = new System.Drawing.Size(304, 24);
            this.chkPrintAll.TabIndex = 1;
            this.chkPrintAll.Text = "checkBox1";
            this.chkPrintAll.CheckedChanged += new System.EventHandler(this.chkPrintAll_CheckedChanged);
            // 
            // DdSelection
            // 
            this.DdSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DdSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DdSelection.Enabled = false;
            this.DdSelection.Location = new System.Drawing.Point(112, 8);
            this.DdSelection.Name = "DdSelection";
            this.DdSelection.Size = new System.Drawing.Size(304, 21);
            this.DdSelection.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(264, 56);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(344, 56);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(112, 54);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown1.TabIndex = 5;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Visible = false;
            // 
            // lblNumericUpDown1
            // 
            this.lblNumericUpDown1.AutoSize = true;
            this.lblNumericUpDown1.Location = new System.Drawing.Point(163, 56);
            this.lblNumericUpDown1.Name = "lblNumericUpDown1";
            this.lblNumericUpDown1.Size = new System.Drawing.Size(35, 13);
            this.lblNumericUpDown1.TabIndex = 6;
            this.lblNumericUpDown1.Text = "label1";
            this.lblNumericUpDown1.Visible = false;
            // 
            // FPrintSelection
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(424, 86);
            this.Controls.Add(this.lblNumericUpDown1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.DdSelection);
            this.Controls.Add(this.chkPrintAll);
            this.Controls.Add(this.lblSelection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FPrintSelection";
            this.Text = "Skriv ut";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public delegate void EnablePrintHandler(string PrintId, FPrintSelection printSelection);
        public event EnablePrintHandler EnablePrint;

        public delegate void EnableMainHandler();
        public event EnableMainHandler EnableMain;

        public System.Data.DataTable TablePrintSelection;

        private void chkPrintAll_CheckedChanged(object sender, System.EventArgs e)
        {
            this.DdSelection.Enabled = !this.chkPrintAll.Checked;
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (this.chkPrintAll.Checked)
            {
                // Print all
                EnablePrint("-1", this);
            }
            else
            {
                // Print Selection
                EnablePrint((string)this.DdSelection.SelectedValue, this);
            }
            this.Visible = false;
            this.Dispose(true);
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Visible = false;
            try
            {
                EnableMain();
            }
            catch(Exception)
            {
            }
            this.Dispose(true);		
        }
    }
}
