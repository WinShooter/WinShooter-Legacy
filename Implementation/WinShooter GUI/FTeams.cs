// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FTeams.cs" company="John Allberg">
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
//   Summary description for FTeams.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;
    using Allberg.Shooter.Windows.Forms;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for FTeams.
    /// </summary>
    public class FTeams : System.Windows.Forms.Form
    {
        private SafeButton btnSave;
        private SafeButton btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label label1;
        private Allberg.Shooter.Windows.Forms.SafeComboBox DdTeams;
        private SafeButton btnDelete;
        private Allberg.Shooter.Windows.Forms.SafeComboBox DdClubs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private Allberg.Shooter.Windows.Forms.SafeComboBox ddCompetitor1;
        private Allberg.Shooter.Windows.Forms.SafeComboBox ddCompetitor2;
        private Allberg.Shooter.Windows.Forms.SafeComboBox ddCompetitor3;
        private Allberg.Shooter.Windows.Forms.SafeComboBox ddCompetitor4;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtName;
        private Allberg.Shooter.Windows.Forms.SafeComboBox ddWeaponClass;
        //private MethodInvoker BindClubsTable;


        private const string NewTeamString = "<-- Nytt lag -->";
        private System.Data.DataView DdTeamsView;
        private Label label9;
        private Allberg.Shooter.Windows.Forms.SafeComboBox ddCompetitor5;
        private const string NewCompetitorString = "<-- Välj skytt -->";
        internal FTeams(ref Common.Interface newCommon)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            CommonCode = newCommon;

            Trace.WriteLine("FTeams: Creating");
            EnableMeInvoker = new MethodInvoker(this.enableMe);

            PopulateTeamsDropDownThreadInvoker += 
                new MethodInvoker(populateTeamsDropDownThread);
            try
            {
                height = this.Size.Height;
                width = this.Size.Width;
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FTeams: Exception during creation:" + exc.ToString());
                throw;
            }
            finally
            {
                Trace.WriteLine("FTeams: Created.");
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FTeams: Dispose(" + disposing.ToString() + ")" +
                "from thread \"" + System.Threading.Thread.CurrentThread.Name + "\" " +
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
                Trace.WriteLine("FTeams: exception while enabling Main:" + exc.ToString());
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
            Trace.WriteLine("FTeams: Dispose(" + disposing.ToString() + ")" +
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTeams));
            this.btnSave = new SafeButton();
            this.btnCancel = new SafeButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.DdTeams = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.DdTeamsView = new System.Data.DataView();
            this.btnDelete = new SafeButton();
            this.DdClubs = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ddWeaponClass = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ddCompetitor1 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.ddCompetitor2 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.ddCompetitor3 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.ddCompetitor4 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ddCompetitor5 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.DdTeamsView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(240, 228);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Spara";
            this.toolTip1.SetToolTip(this.btnSave, "Spara den ändrade informationen");
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(320, 228);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Stäng";
            this.toolTip1.SetToolTip(this.btnCancel, "Stäng utan att spara");
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Lag";
            // 
            // DdTeams
            // 
            this.DdTeams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DdTeams.DataSource = this.DdTeamsView;
            this.DdTeams.DisplayMember = "TeamId";
            this.DdTeams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DdTeams.Location = new System.Drawing.Point(64, 8);
            this.DdTeams.Name = "DdTeams";
            this.DdTeams.Size = new System.Drawing.Size(248, 21);
            this.DdTeams.TabIndex = 9;
            this.DdTeams.ValueMember = "TeamId";
            this.DdTeams.SelectedIndexChanged += new System.EventHandler(this.DdTeams_SelectedIndexChanged);
            // 
            // DdTeamsView
            // 
            this.DdTeamsView.ApplyDefaultSort = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(320, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Radera";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // DdClubs
            // 
            this.DdClubs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DdClubs.DisplayMember = "ClubName";
            this.DdClubs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DdClubs.Location = new System.Drawing.Point(64, 32);
            this.DdClubs.Name = "DdClubs";
            this.DdClubs.Size = new System.Drawing.Size(328, 21);
            this.DdClubs.TabIndex = 11;
            this.DdClubs.ValueMember = "ClubId";
            this.DdClubs.SelectedIndexChanged += new System.EventHandler(this.DdClubs_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "Klubb";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 23);
            this.label3.TabIndex = 13;
            this.label3.Text = "Namn";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(64, 80);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(328, 20);
            this.txtName.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 23);
            this.label4.TabIndex = 15;
            this.label4.Text = "Nr1";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 23);
            this.label5.TabIndex = 16;
            this.label5.Text = "Nr2";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 23);
            this.label6.TabIndex = 17;
            this.label6.Text = "Nr3";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 23);
            this.label7.TabIndex = 18;
            this.label7.Text = "Klass";
            // 
            // ddWeaponClass
            // 
            this.ddWeaponClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddWeaponClass.DisplayMember = "ClassName";
            this.ddWeaponClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddWeaponClass.Location = new System.Drawing.Point(64, 56);
            this.ddWeaponClass.Name = "ddWeaponClass";
            this.ddWeaponClass.Size = new System.Drawing.Size(328, 21);
            this.ddWeaponClass.TabIndex = 19;
            this.ddWeaponClass.ValueMember = "ClassId";
            this.ddWeaponClass.SelectedIndexChanged += new System.EventHandler(this.ddWeaponClass_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 23);
            this.label8.TabIndex = 20;
            this.label8.Text = "Nr4";
            // 
            // ddCompetitor1
            // 
            this.ddCompetitor1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddCompetitor1.DisplayMember = "CompName";
            this.ddCompetitor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddCompetitor1.Location = new System.Drawing.Point(64, 104);
            this.ddCompetitor1.Name = "ddCompetitor1";
            this.ddCompetitor1.Size = new System.Drawing.Size(328, 21);
            this.ddCompetitor1.TabIndex = 21;
            this.ddCompetitor1.ValueMember = "CompId";
            // 
            // ddCompetitor2
            // 
            this.ddCompetitor2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddCompetitor2.DisplayMember = "CompName";
            this.ddCompetitor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddCompetitor2.Location = new System.Drawing.Point(64, 128);
            this.ddCompetitor2.Name = "ddCompetitor2";
            this.ddCompetitor2.Size = new System.Drawing.Size(328, 21);
            this.ddCompetitor2.TabIndex = 22;
            this.ddCompetitor2.ValueMember = "CompId";
            // 
            // ddCompetitor3
            // 
            this.ddCompetitor3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddCompetitor3.DisplayMember = "CompName";
            this.ddCompetitor3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddCompetitor3.Location = new System.Drawing.Point(64, 152);
            this.ddCompetitor3.Name = "ddCompetitor3";
            this.ddCompetitor3.Size = new System.Drawing.Size(328, 21);
            this.ddCompetitor3.TabIndex = 23;
            this.ddCompetitor3.ValueMember = "CompId";
            // 
            // ddCompetitor4
            // 
            this.ddCompetitor4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddCompetitor4.DisplayMember = "CompName";
            this.ddCompetitor4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddCompetitor4.Location = new System.Drawing.Point(64, 176);
            this.ddCompetitor4.Name = "ddCompetitor4";
            this.ddCompetitor4.Size = new System.Drawing.Size(328, 21);
            this.ddCompetitor4.TabIndex = 24;
            this.ddCompetitor4.ValueMember = "CompId";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 199);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 23);
            this.label9.TabIndex = 25;
            this.label9.Text = "Nr5";
            // 
            // ddCompetitor5
            // 
            this.ddCompetitor5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddCompetitor5.DisplayMember = "CompName";
            this.ddCompetitor5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddCompetitor5.Location = new System.Drawing.Point(64, 199);
            this.ddCompetitor5.Name = "ddCompetitor5";
            this.ddCompetitor5.Size = new System.Drawing.Size(328, 21);
            this.ddCompetitor5.TabIndex = 26;
            this.ddCompetitor5.ValueMember = "CompId";
            // 
            // FTeams
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(400, 258);
            this.Controls.Add(this.ddCompetitor5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ddCompetitor4);
            this.Controls.Add(this.ddCompetitor3);
            this.Controls.Add(this.ddCompetitor2);
            this.Controls.Add(this.ddCompetitor1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ddWeaponClass);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DdClubs);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.DdTeams);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FTeams";
            this.Text = "Lag";
            ((System.ComponentModel.ISupportInitialize)(this.DdTeamsView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        internal bool DisposeNow = false;
        public delegate void EnableMainHandler();
        public event EnableMainHandler EnableMain;

        Common.Interface CommonCode;
        int height;
        int width;

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Visible = false;
            EnableMain();
        }

        internal void EnableMe()
        {
            Trace.WriteLine("FCompetitors: EnableMe() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            /*if (System.Threading.Thread.CurrentThread.Name == "Main Thread")
                enableMe();
            else
            {*/
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(EnableMeInvoker);
                }
                else
                {
                    this.enableMe();
                }
            //}
        }
        private MethodInvoker EnableMeInvoker;
        private void enableMe()
        {
            Trace.WriteLine("FCompetitors: enableMe() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            this.Visible = true;

            populateWeaponsClassDropDown();
            populateTeamsDropDown();
            populateClubsDropDown();
            clearEverything();
            this.Focus();
            Trace.WriteLine("FCompetitors: enableMe() ended.");
        }

        private void clearEverything()
        {
            this.DdTeams.SelectedIndex = 0;
            this.DdClubs.SelectedIndex = 0;
            this.ddCompetitor1.SelectedIndex = 0;
            this.ddCompetitor2.SelectedIndex = 0;
            this.ddCompetitor3.SelectedIndex = 0;
            this.ddCompetitor4.SelectedIndex = 0;
            this.ddCompetitor5.SelectedIndex = 0;
        }

        #region Populate TeamsDropDown
        private void populateTeamsDropDown()
        {
            Trace.WriteLine("FCompetitors: populateTeamsDropDown() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            /*System.Threading.Thread thread = 
                new Thread(
                new ThreadStart(this.populateTeamsDropDownThread));
            thread.IsBackground = true;
            thread.Name = "populateTeamsDropDownThread";
            thread.Start();*/
            populateTeamsDropDownThread();

            Trace.WriteLine("FCompetitors: populateTeamsDropDown() ended.");
        }
        MethodInvoker PopulateTeamsDropDownThreadInvoker;
        private readonly object populateTeamsDropDownThreadLock = new object();
        private void populateTeamsDropDownThread()
        {
            Trace.WriteLine("FCompetitors: populateTeamsDropDown() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            if ( DdTeams.InvokeRequired )
            {
                Invoke(PopulateTeamsDropDownThreadInvoker);
                return;
            }
                
            lock(populateTeamsDropDownThreadLock)
            {
                DdTeamsView.Table = null;

                // Get the teams from the database
                Structs.Team[] teams = CommonCode.GetTeams();
                DatasetTeams ds = new DatasetTeams();

                // Create the default row
                DatasetTeams.TeamsRow defaultrow =
                    ds.Teams.NewTeamsRow();
                defaultrow.TeamsId = -1;
                defaultrow.TeamsName = NewTeamString;
                ds.Teams.AddTeamsRow(defaultrow);

                foreach(Structs.Team team in teams)
                {
                    DatasetTeams.TeamsRow row =
                        ds.Teams.NewTeamsRow();
                    row.TeamsId = team.TeamId;
                    string clubName = CommonCode.GetClub(team.ClubId).Name;
                    row.TeamsName = clubName + " - " + team.Name;
                    ds.Teams.AddTeamsRow(row);
                }
                DdTeamsView.Table = ds.Teams;
                DdTeamsView.Sort = "TeamsName";
                DdTeams.DisplayMember = "TeamsName";
                DdTeams.ValueMember = "TeamsId";
            }
            Trace.WriteLine("FCompetitors: populateTeamsDropDown() ended.");
        }
        #endregion

        #region Populate ClubsDropDown
        private void populateClubsDropDown()
        {
            Trace.WriteLine("FCompetitors: populateClubsDropDown() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            System.Threading.Thread thread = 
                new Thread(
                new ThreadStart(this.populateClubsDropDownThread));
            thread.IsBackground = true;
            thread.Name = "populateClubsDropDownThread";
            thread.Start();

            Trace.WriteLine("FCompetitors: populateClubsDropDown() ended.");
        }
        private readonly object populateClubsDropDownThreadLock = new object();
        private void populateClubsDropDownThread()
        {
            Trace.WriteLine("FCompetitors: populateClubsDropDownThread() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            lock(populateClubsDropDownThreadLock)
            {
                // Get the teams from the database
                Structs.Club[] clubs = CommonCode.GetClubs();
                DatasetTeams ds = new DatasetTeams();

                foreach(Structs.Club club in clubs)
                {
                    DatasetTeams.ClubsRow row =
                        ds.Clubs.NewClubsRow();
                    row.ClubId = club.ClubId;
                    row.ClubName = club.Name;
                    ds.Clubs.AddClubsRow(row);
                }
                this.DdClubs.DataSource = ds.Clubs;
                this.DdClubs.ValueMember = "ClubId";
                this.DdClubs.DisplayMember = "ClubName";
            }
            Trace.WriteLine("FCompetitors: populateClubsDropDownThread() ended.");
        }
        #endregion

        #region Populate WeaponsClassDropDown
        private void populateWeaponsClassDropDown()
        {
            Trace.WriteLine("FCompetitors: populateWeaponsClassDropDown() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            Structs.Competition[] comps = CommonCode.GetCompetitions();
            if (comps.Length == 0)
                return;

            Structs.Competition comp = comps[0];

            DatasetTeams ds = new DatasetTeams();
            for(int i=1;i<=Structs.ResultWeaponsClassMax;i++)
            {
                Structs.ResultWeaponsClass wc = (Structs.ResultWeaponsClass)i;
                try
                {
                    int test = int.Parse(wc.ToString());
                }
                catch(System.FormatException)
                {
                    DatasetTeams.WeaponClassRow row =
                            ds.WeaponClass.NewWeaponClassRow();
                    row.ClassId = i;
                    row.ClassName = wc.ToString() + "-vapen";
                    if ((comp.Type != Structs.CompetitionTypeEnum.MagnumField &
                        row.ClassName.IndexOf("M") == -1) |
                        (comp.Type == Structs.CompetitionTypeEnum.MagnumField &
                        row.ClassName.IndexOf("M") > -1 &
                        row.ClassName != "M-vapen"))
                    {
                        ds.WeaponClass.AddWeaponClassRow(row);
                    }
                }
                this.ddWeaponClass.DataSource = ds.WeaponClass;
                this.ddWeaponClass.DisplayMember = "ClassName";
                this.ddWeaponClass.ValueMember = "ClassId";
            }
            Trace.WriteLine("FCompetitors: populateWeaponsClassDropDown() ended.");
        }
        #endregion

        #region Populating shooters
        private void DdClubs_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FCompetitors: DdClubs_SelectedIndexChanged() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            populateCompetitors();

            Trace.WriteLine("FCompetitors: DdClubs_SelectedIndexChanged() ended.");
        }
        private void ddWeaponClass_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FCompetitors: ddWeaponClass_SelectedIndexChanged() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            populateCompetitors();

            Trace.WriteLine("FCompetitors: ddWeaponClass_SelectedIndexChanged() ended.");
        }
        private void populateCompetitors()
        {
            Trace.WriteLine("FCompetitors: DdClubs_SelectedIndexChanged() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            Structs.Club club = 
                CommonCode.GetClub(
                (string)this.DdClubs.SelectedValue);
            Structs.ResultWeaponsClass wclass = 
                (Structs.ResultWeaponsClass)
                (int)this.ddWeaponClass.SelectedValue;

            // Get all shooters that belong to the current club and 
            // have a competitor with the current class
            //Structs.Shooter[] shooters = CommonCode.GetShooters(club, wclass);
            Structs.Competitor[] comps = CommonCode.GetCompetitors(club, wclass, "Surname, Givenname");

            DatasetTeams ds1 = new DatasetTeams();
            DatasetTeams ds2 = new DatasetTeams();
            DatasetTeams ds3 = new DatasetTeams();
            DatasetTeams ds4 = new DatasetTeams();
            DatasetTeams ds5 = new DatasetTeams();

            shooters = new Hashtable();
            addShootersToDS(ds1, comps);
            addShootersToDS(ds2, comps);
            addShootersToDS(ds3, comps);
            addShootersToDS(ds4, comps);
            addShootersToDS(ds5, comps);

            // Bind the dropdowns to the datatable
            this.ddCompetitor1.DataSource = ds1.Competitors;
            this.ddCompetitor2.DataSource = ds2.Competitors;
            this.ddCompetitor3.DataSource = ds3.Competitors;
            this.ddCompetitor4.DataSource = ds4.Competitors;
            this.ddCompetitor5.DataSource = ds5.Competitors;

            Trace.WriteLine("FCompetitors: DdClubs_SelectedIndexChanged() ended.");
        }

        Hashtable shooters = new Hashtable();
        private void addShootersToDS(DatasetTeams ds, Structs.Competitor[] comps)
        {
            // Add all those shooters to the datatable

            // Add a default line on top
            DatasetTeams.CompetitorsRow defaultrow = 
                ds.Competitors.NewCompetitorsRow();				
            defaultrow.CompId = -1;
            defaultrow.CompName = NewCompetitorString;
            ds.Competitors.AddCompetitorsRow(defaultrow);

            foreach(Structs.Competitor comp in comps)
            {
                DatasetTeams.CompetitorsRow row = 
                    ds.Competitors.NewCompetitorsRow();
                
                Structs.Shooter shooter;
                if (shooters.ContainsKey(comp.ShooterId))
                {
                    shooter = (Structs.Shooter)shooters[comp.ShooterId];
                }
                else
                {
                    shooter = CommonCode.GetShooter(comp.ShooterId);
                    shooters.Add(comp.ShooterId, shooter);
                }

                row.CompName = shooter.Surname + " " + shooter.Givenname;
                if (int.Parse(shooter.CardNr) > 0)
                    row.CompName += " (" + shooter.CardNr + ")";
                row.CompId = comp.CompetitorId;
                ds.Competitors.AddCompetitorsRow(row);
            }
        }
        #endregion

        #region Saving
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FCompetitors: btnSave_Click() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            if (this.txtName.Text == "")
            {
                MessageBox.Show("Du måste sätta ett namn på laget.", 
                    "Felinmatning", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Hand);
                txtName.Focus();
                return;
            }
            try
            {
                Hashtable test = new Hashtable();
                if ((int)this.ddCompetitor1.SelectedValue != -1)
                    test.Add((int)this.ddCompetitor1.SelectedValue, null);
                if ((int)this.ddCompetitor2.SelectedValue != -1)
                    test.Add((int)this.ddCompetitor2.SelectedValue, null);
                if ((int)this.ddCompetitor3.SelectedValue != -1)
                    test.Add((int)this.ddCompetitor3.SelectedValue, null);
                if ((int)this.ddCompetitor4.SelectedValue != -1)
                    test.Add((int)this.ddCompetitor4.SelectedValue, null);
                if ((int)this.ddCompetitor5.SelectedValue != -1)
                    test.Add((int)this.ddCompetitor5.SelectedValue, null);
            }
            catch(System.ArgumentException)
            {
                MessageBox.Show("Du kan inte ange samma skytt flera gånger i samma lag.", 
                    "Felinmatning", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Hand);
                return;
            }

            // Ok, everything is checked, go ahead and save.
            saveMe();

            Trace.WriteLine("FCompetitors: btnSave_Click() ended.");
        }
        private void saveMe()
        {
            Trace.WriteLine("FCompetitors: saveMe() " +
                "started on thread \"" +
                Thread.CurrentThread.Name + 
                "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + 
                " )");

            Structs.Team team;
            if ((int)this.DdTeams.SelectedValue != -1)
            {
                team = CommonCode.GetTeam((int)this.DdTeams.SelectedValue);
                team.CompetitorIds = new List<int>();
            }
            else
            {
                team = new Structs.Team();
                team.CompetitorIds = new List<int>();
                team.TeamId = -1;
            }

            team.Name = this.txtName.Text;
            team.WClass = (Structs.ResultWeaponsClass)(int)this.ddWeaponClass.SelectedValue;
            team.ClubId = (string)this.DdClubs.SelectedValue;
            if ((int)ddCompetitor1.SelectedValue != -1)
                team.CompetitorIds.Add( (int)ddCompetitor1.SelectedValue);
            if ((int)ddCompetitor2.SelectedValue != -1)
                team.CompetitorIds.Add( (int)this.ddCompetitor2.SelectedValue);
            if ((int)ddCompetitor3.SelectedValue != -1)
                team.CompetitorIds.Add((int)this.ddCompetitor3.SelectedValue);
            if ((int)ddCompetitor4.SelectedValue != -1)
                team.CompetitorIds.Add((int)this.ddCompetitor4.SelectedValue);
            if ((int)ddCompetitor5.SelectedValue != -1)
                team.CompetitorIds.Add( (int)this.ddCompetitor5.SelectedValue);

            if (team.TeamId == -1)
                CommonCode.NewTeam(team);
            else
                CommonCode.UpdateTeam(team);

            clearEverything();
            Trace.WriteLine("FCompetitors: saveMe() ended.");
        }
        #endregion

        #region Deleting
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            DialogResult res =
                MessageBox.Show("Är du säker?", 
                    "Är du säker?", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question, 
                    MessageBoxDefaultButton.Button2);
            if (res != DialogResult.Yes)
                return;

            int teamId = (int)this.DdTeams.SelectedValue;
            Structs.Team team = CommonCode.GetTeam(teamId);

            CommonCode.DelTeam(team);
            clearEverything();
        }
        #endregion

        #region Updating
        internal void UpdatedTeams()
        {
            populateTeamsDropDown();
        }
        internal void UpdatedClubs()
        {
            populateClubsDropDown();
        }
        #endregion

        #region updating after change of ddteams
        private void DdTeams_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.ddWeaponClass.SelectedValue == null)
                return;
            if (this.DdTeams.SelectedValue.GetType() != typeof(int))
                return;
            if ((int)this.DdTeams.SelectedValue == -1)
            {
                this.DdClubs.SelectedIndex = 0;
                this.txtName.Text = "";
                this.ddWeaponClass.SelectedIndex = 0;
                this.ddCompetitor1.SelectedIndex = 0;
                this.ddCompetitor2.SelectedIndex = 0;
                this.ddCompetitor3.SelectedIndex = 0;
                this.ddCompetitor4.SelectedIndex = 0;
                this.ddCompetitor5.SelectedIndex = 0;
            }
            else
            {
                Structs.Team team = CommonCode.GetTeam((int)this.DdTeams.SelectedValue);
                this.DdClubs.SelectedValue = team.ClubId;
                this.txtName.Text = team.Name;
                this.ddWeaponClass.SelectedValue = team.WClass;

                this.ddCompetitor1.SelectedIndex = 0;
                this.ddCompetitor2.SelectedIndex = 0;
                this.ddCompetitor3.SelectedIndex = 0;
                this.ddCompetitor4.SelectedIndex = 0;
                this.ddCompetitor5.SelectedIndex = 0;

                int i = 1;
                foreach (int competitorId in team.CompetitorIds)
                {
                    switch (i)
                    {
                        case 1:
                            ddCompetitor1.SelectedValue = competitorId;
                            break;
                        case 2:
                            ddCompetitor2.SelectedValue = competitorId;
                            break;
                        case 3:
                            ddCompetitor3.SelectedValue = competitorId;
                            break;
                        case 4:
                            ddCompetitor4.SelectedValue = competitorId;
                            break;
                        case 5:
                            ddCompetitor5.SelectedValue = competitorId;
                            break;
                    }
                    i++;
                }
            }
        }
        #endregion

    }
}
