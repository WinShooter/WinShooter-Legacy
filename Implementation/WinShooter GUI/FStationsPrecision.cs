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
// $Id: FStationsPrecision.cs 130 2011-05-28 17:32:36Z smuda $ 
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Allberg.Shooter.Windows.Forms;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for FStationsPrecision.
	/// </summary>
	public class FStationsPrecision : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private SafeButton btnSave;
		private SafeLabel SafeLabel1;
		private SafeButton btnCancel;
		private System.Windows.Forms.NumericUpDown numNrOfSeries;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numNrOfShootPerSeries;
		private CheckBox chkUseOnlyOneBoxForResult;
		private System.Windows.Forms.ToolTip toolTip1;

		internal FStationsPrecision(ref Common.Interface newCommon)
		{
			Trace.WriteLine("FStationsPrecision: Creating on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CommonCode = newCommon;

			height = Size.Height;
			width = Size.Width;
			Resize += new EventHandler(FStationsPrecision_Resize);
			Trace.WriteLine("FStationsPrecision: Succesfully created.");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			Trace.WriteLine("FStationsPrecision: Dispose(" + disposing.ToString() + ")" +
				"from thread \"" + Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
				DateTime.Now.ToLongTimeString());

			if(!DisposeNow)
			{
				Visible = false;
				EnableMain();

				return;
			}

			Visible = false;

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );

			Trace.WriteLine("FStationsPrecision: Dispose(" + disposing.ToString() + ")" +
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FStationsPrecision));
			this.btnSave = new SafeButton();
			this.SafeLabel1 = new SafeLabel();
			this.numNrOfSeries = new System.Windows.Forms.NumericUpDown();
			this.btnCancel = new SafeButton();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.numNrOfShootPerSeries = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.chkUseOnlyOneBoxForResult = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numNrOfSeries)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numNrOfShootPerSeries)).BeginInit();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(70, 81);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 3;
			this.btnSave.Text = "Spara";
			this.toolTip1.SetToolTip(this.btnSave, "Spara stationskonfiguration och stäng fönstret.");
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// SafeLabel1
			// 
			this.SafeLabel1.Location = new System.Drawing.Point(8, 8);
			this.SafeLabel1.Name = "SafeLabel1";
			this.SafeLabel1.Size = new System.Drawing.Size(100, 23);
			this.SafeLabel1.TabIndex = 2;
			this.SafeLabel1.Text = "Antal serier";
			// 
			// numNrOfSeries
			// 
			this.numNrOfSeries.Location = new System.Drawing.Point(150, 8);
			this.numNrOfSeries.Maximum = new decimal(new int[] {
			20,
			0,
			0,
			0});
			this.numNrOfSeries.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.numNrOfSeries.Name = "numNrOfSeries";
			this.numNrOfSeries.Size = new System.Drawing.Size(72, 20);
			this.numNrOfSeries.TabIndex = 1;
			this.numNrOfSeries.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.numNrOfSeries, "Välj hur många stationer som ska finnas");
			this.numNrOfSeries.Value = new decimal(new int[] {
			6,
			0,
			0,
			0});
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(150, 81);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Stäng";
			this.toolTip1.SetToolTip(this.btnCancel, "Stäng fönstret");
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// numNrOfShootPerSeries
			// 
			this.numNrOfShootPerSeries.Location = new System.Drawing.Point(150, 32);
			this.numNrOfShootPerSeries.Maximum = new decimal(new int[] {
			6,
			0,
			0,
			0});
			this.numNrOfShootPerSeries.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.numNrOfShootPerSeries.Name = "numNrOfShootPerSeries";
			this.numNrOfShootPerSeries.Size = new System.Drawing.Size(72, 20);
			this.numNrOfShootPerSeries.TabIndex = 2;
			this.numNrOfShootPerSeries.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.numNrOfShootPerSeries, "Välj hur många stationer som ska finnas");
			this.numNrOfShootPerSeries.Value = new decimal(new int[] {
			5,
			0,
			0,
			0});
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 23);
			this.label1.TabIndex = 13;
			this.label1.Text = "Antal skott per serie";
			// 
			// chkUseOnlyOneBoxForResult
			// 
			this.chkUseOnlyOneBoxForResult.AutoSize = true;
			this.chkUseOnlyOneBoxForResult.Location = new System.Drawing.Point(11, 58);
			this.chkUseOnlyOneBoxForResult.Name = "chkUseOnlyOneBoxForResult";
			this.chkUseOnlyOneBoxForResult.Size = new System.Drawing.Size(214, 17);
			this.chkUseOnlyOneBoxForResult.TabIndex = 14;
			this.chkUseOnlyOneBoxForResult.Text = "Sammanräkning av serie sker på station";
			this.chkUseOnlyOneBoxForResult.UseVisualStyleBackColor = true;
			// 
			// FStationsPrecision
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(227, 111);
			this.Controls.Add(this.chkUseOnlyOneBoxForResult);
			this.Controls.Add(this.numNrOfShootPerSeries);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.numNrOfSeries);
			this.Controls.Add(this.SafeLabel1);
			this.Controls.Add(this.btnSave);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "FStationsPrecision";
			this.Text = "Serier";
			((System.ComponentModel.ISupportInitialize)(this.numNrOfSeries)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numNrOfShootPerSeries)).EndInit();
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

		internal void enableMe()
		{
			Trace.WriteLine("FStationsPrecision: enableMe started on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			this.Visible = true;
			this.Focus();

			UpdatedStations();

			Trace.WriteLine("FStationsPrecision: enableMe ended.");
		}

		private void FStationsPrecision_Resize(object sender, EventArgs e)
		{
			Size size = new Size(width, height);
			Size = size;
		}

		internal void UpdatedStations()
		{
			Trace.WriteLine("FStationsPrecision: UpdatedStations on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			if (!Visible)
				return;

			Structs.Station[] stations = 
				CommonCode.GetStations();
			if (stations.Length > 0)
			{
				numNrOfSeries.Value = stations.Length;
				numNrOfShootPerSeries.Value = stations[0].Shoots/10;
				if (stations[0].Figures > 1)
					chkUseOnlyOneBoxForResult.Checked = false;
				else
					chkUseOnlyOneBoxForResult.Checked = true;
			}
			else
			{
				numNrOfSeries.Value = 6;
				numNrOfShootPerSeries.Value = 5;
				chkUseOnlyOneBoxForResult.Checked = true;
			}

			Trace.WriteLine("FStationsPrecision: UpdatedStations ended.");
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			Trace.WriteLine("FStationsPrecision: btnSave_Click on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			saveCurrent();
			// Go to main window
			Visible = false;
			try
			{
				EnableMain();
			}
			catch(Exception)
			{
			}

			Trace.WriteLine("FStationsPrecision: btnSave_Click ended.");
		}

		private void saveCurrent()
		{
			int nrOfStations = (int)numNrOfSeries.Value;
			int nrOfShootsPerStation = (int)numNrOfShootPerSeries.Value;

			int shoots = 0;
			int figures = 0;
			switch (chkUseOnlyOneBoxForResult.Checked)
			{
				case true:
					figures = 1;
					shoots = 10 * nrOfShootsPerStation;
					break;
				case false:
					figures = nrOfShootsPerStation;
					shoots = 10 * nrOfShootsPerStation;
					break;
			}
			for(int i=1; i<=CommonCode.GetStationsCount(); i++)
			{
				Structs.Station station = CommonCode.GetStation(i, false);
				station.Figures = figures;
				station.Shoots = shoots;
				CommonCode.UpdateStation(station);
			}

			while(CommonCode.GetStationsCount() < nrOfStations)
			{
				Structs.Station station = new Structs.Station();
				station.CompetitionId = CommonCode.GetCompetitions()[0].CompetitionId;
				station.Figures = figures;
				station.Points = false;
				station.Shoots = shoots;
				station.StationNr = CommonCode.GetStationsCount() + 1;
				CommonCode.NewStation(station, false);
			}
			while(CommonCode.GetStationsCount() > nrOfStations)
			{
				Structs.Station station = CommonCode.GetStation(CommonCode.GetStationsCount(), false);
				CommonCode.DelStation(station);
			}
			
			Structs.Competition competition = CommonCode.GetCompetitions()[0];
			int timeAvailable = competition.PatrolTimeBetween;
			int timeNeeded = calculateTimeRequirements();
			if (timeNeeded > timeAvailable )
			{
				DialogResult res = MessageBox.Show("Du har avsatt " + timeAvailable.ToString() + 
					" minuter för varje skjutlag.\r\n\r\n" + 
					"Beräknad tid för varje skjutlag med hänsyn taget till " +
					"skjuttid (6 minuter per serie) och markeringstid " +
					"(6 minuter per serie) är " +
					timeNeeded.ToString() + " minuter.\r\n\r\n" +
					"Vill du öka upp avsatt tid?", 
					"Kontrollberäkning", 
					MessageBoxButtons.YesNo, 
					MessageBoxIcon.Warning);
				if (res == DialogResult.Yes)
				{
					competition.PatrolTimeBetween = timeNeeded;
					CommonCode.UpdateCompetition(competition);
					DateTime patrolStart = competition.StartTime;
					Structs.Patrol[] patrols = CommonCode.GetPatrols();
					for(int i=0;i<patrols.Length;i++)
					{
						Structs.Patrol patrol = patrols[i];

						//Trace.WriteLine(patrol.StartDateTimeDisplay.ToShortTimeString());
						patrol.StartDateTime = patrolStart;
						patrol.StartDateTimeDisplay = patrolStart;
						CommonCode.UpdatePatrol(patrol);

						patrolStart = patrolStart.AddMinutes(timeNeeded);
					}
				}
			}
		}

		private int calculateTimeRequirements()
		{
			int shootingTime = 6*(int)(numNrOfSeries.Value+1);
			int markingTime = 6*(int)(numNrOfSeries.Value+1);
			return shootingTime + markingTime;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Visible = false;
			EnableMain();
		}
	}
}
