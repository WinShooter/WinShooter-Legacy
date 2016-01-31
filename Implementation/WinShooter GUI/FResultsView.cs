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
// $Id: FResultsView.cs 130 2011-05-28 17:32:36Z smuda $ 
#define VisualStudio

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Allberg.Shooter.Windows.Forms;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for FResultsView.
	/// </summary>
	public class FResultsView : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal FResultsView(ref Common.Interface newCommon)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CommonCode = newCommon;

			Trace.WriteLine("FResultsView: Creating");
			try
			{
				height = this.Size.Height;
				width = this.Size.Width;

				this.DatasetBindToDataGrid += 
					new DatasetBindToDataGridHandler(this.datasetBindToDataGrid);
				BindDDWClasses += new MethodInvoker(FResultsView_BindDDWClasses);
				BindDDUClasses += new MethodInvoker(FResultsView_BindDDUClasses);
			}
			catch(Exception exc)
			{
				Trace.WriteLine("FResultsView: Exception" + exc.ToString());
				throw;
			}
			finally
			{
				Trace.WriteLine("FResultsView: Created");
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			Trace.WriteLine("FResultsView: Dispose(" + disposing.ToString() + ")" +
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

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );

			Trace.WriteLine("FResultsView: Dispose(" + disposing.ToString() + ")" +
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FResultsView));
			this.dataGrid1 = new Allberg.Shooter.Windows.Forms.DataGridCustom();
			this.datasetResults1 = new Allberg.Shooter.Windows.DatasetResults();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.dataGridPlace = new Allberg.Shooter.Windows.Forms.DataGridTextBoxColumnCustom();
			this.dataGridShooterName = new Allberg.Shooter.Windows.Forms.DataGridTextBoxColumnCustom();
			this.dataGridClubname = new Allberg.Shooter.Windows.Forms.DataGridTextBoxColumnCustom();
			this.dataGridHits = new Allberg.Shooter.Windows.Forms.DataGridTextBoxColumnCustom();
			this.dataGridMedal = new Allberg.Shooter.Windows.Forms.DataGridTextBoxColumnCustom();
			this.dataGridPoints = new Allberg.Shooter.Windows.Forms.DataGridTextBoxColumnCustom();
			this.dataGridHitsPerStations = new Allberg.Shooter.Windows.Forms.DataGridTextBoxColumnCustom();
			this.label1 = new System.Windows.Forms.Label();
			this.ddWClasses = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ddUClasses = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.btnPrint = new SafeButton();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.datasetResults1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGrid1
			// 
			this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGrid1.DataMember = "Results";
			this.dataGrid1.DataSource = this.datasetResults1;
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(8, 32);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.ReadOnly = true;
			this.dataGrid1.Size = new System.Drawing.Size(584, 224);
			this.dataGrid1.TabIndex = 0;
			this.dataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
			this.dataGridTableStyle1});
			// 
			// datasetResults1
			// 
			this.datasetResults1.DataSetName = "DatasetResults";
			this.datasetResults1.Locale = new System.Globalization.CultureInfo("en-US");
			this.datasetResults1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.dataGrid1;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
			this.dataGridPlace,
			this.dataGridShooterName,
			this.dataGridClubname,
			this.dataGridHits,
			this.dataGridMedal,
			this.dataGridPoints,
			this.dataGridHitsPerStations});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "Results";
			this.dataGridTableStyle1.ReadOnly = true;
			this.dataGridTableStyle1.RowHeadersVisible = false;
			// 
			// dataGridPlace
			// 
			this.dataGridPlace.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.dataGridPlace.Format = "";
			this.dataGridPlace.FormatInfo = null;
			this.dataGridPlace.HeaderText = "Plats";
			this.dataGridPlace.MappingName = "Place";
			this.dataGridPlace.ReadOnly = true;
			this.dataGridPlace.Width = 35;
			// 
			// dataGridShooterName
			// 
			this.dataGridShooterName.Format = "";
			this.dataGridShooterName.FormatInfo = null;
			this.dataGridShooterName.HeaderText = "Skytt";
			this.dataGridShooterName.MappingName = "ShooterName";
			this.dataGridShooterName.ReadOnly = true;
			this.dataGridShooterName.Width = 150;
			// 
			// dataGridClubname
			// 
			this.dataGridClubname.Format = "";
			this.dataGridClubname.FormatInfo = null;
			this.dataGridClubname.HeaderText = "Klubb";
			this.dataGridClubname.MappingName = "Club";
			this.dataGridClubname.ReadOnly = true;
			this.dataGridClubname.Width = 125;
			// 
			// dataGridHits
			// 
			this.dataGridHits.Format = "";
			this.dataGridHits.FormatInfo = null;
			this.dataGridHits.HeaderText = "Resultat";
			this.dataGridHits.MappingName = "Hits";
			this.dataGridHits.ReadOnly = true;
			this.dataGridHits.Width = 40;
			// 
			// dataGridMedal
			// 
			this.dataGridMedal.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.dataGridMedal.Format = "";
			this.dataGridMedal.FormatInfo = null;
			this.dataGridMedal.HeaderText = "Stm";
			this.dataGridMedal.MappingName = "Medal";
			this.dataGridMedal.ReadOnly = true;
			this.dataGridMedal.Width = 40;
			// 
			// dataGridPoints
			// 
			this.dataGridPoints.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.dataGridPoints.Format = "";
			this.dataGridPoints.FormatInfo = null;
			this.dataGridPoints.HeaderText = "Poäng";
			this.dataGridPoints.MappingName = "Points";
			this.dataGridPoints.ReadOnly = true;
			this.dataGridPoints.Width = 40;
			// 
			// dataGridHitsPerStations
			// 
			this.dataGridHitsPerStations.Format = "";
			this.dataGridHitsPerStations.FormatInfo = null;
			this.dataGridHitsPerStations.HeaderText = "Per station";
			this.dataGridHitsPerStations.MappingName = "HitsPerStation";
			this.dataGridHitsPerStations.ReadOnly = true;
			this.dataGridHitsPerStations.Width = 150;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Vapengrupp";
			// 
			// ddWClasses
			// 
			this.ddWClasses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddWClasses.Location = new System.Drawing.Point(76, 8);
			this.ddWClasses.Name = "ddWClasses";
			this.ddWClasses.Size = new System.Drawing.Size(121, 21);
			this.ddWClasses.TabIndex = 2;
			this.ddWClasses.SelectedIndexChanged += new System.EventHandler(this.ddWClasses_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(204, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Skytteklass";
			// 
			// ddUClasses
			// 
			this.ddUClasses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddUClasses.Location = new System.Drawing.Point(268, 8);
			this.ddUClasses.Name = "ddUClasses";
			this.ddUClasses.Size = new System.Drawing.Size(121, 21);
			this.ddUClasses.TabIndex = 4;
			this.ddUClasses.SelectedIndexChanged += new System.EventHandler(this.ddUClasses_SelectedIndexChanged);
			// 
			// btnPrint
			// 
			this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrint.Location = new System.Drawing.Point(520, 8);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(75, 23);
			this.btnPrint.TabIndex = 5;
			this.btnPrint.Text = "Skriv ut";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// FResultsView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(600, 262);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.ddUClasses);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ddWClasses);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dataGrid1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FResultsView";
			this.Text = "Resultatsvisning";
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.datasetResults1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		internal bool DisposeNow = false;
		public delegate void EnableMainHandler();
		public event EnableMainHandler EnableMain;
		private delegate void DatasetBindToDataGridHandler();
		private event DatasetBindToDataGridHandler DatasetBindToDataGrid;
		private event MethodInvoker BindDDWClasses;
		private event MethodInvoker BindDDUClasses;

		Common.Interface CommonCode;
		int height;
		//private System.Windows.Forms.DataGrid dataGrid1;
		private Allberg.Shooter.Windows.Forms.DataGridCustom dataGrid1;
		private Allberg.Shooter.Windows.DatasetResults datasetResults1;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private DataGridTextBoxColumnCustom dataGridPlace;
		private DataGridTextBoxColumnCustom dataGridShooterName;
		private DataGridTextBoxColumnCustom dataGridClubname;
		private DataGridTextBoxColumnCustom dataGridHits;
		private DataGridTextBoxColumnCustom dataGridMedal;
		private DataGridTextBoxColumnCustom dataGridPoints;
		private DataGridTextBoxColumnCustom dataGridHitsPerStations;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private SafeButton btnPrint;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddWClasses;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddUClasses;
		int width;

		internal void UpdatedCompetition()
		{
			Trace.WriteLine("FResultsView.UpdatedCompetition: Started.");

			Structs.Competition[] comps = CommonCode.GetCompetitions();
			if (comps.Length == 0)
				return;

			Structs.Competition comp = comps[0];
			switch(comp.Type)
			{
				case Structs.CompetitionTypeEnum.Precision:
				{
					dataGridPoints.Width = 0;
					dataGridPoints.HeaderText = "";
					break;
				}
				case Structs.CompetitionTypeEnum.Field:
				{
					break;
				}
				case Structs.CompetitionTypeEnum.MagnumField:
				{
					break;
				}
				default:
					throw new ApplicationException("Cannot find CompetitionType " + comp.Type.ToString());
			}
			Trace.WriteLine("FResultsView.UpdatedCompetition: Ended.");
		}
		internal void UpdatedCompetitors()
		{
			Trace.WriteLine("FResultsView.UpdatedCompetitors: Started.");
			if (this.Visible)
				updateDdWClasses();
			Trace.WriteLine("FResultsView.UpdatedCompetitors: Ended.");
		}

		internal void UpdatedCompetitorResults()
		{
			Trace.WriteLine("FResultsView.UpdatedCompetitorResults: Started.");
			if (this.Visible)
				updateDdWClasses();
			Trace.WriteLine("FResultsView.UpdatedCompetitorResults: Ended.");
		}

		internal void enableMe()
		{
			Trace.WriteLine("FResultsView.UpdatedCompetitorResults: Started.");
			this.Visible = true;
			this.Focus();

			switch (CommonCode.CompetitionCurrent.Type)
			{
				case Structs.CompetitionTypeEnum.Precision:
					dataGridHitsPerStations.HeaderText = "Per serie";
					break;
			}

			updateDdWClasses();
		}

		#region update ddWClasses
		private void updateDdWClasses()
		{
			Trace.WriteLine("FResultsView.updateDdWClasses " +
				"started on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			Thread ddWClassesChangedThread =
				new Thread(
				new ThreadStart(
				this.updateDdWClassesWithThread));
			ddWClassesChangedThread.Name = "updateDdWClassesThread";
			ddWClassesChangedThread.IsBackground = true;
			ddWClassesChangedThread.Start();

			Trace.WriteLine("FResultsView.updateDdWClasses ended");
		}

		int UpdateDdWClassesInThreadsCount = 0;
		DataTable tableWClasses;
		private void updateDdWClassesWithThread()
		{
			Trace.WriteLine("FResultsView.updateDdWClassesWithThread() " +
				"started on thread " +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			try
			{
				if (UpdateDdWClassesInThreadsCount>1)
				{
					Trace.WriteLine("FResultsView.updateDdWClassesWithThread() " +
						" already has threads waiting");
					return;
				}

				Interlocked.Increment(ref UpdateDdWClassesInThreadsCount);
				Trace.WriteLine("FResultsView.updateDdWClassesWithThread() locking \"" +
					"lockObject\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId.ToString() + " )");
				lock(lockObject)
				{
					Trace.WriteLine("FResultsView.updateDdWClassesWithThread() " +
						"locked \"lockObject\" on thread \"" +
						Thread.CurrentThread.Name + "\" ( " +
						Thread.CurrentThread.ManagedThreadId.ToString() + " )");

					Structs.ResultWeaponsClass[] rwclasses =
						CommonCode.ResultsGetWClasses();

					tableWClasses = new DataTable();
					DataColumn colWName1 = new DataColumn("Name",typeof(string));
					tableWClasses.Columns.Add(colWName1);
					DataColumn colWId1 = new DataColumn("Id",typeof(int));
					tableWClasses.Columns.Add(colWId1);

					foreach(Structs.ResultWeaponsClass rwclass in rwclasses)
					{
						tableWClasses.Rows.Add(
							new object[] {
											 rwclass.ToString(),
											 (int)rwclass
										 });
					}
					if (this.InvokeRequired)
						this.Invoke(BindDDWClasses);
					else
						BindDDWClasses();
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine("FResultsView.updateDdWClassesWithThread() "+
					"Exception: \r\n" + exc.ToString());
			}
			finally
			{
				Interlocked.Decrement(ref UpdateDdWClassesInThreadsCount);
				Trace.WriteLine("FResultsView.populateDropDownUserInThread() ended");
			}
		}
		private void FResultsView_BindDDWClasses()
		{
			object selectedValue = this.ddWClasses.SelectedValue;
			this.ddWClasses.DataSource = this.tableWClasses;
			this.ddWClasses.DisplayMember = "Name";
			this.ddWClasses.ValueMember = "Id";
			if (selectedValue != null)
				this.ddWClasses.SelectedValue = selectedValue;
			else
			{
				if (this.tableWClasses.Rows.Count > 0)
					this.ddWClasses.SelectedIndex = 0;
			}
		}
		#endregion

		#region update ddUClasses
		private void ddWClasses_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Trace.WriteLine("FResultsView.ddWClasses_SelectedIndexChanged " +
				"started on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			Thread ddUClassesChangedThread =
				new Thread(
				new ThreadStart(
				this.updateDdUClassesWithThread));
			ddUClassesChangedThread.Name = "updateDdUClassesThread";
			ddUClassesChangedThread.IsBackground = true;
			ddUClassesChangedThread.Start();

			Trace.WriteLine("FResultsView.ddWClasses_SelectedIndexChanged ended");
		}

		int UpdateDdUClassesInThreadsCount = 0;
		private void updateDdUClassesWithThread()
		{
			Trace.WriteLine("FResultsView.updateDdUClassesWithThread() " +
				"started on thread " +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			try
			{
				if (UpdateDdUClassesInThreadsCount>0)
				{
					Trace.WriteLine("FResultsView.updateDdUClassesWithThread() " +
						" already has threads waiting");
					return;
				}

				Interlocked.Increment(ref UpdateDdUClassesInThreadsCount);
				Trace.WriteLine("FResultsView.updateDdUClassesWithThread() locking \"" +
					"lockObject\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId.ToString() + " )");
				lock(lockObject)
				{
					Trace.WriteLine("FResultsView.updateDdUClassesWithThread() " +
						"locked \"lockObject\" on thread \"" +
						Thread.CurrentThread.Name + "\" ( " +
						Thread.CurrentThread.ManagedThreadId.ToString() + " )");

					Structs.ResultWeaponsClass wclass = this.getCurrentWeaponClass();

					Structs.ShootersClass[] uclasses =
						CommonCode.ResultsGetUClasses(wclass);

					tableUClasses = new DataTable();
					DataColumn colUName1 = new DataColumn("Name",typeof(string));
					tableUClasses.Columns.Add(colUName1);
					DataColumn colUId1 = new DataColumn("Id",typeof(int));
					tableUClasses.Columns.Add(colUId1);

					foreach(Structs.ShootersClass uclass in uclasses)
					{
						tableUClasses.Rows.Add(
							new object[] {
											 uclass.ToString(),
											 (int)uclass
										 });
					}
					if (this.InvokeRequired)
						this.Invoke(BindDDUClasses);
					else
						BindDDUClasses();
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine("FResultsView.updateDdUClassesWithThread() "+
					"Exception: \r\n" + exc.ToString());
			}
			finally
			{
				Interlocked.Decrement(ref UpdateDdUClassesInThreadsCount);
				Trace.WriteLine("FResultsView.updateDdUClassesWithThread() ended");
			}
		}

		DataTable tableUClasses;
		private void FResultsView_BindDDUClasses()
		{
			object selectedValue = this.ddUClasses.SelectedValue;
			this.ddUClasses.DataSource = this.tableUClasses;
			this.ddUClasses.DisplayMember = "Name";
			this.ddUClasses.ValueMember = "Id";
			if (selectedValue != null)
				this.ddUClasses.SelectedValue = selectedValue;
			else
			{
				if (this.tableUClasses.Rows.Count > 0)
					this.ddUClasses.SelectedIndex = 0;
			}
		}
		#endregion

		#region Helpers
		private Structs.ResultWeaponsClass getCurrentWeaponClass()
		{
			try
			{
				if (this.ddWClasses.SelectedIndex == -1)
					return Structs.ResultWeaponsClass.Unknown;




				int selectedValue =  (int)this.ddWClasses.SelectedValue;
				Structs.ResultWeaponsClass thisClass =
					(Structs.ResultWeaponsClass)selectedValue;

				return thisClass;
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine("FResultsView.getCurrentWeaponClass " +
					"Exception:\r\n" + exc.ToString());
				throw;
			}
		}
		private Structs.ShootersClass getCurrentShootersClass()
		{
			try
			{
				if (this.ddUClasses.SelectedIndex == -1)
					return Structs.ShootersClass.Okänd;

				int selectedValue = (int)this.ddUClasses.SelectedValue;
				Structs.ShootersClass thisClass =
					(Structs.ShootersClass)selectedValue;

				return thisClass;
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine("FResultsView.getCurrentShootersClass " +
					"Exception:\r\n" + exc.ToString());
				throw;
			}
		}
		#endregion

		#region Display Results
		private void ddUClasses_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Trace.WriteLine("FResultsView.ddUClasses_SelectedIndexChanged " +
				"started on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			Thread ddUClassesChangedThread =
				new Thread(
				new ThreadStart(
				this.updateDatasetwithThread));
			ddUClassesChangedThread.Name = "updateDataGridThread";
			ddUClassesChangedThread.IsBackground = true;
			ddUClassesChangedThread.Start();

			Trace.WriteLine("FResultsView.ddUClasses_SelectedIndexChanged ended");
		}

		int populateDatasetwithThreadsCount = 0;
		private void updateDatasetwithThread()
		{
			try
			{
				Trace.WriteLine("FResultsView.populateDatasetwithThread() " + 
					"started on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId.ToString() + " )");
				
				if (populateDatasetwithThreadsCount>0)
				{
					Trace.WriteLine("FResultsView.populateDatasetwithThread() " +
						" already has threads waiting");
					return;
				}
				Interlocked.Increment(ref populateDatasetwithThreadsCount);

				Trace.WriteLine("FResultsView.populateDatasetwithThread() locking \"" +
					"lockObject\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId.ToString() + " )");
				lock(lockObject)
				{
					Trace.WriteLine("FResultsView.populateDatasetwithThread() locked \"" +
						"lockObject\" on thread \"" +
						Thread.CurrentThread.Name + "\" ( " +
						Thread.CurrentThread.ManagedThreadId.ToString() + " )");

					Thread.Sleep(100);
					DateTime start = DateTime.Now;

					if (CommonCode.GetCompetitions().Length == 0)
						return;

					this.datasetResults1 = new DatasetResults();
					this.datasetResults1.DataSetName = "DatasetResults";
					this.datasetResults1.Locale = 
						new System.Globalization.CultureInfo("sv");

					Structs.ResultWeaponsClass wclass = getCurrentWeaponClass();
					Structs.ShootersClass uclass = getCurrentShootersClass();
					if (uclass == Structs.ShootersClass.Okänd)
						return;

					ResultsReturn[] results = CommonCode.ResultsGet(wclass, 
						uclass, 
						CommonCode.GetCompetitions()[0],
						false);

					datasetResults1 = new Allberg.Shooter.Windows.DatasetResults();

					int i = 1;
					bool useNorwegian = CommonCode.GetCompetitions()[0].NorwegianCount;
					Hashtable clubs = new Hashtable();
					foreach(ResultsReturn result in results)
					{
						DatasetResults.ResultsRow newRow =
							this.datasetResults1.Results.NewResultsRow();

						newRow.Place = i;
						if (clubs.ContainsKey(result.ClubId))
							newRow.Club = (string)clubs[result.ClubId];
						else
						{
							string clubName = getClubName(result.ClubId);
							newRow.Club = clubName;
							clubs.Add(result.ClubId, clubName);
						}
						switch((Structs.Medal)result.Medal)
						{
							case Structs.Medal.StandardSilver:
								newRow.Medal = "S";
								break;
							case Structs.Medal.StardardBrons:
								newRow.Medal = "B";
								break;
							default:
								newRow.Medal = "";
								break;
						}
						newRow.Points = result.PointsTotal;
						newRow.ShooterName = result.ShooterName;
						switch(CommonCode.GetCompetitions()[0].Type)
						{
							case Structs.CompetitionTypeEnum.Field:
							{
								if (useNorwegian)
								{
									newRow.Hits = (result.HitsTotal + result.FigureHitsTotal).ToString();
									newRow.HitsPerStation = "";
									foreach(string str in result.HitsPerStnString.Split(';'))
									{
										if (str != "")
										{
											newRow.HitsPerStation += str + " ";
										}
									}
									newRow.HitsPerStation = newRow.HitsPerStation.TrimEnd();
								}
								else
								{
									newRow.Hits = result.HitsTotal.ToString() + " / " + 
										result.FigureHitsTotal.ToString();
									newRow.HitsPerStation = 
										result.HitsPerStnString.Replace(";", " ");
								}
								break;
							}
							case Structs.CompetitionTypeEnum.MagnumField:
							{
								newRow.Hits = result.HitsTotal.ToString() + " / " + 
									result.FigureHitsTotal.ToString();
								newRow.HitsPerStation = 
									result.HitsPerStnString.Replace(";", " ");
								break;
							}
							case Structs.CompetitionTypeEnum.Precision:
							{
								newRow.Hits = result.HitsTotal.ToString();
								newRow.HitsPerStation = 
									result.HitsPerStnString.Replace(";", " ");
								break;
							}
							default:
								throw new ApplicationException("Unknown CompetitionType");
						}

						if (result.FinalShootingPlace != 100)
							newRow.HitsPerStation += " (" + result.FinalShootingPlace.ToString() + ")";

						this.datasetResults1.Results.AddResultsRow(newRow);
						i++;
					}
					if (this.InvokeRequired)
						this.Invoke(DatasetBindToDataGrid);
					else
						DatasetBindToDataGrid();
					Trace.WriteLine("FResultsView.populateDatasetwithThread took " +
						(DateTime.Now - start).TotalMilliseconds.ToString() +
						" ms.");
				}

				Trace.WriteLine("FResultsView.populateDatasetwithThread() unlocking \"" +
					"lockObject\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			finally
			{
				Interlocked.Decrement(ref populateDatasetwithThreadsCount);
				Trace.WriteLine("FResultsView.populateDatasetwithThread() ended");
			}
		}

		private void datasetBindToDataGrid()
		{
			Trace.WriteLine("FResultsView.datasetBindToDataGrid() " + 
				"started on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			try
			{
				Trace.WriteLine("datasetResults1.Results contains columns:");
				foreach(DataColumn col in this.datasetResults1.Results.Columns)
				{
					Trace.WriteLine("Name: \"" + col.ColumnName + "\", " + 
						"Type: \"" + col.DataType.ToString() + "\"");
				}
				this.dataGrid1.SetDataBinding(this.datasetResults1,
					"Results");
				this.datasetResults1 = null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine("FResultsView.datasetBindToDataGrid " + 
					"Exception:\r\n" + exc.ToString());
			}
			finally
			{
				Trace.WriteLine("FResultsView.datasetBindToDataGrid() " + 
					"ended.");
			}
		}

		Hashtable clubs = Hashtable.Synchronized(new Hashtable());
		readonly object lockObject = new object();
		private string getClubName(string ClubId)
		{
			if (clubs.ContainsKey(ClubId))
				return (string)clubs[ClubId];

			string name = CommonCode.GetClub(ClubId).Name;
			clubs.Add(ClubId, name);
			return name;
		}
		#endregion

		#region Print Results
		private void btnPrint_Click(object sender, System.EventArgs e)
		{
#if VisualStudio
			// Check if there is any results to print
			if (CommonCode.GetCompetitorResultsCount() == 0)
			{
				MessageBox.Show(
					"Det finns inte några resultat att skriva ut", 
					"WinShooter", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Warning);
				return;
			}
			try 
			{
				CPrintResultlist pd = new CPrintResultlist(ref CommonCode, 
					getCurrentWeaponClass(),
					getCurrentShootersClass(),
					true,
					null);

				PrintDialog dlg = new PrintDialog() ;
				dlg.Document = pd;
				dlg.Document.DefaultPageSettings.Landscape = false;
				DialogResult result = dlg.ShowDialog();

				if (result == DialogResult.OK) 
				{
					pd.Print();
				}

			} 
			catch(Exception exc)
			{
				MessageBox.Show("Fel uppstod vid utskrift:\n" + exc.ToString());
			}
#else
			MessageBox.Show("Printing is not supported in Mono");
#endif
		}
		#endregion

	}
}
