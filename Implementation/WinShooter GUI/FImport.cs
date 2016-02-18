// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FImport.cs" company="John Allberg">
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
//   Summary description for FImport.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    using Allberg.Shooter.Common;
    using Allberg.Shooter.Common.Exceptions;
    using Allberg.Shooter.Windows.Forms;

    /// <summary>
    /// Summary description for FImport.
    /// </summary>
    public class FImport : Form
    {
        /// <summary>
        /// The open file dialog 1.
        /// </summary>
        private OpenFileDialog openFileDialog1;

        /// <summary>
        /// The txt file to import.
        /// </summary>
        private SafeTextBox txtFileToImport;

        /// <summary>
        /// The splitter 1.
        /// </summary>
        private Splitter splitter1;

        /// <summary>
        /// The data grid result.
        /// </summary>
        private DataGrid dataGridResult;

        /// <summary>
        /// The btn validate.
        /// </summary>
        private SafeButton btnValidate;

        /// <summary>
        /// The progress bar 1.
        /// </summary>
        private SafeProgressBar progressBar1;

        // private System.Windows.Forms.ProgressBar progressBar1;
        /// <summary>
        /// The chk add patrols.
        /// </summary>
        private CheckBox chkAddPatrols;

        /// <summary>
        /// The chk add lanes.
        /// </summary>
        private CheckBox chkAddLanes;

        /// <summary>
        /// The tool tip 1.
        /// </summary>
        private ToolTip toolTip1;

        /// <summary>
        /// The components.
        /// </summary>
        private IContainer components;

        /// <summary>
        /// The file content.
        /// </summary>
        private string[] fileContent;

        /// <summary>
        /// The column order.
        /// </summary>
        private readonly SortedList columnOrder = new SortedList();

        /// <summary>
        /// Initializes a new instance of the <see cref="FImport"/> class.
        /// </summary>
        /// <param name="newCommon">
        /// The new common.
        /// </param>
        internal FImport(ref Interface newCommon)
        {
            // Required for Windows Form Designer support
            this.InitializeComponent();

            this.setCursorMethod += this.setCursor;
            this.setVisibleMethod += this.setVisible;

            this.CommonCode = newCommon;
            Trace.WriteLine("FImport: Creating");
            try
            {
                this.Resize += this.FImport_Resize;

                // Default columnorder
                this.columnOrder.Add(Interface.ImportFileColumns.ShooterId.ToString(), 0);
                this.columnOrder.Add(Interface.ImportFileColumns.ClubId.ToString(), 1);
                this.columnOrder.Add(Interface.ImportFileColumns.Surname.ToString(), 2);
                this.columnOrder.Add(Interface.ImportFileColumns.Givenname.ToString(), 3);
                this.columnOrder.Add(Interface.ImportFileColumns.ShooterClass.ToString(), 4);
                this.columnOrder.Add(Interface.ImportFileColumns.Email.ToString(), 5);
                this.columnOrder.Add(Interface.ImportFileColumns.WeaponId.ToString(), 6);
                this.columnOrder.Add(Interface.ImportFileColumns.Patrol.ToString(), 7);
                this.columnOrder.Add(Interface.ImportFileColumns.Lane.ToString(), 8);

                this.CommonCode.UpdatedFileImportCount += this.CommonCode_UpdatedFileImportCount;
                callDataBindToDataGrid = this.dataGridResultDataBind;
            }
            catch (Exception exc)
            {
                Trace.WriteLine("FImport: Exception: " + exc);
                throw;
            }
            finally
            {
                Trace.WriteLine("FImport: Created.");
            }
        }

        /// <summary>
        /// The call data bind to data grid.
        /// </summary>
        private static MethodInvoker callDataBindToDataGrid;

        /// <summary>
        /// The set cursor method invoker.
        /// </summary>
        /// <param name="cursor">
        /// The cursor.
        /// </param>
        private delegate void setCursorMethodInvoker(Cursor cursor);

        /// <summary>
        /// The set cursor method.
        /// </summary>
        private readonly setCursorMethodInvoker setCursorMethod;

        /// <summary>
        /// The set visible method invoker.
        /// </summary>
        /// <param name="Visible">
        /// The visible.
        /// </param>
        private delegate void setVisibleMethodInvoker(bool Visible);

        /// <summary>
        /// The set visible method.
        /// </summary>
        private readonly setVisibleMethodInvoker setVisibleMethod;

        /// <summary>
        /// The set cursor.
        /// </summary>
        /// <param name="cursor">
        /// The cursor.
        /// </param>
        private void setCursor(Cursor cursor)
        {
            this.Cursor = cursor;
        }

        /// <summary>
        /// The set visible.
        /// </summary>
        /// <param name="visible">
        /// The visible.
        /// </param>
        private void setVisible(bool visible)
        {
            this.Visible = visible;
        }

        /// <summary>
        /// The data grid result data bind.
        /// </summary>
        private void dataGridResultDataBind()
        {
            switch (this.currentOperation)
            {
                case CurrentOperation.check:
                    {
                        this.dataGridResult.DataSource = this.CommonCode.ImportFileViewDataset(true);
                        if (this.CommonCode.ImportFileViewDataset(true).Rows.Count > 0)
                        {
                            MessageBox.Show(
                                "Fel hittades vid import av filen. Felen redovisas på respektive rad i kolumnen \"Kontrollera\"."
                                + "\r\n\r\nÅtgärda felen (t.ex. genom att lägga upp de klubbar som saknas) och klicka på importera igen", 
                                "Fel vid import", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Warning);
                            this.currentOperation = CurrentOperation.none;
                        }
                        else
                        {
                            try
                            {
                                // Import check succeded. Call import.
                                this.btnValidate.Enabled = false;
                                this.Invoke(setCursorMethod, new object[] { Cursors.WaitCursor });
                                this.progressBar1.Visible = true;
                                this.currentOperation = CurrentOperation.import;

                                this.CommonCode.ImportFileFlagsAddPatrols = chkAddPatrols.Checked;
                                this.CommonCode.ImportFileFlagsAddLanes = chkAddLanes.Checked;

                                this.CommonCode.ImportFileImportDataset();
                            }
                            catch (ShooterIsAlreadyFullWithCompetitors)
                            {
                                MessageBox.Show(
                                    "Vid import av filen upptäcktes att en skytt har fler än max "
                                    + "antal varv. Filen importerades delvis.", 
                                    "Fel vid import", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                            }
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// The enable main handler.
        /// </summary>
        public delegate void EnableMainHandler();

        /// <summary>
        /// The enable main.
        /// </summary>
        public event EnableMainHandler EnableMain;

        /// <summary>
        /// The dispose now.
        /// </summary>
        internal bool DisposeNow = false;

        /// <summary>
        /// The common code.
        /// </summary>
        private Interface CommonCode;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                Trace.WriteLine(
                    "FImport: Dispose(" + disposing + ")" + "from thread \"" + Thread.CurrentThread.Name
                    + "\" " + " ( " + Thread.CurrentThread.ManagedThreadId + " ) "
                    + DateTime.Now.ToLongTimeString());

                this.Visible = false;
            }
            catch (Exception)
            {
            }

            try
            {
                if (!DisposeNow)
                {
                    EnableMain();
                }
            }
            catch (Exception exc)
            {
                try
                {
                    Trace.WriteLine("FImport: exception while disposing:" + exc);
                }
                catch (Exception)
                {
                }
            }

            if (!this.DisposeNow)
            {
                return;
            }

            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }

            base.Dispose(disposing);

            Trace.WriteLine(
                "FImport: Dispose(" + disposing + ")" + "ended " + DateTime.Now.ToLongTimeString());
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FImport));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtFileToImport = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.dataGridResult = new System.Windows.Forms.DataGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.btnValidate = new SafeButton();
            this.progressBar1 = new Allberg.Shooter.Windows.Forms.SafeProgressBar();
            this.chkAddPatrols = new System.Windows.Forms.CheckBox();
            this.chkAddLanes = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)this.dataGridResult).BeginInit();
            this.SuspendLayout();

            // txtFileToImport
            this.txtFileToImport.Anchor =
                (System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right));
            this.txtFileToImport.Location = new System.Drawing.Point(136, 8);
            this.txtFileToImport.Multiline = true;
            this.txtFileToImport.Name = "txtFileToImport";
            this.txtFileToImport.ReadOnly = true;
            this.txtFileToImport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFileToImport.Size = new System.Drawing.Size(600, 112);
            this.txtFileToImport.TabIndex = 0;
            this.txtFileToImport.WordWrap = false;

            // dataGridResult
            this.dataGridResult.Anchor =
                (System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right));
            this.dataGridResult.DataMember = string.Empty;
            this.dataGridResult.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridResult.Location = new System.Drawing.Point(136, 120);
            this.dataGridResult.Name = "dataGridResult";
            this.dataGridResult.ReadOnly = true;
            this.dataGridResult.Size = new System.Drawing.Size(600, 144);
            this.dataGridResult.TabIndex = 1;

            // splitter1
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(128, 302);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            this.splitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter1_SplitterMoved);

            // btnValidate
            this.btnValidate.Anchor =
                (System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btnValidate.Location = new System.Drawing.Point(136, 272);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(75, 23);
            this.btnValidate.TabIndex = 5;
            this.btnValidate.Text = "Importera";
            this.toolTip1.SetToolTip(this.btnValidate, "Importerar data enligt ovan");
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);

            // progressBar1
            this.progressBar1.Anchor =
                (System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right));
            this.progressBar1.Location = new System.Drawing.Point(216, 272);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(520, 23);
            this.progressBar1.TabIndex = 6;
            this.progressBar1.Visible = false;

            // chkAddPatrols
            this.chkAddPatrols.Checked = true;
            this.chkAddPatrols.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddPatrols.Location = new System.Drawing.Point(8, 12);
            this.chkAddPatrols.Name = "chkAddPatrols";
            this.chkAddPatrols.Size = new System.Drawing.Size(112, 24);
            this.chkAddPatrols.TabIndex = 8;
            this.chkAddPatrols.Text = "Lägg till patruller";
            this.chkAddPatrols.Visible = false;

            // chkAddLanes
            this.chkAddLanes.Checked = true;
            this.chkAddLanes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddLanes.Location = new System.Drawing.Point(8, 33);
            this.chkAddLanes.Name = "chkAddLanes";
            this.chkAddLanes.Size = new System.Drawing.Size(112, 24);
            this.chkAddLanes.TabIndex = 9;
            this.chkAddLanes.Text = "Lägg till banor";
            this.chkAddLanes.Visible = false;

            // FImport
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(744, 302);
            this.Controls.Add(this.chkAddLanes);
            this.Controls.Add(this.chkAddPatrols);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.dataGridResult);
            this.Controls.Add(this.txtFileToImport);
            this.Icon = (System.Drawing.Icon)(resources.GetObject("$this.Icon"));
            this.Name = "FImport";
            this.Text = "Importera skyttar från fil";
            ((System.ComponentModel.ISupportInitialize)this.dataGridResult).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        /// <summary>
        /// The f import_ resize.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FImport_Resize(object sender, EventArgs e)
        {
            // TODO
            dataGridResult.Height = Height - (336 - 144);
        }

        /// <summary>
        /// The splitter 1_ splitter moved.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            MessageBox.Show("Splitter moved: " + e.SplitX.ToString() + " : " + e.SplitY.ToString());
        }

        /// <summary>
        /// The enable me.
        /// </summary>
        internal void enableMe()
        {
            Trace.WriteLine(
                "FImport: enableMe started on thread \"" + Thread.CurrentThread.Name + "\" ( "
                + Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            Visible = true;
            Focus();
            try
            {
                openFile();
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Ett fel uppstod vid öppning av filen. "
                    + "Verifiera att inte filen är öppen i något annat program, t.ex. Excel.", 
                    "Kunde inte öppna filen", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                Visible = false;
                EnableMain();
            }

            Trace.WriteLine("FImport: enableMe ended.");
        }

        /// <summary>
        /// The open file.
        /// </summary>
        private void openFile()
        {
            Trace.WriteLine(
                "FImport: openFile started on thread \"" + Thread.CurrentThread.Name + "\" ( "
                + Thread.CurrentThread.ManagedThreadId + " )");

            DialogResult res = openFileDialog1.ShowDialog();

            if (res == DialogResult.Cancel | res == DialogResult.Abort | res == DialogResult.None)
            {
                try
                {
                    Visible = false;
                    EnableMain();
                }
                catch (Exception)
                {
                }

                return;
            }

            fileContent = CommonCode.ImportFileLoadFile(this.openFileDialog1.FileName, this.columnOrder);
            txtFileToImport.Lines = fileContent;
            dataGridResult.DataSource = CommonCode.ImportFileViewDataset(false);

            Trace.WriteLine("FImport: openFile ended.");
        }

        /// <summary>
        /// The btn validate_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btnValidate_Click(object sender, EventArgs e)
        {
            Trace.WriteLine(
                "FImport: btnValidate_Click started on thread \"" + Thread.CurrentThread.Name + "\" ( "
                + Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            btnValidate.Enabled = false;
            Invoke(setCursorMethod, new object[] { Cursors.WaitCursor });
            progressBar1.Visible = true;
            currentOperation = CurrentOperation.check;

            CommonCode.ImportFileFlagsAddPatrols = chkAddPatrols.Checked;
            CommonCode.ImportFileFlagsAddLanes = chkAddLanes.Checked;

            CommonCode.ImportFileValidateDataset();

            Trace.WriteLine("FImport: btnValidate_Click ended.");
        }

        /// <summary>
        /// The current operation.
        /// </summary>
        private enum CurrentOperation
        {
            /// <summary>
            /// The none.
            /// </summary>
            none, 

            /// <summary>
            /// The check.
            /// </summary>
            check, 

            /// <summary>
            /// The import.
            /// </summary>
            import
        }

        /// <summary>
        /// The current operation.
        /// </summary>
        private CurrentOperation currentOperation = CurrentOperation.none;

        /// <summary>
        /// The common code_ updated file import count.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="totalCount">
        /// The total count.
        /// </param>
        private void CommonCode_UpdatedFileImportCount(int count, int totalCount)
        {
            progressBar1.Maximum = totalCount;
            progressBar1.Value = count;
            progressBar1.Visible = true;

            if (count == totalCount)
            {
                btnValidate.Enabled = true;
                progressBar1.Visible = false;
                Invoke(setCursorMethod, new object[] { Cursors.Default });

                // Operation is finished
                switch (this.currentOperation)
                {
                    case CurrentOperation.check:
                        {
                            Trace.WriteLine(
                                "FImport.CommonCode_UpdatedFileImportCount: Check is done. Closing import window.");
                            if (this.InvokeRequired)
                            {
                                this.BeginInvoke(callDataBindToDataGrid);
                            }
                            else
                            {
                                callDataBindToDataGrid();
                            }

                            break;
                        }

                    case CurrentOperation.import:
                        {
                            Trace.WriteLine(
                                "FImport.CommonCode_UpdatedFileImportCount: import is done. Closing import window.");
                            this.currentOperation = CurrentOperation.none;
                            if (this.InvokeRequired)
                            {
                                this.BeginInvoke(this.setVisibleMethod, false);
                            }
                            else
                            {
                                this.Visible = false;
                            }

                            this.EnableMain();
                            break;
                        }
                }
            }
        }
    }
}
