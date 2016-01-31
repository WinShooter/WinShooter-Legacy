#region copyright
/*
Copyright ©2007 John Allberg

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data;
using Allberg.Shooter.Common.DataSets;
using System.Net;

namespace ManageEmbeddedResources
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabClubs;
		private System.Windows.Forms.TabPage tabWeapons;
		private System.Windows.Forms.TabPage tabShooters;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem menuFileOpen;
		private System.Windows.Forms.MenuItem menuFileSave;
		private System.Windows.Forms.MenuItem menuFileExit;
		private System.Windows.Forms.DataGrid dataGridClubs;
		private System.Windows.Forms.DataGrid dataGridWeapons;
		private System.Windows.Forms.DataGrid dataGridShooters;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuImportClubsFromFile;
		private System.Windows.Forms.MenuItem menuVrfyBankgiro;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuImportInfoFromWinShooter;
		private System.Data.OleDb.OleDbCommand oleDbSelectCommand1;
		private System.Data.OleDb.OleDbCommand oleDbInsertCommand1;
		private System.Data.OleDb.OleDbCommand oleDbUpdateCommand1;
		private System.Data.OleDb.OleDbCommand oleDbDeleteCommand1;
		private System.Data.OleDb.OleDbConnection oleDbConnection1;
		private ManageEmbeddedResources.DataSetWinShooter dataSetWinShooter1;
		private System.Data.OleDb.OleDbCommand oleDbSelectCommand2;
		private System.Data.OleDb.OleDbCommand oleDbInsertCommand2;
		private System.Data.OleDb.OleDbCommand oleDbUpdateCommand2;
		private System.Data.OleDb.OleDbCommand oleDbDeleteCommand2;
		private System.Data.OleDb.OleDbDataAdapter DAShooters;
		private System.Data.OleDb.OleDbDataAdapter DAClubs;
		private System.Windows.Forms.MenuItem menuImportShootersFromFile;
		private MenuItem menuImportInfoFromWinShooterCaache;
		private IContainer components;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			ds.Locale = new System.Globalization.CultureInfo("sv");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabClubs = new System.Windows.Forms.TabPage();
			this.dataGridClubs = new System.Windows.Forms.DataGrid();
			this.tabWeapons = new System.Windows.Forms.TabPage();
			this.dataGridWeapons = new System.Windows.Forms.DataGrid();
			this.tabShooters = new System.Windows.Forms.TabPage();
			this.dataGridShooters = new System.Windows.Forms.DataGrid();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuFile = new System.Windows.Forms.MenuItem();
			this.menuFileOpen = new System.Windows.Forms.MenuItem();
			this.menuFileSave = new System.Windows.Forms.MenuItem();
			this.menuFileExit = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuImportClubsFromFile = new System.Windows.Forms.MenuItem();
			this.menuImportShootersFromFile = new System.Windows.Forms.MenuItem();
			this.menuImportInfoFromWinShooter = new System.Windows.Forms.MenuItem();
			this.menuImportInfoFromWinShooterCaache = new System.Windows.Forms.MenuItem();
			this.menuVrfyBankgiro = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.oleDbSelectCommand1 = new System.Data.OleDb.OleDbCommand();
			this.oleDbConnection1 = new System.Data.OleDb.OleDbConnection();
			this.oleDbInsertCommand1 = new System.Data.OleDb.OleDbCommand();
			this.oleDbUpdateCommand1 = new System.Data.OleDb.OleDbCommand();
			this.oleDbDeleteCommand1 = new System.Data.OleDb.OleDbCommand();
			this.DAShooters = new System.Data.OleDb.OleDbDataAdapter();
			this.dataSetWinShooter1 = new ManageEmbeddedResources.DataSetWinShooter();
			this.oleDbSelectCommand2 = new System.Data.OleDb.OleDbCommand();
			this.oleDbInsertCommand2 = new System.Data.OleDb.OleDbCommand();
			this.oleDbUpdateCommand2 = new System.Data.OleDb.OleDbCommand();
			this.oleDbDeleteCommand2 = new System.Data.OleDb.OleDbCommand();
			this.DAClubs = new System.Data.OleDb.OleDbDataAdapter();
			this.tabControl1.SuspendLayout();
			this.tabClubs.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridClubs)).BeginInit();
			this.tabWeapons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridWeapons)).BeginInit();
			this.tabShooters.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridShooters)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSetWinShooter1)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabClubs);
			this.tabControl1.Controls.Add(this.tabWeapons);
			this.tabControl1.Controls.Add(this.tabShooters);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(552, 328);
			this.tabControl1.TabIndex = 1;
			// 
			// tabClubs
			// 
			this.tabClubs.Controls.Add(this.dataGridClubs);
			this.tabClubs.Location = new System.Drawing.Point(4, 22);
			this.tabClubs.Name = "tabClubs";
			this.tabClubs.Size = new System.Drawing.Size(544, 302);
			this.tabClubs.TabIndex = 0;
			this.tabClubs.Text = "Clubs";
			// 
			// dataGridClubs
			// 
			this.dataGridClubs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridClubs.DataMember = "";
			this.dataGridClubs.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridClubs.Location = new System.Drawing.Point(0, 0);
			this.dataGridClubs.Name = "dataGridClubs";
			this.dataGridClubs.PreferredColumnWidth = 90;
			this.dataGridClubs.Size = new System.Drawing.Size(544, 296);
			this.dataGridClubs.TabIndex = 0;
			// 
			// tabWeapons
			// 
			this.tabWeapons.Controls.Add(this.dataGridWeapons);
			this.tabWeapons.Location = new System.Drawing.Point(4, 22);
			this.tabWeapons.Name = "tabWeapons";
			this.tabWeapons.Size = new System.Drawing.Size(544, 302);
			this.tabWeapons.TabIndex = 1;
			this.tabWeapons.Text = "Weapons";
			// 
			// dataGridWeapons
			// 
			this.dataGridWeapons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridWeapons.DataMember = "";
			this.dataGridWeapons.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridWeapons.Location = new System.Drawing.Point(0, 0);
			this.dataGridWeapons.Name = "dataGridWeapons";
			this.dataGridWeapons.Size = new System.Drawing.Size(544, 296);
			this.dataGridWeapons.TabIndex = 0;
			// 
			// tabShooters
			// 
			this.tabShooters.Controls.Add(this.dataGridShooters);
			this.tabShooters.Location = new System.Drawing.Point(4, 22);
			this.tabShooters.Name = "tabShooters";
			this.tabShooters.Size = new System.Drawing.Size(544, 302);
			this.tabShooters.TabIndex = 2;
			this.tabShooters.Text = "Shooters";
			// 
			// dataGridShooters
			// 
			this.dataGridShooters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridShooters.DataMember = "";
			this.dataGridShooters.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridShooters.Location = new System.Drawing.Point(0, 0);
			this.dataGridShooters.Name = "dataGridShooters";
			this.dataGridShooters.Size = new System.Drawing.Size(544, 296);
			this.dataGridShooters.TabIndex = 0;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuItem1,
            this.menuVrfyBankgiro,
            this.menuItem2});
			// 
			// menuFile
			// 
			this.menuFile.Index = 0;
			this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFileOpen,
            this.menuFileSave,
            this.menuFileExit});
			this.menuFile.Text = "Arkiv";
			// 
			// menuFileOpen
			// 
			this.menuFileOpen.Index = 0;
			this.menuFileOpen.Text = "Öppna";
			this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
			// 
			// menuFileSave
			// 
			this.menuFileSave.Index = 1;
			this.menuFileSave.Text = "Spara";
			this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
			// 
			// menuFileExit
			// 
			this.menuFileExit.Index = 2;
			this.menuFileExit.Text = "Avsluta";
			this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuImportClubsFromFile,
            this.menuImportShootersFromFile,
            this.menuImportInfoFromWinShooter,
            this.menuImportInfoFromWinShooterCaache});
			this.menuItem1.Text = "Importera";
			// 
			// menuImportClubsFromFile
			// 
			this.menuImportClubsFromFile.Index = 0;
			this.menuImportClubsFromFile.Text = "Klubbar från fil";
			this.menuImportClubsFromFile.Click += new System.EventHandler(this.menuImportClubsFromFile_Click);
			// 
			// menuImportShootersFromFile
			// 
			this.menuImportShootersFromFile.Index = 1;
			this.menuImportShootersFromFile.Text = "Skyttar från fil";
			this.menuImportShootersFromFile.Click += new System.EventHandler(this.menuImportShootersFromFile_Click);
			// 
			// menuImportInfoFromWinShooter
			// 
			this.menuImportInfoFromWinShooter.Index = 2;
			this.menuImportInfoFromWinShooter.Text = "Info från WinShooter";
			this.menuImportInfoFromWinShooter.Click += new System.EventHandler(this.menuImportInfoFromWinShooter_Click);
			// 
			// menuImportInfoFromWinShooterCaache
			// 
			this.menuImportInfoFromWinShooterCaache.Index = 3;
			this.menuImportInfoFromWinShooterCaache.Text = "Cache från WinShooter";
			this.menuImportInfoFromWinShooterCaache.Click += new System.EventHandler(this.menuImportInfoFromWinShooterCaache_Click);
			// 
			// menuVrfyBankgiro
			// 
			this.menuVrfyBankgiro.Index = 2;
			this.menuVrfyBankgiro.Text = "Verifiera PFBG";
			this.menuVrfyBankgiro.Click += new System.EventHandler(this.menuVrfyBankgiro_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 3;
			this.menuItem2.Text = "Temp";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// oleDbSelectCommand1
			// 
			this.oleDbSelectCommand1.CommandText = "SELECT Arrived, Automatic, Cardnr, Class, ClubId, Email, EmailResult, Givenname, " +
				"Payed, ShooterId, Surname, ToAutomatic FROM Shooters";
			this.oleDbSelectCommand1.Connection = this.oleDbConnection1;
			// 
			// oleDbConnection1
			// 
			this.oleDbConnection1.ConnectionString = resources.GetString("oleDbConnection1.ConnectionString");
			// 
			// oleDbInsertCommand1
			// 
			this.oleDbInsertCommand1.CommandText = "INSERT INTO Shooters(Arrived, Automatic, Cardnr, Class, ClubId, Email, EmailResul" +
				"t, Givenname, Payed, Surname, ToAutomatic) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?," +
				" ?)";
			this.oleDbInsertCommand1.Connection = this.oleDbConnection1;
			this.oleDbInsertCommand1.Parameters.AddRange(new System.Data.OleDb.OleDbParameter[] {
            new System.Data.OleDb.OleDbParameter("Arrived", System.Data.OleDb.OleDbType.Boolean, 2, "Arrived"),
            new System.Data.OleDb.OleDbParameter("Automatic", System.Data.OleDb.OleDbType.Boolean, 2, "Automatic"),
            new System.Data.OleDb.OleDbParameter("Cardnr", System.Data.OleDb.OleDbType.VarWChar, 150, "Cardnr"),
            new System.Data.OleDb.OleDbParameter("Class", System.Data.OleDb.OleDbType.Integer, 0, "Class"),
            new System.Data.OleDb.OleDbParameter("ClubId", System.Data.OleDb.OleDbType.VarWChar, 150, "ClubId"),
            new System.Data.OleDb.OleDbParameter("Email", System.Data.OleDb.OleDbType.VarWChar, 150, "Email"),
            new System.Data.OleDb.OleDbParameter("EmailResult", System.Data.OleDb.OleDbType.Boolean, 2, "EmailResult"),
            new System.Data.OleDb.OleDbParameter("Givenname", System.Data.OleDb.OleDbType.VarWChar, 150, "Givenname"),
            new System.Data.OleDb.OleDbParameter("Payed", System.Data.OleDb.OleDbType.Integer, 0, "Payed"),
            new System.Data.OleDb.OleDbParameter("Surname", System.Data.OleDb.OleDbType.VarWChar, 150, "Surname"),
            new System.Data.OleDb.OleDbParameter("ToAutomatic", System.Data.OleDb.OleDbType.Boolean, 2, "ToAutomatic")});
			// 
			// oleDbUpdateCommand1
			// 
			this.oleDbUpdateCommand1.CommandText = resources.GetString("oleDbUpdateCommand1.CommandText");
			this.oleDbUpdateCommand1.Connection = this.oleDbConnection1;
			this.oleDbUpdateCommand1.Parameters.AddRange(new System.Data.OleDb.OleDbParameter[] {
            new System.Data.OleDb.OleDbParameter("Arrived", System.Data.OleDb.OleDbType.Boolean, 2, "Arrived"),
            new System.Data.OleDb.OleDbParameter("Automatic", System.Data.OleDb.OleDbType.Boolean, 2, "Automatic"),
            new System.Data.OleDb.OleDbParameter("Cardnr", System.Data.OleDb.OleDbType.VarWChar, 150, "Cardnr"),
            new System.Data.OleDb.OleDbParameter("Class", System.Data.OleDb.OleDbType.Integer, 0, "Class"),
            new System.Data.OleDb.OleDbParameter("ClubId", System.Data.OleDb.OleDbType.VarWChar, 150, "ClubId"),
            new System.Data.OleDb.OleDbParameter("Email", System.Data.OleDb.OleDbType.VarWChar, 150, "Email"),
            new System.Data.OleDb.OleDbParameter("EmailResult", System.Data.OleDb.OleDbType.Boolean, 2, "EmailResult"),
            new System.Data.OleDb.OleDbParameter("Givenname", System.Data.OleDb.OleDbType.VarWChar, 150, "Givenname"),
            new System.Data.OleDb.OleDbParameter("Payed", System.Data.OleDb.OleDbType.Integer, 0, "Payed"),
            new System.Data.OleDb.OleDbParameter("Surname", System.Data.OleDb.OleDbType.VarWChar, 150, "Surname"),
            new System.Data.OleDb.OleDbParameter("ToAutomatic", System.Data.OleDb.OleDbType.Boolean, 2, "ToAutomatic"),
            new System.Data.OleDb.OleDbParameter("Original_ShooterId", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ShooterId", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Arrived", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Arrived", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Automatic", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Automatic", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Cardnr", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Cardnr", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Cardnr1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Cardnr", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Class", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Class", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Class1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Class", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_ClubId", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_ClubId1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Email", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Email", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Email1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Email", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_EmailResult", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "EmailResult", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Givenname", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Givenname", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Givenname1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Givenname", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Payed", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Payed", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Payed1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Payed", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Surname", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Surname", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Surname1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Surname", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_ToAutomatic", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ToAutomatic", System.Data.DataRowVersion.Original, null)});
			// 
			// oleDbDeleteCommand1
			// 
			this.oleDbDeleteCommand1.CommandText = resources.GetString("oleDbDeleteCommand1.CommandText");
			this.oleDbDeleteCommand1.Connection = this.oleDbConnection1;
			this.oleDbDeleteCommand1.Parameters.AddRange(new System.Data.OleDb.OleDbParameter[] {
            new System.Data.OleDb.OleDbParameter("Original_ShooterId", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ShooterId", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Arrived", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Arrived", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Automatic", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Automatic", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Cardnr", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Cardnr", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Cardnr1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Cardnr", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Class", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Class", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Class1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Class", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_ClubId", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_ClubId1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Email", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Email", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Email1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Email", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_EmailResult", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "EmailResult", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Givenname", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Givenname", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Givenname1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Givenname", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Payed", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Payed", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Payed1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Payed", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Surname", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Surname", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Surname1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Surname", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_ToAutomatic", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ToAutomatic", System.Data.DataRowVersion.Original, null)});
			// 
			// DAShooters
			// 
			this.DAShooters.DeleteCommand = this.oleDbDeleteCommand1;
			this.DAShooters.InsertCommand = this.oleDbInsertCommand1;
			this.DAShooters.SelectCommand = this.oleDbSelectCommand1;
			this.DAShooters.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "Shooters", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("Arrived", "Arrived"),
                        new System.Data.Common.DataColumnMapping("Automatic", "Automatic"),
                        new System.Data.Common.DataColumnMapping("Cardnr", "Cardnr"),
                        new System.Data.Common.DataColumnMapping("Class", "Class"),
                        new System.Data.Common.DataColumnMapping("ClubId", "ClubId"),
                        new System.Data.Common.DataColumnMapping("Email", "Email"),
                        new System.Data.Common.DataColumnMapping("EmailResult", "EmailResult"),
                        new System.Data.Common.DataColumnMapping("Givenname", "Givenname"),
                        new System.Data.Common.DataColumnMapping("Payed", "Payed"),
                        new System.Data.Common.DataColumnMapping("ShooterId", "ShooterId"),
                        new System.Data.Common.DataColumnMapping("Surname", "Surname"),
                        new System.Data.Common.DataColumnMapping("ToAutomatic", "ToAutomatic")})});
			this.DAShooters.UpdateCommand = this.oleDbUpdateCommand1;
			// 
			// dataSetWinShooter1
			// 
			this.dataSetWinShooter1.DataSetName = "DataSetWinShooter";
			this.dataSetWinShooter1.Locale = new System.Globalization.CultureInfo("sv-SE");
			this.dataSetWinShooter1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// oleDbSelectCommand2
			// 
			this.oleDbSelectCommand2.CommandText = "SELECT Automatic, Bankgiro, ClubId, Country, Email, Name, Plusgiro, ToAutomatic F" +
				"ROM Clubs";
			this.oleDbSelectCommand2.Connection = this.oleDbConnection1;
			// 
			// oleDbInsertCommand2
			// 
			this.oleDbInsertCommand2.CommandText = "INSERT INTO Clubs(Automatic, Bankgiro, ClubId, Country, Email, Name, Plusgiro, To" +
				"Automatic) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
			this.oleDbInsertCommand2.Connection = this.oleDbConnection1;
			this.oleDbInsertCommand2.Parameters.AddRange(new System.Data.OleDb.OleDbParameter[] {
            new System.Data.OleDb.OleDbParameter("Automatic", System.Data.OleDb.OleDbType.Boolean, 2, "Automatic"),
            new System.Data.OleDb.OleDbParameter("Bankgiro", System.Data.OleDb.OleDbType.VarWChar, 150, "Bankgiro"),
            new System.Data.OleDb.OleDbParameter("ClubId", System.Data.OleDb.OleDbType.VarWChar, 150, "ClubId"),
            new System.Data.OleDb.OleDbParameter("Country", System.Data.OleDb.OleDbType.VarWChar, 150, "Country"),
            new System.Data.OleDb.OleDbParameter("Email", System.Data.OleDb.OleDbType.VarWChar, 150, "Email"),
            new System.Data.OleDb.OleDbParameter("Name", System.Data.OleDb.OleDbType.VarWChar, 150, "Name"),
            new System.Data.OleDb.OleDbParameter("Plusgiro", System.Data.OleDb.OleDbType.VarWChar, 150, "Plusgiro"),
            new System.Data.OleDb.OleDbParameter("ToAutomatic", System.Data.OleDb.OleDbType.Boolean, 2, "ToAutomatic")});
			// 
			// oleDbUpdateCommand2
			// 
			this.oleDbUpdateCommand2.CommandText = resources.GetString("oleDbUpdateCommand2.CommandText");
			this.oleDbUpdateCommand2.Connection = this.oleDbConnection1;
			this.oleDbUpdateCommand2.Parameters.AddRange(new System.Data.OleDb.OleDbParameter[] {
            new System.Data.OleDb.OleDbParameter("Automatic", System.Data.OleDb.OleDbType.Boolean, 2, "Automatic"),
            new System.Data.OleDb.OleDbParameter("Bankgiro", System.Data.OleDb.OleDbType.VarWChar, 150, "Bankgiro"),
            new System.Data.OleDb.OleDbParameter("ClubId", System.Data.OleDb.OleDbType.VarWChar, 150, "ClubId"),
            new System.Data.OleDb.OleDbParameter("Country", System.Data.OleDb.OleDbType.VarWChar, 150, "Country"),
            new System.Data.OleDb.OleDbParameter("Email", System.Data.OleDb.OleDbType.VarWChar, 150, "Email"),
            new System.Data.OleDb.OleDbParameter("Name", System.Data.OleDb.OleDbType.VarWChar, 150, "Name"),
            new System.Data.OleDb.OleDbParameter("Plusgiro", System.Data.OleDb.OleDbType.VarWChar, 150, "Plusgiro"),
            new System.Data.OleDb.OleDbParameter("ToAutomatic", System.Data.OleDb.OleDbType.Boolean, 2, "ToAutomatic"),
            new System.Data.OleDb.OleDbParameter("Original_ClubId", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Automatic", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Automatic", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Bankgiro", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Bankgiro", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Bankgiro1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Bankgiro", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Country", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Country", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Country1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Country", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Email", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Email", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Email1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Email", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Name", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Name1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Plusgiro", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Plusgiro", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Plusgiro1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Plusgiro", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_ToAutomatic", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ToAutomatic", System.Data.DataRowVersion.Original, null)});
			// 
			// oleDbDeleteCommand2
			// 
			this.oleDbDeleteCommand2.CommandText = resources.GetString("oleDbDeleteCommand2.CommandText");
			this.oleDbDeleteCommand2.Connection = this.oleDbConnection1;
			this.oleDbDeleteCommand2.Parameters.AddRange(new System.Data.OleDb.OleDbParameter[] {
            new System.Data.OleDb.OleDbParameter("Original_ClubId", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Automatic", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Automatic", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Bankgiro", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Bankgiro", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Bankgiro1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Bankgiro", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Country", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Country", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Country1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Country", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Email", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Email", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Email1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Email", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Name", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Name1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Name", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Plusgiro", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Plusgiro", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_Plusgiro1", System.Data.OleDb.OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "Plusgiro", System.Data.DataRowVersion.Original, null),
            new System.Data.OleDb.OleDbParameter("Original_ToAutomatic", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "ToAutomatic", System.Data.DataRowVersion.Original, null)});
			// 
			// DAClubs
			// 
			this.DAClubs.DeleteCommand = this.oleDbDeleteCommand2;
			this.DAClubs.InsertCommand = this.oleDbInsertCommand2;
			this.DAClubs.SelectCommand = this.oleDbSelectCommand2;
			this.DAClubs.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "Clubs", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("Automatic", "Automatic"),
                        new System.Data.Common.DataColumnMapping("Bankgiro", "Bankgiro"),
                        new System.Data.Common.DataColumnMapping("ClubId", "ClubId"),
                        new System.Data.Common.DataColumnMapping("Country", "Country"),
                        new System.Data.Common.DataColumnMapping("Email", "Email"),
                        new System.Data.Common.DataColumnMapping("Name", "Name"),
                        new System.Data.Common.DataColumnMapping("Plusgiro", "Plusgiro"),
                        new System.Data.Common.DataColumnMapping("ToAutomatic", "ToAutomatic")})});
			this.DAClubs.UpdateCommand = this.oleDbUpdateCommand2;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(552, 326);
			this.Controls.Add(this.tabControl1);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "Manage Embedded Resources";
			this.tabControl1.ResumeLayout(false);
			this.tabClubs.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridClubs)).EndInit();
			this.tabWeapons.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridWeapons)).EndInit();
			this.tabShooters.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridShooters)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSetWinShooter1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		DSStartupResources ds = new DSStartupResources();
		string orgFilename = "";
		private void menuFileOpen_Click(object sender, System.EventArgs e)
		{
			this.openFileDialog1.Filter = "*.xml|*.xml";
			DialogResult res =
				this.openFileDialog1.ShowDialog();

			if (res == DialogResult.Cancel)
				return;

			orgFilename = this.openFileDialog1.FileName;

			ds.ReadXml(this.openFileDialog1.FileName);

            DataTable table = (DataTable)ds.Clubs;
            checkLastUpdate(ref table);

            table = (DataTable)ds.Shooters;
            checkLastUpdate(ref table);

            table = (DataTable)ds.Weapons;
            checkLastUpdate(ref table);

            this.dataGridClubs.DataSource = ds;
			this.dataGridClubs.DataMember = "Clubs";

			this.dataGridShooters.DataSource = ds;
			this.dataGridShooters.DataMember= "Shooters";

			this.dataGridWeapons.DataSource = ds;
			this.dataGridWeapons.DataMember = "Weapons";
		}

        void checkLastUpdate(ref DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row.IsNull("LastUpdate"))
                {
                    row["LastUpdate"] = DateTime.Parse("2004-01-01");
                }
            }
        }

		private void menuFileSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				ds.WriteXml(this.orgFilename);		
			}
			catch(Exception exc)
			{
				MessageBox.Show(exc.ToString());
			}
		}

		private void menuFileExit_Click(object sender, System.EventArgs e)
		{
			this.Dispose(true);
		}

		private void menuImportClubsFromFile_Click(object sender, System.EventArgs e)
		{
			this.tabClubs.Focus();

			this.openFileDialog1.Filter = "*.*|*.*";
			this.openFileDialog1.ShowDialog();
			System.IO.Stream stream = this.openFileDialog1.OpenFile();
			System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding(1252));

			string file = reader.ReadToEnd();
			reader.Close();
			stream.Close();

			file = file.Replace("\r", "").Replace("\t", "");
			string[] lines = file.Split('\n');

			foreach (string line in lines)
			{
				if (line.Trim().Length>5)
				{
					int nextPart = 0;
					string[] parts = line.Split(';');
					string clubId1 = parts[nextPart].Trim();
					nextPart++;
					while (clubId1.Length<2)
						clubId1 = "0" + clubId1;
					string clubId2 = parts[nextPart].Trim();
					nextPart++;
					while (clubId2.Length<3)
						clubId2 = "0" + clubId2;
					string clubId = clubId1 + "-" + clubId2;
					string clubName = parts[nextPart].Trim();
					nextPart++;
					string email = parts[nextPart].Trim();
					nextPart++;
					if (email.StartsWith("\""))
					{
						while(!email.EndsWith("\""))
						{
							email += ";" + parts[nextPart];
							nextPart++;
						}
						email = email.Replace("\"", "");
					}
					string plusgiro = parts[nextPart].Trim();

					if (clubId != "" & clubName != "")
					{
						System.Data.DataRow[] rows = ds.Clubs.Select("ClubId='" + clubId + "'");
						if (rows.Length>0)
						{
							// Club exists
							DSStartupResources.ClubsRow row = 
								(DSStartupResources.ClubsRow)rows[0];
							//row.ClubId = clubId;
							row.Name = clubName;
							row.Email = email;
							row.Plusgiro = plusgiro;
							row.Bankgiro = "";
						}
						else
						{
							// New Club
							DSStartupResources.ClubsRow row = ds.Clubs.NewClubsRow();
							row.ClubId = clubId;
							row.Country = "SE";
							row.Name = clubName;
							row.Email = email;
							row.Plusgiro = plusgiro;
							row.Bankgiro = "";
							ds.Clubs.AddClubsRow(row);
						}
					}
				}
			}
		}

		private void menuVrfyBankgiro_Click(object sender, System.EventArgs e)
		{
			foreach(DSStartupResources.ClubsRow row in this.ds.Clubs)
			{
				if (!row.IsBankgiroNull() && row.Bankgiro != "")
				{
					string bankgiro = row.Plusgiro.Replace(" ", "").Replace("-", "").Replace("?", "");
					if (!checkNrIsOk(row.Bankgiro))
					{
						System.Diagnostics.Trace.WriteLine("Club " + row.ClubId + " has not a correct Bankgiro.");
					}
					else
					{
						try
						{
							row.Bankgiro = fixupBankgiro(bankgiro);
						}
						catch(Exception)
						{
							row.Bankgiro = "?Length?" + bankgiro;
						}
					}
				}
				if (!row.IsPlusgiroNull() && row.Plusgiro != "")
				{
					string plusgiro = row.Plusgiro.Replace(" ", "").Replace("-", "").Replace("?", "");
					if (!checkNrIsOk(plusgiro))
					{
						Trace.WriteLine("Club " + row.ClubId + " has not a correct Plusgiro.");
						row.Plusgiro = "?Check?" + plusgiro;
					}
					else
					{
						try
						{
							row.Plusgiro = fixupPlusgiro(plusgiro);
						}
						catch(Exception)
						{
							row.Plusgiro = "?Length?" + plusgiro;
						}
					}
				}
			}
		}

		private bool checkNrIsOk(string nrToCheck)
		{
			string nr = nrToCheck.Replace(" ", "").Replace("-", "");

			System.Text.StringBuilder sumString = new System.Text.StringBuilder();
			int multiplier = 2-nr.Length%2;
			for(int i=0;i<nr.Length-1;i++)
			{
				int j = int.Parse(nr.Substring(i,1));

				j = multiplier * j;

				if (multiplier == 1)
					multiplier = 2;
				else
					multiplier = 1;
				sumString.Append(j.ToString());
			}

			int totalSum = 0;
			foreach(char thisChar in sumString.ToString())
			{
				totalSum += int.Parse(thisChar.ToString());
			}

			int checkNr = -1;
			for(int i=10;i<totalSum+10;i=i+10)
			{
				checkNr = i-totalSum;
			}

			if (checkNr != int.Parse(nr.Substring(nr.Length-1, 1)))
			{
				return false;
			}
			else
			{
				return true;
			}
		}


		private string fixupPlusgiro(string plusgiroOrg)
		{
			string plusgiroNew;
			switch (plusgiroOrg.Length)
			{
				case 2:
					plusgiroNew = plusgiroOrg.Substring(0,1) + 
						"-" +
						plusgiroOrg.Substring(1,1);
					return plusgiroNew;
				case 3:
					plusgiroNew = plusgiroOrg.Substring(0,2) + 
						"-" +
						plusgiroOrg.Substring(2,1);
					return plusgiroNew;
				case 4:
					plusgiroNew = plusgiroOrg.Substring(0,3) + 
						"-" +
						plusgiroOrg.Substring(3,1);
					return plusgiroNew;
				case 5:
					plusgiroNew = plusgiroOrg.Substring(2,2) + 
						"-" +
						plusgiroOrg.Substring(4,1);
					return plusgiroNew;
				case 6:
					plusgiroNew = plusgiroOrg.Substring(0,2) + " " +
						plusgiroOrg.Substring(2,3) + "-" +
						plusgiroOrg.Substring(5,1);
					return plusgiroNew;
				case 7:
					plusgiroNew = plusgiroOrg.Substring(0,2) + " " +
						plusgiroOrg.Substring(2,2) + " " +
						plusgiroOrg.Substring(4,2) + "-" +
						plusgiroOrg.Substring(6,1);
					return plusgiroNew;
				case 8:
					plusgiroNew = plusgiroOrg.Substring(0,3) + " " +
						plusgiroOrg.Substring(3,2) + " " +
						plusgiroOrg.Substring(5,2) + "-" +
						plusgiroOrg.Substring(7,1);
					return plusgiroNew;

				default:
					throw new ApplicationException("Unknown plusgiro-längd: " + plusgiroOrg.Length.ToString());
			}
		}

		private string fixupBankgiro(string bankgiroOrg)
		{
			string bankgiroNew;
			switch (bankgiroOrg.Length)
			{
				case 7:
					bankgiroNew = bankgiroOrg.Substring(0,3) + "-" +
						bankgiroOrg.Substring(3,4);
					return bankgiroNew;
				case 8:
					bankgiroNew = bankgiroOrg.Substring(0,3) + "-" +
						bankgiroOrg.Substring(3,4);
					return bankgiroNew;

				default:
					throw new ApplicationException("Unknown plusgiro-längd: " + bankgiroOrg.Length.ToString());
			}
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			checkBankgiroIsValidWebb("5002-4553");
			return;
			foreach(DSStartupResources.ClubsRow row in ds.Clubs)
			{
				if (!row.IsBankgiroNull() &&
					row.Bankgiro != "")
				{
					if (!checkBankgiroIsValidWebb(row.Bankgiro))
						row.Bankgiro = "";
				}
			}
		}

		private bool checkBankgiroIsValidWebb(string bankgiro)
		{
			try
			{
				string toPostString = "Dynamicframeworkselector1:BGCFramework:Content2:BGNrSearch1:txtBankgironummer=" + bankgiro + 
					"&Dynamicframeworkselector1:BGCFramework:Content2:BGNrSearch1:txtOrganisationsnummer=" +
					"&Dynamicframeworkselector1:BGCFramework:Content2:BGNrSearch1:txtNamn=" +
					"&Dynamicframeworkselector1:BGCFramework:Content2:BGNrSearch1:txtOrt=";

				HttpWebRequest wreq = 
					(HttpWebRequest)WebRequest.Create("http://www.bankgirot.se/templates/BGNrSearchPage.aspx?id=3209");
				// Turn off connection keep-alives.
				wreq.KeepAlive = false;
				wreq.Method = "POST";
				wreq.ContentLength = toPostString.Length;
				System.IO.Stream reqStream = wreq.GetRequestStream();
				byte[] toPost = System.Text.Encoding.ASCII.GetBytes(toPostString);
				reqStream.Write(toPost, 0, toPost.Length);


				HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();

				System.IO.Stream respStream = wresp.GetResponseStream();

				byte[] responseBytes = new byte[wresp.ContentLength];
				respStream.Read(responseBytes, 0, (int)wresp.ContentLength);
				string returned = System.Text.Encoding.ASCII.GetString(responseBytes);
				
				wresp.Close();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				return true;
			}
			return true;
		}

		#region Import shooters from file
		private void menuImportShootersFromFile_Click(object sender, System.EventArgs e)
		{
			this.tabShooters.Focus();

			this.openFileDialog1.Filter = "*.*|*.*";
			this.openFileDialog1.ShowDialog();
			System.IO.Stream stream = this.openFileDialog1.OpenFile();
			System.IO.StreamReader reader = 
				new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding(1252));

			string file = reader.ReadToEnd();
			reader.Close();
			stream.Close();

			file = file.Replace("\r", "").Replace("\t", "");
			string[] lines = file.Split('\n');

			foreach (string line in lines)
			{
				try
				{
					if (line.Trim().Length>5)
					{
						int nextPart = 0;
						string[] parts = line.Split(';');
						string cardnr = parts[nextPart].Trim();
						nextPart++;

						string firstname = parts[nextPart].Trim();
						nextPart++;

						string lastname = parts[nextPart].Trim();
						nextPart++;

						string sclass = parts[nextPart]
							.Replace("J", "")
							.Replace("VY", "")
							.Replace("VÄ", "")
							.Replace("F", "")
							.Replace("D", "")
							.Replace("+", "")
							.Trim();
						if (sclass.Length > 0)
							int.Parse(sclass);
						nextPart++;
						nextPart++;

						string clubid = parts[nextPart].Trim();
						nextPart++;


						if (cardnr != "" & 
							clubid != "" &
							sclass != "" &
							firstname != "" &
							lastname != "")
						{
							DSStartupResources.ShootersRow[] rows = 
								(DSStartupResources.ShootersRow[])
								ds.Shooters.Select("CardNr='" + cardnr + "'");
							if (rows.Length>0)
							{	
								Trace.WriteLine("Shooter already exist. Updating " + cardnr);
								// shooter exists
								DSStartupResources.ShootersRow row = 
									rows[0];

								row.Class = int.Parse(sclass);
								row.Email = "";
								row.ClubId = clubid;
								row.Surname = firstname;
								row.Givenname = lastname;
							}
							else
							{	
								Trace.WriteLine("Shooter doesn't exist. Adding " + cardnr);
								// New shooter
								DSStartupResources.ShootersRow row = 
									ds.Shooters.NewShootersRow();
								row.Cardnr = cardnr;
								row.Class = int.Parse(sclass);
								row.Email = "";
								row.ClubId = clubid;
								row.Givenname = firstname;
								row.Surname = lastname;
								ds.Shooters.AddShootersRow(row);
							}
						}
					}
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.ToString());
				}
			}
		}
		#endregion

		#region Import info from Winshooter
		private void menuImportInfoFromWinShooter_Click(object sender, System.EventArgs e)
		{
			this.openFileDialog1.Filter = "*.wsk|*.wsk";
			DialogResult res = this.openFileDialog1.ShowDialog();
			if (res != DialogResult.OK)
				return;

			string filename = this.openFileDialog1.FileName;

			try
			{
				this.oleDbConnection1.ConnectionString = "Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Registry Path=;Jet OLEDB:Database Locking Mode=1;Data Source=\"" + filename + "\";Jet OLEDB:Engine Type=5;Provider=\"Microsoft.Jet.OLEDB.4.0\";Jet OLEDB:System database=;Jet OLEDB:SFP=False;persist security info=False;Extended Properties=;Mode=Share Deny None;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Create System Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;User ID=Admin;Jet OLEDB:Global Bulk Transactions=1";
				dataSetWinShooter1 = new DataSetWinShooter();
				this.DAClubs.Fill(dataSetWinShooter1, "Clubs");
				this.DAShooters.Fill(dataSetWinShooter1, "Shooters");
			}
			catch(Exception exc)
			{
				MessageBox.Show(exc.ToString());
				return;
			}

			foreach(DataSetWinShooter.ClubsRow clubRow in this.dataSetWinShooter1.Clubs)
			{
				checkToUpdate(clubRow);
			}

			foreach(DataSetWinShooter.ShootersRow shooterRow in this.dataSetWinShooter1.Shooters)
			{
				if (int.Parse(shooterRow.Cardnr) > 0)
					checkToUpdate(shooterRow);
			}
			MessageBox.Show("Done!");
		}

		#region Clubs
		private void checkToUpdate(DataSetWinShooter.ClubsRow clubRow)
		{
			DSLocalCache temp = new DSLocalCache();
			DSLocalCache.ClubsRow newClubsRow = temp.Clubs.NewClubsRow();

			newClubsRow.Bankgiro = clubRow.Bankgiro;
			newClubsRow.ClubId = clubRow.ClubId;
			newClubsRow.Country = clubRow.Country;
			newClubsRow.Email = clubRow.Email;
			newClubsRow.Name = clubRow.Name;
			newClubsRow.Plusgiro = clubRow.Plusgiro;
			newClubsRow.LastUpdate = DateTime.Parse("2004-01-01");

			checkToUpdate(newClubsRow);
		}
		private void checkToUpdate(DSLocalCache.ClubsRow clubRow)
		{
			if (clubRow.IsBankgiroNull())
				clubRow.Bankgiro = "";
			if (clubRow.IsPlusgiroNull())
				clubRow.Plusgiro = "";
			if (clubRow.IsEmailNull())
				clubRow.Email = "";
			try
			{
				DSStartupResources.ClubsRow current = getClub(clubRow.ClubId);

				// Since we got here, the club already exist
				if ((current.Bankgiro != clubRow.Bankgiro & clubRow.Bankgiro!="")|
					current.Country != clubRow.Country |
					(current.Email != clubRow.Email & clubRow.Email != "") |
					current.Name != clubRow.Name |
					(current.Plusgiro != clubRow.Plusgiro & clubRow.Plusgiro != ""))
				{
					//do update
					string toView = "Do you want to update club " + current.Name;

					if (current.Bankgiro != clubRow.Bankgiro &&
                        clubRow.Bankgiro != "")
						toView += "\r\nBankgiro: " + current.Bankgiro + " < " + clubRow.Bankgiro;

					if (current.Country != clubRow.Country)
						toView += "\r\nCountry: " + current.Country + " < " + clubRow.Country;

					if (current.Email != clubRow.Email &&
                        clubRow.Email != "")
						toView += "\r\nEmail: \"" + current.Email + "\" < \"" + clubRow.Email + "\"";

					if (current.Name != clubRow.Name)
						toView += "\r\nName: " + current.Name + " < " + clubRow.Name;

					if (current.Plusgiro != clubRow.Plusgiro &&
                        clubRow.Plusgiro != "")
						toView += "\r\nPlusgiro: " + current.Plusgiro + " < " + clubRow.Plusgiro;

					toView += "\r\nLastUpdate: " + current.LastUpdate + " < " + clubRow.LastUpdate;

					DialogResult res = MessageBox.Show(toView, 
						"Update", MessageBoxButtons.YesNo);
					if (res == DialogResult.Yes)
					{
						current.Bankgiro = clubRow.Bankgiro;
						current.Country = clubRow.Country;
						current.Email = clubRow.Email;
						current.Name = clubRow.Name;
						current.LastUpdate = clubRow.LastUpdate;
						current.Plusgiro = clubRow.Plusgiro;
					}
				}
			}
			catch(ApplicationException)
			{
				DialogResult res = MessageBox.Show("Do you want to add club" +
					clubRow.ClubId + " (" + clubRow.Name  + ")", "Add", MessageBoxButtons.YesNo);

				if (res == DialogResult.Yes)
				{
					DSStartupResources.ClubsRow newRow = this.ds.Clubs.NewClubsRow();
					newRow.Bankgiro = clubRow.Bankgiro;
					newRow.ClubId = clubRow.ClubId;
					newRow.Country = clubRow.Country;
					newRow.Email = clubRow.Email;
					newRow.Name = clubRow.Name;
					newRow.Plusgiro = clubRow.Plusgiro;
					newRow.LastUpdate = clubRow.LastUpdate;
					this.ds.Clubs.AddClubsRow(newRow);
				}
			}
		}
		private DSStartupResources.ClubsRow getClub(string clubid)
		{
			DSStartupResources.ClubsRow[] rows = (DSStartupResources.ClubsRow[])this.ds.Clubs.Select("ClubId='" + clubid + "'");
			if (rows.Length > 0)
			{
				DSStartupResources.ClubsRow toReturn = rows[0];
				if (toReturn.IsBankgiroNull())
					toReturn.Bankgiro = "";
				if (toReturn.IsCountryNull())
					toReturn.Country = "SE";
				if (toReturn.IsEmailNull())
					toReturn.Email = "";
				if (toReturn.IsNameNull())
					toReturn.Name = "";
				if (toReturn.IsPlusgiroNull())
					toReturn.Plusgiro = "";

				return toReturn;
			}
			else
				throw new ApplicationException("ClubId does not exist");
		}
		#endregion

		#region Shooters
		private void checkToUpdate(DataSetWinShooter.ShootersRow shooterRow)
		{
			DSLocalCache temp = new DSLocalCache();
			DSLocalCache.ShootersRow newShooterRow = temp.Shooters.NewShootersRow();
			newShooterRow.Cardnr = shooterRow.Cardnr;
			newShooterRow.Class = shooterRow.Class;
			newShooterRow.ClubId = shooterRow.ClubId;
			newShooterRow.Email = shooterRow.Email;
			newShooterRow.Givenname = shooterRow.Givenname;
			newShooterRow.LastUpdate = DateTime.Parse("2004-01-01");
			newShooterRow.Surname = shooterRow.Surname;

			checkToUpdate(newShooterRow);
		}
		private void checkToUpdate(DSLocalCache.ShootersRow shooterRow)
		{
			if (int.Parse(shooterRow.Cardnr) > 45000)
				return;
			try
			{
				DSStartupResources.ShootersRow current = getShooter(shooterRow.Cardnr);

				// Since we got here, shooter already exist
				bool changed = false;
				string startString = "Do you want to update shooter " +
					current.Surname + " " + current.Givenname;
				string toView = startString;
				if (current.Class != shooterRow.Class)
				{
					toView += "\r\nClass=" + current.Class + " < " + shooterRow.Class;
					changed = true;
				}

				if (current.ClubId != shooterRow.ClubId)
				{
					toView += "\r\nClubId=" + current.ClubId + " < " + shooterRow.ClubId;
					changed = true;
				}

				if (current.Email != shooterRow.Email)
				{
					toView += "\r\nEmail=" + current.Email + " < " + shooterRow.Email;
					changed = true;
				}

				if (current.Givenname != shooterRow.Givenname)
				{
					toView += "\r\nGivenname=" + current.Givenname + " < " + shooterRow.Givenname;
					changed = true;
				}

				if (current.Surname != shooterRow.Surname)
				{
					toView += "\r\nSurname=" + current.Surname + " < " + shooterRow.Surname;
					changed = true;
				}

				toView += "\r\nLastUpdate=" + current.LastUpdate + " < " + shooterRow.LastUpdate;

				if (changed)
				{
					DialogResult res = MessageBox.Show(toView, "Update", MessageBoxButtons.YesNo);

					if (res == DialogResult.Yes)
					{
						current.Cardnr = shooterRow.Cardnr;
						current.Class = shooterRow.Class;
						current.ClubId = shooterRow.ClubId;
						current.Email = shooterRow.Email;
						current.Givenname = shooterRow.Givenname;
						current.Surname = shooterRow.Surname;
						current.LastUpdate = shooterRow.LastUpdate;
					}
				}
				else
				{
					if (current.LastUpdate < shooterRow.LastUpdate)
						current.LastUpdate = shooterRow.LastUpdate;
				}
			}
			catch(ApplicationException)
			{
				string toView = "Do you want to add shooter\r\n" +
					"Cardnr = " + shooterRow.Cardnr + "\r\n" +
					"Class = " + shooterRow.Class + "\r\n" +
					"ClubId = " + shooterRow.ClubId + "\r\n" +
					"Email = " + shooterRow.Email + "\r\n" +
					"Givenname = " + shooterRow.Givenname + "\r\n" +
					"Surname = " + shooterRow.Surname;
				DialogResult res = MessageBox.Show(toView, "Add", MessageBoxButtons.YesNo);

				if (res == DialogResult.Yes)
				{
					DSStartupResources.ShootersRow newShooter = this.ds.Shooters.NewShootersRow();
					newShooter.Cardnr = shooterRow.Cardnr;
					newShooter.Class = shooterRow.Class;
					newShooter.ClubId = shooterRow.ClubId;
					newShooter.Email = shooterRow.Email;
					newShooter.Givenname = shooterRow.Givenname;
					newShooter.Surname = shooterRow.Surname;
					newShooter.LastUpdate = shooterRow.LastUpdate;
					this.ds.Shooters.AddShootersRow(newShooter);
				}
			}
		}
		private DSStartupResources.ShootersRow getShooter(string cardNr)
		{
			DSStartupResources.ShootersRow[] rows = (DSStartupResources.ShootersRow[])
				this.ds.Shooters.Select("CardNr='" + cardNr + "'");
			if (rows.Length > 0)
			{
				DSStartupResources.ShootersRow toReturn = rows[0];

				return toReturn;
			}
			else
				throw new ApplicationException("ClubId does not exist");
		}
		#endregion

		private void menuImportInfoFromWinShooterCaache_Click(object sender, EventArgs e)
		{
			this.openFileDialog1.Filter = "*.xml|*.xml";
			DialogResult res = this.openFileDialog1.ShowDialog();
			if (res != DialogResult.OK)
				return;

			DSLocalCache temp = new DSLocalCache();
			temp.ReadXml(this.openFileDialog1.FileName);

			foreach (DSLocalCache.ClubsRow clubRow in temp.Clubs)
			{
				checkToUpdate(clubRow);
			}

			foreach (DSLocalCache.ShootersRow shooterRow in temp.Shooters)
			{
				if (int.Parse(shooterRow.Cardnr) > 0)
					checkToUpdate(shooterRow);
			}
			MessageBox.Show("Done!");
		}




		#endregion
	}
}
