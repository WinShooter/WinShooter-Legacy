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
// $Id: FResults.cs 130 2011-05-28 17:32:36Z smuda $ 
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Allberg.Shooter.Common;
using Allberg.Shooter.Windows.Forms;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for FResults.
	/// </summary>
	public class FResults : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private SafeButton btnSave;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox chkCalculateResults;
		private System.Windows.Forms.Label lblPatrols;
		private System.Windows.Forms.ComboBox ddPatrols;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox ddCompetitors;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtShooter;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox chkSaveAutomatic;
		private CheckBox chkMoveNextShooter;
		private System.Windows.Forms.TextBox txtResults;

		internal FResults(ref Common.Interface newCommon)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CommonCode = newCommon;

			Trace.WriteLine("FResults: Creating");
			try
			{
				this.Resize += new EventHandler(FStations_Resize);

				// Databinding
				ddPatrols.DataSource = patrolsDs.Patrols;
				ddPatrols.DisplayMember = "DisplayName";
				ddPatrols.ValueMember = "Id";

				ddCompetitors.DataSource = this.competitorsDs.Shooters;
				ddCompetitors.DisplayMember = "Name";
				ddCompetitors.ValueMember = "Id";
			}
			catch(Exception exc)
			{
				Trace.WriteLine("FResults: Exception" + exc.ToString());
				throw;
			}
			finally
			{
				Trace.WriteLine("FResults: Created.");
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			Trace.WriteLine("FResults: Dispose(" + disposing.ToString() + ")" +
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

			Trace.WriteLine("FResults: Dispose(" + disposing.ToString() + ")" +
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FResults));
			this.btnSave = new SafeButton();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.chkCalculateResults = new System.Windows.Forms.CheckBox();
			this.lblPatrols = new System.Windows.Forms.Label();
			this.ddPatrols = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ddCompetitors = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtShooter = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtResults = new System.Windows.Forms.TextBox();
			this.chkSaveAutomatic = new System.Windows.Forms.CheckBox();
			this.chkMoveNextShooter = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(397, 6);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Stäng";
			this.toolTip1.SetToolTip(this.btnSave, "Väljer patrull. Du blir tillfrågad om skytten ska sparas.");
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.AutoScroll = true;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Location = new System.Drawing.Point(2, 86);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(686, 274);
			this.panel1.TabIndex = 6;
			// 
			// chkCalculateResults
			// 
			this.chkCalculateResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkCalculateResults.Checked = true;
			this.chkCalculateResults.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCalculateResults.Location = new System.Drawing.Point(478, 30);
			this.chkCalculateResults.Name = "chkCalculateResults";
			this.chkCalculateResults.Size = new System.Drawing.Size(203, 24);
			this.chkCalculateResults.TabIndex = 9;
			this.chkCalculateResults.Text = "Beräkna resultat under inskrivning";
			this.chkCalculateResults.CheckedChanged += new System.EventHandler(this.chkCalculateResults_CheckedChanged);
			// 
			// lblPatrols
			// 
			this.lblPatrols.Location = new System.Drawing.Point(8, 8);
			this.lblPatrols.Name = "lblPatrols";
			this.lblPatrols.Size = new System.Drawing.Size(56, 23);
			this.lblPatrols.TabIndex = 10;
			this.lblPatrols.Text = "Patruller";
			// 
			// ddPatrols
			// 
			this.ddPatrols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ddPatrols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddPatrols.Location = new System.Drawing.Point(64, 8);
			this.ddPatrols.Name = "ddPatrols";
			this.ddPatrols.Size = new System.Drawing.Size(288, 21);
			this.ddPatrols.TabIndex = 11;
			this.ddPatrols.SelectedIndexChanged += new System.EventHandler(this.ddPatrols_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 12;
			this.label2.Text = "Skytt";
			// 
			// ddCompetitors
			// 
			this.ddCompetitors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ddCompetitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddCompetitors.Location = new System.Drawing.Point(64, 32);
			this.ddCompetitors.Name = "ddCompetitors";
			this.ddCompetitors.Size = new System.Drawing.Size(408, 21);
			this.ddCompetitors.TabIndex = 13;
			this.ddCompetitors.SelectedIndexChanged += new System.EventHandler(this.ddCompetitors_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 23);
			this.label3.TabIndex = 14;
			this.label3.Text = "Skytt";
			// 
			// txtShooter
			// 
			this.txtShooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtShooter.Location = new System.Drawing.Point(64, 56);
			this.txtShooter.Name = "txtShooter";
			this.txtShooter.ReadOnly = true;
			this.txtShooter.Size = new System.Drawing.Size(288, 20);
			this.txtShooter.TabIndex = 15;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(360, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 23);
			this.label4.TabIndex = 16;
			this.label4.Text = "Resultat";
			// 
			// txtResults
			// 
			this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtResults.Location = new System.Drawing.Point(416, 56);
			this.txtResults.Name = "txtResults";
			this.txtResults.ReadOnly = true;
			this.txtResults.Size = new System.Drawing.Size(56, 20);
			this.txtResults.TabIndex = 17;
			// 
			// chkSaveAutomatic
			// 
			this.chkSaveAutomatic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkSaveAutomatic.Location = new System.Drawing.Point(478, 6);
			this.chkSaveAutomatic.Name = "chkSaveAutomatic";
			this.chkSaveAutomatic.Size = new System.Drawing.Size(200, 24);
			this.chkSaveAutomatic.TabIndex = 18;
			this.chkSaveAutomatic.Text = "Spara utan att fråga";
			// 
			// chkMoveNextShooter
			// 
			this.chkMoveNextShooter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkMoveNextShooter.AutoSize = true;
			this.chkMoveNextShooter.Location = new System.Drawing.Point(478, 58);
			this.chkMoveNextShooter.Name = "chkMoveNextShooter";
			this.chkMoveNextShooter.Size = new System.Drawing.Size(206, 17);
			this.chkMoveNextShooter.TabIndex = 19;
			this.chkMoveNextShooter.Text = "Skrivning av en station ger nästa skytt";
			this.chkMoveNextShooter.UseVisualStyleBackColor = true;
			// 
			// FResults
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(692, 366);
			this.Controls.Add(this.chkMoveNextShooter);
			this.Controls.Add(this.chkSaveAutomatic);
			this.Controls.Add(this.txtResults);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.chkCalculateResults);
			this.Controls.Add(this.txtShooter);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.ddCompetitors);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ddPatrols);
			this.Controls.Add(this.lblPatrols);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FResults";
			this.Text = "Resultatinmatning";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		internal bool DisposeNow = false;
		public delegate void EnableMainHandler();
		public event EnableMainHandler EnableMain;

		public delegate void PrintPatrolResultHandler(string PrintId, 
			FPrintSelection printSelection);
		public event PrintPatrolResultHandler PrintPatrolResult;

		Common.Interface CommonCode;
		DatasetPatrols patrolsDs = new DatasetPatrols();
		DatasetCompetitors competitorsDs = new DatasetCompetitors();
		int currentCompetitorId = -1;
		ArrayList resultBoxes = new ArrayList();
		ArrayList resultFigures = new ArrayList();
		ArrayList resultPoints = new ArrayList();

		Structs.CompetitionTypeEnum CompetitionType = 
			Structs.CompetitionTypeEnum.Field;

		#region Window Handling
		internal void enableMe()
		{
			CompetitionType = CommonCode.GetCompetitions()[0].Type;
			switch (CompetitionType)
			{
				case Structs.CompetitionTypeEnum.Field:
					break;
				case Structs.CompetitionTypeEnum.MagnumField:
					break;
				case Structs.CompetitionTypeEnum.Precision:
					lblPatrols.Text = "Skjutlag";
					chkMoveNextShooter.Text = "Skrivning av en serie ger nästa skytt";
					break;
				default:
					throw new ApplicationException("Unknown CompetitionType");
			}
			this.Visible = true;
			this.Focus();
			this.redrawStations();
			this.PatrolsUpdate();
			if (patrolsDs.Patrols.Count > 0)
			{
				ddPatrols.SelectedIndex = 0;
				ddPatrols_SelectedIndexChanged();
			}
			this.viewCurrent();
			this.resultIsChanged = false;
		}

		private void FStations_Resize(object sender, EventArgs e)
		{
			this.redrawStations();

			this.viewCurrent();
		}

		private void PatrolsUpdate()
		{
			if (this.Visible == false)
				return;

			Structs.Patrol[] patrols =
				CommonCode.GetPatrols();

			patrolsDs.Patrols.Clear();

			foreach (Structs.Patrol patrol in patrols)
			{
				DatasetPatrols.PatrolsRow row = 
					patrolsDs.Patrols.NewPatrolsRow();

				row.Class = patrol.PClass.ToString();
				row.Id = patrol.PatrolId;
				row.NumberOfCompetitors = CommonCode.GetCompetitorsCountPatrol(patrol);
				row.Starttime = patrol.StartDateTime;

				// Displayname
				row.DisplayName = "Nr ";
				for (int i=0 ; i< (3-patrol.PatrolId.ToString().Length); i++)
					row.DisplayName += "0";
				row.DisplayName += patrol.PatrolId.ToString() + " - ";
				row.DisplayName += patrol.StartDateTimeDisplay.ToShortTimeString();
				if (patrol.PClass != Structs.PatrolClass.Okänd)
					row.DisplayName += ", Klass " + patrol.PClass.ToString();

				patrolsDs.Patrols.AddPatrolsRow(row);
			}
		}
		private void ddPatrols_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ddPatrols_SelectedIndexChanged();
		}
		private void ddPatrols_SelectedIndexChanged()
		{
			if (this.ddPatrols.SelectedValue == null)
			{
				Trace.WriteLine("FResults: ddPatrols.SelectedValue is null?!?");
				return;
			}
			Structs.Patrol patrol = 
				CommonCode.GetPatrol((int)this.ddPatrols.SelectedValue);

			competitorsDs.Shooters.Clear();

			Structs.Competitor[] competitors =
				CommonCode.GetCompetitors(patrol, "Lane");

			foreach(Structs.Competitor competitor in competitors)
			{
				DatasetCompetitors.ShootersRow row =
					competitorsDs.Shooters.NewShootersRow();
				row.Id = competitor.CompetitorId;
				Structs.Shooter shooter = 
					CommonCode.GetShooter(competitor.ShooterId);
				row.Club = CommonCode.GetClub(shooter.ClubId).Name;
				row.Name = ""; 
				if (competitor.Lane.ToString().Length < 2) 
					row.Name += "0";
				row.Name += competitor.Lane.ToString() + " - " +
					shooter.Givenname + ", " + shooter.Surname + 
					" ( " + row.Club + " )";
				competitorsDs.Shooters.AddShootersRow( row );
			}

			if (this.competitorsDs.Shooters.Count > 0)
			{
				this.ddCompetitors.SelectedIndex = 0;
				this.ddCompetitors_SelectedIndexChanged();
			}
			else
				this.cleanCurrentResults();
		}

		private void ddCompetitors_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.currentCompetitorId != -1)
			{
				if (this.resultIsChanged)
				{
					if (!this.chkSaveAutomatic.Checked)
					{
						if (DialogResult.Yes == MessageBox.Show(
							"Vill du spara nuvarande skytt?", 
							"Spara Skytt", 
							MessageBoxButtons.YesNo, 
							MessageBoxIcon.Question))
						{
							this.saveCurrent();
						}
					}
					else
						this.saveCurrent();
				}
			}
			ddCompetitors_SelectedIndexChanged();
		}
		private void ddCompetitors_SelectedIndexChanged()
		{
			viewCurrent();
			resultIsChanged = false;
			if (chkCalculateResults.Checked)
				calculateResults();
		}
		bool resultIsChanged = false;

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Visible = false;
			EnableMain();		
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (this.currentCompetitorId != -1)
			{
				if (resultIsChanged)
				{
					if (!this.chkSaveAutomatic.Checked)
					{
						if (DialogResult.Yes == MessageBox.Show(
							"Vill du spara nuvarande skytt?", 
							"Spara Skytt", 
							MessageBoxButtons.YesNo, 
							MessageBoxIcon.Question))
						{
							this.saveCurrent();
						}
					}
					else
						this.saveCurrent();
				}
			}
			this.Visible = false;
			currentCompetitorId = -1;
			EnableMain();
		}
		#endregion

		#region Draw stations on screen

		const int groupBoxStartX = 8;
		const int groupBoxStartY = 8;
		//const int groupBoxStartY = 80;
		int groupBoxWidth = 80;
		int groupBoxHeight = 125;

		int groupBoxNextY = 125 + 10;
		int groupBoxNextX = 80 + 10;

		private void redrawStations()
		{
			switch(CompetitionType)
			{
				case Structs.CompetitionTypeEnum.Precision:
					groupBoxWidth = 80;
					groupBoxHeight = 110; // 80
					break;
				case Structs.CompetitionTypeEnum.Field:
					groupBoxWidth = 80;
					groupBoxHeight = 125;
					break;
				case Structs.CompetitionTypeEnum.MagnumField:
					groupBoxWidth = 80;
					groupBoxHeight = 125;
					break;
				default:
					throw new ApplicationException("Unknown Competition Type:" + 
						CompetitionType.ToString());
			}
			groupBoxNextY = groupBoxHeight + 10;
			groupBoxNextX = groupBoxWidth + 10;

			clearCurrentStations();

			// NextGroup Startposition
			int x = groupBoxStartX;
			int y = groupBoxStartY;

			// Draw all stations on screen
			Structs.Station[] stations = CommonCode.GetStations();
			Structs.Competition comp = CommonCode.GetCompetitions()[0];
			//bool norwegianCount = comp.NorwegianCount;
			bool doFinalShooting = comp.DoFinalShooting;

			foreach(Structs.Station station in stations)
			{
				// Create new group
				GroupBox thisGroup = new GroupBox();
				thisGroup.SuspendLayout();
				thisGroup.Location = new System.Drawing.Point(x, y);
				setNewGroupBoxDrawPoint(ref x, ref y);
				thisGroup.Name = "target" + station.StationNr.ToString();
				thisGroup.Size = new System.Drawing.Size(groupBoxWidth, groupBoxHeight);
				thisGroup.TabStop = false;
				switch (CompetitionType)
				{
					case Structs.CompetitionTypeEnum.Precision:
						thisGroup.Text = "Serie " + station.StationNr.ToString();
						break;
					case Structs.CompetitionTypeEnum.Field:
						thisGroup.Text = "Station " + station.StationNr.ToString();
						break;
					case Structs.CompetitionTypeEnum.MagnumField:
						thisGroup.Text = "Station " + station.StationNr.ToString();
						break;
					default:
						throw new ApplicationException("Unknown CompetitionType: " +
							CompetitionType.ToString());
				}

				// Add SafeLabels
				SafeLabel SafeLabelPoints = new Forms.SafeLabel();
				SafeLabelPoints.Location = new System.Drawing.Point(4, groupBoxHeight-25);
				SafeLabelPoints.Text = "P:";
				SafeLabelPoints.Size = new System.Drawing.Size(20, 23);
				thisGroup.Controls.Add(SafeLabelPoints);

				// Add txtBoxes
				foreach(SafeTextBox thisBox in createTxtBoxes(station.StationNr, station.Figures))
				{
					thisGroup.Controls.Add(thisBox);
					thisBox.TextChanged+=new EventHandler(figureBox_TextChanged);
					thisBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
					resultFigures.Add(thisBox);
				}

				// Add points txtBox
				Allberg.Shooter.Windows.Forms.SafeTextBox txtPoints = new SafeTextBox();
				txtPoints.Name = "Points" + station.StationNr.ToString();
				txtPoints.Location = new Point(SafeLabelPoints.Width+5, groupBoxHeight-25);
				txtPoints.Text = "";
				txtPoints.Size = new Size(30,20);
				txtPoints.Enabled = station.Points;
				txtPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
				txtPoints.TextChanged += new System.EventHandler(this.txtPoints_TextChanged);
				thisGroup.Controls.Add(txtPoints);
				if (CompetitionType == Structs.CompetitionTypeEnum.Precision)
				{
					txtPoints.Visible = false;
					SafeLabelPoints.Visible = false;
				}
				resultPoints.Add(txtPoints);

				// Draw the whole thing
				thisGroup.ResumeLayout();

				// Add to form
				this.panel1.Controls.Add(thisGroup);

				// Add to form holder
				this.resultBoxes.Add(thisGroup);
			}

			if (doFinalShooting)
			{
				GroupBox finalGroup = new GroupBox();
				finalGroup.SuspendLayout();

				finalGroup.Location = new System.Drawing.Point(x, y);
				setNewGroupBoxDrawPoint(ref x, ref y);
				finalGroup.Name = "Final";
				finalGroup.Size = new System.Drawing.Size(groupBoxWidth+5, groupBoxHeight);
				finalGroup.TabStop = false;
				finalGroup.Text = "Särskjutning";

				// Add SafeLabels
				SafeLabel SafeLabelPoints = new SafeLabel();
				SafeLabelPoints.Location = getTxtBoxLocation(3, 1);
				SafeLabelPoints.Text = "Placering:";
				SafeLabelPoints.Size = new System.Drawing.Size(60, 23);
				finalGroup.Controls.Add(SafeLabelPoints);

				// Add SafeTextBox
				txtFinalPlace = new SafeTextBox();
				txtFinalPlace.Location = getTxtBoxLocation(3, 2);
				txtFinalPlace.Text = "";
				txtFinalPlace.Size = new System.Drawing.Size(40,23);
				txtFinalPlace.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
				txtFinalPlace.TextChanged += new System.EventHandler(this.txtFinalPlace_TextChanged);
				finalGroup.Controls.Add(txtFinalPlace);

				// Add to form
				this.panel1.Controls.Add(finalGroup);

				// Add to form holder
				this.resultBoxes.Add(finalGroup);
			}
		}
		SafeTextBox txtFinalPlace = new SafeTextBox();
		private void clearCurrentStations()
		{
			foreach(SafeTextBox tbox in resultFigures.ToArray(new SafeTextBox().GetType()))
			{
				resultFigures.Remove(tbox);
				tbox.Dispose();
			}
			foreach(SafeTextBox tbox in resultPoints.ToArray(new SafeTextBox().GetType()))
			{
				resultPoints.Remove(tbox);
				tbox.Dispose();
			}
			foreach(GroupBox gbox in resultBoxes.ToArray(new GroupBox().GetType()))
			{
				resultBoxes.Remove(gbox);
				gbox.Dispose();
			}
		}

		private void setNewGroupBoxDrawPoint(ref int x, ref int y)
		{
			if((x+2*groupBoxNextX)>this.panel1.Size.Width)
			{
				x = groupBoxStartX;
				y+= groupBoxNextY;
			}
			else
			{
				x += groupBoxNextX;
			}
		}

		private SafeTextBox[] createTxtBoxes(int stationNr, int figures)
		{
			ArrayList boxes = new ArrayList();

			for (int i=1 ; i<= figures ; i++ )
			{
				SafeTextBox thisBox = new SafeTextBox();
				thisBox.Name = "Figure" + stationNr.ToString() + "-" + i.ToString();
				thisBox.Text = "";
				thisBox.Location = getTxtBoxLocation(figures, i);
				thisBox.Size = getTxtBoxSize(figures, i);
				boxes.Add(thisBox);
			}

			SafeTextBox[] boxesArray =  (SafeTextBox[])boxes.ToArray(new SafeTextBox().GetType());
			return boxesArray;
		}

		private Point getTxtBoxLocation(int figuresTot, int currentFigure)
		{
			int leftmargin = 15;
			int centermargin = 35;
			int line1 = 24;
			int line2 = 48;
			int line3 = 72;
			switch(figuresTot)
			{
				case 1:
				{
					return new Point(leftmargin,line1);
				}
				case 2:
				{
					switch(currentFigure)
					{
						case 1:
							return new Point(leftmargin,line1);
						case 2:
							return new Point(leftmargin,line2);
					}
					break;
				}
				case 3:
				{
					switch(currentFigure)
					{
						case 1:
							return new Point(leftmargin,line1);
						case 2:
							return new Point(leftmargin,line2);
						case 3:
							return new Point(leftmargin,line3);
					}
					break;
				}
				case 4:
				{
					switch(currentFigure)
					{
						case 1:
							return new Point(leftmargin,line1);
						case 2:
							return new Point(leftmargin,line2);
						case 3:
							return new Point(centermargin,line2);
						case 4:
							return new Point(leftmargin,line3);
					}
					break;
				}
				case 5:
				{
					switch(currentFigure)
					{
						case 1:
							return new Point(leftmargin,line1);
						case 2:
							return new Point(centermargin,line1);
						case 3:
							return new Point(leftmargin,line2);
						case 4:
							return new Point(leftmargin,line3);
						case 5:
							return new Point(centermargin,line3);
					}
					break;
				}
				case 6:
				{
					switch(currentFigure)
					{
						case 1:
							return new Point(leftmargin,line1);
						case 2:
							return new Point(centermargin,line1);
						case 3:
							return new Point(leftmargin,line2);
						case 4:
							return new Point(centermargin,line2);
						case 5:
							return new Point(leftmargin,line3);
						case 6:
							return new Point(centermargin,line3);
					}
					break;
				}
			}

			throw new ApplicationException(
				"Kan inte skapa tavla " + currentFigure.ToString() + " av " + 
				figuresTot.ToString() + ".");
		}
		private Size getTxtBoxSize(int figures, int currentFigure)
		{
			switch(figures)
			{
				case 4:
				{
					if (currentFigure == 2 | currentFigure == 3)
						return new Size(20,20);
					break;
				}
				case 5:
				{
					if (currentFigure == 1 | currentFigure == 2 | 
						currentFigure == 4 | currentFigure == 5)
						return new Size(20,20);
					break;
				}
				case 6:
				{
					return new Size(20,20);
				}
			}
			return new Size(40,20);
		}

		#endregion

		#region Save station
		private void saveCurrent()
		{
			DateTime startTime = DateTime.Now;
			Trace.WriteLine("FResults: saveCurrent started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
				DateTime.Now.ToLongTimeString());

			// If no current user, abort
			if (this.currentCompetitorId == -1)
				return;

			bool allHitsIsZero = true;

			// Get all values into hash tables for easy retreival
			Hashtable hitsTable = new Hashtable();
			foreach(SafeTextBox safeTextBox in resultFigures.ToArray(typeof(SafeTextBox)))
			{
				int parse = 0;
				try
				{
					parse = int.Parse(safeTextBox.Text);
				}
				catch(Exception)
				{
				}
				hitsTable.Add(safeTextBox.Name, parse);
				if (parse > 0)
					allHitsIsZero = false;
			}

			Hashtable pointsTable = new Hashtable();
			foreach(SafeTextBox safeTextBox in resultPoints.ToArray(new SafeTextBox().GetType()))
			{
				int parse = 0;
				try
				{
					parse = int.Parse(safeTextBox.Text);
				}
				catch(Exception)
				{
				}
				pointsTable.Add(safeTextBox.Name, parse);
				if (parse > 0)
					allHitsIsZero = false;
			}

			// go throw all stations, searching for values and adding
			// all together
			Structs.Station[] stations = CommonCode.GetStations();
			foreach(Structs.Station station in stations)
			{
				int totHits = 0;
				int totFigureHits = 0;
				string stationFigureHits = "";
				for(int figure=1; figure<=station.Figures ; figure++)
				{
					int thisHit = (int)hitsTable["Figure" + station.StationNr.ToString() +
							"-" + figure.ToString()];
					totHits += thisHit;
					if (thisHit > 0)
						totFigureHits++;
					stationFigureHits += thisHit.ToString() + ";";
				}

				// ok, we have all stuff for this station.
				// does this station already exist for this competitor?
				try
				{
					Structs.CompetitorResult res =
						CommonCode.GetCompetitorResult(
							currentCompetitorId, station.StationNr);
					// result already exist. Edit and return.
					if (!allHitsIsZero)
					{
						res.Hits = totHits;
						res.FigureHits = totFigureHits;
						res.Points = (int)pointsTable["Points" + 
							station.StationNr.ToString()];
						res.StationFigureHits = stationFigureHits;
						CommonCode.UpdateCompetitorResult(res, false);
					}
					else
					{
						CommonCode.DelCompetitorResult(res);
					}
				}
				catch (CannotFindIdException)
				{
					// Didnt exist. Create new
					Structs.CompetitorResult res =
						new Structs.CompetitorResult();
					res.Hits = totHits;
					res.FigureHits = totFigureHits;
					res.Points = (int)pointsTable["Points" + 
						station.StationNr.ToString()];
					res.StationFigureHits = stationFigureHits;
					res.CompetitorId = currentCompetitorId;
					res.Station = station.StationNr;
					CommonCode.NewCompetitorResult(res);
				}
			}

			Structs.Competitor competitor = CommonCode.GetCompetitor(currentCompetitorId);
			if (this.txtFinalPlace.Text == "")
				competitor.FinalShootingPlace = 100;
			else
			{
				try
				{
					competitor.FinalShootingPlace = 
						int.Parse(this.txtFinalPlace.Text);
					if (competitor.FinalShootingPlace == 0)
						competitor.FinalShootingPlace = 100;
				}
				catch(Exception exc)
				{
					Trace.WriteLine("FResults: saveCurrent Exception"+
						" while parsing FinalShootingPlace: " +
						exc.ToString());
					MessageBox.Show("Placering efter särskjutning " +
						"verkar inte vara en siffra. Avbryter.", 
						"Felmeddelande",
						MessageBoxButtons.OK,
						MessageBoxIcon.Stop);
					return;
				}
			}
			CommonCode.UpdateCompetitor(competitor);
			resultIsChanged = false;
			TimeSpan span = DateTime.Now - startTime;
			Trace.WriteLine("FResults: saveCurrent ended after " +
				span.TotalMilliseconds.ToString() + " ms.");

			int patrolId = (int)ddPatrols.SelectedValue;
			Structs.Patrol patrol = CommonCode.GetPatrol(patrolId);
			int compsInPatrol = 
				CommonCode.GetCompetitorsCountPatrol(patrol);
			int compsInPatrolWithResult = 
				CommonCode.GetCompetitorsWithResultCountPatrol(patrol);

			if (compsInPatrol == compsInPatrolWithResult)
			{
				DialogResult res = MessageBox.Show(
					"Det finns nu resultat för alla skyttar i denna patrull." +
					"Vill du skriva ut resultat för denna patrull?",
					"Utskrift av resultat",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);

				if (res == DialogResult.Yes)
				{
					try
					{
						PrintPatrolResult(patrolId.ToString(), null);
					}
					catch (Exception exc)
					{
						Trace.WriteLine("Exception while printing current patrol:" +
							exc.ToString());
					}
				}
			}
		}
		#endregion

		#region View current
		private void viewCurrent()
		{
			DateTime startTime = DateTime.Now;
			try
			{
				automaticChange = true;
				Trace.WriteLine("FResults: viewCurrent started from thread \"" + 
					Thread.CurrentThread.Name + "\" " +
					" ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
					DateTime.Now.ToLongTimeString());
				

				if (ddCompetitors.SelectedValue == null)
					return;
				cleanCurrentResults();
				automaticChange = true;
				currentCompetitorId = (int)ddCompetitors.SelectedValue;
				Structs.Competitor competitor = CommonCode.GetCompetitor(currentCompetitorId);
				Structs.Shooter shooter = CommonCode.GetShooter(competitor.ShooterId);
				this.txtShooter.Text = shooter.Givenname + ", " + shooter.Surname;
				if (competitor.FinalShootingPlace == 100 |
					competitor.FinalShootingPlace == 0)
					this.txtFinalPlace.Text = "";
				else
					this.txtFinalPlace.Text = competitor.FinalShootingPlace.ToString();
				foreach (Structs.CompetitorResult res in 
					CommonCode.GetCompetitorResults(
					currentCompetitorId))
				{
					SafeTextBox points = getSafeTextBoxPoints(res.Station);
					if (points.Enabled)
						points.Text = res.Points.ToString();

					int figure = 0;
					try
					{
						foreach(string str in res.StationFigureHits.Split(';'))
						{
							figure++;
							string figureText = "Figure" + res.Station.ToString() +
								"-" + figure.ToString();
							string temp = str.Replace(";", "");
							if (temp.Length>0)
							{
								SafeTextBox figureBox = this.getSafeTextBoxFigure(figureText);
								if (figureBox != null)
									figureBox.Text = temp;
							}
						}
					}
					catch(Exception)
					{
					}
				}

				SafeTextBox start = this.getSafeTextBoxFigure("Figure1-1");
				start.Focus();
				start.SelectAll();
			}
			finally
			{
				automaticChange = false;
				TimeSpan span = DateTime.Now - startTime;
				Trace.WriteLine("FResults: viewCurrent ended. " + 
					span.TotalMilliseconds.ToString() + " ms.");
			}
		}
		private void cleanCurrentResults()
		{
			Trace.WriteLine("FResults: cleanCurrentResults started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
				DateTime.Now.ToLongTimeString());
			DateTime startTime = DateTime.Now;
			try
			{
				automaticChange = true;

				foreach(SafeTextBox thisBox in this.resultFigures.ToArray(new SafeTextBox().GetType()))
				{
					thisBox.Text = "";
				}
				foreach(SafeTextBox thisBox in this.resultPoints.ToArray(new SafeTextBox().GetType()))
				{
					thisBox.Text = "";
				}
				this.currentCompetitorId = -1;
			}
			finally
			{
				automaticChange = false;
			}
			TimeSpan span = DateTime.Now - startTime;
			Trace.WriteLine("FResults: cleanCurrentResults ended after " +
				span.TotalMilliseconds.ToString() + " ms.");
		}
		#endregion

		#region Boxes change
		private void txtPoints_TextChanged(object sender, EventArgs e)
		{
			Trace.WriteLine("FResults: txtPoints_TextChanged started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
				DateTime.Now.ToLongTimeString());

			if (automaticChange)
			{
				Trace.WriteLine("FResults: txtPoints_TextChanged ended since automaticChange");
				return;
			}

			SafeTextBox thisBox = (SafeTextBox)sender;
			// Current name is "Points" + stationNr
			int stationNr = int.Parse(thisBox.Name.Substring(6));

			// Do sanity check
			try
			{
				if (thisBox.Text != "")
					int.Parse(thisBox.Text);
			}
			catch(Exception)
			{
				MessageBox.Show("Inskriven text verkar inte vara en siffra.", 
					"Felinmatning", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
				thisBox.Focus();
				thisBox.SelectAll();
				return;
			}
			if (chkCalculateResults.Checked)
			{
				// Calculate results
				calculateResults();
			}

			// If there is 0 hits on this station, 
			// set 0 as points and continue
			if (calculateFigureHits(stationNr) == 0)
			{
				if (thisBox.Text != "" &
					thisBox.Text != "0")
				{
					MessageBox.Show("Om skytten inte har några träffar " +
						"så kan inte några poäng skrivas i.",  
						"Felinmatning", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					thisBox.Text = "0";
				}
			}

			if (thisBox.Text.Length >= 2)
			{
				// Go to next box
				SafeTextBox nextBox = getNextTextBox(thisBox.Name);
				if (nextBox != null)
				{
					nextBox.Focus();
					nextBox.SelectAll();
				}
			}

			this.resultIsChanged = true;

			Trace.WriteLine("FResults: txtPoints_TextChanged ended.");
		}
		private void calculateResults()
		{
			Trace.WriteLine("FResults: calculateResults started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
				DateTime.Now.ToLongTimeString());
			DateTime startTime = DateTime.Now;

			int hits = 0;
			int figurehits = 0;
			foreach(object key in this.resultFigures)
			{
				SafeTextBox currentBox = (SafeTextBox)key;
				try
				{
					int test = int.Parse(currentBox.Text);
					if (test > 0)
					{
						hits = hits + test;
						figurehits++;
					}
				}
				catch(Exception)
				{
				}
			}
			switch(CompetitionType)
			{
				case Structs.CompetitionTypeEnum.Field:
				{
					if (this.CommonCode.GetCompetitions()[0].NorwegianCount)
					{
						this.txtResults.Text = (hits+figurehits).ToString();
					}
					else
					{
						this.txtResults.Text = 
							hits.ToString() + " / " + figurehits.ToString();
					}
					break;
				}
				case Structs.CompetitionTypeEnum.MagnumField:
				{
					this.txtResults.Text = 
						hits.ToString() + " / " + figurehits.ToString();
					break;
				}
				case Structs.CompetitionTypeEnum.Precision:
				{
					this.txtResults.Text = hits.ToString();
					break;
				}
				default:
					throw new ApplicationException("Unknown CompetitionType: " +
						CompetitionType.ToString());
			}

			TimeSpan span = DateTime.Now - startTime;
			Trace.WriteLine("FResults: calculateResults ended " +
				span.TotalMilliseconds.ToString() + " ms.");
		}
		private void txtFinalPlace_TextChanged(object sender, EventArgs e)
		{
			Trace.WriteLine("FResults: txtFinalPlace_TextChanged started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
				DateTime.Now.ToLongTimeString());

			if (automaticChange)
			{
				Trace.WriteLine("FResults: txtFinalPlace_TextChanged ended since automaticChange");
				return;
			}

			SafeTextBox thisBox = (SafeTextBox)sender;

			// Do sanity check
			try
			{
				int.Parse(thisBox.Text);
			}
			catch(Exception)
			{
				MessageBox.Show("Inskriven text verkar inte vara en siffra.",  
					"Felinmatning", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
				thisBox.Focus();
				thisBox.SelectAll();
				return;
			}
			this.resultIsChanged = true;

			Trace.WriteLine("FResults: txtFinalPlace_TextChanged ended");
		}
		bool automaticChange = false;
		private void figureBox_TextChanged(object sender, EventArgs e)
		{
			Trace.WriteLine("FResults: figureBox_TextChanged started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
				DateTime.Now.ToLongTimeString());

			if (automaticChange)
			{
				Trace.WriteLine("FResults: figureBox_TextChanged ended since automaticChange");
				return;
			}

			resultIsChanged = true;

			SafeTextBox thisBox = (SafeTextBox)sender;

			// If theres nothing in the box, let the user write.
			if (thisBox.Text.Length == 0)
				return;

			if (thisBox.Text.ToLower() == "x" ||
				thisBox.Text == "*" ||
				thisBox.Text == "+")
			{
				thisBox.Text = "10";
				return;
			}

			// Do sanity check
			try
			{
				int.Parse(thisBox.Text);
			}
			catch(Exception)
			{
				MessageBox.Show("Inskriven text verkar inte vara en siffra.", 
					"Felinmatning", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
				thisBox.Focus();
				thisBox.SelectAll();
				return;
			}

			// Check for more results than available shoots
			Structs.Station station;
			try
			{
				int end = thisBox.Name.IndexOf("-");
				string stnNr = thisBox.Name.Substring("Figure".Length,end-"Figure".Length);
				station = CommonCode.GetStation(int.Parse(stnNr), false);
				int result = getResultsForStationTot(stnNr, station.Figures);
				if ( result > station.Shoots )
				{
					MessageBox.Show("Verkar som om du skrivit in för många " +
						"träffar i förhållande till hur många skott som " + 
						"fanns tillgängliga.");
					thisBox.Focus();
					thisBox.SelectAll();
					Trace.WriteLine("FResults: to many shoots entered.");
					Trace.WriteLine("FResults: figureBox_TextChanged ended");
					return;
				}
			}			
			catch(Exception exc)
			{
				Trace.WriteLine("Exception while checking for more results than available shoots: " +
					exc.ToString());
				// TODO Handle Error
				MessageBox.Show("Ett fel uppstod: " + exc.Message);
				return;
			}

			try
			{
				if (chkCalculateResults.Checked)
				{
					// Calculate results
					calculateResults();
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
			}

			if (CompetitionType == Structs.CompetitionTypeEnum.Precision &&
				station.Figures == 1)
			{
				if (thisBox.Text.Length < 2)
					return;
			}
			// Set focus on next Points or Target
			SafeTextBox nextBox = getNextTextBox(thisBox.Name);

			int stationNr = getStationNr(thisBox.Name);

			// Check that
			if (calculateFigureHits(stationNr) == 0)
			{
				SafeTextBox pointsBox = getSafeTextBoxPoints(stationNr);
				if (pointsBox.Text != "" &
					pointsBox.Text != "0")
				{
					MessageBox.Show(
						"Eftersom skytten numera har 0 träffar så " + 
						"ändras poängen automatiskt till 0 från " + 
						pointsBox.Text + ".", 
						"Inmatning", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Information);
					pointsBox.Text = "0";
				}
			}

			try
			{
				if (nextBox == null)
				{
					if (!chkMoveNextShooter.Checked)
						this.ddCompetitors.Focus();
					else
					{
						string newBoxName = thisBox.Name.Substring(0, 1 + thisBox.Name.IndexOf("-")) + "1";

						int selectedIndex = ddCompetitors.SelectedIndex;

						if (ddCompetitors.SelectedIndex < ddCompetitors.Items.Count - 1)
						{
							ddCompetitors.SelectedIndex++;
							nextBox = getSafeTextBoxFigure(newBoxName);
						}
						else
						{
							ddCompetitors.SelectedIndex = 0;
							int figurenr = int.Parse(newBoxName.Substring(0, newBoxName.IndexOf("-")).Replace("Figure", ""));
							figurenr++;
							newBoxName = "Figure" + figurenr.ToString() + "-1";
							nextBox = getSafeTextBoxFigure(newBoxName);
						}

						if (nextBox != null)
						{
							nextBox.Focus();
							nextBox.SelectAll();
						}
					}
				}
				else
				{
					nextBox.Focus();
					nextBox.SelectAll();
				}
			}
			catch (Exception exc)
			{
				Trace.WriteLine("Exception: " + exc.ToString());
			}

			Trace.WriteLine("FResults: figureBox_TextChanged ended");
		}

		private int getStationNr(string currentTextBoxName)
		{
			// Current name is "Figure" + stationNr +"-" + figure
			string boxName = currentTextBoxName.Replace("Figure", "").Replace("Points", "");
			int length = 2;
			if (boxName.Length < 2)
				length = boxName.Length;
			int stationNr = int.Parse(boxName.Substring(0,length).Replace("-", ""));

			return stationNr;
		}
		private int getFigureNr(string currentTextBoxName)
		{
			int index = currentTextBoxName.IndexOf("-");
			if (index == -1)
			{
				// this is a points
				return 100;
			}
			int figureNr = int.Parse(currentTextBoxName.Substring(index+1, currentTextBoxName.Length-index-1));
			return figureNr;
		}
		private SafeTextBox getNextTextBox(string currentTextBoxName)
		{
			int stationNr = getStationNr(currentTextBoxName);
			int figureNr = getFigureNr(currentTextBoxName);

			string newName1 = "Figure" + stationNr.ToString() + "-" + (figureNr+1).ToString();
			string newName2 = "Figure" + (stationNr+1).ToString() + "-1";

			SafeTextBox nextBox = getSafeTextBoxFigure(newName1);

			if (nextBox == null)
			{
				if (chkMoveNextShooter.Checked)
				{
					// If we couldn't find next figure and movenextshooter is
					// checked, we should move to next shooter.
					// That is done in calling method.
					return null;
				}
				
				// Couldn't find this SafeTextBox
				nextBox = getSafeTextBoxPoints(stationNr);
				if ((nextBox == null)  | (figureNr >= 100))
				{
					// Couldn't get Points for this stn?!?
					nextBox = getSafeTextBoxFigure(newName2);
					if (nextBox == null)
					{
						// There is no next station
						this.ddCompetitors.Focus();
						return null;
					}
				}
				if ( nextBox.Enabled == false)
				{
					// Points not enabled for this stn
					nextBox = getSafeTextBoxFigure(newName2);
					if (nextBox == null)
					{
						// There is no next station
						return null;
					}
				}
				else
				{
					// If there is 0 hits on this station, 
					// set 0 as points and continue
					if (calculateFigureHits(stationNr) == 0)
					{
						nextBox.Text = "0";
						nextBox = getSafeTextBoxFigure(newName2);
					}
				}
			}
			return nextBox;
		}

		private int getResultsForStationTot(string stationNr, int nrOfFigures)
		{
			int result = 0;
			for(int i = 1; i<=nrOfFigures; i++)
			{
				SafeTextBox thisBox = getSafeTextBoxFigure("Figure" + stationNr + "-" + i.ToString());
				try
				{
					result = result + int.Parse(thisBox.Text);
				}
				catch(Exception)
				{
				}
			}
			return result;
		}

		private SafeTextBox getSafeTextBoxFigure(string name)
		{
			Trace.WriteLine("FResults: getSafeTextBoxFigure(" + name + " started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
				DateTime.Now.ToLongTimeString());

			foreach(SafeTextBox searchBox in this.resultFigures.ToArray(new SafeTextBox().GetType()))
			{
				if (searchBox.Name == name)
				{
					// Found box 
					Trace.WriteLine("FResults: getSafeTextBoxFigure() ended.");
					return searchBox;
				}
			}

			Trace.WriteLine("FResults: getSafeTextBoxFigure() ended.");
			return null;
		}

		private SafeTextBox getSafeTextBoxPoints(int stationNr)
		{
			string name = "Points" + stationNr.ToString();
			foreach(SafeTextBox searchBox in this.resultPoints.ToArray(new SafeTextBox().GetType()))
			{
				if (searchBox.Name == name)
				{
					// Found box 
					return searchBox;
				}
			}

			return null;
		}
		private void chkCalculateResults_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.chkCalculateResults.Checked)
			{
				calculateResults();
			}
			else
			{
				this.txtResults.Text = "";
			}
		}
		#endregion


		#region ResultsCalculation
		private int calculateFigureHits(int stationNr)
		{
			int figureHits = 0;
			foreach(SafeTextBox safeTextBox in resultFigures.ToArray(typeof(SafeTextBox)))
			{
				if (safeTextBox.Name.StartsWith("Figure" + stationNr.ToString() + "-"))
				{
					int parse = 0;
					try
					{
						parse = int.Parse(safeTextBox.Text);
						if (parse > 0)
							figureHits++;
					}
					catch(Exception)
					{
					}
				}
			}
			return figureHits;
		}
		#endregion



	}
}
