#region copyright
/*
Copyright ©2009 John Allberg

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
// $Id: FCompetitors.cs 130 2011-05-28 17:32:36Z smuda $ 
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Allberg.Shooter.Common;
using Allberg.Shooter.Windows.Forms;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for FCompetitors.
	/// </summary>
	public class FCompetitors : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private SafeButton btnCancel;
		private SafeLabel SafeLabel1;
		private SafeButton btnSave;
		private SafeLabel Skyttekortsnr;
		private SafeLabel SafeLabel2;
		private SafeLabel SafeLabel3;
		private SafeLabel SafeLabel4;
		private SafeLabel SafeLabel5;
		private SafeLabel SafeLabel6;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddShooters;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddWeapon1;
		private Allberg.Shooter.Windows.Forms.SafeTextBox txtSurName;
		private Allberg.Shooter.Windows.Forms.SafeTextBox txtGivenName;
		private Allberg.Shooter.Windows.Forms.SafeTextBox txtEmail;
		private Allberg.Shooter.Windows.Forms.SafeTextBox txtPayed;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddWeapon2;
		private System.Windows.Forms.GroupBox groupBox1;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddWeapon3;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddPatrol1;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddPatrol2;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddClubs;
		private System.Windows.Forms.CheckBox chkCompetitor1;
		private System.Windows.Forms.CheckBox chkCompetitor2;
		private System.Windows.Forms.CheckBox chkCompetitor3;
		private Allberg.Shooter.Windows.Forms.SafeTextBox txtCardNr;
		private System.Windows.Forms.LinkLabel linkFetchShootersAutomatic;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddWeapon4;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddPatrol4;
		private System.Windows.Forms.CheckBox chkCompetitor4;
		private SafeButton btnDelete;
		private SafeLabel SafeLabel7;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddShooterClass;
		private System.Windows.Forms.GroupBox groupBox3;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddShooterClass1;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddShooterClass2;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddShooterClass3;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddShooterClass4;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox chkArrived;
		private System.Windows.Forms.GroupBox groupBoxPatrols;
		private Panel panel1;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddPatrol3;

		public delegate void EnableMainHandler();
		public event EnableMainHandler EnableMain;
		//private delegate void DDShooterResetHandler();
		private event MethodInvoker DDShooterBind;
		private delegate void DDShootersSetSelectedValueHandler(string setvalue);
		private event DDShootersSetSelectedValueHandler DDShootersSetSelectedValue;
		private delegate string DDShootersGetSelectedValueHandler();
		private event DDShootersGetSelectedValueHandler DDShootersGetSelectedValue;

		private event MethodInvoker updatedShootersWithThreadMethod;
		private event MethodInvoker updatedWeaponsWithThreadMethod;
		private event MethodInvoker updatedClubsWithThreadMethod;
		private event MethodInvoker updatedPatrolsWithThreadMethod;

		internal FCompetitors(ref Common.Interface newCommon)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			CommonCode = newCommon;

			Trace.WriteLine("FCompetitors: Creating");
			try
			{
				height = this.Size.Height;
				width = this.Size.Width;

				// Events
				this.Resize +=new EventHandler(FCompetitors_Resize);
				this.DDShooterBind += new MethodInvoker(FCompetitors_DDShooterBind);
				this.DDShootersSetSelectedValue += 
					new DDShootersSetSelectedValueHandler(FCompetitors_DDShootersSetSelectedValue);
				this.DDShootersGetSelectedValue += 
					new DDShootersGetSelectedValueHandler(FCompetitors_DDShootersGetSelectedValue);
				BindClubs += new MethodInvoker(this.bindClubs);
				BindWeapons += new MethodInvoker(this.bindWeapons);
				EnableMeInvoker += new MethodInvoker(this.enableMe);

				updatedShootersWithThreadMethod += new MethodInvoker(updatedShootersWithThread);
				updatedWeaponsWithThreadMethod += new MethodInvoker(updatedWeaponsWithThread);
				updatedClubsWithThreadMethod += new MethodInvoker(updatedClubsWithThread);
				updatedPatrolsWithThreadMethod += new MethodInvoker(updatedPatrolsWithThread);

				// Create one DataTable to bind dropdown Weapon with.
				createWeaponTable(ref weaponTable1);
				createWeaponTable(ref weaponTable2);
				createWeaponTable(ref weaponTable3);
				createWeaponTable(ref weaponTable4);

				// Display the right things in weapons
				this.ddWeapon1.DataSource = weaponTable1;
				this.ddWeapon1.ValueMember = "Id";
				this.ddWeapon1.DisplayMember = "Name";
				this.ddWeapon2.DataSource = weaponTable2;
				this.ddWeapon2.ValueMember = "Id";
				this.ddWeapon2.DisplayMember = "Name";
				this.ddWeapon3.DataSource = weaponTable3;
				this.ddWeapon3.ValueMember = "Id";
				this.ddWeapon3.DisplayMember = "Name";
				this.ddWeapon4.DataSource = weaponTable4;
				this.ddWeapon4.ValueMember = "Id";
				this.ddWeapon4.DisplayMember = "Name";

				
				// Create DataTables to bind dropdown Patrols with.
				patrolTable1 = new DataTable();
				DataColumn colPName1 = new DataColumn("Name",Type.GetType("System.String"));
				patrolTable1.Columns.Add(colPName1);
				DataColumn colPId1 = new DataColumn("Id",Type.GetType("System.Int32"));
				patrolTable1.Columns.Add(colPId1);

				patrolTable2 = new DataTable();
				DataColumn colPName2 = new DataColumn("Name",Type.GetType("System.String"));
				patrolTable2.Columns.Add(colPName2);
				DataColumn colPId2 = new DataColumn("Id",Type.GetType("System.Int32"));
				patrolTable2.Columns.Add(colPId2);

				patrolTable3 = new DataTable();
				DataColumn colPName3 = new DataColumn("Name",Type.GetType("System.String"));
				patrolTable3.Columns.Add(colPName3);
				DataColumn colPId3 = new DataColumn("Id",Type.GetType("System.Int32"));
				patrolTable3.Columns.Add(colPId3);

				patrolTable4 = new DataTable();
				DataColumn colPName4 = new DataColumn("Name",Type.GetType("System.String"));
				patrolTable4.Columns.Add(colPName4);
				DataColumn colPId4 = new DataColumn("Id",Type.GetType("System.Int32"));
				patrolTable4.Columns.Add(colPId4);

				// Display the right things in patrols
				this.ddPatrol1.DataSource = patrolTable1;
				this.ddPatrol2.DataSource = patrolTable2;
				this.ddPatrol3.DataSource = patrolTable3;
				this.ddPatrol4.DataSource = patrolTable4;
				this.ddPatrol1.ValueMember = "Id";
				this.ddPatrol2.ValueMember = "Id";
				this.ddPatrol3.ValueMember = "Id";
				this.ddPatrol4.ValueMember = "Id";
				this.ddPatrol1.DisplayMember = "Name";
				this.ddPatrol2.DisplayMember = "Name";
				this.ddPatrol3.DisplayMember = "Name";
				this.ddPatrol4.DisplayMember = "Name";


				// Create one DataTable to bind dropdown Shooters with.
				shooterTable = new DataTable();
				DataColumn colSName = new DataColumn("Name",Type.GetType("System.String"));
				shooterTable.Columns.Add(colSName);

				DataColumn colSId = new DataColumn("Id",Type.GetType("System.String"));
				shooterTable.Columns.Add(colSId);

				// Display the right things in weapons
				this.ddShooters.DataSource = shooterTable;
				this.ddShooters.ValueMember = "Id";
				this.ddShooters.DisplayMember = "Name";

				// Create DataTables to bind dropdown shootersclass with
				shooterClassTable = new DataTable();
				DataColumn colSCName = new DataColumn("Name",Type.GetType("System.String"));
				shooterClassTable.Columns.Add(colSCName);
				DataColumn colSCId = new DataColumn("Id",Type.GetType("System.String"));
				shooterClassTable.Columns.Add(colSCId);

				shooterClassTable1 = new DataTable();
				DataColumn colSCName1 = new DataColumn("Name",Type.GetType("System.String"));
				shooterClassTable1.Columns.Add(colSCName1);
				DataColumn colSCId1 = new DataColumn("Id",Type.GetType("System.String"));
				shooterClassTable1.Columns.Add(colSCId1);

				shooterClassTable2 = new DataTable();
				DataColumn colSCName2 = new DataColumn("Name",Type.GetType("System.String"));
				shooterClassTable2.Columns.Add(colSCName2);
				DataColumn colSCId2 = new DataColumn("Id",Type.GetType("System.String"));
				shooterClassTable2.Columns.Add(colSCId2);

				shooterClassTable3 = new DataTable();
				DataColumn colSCName3 = new DataColumn("Name",Type.GetType("System.String"));
				shooterClassTable3.Columns.Add(colSCName3);
				DataColumn colSCId3 = new DataColumn("Id",Type.GetType("System.String"));
				shooterClassTable3.Columns.Add(colSCId3);

				shooterClassTable4 = new DataTable();
				DataColumn colSCName4 = new DataColumn("Name",Type.GetType("System.String"));
				shooterClassTable4.Columns.Add(colSCName4);
				DataColumn colSCId4 = new DataColumn("Id",Type.GetType("System.String"));
				shooterClassTable4.Columns.Add(colSCId4);

				// Populate shooterClassTable
				populateShooterClass();

				// Display the right things in ShootersClass
				this.ddShooterClass.DataSource = shooterClassTable;
				this.ddShooterClass.ValueMember = "Id";
				this.ddShooterClass.DisplayMember = "Name";

				this.ddShooterClass1.DataSource = shooterClassTable1;
				this.ddShooterClass1.ValueMember = "Id";
				this.ddShooterClass1.DisplayMember = "Name";

				this.ddShooterClass2.DataSource = shooterClassTable2;
				this.ddShooterClass2.ValueMember = "Id";
				this.ddShooterClass2.DisplayMember = "Name";

				this.ddShooterClass3.DataSource = shooterClassTable3;
				this.ddShooterClass3.ValueMember = "Id";
				this.ddShooterClass3.DisplayMember = "Name";

				this.ddShooterClass4.DataSource = shooterClassTable4;
				this.ddShooterClass4.ValueMember = "Id";
				this.ddShooterClass4.DisplayMember = "Name";

				// Enable internet database
				this.linkFetchShootersAutomatic.Visible = CommonCode.EnableInternetConnections;
			}
			catch(Exception exc)
			{
				Trace.WriteLine("FCompetitors: Exception:" + exc.ToString());
			}
			finally
			{
				Trace.WriteLine("FCompetitors: Created.");
			}
		}

		private void populateShooterClass()
		{
			Trace.WriteLine("FCompetitors: populateShooterClass " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			for(int i=1;i<=3;i++)
			{
				DataRow newRow = shooterClassTable.NewRow();
				newRow["Id"] = i;
				newRow["Name"] = "Klass " + i.ToString();
				shooterClassTable.Rows.Add(newRow);
			}

			// Competitor 1 shooterclass 
			DataRow newRow1Default = shooterClassTable1.NewRow();
			newRow1Default["Id"] = 0;
			newRow1Default["Name"] = "Klass 1";
			shooterClassTable1.Rows.Add(newRow1Default);

			DataRow newRow1Dam = shooterClassTable1.NewRow();
			newRow1Dam["Id"] = 1;
			newRow1Dam["Name"] = "Dam";
			shooterClassTable1.Rows.Add(newRow1Dam);

			DataRow newRow1Junior = shooterClassTable1.NewRow();
			newRow1Junior["Id"] = 2;
			newRow1Junior["Name"] = "Junior";
			shooterClassTable1.Rows.Add(newRow1Junior);

			DataRow newRow1VY = shooterClassTable1.NewRow();
			newRow1VY["Id"] = 3;
			newRow1VY["Name"] = "VY";
			shooterClassTable1.Rows.Add(newRow1VY);

			DataRow newRow1VA = shooterClassTable1.NewRow();
			newRow1VA["Id"] = 4;
			newRow1VA["Name"] = "VÄ";
			shooterClassTable1.Rows.Add(newRow1VA);

			DataRow newRow1ÖP = shooterClassTable1.NewRow();
			newRow1ÖP["Id"] = 5;
			newRow1ÖP["Name"] = "ÖP";
			shooterClassTable1.Rows.Add(newRow1ÖP);

			// Competitor 2 shooterclass 
			DataRow newRow2Default = shooterClassTable2.NewRow();
			newRow2Default["Id"] = 0;
			newRow2Default["Name"] = "Klass 1";
			shooterClassTable2.Rows.Add(newRow2Default);

			DataRow newRow2Dam = shooterClassTable2.NewRow();
			newRow2Dam["Id"] = 1;
			newRow2Dam["Name"] = "Dam";
			shooterClassTable2.Rows.Add(newRow2Dam);

			DataRow newRow2Junior = shooterClassTable2.NewRow();
			newRow2Junior["Id"] = 2;
			newRow2Junior["Name"] = "Junior";
			shooterClassTable2.Rows.Add(newRow2Junior);

			DataRow newRow2VY = shooterClassTable2.NewRow();
			newRow2VY["Id"] = 3;
			newRow2VY["Name"] = "VY";
			shooterClassTable2.Rows.Add(newRow2VY);

			DataRow newRow2VA = shooterClassTable2.NewRow();
			newRow2VA["Id"] = 4;
			newRow2VA["Name"] = "VÄ";
			shooterClassTable2.Rows.Add(newRow2VA);

			DataRow newRow2ÖP = shooterClassTable2.NewRow();
			newRow2ÖP["Id"] = 5;
			newRow2ÖP["Name"] = "ÖP";
			shooterClassTable2.Rows.Add(newRow2ÖP);

			// Competitor 3 shooterclass 
			DataRow newRow3Default = shooterClassTable3.NewRow();
			newRow3Default["Id"] = 0;
			newRow3Default["Name"] = "Klass 1";
			shooterClassTable3.Rows.Add(newRow3Default);

			DataRow newRow3Dam = shooterClassTable3.NewRow();
			newRow3Dam["Id"] = 1;
			newRow3Dam["Name"] = "Dam";
			shooterClassTable3.Rows.Add(newRow3Dam);

			DataRow newRow3Junior = shooterClassTable3.NewRow();
			newRow3Junior["Id"] = 2;
			newRow3Junior["Name"] = "Junior";
			shooterClassTable3.Rows.Add(newRow3Junior);

			DataRow newRow3VY = shooterClassTable3.NewRow();
			newRow3VY["Id"] = 3;
			newRow3VY["Name"] = "VY";
			shooterClassTable3.Rows.Add(newRow3VY);

			DataRow newRow3VA = shooterClassTable3.NewRow();
			newRow3VA["Id"] = 4;
			newRow3VA["Name"] = "VÄ";
			shooterClassTable3.Rows.Add(newRow3VA);

			DataRow newRow3ÖP = shooterClassTable3.NewRow();
			newRow3ÖP["Id"] = 5;
			newRow3ÖP["Name"] = "ÖP";
			shooterClassTable3.Rows.Add(newRow3ÖP);

			// Competitor 4 shooterclass 
			DataRow newRow4Default = shooterClassTable4.NewRow();
			newRow4Default["Id"] = 0;
			newRow4Default["Name"] = "Klass 1";
			shooterClassTable4.Rows.Add(newRow4Default);

			DataRow newRow4Dam = shooterClassTable4.NewRow();
			newRow4Dam["Id"] = 1;
			newRow4Dam["Name"] = "Dam";
			shooterClassTable4.Rows.Add(newRow4Dam);

			DataRow newRow4Junior = shooterClassTable4.NewRow();
			newRow4Junior["Id"] = 2;
			newRow4Junior["Name"] = "Junior";
			shooterClassTable4.Rows.Add(newRow4Junior);

			DataRow newRow4VY = shooterClassTable4.NewRow();
			newRow4VY["Id"] = 3;
			newRow4VY["Name"] = "VY";
			shooterClassTable4.Rows.Add(newRow4VY);

			DataRow newRow4VA = shooterClassTable4.NewRow();
			newRow4VA["Id"] = 4;
			newRow4VA["Name"] = "VÄ";
			shooterClassTable4.Rows.Add(newRow4VA);

			DataRow newRow4ÖP = shooterClassTable4.NewRow();
			newRow4ÖP["Id"] = 5;
			newRow4ÖP["Name"] = "ÖP";
			shooterClassTable4.Rows.Add(newRow4ÖP);
			
			Trace.WriteLine("FCompetitors: populateShooterClass ended.");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			Trace.WriteLine("FCompetitors: Dispose(" + disposing.ToString() + ")" +
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
				Trace.WriteLine("FCompetitors: exception while disposing:" + exc.ToString());
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
			Trace.WriteLine("FCompetitors: Dispose(" + disposing.ToString() + ")" +
				"ended " +
				DateTime.Now.ToLongTimeString());
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCompetitors));
			this.btnCancel = new SafeButton();
			this.SafeLabel1 = new SafeLabel();
			this.Skyttekortsnr = new SafeLabel();
			this.ddShooters = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.btnDelete = new SafeButton();
			this.txtCardNr = new Allberg.Shooter.Windows.Forms.SafeTextBox();
			this.btnSave = new SafeButton();
			this.linkFetchShootersAutomatic = new System.Windows.Forms.LinkLabel();
			this.SafeLabel2 = new SafeLabel();
			this.SafeLabel3 = new SafeLabel();
			this.SafeLabel4 = new SafeLabel();
			this.SafeLabel5 = new SafeLabel();
			this.SafeLabel6 = new SafeLabel();
			this.chkCompetitor1 = new System.Windows.Forms.CheckBox();
			this.chkCompetitor2 = new System.Windows.Forms.CheckBox();
			this.chkCompetitor3 = new System.Windows.Forms.CheckBox();
			this.ddWeapon1 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.txtSurName = new Allberg.Shooter.Windows.Forms.SafeTextBox();
			this.txtGivenName = new Allberg.Shooter.Windows.Forms.SafeTextBox();
			this.txtEmail = new Allberg.Shooter.Windows.Forms.SafeTextBox();
			this.txtPayed = new Allberg.Shooter.Windows.Forms.SafeTextBox();
			this.ddClubs = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.ddWeapon2 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ddWeapon4 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.ddWeapon3 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.groupBoxPatrols = new System.Windows.Forms.GroupBox();
			this.ddPatrol4 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.ddPatrol3 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.ddPatrol2 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.ddPatrol1 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.chkCompetitor4 = new System.Windows.Forms.CheckBox();
			this.SafeLabel7 = new SafeLabel();
			this.ddShooterClass = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.ddShooterClass4 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.ddShooterClass3 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.ddShooterClass2 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.ddShooterClass1 = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.chkArrived = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1.SuspendLayout();
			this.groupBoxPatrols.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(424, 336);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 23;
			this.btnCancel.Text = "Stäng";
			this.toolTip1.SetToolTip(this.btnCancel, "Stäng utan att spara skytt");
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// SafeLabel1
			// 
			this.SafeLabel1.Location = new System.Drawing.Point(8, 8);
			this.SafeLabel1.Name = "SafeLabel1";
			this.SafeLabel1.Size = new System.Drawing.Size(100, 23);
			this.SafeLabel1.TabIndex = 1;
			this.SafeLabel1.Text = "Skyttar";
			// 
			// Skyttekortsnr
			// 
			this.Skyttekortsnr.Location = new System.Drawing.Point(8, 32);
			this.Skyttekortsnr.Name = "Skyttekortsnr";
			this.Skyttekortsnr.Size = new System.Drawing.Size(100, 23);
			this.Skyttekortsnr.TabIndex = 2;
			this.Skyttekortsnr.Text = "Pistolskyttekort";
			// 
			// ddShooters
			// 
			this.ddShooters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ddShooters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddShooters.Location = new System.Drawing.Point(112, 8);
			this.ddShooters.Name = "ddShooters";
			this.ddShooters.Size = new System.Drawing.Size(312, 21);
			this.ddShooters.TabIndex = 1;
			this.toolTip1.SetToolTip(this.ddShooters, "Välj en skytt för att kunna editera information om honom/henne");
			this.ddShooters.SelectedIndexChanged += new System.EventHandler(this.ddShooters_SelectedIndexChanged);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.Location = new System.Drawing.Point(432, 8);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "Radera";
			this.toolTip1.SetToolTip(this.btnDelete, "Raderar den valda skytten");
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// txtCardNr
			// 
			this.txtCardNr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCardNr.Location = new System.Drawing.Point(112, 32);
			this.txtCardNr.Name = "txtCardNr";
			this.txtCardNr.Size = new System.Drawing.Size(392, 20);
			this.txtCardNr.TabIndex = 3;
			this.toolTip1.SetToolTip(this.txtCardNr, "Fyll i pistolskyttekort");
			this.txtCardNr.Leave += new System.EventHandler(this.txtCardNr_CursorLeave);
			this.txtCardNr.TextChanged += new System.EventHandler(this.txtCardNr_TextChanged);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(344, 336);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 22;
			this.btnSave.Text = "Spara";
			this.toolTip1.SetToolTip(this.btnSave, "Sparar skytt");
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// linkFetchShootersAutomatic
			// 
			this.linkFetchShootersAutomatic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.linkFetchShootersAutomatic.Location = new System.Drawing.Point(8, 336);
			this.linkFetchShootersAutomatic.Name = "linkFetchShootersAutomatic";
			this.linkFetchShootersAutomatic.Size = new System.Drawing.Size(144, 23);
			this.linkFetchShootersAutomatic.TabIndex = 21;
			this.linkFetchShootersAutomatic.TabStop = true;
			this.linkFetchShootersAutomatic.Text = "Hämta skyttar från Internet";
			this.linkFetchShootersAutomatic.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkFetchShootersAutomatic_LinkClicked);
			// 
			// SafeLabel2
			// 
			this.SafeLabel2.Location = new System.Drawing.Point(8, 56);
			this.SafeLabel2.Name = "SafeLabel2";
			this.SafeLabel2.Size = new System.Drawing.Size(100, 23);
			this.SafeLabel2.TabIndex = 8;
			this.SafeLabel2.Text = "Förnamn";
			// 
			// SafeLabel3
			// 
			this.SafeLabel3.Location = new System.Drawing.Point(8, 80);
			this.SafeLabel3.Name = "SafeLabel3";
			this.SafeLabel3.Size = new System.Drawing.Size(100, 23);
			this.SafeLabel3.TabIndex = 9;
			this.SafeLabel3.Text = "Efternamn";
			// 
			// SafeLabel4
			// 
			this.SafeLabel4.Location = new System.Drawing.Point(8, 128);
			this.SafeLabel4.Name = "SafeLabel4";
			this.SafeLabel4.Size = new System.Drawing.Size(100, 23);
			this.SafeLabel4.TabIndex = 10;
			this.SafeLabel4.Text = "Epost";
			// 
			// SafeLabel5
			// 
			this.SafeLabel5.Location = new System.Drawing.Point(8, 152);
			this.SafeLabel5.Name = "SafeLabel5";
			this.SafeLabel5.Size = new System.Drawing.Size(100, 23);
			this.SafeLabel5.TabIndex = 11;
			this.SafeLabel5.Text = "Klubb";
			// 
			// SafeLabel6
			// 
			this.SafeLabel6.Location = new System.Drawing.Point(8, 176);
			this.SafeLabel6.Name = "SafeLabel6";
			this.SafeLabel6.Size = new System.Drawing.Size(100, 23);
			this.SafeLabel6.TabIndex = 12;
			this.SafeLabel6.Text = "Betalat";
			// 
			// chkCompetitor1
			// 
			this.chkCompetitor1.Checked = true;
			this.chkCompetitor1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCompetitor1.Location = new System.Drawing.Point(7, 12);
			this.chkCompetitor1.Name = "chkCompetitor1";
			this.chkCompetitor1.Size = new System.Drawing.Size(68, 24);
			this.chkCompetitor1.TabIndex = 10;
			this.chkCompetitor1.TabStop = false;
			this.chkCompetitor1.Text = "Vapen 1";
			this.chkCompetitor1.CheckedChanged += new System.EventHandler(this.chkCompetitor1_CheckedChanged);
			// 
			// chkCompetitor2
			// 
			this.chkCompetitor2.Location = new System.Drawing.Point(7, 37);
			this.chkCompetitor2.Name = "chkCompetitor2";
			this.chkCompetitor2.Size = new System.Drawing.Size(68, 24);
			this.chkCompetitor2.TabIndex = 12;
			this.chkCompetitor2.Text = "Vapen 2";
			this.chkCompetitor2.CheckedChanged += new System.EventHandler(this.chkCompetitor2_CheckedChanged);
			// 
			// chkCompetitor3
			// 
			this.chkCompetitor3.Location = new System.Drawing.Point(7, 60);
			this.chkCompetitor3.Name = "chkCompetitor3";
			this.chkCompetitor3.Size = new System.Drawing.Size(68, 24);
			this.chkCompetitor3.TabIndex = 15;
			this.chkCompetitor3.Text = "Vapen 3";
			this.chkCompetitor3.CheckedChanged += new System.EventHandler(this.chkCompetitor3_CheckedChanged);
			// 
			// ddWeapon1
			// 
			this.ddWeapon1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddWeapon1.Location = new System.Drawing.Point(8, 16);
			this.ddWeapon1.Name = "ddWeapon1";
			this.ddWeapon1.Size = new System.Drawing.Size(121, 21);
			this.ddWeapon1.TabIndex = 10;
			this.toolTip1.SetToolTip(this.ddWeapon1, "Välj vilket vapen som skytten ska använda. Om du inte vet vilket vapen men vilken" +
					" klass, välj ett av \"Unknown\"-vapnen");
			this.ddWeapon1.SelectedIndexChanged += new System.EventHandler(this.ddWeapon1_SelectedIndexChanged);
			this.ddWeapon1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ddWeapon1_MouseUp);
			// 
			// txtSurName
			// 
			this.txtSurName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSurName.Location = new System.Drawing.Point(112, 56);
			this.txtSurName.Name = "txtSurName";
			this.txtSurName.Size = new System.Drawing.Size(392, 20);
			this.txtSurName.TabIndex = 4;
			this.toolTip1.SetToolTip(this.txtSurName, "Fyll i skyttens förnamn");
			this.txtSurName.TextChanged += new System.EventHandler(this.txtSurName_TextChanged);
			// 
			// txtGivenName
			// 
			this.txtGivenName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtGivenName.Location = new System.Drawing.Point(112, 80);
			this.txtGivenName.Name = "txtGivenName";
			this.txtGivenName.Size = new System.Drawing.Size(392, 20);
			this.txtGivenName.TabIndex = 5;
			this.toolTip1.SetToolTip(this.txtGivenName, "Fyll i skyttens efternamn");
			this.txtGivenName.TextChanged += new System.EventHandler(this.txtGivenName_TextChanged);
			// 
			// txtEmail
			// 
			this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtEmail.Location = new System.Drawing.Point(112, 128);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(392, 20);
			this.txtEmail.TabIndex = 7;
			this.toolTip1.SetToolTip(this.txtEmail, "Fyll i skyttens e-postadress");
			this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
			// 
			// txtPayed
			// 
			this.txtPayed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPayed.Location = new System.Drawing.Point(112, 176);
			this.txtPayed.Name = "txtPayed";
			this.txtPayed.Size = new System.Drawing.Size(160, 20);
			this.txtPayed.TabIndex = 9;
			this.txtPayed.TextChanged += new System.EventHandler(this.txtPayed_TextChanged);
			// 
			// ddClubs
			// 
			this.ddClubs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ddClubs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddClubs.Location = new System.Drawing.Point(112, 152);
			this.ddClubs.Name = "ddClubs";
			this.ddClubs.Size = new System.Drawing.Size(392, 21);
			this.ddClubs.TabIndex = 8;
			this.toolTip1.SetToolTip(this.ddClubs, "Välj skyttens klubb");
			this.ddClubs.SelectedIndexChanged += new System.EventHandler(this.ddClubs_SelectedIndexChanged);
			// 
			// ddWeapon2
			// 
			this.ddWeapon2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddWeapon2.Enabled = false;
			this.ddWeapon2.Location = new System.Drawing.Point(8, 40);
			this.ddWeapon2.Name = "ddWeapon2";
			this.ddWeapon2.Size = new System.Drawing.Size(121, 21);
			this.ddWeapon2.TabIndex = 13;
			this.toolTip1.SetToolTip(this.ddWeapon2, "Välj vilket vapen som skytten ska använda. Om du inte vet vilket vapen men vilken" +
					" klass, välj ett av \"Unknown\"-vapnen");
			this.ddWeapon2.SelectedIndexChanged += new System.EventHandler(this.ddWeapon2_SelectedIndexChanged);
			this.ddWeapon2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ddWeapon2_MouseUp);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.ddWeapon4);
			this.groupBox1.Controls.Add(this.ddWeapon3);
			this.groupBox1.Controls.Add(this.ddWeapon1);
			this.groupBox1.Controls.Add(this.ddWeapon2);
			this.groupBox1.Location = new System.Drawing.Point(90, -1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(136, 120);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Vapen";
			// 
			// ddWeapon4
			// 
			this.ddWeapon4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddWeapon4.Enabled = false;
			this.ddWeapon4.Location = new System.Drawing.Point(8, 88);
			this.ddWeapon4.Name = "ddWeapon4";
			this.ddWeapon4.Size = new System.Drawing.Size(121, 21);
			this.ddWeapon4.TabIndex = 19;
			this.toolTip1.SetToolTip(this.ddWeapon4, "Välj vilket vapen som skytten ska använda. Om du inte vet vilket vapen men vilken" +
					" klass, välj ett av \"Unknown\"-vapnen");
			this.ddWeapon4.SelectedIndexChanged += new System.EventHandler(this.ddWeapon4_SelectedIndexChanged);
			this.ddWeapon4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ddWeapon4_MouseUp);
			// 
			// ddWeapon3
			// 
			this.ddWeapon3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddWeapon3.Enabled = false;
			this.ddWeapon3.Location = new System.Drawing.Point(8, 64);
			this.ddWeapon3.Name = "ddWeapon3";
			this.ddWeapon3.Size = new System.Drawing.Size(121, 21);
			this.ddWeapon3.TabIndex = 16;
			this.toolTip1.SetToolTip(this.ddWeapon3, "Välj vilket vapen som skytten ska använda. Om du inte vet vilket vapen men vilken" +
					" klass, välj ett av \"Unknown\"-vapnen");
			this.ddWeapon3.SelectedIndexChanged += new System.EventHandler(this.ddWeapon3_SelectedIndexChanged);
			this.ddWeapon3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ddWeapon3_MouseUp);
			// 
			// groupBoxPatrols
			// 
			this.groupBoxPatrols.Controls.Add(this.ddPatrol4);
			this.groupBoxPatrols.Controls.Add(this.ddPatrol3);
			this.groupBoxPatrols.Controls.Add(this.ddPatrol2);
			this.groupBoxPatrols.Controls.Add(this.ddPatrol1);
			this.groupBoxPatrols.Location = new System.Drawing.Point(232, 0);
			this.groupBoxPatrols.Name = "groupBoxPatrols";
			this.groupBoxPatrols.Size = new System.Drawing.Size(136, 120);
			this.groupBoxPatrols.TabIndex = 24;
			this.groupBoxPatrols.TabStop = false;
			this.groupBoxPatrols.Text = "Patrull";
			// 
			// ddPatrol4
			// 
			this.ddPatrol4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddPatrol4.Enabled = false;
			this.ddPatrol4.Location = new System.Drawing.Point(8, 88);
			this.ddPatrol4.Name = "ddPatrol4";
			this.ddPatrol4.Size = new System.Drawing.Size(121, 21);
			this.ddPatrol4.TabIndex = 20;
			this.toolTip1.SetToolTip(this.ddPatrol4, "Här kan du manuellt välja vilken patrull som skytten ska vara med i");
			this.ddPatrol4.SelectedIndexChanged += new System.EventHandler(this.ddPatrol4_SelectedIndexChanged);
			// 
			// ddPatrol3
			// 
			this.ddPatrol3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddPatrol3.Enabled = false;
			this.ddPatrol3.Location = new System.Drawing.Point(8, 64);
			this.ddPatrol3.Name = "ddPatrol3";
			this.ddPatrol3.Size = new System.Drawing.Size(121, 21);
			this.ddPatrol3.TabIndex = 17;
			this.toolTip1.SetToolTip(this.ddPatrol3, "Här kan du manuellt välja vilken patrull som skytten ska vara med i");
			this.ddPatrol3.SelectedIndexChanged += new System.EventHandler(this.ddPatrol3_SelectedIndexChanged);
			// 
			// ddPatrol2
			// 
			this.ddPatrol2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddPatrol2.Enabled = false;
			this.ddPatrol2.Location = new System.Drawing.Point(8, 40);
			this.ddPatrol2.Name = "ddPatrol2";
			this.ddPatrol2.Size = new System.Drawing.Size(121, 21);
			this.ddPatrol2.TabIndex = 14;
			this.toolTip1.SetToolTip(this.ddPatrol2, "Här kan du manuellt välja vilken patrull som skytten ska vara med i");
			this.ddPatrol2.SelectedIndexChanged += new System.EventHandler(this.ddPatrol2_SelectedIndexChanged);
			// 
			// ddPatrol1
			// 
			this.ddPatrol1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddPatrol1.Location = new System.Drawing.Point(8, 16);
			this.ddPatrol1.Name = "ddPatrol1";
			this.ddPatrol1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ddPatrol1.Size = new System.Drawing.Size(121, 21);
			this.ddPatrol1.TabIndex = 11;
			this.toolTip1.SetToolTip(this.ddPatrol1, "Här kan du manuellt välja vilken patrull som skytten ska vara med i");
			this.ddPatrol1.SelectedIndexChanged += new System.EventHandler(this.ddPatrol1_SelectedIndexChanged);
			// 
			// chkCompetitor4
			// 
			this.chkCompetitor4.Location = new System.Drawing.Point(7, 84);
			this.chkCompetitor4.Name = "chkCompetitor4";
			this.chkCompetitor4.Size = new System.Drawing.Size(68, 24);
			this.chkCompetitor4.TabIndex = 18;
			this.chkCompetitor4.Text = "Vapen 4";
			this.chkCompetitor4.CheckedChanged += new System.EventHandler(this.chkCompetitor4_CheckedChanged);
			// 
			// SafeLabel7
			// 
			this.SafeLabel7.Location = new System.Drawing.Point(8, 104);
			this.SafeLabel7.Name = "SafeLabel7";
			this.SafeLabel7.Size = new System.Drawing.Size(100, 23);
			this.SafeLabel7.TabIndex = 26;
			this.SafeLabel7.Text = "Skytteklass";
			// 
			// ddShooterClass
			// 
			this.ddShooterClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ddShooterClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddShooterClass.Location = new System.Drawing.Point(112, 104);
			this.ddShooterClass.Name = "ddShooterClass";
			this.ddShooterClass.Size = new System.Drawing.Size(392, 21);
			this.ddShooterClass.TabIndex = 6;
			this.toolTip1.SetToolTip(this.ddShooterClass, "Välj skyttens klass");
			this.ddShooterClass.SelectedIndexChanged += new System.EventHandler(this.ddShooterClass_SelectedIndexChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.ddShooterClass4);
			this.groupBox3.Controls.Add(this.ddShooterClass3);
			this.groupBox3.Controls.Add(this.ddShooterClass2);
			this.groupBox3.Controls.Add(this.ddShooterClass1);
			this.groupBox3.Location = new System.Drawing.Point(374, 0);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(109, 120);
			this.groupBox3.TabIndex = 27;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Klass";
			// 
			// ddShooterClass4
			// 
			this.ddShooterClass4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddShooterClass4.Enabled = false;
			this.ddShooterClass4.Location = new System.Drawing.Point(8, 88);
			this.ddShooterClass4.Name = "ddShooterClass4";
			this.ddShooterClass4.Size = new System.Drawing.Size(88, 21);
			this.ddShooterClass4.TabIndex = 3;
			this.toolTip1.SetToolTip(this.ddShooterClass4, "Här väljer du specialklasser så som dam, junior och veteran. Samtliga dessa kan e" +
					"ndast väljas om vapnet är ett C-vapen, då dessa klasser endast tävlar i C");
			this.ddShooterClass4.SelectedIndexChanged += new System.EventHandler(this.ddShooterClass4_SelectedIndexChanged);
			// 
			// ddShooterClass3
			// 
			this.ddShooterClass3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddShooterClass3.Enabled = false;
			this.ddShooterClass3.Location = new System.Drawing.Point(8, 64);
			this.ddShooterClass3.Name = "ddShooterClass3";
			this.ddShooterClass3.Size = new System.Drawing.Size(88, 21);
			this.ddShooterClass3.TabIndex = 2;
			this.toolTip1.SetToolTip(this.ddShooterClass3, "Här väljer du specialklasser så som dam, junior och veteran. Samtliga dessa kan e" +
					"ndast väljas om vapnet är ett C-vapen, då dessa klasser endast tävlar i C");
			this.ddShooterClass3.SelectedIndexChanged += new System.EventHandler(this.ddShooterClass3_SelectedIndexChanged);
			// 
			// ddShooterClass2
			// 
			this.ddShooterClass2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddShooterClass2.Enabled = false;
			this.ddShooterClass2.Location = new System.Drawing.Point(8, 40);
			this.ddShooterClass2.Name = "ddShooterClass2";
			this.ddShooterClass2.Size = new System.Drawing.Size(88, 21);
			this.ddShooterClass2.TabIndex = 1;
			this.toolTip1.SetToolTip(this.ddShooterClass2, "Här väljer du specialklasser så som dam, junior och veteran. Samtliga dessa kan e" +
					"ndast väljas om vapnet är ett C-vapen, då dessa klasser endast tävlar i C");
			this.ddShooterClass2.SelectedIndexChanged += new System.EventHandler(this.ddShooterClass2_SelectedIndexChanged);
			// 
			// ddShooterClass1
			// 
			this.ddShooterClass1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddShooterClass1.Location = new System.Drawing.Point(8, 16);
			this.ddShooterClass1.Name = "ddShooterClass1";
			this.ddShooterClass1.Size = new System.Drawing.Size(88, 21);
			this.ddShooterClass1.TabIndex = 0;
			this.toolTip1.SetToolTip(this.ddShooterClass1, "Här väljer du specialklasser så som dam, junior och veteran. Samtliga dessa kan e" +
					"ndast väljas om vapnet är ett C-vapen, då dessa klasser endast tävlar i C");
			this.ddShooterClass1.SelectedIndexChanged += new System.EventHandler(this.ddShooterClass1_SelectedIndexChanged);
			// 
			// chkArrived
			// 
			this.chkArrived.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkArrived.Location = new System.Drawing.Point(392, 176);
			this.chkArrived.Name = "chkArrived";
			this.chkArrived.Size = new System.Drawing.Size(104, 24);
			this.chkArrived.TabIndex = 28;
			this.chkArrived.Text = "Ankommit";
			this.chkArrived.CheckedChanged += new System.EventHandler(this.chkArrived_CheckedChanged);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.AutoScroll = true;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.chkCompetitor1);
			this.panel1.Controls.Add(this.chkCompetitor2);
			this.panel1.Controls.Add(this.groupBox3);
			this.panel1.Controls.Add(this.chkCompetitor3);
			this.panel1.Controls.Add(this.chkCompetitor4);
			this.panel1.Controls.Add(this.groupBoxPatrols);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Location = new System.Drawing.Point(11, 208);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(496, 125);
			this.panel1.TabIndex = 29;
			// 
			// FCompetitors
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(512, 367);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.chkArrived);
			this.Controls.Add(this.ddShooterClass);
			this.Controls.Add(this.SafeLabel7);
			this.Controls.Add(this.ddClubs);
			this.Controls.Add(this.txtPayed);
			this.Controls.Add(this.txtEmail);
			this.Controls.Add(this.txtGivenName);
			this.Controls.Add(this.txtSurName);
			this.Controls.Add(this.txtCardNr);
			this.Controls.Add(this.SafeLabel6);
			this.Controls.Add(this.SafeLabel5);
			this.Controls.Add(this.SafeLabel4);
			this.Controls.Add(this.SafeLabel3);
			this.Controls.Add(this.SafeLabel2);
			this.Controls.Add(this.linkFetchShootersAutomatic);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.ddShooters);
			this.Controls.Add(this.Skyttekortsnr);
			this.Controls.Add(this.SafeLabel1);
			this.Controls.Add(this.btnCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FCompetitors";
			this.Text = "Skyttar";
			this.groupBox1.ResumeLayout(false);
			this.groupBoxPatrols.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		internal bool DisposeNow = false;
		Common.Interface CommonCode;
		int height;
		int width;

		Structs.Weapon[] weapons;
		DataTable weaponTable1;
		DataTable weaponTable2;
		DataTable weaponTable3;
		DataTable weaponTable4;

		Structs.Club[] clubs;
		DataTable clubTable;

		Structs.Patrol[] patrols;
		DataTable patrolTable1;
		DataTable patrolTable2;
		DataTable patrolTable3;
		DataTable patrolTable4;

		Structs.Shooter[] shooters;
		DataTable shooterTable;
		DataTable shooterClassTable;
		DataTable shooterClassTable1;
		DataTable shooterClassTable2;
		DataTable shooterClassTable3;
		DataTable shooterClassTable4;

		private const string NewShooterValue = "NewAllbergShooter";
		private const string NewShooterString = "<-- Ny skytt -->";



		private void FCompetitors_Resize(object sender, EventArgs e)
		{
			//Size size = new Size(this.width, this.height);
			//this.Size = size;
		}
		internal void EnableMe()
		{
			Trace.WriteLine("FCompetitors: EnableMe() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			/*if (Thread.CurrentThread.Name == "Main Thread")
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

			switch(CommonCode.CompetitionCurrent.Type)
			{
				case Structs.CompetitionTypeEnum.Precision:
					this.groupBoxPatrols.Text = "Lag";
					break;
			}

			this.Visible = true;
			this.Focus();
			if (this.ddClubs.Items.Count == 0)
				updatedClubs();
			if (this.ddShooters.Items.Count == 0)
				updatedShooters();
			if (this.ddWeapon1.Items.Count == 0)
				updatedWeapons();
			clearEverything();
			updatedPatrols1();
		}

		//Thread updatedShootersThread;
		private readonly object updatedShootersThreadCreatingLock = new object();
		internal void updatedShooters()
		{
			Trace.WriteLine("FCompetitors: updatedShooters " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			Trace.WriteLine("FCompetitors.updatedShooters() locking " +
				"\"updatedShootersThreadCreatingLock\"  - " +
				Thread.CurrentThread.ManagedThreadId.ToString());
			lock(updatedShootersThreadCreatingLock)
			{
				Trace.WriteLine("FCompetitors.updatedShooters() locked " +
					"\"updatedShootersThreadCreatingLock\"  - " +
					Thread.CurrentThread.ManagedThreadId.ToString());

				/*if (updatedShootersThread == null)
				{
					updatedShootersThread = new Thread(
						new ThreadStart(updatedShootersWithThread));
				}
				else
				{
					updatedShootersThread = new Thread(
						new ThreadStart(updatedShootersWithThread));
				}
				updatedShootersThread.Name = "FCompetitors:updatedShootersThread";
				updatedShootersThread.IsBackground = true;
				updatedShootersThread.Start();*/

				if (this.InvokeRequired)
					this.BeginInvoke(updatedShootersWithThreadMethod);
				else
					updatedShootersWithThreadMethod();

				Trace.WriteLine("FCompetitors.updatedShooters() unlocking " +
					"\"updatedShootersThreadCreatingLock\"  - " +
					Thread.CurrentThread.ManagedThreadId.ToString());
			}
		}
		bool runningBind = false;
		private void FCompetitors_DDShooterBind()
		{
			Trace.WriteLine("FCompetitors: FCompetitors_DDShooterBind " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");
			try
			{
				runningBind = true;
				string selectedValue = FCompetitors_DDShootersGetSelectedValue();
				this.ddShooters.SelectedIndex = -1;
				this.ddShooters.DataSource = this.shooterTable;
				FCompetitors_DDShootersSetSelectedValue(selectedValue);
			}
			catch(Exception exc)
			{
				Trace.WriteLine("FCompetitors: FCompetitors_DDShooterBind " +
					"handled exception: " + exc.ToString());
			}
			finally
			{
				runningBind = false;
			}
		}
		private void FCompetitors_DDShootersSetSelectedValue(string setvalue)
		{
			Trace.WriteLine("FCompetitors: FCompetitors_DDShootersSetSelectedValue " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			this.ddShooters.SelectedValue = setvalue;
		}
		private string FCompetitors_DDShootersGetSelectedValue()
		{
			return (string)this.ddShooters.SelectedValue;
		}

		private void updatedShootersWithThread()
		{
			Trace.WriteLine("FCompetitors: updatedShootersWithThread() started on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			try
			{
				shooters = CommonCode.GetShooters();

				// Create new table
				shooterTable = new DataTable();
				DataColumn colSName = new DataColumn("Name",Type.GetType("System.String"));
				shooterTable.Columns.Add(colSName);

				DataColumn colSId = new DataColumn("Id",Type.GetType("System.String"));
				shooterTable.Columns.Add(colSId);

				// Add default string to table.
				DataRow newRow = shooterTable.NewRow();
				newRow["Name"] = NewShooterString;
				newRow["Id"] = NewShooterValue;
				shooterTable.Rows.Add(newRow);
			
				// Add shooters to table
				foreach(Structs.Shooter shooter in shooters)
				{
					newRow = shooterTable.NewRow();
					newRow["Name"] = shooter.Givenname + ", " +
						shooter.Surname;
					newRow["Id"] = shooter.ShooterId;
					shooterTable.Rows.Add(newRow);
				}

				// Bind dropdown to table
				if (this.InvokeRequired)
					this.Invoke(DDShooterBind); // This goes to the main thread, changing the gui;
				else
					this.FCompetitors_DDShooterBind();
			}
			catch(ThreadAbortException)
			{
				Console.WriteLine(Thread.CurrentThread.Name + 
					" is being canceled.");
			}
			catch(Exception exc)
			{
				Console.WriteLine(Thread.CurrentThread.Name + 
					" is being canceled with Exception\r\n" + exc.ToString());
				throw;
			}
			finally
			{
				Trace.WriteLine("FCompetitors.updatedShootersWithThread() ended.");
			}
		}

		readonly object updatedWeaponsThreadLock = new object();
		//Thread updatedWeaponsThread;
		internal void updatedWeapons()
		{
			Trace.WriteLine("FCompetitors: updatedWeapons() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			Trace.WriteLine("FCompetitors.updatedWeapons() locking " +
				"\"updatedWeaponsThreadLock\"  - " +
				Thread.CurrentThread.ManagedThreadId.ToString());
			lock(updatedWeaponsThreadLock)
			{
				Trace.WriteLine("FCompetitors.updatedWeapons() locked " +
					"\"updatedWeaponsThreadLock\"  - " +
					Thread.CurrentThread.ManagedThreadId.ToString());

				/*if (updatedWeaponsThread == null)
				{
					updatedWeaponsThread = new Thread(
						new ThreadStart(updatedWeaponsWithThread));
				}
				else
				{
					if (updatedWeaponsThread.ThreadState == ThreadState.Running)
					{
						if(updatedShootersThread != null)
							updatedShootersThread.Abort();
					}
					updatedWeaponsThread = new Thread(
						new ThreadStart(updatedWeaponsWithThread));
				}
				updatedWeaponsThread.Name = "FCompetitors:updatedWeaponsThread";
				updatedWeaponsThread.IsBackground = true;
				updatedWeaponsThread.Start();*/

				if (this.InvokeRequired)
					this.BeginInvoke(updatedWeaponsWithThreadMethod);
				else
					updatedWeaponsWithThreadMethod();

				Trace.WriteLine("FCompetitors.updatedWeapons() unlocking " +
					"\"updatedWeaponsThreadLock\"  - " +
					Thread.CurrentThread.ManagedThreadId.ToString());
			}
			Trace.WriteLine("FCompetitors: updatedWeapons() ended.");
		}

		readonly object updatedWeaponsWithThreadLock = new object();
		private void updatedWeaponsWithThread()
		{
			Trace.WriteLine("FCompetitors.updatedWeaponsWithThread() started on thread " +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			if (CommonCode.GetCompetitions().Length == 0)
				return;

			Trace.WriteLine("FCompetitors.updatedWeaponsWithThread() locking " +
				"\"updatedWeaponsWithThreadLock\"  - " +
				Thread.CurrentThread.ManagedThreadId.ToString());
			lock(updatedWeaponsWithThreadLock)
			{
				Trace.WriteLine("FCompetitors.updatedWeaponsWithThread() locked " +
					"\"updatedWeaponsWithThreadLock\"  - " +
					Thread.CurrentThread.ManagedThreadId.ToString());
				try
				{
					weapons = CommonCode.GetWeapons("WeaponId");
			
					createWeaponTable(ref weaponTable1);
					createWeaponTable(ref weaponTable2);
					createWeaponTable(ref weaponTable3);
					createWeaponTable(ref weaponTable4);

					DataRow newRow;
			
					if (weapons.GetUpperBound(0) >-1)
					{
						foreach(Structs.Weapon weapon in weapons)
						{
							try
							{
								newRow = weaponTable1.NewRow();
								newRow["Name"] = weapon.WeaponId;
								newRow["Id"] = weapon.WeaponId;
								weaponTable1.Rows.Add(newRow);

								newRow = weaponTable2.NewRow();
								newRow["Name"] = weapon.WeaponId;
								newRow["Id"] = weapon.WeaponId;
								weaponTable2.Rows.Add(newRow);
				
								newRow = weaponTable3.NewRow();
								newRow["Name"] = weapon.WeaponId;
								newRow["Id"] = weapon.WeaponId;
								weaponTable3.Rows.Add(newRow);

								newRow = weaponTable4.NewRow();
								newRow["Name"] = weapon.WeaponId;
								newRow["Id"] = weapon.WeaponId;
								weaponTable4.Rows.Add(newRow);
							}
							catch(Exception exc)
							{
								Trace.WriteLine("Exception:" + exc.ToString());
							}
						}
					}

					if (InvokeRequired)
						this.Invoke(BindWeapons);
					else
						this.bindWeapons();
				}
				catch(Exception exc)
				{
					Trace.WriteLine("FCompetitors.updatedWeaponsWithThread() Exception " +
						exc.ToString());
					//throw;
				}
				finally
				{
					Trace.WriteLine("FCompetitors.updatedWeaponsWithThread() unlocking " +
						"\"updatedWeaponsWithThreadLock\"  - " +
						Thread.CurrentThread.ManagedThreadId.ToString());
					Trace.WriteLine("FCompetitors.updatedWeaponsWithThread() ended");
				}
			}
		}

		private void createWeaponTable(ref DataTable weaponTableX)
		{
			weaponTableX = new DataTable();

			DataColumn colWName = new DataColumn("Name",Type.GetType("System.String"));
			weaponTableX.Columns.Add(colWName);
			DataColumn colWId = new DataColumn("Id",Type.GetType("System.String"));
			weaponTableX.Columns.Add(colWId);
		}

		private event MethodInvoker BindWeapons;
		private void bindWeapons()
		{
			try
			{
				Trace.WriteLine("FCompetitors: bindWeapons() " +
					"started on thread \"" +
					Thread.CurrentThread.Name + 
					"\" ( " +
					Thread.CurrentThread.ManagedThreadId.ToString() + 
					" )");

				// Display the right things in ddWeapon1
				this.ddWeapon1.DataSource = this.weaponTable1;
				this.ddWeapon1.ValueMember = "Id";
				this.ddWeapon1.DisplayMember = "Name";

				// Display the right things in ddWeapon1
				this.ddWeapon2.DataSource = this.weaponTable2;
				this.ddWeapon2.ValueMember = "Id";
				this.ddWeapon2.DisplayMember = "Name";
			
				// Display the right things in ddWeapon1
				this.ddWeapon3.DataSource = this.weaponTable3;
				this.ddWeapon3.ValueMember = "Id";
				this.ddWeapon3.DisplayMember = "Name";
			
				// Display the right things in ddWeapon1
				this.ddWeapon4.DataSource = this.weaponTable4;
				this.ddWeapon4.ValueMember = "Id";
				this.ddWeapon4.DisplayMember = "Name";
			}
			catch(Exception exc)
			{
				Trace.WriteLine("FCompetitors: bindWeapons: Exception: " + 
					exc.ToString());
			}
			finally
			{
				this.weaponTable1 = null;
				this.weaponTable2 = null;
				this.weaponTable3 = null;
				this.weaponTable4 = null;
				Trace.WriteLine("FCompetitors: bindWeapons() ended.");
			}
		}

		//Thread updatedClubsThread;
		internal void updatedClubs()
		{
			Trace.WriteLine("FCompetitors: updatedClubs() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			/*
			if (updatedClubsThread == null)
			{
				updatedClubsThread = new Thread(
					new ThreadStart(updatedClubsWithThread));
			}
			else
			{
				if (updatedClubsThread.ThreadState == ThreadState.Running)
				{
					updatedClubsThread.Abort();
				}
				updatedClubsThread = new Thread(
					new ThreadStart(updatedClubsWithThread));
			}
			updatedClubsThread.IsBackground = true;
			updatedClubsThread.Name = "FCompetitors:updatedClubsThread";
			updatedClubsThread.Start();*/
			if (this.InvokeRequired)
				this.BeginInvoke(updatedClubsWithThreadMethod);
			else
				updatedClubsWithThreadMethod();
			
			Trace.WriteLine("FCompetitors: updatedClubs ended.");
		}

		private event MethodInvoker BindClubs;
		private void bindClubs()
		{
			Trace.WriteLine("FCompetitors: bindClubs() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			try
			{
				// Display the right things in clubs
				this.ddClubs.DataSource = clubTable;
				this.ddClubs.ValueMember = "Id";
				this.ddClubs.DisplayMember = "Name";
			}
			catch(Exception exc)
			{
				Trace.WriteLine("FCompetitors:bindClubs: Exception: " + 
					exc.ToString());
			}
			finally
			{
				this.clubTable = null;
				Trace.WriteLine("FCompetitors: bindClubs() ended.");
			}
		}

		readonly object updatedClubsWithThreadLock = new object();
		int nrOfThreadsWaitingToUpdateClubs = 0;
		private void updatedClubsWithThread()
		{
			Trace.WriteLine("FCompetitors.updatedClubsWithThread() started on thread " +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			if (nrOfThreadsWaitingToUpdateClubs>1)
				return;
			nrOfThreadsWaitingToUpdateClubs++;
			try
			{
				Trace.WriteLine("FCompetitors.updatedClubsWithThreadLock() locking " +
					"\"updatedClubsWithThreadLock\"  - " +
					Thread.CurrentThread.ManagedThreadId.ToString());

				lock(updatedClubsWithThreadLock)
				{
					Trace.WriteLine("FCompetitors.updatedClubsWithThreadLock() locked " +
						"\"updatedClubsWithThreadLock\"  - " +
						Thread.CurrentThread.ManagedThreadId.ToString());

					clubs = CommonCode.GetClubs();
					clubTable = new DataTable();
					DataColumn colCName = new DataColumn("Name",Type.GetType("System.String"));
					clubTable.Columns.Add(colCName);

					DataColumn colCId = new DataColumn("Id",Type.GetType("System.String"));
					clubTable.Columns.Add(colCId);

					DataRow newRow;
					if (clubs.GetUpperBound(0)>-1)
					{
						foreach(Structs.Club club in clubs)
						{
							newRow = clubTable.NewRow();
							newRow["Name"] = club.Name;
							newRow["Id"] = club.ClubId;
							clubTable.Rows.Add(newRow);
						}
					}
					try
					{
						if (this.InvokeRequired)
							this.Invoke(BindClubs);
						else
							this.bindClubs();
					}
					catch(Exception exc)
					{
						Console.WriteLine(exc.ToString());
					}

					Trace.WriteLine("FCompetitors.updatedClubsWithThreadLock() unlocking " +
						"\"updatedClubsWithThreadLock\"  - " +
						Thread.CurrentThread.ManagedThreadId.ToString());
				}
			}
			finally
			{
				Trace.WriteLine("FCompetitors.updatedClubsWithThread() ended)");
				nrOfThreadsWaitingToUpdateClubs--;
			}
		}

		internal void updatedPatrols()
		{
			Trace.WriteLine("FCompetitors: updatedPatrols() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			if (!this.Visible)
				return;

			if (this.InvokeRequired)
				this.BeginInvoke(updatedPatrolsWithThreadMethod);
			else
				updatedPatrolsWithThreadMethod();
		}
		private void updatedPatrolsWithThread()
		{
			Trace.WriteLine("FCompetitors: updatedPatrolsWithThread() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			Trace.WriteLine("FCompetitors.updatedPatrolsWithThread() locking " +
				"\"SavingCompetitors\"  - " +
				Thread.CurrentThread.ManagedThreadId.ToString());
			lock(SavingCompetitors)
			{
				Trace.WriteLine("FCompetitors.updatedPatrolsWithThread() locked " +
					"\"SavingCompetitors\"  - " +
					Thread.CurrentThread.ManagedThreadId.ToString());

				if (this.chkCompetitor1.Checked)
				{
					updatedPatrols1();
				}

				if (this.chkCompetitor2.Checked)
				{
					updatedPatrols2();
				}
				if (this.chkCompetitor3.Checked)
				{
					updatedPatrols3();
				}
				if (this.chkCompetitor4.Checked)
				{
					updatedPatrols4();
				}

				Trace.WriteLine("FCompetitors.updatedPatrolsWithThread() unlocking " +
					"\"SavingCompetitors\"  - " +
					Thread.CurrentThread.ManagedThreadId.ToString());
			}
		}

		private void updatedPatrols1()
		{
			updatedPatrols1(-1);
		}
		private void updatedPatrols1(int PatrolIdToAlwaysView)
		{
			updatedPatrolsX(PatrolIdToAlwaysView,
				ddWeapon1,
				ddPatrol1,
				patrolTable1,
				0);
		}
		private void updatedPatrols2()
		{
			updatedPatrols2(-1);
		}
		private void updatedPatrols2(int PatrolIdToAlwaysView)
		{
			updatedPatrolsX(PatrolIdToAlwaysView,
				ddWeapon2,
				ddPatrol2,
				patrolTable2,
				1);
		}
		private void updatedPatrols3()
		{
			updatedPatrols3(-1);
		}
		private void updatedPatrols3(int PatrolIdToAlwaysView)
		{
			updatedPatrolsX(PatrolIdToAlwaysView,
				ddWeapon3,
				ddPatrol3,
				patrolTable3,
				2);
		}
		private void updatedPatrols4()
		{
			updatedPatrols4(-1);
		}
		private void updatedPatrols4(int PatrolIdToAlwaysView)
		{
			updatedPatrolsX(PatrolIdToAlwaysView,
				ddWeapon4,
				ddPatrol4,
				patrolTable4,
				3);
		}
		private void updatedPatrolsX(int PatrolIdToAlwaysView,
			Allberg.Shooter.Windows.Forms.SafeComboBox ddWeaponX,
			Allberg.Shooter.Windows.Forms.SafeComboBox ddPatrolX,
			DataTable patrolTableX,
			int compsIndex)
		{
			Trace.WriteLine("FCompetitors: updatedPatrolsX() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			object currentlySelectedPatrol = ddPatrolX.SelectedValue;

			if (ddWeaponX.SelectedValue == null)
			{
				if (ddWeaponX.Items.Count > 0)
				{
					try
					{
						doNotUpdateWeapon = true;
						ddWeaponX.SelectedIndex = 0;
					}
					finally
					{
						doNotUpdateWeapon = true;
					}
				}
				else
				{
					Trace.WriteLine("FCompetitors: updatedPatrols1() " +
						ddWeaponX.Name +
						" contains no weapons.");
					return;
				}
			}
			
			// Get weapons-class
			Structs.Weapon weapon = 
				CommonCode.GetWeapon((string)ddWeaponX.SelectedValue);

			// Get patrols
			patrols = CommonCode.GetPatrols(
				CommonCode.ConvertWeaponsClassToPatrolClass(
					weapon.WClass), true,
					true, 
					PatrolIdToAlwaysView);
			patrolTableX.Clear();

			// Lägg till "tom" patrull
			DataRow newRow = patrolTableX.NewRow();
			newRow["Name"] = "Ej tilldelad";
			newRow["Id"] = "-1";
			patrolTableX.Rows.Add(newRow);

			if (ddShooters.SelectedValue == null)
			{
				updatedShootersWithThread();
			}
			while(ddShooters.SelectedValue == null)
			{
				Thread.Sleep(50);
				try
				{
					ddShooters.SelectedIndex = 0;
				}
				catch(Exception)
				{
				}
			}

			string strShooter = ((string)ddShooters.SelectedValue)
				.Replace("NewAllbergShooter", "-1");
			int shooter = int.Parse(strShooter);

			Structs.Competitor[] comps = CommonCode.GetCompetitors(shooter);

			foreach(Structs.Patrol patrol in patrols)
			{
				bool addRow = false;
				if (comps.Length > 0)
				{
					try
					{
						if (comps[compsIndex].PatrolId == patrol.PatrolId)
							addRow = true;
					}
					catch(System.IndexOutOfRangeException)
					{
					}
				}
				if (addRow == false)
				{
					if (patrol.PClass == Structs.PatrolClass.Okänd |
						patrol.PClass == CommonCode
							.ConvertWeaponsClassToPatrolClass(weapon.WClass)
						)
					{
						addRow = true;
					}
				}

				if (addRow == true)
				{
					string name = patrol.PatrolId.ToString();
					if (patrol.LockedForAutomatic)
						name += "*";
					else
						name += " ";
					name += "(" + patrol.StartDateTimeDisplay.ToShortTimeString() + ")";
					newRow = patrolTableX.NewRow();
					newRow["Name"] = name;
					newRow["Id"] = patrol.PatrolId;
					patrolTableX.Rows.Add(newRow);
				}
			}

			// Try to set same patrol as before
			if (currentlySelectedPatrol != null)
			{
				ddPatrolX.SelectedValue = currentlySelectedPatrol;
				if (ddPatrolX.SelectedValue == null)
					ddPatrolX.SelectedIndex = 0;
			}

			Trace.WriteLine("FCompetitors: updatedPatrolsX() ended.");
		}

		private void checkToEnableShooterClass1()
		{
			Trace.WriteLine("FCompetitors: checkToEnableShooterClass1() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			shooterClassTable1 = new DataTable();

			DataColumn colPName1 = new DataColumn("Name",Type.GetType("System.String"));
			shooterClassTable1.Columns.Add(colPName1);
			DataColumn colPId1 = new DataColumn("Id",Type.GetType("System.Int32"));
			shooterClassTable1.Columns.Add(colPId1);

			checkToEnableShooterClassX(ddShooterClass1, 
				shooterClassTable1,
				ddWeapon1);

			this.ddShooterClass1.DataSource = shooterClassTable1;

			Trace.WriteLine("FCompetitors: checkToEnableShooterClass1() ended.");
		}
		private void checkToEnableShooterClass2()
		{
			Trace.WriteLine("FCompetitors: checkToEnableShooterClass2() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			shooterClassTable2 = new DataTable();

			DataColumn colPName1 = new DataColumn("Name",Type.GetType("System.String"));
			shooterClassTable2.Columns.Add(colPName1);
			DataColumn colPId1 = new DataColumn("Id",Type.GetType("System.Int32"));
			shooterClassTable2.Columns.Add(colPId1);

			checkToEnableShooterClassX(ddShooterClass2, 
				shooterClassTable2,
				ddWeapon2);

			this.ddShooterClass2.DataSource = shooterClassTable2;

			Trace.WriteLine("FCompetitors: checkToEnableShooterClass2() ended.");
		}
		private void checkToEnableShooterClass3()
		{
			Trace.WriteLine("FCompetitors: checkToEnableShooterClass3() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			shooterClassTable3 = new DataTable();

			DataColumn colPName1 = new DataColumn("Name",Type.GetType("System.String"));
			shooterClassTable3.Columns.Add(colPName1);
			DataColumn colPId1 = new DataColumn("Id",Type.GetType("System.Int32"));
			shooterClassTable3.Columns.Add(colPId1);

			checkToEnableShooterClassX(ddShooterClass3, 
				shooterClassTable3,
				ddWeapon3);

			this.ddShooterClass3.DataSource = shooterClassTable3;

			Trace.WriteLine("FCompetitors: checkToEnableShooterClass3() ended.");
		}
		private void checkToEnableShooterClass4()
		{
			Trace.WriteLine("FCompetitors: checkToEnableShooterClass4() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			shooterClassTable4 = new DataTable();

			DataColumn colPName1 = new DataColumn("Name",Type.GetType("System.String"));
			shooterClassTable4.Columns.Add(colPName1);
			DataColumn colPId1 = new DataColumn("Id",Type.GetType("System.Int32"));
			shooterClassTable4.Columns.Add(colPId1);

			checkToEnableShooterClassX(ddShooterClass4, 
				shooterClassTable4,
				ddWeapon4);

			this.ddShooterClass4.DataSource = shooterClassTable4;

			Trace.WriteLine("FCompetitors: checkToEnableShooterClass4() ended.");
		}
		private void checkToEnableShooterClassX(SafeComboBox ddShooterClassX,
			DataTable shooterClassTableX,
			SafeComboBox ddWeaponX)
		{
			Trace.WriteLine("FCompetitors: checkToEnableShooterClassX() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			if (ddShooterClass.SelectedValue == null)
				return; // ops

			Structs.ShootersClass currentClass = 
				(Structs.ShootersClass)int.Parse((string)ddShooterClass.SelectedValue);
			
			string currentClassString = currentClass.ToString().Replace("Klass", "Klass ");
			object currentValue = null;
			try
			{
				currentValue = ddShooterClassX.SelectedValue;
			}
			catch(Exception)
			{
			}
			ddShooterClassX.SelectedIndex = -1;
			shooterClassTableX.Clear();

			Structs.Weapon weapon = new Structs.Weapon(); // This is just a fake to get throw
			if (ddWeaponX.SelectedValue == null)
			{
				if (ddWeaponX.Items.Count > 0)
				{
					ddWeaponX.SelectedIndex = 0;
					return; // This automaticly calls this procedure again
				}
			}
			else
			{
				weapon = CommonCode.GetWeapon((string)ddWeaponX.SelectedValue);
			}

			if (weapon.WClass == Structs.WeaponClass.C)
			{
				DataRow newRow1Default = shooterClassTableX.NewRow();
				newRow1Default["Id"] = 0;
				newRow1Default["Name"] = currentClassString;
				shooterClassTableX.Rows.Add(newRow1Default);

				DataRow newRow1Dam = shooterClassTableX.NewRow();
				newRow1Dam["Id"] = 1;
				newRow1Dam["Name"] = "Dam";
				shooterClassTableX.Rows.Add(newRow1Dam);

				DataRow newRow1Junior = shooterClassTableX.NewRow();
				newRow1Junior["Id"] = 2;
				newRow1Junior["Name"] = "Junior";
				shooterClassTableX.Rows.Add(newRow1Junior);

				DataRow newRow1VY = shooterClassTableX.NewRow();
				newRow1VY["Id"] = 3;
				newRow1VY["Name"] = "VY";
				shooterClassTableX.Rows.Add(newRow1VY);

				DataRow newRow1VA = shooterClassTableX.NewRow();
				newRow1VA["Id"] = 4;
				newRow1VA["Name"] = "VÄ";
				shooterClassTableX.Rows.Add(newRow1VA);

				DataRow newRow1ÖP = shooterClassTableX.NewRow();
				newRow1ÖP["Id"] = 5;
				newRow1ÖP["Name"] = "ÖP";
				shooterClassTableX.Rows.Add(newRow1ÖP);
			}
			else
			{
				DataRow newRow1Default = shooterClassTableX.NewRow();
				newRow1Default["Id"] = 0;
				newRow1Default["Name"] = currentClassString;
				shooterClassTableX.Rows.Add(newRow1Default);

				DataRow newRow1ÖP = shooterClassTableX.NewRow();
				newRow1ÖP["Id"] = 5;
				newRow1ÖP["Name"] = "ÖP";
				shooterClassTableX.Rows.Add(newRow1ÖP);
			}
			
			try
			{
				if (currentValue != null)
					ddShooterClassX.SelectedValue = currentValue;
				else
					ddShooterClassX.SelectedIndex = 0;
			}
			catch(Exception)
			{
				ddShooterClassX.SelectedIndex = 0;
			}
			
			Trace.WriteLine("FCompetitors: checkToEnableShooterClassX() ended.");
		}

		/*private void checkToEnableShooterClass2()
		{
			Trace.WriteLine("FCompetitors: checkToEnableShooterClass2() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			this.ddShooterClass2.Enabled = true;
			string currentClass = (string)this.ddShooterClass.SelectedValue;
			string currentValue = null;
			if (shooterClassTable2.Rows.Count>0)
			{
				try
				{
					currentValue = (string)shooterClassTable2.Rows[0]["Name"];
				}
				catch(Exception)
				{
				}
			}
			this.ddShooterClass2.SelectedIndex = -1;
			shooterClassTable2.Clear();

			Structs.Weapon weapon = CommonCode.GetWeapon((string)this.ddWeapon2.SelectedValue);
			if (weapon.WClass == Structs.WeaponClass.C)
			{
				DataRow newRow2Default = shooterClassTable2.NewRow();
				newRow2Default["Id"] = 0;
				newRow2Default["Name"] = currentClass;
				shooterClassTable2.Rows.Add(newRow2Default);

				DataRow newRow2Dam = shooterClassTable2.NewRow();
				newRow2Dam["Id"] = 1;
				newRow2Dam["Name"] = "Dam";
				shooterClassTable2.Rows.Add(newRow2Dam);

				DataRow newRow2Junior = shooterClassTable2.NewRow();
				newRow2Junior["Id"] = 2;
				newRow2Junior["Name"] = "Junior";
				shooterClassTable2.Rows.Add(newRow2Junior);

				DataRow newRow2VY = shooterClassTable2.NewRow();
				newRow2VY["Id"] = 3;
				newRow2VY["Name"] = "VY";
				shooterClassTable2.Rows.Add(newRow2VY);

				DataRow newRow2VA = shooterClassTable2.NewRow();
				newRow2VA["Id"] = 4;
				newRow2VA["Name"] = "VÄ";
				shooterClassTable2.Rows.Add(newRow2VA);

				DataRow newRow2ÖP = shooterClassTable2.NewRow();
				newRow2ÖP["Id"] = 5;
				newRow2ÖP["Name"] = "ÖP";
				shooterClassTable2.Rows.Add(newRow2ÖP);
			}
			else
			{
				DataRow newRow2Default = shooterClassTable2.NewRow();
				newRow2Default["Id"] = 0;
				newRow2Default["Name"] = currentClass;
				shooterClassTable2.Rows.Add(newRow2Default);

				DataRow newRow2ÖP = shooterClassTable2.NewRow();
				newRow2ÖP["Id"] = 5;
				newRow2ÖP["Name"] = "ÖP";
				shooterClassTable2.Rows.Add(newRow2ÖP);
			}
			
			try
			{
				this.ddShooterClass2.SelectedValue = currentValue;
			}
			catch(Exception)
			{
				this.ddShooterClass2.SelectedIndex = 0;
			}

			Trace.WriteLine("FCompetitors: checkToEnableShooterClass2() ended.");
		}

		private void checkToEnableShooterClass3()
		{
			Trace.WriteLine("FCompetitors: checkToEnableShooterClass3() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			this.ddShooterClass3.Enabled = true;
			string currentClass = (string)this.ddShooterClass.SelectedValue;
			string currentValue = null;
			if (shooterClassTable3.Rows.Count>0)
			{
				try
				{
					currentValue = (string)shooterClassTable3.Rows[0]["Name"];
				}
				catch(Exception)
				{
				}
			}
			this.ddShooterClass3.SelectedIndex = -1;
			shooterClassTable3.Clear();

			Structs.Weapon weapon = CommonCode.GetWeapon((string)this.ddWeapon3.SelectedValue);
			if (weapon.WClass == Structs.WeaponClass.C)
			{
				DataRow newRow3Default = shooterClassTable3.NewRow();
				newRow3Default["Id"] = 0;
				newRow3Default["Name"] = currentClass;
				shooterClassTable3.Rows.Add(newRow3Default);

				DataRow newRow3Dam = shooterClassTable3.NewRow();
				newRow3Dam["Id"] = 1;
				newRow3Dam["Name"] = "Dam";
				shooterClassTable3.Rows.Add(newRow3Dam);

				DataRow newRow3Junior = shooterClassTable3.NewRow();
				newRow3Junior["Id"] = 2;
				newRow3Junior["Name"] = "Junior";
				shooterClassTable3.Rows.Add(newRow3Junior);

				DataRow newRow3VY = shooterClassTable3.NewRow();
				newRow3VY["Id"] = 3;
				newRow3VY["Name"] = "VY";
				shooterClassTable3.Rows.Add(newRow3VY);

				DataRow newRow3VA = shooterClassTable3.NewRow();
				newRow3VA["Id"] = 4;
				newRow3VA["Name"] = "VÄ";
				shooterClassTable3.Rows.Add(newRow3VA);

				DataRow newRow3ÖP = shooterClassTable3.NewRow();
				newRow3ÖP["Id"] = 5;
				newRow3ÖP["Name"] = "ÖP";
				shooterClassTable3.Rows.Add(newRow3ÖP);
			}
			else
			{
				DataRow newRow3Default = shooterClassTable3.NewRow();
				newRow3Default["Id"] = 0;
				newRow3Default["Name"] = currentClass;
				shooterClassTable3.Rows.Add(newRow3Default);

				DataRow newRow3ÖP = shooterClassTable3.NewRow();
				newRow3ÖP["Id"] = 5;
				newRow3ÖP["Name"] = "ÖP";
				shooterClassTable3.Rows.Add(newRow3ÖP);
			}
			
			try
			{
				this.ddShooterClass3.SelectedValue = currentValue;
			}
			catch(Exception)
			{
				this.ddShooterClass3.SelectedIndex = 0;
			}

			Trace.WriteLine("FCompetitors: checkToEnableShooterClass3() ended.");
		}

		private void checkToEnableShooterClass4()
		{
			Trace.WriteLine("FCompetitors: checkToEnableShooterClass4() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			this.ddShooterClass4.Enabled = true;
			string currentClass = (string)this.ddShooterClass.SelectedValue;
			this.ddShooterClass4.SelectedIndex = -1;
			string currentValue = null;
			if (shooterClassTable4.Rows.Count>0)
			{
				try
				{
					currentValue = (string)shooterClassTable4.Rows[0]["Name"];
				}
				catch(Exception)
				{
				}
			}

			shooterClassTable4.Clear();

			Structs.Weapon weapon = CommonCode.GetWeapon((string)this.ddWeapon4.SelectedValue);
			if (weapon.WClass == Structs.WeaponClass.C)
			{
				DataRow newRow4Default = shooterClassTable4.NewRow();
				newRow4Default["Id"] = 0;
				newRow4Default["Name"] = currentClass;
				shooterClassTable4.Rows.Add(newRow4Default);

				DataRow newRow4Dam = shooterClassTable4.NewRow();
				newRow4Dam["Id"] = 1;
				newRow4Dam["Name"] = "Dam";
				shooterClassTable4.Rows.Add(newRow4Dam);

				DataRow newRow4Junior = shooterClassTable4.NewRow();
				newRow4Junior["Id"] = 2;
				newRow4Junior["Name"] = "Junior";
				shooterClassTable4.Rows.Add(newRow4Junior);

				DataRow newRow4VY = shooterClassTable4.NewRow();
				newRow4VY["Id"] = 3;
				newRow4VY["Name"] = "VY";
				shooterClassTable4.Rows.Add(newRow4VY);

				DataRow newRow4VA = shooterClassTable4.NewRow();
				newRow4VA["Id"] = 4;
				newRow4VA["Name"] = "VÄ";
				shooterClassTable4.Rows.Add(newRow4VA);

				DataRow newRow4ÖP = shooterClassTable4.NewRow();
				newRow4ÖP["Id"] = 5;
				newRow4ÖP["Name"] = "ÖP";
				shooterClassTable4.Rows.Add(newRow4ÖP);
			}
			else
			{
				DataRow newRow4Default = shooterClassTable4.NewRow();
				newRow4Default["Id"] = 0;
				newRow4Default["Name"] = currentClass;
				shooterClassTable4.Rows.Add(newRow4Default);

				DataRow newRow4ÖP = shooterClassTable4.NewRow();
				newRow4ÖP["Id"] = 5;
				newRow4ÖP["Name"] = "ÖP";
				shooterClassTable4.Rows.Add(newRow4ÖP);
			}
			
			try
			{
				this.ddShooterClass4.SelectedValue = currentValue;
			}
			catch(Exception)
			{
				this.ddShooterClass4.SelectedIndex = 0;
			}

			Trace.WriteLine("FCompetitors: checkToEnableShooterClass4() ended.");
		}*/

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Trace.WriteLine("FCompetitors: btnCancel_Click() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");

			this.Visible = false;
			this.EnableMain();
		}

		int[] competitorIds = new int[4];
		private void ddShooters_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Trace.WriteLine("FCompetitors: ddShooters_SelectedIndexChanged(sender, e) " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" ) time = 0 ms");
			DateTime start = DateTime.Now;

			if (runningBind)
			{
				Trace.WriteLine("FCompetitors: ddShooters_SelectedIndexChanged " +
					"running bind. Exiting." +
					(DateTime.Now - start).TotalMilliseconds.ToString());
				return;
			}

			Trace.WriteLine("FCompetitors.ddShooters_SelectedIndexChanged(object sender, System.EventArgs e) locking " +
				"\"SavingCompetitors\"  - ThreadId " +
				Thread.CurrentThread.ManagedThreadId.ToString() +
				" - " +
				(DateTime.Now - start).TotalMilliseconds.ToString() +
				" ms.");
			lock(SavingCompetitors)
			{
				Trace.WriteLine("FCompetitors.ddShooters_SelectedIndexChanged(object sender, System.EventArgs e) locked " +
					"\"SavingCompetitors\"  - ThreadId " +
					Thread.CurrentThread.ManagedThreadId.ToString() +
					" - " +
					(DateTime.Now - start).TotalMilliseconds.ToString() +
					" ms.");

				clearEverything(false, false);
				Trace.WriteLine("FCompetitors.ddShooters_SelectedIndexChanged(object sender, System.EventArgs e) back " +
					"from clearEverything " +
					(DateTime.Now - start).TotalMilliseconds.ToString() + 
					" ms.");

				ddShooters_SelectedIndexChanged();

				Trace.WriteLine("FCompetitors.ddShooters_SelectedIndexChanged(" + 
					"object sender, System.EventArgs e) unlocking " +
					"\"SavingCompetitors\"  - " +
					Thread.CurrentThread.ManagedThreadId.ToString() +
					" - " +
					(DateTime.Now - start).TotalMilliseconds.ToString() + 
					" ms.");
			}
			this.btnSave.Enabled = false;
		}

		private void ddShooters_SelectedIndexChanged()
		{
			Trace.WriteLine("FCompetitors: ddShooters_SelectedIndexChanged() " +
				"started on thread \"" +
				Thread.CurrentThread.Name + 
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + 
				" )");
			DateTime start = DateTime.Now;

			try
			{
				if ((string)this.ddShooters.SelectedValue == NewShooterValue)
				{
					// New shooter
					clearEverything();
				}
				else
				{
					// Display shooter
					if (ddShooters.SelectedValue == null)
						return; // Ops

					int shooterId = int.Parse(
						(string)ddShooters.SelectedValue);
					Trace.WriteLine("FCompetitors: ddShooters_SelectedIndexChanged() " +
						"choosen shooter is " + shooterId.ToString());
					Structs.Shooter shooter = 
						CommonCode.GetShooter(shooterId);

					txtEmail.Text = shooter.Email;
					chkArrived.Checked = shooter.Arrived;
					txtGivenName.Text = shooter.Givenname;
					txtPayed.Text = shooter.Payed.ToString();
					if (int.Parse(shooter.CardNr)>0)
						txtCardNr.Text = shooter.CardNr; // This is a real cardnr

					txtSurName.Text = shooter.Surname;
					ddClubs.SelectedValue = shooter.ClubId;
					if (shooter.Class == Structs.ShootersClass.Okänd)
						ddShooterClass.SelectedIndex = 0;
					else
						ddShooterClass.SelectedValue = (int)shooter.Class;

					//int shooterId = shooter.ShooterId;
					Trace.WriteLine("FCompetitors: ddShooters_SelectedIndexChanged() " +
						"displayed shooter took " +
						(DateTime.Now-start).TotalMilliseconds.ToString() + 
						" ms.");

					// Competitor part
					int i=1;
					this.chkCompetitor1.Checked = false;

					Structs.Competitor[] comps = CommonCode.GetCompetitors(shooterId);
					if (comps.Length > 4)
					{
						// Ops
						MessageBox.Show("Denna skytt har fler varv inlagda än vad som stöds av WinShooter.\r\n\r\n" +
							"De överblivna varven kommer nu att automatiskt raderas.", 
							"För många varv", 
							MessageBoxButtons.OK, 
							MessageBoxIcon.Error);
						for (int competitori = 4; competitori < comps.Length; competitori++)
						{
							Structs.Competitor toHighCompetitor = comps[competitori];
							CommonCode.DelCompetitor(toHighCompetitor);
						}
						comps = CommonCode.GetCompetitors(shooterId);
					}
					foreach(Structs.Competitor comp in comps)
					{
						competitorIds[i-1] = comp.CompetitorId;
						int intClass = (int)comp.ShooterClass / 10;
						switch(i)
						{
							case 1:
								this.chkCompetitor1.Checked = true;
								this.ddWeapon1.SelectedValue = comp.WeaponId;
								this.updatedPatrols1(comp.PatrolId);
								this.ddPatrol1.SelectedValue = comp.PatrolId;
								this.ddShooterClass1.SelectedValue = intClass;
								break;
							case 2:
								this.chkCompetitor2.Checked = true;
								this.ddWeapon2.SelectedValue = comp.WeaponId;
								this.updatedPatrols2(comp.PatrolId);
								this.ddPatrol2.SelectedValue = comp.PatrolId;
								this.ddShooterClass2.SelectedValue = intClass;
								break;
							case 3:
								this.chkCompetitor3.Checked = true;
								this.ddWeapon3.SelectedValue = comp.WeaponId;
								this.updatedPatrols3(comp.PatrolId);
								this.ddPatrol3.SelectedValue = comp.PatrolId;
								this.ddShooterClass3.SelectedValue = intClass;
								break;
							case 4:
								this.chkCompetitor4.Checked = true;
								this.ddWeapon4.SelectedValue = comp.WeaponId;
								this.updatedPatrols4(comp.PatrolId);
								this.ddPatrol4.SelectedValue = comp.PatrolId;
								this.ddShooterClass4.SelectedValue = intClass.ToString();
								break;
						}
						Trace.WriteLine("FCompetitors: ddShooters_SelectedIndexChanged() " +
							"displayed competitor " + i.ToString() + " took " +
							(DateTime.Now-start).TotalMilliseconds.ToString() + 
							" ms.");
						i++;
					}
				}
			}
			catch( System.ArgumentNullException exc)
			{
				// Will occur when dropdown isn't populated yet
				Trace.WriteLine(exc.ToString());
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
			finally
			{
				Trace.WriteLine("FCompetitors: ddShooters_SelectedIndexChanged() ended." +
					" after " +
					(DateTime.Now - start).TotalMilliseconds.ToString() + 
					" ms.");
			}
		}

		private void chkCompetitor2_CheckedChanged(object sender, System.EventArgs e)
		{
			this.ddWeapon2.Enabled = this.chkCompetitor2.Checked;
			this.ddPatrol2.Enabled = this.chkCompetitor2.Checked;
			this.ddShooterClass2.Enabled = this.chkCompetitor2.Checked;

			if (!this.chkCompetitor2.Checked)
				return; 
			if ( this.ddWeapon2.SelectedIndex == -1 )
				this.ddWeapon2.SelectedIndex = 0;

			this.updatedPatrols2();
			this.ddPatrol2.SelectedIndex = 0;

			checkToEnableShooterClass2();

			this.btnSave.Enabled = true;
		}

		private void chkCompetitor3_CheckedChanged(object sender, System.EventArgs e)
		{
			this.ddWeapon3.Enabled = this.chkCompetitor3.Checked;
			this.ddPatrol3.Enabled = this.chkCompetitor3.Checked;
			this.ddShooterClass3.Enabled = this.chkCompetitor3.Checked;

			if ( this.ddWeapon3.SelectedIndex == -1 )
				this.ddWeapon3.SelectedIndex = 0;
			this.updatedPatrols3();
			this.ddPatrol3.SelectedIndex = 0;

			checkToEnableShooterClass3();

			this.btnSave.Enabled = true;
		}
		private void chkCompetitor4_CheckedChanged(object sender, System.EventArgs e)
		{
			this.ddWeapon4.Enabled = this.chkCompetitor4.Checked;
			this.ddPatrol4.Enabled = this.chkCompetitor4.Checked;
			this.ddShooterClass4.Enabled = this.chkCompetitor4.Checked;

			if ( this.ddWeapon4.SelectedIndex == -1 )
				this.ddWeapon4.SelectedIndex = 0;

			this.updatedPatrols4();
			this.ddPatrol4.SelectedIndex = 0;

			checkToEnableShooterClass4();

			this.btnSave.Enabled = true;
		}

		readonly object SavingCompetitors = new object();
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			Trace.WriteLine("FCompetitors: Starting btnSave_Click() " +
				"on thread \"" +
				Thread.CurrentThread.Name +
				"\" ( " +
				Thread.CurrentThread.ManagedThreadId + " ) ");

			#region Make sure we have something in patrols and weapons
			if (this.ddPatrol1.Items.Count == 0)
			{
				this.updatedPatrols1();
				while (this.ddPatrol1.Items.Count == 0 & this.chkCompetitor1.Checked)
				{
					Thread.Sleep(100);
				}
				this.ddPatrol1.SelectedIndex = 0;
			}
			if (this.ddPatrol2.Items.Count == 0 & this.chkCompetitor2.Checked )
			{
				this.updatedPatrols2();
				while (this.ddPatrol2.Items.Count == 0)
				{
					Thread.Sleep(100);
				}
				this.ddPatrol2.SelectedIndex = 0;
			}
			if (this.ddPatrol3.Items.Count == 0 & this.chkCompetitor3.Checked)
			{
				this.updatedPatrols3();
				while (this.ddPatrol3.Items.Count == 0)
				{
					Thread.Sleep(100);
				}
				this.ddPatrol3.SelectedIndex = 0;
			}
			if (this.ddPatrol4.Items.Count == 0 & this.chkCompetitor4.Checked)
			{
				this.updatedPatrols4();
				while (this.ddPatrol4.Items.Count == 0)
				{
					Thread.Sleep(100);
				}
				this.ddPatrol4.SelectedIndex = 0;
			}
			if (this.ddWeapon1.SelectedValue == null & 
				this.chkCompetitor1.Checked)
			{
				this.updatedWeapons();
				while(this.ddWeapon1.Items.Count == 0)
				{					
					Thread.Sleep(100);
				}
				this.ddWeapon1.SelectedIndex = 0;
				MessageBox.Show("Vapnet för runda 1 verkar inte vara korrekt");
				return;
			}
			if (this.ddWeapon2.SelectedValue == null & 
				this.chkCompetitor2.Checked)
			{
				this.updatedWeapons();
				while(this.ddWeapon2.Items.Count == 0)
				{					
					Thread.Sleep(100);
				}
				this.ddWeapon2.SelectedIndex = 0;
				MessageBox.Show("Vapnet för runda 2 verkar inte vara korrekt");
				return;
			}
			if (this.ddWeapon3.SelectedValue == null & 
				this.chkCompetitor3.Checked)
			{
				this.updatedWeapons();
				while(this.ddWeapon3.Items.Count == 0)
				{					
					Thread.Sleep(100);
				}
				this.ddWeapon3.SelectedIndex = 0;
				MessageBox.Show("Vapnet för runda 3 verkar inte vara korrekt");
				return;
			}
			if (this.ddWeapon4.SelectedValue == null & 
				this.chkCompetitor4.Checked)
			{
				this.updatedWeapons();
				while(this.ddWeapon4.Items.Count == 0)
				{					
					Thread.Sleep(100);
				}
				this.ddWeapon4.SelectedIndex = 0;
				MessageBox.Show("Vapnet för runda 4 verkar inte vara korrekt");
				return;
			}
			#endregion

			Trace.WriteLine("FCompetitors.updatedShooters() locking " +
				"\"updatedShootersThreadCreatingLock\"  - " +
				Thread.CurrentThread.ManagedThreadId.ToString());
			lock(SavingCompetitors)
			{
				Trace.WriteLine("FCompetitors.updatedShooters() locked " +
					"\"SavingCompetitors\"  - " +
					Thread.CurrentThread.ManagedThreadId.ToString());

				#region Make sure we have something to save

				string cardnr = txtCardNr.Text;

				// Check cardnr
				if (cardnr.Length == 0 & (string)this.ddShooters.SelectedValue == NewShooterValue)
				{
					Trace.WriteLine("FCompetitors: btnSave_Click CardNr missing.");
					DialogResult res =
						MessageBox.Show("Varje skytt har normalt ett Skyttekortsnr. " + 
							"Har du tillgång till skyttekortsnumret?",
							"Inmatningsfel",
							MessageBoxButtons.YesNo,
							MessageBoxIcon.Error);

					if (res == DialogResult.Yes)
					{
						txtCardNr.Focus();
						return;
					}
					else
					{
						// Ok, generate a negative cardnr
						while (cardnr == "")
						{
							Random rnd = new Random();
							string check = (-100 - rnd.Next(10000)).ToString();
							try
							{
								Structs.Shooter shooter = CommonCode.GetShooter(check);
							}
							catch (CannotFindIdException)
							{
								// Ok, Exception occured, this shooter doesn't exist
								cardnr = check;
							}
						}
					}
				}
				try
				{
					if (cardnr != "")
						int.Parse(cardnr);
				}
				catch(System.OverflowException)
				{
					Trace.WriteLine("FCompetitors: Cardnr is to large or to small:" +
						cardnr);
					MessageBox.Show("Kortnr kan endast innehålla heltal " +
						"som är större än 0 och mindre än " + int.MaxValue.ToString() + ".",
						"Inmatningsfel", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					return;
				}
				catch(System.FormatException)
				{
					Trace.WriteLine("FCompetitors: Cardnr contains non integer:\"" +
						cardnr + "\"");
					MessageBox.Show("Kortnr kan endast innehålla heltal " +
						"och inte någon text.",
						"Inmatningsfel", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					return;
				}
				catch(Exception exc)
				{
					Trace.WriteLine("FCompetitors: An exception occured " +
						"while checking Cardnr:" + exc.ToString());
					MessageBox.Show("Ett fel uppstod vid kontroll av kortnr." +
						"Kontrollera att det inte innehåller text och att " + 
						"det är ett heltal mellan 0 och " + 
						int.MaxValue.ToString() + ".",
						"Inmatningsfel", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					return;
				}
				// Check payment
				try
				{
					int.Parse(this.txtPayed.Text);
				}
				catch(Exception)
				{
					Trace.WriteLine(
						"FCompetitors: btnSave_Click Payed contained noninteger");
					MessageBox.Show("Betalat kan endast innehålla heltal " +
						"och inte någon text.",
						"Inmatningsfel", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					return;
				}

				// Check club
				if ((string)this.ddClubs.SelectedValue == null)
				{
					Trace.WriteLine("FCompetitors: btnSave_Click " + 
						"Club was not selected");
					MessageBox.Show("En klubb måste anges.",
						"Inmatningsfel", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					return;
				}
				// Check surname
				if (this.txtSurName.Text == "")
				{
					Trace.WriteLine("FCompetitors: btnSave_Click " + 
						"SurnName was empty");
					MessageBox.Show("Ett förnamn måste anges.",
						"Inmatningsfel", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					this.txtSurName.Focus();
					return;
				}
				// Check givenname
				if (this.txtGivenName.Text == "")
				{
					Trace.WriteLine("FCompetitors: btnSave_Click " + 
						"GivenName was empty");
					MessageBox.Show("Ett efternamn måste anges.",
						"Inmatningsfel", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					this.txtGivenName.Focus();
					return;
				}
				#endregion

				// Ok, save
				try
				{
					if (cardnr != "")
					{
						int.Parse(cardnr);
					}
				}
				catch (FormatException exc)
				{
					throw new FormatException("Could not parse cardnr. Value=\"" +
						cardnr + "\"", exc);
				}
				if ((string)this.ddShooters.SelectedValue == NewShooterValue)
				{
					btnSave_ClickNewShooter(cardnr);
				}
				else
				{
					btnSave_ClickEditShooter(cardnr);
				}
			}
			Trace.WriteLine("FCompetitors: btnSave_Click clearing.");
			clearEverything();
		}

		private void btnSave_ClickNewShooter(string cardnr)
		{
			#region New Shooter
			Trace.WriteLine("FCompetitors: btnSave_Click saving new shooter.");

			// New Shooter
			Structs.Shooter shooter =
				new Structs.Shooter();
			shooter.ClubId = (string)this.ddClubs.SelectedValue;
			shooter.CardNr = cardnr;
			shooter.Email = this.txtEmail.Text;
			shooter.Givenname = this.txtGivenName.Text;
			try
			{
				shooter.Payed = int.Parse(this.txtPayed.Text);
			}
			catch (System.FormatException exc)
			{
				throw new FormatException("Could not parse \"Payed\" as an integer. Value=\"" + 
					this.txtPayed.Text + "\"", exc);
			}
			shooter.ShooterId = -1;
			shooter.Surname = this.txtSurName.Text;
			shooter.ToAutomatic = false;
			shooter.Automatic = false;
			shooter.Arrived = chkArrived.Checked;
			int iClass;
			try
			{
				iClass = int.Parse((string)ddShooterClass.SelectedValue);
			}
			catch (FormatException exc)
			{
				throw new FormatException("Could not parse ddShooterClass for Shooter class. Value=\"" +
					(string)ddShooterClass.SelectedValue, exc);
			}
			shooter.Class = (Structs.ShootersClass)iClass;

			if (CommonCode.EnableInternetConnections & int.Parse(cardnr) > 0)
			{
				DialogResult res =
					MessageBox.Show("Fråga skytten om han vill läggas till " +
					"i Internet-databasen. Vill han/hon det?",
					"Internet-databasen",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);
				if (res == DialogResult.Yes)
					shooter.ToAutomatic = true;
			}

			ArrayList competitors = new ArrayList();

			// Competitor part
			Structs.Competitor comp = new Structs.Competitor();
			if (this.chkCompetitor1.Checked)
			{
				Trace.WriteLine("FCompetitors: btnSave_Click Competitor 1");
				comp = new Structs.Competitor();
				comp.CompetitionId = CommonCode.GetCompetitions()
					[0].CompetitionId;
				//comp.CompetitorId = -1;
				//comp.ShooterId = shooterId;
				comp.WeaponId = (string)ddWeapon1.SelectedValue;
				comp.PatrolId = (int)ddPatrol1.SelectedValue;
				comp.Lane = CommonCode.PatrolGetNextLane(comp.PatrolId);
				comp.ShooterClass = calculateShootersClass(ref this.ddShooterClass, ref this.ddShooterClass1);
				//CommonCode.NewCompetitor(comp);
				//Trace.WriteLine("FCompetitors: btnSave_Click competitor 1 saved");
				competitors.Add(comp);
			}

			if (this.chkCompetitor2.Checked)
			{
				Trace.WriteLine("FCompetitors: btnSave_Click competitor 2");
				comp = new Structs.Competitor();
				comp.CompetitionId = CommonCode.GetCompetitions()
					[0].CompetitionId;
				//comp.CompetitorId = -1;
				//comp.ShooterId = shooterId;
				comp.WeaponId = (string)ddWeapon2.SelectedValue;
				comp.PatrolId = (int)ddPatrol2.SelectedValue;
				comp.Lane = CommonCode.PatrolGetNextLane(comp.PatrolId);
				comp.ShooterClass = calculateShootersClass(ref this.ddShooterClass, ref this.ddShooterClass2);

				//CommonCode.NewCompetitor(comp);
				//Trace.WriteLine("FCompetitors: btnSave_Click competitor 2 saved");
				competitors.Add(comp);
			}

			if (this.chkCompetitor3.Checked)
			{
				Trace.WriteLine("FCompetitors: btnSave_Click competitor 3");
				comp = new Structs.Competitor();
				comp.CompetitionId = CommonCode.GetCompetitions()
					[0].CompetitionId;
				//comp.CompetitorId = -1;
				//comp.ShooterId = shooterId;
				comp.WeaponId = (string)ddWeapon3.SelectedValue;
				comp.PatrolId = (int)ddPatrol3.SelectedValue;
				comp.Lane = CommonCode.PatrolGetNextLane(comp.PatrolId);
				comp.ShooterClass = calculateShootersClass(ref this.ddShooterClass, ref this.ddShooterClass3);

				//CommonCode.NewCompetitor(comp);
				//Trace.WriteLine("FCompetitors: btnSave_Click competotor 3 saved");
				competitors.Add(comp);
			}

			if (this.chkCompetitor4.Checked)
			{
				Trace.WriteLine("FCompetitors: btnSave_Click competitor 4");
				comp = new Structs.Competitor();
				comp.CompetitionId = CommonCode.GetCompetitions()
					[0].CompetitionId;
				//comp.CompetitorId = -1;
				//comp.ShooterId = shooterId;
				comp.WeaponId = (string)ddWeapon4.SelectedValue;
				comp.PatrolId = (int)ddPatrol4.SelectedValue;
				comp.Lane = CommonCode.PatrolGetNextLane(comp.PatrolId);
				comp.ShooterClass = calculateShootersClass(ref this.ddShooterClass, ref this.ddShooterClass4);
				//CommonCode.NewCompetitor(comp);
				competitors.Add(comp);
			}

			// Do actual saving
			Trace.WriteLine("FCompetitors: btnSave_Click Saving shooter...");

			int shooterId = CommonCode.NewShooter(shooter);
			if (shooterId < 1)
			{
				// Oopss...
				Trace.WriteLine("FCompetitors: btnSave_Click " +
					"Failed to save shooter. Returned id was " +
					shooterId);
				MessageBox.Show("Misslyckades med att spara.",
					"Felmeddelande", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}
			Trace.WriteLine("FCompetitors: btnSave_Click New ShooterId is " +
				shooterId.ToString());

			//int i = 0;
			Structs.Competitor[] competitorArray =
				(Structs.Competitor[])competitors.ToArray(comp.GetType());
			for (int i = 0; i < competitorArray.Length; i++)
			{
				Structs.Competitor compsaving = competitorArray[i];
				compsaving.ShooterId = shooterId;
				CommonCode.NewCompetitor(compsaving);
				Trace.WriteLine("FCompetitors: btnSave_Click competitor " + i.ToString() + " saved");
			}

			clearEverything();
			Trace.WriteLine("FCompetitors: btnSave_Click ended");
			#endregion
		}

		private void btnSave_ClickEditShooter(string cardnr)
		{
			#region Edit Shooter
			//edit shooter
			int shooterId = int.Parse((string)this.ddShooters.SelectedValue);

			Structs.Shooter shooter = CommonCode.GetShooter(shooterId);
			if (cardnr != "")
				shooter.CardNr = cardnr;
			shooter.ClubId = (string)this.ddClubs.SelectedValue;
			shooter.Email = this.txtEmail.Text;
			shooter.Givenname = this.txtGivenName.Text;
			shooter.Payed = int.Parse(this.txtPayed.Text);
			shooter.Surname = this.txtSurName.Text;
			shooter.Arrived = chkArrived.Checked;

			int iClass = int.Parse((string)ddShooterClass.SelectedValue);
			shooter.Class = (Structs.ShootersClass)iClass;

			ArrayList updatedCompetitors = new ArrayList();

			// Fixup competitor 1
			if (this.chkCompetitor1.Checked)
			{
				Structs.Competitor comp;
				if (this.competitorIds[0] > -1)
					comp =
						CommonCode.GetCompetitor(this.competitorIds[0]);
				else
				{
					comp = new Structs.Competitor();
					comp.PatrolId = -1;
					comp.Lane = -1;
					comp.CompetitorId = -1;
				}

				comp.WeaponId = (string)this.ddWeapon1.SelectedValue;
				comp.ShooterClass = calculateShootersClass(ref this.ddShooterClass, ref this.ddShooterClass1);
				if (this.ddPatrol1.SelectedIndex == -1)
					comp.PatrolId = -1;
				else
				{
					int newPatrol1 = (int)this.ddPatrol1.SelectedValue;
					if (comp.PatrolId != newPatrol1)
					{
						comp.Lane =
							CommonCode.PatrolGetNextLane(newPatrol1);
						comp.PatrolId = newPatrol1;
					}
				}
				updatedCompetitors.Add(comp);
			}
			else
			{
				// Remove
				if (this.competitorIds[0] > -1)
				{
					removeCompetitor(this.competitorIds[0]);
				}
			}

			// Fixup competitor 2
			if (this.chkCompetitor2.Checked)
			{
				// Add or update
				Structs.Competitor comp;
				if (this.competitorIds[1] > -1)
					comp =
						CommonCode.GetCompetitor(this.competitorIds[1]);
				else
				{
					comp = new Structs.Competitor();
					comp.PatrolId = -1;
					comp.Lane = -1;
					comp.CompetitorId = -1;
				}

				comp.WeaponId = (string)this.ddWeapon2.SelectedValue;
				comp.ShooterClass = calculateShootersClass(ref this.ddShooterClass, ref this.ddShooterClass2);
				if (this.ddPatrol2.SelectedIndex == -1)
					comp.PatrolId = -1;
				else
				{
					int newPatrol2 = (int)this.ddPatrol2.SelectedValue;
					if (comp.PatrolId != newPatrol2)
					{
						comp.Lane =
							CommonCode.PatrolGetNextLane(newPatrol2);
						comp.PatrolId = newPatrol2;
					}
				}
				updatedCompetitors.Add(comp);
			}
			else
			{
				// Remove
				if (this.competitorIds[1] > -1)
				{
					removeCompetitor(this.competitorIds[1]);
				}
			}

			// Fixup competitor 3
			if (this.chkCompetitor3.Checked)
			{
				Structs.Competitor comp;
				if (this.competitorIds[2] > -1)
					comp =
						CommonCode.GetCompetitor(this.competitorIds[2]);
				else
				{
					comp = new Structs.Competitor();
					comp.PatrolId = -1;
					comp.Lane = -1;
					comp.CompetitorId = -1;
				}

				comp.WeaponId = (string)this.ddWeapon3.SelectedValue;
				comp.ShooterClass = calculateShootersClass(ref this.ddShooterClass, ref this.ddShooterClass3);
				if (this.ddPatrol3.SelectedIndex == -1)
					comp.PatrolId = -1;
				else
				{
					int newPatrol3 = (int)this.ddPatrol3.SelectedValue;
					if (comp.PatrolId != newPatrol3)
					{
						comp.Lane =
							CommonCode.PatrolGetNextLane(newPatrol3);
						comp.PatrolId = newPatrol3;
					}
				}
				updatedCompetitors.Add(comp);
			}
			else
			{
				// Remove
				if (this.competitorIds[2] > -1)
				{
					removeCompetitor(this.competitorIds[2]);
				}
			}

			// Fixup competitor 4
			if (this.chkCompetitor4.Checked)
			{
				Structs.Competitor comp;
				if (this.competitorIds[3] > -1)
					comp =
						CommonCode.GetCompetitor(this.competitorIds[3]);
				else
				{
					comp = new Structs.Competitor();
					comp.PatrolId = -1;
					comp.Lane = -1;
					comp.CompetitorId = -1;
				}

				comp.WeaponId = (string)ddWeapon4.SelectedValue;
				comp.ShooterClass = calculateShootersClass(ref this.ddShooterClass, ref this.ddShooterClass4);
				if (this.ddPatrol4.SelectedIndex == -1)
					comp.PatrolId = -1;
				else
				{
					int newPatrol4 = (int)this.ddPatrol4.SelectedValue;
					if (comp.PatrolId != newPatrol4)
					{
						comp.Lane =
							CommonCode.PatrolGetNextLane(newPatrol4);
						comp.PatrolId = newPatrol4;
					}
				}
				updatedCompetitors.Add(comp);
			}
			else
			{
				// Remove
				if (this.competitorIds[3] > -1)
				{
					removeCompetitor(this.competitorIds[3]);
				}
			}

			// UpdateEverything
			Trace.WriteLine("FCompetitors: btnSave_Click saving.");
			CommonCode.UpdateShooter(shooter);
			Structs.Competitor[] compsArray =
				(Structs.Competitor[])updatedCompetitors.ToArray(
				(new Structs.Competitor()).GetType());
			for (int i = 0; i < compsArray.Length; i++)
			{
				Structs.Competitor thisComp = compsArray[i];
				try
				{
					try
					{
						Trace.WriteLine("FCompetitors:btnSave_Click: " +
							"Updating competitor " +
							(i + 1).ToString() + ".");
						CommonCode.UpdateCompetitor(thisComp);
					}
					catch (CannotFindIdException)
					{
						Trace.WriteLine("FCompetitors:btnSave_Click: " +
							"Updating competitor failed for competitor " +
							(i + 1).ToString() + " with " +
							"CannotFindIdException. Saving as " +
							"new competitor.");
						thisComp.CompetitionId =
							CommonCode.GetCompetitions()[0].CompetitionId;
						thisComp.ShooterId = shooterId;
						CommonCode.NewCompetitor(thisComp);
					}
				}
				catch (PatrolAndLaneAlreadyTakenException exc)
				{
					Trace.WriteLine("FCompetitors:btnSave_Click: " +
						"PatrolAndLaneAlreadyTakenException:" +
						exc.ToString());
					MessageBox.Show("Misslyckades med att spara eftersom" +
						"det redan finns en tävlande på denna plats");
				}
			}
			Trace.WriteLine("FCompetitors: btnSave_Click saving done.");
			#endregion
		}

		private void removeCompetitor(int competitorId)
		{
			Structs.Competitor comp =
				CommonCode.GetCompetitor(competitorId);

			// Check results exist
			Structs.CompetitorResult[] results = 
				CommonCode.GetCompetitorResults(competitorId);
			foreach (Structs.CompetitorResult res in results)
			{
				CommonCode.DelCompetitorResult(res);
			}

			// Check if he is part of a team
			Structs.Team[] teams = CommonCode.GetTeams();
			for (int i = 0; i < teams.Length; i++)
			{
				Structs.Team team = teams[i];
				foreach (int competitorTeamId in team.CompetitorIds)
				{
					if (competitorTeamId == comp.CompetitorId)
					{
						team.CompetitorIds.Remove(competitorTeamId);
						CommonCode.UpdateTeam(team);
					}
				}
			}
			CommonCode.DelCompetitor(comp, false);

		}

		private Structs.ShootersClass calculateShootersClass(ref Forms.SafeComboBox ddshooterClass, 
			ref Forms.SafeComboBox ddcompetitorClass)
		{
			int classInt = int.Parse((string)ddshooterClass.SelectedValue);
			classInt = classInt + 10*(int)ddcompetitorClass.SelectedValue;
			Structs.ShootersClass shooterClass =
				(Structs.ShootersClass)classInt;

			while (checkShootersClass(shooterClass) == false)
			{
				classInt -= 1;
				shooterClass = (Structs.ShootersClass)classInt;
			}
			return shooterClass;
		}

		private bool checkShootersClass(Structs.ShootersClass uClass)
		{
			try
			{
				int.Parse(uClass.ToString());
				return false;
			}
			catch(System.FormatException)
			{
				return true;
			}
		}

		private void clearEverything()
		{
			Trace.WriteLine("FCompetitors: clearEverything started");

			clearEverything(true, false);
		}

		private void clearEverything(bool setShooterDD, bool runningBackup)
		{
			try
			{
				Trace.WriteLine("FCompetitors: clearEverything(" + 
					setShooterDD.ToString() + "," + 
					runningBackup.ToString() + ") on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId.ToString() +
					" )");

				this.txtCardNr.Text = "";
				this.txtEmail.Text = "";
				this.txtGivenName.Text = "";
				this.txtPayed.Text ="0";
				this.txtSurName.Text = "";
				this.chkArrived.Checked = false;
			
				if (this.ddClubs.Items.Count > 0)
					this.ddClubs.SelectedIndex = 0;
				if (this.ddShooterClass.Items.Count > 0)
					this.ddShooterClass.SelectedIndex = 0;

				this.ddPatrol1.SelectedValue = -1;
				this.ddPatrol2.SelectedValue = -1;
				this.ddPatrol3.SelectedValue = -1;
				this.ddPatrol4.SelectedValue = -1;

				this.ddWeapon1.SelectedIndex = 0;
				this.ddWeapon2.SelectedIndex = -1;
				this.ddWeapon3.SelectedIndex = -1;
				this.ddWeapon4.SelectedIndex = -1;

				this.chkCompetitor1.Checked = true;
				this.chkCompetitor2.Checked = false;
				this.ddShooterClass2.Enabled = false;

				//this.chkCompetitor3.Enabled = false;
				this.chkCompetitor3.Checked = false;
				this.ddShooterClass3.Enabled = false;

				//this.chkCompetitor4.Enabled = false;
				this.chkCompetitor4.Checked = false;
				this.ddShooterClass4.Enabled = false;

				this.competitorIds = new int[4] {-1,-1,-1,-1};

				if (this.ddShooterClass1.Items.Count>0)
					this.ddShooterClass1.SelectedIndex = 0;
				else
					this.ddShooterClass1.SelectedIndex = -1;
				if (this.ddShooterClass2.Items.Count>0)
					this.ddShooterClass2.SelectedIndex = 0;
				else
					this.ddShooterClass2.SelectedIndex = -1;
				if (this.ddShooterClass3.Items.Count>0)
					this.ddShooterClass3.SelectedIndex = 0;
				else
					this.ddShooterClass3.SelectedIndex = -1;
				if (this.ddShooterClass4.Items.Count>0)
					this.ddShooterClass4.SelectedIndex = 0;
				else
					this.ddShooterClass4.SelectedIndex = -1;

				if (setShooterDD)
				{
					if ((string)this.ddShooters.SelectedValue != NewShooterValue)
						this.ddShooters.SelectedValue = NewShooterValue;
				}

				this.btnSave.Enabled = false;
				checkToEnableShooterClass1();
			}
			catch(Exception exc)
			{
				Trace.WriteLine("FCompetitors.clearEverything Exception: " + exc.ToString());
				if (!runningBackup)
				{
					Thread.Sleep(100);
					clearEverything(setShooterDD, true);
				}
				else
				{
					Trace.WriteLine("FCompetitors: clearEverything Exception: " + 
						exc.ToString());
				}
			}
			finally
			{
				Trace.WriteLine("FCompetitors: clearEverything ended.");
			}
		}

		private void linkFetchShootersAutomatic_LinkClicked(
			object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			// TODO Implement
			MessageBox.Show("Not implemented yet!", 
				"Implementation Status", 
				MessageBoxButtons.OK, 
				MessageBoxIcon.Information);
		}

		private void txtCardNr_CursorLeave(object sender, System.EventArgs e)
		{
			// Check if its a current shooter
			if ((string)ddShooters.SelectedValue != NewShooterValue)
			{
				if ((string)this.ddShooters.SelectedValue == null)
					return;

				int selectedShooterId = int.Parse((string)this.ddShooters.SelectedValue);
				Structs.Shooter selectedShooter = new Structs.Shooter();
				try
				{
					selectedShooter = CommonCode.GetShooter(selectedShooterId);
				}
				catch (CannotFindIdException)
				{	return;
				}

				// Current Shooter
				if (selectedShooter.CardNr == this.txtCardNr.Text)
				{
					// Ok, nothings changed.
					return;
				}
				DialogResult res = MessageBox.Show("Vill du ändra " +
					"pistolkortet för denna skytt? \r\n\r\n(" +
					"Om du svarar nej så kommer du att försöka öppna en annan " +
					"skytt med detta nummer)", 
					"Fråga", 
					MessageBoxButtons.YesNo, 
					MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
				if (res == DialogResult.No |
					res == DialogResult.Cancel)
				{
					// Ok, retrieve shooter
					try
					{
						selectedShooter = CommonCode.GetShooter(this.txtCardNr.Text);
						this.ddShooters.SelectedValue = selectedShooter.ShooterId;
					}
					catch (CannotFindIdException)
					{
						// Shooter didn't exist in current db
						string cardnr = this.txtCardNr.Text;
						this.ddShooters.SelectedIndex = 0;
						try
						{
							selectedShooter = CommonCode.GetShooterFromCache(cardnr);
							this.txtSurName.Text = selectedShooter.Surname;
							this.txtGivenName.Text = selectedShooter.Givenname;
							this.txtEmail.Text = selectedShooter.Email;
							this.ddShooterClass.SelectedValue = (int)selectedShooter.Class;
							this.ddClubs.SelectedValue = selectedShooter.ClubId;
						}
						catch (CannotFindIdException)
						{
							// Person didn't exit in cache
						}
					}

				}
			}
			else
			{
				// New Shooter
				try
				{
					// Check if shooter already exist in db
					Structs.Shooter shooter = CommonCode.GetShooter(this.txtCardNr.Text);
					// Ok, person exists, select it and retreive other values
					ddShooters.SelectedValue = shooter.ShooterId;
					ddShooters_SelectedIndexChanged();
					return;
				}
				catch (CannotFindIdException)
				{
					// We get an CannotFindIdException if person does not exist
					// in db. This is OK.
				}

				// Ok, person does not exist in Db. How about embedded resources?
				try
				{
					Structs.Shooter shooter =
						CommonCode.GetShooterFromCache(txtCardNr.Text);

					// Ok, we've got a shooter since no exception. View him
					txtSurName.Text = shooter.Surname;
					txtGivenName.Text = shooter.Givenname;
					txtEmail.Text = shooter.Email;
					ddShooterClass.SelectedValue = (int)shooter.Class;
					ddClubs.SelectedValue = shooter.ClubId;
				}
				catch (CannotFindIdException)
				{
					// Shooter didn't exist in Cache
					return;
				}
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			Trace.WriteLine("FCompetitors: btnDelete_Click started on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() +
				" )");

			Structs.Shooter shooter = new Structs.Shooter();
			DialogResult res =
				MessageBox.Show("Är du säker på att du vill radera både " +
				"resultat och skytt?", 
				"Felmeddelande", 
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question);
			if (res == DialogResult.Yes)
			{
				try
				{
					if ((string)this.ddShooters.SelectedValue != NewShooterValue)
					{
						shooter = 
							CommonCode.GetShooter(int.Parse((string)this.ddShooters.SelectedValue));

						foreach (Structs.Competitor comp in 
							CommonCode.GetCompetitors(shooter.ShooterId))
						{
							foreach (Structs.CompetitorResult result in 
								CommonCode.GetCompetitorResults(comp.CompetitorId))
							{
								CommonCode.DelCompetitorResult(result);
							}
							CommonCode.DelCompetitor(comp);
						}
						CommonCode.DelShooter(shooter);
						clearEverything();
					}
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.ToString());
					throw;
				}
			}
		}

		bool doNotUpdateWeapon = false;
		private void ddWeapon1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!doNotUpdateWeapon)
				{
					this.updatedPatrols1();
				}
				checkToEnableShooterClass1();
				this.btnSave.Enabled = true;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Competitors.ddWeapon1_SelectedIndexChanged Exception: " + 
					exc.ToString());
			}
		}

		private void ddWeapon2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!doNotUpdateWeapon)
					this.updatedPatrols2();
				checkToEnableShooterClass2();
				this.btnSave.Enabled = true;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Competitors.ddWeapon2_SelectedIndexChanged Exception: " + 
					exc.ToString());
			}
		}

		private void ddWeapon3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!doNotUpdateWeapon)
					this.updatedPatrols3();
				checkToEnableShooterClass3();
				this.btnSave.Enabled = true;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Competitors.ddWeapon3_SelectedIndexChanged Exception: " + 
					exc.ToString());
			}
		}

		private void ddWeapon4_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!doNotUpdateWeapon)
					this.updatedPatrols4();
				checkToEnableShooterClass4();
				this.btnSave.Enabled = true;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Competitors.ddWeapon4_SelectedIndexChanged Exception: " + 
					exc.ToString());
			}
		}

		private void ddShooterClass_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.updatedShooterClass();
			this.btnSave.Enabled = true;
		}

		private void updatedShooterClass()
		{
			if (this.shooterClassTable1.Rows.Count > 0)
			{
				this.shooterClassTable1.Rows[0]["Name"] = "Klass " + this.ddShooterClass.SelectedValue;
			}
			if (this.shooterClassTable2.Rows.Count > 0)
			{
				this.shooterClassTable2.Rows[0]["Name"] = "Klass " + this.ddShooterClass.SelectedValue;
			}
			if (this.shooterClassTable3.Rows.Count > 0)
			{
				this.shooterClassTable3.Rows[0]["Name"] = "Klass " + this.ddShooterClass.SelectedValue;
			}
			if (this.shooterClassTable4.Rows.Count > 0)
			{
				this.shooterClassTable4.Rows[0]["Name"] = "Klass " + this.ddShooterClass.SelectedValue;
			}
		}

		private void txtSurName_TextChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void txtGivenName_TextChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void txtEmail_TextChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void ddClubs_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void txtPayed_TextChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void chkCompetitor1_CheckedChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void ddPatrol1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void ddPatrol2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void ddPatrol3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void ddPatrol4_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void ddShooterClass1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void ddShooterClass2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void ddShooterClass3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void ddShooterClass4_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void txtCardNr_TextChanged(object sender, System.EventArgs e)
		{
			this.btnSave.Enabled = true;
		}

		private void ddWeapon1_MouseUp(object sender, MouseEventArgs e)
		{
			this.doNotUpdateWeapon = false;
		}

		private void ddWeapon2_MouseUp(object sender, MouseEventArgs e)
		{
			this.doNotUpdateWeapon = false;
		}

		private void ddWeapon3_MouseUp(object sender, MouseEventArgs e)
		{
			this.doNotUpdateWeapon = false;
		}

		private void ddWeapon4_MouseUp(object sender, MouseEventArgs e)
		{
			this.doNotUpdateWeapon = false;
		}

		private void chkArrived_CheckedChanged(object sender, EventArgs e)
		{
			btnSave.Enabled = true;
		}

	}
}
