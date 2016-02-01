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
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;

    using Allberg.Shooter.Common.Exceptions;
    using Allberg.Shooter.Windows.Forms;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for FImport.
    /// </summary>
    public class FImport : System.Windows.Forms.Form
    {
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtFileToImport;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGrid dataGridResult;
        private SafeButton btnValidate;
        private Allberg.Shooter.Windows.Forms.SafeProgressBar progressBar1;
        //private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox chkAddPatrols;
        private System.Windows.Forms.CheckBox chkAddLanes;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;

        internal FImport(ref Common.Interface newCommon)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            setCursorMethod += new setCursorMethodInvoker(setCursor);
            setVisibleMethod += new setVisibleMethodInvoker(setVisible);

            CommonCode = newCommon;
            Trace.WriteLine("FImport: Creating");
            try
            {
                Resize +=new EventHandler(FImport_Resize);

                // Default columnorder
                columnOrder.Add(Common.Interface.ImportFileColumns.ShooterId.ToString(), 0);
                columnOrder.Add(Common.Interface.ImportFileColumns.ClubId.ToString(), 1);
                columnOrder.Add(Common.Interface.ImportFileColumns.Surname.ToString(), 2);
                columnOrder.Add(Common.Interface.ImportFileColumns.Givenname.ToString(), 3);
                columnOrder.Add(Common.Interface.ImportFileColumns.ShooterClass.ToString(), 4);
                columnOrder.Add(Common.Interface.ImportFileColumns.Email.ToString(), 5);
                columnOrder.Add(Common.Interface.ImportFileColumns.WeaponId.ToString(), 6);
                columnOrder.Add(Common.Interface.ImportFileColumns.Patrol.ToString(), 7);
                columnOrder.Add(Common.Interface.ImportFileColumns.Lane.ToString(), 8);

                CommonCode.UpdatedFileImportCount += new UpdatedFileImportCountHandler(CommonCode_UpdatedFileImportCount);
                CallDataBindToDataGrid = new MethodInvoker(dataGridResultDataBind);
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FImport: Exception: " + exc.ToString());
                throw;
            }
            finally
            {
                Trace.WriteLine("FImport: Created.");
            }
        }

        static MethodInvoker CallDataBindToDataGrid;
        private delegate void setCursorMethodInvoker(Cursor cursor);
        private setCursorMethodInvoker setCursorMethod;
        private delegate void setVisibleMethodInvoker(bool Visible);
        private setVisibleMethodInvoker setVisibleMethod;

        private void setCursor(Cursor cursor)
        {
            Cursor = cursor;
        }
        private void setVisible(bool visible)
        {
            Visible = visible;
        }
        private void dataGridResultDataBind()
        {
            switch(currentOperation)
            {
                case CurrentOperation.check:
                {
                    dataGridResult.DataSource =
                        CommonCode.ImportFileViewDataset(true);
                    if (CommonCode.ImportFileViewDataset(true).Rows.Count > 0)
                    {
                        MessageBox.Show("Fel hittades vid import av filen. Felen redovisas på respektive rad i kolumnen \"Kontrollera\"." +
                            "\r\n\r\nÅtgärda felen (t.ex. genom att lägga upp de klubbar som saknas) och klicka på importera igen",
                            "Fel vid import",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        currentOperation = CurrentOperation.none;
                    }
                    else
                    {
                        try
                        {
                            // Import check succeded. Call import.
                            btnValidate.Enabled = false;
                            Invoke(setCursorMethod, new object[] { Cursors.WaitCursor });
                            progressBar1.Visible = true;
                            currentOperation = CurrentOperation.import;

                            CommonCode.ImportFileFlagsAddPatrols = chkAddPatrols.Checked;
                            CommonCode.ImportFileFlagsAddLanes = chkAddLanes.Checked;

                            CommonCode.ImportFileImportDataset();
                        }
                        catch (ShooterIsAlreadyFullWithCompetitors)
                        {
                            MessageBox.Show("Vid import av filen upptäcktes att en skytt har fler än max " +
                                "antal varv. Filen importerades delvis.",
                                "Fel vid import",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        }
                    }
                    break;
                }
            }

        }

        
        public delegate void EnableMainHandler();
        public event EnableMainHandler EnableMain;
        internal bool DisposeNow = false;
        Common.Interface CommonCode;
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            try
            {
                Trace.WriteLine("FImport: Dispose(" + disposing.ToString() + ")" +
                    "from thread \"" + Thread.CurrentThread.Name + "\" " +
                    " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                    DateTime.Now.ToLongTimeString());

                Visible = false;
            }
            catch(Exception)
            {
            }
            try
            {
                if (!DisposeNow)
                    EnableMain();
            }
            catch(Exception exc)
            {
                try
                {
                    Trace.WriteLine("FImport: exception while disposing:" + exc.ToString());
                }
                catch(Exception)
                {
                }
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

            Trace.WriteLine("FImport: Dispose(" + disposing.ToString() + ")" +
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FImport));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtFileToImport = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.dataGridResult = new System.Windows.Forms.DataGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.btnValidate = new SafeButton();
            this.progressBar1 = new Allberg.Shooter.Windows.Forms.SafeProgressBar();
            this.chkAddPatrols = new System.Windows.Forms.CheckBox();
            this.chkAddLanes = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResult)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFileToImport
            // 
            this.txtFileToImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileToImport.Location = new System.Drawing.Point(136, 8);
            this.txtFileToImport.Multiline = true;
            this.txtFileToImport.Name = "txtFileToImport";
            this.txtFileToImport.ReadOnly = true;
            this.txtFileToImport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFileToImport.Size = new System.Drawing.Size(600, 112);
            this.txtFileToImport.TabIndex = 0;
            this.txtFileToImport.WordWrap = false;
            // 
            // dataGridResult
            // 
            this.dataGridResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridResult.DataMember = "";
            this.dataGridResult.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridResult.Location = new System.Drawing.Point(136, 120);
            this.dataGridResult.Name = "dataGridResult";
            this.dataGridResult.ReadOnly = true;
            this.dataGridResult.Size = new System.Drawing.Size(600, 144);
            this.dataGridResult.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(128, 302);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            this.splitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter1_SplitterMoved);
            // 
            // btnValidate
            // 
            this.btnValidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnValidate.Location = new System.Drawing.Point(136, 272);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(75, 23);
            this.btnValidate.TabIndex = 5;
            this.btnValidate.Text = "Importera";
            this.toolTip1.SetToolTip(this.btnValidate, "Importerar data enligt ovan");
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(216, 272);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(520, 23);
            this.progressBar1.TabIndex = 6;
            this.progressBar1.Visible = false;
            // 
            // chkAddPatrols
            // 
            this.chkAddPatrols.Checked = true;
            this.chkAddPatrols.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddPatrols.Location = new System.Drawing.Point(8, 12);
            this.chkAddPatrols.Name = "chkAddPatrols";
            this.chkAddPatrols.Size = new System.Drawing.Size(112, 24);
            this.chkAddPatrols.TabIndex = 8;
            this.chkAddPatrols.Text = "Lägg till patruller";
            this.chkAddPatrols.Visible = false;
            // 
            // chkAddLanes
            // 
            this.chkAddLanes.Checked = true;
            this.chkAddLanes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddLanes.Location = new System.Drawing.Point(8, 33);
            this.chkAddLanes.Name = "chkAddLanes";
            this.chkAddLanes.Size = new System.Drawing.Size(112, 24);
            this.chkAddLanes.TabIndex = 9;
            this.chkAddLanes.Text = "Lägg till banor";
            this.chkAddLanes.Visible = false;
            // 
            // FImport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(744, 302);
            this.Controls.Add(this.chkAddLanes);
            this.Controls.Add(this.chkAddPatrols);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.dataGridResult);
            this.Controls.Add(this.txtFileToImport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FImport";
            this.Text = "Importera skyttar från fil";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void FImport_Resize(object sender, EventArgs e)
        {
            // TODO
            dataGridResult.Height = Height - (336-144);

        }

        private void splitter1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            MessageBox.Show("Splitter moved: " + e.SplitX.ToString() + " : " + e.SplitY.ToString());
        
        }

        internal void enableMe()
        {
            Trace.WriteLine("FImport: enableMe started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " + Thread.CurrentThread.ManagedThreadId.ToString() +
                " )");

            Visible = true;
            Focus();
            try
            {
                openFile();
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Ett fel uppstod vid öppning av filen. " + 
                    "Verifiera att inte filen är öppen i något annat program, t.ex. Excel.", 
                    "Kunde inte öppna filen", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                Visible = false;
                EnableMain();
            }

            Trace.WriteLine("FImport: enableMe ended.");
        }


        private string[] fileContent;
        private SortedList columnOrder = new SortedList();

        private void openFile()
        {
            Trace.WriteLine("FImport: openFile started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " + Thread.CurrentThread.ManagedThreadId.ToString() +
                " )");

            DialogResult res =
                openFileDialog1.ShowDialog();

            if (res == DialogResult.Cancel |
                res == DialogResult.Abort |
                res == DialogResult.None)
            {
                try
                {
                    Visible = false;
                    EnableMain();
                }
                catch(Exception)
                {
                }
                return;
            }

            fileContent = CommonCode.ImportFileLoadFile(openFileDialog1.FileName, columnOrder);
            txtFileToImport.Lines = fileContent;
            dataGridResult.DataSource = CommonCode.ImportFileViewDataset(false);

            Trace.WriteLine("FImport: openFile ended.");
        }

        private void btnValidate_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FImport: btnValidate_Click started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " + Thread.CurrentThread.ManagedThreadId.ToString() +
                " )");

            btnValidate.Enabled = false;
            Invoke(setCursorMethod, new object[] { Cursors.WaitCursor });
            progressBar1.Visible = true;
            currentOperation = CurrentOperation.check;

            CommonCode.ImportFileFlagsAddPatrols = chkAddPatrols.Checked;
            CommonCode.ImportFileFlagsAddLanes = chkAddLanes.Checked;

            CommonCode.ImportFileValidateDataset();

            Trace.WriteLine("FImport: btnValidate_Click ended.");
        }

        private enum CurrentOperation
        {
            none,
            check,
            import
        };
        CurrentOperation currentOperation = CurrentOperation.none;

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
                switch(currentOperation)
                {
                    case CurrentOperation.check:
                    {
                        Trace.WriteLine("FImport.CommonCode_UpdatedFileImportCount: Check is done. Closing import window.");
                        if (InvokeRequired)
                            BeginInvoke(CallDataBindToDataGrid);
                        else
                            CallDataBindToDataGrid();
                        break;
                    }
                    case CurrentOperation.import:
                    {
                        Trace.WriteLine("FImport.CommonCode_UpdatedFileImportCount: import is done. Closing import window.");
                        currentOperation = CurrentOperation.none;
                        if (InvokeRequired)
                            BeginInvoke(setVisibleMethod, new object[] { false });
                        else
                            Visible = false;
                        EnableMain();
                        break;
                    }
                }
            }
        }
    }
}
