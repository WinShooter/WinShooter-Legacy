// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FPatrols.cs" company="John Allberg">
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
//   Summary description for FPatrols.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using Allberg.Shooter.Windows.Forms;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for FPatrols.
    /// </summary>
    public class FPatrols : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components;
        private Allberg.Shooter.Windows.DatasetPatrols patrolsDs;
        private SafeLabel lblPatrolsCount;
        private Allberg.Shooter.Windows.Forms.DataGridCustom dataGrid1;
        //private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
        private SafeButton btnAddPatrol;
        private SafeButton btnRemPatrol;
        private SafeButton btnPatrolAutomatic;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridTextBoxColumn DataGridTextBoxPatrol;
        private System.Windows.Forms.DataGridTextBoxColumn DataGridTextBoxStartTime;
        private System.Windows.Forms.DataGridTextBoxColumn DataGridTextBoxClass;
        private System.Windows.Forms.DataGridTextBoxColumn DataGridTextBoxNrOfCompetitors;
        private SafeLabel lblPatrols;
        private DataGridTextBoxColumn DataGridTextBoxNrOfCompetitorsWithResults;
        private DataGridTextBoxColumn dataGridTextBoxStartTimeDisplay;
        private DataGridTextBoxColumn DataGridTextBoxNrOfCompetitorsArrived;
        private System.Windows.Forms.ProgressBar progressBar1;

        public delegate void EnableMainHandler();
        public event EnableMainHandler EnableMain;
        private delegate void DatasetBindToDataGridHandler();
        private event MethodInvoker DatasetBindToDataGrid;
        private event MethodInvoker EnableAndStartProgressbar;
        private delegate void UpdateNrOfPatrolsHandler(int nrOfPatrols);
        private event UpdateNrOfPatrolsHandler UpdateNrOfPatrols;


        internal FPatrols(ref Common.Interface newCommon)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            CommonCode = newCommon;

            Trace.WriteLine("FPatrols: Creating");
            try
            {
                height = this.Size.Height;
                width = this.Size.Width;

                this.dataGrid1.MouseUp +=new MouseEventHandler(dataGrid1_MouseUp);

                myPatrolWindow = new FPatrol(ref CommonCode, this);
                myPatrolWindow.EnablePatrols += 
                    new MethodInvoker(enableMe);
                DatasetBindToDataGrid += new MethodInvoker(datasetBindToDataGrid);
                UpdateNrOfPatrols += new UpdateNrOfPatrolsHandler(FPatrols_UpdateNrOfPatrols);
                CommonCode.UpdatedPatrolAddAutomaticCompetitors +=
                    new UpdatedPatrolAddAutomaticCompetitorsHandler(
                        updatePatrolAddAutomaticCompetitors);

                EnableAndStartProgressbar += new MethodInvoker(enableAndStartProgressbar);


                updatedPatrolsThread = new Thread(
                    new ThreadStart(updatedPatrolsWithThread));
                updatedPatrolsThread.IsBackground = true;
                updatedPatrolsThread.Name = "FPatrols:updatedPatrolsThread";
                updatedCompetitorsThread = new Thread(
                    new ThreadStart(updatedCompetitorsWithThread));
                updatedCompetitorsThread.Name = "FPatrols:updatedCompetitorsThread";
                updatedCompetitorsThread.IsBackground = true;
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FPatrols: Exception: " + exc.ToString());
                throw;
            }
            finally
            {
                Trace.WriteLine("FPatrols: Created.");
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FPatrols: Dispose(" + disposing.ToString() + ")" +
                "from thread \"" + Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            this.Visible = false;
            try
            {
                if (!this.DisposeNow)
                    EnableMain();
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FImport: exception while disposing:" + exc.ToString());
            }

            if(!DisposeNow)
                return;

            myPatrolWindow.DisposeNow = true;
            myPatrolWindow.Dispose();

            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );

            Trace.WriteLine("FPatrols: Dispose(" + disposing.ToString() + ")" +
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPatrols));
            this.lblPatrols = new SafeLabel();
            this.btnAddPatrol = new SafeButton();
            this.lblPatrolsCount = new SafeLabel();
            this.btnRemPatrol = new SafeButton();
            this.btnPatrolAutomatic = new SafeButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGrid1 = new Allberg.Shooter.Windows.Forms.DataGridCustom();
            this.patrolsDs = new Allberg.Shooter.Windows.DatasetPatrols();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.DataGridTextBoxPatrol = new System.Windows.Forms.DataGridTextBoxColumn();
            this.DataGridTextBoxStartTime = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxStartTimeDisplay = new System.Windows.Forms.DataGridTextBoxColumn();
            this.DataGridTextBoxClass = new System.Windows.Forms.DataGridTextBoxColumn();
            this.DataGridTextBoxNrOfCompetitors = new System.Windows.Forms.DataGridTextBoxColumn();
            this.DataGridTextBoxNrOfCompetitorsWithResults = new System.Windows.Forms.DataGridTextBoxColumn();
            this.DataGridTextBoxNrOfCompetitorsArrived = new System.Windows.Forms.DataGridTextBoxColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patrolsDs)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPatrols
            // 
            this.lblPatrols.Location = new System.Drawing.Point(8, 8);
            this.lblPatrols.Name = "lblPatrols";
            this.lblPatrols.Size = new System.Drawing.Size(100, 23);
            this.lblPatrols.TabIndex = 1;
            this.lblPatrols.Text = "Antal patruller";
            // 
            // btnAddPatrol
            // 
            this.btnAddPatrol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPatrol.Location = new System.Drawing.Point(302, 8);
            this.btnAddPatrol.Name = "btnAddPatrol";
            this.btnAddPatrol.Size = new System.Drawing.Size(88, 23);
            this.btnAddPatrol.TabIndex = 2;
            this.btnAddPatrol.Text = "Lägg till patrull";
            this.toolTip1.SetToolTip(this.btnAddPatrol, "Lägger till en ny patrull på slutet");
            this.btnAddPatrol.Click += new System.EventHandler(this.btnAddPatrol_Click);
            // 
            // lblPatrolsCount
            // 
            this.lblPatrolsCount.Location = new System.Drawing.Point(88, 8);
            this.lblPatrolsCount.Name = "lblPatrolsCount";
            this.lblPatrolsCount.Size = new System.Drawing.Size(24, 23);
            this.lblPatrolsCount.TabIndex = 4;
            this.lblPatrolsCount.Text = "0";
            // 
            // btnRemPatrol
            // 
            this.btnRemPatrol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemPatrol.Location = new System.Drawing.Point(390, 8);
            this.btnRemPatrol.Name = "btnRemPatrol";
            this.btnRemPatrol.Size = new System.Drawing.Size(88, 23);
            this.btnRemPatrol.TabIndex = 3;
            this.btnRemPatrol.Text = "Ta bort patrull";
            this.toolTip1.SetToolTip(this.btnRemPatrol, "Tar bort den sista patrullen. Kan endast ta bort tomma patruller.");
            this.btnRemPatrol.Click += new System.EventHandler(this.btnRemPatrol_Click);
            // 
            // btnPatrolAutomatic
            // 
            this.btnPatrolAutomatic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPatrolAutomatic.Location = new System.Drawing.Point(214, 8);
            this.btnPatrolAutomatic.Name = "btnPatrolAutomatic";
            this.btnPatrolAutomatic.Size = new System.Drawing.Size(88, 23);
            this.btnPatrolAutomatic.TabIndex = 1;
            this.btnPatrolAutomatic.Text = "Automat";
            this.toolTip1.SetToolTip(this.btnPatrolAutomatic, "Här raderas samtliga tidigare patruller och skyttarna läggs ut i nya patruller me" +
                    "d hänsyn till patrulltid, maxantal, vilotid, skytteklass och klubbtillhörighet");
            this.btnPatrolAutomatic.Click += new System.EventHandler(this.btnPatrolAutomatic_Click);
            // 
            // dataGrid1
            // 
            this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid1.DataMember = "Patrols";
            this.dataGrid1.DataSource = this.patrolsDs;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(8, 32);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.Size = new System.Drawing.Size(470, 328);
            this.dataGrid1.TabIndex = 5;
            this.dataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            this.toolTip1.SetToolTip(this.dataGrid1, "Här visas en lista på alla patruller och dess klass. Klicka på en patrull för att" +
                    " visa dess innehåll.");
            // 
            // patrolsDs
            // 
            this.patrolsDs.DataSetName = "DatasetPatrols";
            this.patrolsDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.patrolsDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = this.dataGrid1;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.DataGridTextBoxPatrol,
            this.DataGridTextBoxStartTime,
            this.dataGridTextBoxStartTimeDisplay,
            this.DataGridTextBoxClass,
            this.DataGridTextBoxNrOfCompetitors,
            this.DataGridTextBoxNrOfCompetitorsArrived,
            this.DataGridTextBoxNrOfCompetitorsWithResults});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "Patrols";
            this.dataGridTableStyle1.ReadOnly = true;
            this.dataGridTableStyle1.RowHeadersVisible = false;
            // 
            // DataGridTextBoxPatrol
            // 
            this.DataGridTextBoxPatrol.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.DataGridTextBoxPatrol.Format = "";
            this.DataGridTextBoxPatrol.FormatInfo = null;
            this.DataGridTextBoxPatrol.HeaderText = "Patrull";
            this.DataGridTextBoxPatrol.MappingName = "Id";
            this.DataGridTextBoxPatrol.ReadOnly = true;
            this.DataGridTextBoxPatrol.Width = 50;
            // 
            // DataGridTextBoxStartTime
            // 
            this.DataGridTextBoxStartTime.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.DataGridTextBoxStartTime.Format = "HH:mm";
            this.DataGridTextBoxStartTime.FormatInfo = null;
            this.DataGridTextBoxStartTime.HeaderText = "Starttid";
            this.DataGridTextBoxStartTime.MappingName = "Starttime";
            this.DataGridTextBoxStartTime.ReadOnly = true;
            this.DataGridTextBoxStartTime.Width = 60;
            // 
            // dataGridTextBoxStartTimeDisplay
            // 
            this.dataGridTextBoxStartTimeDisplay.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.dataGridTextBoxStartTimeDisplay.Format = "HH:mm";
            this.dataGridTextBoxStartTimeDisplay.FormatInfo = null;
            this.dataGridTextBoxStartTimeDisplay.HeaderText = "Visas";
            this.dataGridTextBoxStartTimeDisplay.MappingName = "StartTimeDisplay";
            this.dataGridTextBoxStartTimeDisplay.ReadOnly = true;
            this.dataGridTextBoxStartTimeDisplay.Width = 60;
            // 
            // DataGridTextBoxClass
            // 
            this.DataGridTextBoxClass.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.DataGridTextBoxClass.Format = "";
            this.DataGridTextBoxClass.FormatInfo = null;
            this.DataGridTextBoxClass.HeaderText = "Klass";
            this.DataGridTextBoxClass.MappingName = "Class";
            this.DataGridTextBoxClass.ReadOnly = true;
            this.DataGridTextBoxClass.Width = 65;
            // 
            // DataGridTextBoxNrOfCompetitors
            // 
            this.DataGridTextBoxNrOfCompetitors.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.DataGridTextBoxNrOfCompetitors.Format = "";
            this.DataGridTextBoxNrOfCompetitors.FormatInfo = null;
            this.DataGridTextBoxNrOfCompetitors.HeaderText = "Antal";
            this.DataGridTextBoxNrOfCompetitors.MappingName = "NumberOfCompetitors";
            this.DataGridTextBoxNrOfCompetitors.ReadOnly = true;
            this.DataGridTextBoxNrOfCompetitors.Width = 50;
            // 
            // DataGridTextBoxNrOfCompetitorsWithResults
            // 
            this.DataGridTextBoxNrOfCompetitorsWithResults.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.DataGridTextBoxNrOfCompetitorsWithResults.Format = "";
            this.DataGridTextBoxNrOfCompetitorsWithResults.FormatInfo = null;
            this.DataGridTextBoxNrOfCompetitorsWithResults.HeaderText = "Resultat finns";
            this.DataGridTextBoxNrOfCompetitorsWithResults.MappingName = "NumberOfCompetitorsWithResult";
            this.DataGridTextBoxNrOfCompetitorsWithResults.ReadOnly = true;
            this.DataGridTextBoxNrOfCompetitorsWithResults.Width = 80;
            // 
            // DataGridTextBoxNrOfCompetitorsArrived
            // 
            this.DataGridTextBoxNrOfCompetitorsArrived.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.DataGridTextBoxNrOfCompetitorsArrived.Format = "";
            this.DataGridTextBoxNrOfCompetitorsArrived.FormatInfo = null;
            this.DataGridTextBoxNrOfCompetitorsArrived.HeaderText = "Ankomna";
            this.DataGridTextBoxNrOfCompetitorsArrived.MappingName = "NrOfArrived";
            this.DataGridTextBoxNrOfCompetitorsArrived.NullText = "";
            this.DataGridTextBoxNrOfCompetitorsArrived.ReadOnly = true;
            this.DataGridTextBoxNrOfCompetitorsArrived.Width = 65;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(8, 32);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(470, 16);
            this.progressBar1.TabIndex = 6;
            this.progressBar1.Visible = false;
            // 
            // FPatrols
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(486, 365);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnPatrolAutomatic);
            this.Controls.Add(this.btnRemPatrol);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.lblPatrolsCount);
            this.Controls.Add(this.btnAddPatrol);
            this.Controls.Add(this.lblPatrols);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FPatrols";
            this.Text = "Patruller";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patrolsDs)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        internal bool DisposeNow = false;
        private Common.Interface CommonCode;
        private int height;
        private int width;
        private int MaxCompetitorsInPatrol = 8;
        private Allberg.Shooter.Windows.FPatrol myPatrolWindow;

        internal void UpdatedCompetition()
        {
            Structs.Competition[] comp =
                CommonCode.GetCompetitions();
            if (comp.Length == 0)
                return;

            MaxCompetitorsInPatrol = 
                comp[0].PatrolSize;
            UpdatedCompetitors();
        }
        
        Thread updatedCompetitorsThread;
        readonly object UpdatingCompetitorsThreadRunning = new object();
        internal void UpdatedCompetitors()
        {
            Trace.WriteLine("FPatrols.UpdatedCompetitors: Started");

            Trace.WriteLine("FPatrols.UpdatedCompetitors() locking \"" +
                "UpdatingCompetitorsThreadRunning\" on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");
            lock(UpdatingCompetitorsThreadRunning)
            {
                Trace.WriteLine("FPatrols.UpdatedCompetitors() locked \"" +
                    "UpdatingCompetitorsThreadRunning\" on thread \"" +
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString() + " )");

                updatedCompetitorsThread = new Thread(
                    new ThreadStart(updatedCompetitorsWithThread));

                updatedCompetitorsThread.Name = "FPatrols.updatedCompetitorsThread";
                updatedCompetitorsThread.IsBackground = true;
                updatedCompetitorsThread.Start();

                Trace.WriteLine("FPatrols.UpdatedCompetitors() unlocking \"" +
                    "UpdatingCompetitorsThreadRunning\" on thread \"" +
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString() + " )");
                Trace.WriteLine("FPatrols.UpdatedCompetitors: Ended");
            }
        }
        
        private void updatedCompetitorsWithThread()
        {
            try
            {
                Trace.WriteLine("FPatrols.updatedCompetitorsWithThread " + 
                    "started in thread " + 
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString()
                    + " ) ");
                updatePatrols();
                this.myPatrolWindow.UpdatedCompetitors();
                Trace.WriteLine("FPatrols.updatedCompetitorsWithThread ended.");
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FPatrols.updatedCompetitorsWithThread: Exception " + exc.ToString());
            }
            finally
            {
                Trace.WriteLine("FPatrols.updatedCompetitorsWithThread() ended");
            }
        }

        internal void enableMe()
        {
            this.Visible = true;
            try
            {
                Trace.WriteLine("FPatrols.enableMe() started on thread " +
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString() + " )");

                if (!DisposeNow)
                {
                    switch(CommonCode.GetCompetitions()[0].Type)
                    {
                        case Structs.CompetitionTypeEnum.Precision:
                            this.Text = "Skjutlag";
                            lblPatrols.Text = "Antal lag";
                            btnAddPatrol.Text = "Lägg till lag";
                            btnRemPatrol.Text = "Ta bort lag";
                            break;
                        case Structs.CompetitionTypeEnum.Field:
                            this.Text = "Patruller";
                            lblPatrols.Text = "Antal patruller";
                            btnAddPatrol.Text = "Lägg till patrull";
                            btnRemPatrol.Text = "Ta bort patrull";
                            break;
                    }
                    this.Focus();
                    updatePatrols();
                }
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FPatrols.enableMe Exception: " + exc.ToString());
            }
        }

        #region Display patrols
        readonly object UpdatedPatrolsThreadLock = new object();
        Thread updatedPatrolsThread;
        internal void UpdatedPatrols()
        {
            Trace.WriteLine("FPatrols.UpdatedPatrols() locking \"" +
                "UpdatedPatrolsThreadLock\" on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            lock(UpdatedPatrolsThreadLock)
            {
                Trace.WriteLine("FPatrols.UpdatedPatrols() locked \"" +
                    "UpdatedPatrolsThreadLock\" on thread \"" +
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString() + " )");

                updatedPatrolsThread = new Thread(
                    new ThreadStart(updatedPatrolsWithThread));

                updatedPatrolsThread.Name = "FPatrols:updatedPatrolsThread";
                updatedPatrolsThread.IsBackground = true;
                updatedPatrolsThread.Start();

                Trace.WriteLine("FPatrols.UpdatedPatrols() unlocking \"" +
                    "UpdatedPatrolsThreadLock\" on thread \"" +
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString() + " )");
            }		
        }

        private void datasetBindToDataGrid()
        {
            try
            {
                Trace.WriteLine("FPatrols.datasetBindToDataGrid " + 
                    "started in thread \"" + 
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString()
                    + " ) ");
                this.dataGrid1.SetDataBinding(this.patrolsDs, "Patrols");
                this.patrolsDs = null;
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }
            finally
            {
                Trace.WriteLine("FPatrols.datasetBindToDataGrid ended.");
            }
        }
        private void FPatrols_UpdateNrOfPatrols(int nrOfPatrols)
        {
            Trace.WriteLine("FPatrols.FPatrols_UpdateNrOfPatrols " + 
                "started in thread \"" + 
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.GetHashCode().ToString()
                + " ) ");

            this.lblPatrolsCount.Text = nrOfPatrols.ToString();

            Trace.WriteLine("FPatrols.FPatrols_UpdateNrOfPatrols ended.");
        }
        private void updatedPatrolsWithThread()
        {
            Trace.WriteLine("FPatrols.updatedPatrolsWithThread() started on thread " +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            updatePatrols();
            this.myPatrolWindow.UpdatedPatrol();

            Trace.WriteLine("FPatrols.updatedPatrolsWithThread() ended");
        }
        readonly object UpdatingPatrols = new object();
        int updatePatrolThreadsRunning = 0;
        private void updatePatrols()
        {
            if (!this.Visible)
                return;

            Trace.WriteLine("FPatrols.updatePatrols() started on thread " +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            if (this.updatePatrolThreadsRunning>1)
                return;

            Interlocked.Increment(ref updatePatrolThreadsRunning);

            Trace.WriteLine("FPatrols.updatePatrols() locking \"" +
                "UpdatingPatrols\" on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            bool result = Monitor.TryEnter(UpdatingPatrols);
            if (result == false)
                return;

            lock(UpdatingPatrols)
            {
                Trace.WriteLine("FPatrols.updatePatrols() locked \"" +
                    "UpdatingPatrols\" on thread \"" +
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString() + " )");
                try
                {
                    this.patrolsDs = new Allberg.Shooter.Windows.DatasetPatrols();
                    
                    this.patrolsDs.Clear();

                    foreach(Structs.Patrol patrol in CommonCode.GetPatrols())
                    {
                        DatasetPatrols.PatrolsRow row = 
                            patrolsDs.Patrols.NewPatrolsRow();
                        row.Class = patrol.PClass.ToString();
                        if (patrol.LockedForAutomatic)
                            row.Class += "-S";
                        row.Id = patrol.PatrolId;
                        row.NumberOfCompetitors = 
                            CommonCode.GetCompetitorsCountPatrol(patrol);
                        row.NumberOfCompetitorsWithResult =
                            CommonCode.GetCompetitorsWithResultCountPatrol(patrol);
                        row.Starttime = patrol.StartDateTime;
                        row.StartTimeDisplay = patrol.StartDateTimeDisplay;
                        row.NrOfArrived = CommonCode.GetCompetitorsArrivedCountPatrol(patrol);
                        patrolsDs.Patrols.AddPatrolsRow(row);
                    }
                    if (this.InvokeRequired)
                    {
                        this.Invoke(UpdateNrOfPatrols, new object[] { this.patrolsDs.Patrols.Count });
                        this.Invoke(DatasetBindToDataGrid);
                    }
                    else
                    {
                        UpdateNrOfPatrols(this.patrolsDs.Patrols.Count);
                        datasetBindToDataGrid();
                    }
                }
                catch(ApplicationException)
                {
                    // Occurs where there is no patrol. Do nothing
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
                finally
                {
                    Interlocked.Decrement(ref updatePatrolThreadsRunning);
                }
                Trace.WriteLine("FPatrols.updatePatrols() unlocking \"" +
                    "UpdatingPatrols\" on thread \"" +
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString() + " )");
                Trace.WriteLine("FPatrols.updatePatrols() ended.");

            }
            Monitor.Exit(UpdatingPatrols);
        }

        private void dataGrid1_MouseUp(object sender, MouseEventArgs e)
        {
            System.Drawing.Point pt = new Point(e.X, e.Y); 
            DataGrid.HitTestInfo hti = dataGrid1.HitTest(pt); 
            if(hti.Type == DataGrid.HitTestType.Cell) 
            {
                dataGrid1.CurrentCell = new DataGridCell(hti.Row, 0); 

                DataGridTableStyle ts = dataGrid1.TableStyles[0];
                DataGridTextBoxColumn cs = (DataGridTextBoxColumn)ts.GridColumnStyles[dataGrid1.CurrentCell.ColumnNumber]; 
                try
                {
                    myPatrolWindow.enableMe(int.Parse(cs.TextBox.Text));
                }
                catch(System.FormatException)
                {}
                catch(Exception exc)
                {
                    MessageBox.Show("Exception occured:\n" + exc.ToString());
                }
            } 
        }
        #endregion


        #region Add/remove patrol
        private void btnAddPatrol_Click(object sender, System.EventArgs e)
        {
            CommonCode.PatrolAddEmpty();
        }

        private void btnRemPatrol_Click(object sender, System.EventArgs e)
        {
            try
            {
                CommonCode.PatrolRemoveLast();
            }
            catch(ApplicationException exc)
            {
                if (exc.Message == "CompetitorsExist")
                    MessageBox.Show("Den sista patrullen innehåller " +
                        "tävlande och kan därför inte tas bort.", 
                        "Patrullborttagning", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Automatic placement of competitors
        private void btnPatrolAutomatic_Click(object sender, System.EventArgs e)
        {
            // Spread out patrols
            Thread PatrolAddAutomaticCompetitorsThread =
                new Thread(
                new ThreadStart(
                btnPatrolAutomatic_ClickThread));
            PatrolAddAutomaticCompetitorsThread.IsBackground = true;
            PatrolAddAutomaticCompetitorsThread.Name =
                "FPatrol.PatrolAddAutomaticCompetitorsThread";
            PatrolAddAutomaticCompetitorsThread.Start();
        }
        private void btnPatrolAutomatic_ClickThread()
        {
            Trace.WriteLine("FPatrols: btnPatrolAutomatic_Click started" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            DialogResult res = DialogResult.No;
            if (CommonCode.GetPatrolsCount() > 0)
            {
                res = MessageBox.Show("Det finns redan patruller och "+
                    "eventuellt skyttar utlagda. Vill du rensa dessa " +
                    "patruller och lägga ut samtliga skyttar " +
                    "till patruller med automatik?\n\n" +
                    "Detta kommer att radera de patruller som du redan skapat och " +
                    "innehållet i dessa!",
                    "Varning", 
                    MessageBoxButtons.YesNoCancel, 
                    MessageBoxIcon.Warning, 
                    MessageBoxDefaultButton.Button3);

                if (res == DialogResult.Cancel)
                    return;
            }

            bool CleanPatrols = true;
            if (res == DialogResult.No)
                CleanPatrols = false;

            if (this.InvokeRequired)
                this.Invoke(EnableAndStartProgressbar);
            else
                enableAndStartProgressbar();
            CommonCode.PatrolAddAutomaticCompetitors(CleanPatrols);
        }
        private void enableAndStartProgressbar()
        {
            this.progressBar1.Visible = true;
            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = CommonCode.GetCompetitorsCount();
        }
        private void updatePatrolAddAutomaticCompetitors(int current, int max)
        {
            progressBar1.Maximum = max;
            if (current<=max)
                this.progressBar1.Value = current;
            else
                this.progressBar1.Value = max;

            if (current == max)
            {
                this.progressBar1.Visible = false;
            }
        }
        #endregion


    }
}
