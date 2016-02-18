// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FPatrol.cs" company="John Allberg">
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
//   Summary description for FPatrol.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    using Allberg.Shooter.Common.Exceptions;
    using Allberg.Shooter.Windows.Forms;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for FPatrol.
    /// </summary>
    public class FPatrol : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>

        private int MaxCompetitorsInPatrol = 8;
        internal bool DisposeNow = false;
        private Common.Interface CommonCode;
        private FPatrols parentWindow;
        public event MethodInvoker EnablePatrols;
        private MethodInvoker BindPatrolNumberAndClass;
        private Allberg.Shooter.Windows.DatasetPatrol datasetPatrol;
        private MethodInvoker BindDatagrid;
        private int patrolid = -1;
        private SafeButton btnMoveUp;
        private SafeButton btnMoveDown;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridAssignedWeaponClass;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridUnassignedWeaponClass;
        private SafeLabel lblPatrolnumber;
        private SafeLabel safeLabel1;
        private SafeDateTimePicker timePatrol;
        private SafeCheckBox chkUseAutomaticTime;
        private CheckBox chkBlockThisPatrolForAutomatic;
        private Structs.Patrol Patrol;

        private delegate void SetVisibillityHandler(bool setVisible);
        private event SetVisibillityHandler SetVisibillity;

        internal FPatrol(ref Common.Interface newCommon, FPatrols newParentWindow)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            CommonCode = newCommon;
            parentWindow = newParentWindow;
            BindPatrolNumberAndClass += 
                new MethodInvoker(bindPatrolNumberAndClass);
            BindDatagrid += 
                new MethodInvoker(bindDatagrid);
            SetVisibillity += new SetVisibillityHandler(FPatrol_SetVisibillity);

            this.Resize += new EventHandler(FPatrol_Resize);
        }

        void FPatrol_SetVisibillity(bool setVisible)
        {
            Visible = setVisible;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FPatrol: Dispose(" + disposing.ToString() + ")" +
                "from thread \"" + Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            this.Visible = false;

            if(!DisposeNow & !parentWindow.DisposeNow)
            {
                EnablePatrols();
                return;
            }

            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );

            Trace.WriteLine("FPatrol: Dispose(" + disposing.ToString() + ")" +
                "ended " +
                DateTime.Now.ToLongTimeString());
        }

        private System.ComponentModel.IContainer components;

        #region Windows Form Designer generated code

        private System.Windows.Forms.DataGrid dataGridAssigned;
        private System.Windows.Forms.DataGrid dataGridUnassigned;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyleAssigned;
        private System.Windows.Forms.DataGridBoolColumn dataGridAssignedMove;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridAssignedLane;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridAssignedName;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridAssignedClub;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridAssignedWeapon;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyleUnassigned;
        private System.Windows.Forms.DataGridBoolColumn dataGridUnassignedLane;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridUnassignedName;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridUnassignedClub;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridUnassignedWeapon;
        private SafeLabel lblPatrol;
        private SafeLabel SafeLabel2;
        private SafeLabel lblPatrolClass;
        private SafeButton btnAddToPatrol;
        private SafeButton btnRemoveFromPatrol;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPatrol));
            this.datasetPatrol = new Allberg.Shooter.Windows.DatasetPatrol();
            this.dataGridAssigned = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyleAssigned = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridAssignedMove = new System.Windows.Forms.DataGridBoolColumn();
            this.dataGridAssignedLane = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridAssignedName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridAssignedClub = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridAssignedWeaponClass = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridAssignedWeapon = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridUnassigned = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyleUnassigned = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridUnassignedLane = new System.Windows.Forms.DataGridBoolColumn();
            this.dataGridUnassignedName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridUnassignedClub = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridUnassignedWeaponClass = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridUnassignedWeapon = new System.Windows.Forms.DataGridTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timePatrol = new Allberg.Shooter.Windows.Forms.SafeDateTimePicker();
            this.btnMoveDown = new SafeButton();
            this.btnMoveUp = new SafeButton();
            this.btnRemoveFromPatrol = new SafeButton();
            this.btnAddToPatrol = new SafeButton();
            this.chkUseAutomaticTime = new Allberg.Shooter.Windows.Forms.SafeCheckBox();
            this.safeLabel1 = new SafeLabel();
            this.lblPatrolClass = new SafeLabel();
            this.SafeLabel2 = new SafeLabel();
            this.lblPatrol = new SafeLabel();
            this.lblPatrolnumber = new SafeLabel();
            this.chkBlockThisPatrolForAutomatic = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.datasetPatrol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAssigned)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUnassigned)).BeginInit();
            this.SuspendLayout();
            // 
            // datasetPatrol
            // 
            this.datasetPatrol.DataSetName = "DatasetPatrol";
            this.datasetPatrol.Locale = new System.Globalization.CultureInfo("en-US");
            this.datasetPatrol.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridAssigned
            // 
            this.dataGridAssigned.AllowNavigation = false;
            this.dataGridAssigned.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridAssigned.CaptionText = "Skyttar i patrullen";
            this.dataGridAssigned.DataMember = "shooters";
            this.dataGridAssigned.DataSource = this.datasetPatrol;
            this.dataGridAssigned.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridAssigned.Location = new System.Drawing.Point(8, 47);
            this.dataGridAssigned.Name = "dataGridAssigned";
            this.dataGridAssigned.Size = new System.Drawing.Size(272, 244);
            this.dataGridAssigned.TabIndex = 6;
            this.dataGridAssigned.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyleAssigned});
            this.toolTip1.SetToolTip(this.dataGridAssigned, "Skyttar som redan finns i patrullen");
            // 
            // dataGridTableStyleAssigned
            // 
            this.dataGridTableStyleAssigned.DataGrid = this.dataGridAssigned;
            this.dataGridTableStyleAssigned.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dataGridAssignedMove,
            this.dataGridAssignedLane,
            this.dataGridAssignedName,
            this.dataGridAssignedClub,
            this.dataGridAssignedWeaponClass,
            this.dataGridAssignedWeapon});
            this.dataGridTableStyleAssigned.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyleAssigned.MappingName = "shooters";
            this.dataGridTableStyleAssigned.RowHeadersVisible = false;
            // 
            // dataGridAssignedMove
            // 
            this.dataGridAssignedMove.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.dataGridAssignedMove.AllowNull = false;
            this.dataGridAssignedMove.MappingName = "Move";
            this.dataGridAssignedMove.Width = 20;
            // 
            // dataGridAssignedLane
            // 
            this.dataGridAssignedLane.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.dataGridAssignedLane.Format = "";
            this.dataGridAssignedLane.FormatInfo = null;
            this.dataGridAssignedLane.HeaderText = "Bana";
            this.dataGridAssignedLane.MappingName = "Lane";
            this.dataGridAssignedLane.ReadOnly = true;
            this.dataGridAssignedLane.Width = 40;
            // 
            // dataGridAssignedName
            // 
            this.dataGridAssignedName.Format = "";
            this.dataGridAssignedName.FormatInfo = null;
            this.dataGridAssignedName.HeaderText = "Namn";
            this.dataGridAssignedName.MappingName = "Name";
            this.dataGridAssignedName.ReadOnly = true;
            this.dataGridAssignedName.Width = 75;
            // 
            // dataGridAssignedClub
            // 
            this.dataGridAssignedClub.Format = "";
            this.dataGridAssignedClub.FormatInfo = null;
            this.dataGridAssignedClub.HeaderText = "Klubb";
            this.dataGridAssignedClub.MappingName = "Club";
            this.dataGridAssignedClub.ReadOnly = true;
            this.dataGridAssignedClub.Width = 75;
            // 
            // dataGridAssignedWeaponClass
            // 
            this.dataGridAssignedWeaponClass.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.dataGridAssignedWeaponClass.Format = "";
            this.dataGridAssignedWeaponClass.FormatInfo = null;
            this.dataGridAssignedWeaponClass.HeaderText = "Klass";
            this.dataGridAssignedWeaponClass.MappingName = "WeaponClass";
            this.dataGridAssignedWeaponClass.NullText = "";
            this.dataGridAssignedWeaponClass.ReadOnly = true;
            this.dataGridAssignedWeaponClass.Width = 40;
            // 
            // dataGridAssignedWeapon
            // 
            this.dataGridAssignedWeapon.Format = "";
            this.dataGridAssignedWeapon.FormatInfo = null;
            this.dataGridAssignedWeapon.HeaderText = "Vapen";
            this.dataGridAssignedWeapon.MappingName = "Weapon";
            this.dataGridAssignedWeapon.ReadOnly = true;
            this.dataGridAssignedWeapon.Width = 75;
            // 
            // dataGridUnassigned
            // 
            this.dataGridUnassigned.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridUnassigned.CaptionText = "Skyttar utan patrull";
            this.dataGridUnassigned.DataMember = "unassigned";
            this.dataGridUnassigned.DataSource = this.datasetPatrol;
            this.dataGridUnassigned.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridUnassigned.Location = new System.Drawing.Point(318, 47);
            this.dataGridUnassigned.Name = "dataGridUnassigned";
            this.dataGridUnassigned.Size = new System.Drawing.Size(272, 244);
            this.dataGridUnassigned.TabIndex = 9;
            this.dataGridUnassigned.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyleUnassigned});
            this.toolTip1.SetToolTip(this.dataGridUnassigned, "Skyttar som inte blivit tilldelad någon patrull");
            // 
            // dataGridTableStyleUnassigned
            // 
            this.dataGridTableStyleUnassigned.DataGrid = this.dataGridUnassigned;
            this.dataGridTableStyleUnassigned.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dataGridUnassignedLane,
            this.dataGridUnassignedName,
            this.dataGridUnassignedClub,
            this.dataGridUnassignedWeaponClass,
            this.dataGridUnassignedWeapon});
            this.dataGridTableStyleUnassigned.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyleUnassigned.MappingName = "unassigned";
            this.dataGridTableStyleUnassigned.RowHeadersVisible = false;
            // 
            // dataGridUnassignedLane
            // 
            this.dataGridUnassignedLane.AllowNull = false;
            this.dataGridUnassignedLane.MappingName = "Move";
            this.dataGridUnassignedLane.Width = 20;
            // 
            // dataGridUnassignedName
            // 
            this.dataGridUnassignedName.Format = "";
            this.dataGridUnassignedName.FormatInfo = null;
            this.dataGridUnassignedName.HeaderText = "Namn";
            this.dataGridUnassignedName.MappingName = "Name";
            this.dataGridUnassignedName.ReadOnly = true;
            this.dataGridUnassignedName.Width = 75;
            // 
            // dataGridUnassignedClub
            // 
            this.dataGridUnassignedClub.Format = "";
            this.dataGridUnassignedClub.FormatInfo = null;
            this.dataGridUnassignedClub.HeaderText = "Klubb";
            this.dataGridUnassignedClub.MappingName = "Club";
            this.dataGridUnassignedClub.ReadOnly = true;
            this.dataGridUnassignedClub.Width = 75;
            // 
            // dataGridUnassignedWeaponClass
            // 
            this.dataGridUnassignedWeaponClass.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.dataGridUnassignedWeaponClass.Format = "";
            this.dataGridUnassignedWeaponClass.FormatInfo = null;
            this.dataGridUnassignedWeaponClass.HeaderText = "Klass";
            this.dataGridUnassignedWeaponClass.MappingName = "WeaponClass";
            this.dataGridUnassignedWeaponClass.NullText = "";
            this.dataGridUnassignedWeaponClass.ReadOnly = true;
            this.dataGridUnassignedWeaponClass.Width = 40;
            // 
            // dataGridUnassignedWeapon
            // 
            this.dataGridUnassignedWeapon.Format = "";
            this.dataGridUnassignedWeapon.FormatInfo = null;
            this.dataGridUnassignedWeapon.HeaderText = "Vapen";
            this.dataGridUnassignedWeapon.MappingName = "Weapon";
            this.dataGridUnassignedWeapon.ReadOnly = true;
            this.dataGridUnassignedWeapon.Width = 75;
            // 
            // timePatrol
            // 
            this.timePatrol.CustomFormat = "hh:mm";
            this.timePatrol.Enabled = false;
            this.timePatrol.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timePatrol.Location = new System.Drawing.Point(276, 6);
            this.timePatrol.Name = "timePatrol";
            this.timePatrol.ShowUpDown = true;
            this.timePatrol.Size = new System.Drawing.Size(73, 20);
            this.timePatrol.TabIndex = 14;
            this.toolTip1.SetToolTip(this.timePatrol, "Patrullens starttid kan sättas till annan tid än den som automatiskt sätts");
            this.timePatrol.Value = new System.DateTime(2006, 6, 13, 19, 2, 18, 562);
            this.timePatrol.ValueChanged += new System.EventHandler(this.timePatrol_ValueChanged);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveDown.Location = new System.Drawing.Point(280, 128);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(32, 23);
            this.btnMoveDown.TabIndex = 11;
            this.btnMoveDown.Text = "ß";
            this.toolTip1.SetToolTip(this.btnMoveDown, "Flytta skyttar till en senare bana");
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveUp.Location = new System.Drawing.Point(280, 104);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(32, 23);
            this.btnMoveUp.TabIndex = 10;
            this.btnMoveUp.Text = "Ý";
            this.toolTip1.SetToolTip(this.btnMoveUp, "Flytta skyttar till en tidigare bana");
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnRemoveFromPatrol
            // 
            this.btnRemoveFromPatrol.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveFromPatrol.Location = new System.Drawing.Point(280, 56);
            this.btnRemoveFromPatrol.Name = "btnRemoveFromPatrol";
            this.btnRemoveFromPatrol.Size = new System.Drawing.Size(32, 23);
            this.btnRemoveFromPatrol.TabIndex = 8;
            this.btnRemoveFromPatrol.Text = "Þ";
            this.toolTip1.SetToolTip(this.btnRemoveFromPatrol, "Ta bort skyttar ur patrullen");
            this.btnRemoveFromPatrol.Click += new System.EventHandler(this.btnRemoveFromPatrol_Click);
            // 
            // btnAddToPatrol
            // 
            this.btnAddToPatrol.Font = new System.Drawing.Font("Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddToPatrol.Location = new System.Drawing.Point(280, 80);
            this.btnAddToPatrol.Name = "btnAddToPatrol";
            this.btnAddToPatrol.Size = new System.Drawing.Size(32, 23);
            this.btnAddToPatrol.TabIndex = 7;
            this.btnAddToPatrol.Text = "Ü";
            this.toolTip1.SetToolTip(this.btnAddToPatrol, "Lägg till skyttar i patrullen");
            this.btnAddToPatrol.Click += new System.EventHandler(this.btnAddToPatrol_Click);
            // 
            // chkUseAutomaticTime
            // 
            this.chkUseAutomaticTime.AutoSize = true;
            this.chkUseAutomaticTime.Location = new System.Drawing.Point(355, 8);
            this.chkUseAutomaticTime.Name = "chkUseAutomaticTime";
            this.chkUseAutomaticTime.Size = new System.Drawing.Size(131, 17);
            this.chkUseAutomaticTime.TabIndex = 15;
            this.chkUseAutomaticTime.Text = "Använd automatisk tid";
            this.chkUseAutomaticTime.UseVisualStyleBackColor = true;
            this.chkUseAutomaticTime.CheckedChanged += new System.EventHandler(this.chkUseAutomaticTime_CheckedChanged);
            // 
            // safeLabel1
            // 
            this.safeLabel1.AutoSize = true;
            this.safeLabel1.Location = new System.Drawing.Point(230, 8);
            this.safeLabel1.Name = "safeLabel1";
            this.safeLabel1.Size = new System.Drawing.Size(40, 13);
            this.safeLabel1.TabIndex = 13;
            this.safeLabel1.Text = "Starttid";
            // 
            // lblPatrolClass
            // 
            this.lblPatrolClass.Location = new System.Drawing.Point(184, 8);
            this.lblPatrolClass.Name = "lblPatrolClass";
            this.lblPatrolClass.Size = new System.Drawing.Size(40, 23);
            this.lblPatrolClass.TabIndex = 5;
            this.lblPatrolClass.Text = "Klass1";
            // 
            // SafeLabel2
            // 
            this.SafeLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SafeLabel2.Location = new System.Drawing.Point(136, 8);
            this.SafeLabel2.Name = "SafeLabel2";
            this.SafeLabel2.Size = new System.Drawing.Size(40, 23);
            this.SafeLabel2.TabIndex = 4;
            this.SafeLabel2.Text = "Klass";
            // 
            // lblPatrol
            // 
            this.lblPatrol.Location = new System.Drawing.Point(96, 8);
            this.lblPatrol.Name = "lblPatrol";
            this.lblPatrol.Size = new System.Drawing.Size(32, 23);
            this.lblPatrol.TabIndex = 3;
            this.lblPatrol.Text = "0";
            this.lblPatrol.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblPatrolnumber
            // 
            this.lblPatrolnumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatrolnumber.Location = new System.Drawing.Point(8, 8);
            this.lblPatrolnumber.Name = "lblPatrolnumber";
            this.lblPatrolnumber.Size = new System.Drawing.Size(88, 23);
            this.lblPatrolnumber.TabIndex = 2;
            this.lblPatrolnumber.Text = "Patrullnummer";
            // 
            // chkBlockThisPatrolForAutomatic
            // 
            this.chkBlockThisPatrolForAutomatic.AutoSize = true;
            this.chkBlockThisPatrolForAutomatic.Location = new System.Drawing.Point(12, 24);
            this.chkBlockThisPatrolForAutomatic.Name = "chkBlockThisPatrolForAutomatic";
            this.chkBlockThisPatrolForAutomatic.Size = new System.Drawing.Size(233, 17);
            this.chkBlockThisPatrolForAutomatic.TabIndex = 16;
            this.chkBlockThisPatrolForAutomatic.Text = "Stäng denna patrull för automatisk placering";
            this.chkBlockThisPatrolForAutomatic.UseVisualStyleBackColor = true;
            this.chkBlockThisPatrolForAutomatic.CheckedChanged += new System.EventHandler(this.chkBlockThisPatrolForAutomatic_CheckedChanged);
            // 
            // FPatrol
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(598, 297);
            this.Controls.Add(this.chkBlockThisPatrolForAutomatic);
            this.Controls.Add(this.chkUseAutomaticTime);
            this.Controls.Add(this.timePatrol);
            this.Controls.Add(this.safeLabel1);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.dataGridUnassigned);
            this.Controls.Add(this.btnRemoveFromPatrol);
            this.Controls.Add(this.btnAddToPatrol);
            this.Controls.Add(this.dataGridAssigned);
            this.Controls.Add(this.lblPatrolClass);
            this.Controls.Add(this.SafeLabel2);
            this.Controls.Add(this.lblPatrol);
            this.Controls.Add(this.lblPatrolnumber);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FPatrol";
            this.Text = "Patrull";
            ((System.ComponentModel.ISupportInitialize)(this.datasetPatrol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAssigned)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUnassigned)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Interface Updates
        internal void enableMe(int newpatrolid)
        {
            Trace.WriteLine("FPatrol: enableMe started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            switch(CommonCode.GetCompetitions()[0].Type)
            {
                case Structs.CompetitionTypeEnum.Precision:
                    this.Text = "Skjutlag";
                    lblPatrolnumber.Text = "Skjutlagsnr";
                    dataGridAssigned.CaptionText = "Skyttar i laget";
                    dataGridUnassigned.CaptionText = "Skyttar utan lag";
                    break;
                case Structs.CompetitionTypeEnum.Field:
                    this.Text = "Patrull";
                    lblPatrolnumber.Text = "Patrullnummer";
                    dataGridAssigned.CaptionText = "Skyttar i patrullen";
                    dataGridUnassigned.CaptionText = "Skyttar utan patrull";
                    break;
            }

            this.Visible = true;
            this.Focus();
            patrolid = newpatrolid;
            updatePatrol();

            Trace.WriteLine("FPatrol: enableMe ended.");
        }

        internal void UpdatedCompetitors()
        {
            Trace.WriteLine("FPatrol: UpdatedCompetitors started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            if (this.Visible)
                updatePatrol();
            
            Trace.WriteLine("FPatrol: UpdatedCompetitors ended.");
        }
        internal void UpdatedPatrol()
        {
            Trace.WriteLine("FPatrol: UpdatedPatrol started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            if (this.Visible)
                updatePatrol();
            
            Trace.WriteLine("FPatrol: UpdatedPatrol ended.");
        }
        #endregion

        #region Update patrol content
        #region Updating
        private void updatePatrol()
        {
            Trace.WriteLine("FPatrol: updatePatrol started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            Thread updatePatrolThread =
                new Thread(new ThreadStart(updatePatrolWithThread));
            updatePatrolThread.Name = "FPatrol.updatePatrolThread";
            updatePatrolThread.IsBackground = true;
            updatePatrolThread.Start();

            Trace.WriteLine("FPatrol: updatePatrol ended.");
        }
        private readonly object bindingLock = new object();
        bool Updating = false;
        private void updatePatrolWithThread()
        {
            Trace.WriteLine("FPatrol: updatePatrolWithThread started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            Trace.WriteLine("FPatrol: updatePatrolWithThread locking " +
                "\"bindingLock\" on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");
            lock(bindingLock)
            {
                try
                {
                    Updating = true;
                    Trace.WriteLine("FPatrol: updatePatrolWithThread locked " +
                        "\"bindingLock\" on thread \"" +
                        Thread.CurrentThread.Name + "\" ( " +
                        Thread.CurrentThread.ManagedThreadId.ToString() + " )");

                    try
                    {
                        Patrol = CommonCode.GetPatrol(patrolid);
                    }
                    catch (CannotFindIdException)
                    {
                        if (InvokeRequired)
                            Invoke(SetVisibillity, new object[] { false });
                        else
                            Visible = false;

                        return;
                    }
                    catch (ApplicationException)
                    {
                        if (InvokeRequired)
                            Invoke(SetVisibillity, new object[] { false });
                        else
                            Visible = false;

                        return;
                    }
                    if (this.InvokeRequired)
                    {
                        this.Invoke(BindPatrolNumberAndClass);
                    }
                    else
                    {
                        this.bindPatrolNumberAndClass();
                    }

                    createDatasetPatrol();

                    populatePatrol(Patrol);
                    populateUnassigned(Patrol.PClass);
                    if (this.InvokeRequired)
                        this.Invoke(BindDatagrid);
                    else
                        this.bindDatagrid();

                    Trace.WriteLine("FPatrol: updatePatrolWithThread unlocking " +
                        "\"bindingLock\" on thread \"" +
                        Thread.CurrentThread.Name + "\" ( " +
                        Thread.CurrentThread.ManagedThreadId.ToString() + " )");
                }
                finally
                {
                    Updating = false;
                }
            }
            
            Trace.WriteLine("FPatrol: updatePatrolWithThread ended.");
        }
        private void createDatasetPatrol()
        {
            Trace.WriteLine("FPatrol: createDatasetPatrol started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            this.datasetPatrol = new DatasetPatrol();

            Trace.WriteLine("FPatrol: createDatasetPatrol ended.");
        }
        private void populatePatrol(Structs.Patrol thisPatrol)
        {
            Trace.WriteLine("FPatrol: populatePatrol started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            this.datasetPatrol.shooters.Clear();

            // Populate list with competitors from this patrol
            Structs.Competitor[] competitors =
                CommonCode.GetCompetitors(thisPatrol, "Lane");

            foreach(Structs.Competitor competitor in competitors)
            {
                DatasetPatrol.shootersRow patrolRow =
                    this.datasetPatrol.shooters.NewshootersRow();

                // Fetch data for row
                Structs.Shooter shooter =
                    CommonCode.GetShooter(competitor.ShooterId);
                Structs.Club club =
                    CommonCode.GetClub(shooter.ClubId);
                Structs.Weapon weapon =
                    CommonCode.GetWeapon(competitor.WeaponId);

                // Populate row
                patrolRow.Move = false;
                patrolRow.competitorId = competitor.CompetitorId;
                patrolRow.Name = shooter.Givenname + " " + shooter.Surname;
                patrolRow.Club = club.Name;
                patrolRow.Weapon = weapon.Manufacturer + "," +
                    weapon.Model + "," +
                    weapon.Caliber;
                patrolRow.Lane = competitor.Lane;
                patrolRow.WeaponClass = weapon.WClass.ToString().Substring(0,1);				

                this.datasetPatrol.shooters.AddshootersRow(patrolRow);
            }
            Trace.WriteLine("FPatrol: populatePatrol ended.");
        }
        private void populateUnassigned(Structs.PatrolClass thisClass)
        {
            Trace.WriteLine("FPatrol: populateCompetitorList started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            this.datasetPatrol.unassigned.Clear();

            // Populate list with competitors, not yet assigned to a patrol
            Structs.Competitor[] competitors =
                CommonCode.GetCompetitorsWithNoPatrol(thisClass);

            foreach(Structs.Competitor competitor in competitors)
            {
                DatasetPatrol.unassignedRow patrolRow =
                    this.datasetPatrol.unassigned.NewunassignedRow();

                // Fetch data for row
                Structs.Shooter shooter =
                    CommonCode.GetShooter(competitor.ShooterId);
                Structs.Club club =
                    CommonCode.GetClub(shooter.ClubId);
                Structs.Weapon weapon =
                    CommonCode.GetWeapon(competitor.WeaponId);

                // Populate row
                patrolRow.Move = false;
                patrolRow.competitorId = competitor.CompetitorId;
                patrolRow.Name = shooter.Givenname + " " + shooter.Surname;
                patrolRow.Club = club.Name;
                patrolRow.Weapon = weapon.Manufacturer + "," +
                    weapon.Model;
                patrolRow.WeaponClass = weapon.WClass.ToString();

                this.datasetPatrol.unassigned.AddunassignedRow(patrolRow);
            }

            Trace.WriteLine("FPatrol: populateCompetitorList ended.");
        }
        #endregion

        #region Binding
        private void bindPatrolNumberAndClass()
        {
            Trace.WriteLine("FPatrol: bindPatrolNumberAndClass started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            lblPatrol.Text = patrolid.ToString();
            lblPatrolClass.Text = Patrol.PClass.ToString();
            timePatrol.Value = Patrol.StartDateTimeDisplay;
            if (Patrol.StartDateTime == Patrol.StartDateTimeDisplay)
            {
                chkUseAutomaticTime.Checked = true;
                timePatrol.Enabled = false;
            }
            else
            {
                chkUseAutomaticTime.Checked = false;
                timePatrol.Enabled = true;
            }

            Trace.WriteLine("FPatrol: bindPatrolNumberAndClass.");
        }
        private void bindDatagrid()
        {
            Trace.WriteLine("FPatrol: bindDatagrid started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            // Binding
            this.dataGridAssigned.DataSource = 
                this.datasetPatrol.shooters;
            this.dataGridUnassigned.DataSource =
                this.datasetPatrol.unassigned;

            // Fixup

            Trace.WriteLine("FPatrol: bindDatagrid ended.");
        }
        #endregion

        #endregion

        #region Moving
        private void btnRemoveFromPatrol_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FPatrol: btnRemoveFromPatrol_Click started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId + " )");

            var competitorIds = new List<int>();
            foreach (DatasetPatrol.shootersRow row in this.datasetPatrol.shooters)
            {
                if (row.Move)
                {
                    competitorIds.Add(row.competitorId);
                }
            }

            int[] competitorIdInts = competitorIds.ToArray();
            foreach(int thisCompId in competitorIdInts)
            {
                Structs.Competitor comp = CommonCode.GetCompetitor(thisCompId);
                comp.PatrolId = -1;
                comp.Lane = -1;

                CommonCode.UpdateCompetitor(comp);
            }

            Trace.WriteLine("FPatrol: btnRemoveFromPatrol_Click ended.");
        }

        private void btnAddToPatrol_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FPatrol: btnAddToPatrol_Click started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            try
            {
                var competitorIds = new List<int>();
                foreach (DatasetPatrol.unassignedRow row in this.datasetPatrol.unassigned)
                {
                    if (row.Move)
                    {
                        competitorIds.Add(row.competitorId);
                    }
                }

                this.checkAddPerson();

                var competitorIdInts = competitorIds.ToArray();
                foreach (int thisCompId in competitorIdInts)
                {
                    Structs.Competitor comp = CommonCode.GetCompetitor(thisCompId);
                    comp.PatrolId = this.Patrol.PatrolId;
                    comp.Lane = -1;

                    this.CommonCode.UpdateCompetitor(comp);
                }
            }
            catch(PatrolAlreadyFullException)
            {
                MessageBox.Show("Patrullen är redan full. " + 
                    "Alla markerade skyttar kan inte läggas in.");

                Trace.WriteLine("FPatrol: btnAddToPatrol_Click PatrolAlreadyFullException");
            }
            catch(Exception exc)
            {
                MessageBox.Show("Ett fel uppstod:" + exc.ToString());
            }
            Trace.WriteLine("FPatrol: btnAddToPatrol_Click started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");
        }
        private void checkAddPerson()
        {
            Trace.WriteLine("FPatrol: checkAddPerson started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            // TODO Remove
            this.MaxCompetitorsInPatrol = 
                CommonCode.GetCompetitions()[0].PatrolSize;

            // Check there is room in patrol
            if (CommonCode.GetCompetitorsCountPatrol(Patrol) + 1 > 
                this.MaxCompetitorsInPatrol)
            {
                throw new PatrolAlreadyFullException(
                    Patrol.PatrolId);
            }

            Trace.WriteLine("FPatrol: checkAddPerson ended.");
        }
        private void btnMoveUp_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FPatrol: btnMoveUp_Click started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId + " )");

            var competitorIds = new List<int>();
            foreach (DatasetPatrol.shootersRow row in this.datasetPatrol.shooters)
            {
                if (row.Move)
                {
                    competitorIds.Add(row.competitorId);
                }
            }

            int[] competitorIdInts = competitorIds.ToArray();
            foreach(int thisCompId in competitorIdInts)
            {
                Structs.Competitor comp = CommonCode.GetCompetitor(thisCompId);
                
                try
                {
                    comp.Lane = CommonCode.PatrolGetNextLaneUp(this.patrolid, comp.Lane);
                    CommonCode.UpdateCompetitor(comp);
                }
                catch(ApplicationException)
                {
                    MessageBox.Show("Kan inte flytta skytt uppåt, det finns inte någon plats.");
                }
            }

            Trace.WriteLine("FPatrol: btnMoveUp_Click ended.");
        }
        private void btnMoveDown_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FPatrol: btnMoveDown_Click started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId + " )");

            var competitorIds = new List<int>();
            foreach (DatasetPatrol.shootersRow row in this.datasetPatrol.shooters)
            {
                if (row.Move)
                {
                    competitorIds.Add(row.competitorId);
                }
            }

            int[] competitorIdInts = competitorIds.ToArray();
            for(int i = competitorIdInts.Length-1; i >= 0; i--)
            {
                Structs.Competitor comp = CommonCode.GetCompetitor(competitorIdInts[i]);
                
                try
                {
                    comp.Lane = CommonCode.PatrolGetNextLane(this.patrolid, comp.Lane);
                    CommonCode.UpdateCompetitor(comp);
                }
                catch(ApplicationException)
                {
                    MessageBox.Show("Kan inte flytta skytt uppåt, det finns inte någon plats.");
                }
            }

            Trace.WriteLine("FPatrol: btnMoveDown_Click ended.");
        }
        #endregion

        #region Windowhandling
        private void FPatrol_Resize(object sender, EventArgs e)
        {
            Trace.WriteLine("FPatrol: FPatrol_Resize started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            // org width = 616
            // org width = 272
            // Fixup size
            int size = (this.Size.Width - (616 - 2*272))/2;

            Size newSize = new Size(size, this.dataGridAssigned.Size.Height);
            this.dataGridAssigned.Size = newSize;
            this.dataGridUnassigned.Size = newSize;

            // New place for right datagrid.
            Point newLoc = new Point(size + (312-272), this.dataGridUnassigned.Location.Y);
            this.dataGridUnassigned.Location = newLoc;

            // new place for buttons
            newLoc = new Point(size + (280-272), 56);
            this.btnRemoveFromPatrol.Location = newLoc;

            newLoc = new Point(size + (280-272), 80);
            this.btnAddToPatrol.Location = newLoc;

            newLoc = new Point(size + (280-272), 104);
            this.btnMoveUp.Location = newLoc;

            newLoc = new Point(size + (280-272), 128);
            this.btnMoveDown.Location = newLoc;

            Trace.WriteLine("FPatrol: FPatrol_Resize ended.");
        }
        #endregion

        private void chkUseAutomaticTime_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
                return;

            if (chkUseAutomaticTime.Checked)
            {
                if (Patrol.StartDateTime != timePatrol.Value)
                {
                    DialogResult res =
                        MessageBox.Show(
                            "Är du säker på att du vill normalisera " +
                            "starttiden för denna patrull till " +
                            Patrol.StartDateTime.ToShortTimeString() + "?", "Är du säker?",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1);
                    if (res == DialogResult.Yes)
                    {
                        Patrol.StartDateTimeDisplay = Patrol.StartDateTime;
                        timePatrol.Enabled = false;
                        timePatrol.Value = Patrol.StartDateTime;
                        CommonCode.UpdatePatrol(Patrol);
                    }
                    else
                    {
                        chkUseAutomaticTime.Checked = false;
                    }
                }
            }
            else
            {
                DialogResult res =
                    MessageBox.Show(
                    "Är du säker på att du vill avnormalisera " +
                    "starttiden för denna patrull från " +
                    Patrol.StartDateTime.ToShortTimeString() + "?", "Är du säker?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                if (res == DialogResult.Yes)
                {
                    timePatrol.Enabled = true;
                }
                else
                {
                    chkUseAutomaticTime.Checked = true;
                }
            }
        }

        private void timePatrol_ValueChanged(object sender, EventArgs e)
        {
            if (timePatrol.Enabled &&
                Patrol.StartDateTimeDisplay != timePatrol.Value)
            {
                Patrol.StartDateTimeDisplay = timePatrol.Value;
                CommonCode.UpdatePatrol(Patrol);
            }
        }

        private void chkBlockThisPatrolForAutomatic_CheckedChanged(object sender, EventArgs e)
        {
            if (Updating)
                return;

            Patrol.LockedForAutomatic = chkBlockThisPatrolForAutomatic.Checked;
            CommonCode.UpdatePatrol(Patrol);
        }
    }
}
