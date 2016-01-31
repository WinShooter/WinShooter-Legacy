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
// $Id: FStationsField.cs 130 2011-05-28 17:32:36Z smuda $ 
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
	/// Summary description for FStationsField.
	/// </summary>
	public class FStationsField : System.Windows.Forms.Form
	{
		private SafeLabel SafeLabel1;
		private System.Windows.Forms.Panel panelStations;
		private System.Windows.Forms.Label lblNumberOfStations;
		private SafeButton btnAddStation;
		private Allberg.Shooter.Windows.Forms.SafeCheckBox chkDistiguish;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FStationsField(ref Common.Interface newCommon)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CommonCode = newCommon;
			updatedStationsInvoker += new MethodInvoker(updatedStations);
			this.Resize += new EventHandler(FStationsField_Resize);
		}
		private MethodInvoker updatedStationsInvoker;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			Trace.WriteLine("FStationsField: Dispose(" + disposing.ToString() + ")" +
				"from thread \"" + Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
				DateTime.Now.ToLongTimeString());

			if(!DisposeNow)
			{
				// TODO Implement
				Visible = false;
				EnableMain();

				return;
			}

			this.Visible = false;

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );

			Trace.WriteLine("FStationsField: Dispose(" + disposing.ToString() + ")" +
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FStationsField));
			this.SafeLabel1 = new SafeLabel();
			this.panelStations = new System.Windows.Forms.Panel();
			this.lblNumberOfStations = new System.Windows.Forms.Label();
			this.btnAddStation = new SafeButton();
			this.chkDistiguish = new Allberg.Shooter.Windows.Forms.SafeCheckBox();
			this.SuspendLayout();
			// 
			// SafeLabel1
			// 
			this.SafeLabel1.Location = new System.Drawing.Point(8, 8);
			this.SafeLabel1.Name = "SafeLabel1";
			this.SafeLabel1.Size = new System.Drawing.Size(100, 23);
			this.SafeLabel1.TabIndex = 4;
			this.SafeLabel1.Text = "Antal stationer";
			// 
			// panelStations
			// 
			this.panelStations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelStations.AutoScroll = true;
			this.panelStations.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelStations.Location = new System.Drawing.Point(8, 32);
			this.panelStations.Name = "panelStations";
			this.panelStations.Size = new System.Drawing.Size(584, 232);
			this.panelStations.TabIndex = 0;
			// 
			// lblNumberOfStations
			// 
			this.lblNumberOfStations.Location = new System.Drawing.Point(120, 8);
			this.lblNumberOfStations.Name = "lblNumberOfStations";
			this.lblNumberOfStations.Size = new System.Drawing.Size(100, 24);
			this.lblNumberOfStations.TabIndex = 5;
			this.lblNumberOfStations.Text = "0";
			// 
			// btnAddStation
			// 
			this.btnAddStation.Location = new System.Drawing.Point(224, 8);
			this.btnAddStation.Name = "btnAddStation";
			this.btnAddStation.Size = new System.Drawing.Size(75, 23);
			this.btnAddStation.TabIndex = 0;
			this.btnAddStation.Text = "Lägg till";
			this.btnAddStation.Click += new System.EventHandler(this.btnAddStation_Click);
			// 
			// chkDistiguish
			// 
			this.chkDistiguish.AutoSize = true;
			this.chkDistiguish.Location = new System.Drawing.Point(306, 9);
			this.chkDistiguish.Name = "chkDistiguish";
			this.chkDistiguish.Size = new System.Drawing.Size(120, 17);
			this.chkDistiguish.TabIndex = 6;
			this.chkDistiguish.Text = "Särskjutningsstation";
			this.chkDistiguish.UseVisualStyleBackColor = true;
			this.chkDistiguish.CheckedChanged += new System.EventHandler(this.chkDistiguish_CheckedChanged);
			// 
			// FStationsField
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(600, 270);
			this.Controls.Add(this.chkDistiguish);
			this.Controls.Add(this.btnAddStation);
			this.Controls.Add(this.lblNumberOfStations);
			this.Controls.Add(this.panelStations);
			this.Controls.Add(this.SafeLabel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FStationsField";
			this.Text = "Stationer (Fältskytte)";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion


		Common.Interface CommonCode;
		public bool DisposeNow = false;
		public delegate void EnableMainHandler();
		public event EnableMainHandler EnableMain;

		internal void UpdatedStations()
		{
			Trace.WriteLine("FStationsField: UpdatedStations on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			if (this.InvokeRequired)
				this.Invoke(updatedStationsInvoker);
			else
				updatedStations();

			Trace.WriteLine("FStationsField: UpdatedStations ended.");
		}

		private void updatedStations()
		{
			drawStations();
		}

		private void FStationsField_Resize(object sender, EventArgs e)
		{
			drawStations();
		}

		private void btnAddStation_Click(object sender, System.EventArgs e)
		{
			Structs.Station station = new Structs.Station();
			station.CompetitionId = CommonCode.GetCompetitions()[0].CompetitionId;
			station.Figures = 3;
			station.Points = false;
			station.Shoots = 6;
			station.StationNr = CommonCode.GetStationsCount() + 1;
			CommonCode.NewStation(station, false);
		}

		internal void enableMe()
		{
			this.Visible = true;
			this.Focus();

			checkStationsNumbers();
			drawStations();
		}

		private void drawStations()
		{
			Trace.WriteLine("FStationsField: drawStation started on thread " + Thread.CurrentThread.Name);
#if DEBUG
			DateTime start = DateTime.Now;
#endif

			this.btnAddStation.Enabled = !chkDistiguish.Checked;

			// Clean all currently drawed stations
			clearPanelStations();
			int x = 4;
			int y = 0;

			Structs.Station[] stations = GetStations();

			this.lblNumberOfStations.Text = stations.Length.ToString();

			int tabStop = 1;
			foreach(Structs.Station station in stations)
			{
				if (x + stationWidth > panelStations.Width)
				{
					y = y + stationHeight;
					x = 4;
				}
				drawPanelStation(station, x, y, ref tabStop);
				x += stationWidth;
			}
#if DEBUG
			TimeSpan span = DateTime.Now - start;
			Trace.WriteLine("FStationsField: drawStation ended after " + span.TotalSeconds.ToString());
#else
			Trace.WriteLine("FStationsField: drawStation ended");
#endif
		}

		private void clearPanelStations()
		{
			Trace.WriteLine("FStationsField: clearPanelStations started on thread " + Thread.CurrentThread.Name);
#if DEBUG
			DateTime start = DateTime.Now;
#endif
			try
			{
				int rounds = 0;
				while(panelStations.Controls.Count>0 & rounds<10)
				{
					Trace.WriteLine("FStationsField: cleaninground " + rounds.ToString());
					clearObjects(panelStations);
					rounds++;
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine("An error occured while cleaning panelStations: " + exc.ToString());
				MessageBox.Show("An error occured while cleaning panelStations: " + exc.ToString());
			}

			GC.Collect();
#if DEBUG
			Trace.WriteLine("FStationsField: After cleaning all objects in panel there is " + 
				this.panelStations.Controls.Count + " objects in panel:");
			foreach(Control ctrl in this.panelStations.Controls)
			{
				Trace.Write("\t" + ctrl.GetType().ToString());
				if (ctrl == null)
					Trace.WriteLine(" (null)");
				else
					Trace.WriteLine("");
			}

			TimeSpan span = DateTime.Now - start;
			Trace.WriteLine("FStationsField: clearPanelStations ended after " + span.TotalSeconds.ToString());
#else
			Trace.WriteLine("FStationsField: clearPanelStations ended");
#endif
		}
		private void clearObjects(System.Windows.Forms.Control ctrl)
		{
			foreach(Control obj in ctrl.Controls)
			{
				if (obj != null)
				{
					clearObjects(obj);
					obj.Dispose();
				}
				ctrl.Controls.Remove(obj);
			}
		}

		int stationWidth = 160;
		int stationHeight = 148;
		private void drawPanelStation(Structs.Station station, int x, int y, ref int tabStop)
		{
			Trace.WriteLine("FStationsField: Drawing station " + station.StationNr);
			int x1 = 8;
			int x2 = 112;
			int y1 = 24;
			int y2 = 48;
			int y3 = 72;
			int y4 = 96;
			int y5 = 120;
			GroupBox grouping = new GroupBox();
			grouping.Name = "GroupboxStation" + station.StationNr.ToString();
			grouping.Text = "Station " + station.StationNr.ToString();
			grouping.Location = new Point(x,y);
			grouping.Size = new Size(stationWidth, stationHeight);
			panelStations.Controls.Add(grouping);

			#region Labels
			// Labels
			Label label1 = new Label();
			label1.Text = "Antal figurer";
			label1.Location = new Point(x1, y1);
			label1.Size = new Size(100,23);
			grouping.Controls.Add(label1);

			Label label2 = new Label();
			label2.Text = "Antal skott";
			label2.Location = new Point(x1, y2);
			label2.Size = new Size(100,23);
			grouping.Controls.Add(label2);
			#endregion

			#region NumericUpDowns
			// nums
			NumericUpDown numNumberOfFigures = new NumericUpDown();
			numNumberOfFigures.Minimum = 1;
			numNumberOfFigures.Maximum = 6;
			numNumberOfFigures.Value = station.Figures;
			numNumberOfFigures.Location = new Point(x2, y1);
			numNumberOfFigures.Size = new Size(40,20);
			numNumberOfFigures.Name = "NumberOfFigures" + station.StationNr.ToString();
			numNumberOfFigures.ValueChanged += new EventHandler(numNumberOfFigures_ValueChanged);
			numNumberOfFigures.KeyUp += new KeyEventHandler(numNumberOfFigures_KeyUp);
			numNumberOfFigures.TabIndex = tabStop++;
			numNumberOfFigures.TextAlign = HorizontalAlignment.Right;
			grouping.Controls.Add(numNumberOfFigures);
			Trace.WriteLine("FStationsField: station " + station.StationNr + 
				", Figures=" + station.Figures.ToString());

			NumericUpDown numNumberOfShoots = new NumericUpDown();
			numNumberOfShoots.Minimum = 1;
			numNumberOfShoots.Maximum = 6;
			numNumberOfShoots.Value = station.Shoots;
			numNumberOfShoots.Location = new Point(x2, y2);
			numNumberOfShoots.Size = new Size(40,20);
			numNumberOfShoots.Name = "NumberOfShoots" + station.StationNr.ToString();
			numNumberOfShoots.ValueChanged += new EventHandler(numNumberOfShoots_ValueChanged);
			numNumberOfShoots.KeyUp += new KeyEventHandler(numNumberOfShoots_KeyUp);
			numNumberOfShoots.TabIndex = tabStop++;
			numNumberOfShoots.TextAlign = HorizontalAlignment.Right;
			grouping.Controls.Add(numNumberOfShoots);
			Trace.WriteLine("FStationsField: station " + station.StationNr + 
				", Shoots=" + station.Shoots.ToString());
			#endregion

			#region Checkboxes
			// Checkbox
			CheckBox chkPoints = new CheckBox();
			chkPoints.Checked = station.Points;
			chkPoints.Text = "Poängtavla/or";
			chkPoints.Location = new Point(x1, y3);
			chkPoints.Size = new Size(144,24);
			chkPoints.Name = "Points" + station.StationNr.ToString();
			chkPoints.Click += new EventHandler(chkPoints_Click);
			chkPoints.TabIndex = tabStop++;
			grouping.Controls.Add(chkPoints);
			Trace.WriteLine("FStationsField: station " + station.StationNr + 
				", Points=" + station.Points.ToString());
			#endregion

			#region Buttons
			// Buttons
			Button btnPrevious = new Button();
			btnPrevious.Name = "PreviousStation" + station.StationNr.ToString();
			btnPrevious.Text = "Tidigare";
			btnPrevious.Location = new Point(x1,y4);
			btnPrevious.Size = new Size(72,23);
			btnPrevious.Click += new EventHandler(btnPrevious_Click);
			btnPrevious.TabIndex = tabStop++;
			if (station.StationNr == 1)
				btnPrevious.Enabled = false;
			grouping.Controls.Add(btnPrevious);

			Button btnNext = new Button();
			btnNext.Name = "NextStation" + station.StationNr.ToString();
			btnNext.Text = "Senare";
			btnNext.Location = new Point(btnPrevious.Location.X + btnPrevious.Size.Width,y4);
			btnNext.Size = new Size(btnPrevious.Size.Width,23);
			btnNext.Click += new EventHandler(btnNext_Click);
			btnNext.TabIndex = tabStop++;
			if (station.StationNr == CommonCode.GetStationsCount())
				btnNext.Enabled = false;
			grouping.Controls.Add(btnNext);

			Button btnDelete = new Button();
			btnDelete.Name = "DeleteStation" + station.StationNr.ToString();
			btnDelete.Text = "Radera";
			btnDelete.Location = new Point(x1,y5);
			btnDelete.Size = new Size(144,23);
			btnDelete.Click += new EventHandler(btnDelete_Click);
			btnDelete.TabIndex = tabStop++;
			grouping.Controls.Add(btnDelete);
			#endregion
		}
		#region Handlers for changing Stations
		private void numNumberOfFigures_ValueChanged(object sender, EventArgs e)
		{
			NumericUpDown numNumberOfFigures = (NumericUpDown)sender;
			numNumberOfFigures_ValueChanged(numNumberOfFigures);
		}
		private void numNumberOfFigures_KeyUp(object sender, KeyEventArgs e)
		{
			NumericUpDown numNumberOfFigures = (NumericUpDown)sender;
			numNumberOfFigures_ValueChanged(numNumberOfFigures);
		}
		private void numNumberOfFigures_ValueChanged(NumericUpDown numNumberOfFigures)
		{
			string stationnrString = numNumberOfFigures.Name.Replace("NumberOfFigures","");
			Structs.Station station = CommonCode.GetStation(int.Parse(stationnrString), 
				chkDistiguish.Checked);
			if (numNumberOfFigures.Value > numNumberOfFigures.Maximum)
			{
				numNumberOfFigures.Value = numNumberOfFigures.Maximum;
			}
			if (numNumberOfFigures.Value < numNumberOfFigures.Minimum)
			{
				numNumberOfFigures.Value = numNumberOfFigures.Minimum;
			}
			station.Figures = (int)numNumberOfFigures.Value;
			CommonCode.UpdateStation(station);
		}
		private void numNumberOfShoots_ValueChanged(object sender, EventArgs e)
		{
			NumericUpDown numNumberOfShoots = (NumericUpDown)sender;
			numNumberOfShoots_ValueChanged(numNumberOfShoots);
		}
		private void numNumberOfShoots_KeyUp(object sender, KeyEventArgs e)
		{
			NumericUpDown numNumberOfShoots = (NumericUpDown)sender;
			numNumberOfShoots_ValueChanged(numNumberOfShoots);
		}
		private void numNumberOfShoots_ValueChanged(NumericUpDown numNumberOfShoots)
		{
			string stationnrString = numNumberOfShoots.Name.Replace("NumberOfShoots","");
			Structs.Station station = CommonCode.GetStation(int.Parse(stationnrString), 
				chkDistiguish.Checked);
			if (numNumberOfShoots.Value > numNumberOfShoots.Maximum)
			{
				numNumberOfShoots.Value = numNumberOfShoots.Maximum;
			}
			if (numNumberOfShoots.Value < numNumberOfShoots.Minimum)
			{
				numNumberOfShoots.Value = numNumberOfShoots.Minimum;
			}
			station.Shoots = (int)numNumberOfShoots.Value;
			CommonCode.UpdateStation(station);
		}
		#region Checkboxes
		private void chkPoints_Click(object sender, EventArgs e)
		{
			CheckBox chkPoints = (CheckBox)sender;
			string stationnrString = chkPoints.Name.Replace("Points","");
			Structs.Station station = CommonCode.GetStation(int.Parse(stationnrString), 
				chkDistiguish.Checked);
			station.Points = chkPoints.Checked;
			CommonCode.UpdateStation(station);
		}
		#endregion
		#region Buttons
		private void btnPrevious_Click(object sender, EventArgs e)
		{
			Button btnPrevious = (Button)sender;
			string stationnrString = btnPrevious.Name.Replace("PreviousStation","");

			Structs.Station stationCurrent = CommonCode.GetStation(int.Parse(stationnrString),
				chkDistiguish.Checked);
			Structs.Station stationPrevious = CommonCode.GetStation(int.Parse(stationnrString)-1,
				chkDistiguish.Checked);

			changePlace(stationCurrent, stationPrevious);
		}
		private void btnNext_Click(object sender, EventArgs e)
		{
			Button btnPrevious = (Button)sender;
			string stationnrString = btnPrevious.Name.Replace("NextStation","");

			Structs.Station stationCurrent = CommonCode.GetStation(int.Parse(stationnrString),
				chkDistiguish.Checked);
			Structs.Station stationPrevious = CommonCode.GetStation(int.Parse(stationnrString)+1,
				chkDistiguish.Checked);

			changePlace(stationCurrent, stationPrevious);
		}
		private void changePlace(Structs.Station station1, Structs.Station station2)
		{
			int tempStationNr = station1.StationNr;
			station1.StationNr = station2.StationNr;
			station2.StationNr = tempStationNr;
			CommonCode.UpdateStation(station1);
			CommonCode.UpdateStation(station2);
			checkStationsNumbers();
		}
		private void btnDelete_Click(object sender, EventArgs e)
		{
			Button btnDelete = (Button)sender;
			string stationnrString = btnDelete.Name.Replace("DeleteStation","");
			Structs.Station station = CommonCode.GetStation(int.Parse(stationnrString),
				chkDistiguish.Checked);
			DialogResult res =
				MessageBox.Show("Är du säker på att du vill radera station " + 
				station.StationNr.ToString() + "?", 
				"Bekräfta radering", 
				MessageBoxButtons.YesNo);
			try
			{
				if (res == DialogResult.Yes)
				{
					Trace.WriteLine("FStationsField: Deleting station " + 
						station.StationNr.ToString());
					CommonCode.DelStation(station);
					checkStationsNumbers();
				}
			}
			catch(ApplicationException exc)
			{
				if (exc.Message.IndexOf("CompetitorResults")>-1)
				{
					Trace.WriteLine(
						"FStationsField: Failed to delete station since there is results for this station." +
						"Asking user what to do.");

					res =
						MessageBox.Show("Det finns inmatade resultat för denna station. " + 
						"Är du verkligen säker på att du vill radera station " + 
						station.StationNr.ToString() + "?", 
						"Bekräfta radering", 
						MessageBoxButtons.YesNo, 
						MessageBoxIcon.Warning, 
						MessageBoxDefaultButton.Button2);
					if (res == DialogResult.Yes)
					{
						Trace.WriteLine("FStationsField: Deleting results and station " + 
							station.StationNr.ToString());
						CommonCode.DelStation(station, true);
						checkStationsNumbers();
					}
				}
				else throw;
			}
		}
		#endregion
		#endregion

		private Structs.Station[] GetStations()
		{
			if (!chkDistiguish.Checked)
				return CommonCode.GetStations();
			else
				return CommonCode.GetStationsDistinguish();
		}

		#region Cleanup
		private void checkStationsNumbers()
		{
			Trace.WriteLine("FStationsField: checkStationsNumbers started.");
			Structs.Station[] stations = GetStations();

			for(int i=0;i<stations.Length;i++)
			{
				Structs.Station station = stations[i];
				if (station.StationNr != i+1)
				{
					Trace.WriteLine("FStationsField: checkStationsNumbers found one problem:" +
						"Station " + station.StationNr.ToString() + 
						" is changing number to " + (i+1).ToString());
					station.StationNr = i+1;
					CommonCode.UpdateStation(station);
				}
			}
			Trace.WriteLine("FStationsField: checkStationsNumbers started.");
		}
		#endregion

		private void chkDistiguish_CheckedChanged(object sender, EventArgs e)
		{
			enableMe();
		}

	}
}
