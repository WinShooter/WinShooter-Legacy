using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for FStations.
	/// </summary>
	public class FStations : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btnSave;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel1;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel2;
		private System.Windows.Forms.NumericUpDown numDdStationsTot;
		private System.Windows.Forms.ProgressBar progressBarKonfig;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel3;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private Allberg.Shooter.Windows.Forms.SafeLabel lblStationNr;
		private System.Windows.Forms.GroupBox groupBox1;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel4;
		private System.Windows.Forms.CheckBox chkPoints;
		private System.Windows.Forms.NumericUpDown numNumberOfShoots;
		private System.Windows.Forms.NumericUpDown numNumberOfFigures;
		private System.Windows.Forms.GroupBox groupBoxSample;
		private Allberg.Shooter.Windows.Forms.SafeTextBox txtPoints;
		private Allberg.Shooter.Windows.Forms.SafeLabel lblPoints;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ToolTip toolTip1;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel5;

		internal FStations(ref Common.Interface newCommon)
		{
			Trace.WriteLine("FStations: Creating on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				AppDomain.GetCurrentThreadId().ToString() + " )");
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CommonCode = newCommon;

			height = this.Size.Height;
			width = this.Size.Width;
			this.Resize += new EventHandler(FStations_Resize);
			Trace.WriteLine("FStations: Succesfully created.");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			Trace.WriteLine("FStations: Dispose(" + disposing.ToString() + ")" +
				"from thread \"" + System.Threading.Thread.CurrentThread.Name + "\" " +
				" ( " + System.AppDomain.GetCurrentThreadId().ToString() + " ) " +
				DateTime.Now.ToLongTimeString());

			if(!DisposeNow)
			{
				saveCurrentStation();
				numDdStationsTot_ValueChanged();

				if (totalNumberOfStationsDone<totalNumberOfStations)
				{
					MessageBox.Show("Du måste konfigurera alla stationer");
					return;
				}

				this.Visible = false;
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

			Trace.WriteLine("FStations: Dispose(" + disposing.ToString() + ")" +
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FStations));
			this.btnSave = new System.Windows.Forms.Button();
			this.SafeLabel1 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.numDdStationsTot = new System.Windows.Forms.NumericUpDown();
			this.SafeLabel2 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.progressBarKonfig = new System.Windows.Forms.ProgressBar();
			this.SafeLabel3 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.lblStationNr = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkPoints = new System.Windows.Forms.CheckBox();
			this.numNumberOfShoots = new System.Windows.Forms.NumericUpDown();
			this.SafeLabel5 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.numNumberOfFigures = new System.Windows.Forms.NumericUpDown();
			this.SafeLabel4 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.groupBoxSample = new System.Windows.Forms.GroupBox();
			this.txtPoints = new Allberg.Shooter.Windows.Forms.SafeTextBox();
			this.lblPoints = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.numDdStationsTot)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numNumberOfShoots)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numNumberOfFigures)).BeginInit();
			this.groupBoxSample.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(168, 192);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 11;
			this.btnSave.Text = "Spara";
			this.toolTip1.SetToolTip(this.btnSave, "Spara stationskonfiguration och stäng fönstret.");
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// SafeLabel1
			// 
			this.SafeLabel1.Location = new System.Drawing.Point(8, 8);
			this.SafeLabel1.Name = "SafeLabel1";
			this.SafeLabel1.TabIndex = 2;
			this.SafeLabel1.Text = "Antal stationer";
			// 
			// numDdStationsTot
			// 
			this.numDdStationsTot.Location = new System.Drawing.Point(120, 8);
			this.numDdStationsTot.Maximum = new System.Decimal(new int[] {
																			 15,
																			 0,
																			 0,
																			 0});
			this.numDdStationsTot.Minimum = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.numDdStationsTot.Name = "numDdStationsTot";
			this.numDdStationsTot.Size = new System.Drawing.Size(72, 20);
			this.numDdStationsTot.TabIndex = 3;
			this.numDdStationsTot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.numDdStationsTot, "Välj hur många stationer som ska finnas");
			this.numDdStationsTot.Value = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.numDdStationsTot.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numDdStationsTot_KeyUp);
			this.numDdStationsTot.ValueChanged += new System.EventHandler(this.numDdStationsTot_ValueChanged);
			// 
			// SafeLabel2
			// 
			this.SafeLabel2.Location = new System.Drawing.Point(8, 32);
			this.SafeLabel2.Name = "SafeLabel2";
			this.SafeLabel2.Size = new System.Drawing.Size(112, 23);
			this.SafeLabel2.TabIndex = 4;
			this.SafeLabel2.Text = "Konfigurationsstatus";
			// 
			// progressBarKonfig
			// 
			this.progressBarKonfig.Location = new System.Drawing.Point(120, 32);
			this.progressBarKonfig.Maximum = 1;
			this.progressBarKonfig.Name = "progressBarKonfig";
			this.progressBarKonfig.Size = new System.Drawing.Size(72, 23);
			this.progressBarKonfig.TabIndex = 5;
			this.toolTip1.SetToolTip(this.progressBarKonfig, "Denna visar konfigurationsstatus. Samtliga stationer måste konfigureras.");
			// 
			// SafeLabel3
			// 
			this.SafeLabel3.Location = new System.Drawing.Point(8, 16);
			this.SafeLabel3.Name = "SafeLabel3";
			this.SafeLabel3.TabIndex = 6;
			this.SafeLabel3.Text = "Stationsnummer";
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.vScrollBar1.LargeChange = 1;
			this.vScrollBar1.Location = new System.Drawing.Point(160, 16);
			this.vScrollBar1.Maximum = 1;
			this.vScrollBar1.Minimum = 1;
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(16, 96);
			this.vScrollBar1.TabIndex = 7;
			this.toolTip1.SetToolTip(this.vScrollBar1, "Använd denna för att gå från station till station");
			this.vScrollBar1.Value = 1;
			this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
			// 
			// lblStationNr
			// 
			this.lblStationNr.Location = new System.Drawing.Point(112, 16);
			this.lblStationNr.Name = "lblStationNr";
			this.lblStationNr.Size = new System.Drawing.Size(40, 23);
			this.lblStationNr.TabIndex = 8;
			this.lblStationNr.Text = "1";
			this.lblStationNr.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.toolTip1.SetToolTip(this.lblStationNr, "Här visas vilket stationsnr som just nu håller på att konfigureras.");
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkPoints);
			this.groupBox1.Controls.Add(this.numNumberOfShoots);
			this.groupBox1.Controls.Add(this.SafeLabel5);
			this.groupBox1.Controls.Add(this.numNumberOfFigures);
			this.groupBox1.Controls.Add(this.SafeLabel4);
			this.groupBox1.Controls.Add(this.SafeLabel3);
			this.groupBox1.Controls.Add(this.lblStationNr);
			this.groupBox1.Controls.Add(this.vScrollBar1);
			this.groupBox1.Location = new System.Drawing.Point(8, 64);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(184, 116);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Aktuell station";
			this.toolTip1.SetToolTip(this.groupBox1, "Data om aktuell station");
			// 
			// chkPoints
			// 
			this.chkPoints.Location = new System.Drawing.Point(8, 88);
			this.chkPoints.Name = "chkPoints";
			this.chkPoints.Size = new System.Drawing.Size(144, 24);
			this.chkPoints.TabIndex = 13;
			this.chkPoints.Text = "Poängtavla/or";
			this.toolTip1.SetToolTip(this.chkPoints, "Fyll i om det finns poängmål på denna station");
			this.chkPoints.CheckedChanged += new System.EventHandler(this.chkPoints_CheckedChanged);
			// 
			// numNumberOfShoots
			// 
			this.numNumberOfShoots.Location = new System.Drawing.Point(112, 64);
			this.numNumberOfShoots.Maximum = new System.Decimal(new int[] {
																			  6,
																			  0,
																			  0,
																			  0});
			this.numNumberOfShoots.Minimum = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.numNumberOfShoots.Name = "numNumberOfShoots";
			this.numNumberOfShoots.Size = new System.Drawing.Size(40, 20);
			this.numNumberOfShoots.TabIndex = 12;
			this.toolTip1.SetToolTip(this.numNumberOfShoots, "Fyll i hur många skott skyttarna får använda på denna station");
			this.numNumberOfShoots.Value = new System.Decimal(new int[] {
																			6,
																			0,
																			0,
																			0});
			this.numNumberOfShoots.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numNumberOfShoots_KeyUp);
			this.numNumberOfShoots.ValueChanged += new System.EventHandler(this.numNumberOfShoots_ValueChanged);
			// 
			// SafeLabel5
			// 
			this.SafeLabel5.Location = new System.Drawing.Point(8, 64);
			this.SafeLabel5.Name = "SafeLabel5";
			this.SafeLabel5.TabIndex = 11;
			this.SafeLabel5.Text = "Antal skott";
			// 
			// numNumberOfFigures
			// 
			this.numNumberOfFigures.Location = new System.Drawing.Point(112, 40);
			this.numNumberOfFigures.Maximum = new System.Decimal(new int[] {
																			   6,
																			   0,
																			   0,
																			   0});
			this.numNumberOfFigures.Minimum = new System.Decimal(new int[] {
																			   1,
																			   0,
																			   0,
																			   0});
			this.numNumberOfFigures.Name = "numNumberOfFigures";
			this.numNumberOfFigures.Size = new System.Drawing.Size(40, 20);
			this.numNumberOfFigures.TabIndex = 10;
			this.toolTip1.SetToolTip(this.numNumberOfFigures, "Fyll i hur många figurer som ska visas på denna station");
			this.numNumberOfFigures.Value = new System.Decimal(new int[] {
																			 1,
																			 0,
																			 0,
																			 0});
			this.numNumberOfFigures.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numNumberOfFigures_KeyUp);
			this.numNumberOfFigures.ValueChanged += new System.EventHandler(this.numNumberOfFigures_ValueChanged);
			// 
			// SafeLabel4
			// 
			this.SafeLabel4.Location = new System.Drawing.Point(8, 40);
			this.SafeLabel4.Name = "SafeLabel4";
			this.SafeLabel4.TabIndex = 9;
			this.SafeLabel4.Text = "Antal figurer";
			// 
			// groupBoxSample
			// 
			this.groupBoxSample.Controls.Add(this.txtPoints);
			this.groupBoxSample.Controls.Add(this.lblPoints);
			this.groupBoxSample.Location = new System.Drawing.Point(200, 12);
			this.groupBoxSample.Name = "groupBoxSample";
			this.groupBoxSample.Size = new System.Drawing.Size(120, 168);
			this.groupBoxSample.TabIndex = 10;
			this.groupBoxSample.TabStop = false;
			this.groupBoxSample.Text = "Stationsexempel";
			this.toolTip1.SetToolTip(this.groupBoxSample, "Här visas ett exempel på hur det kommer att se ut vid ifyllnad av resultaten");
			// 
			// txtPoints
			// 
			this.txtPoints.Enabled = false;
			this.txtPoints.Location = new System.Drawing.Point(56, 136);
			this.txtPoints.Name = "txtPoints";
			this.txtPoints.Size = new System.Drawing.Size(56, 20);
			this.txtPoints.TabIndex = 1;
			this.txtPoints.Text = "";
			this.txtPoints.Visible = false;
			// 
			// lblPoints
			// 
			this.lblPoints.Location = new System.Drawing.Point(8, 136);
			this.lblPoints.Name = "lblPoints";
			this.lblPoints.Size = new System.Drawing.Size(40, 23);
			this.lblPoints.TabIndex = 0;
			this.lblPoints.Text = "Poäng";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(248, 192);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "Stäng";
			this.toolTip1.SetToolTip(this.btnCancel, "Stäng fönstret");
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// FStations
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(328, 221);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBoxSample);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.progressBarKonfig);
			this.Controls.Add(this.SafeLabel2);
			this.Controls.Add(this.numDdStationsTot);
			this.Controls.Add(this.SafeLabel1);
			this.Controls.Add(this.btnSave);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "FStations";
			this.Text = "Stationer";
			((System.ComponentModel.ISupportInitialize)(this.numDdStationsTot)).EndInit();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numNumberOfShoots)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numNumberOfFigures)).EndInit();
			this.groupBoxSample.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		internal bool DisposeNow = false;
		public delegate void EnableMainHandler();
		public event EnableMainHandler EnableMain;
		private ArrayList stationList;

		Common.Interface CommonCode;
		int height;
		int width;

		int totalNumberOfStations = 1;
		int totalNumberOfStationsDone = 0;

		internal void enableMe()
		{
			Trace.WriteLine("FStations: enableMe started on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				AppDomain.GetCurrentThreadId().ToString() + " )");

			this.Visible = true;
			this.Focus();

			UpdatedStations();

			totalNumberOfStations = stationList.Count;
			if (totalNumberOfStations > numDdStationsTot.Value)
				numDdStationsTot.Value = totalNumberOfStations;

			if (totalNumberOfStations > progressBarKonfig.Maximum)
			{
				progressBarKonfig.Maximum = totalNumberOfStations;
				progressBarKonfig.Value = totalNumberOfStations;
			}
			if (totalNumberOfStations > this.vScrollBar1.Maximum)
				vScrollBar1.Maximum = totalNumberOfStations;
			setCurrentStation(1);

			Trace.WriteLine("FStations: enableMe ended.");
		}

		private void FStations_Resize(object sender, EventArgs e)
		{
			Size size = new Size(this.width, this.height);
			this.Size = size;
		}

		internal void UpdatedStations()
		{
			Trace.WriteLine("FStations: UpdatedStations on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				AppDomain.GetCurrentThreadId().ToString() + " )");

			if (!this.Visible)
				return;

			stationList = new ArrayList();
			Structs.Station[] stations = 
				this.CommonCode.GetStations();
			foreach(Structs.Station station in
				stations)
			{
				stationList.Add(station);
			}
			totalNumberOfStationsDone = stationList.Count;
			if (this.progressBarKonfig.Maximum < totalNumberOfStationsDone)
				this.progressBarKonfig.Maximum = totalNumberOfStationsDone;
			this.progressBarKonfig.Value = totalNumberOfStationsDone;

			Trace.WriteLine("FStations: UpdatedStations ended.");
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			Trace.WriteLine("FStations: btnSave_Click on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				AppDomain.GetCurrentThreadId().ToString() + " )");

			saveCurrentStation();
			while (totalNumberOfStationsDone != CommonCode.GetStationsCount())
				Thread.Sleep(50);

			if (totalNumberOfStationsDone<totalNumberOfStations)
			{
				MessageBox.Show("Du måste konfigurera alla stationer");
			}
			else
			{
				// Go to main window
				this.Visible = false;
				try
				{
					EnableMain();
				}
				catch(Exception)
				{
				}
			}

			Trace.WriteLine("FStations: btnSave_Click ended.");
		}

		private void numDdStationsTot_ValueChanged(object sender, System.EventArgs e)
		{
			numDdStationsTot_ValueChanged();
		}
		private void numDdStationsTot_KeyUp(object sender, KeyEventArgs e)
		{
			numDdStationsTot_ValueChanged();
		}


		private void numDdStationsTot_ValueChanged()
		{
			Trace.WriteLine("FStations: numDdStationsTot_ValueChanged on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				AppDomain.GetCurrentThreadId().ToString() + " )");

			// Check if a stations is to be removed
			if ((int)this.numDdStationsTot.Value < this.totalNumberOfStations)
			{
				foreach(Structs.Station station in stationList)
				{
					if (station.StationNr == this.totalNumberOfStations)
					{
						// Check if they're sure
						DialogResult res =
							MessageBox.Show("Är du säker? Detta kommer att " + 
							"radera station nr " + 
							this.totalNumberOfStations.ToString() + 
							".", 
							"Konfirmera radering", 
							MessageBoxButtons.YesNo, 
							MessageBoxIcon.Question);
						if (res == DialogResult.No)
						{
							this.numDdStationsTot.Value = 
								this.totalNumberOfStations;
							return;
						}
						// Set current station to 1
						this.vScrollBar1.Value = 1;
						this.setCurrentStation(1);

						// Delete station
						CommonCode.DelStation(station);
					}
				}
			}
			// Update all maximum
			totalNumberOfStations = (int)numDdStationsTot.Value;
			progressBarKonfig.Maximum = totalNumberOfStations;
			this.vScrollBar1.Maximum = totalNumberOfStations;

			Trace.WriteLine("FStations: numDdStationsTot_ValueChanged ended.");
		}

		private void vScrollBar1_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			Trace.WriteLine("FStations: vScrollBar1_Scroll on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				AppDomain.GetCurrentThreadId().ToString() + " )");

			saveCurrentStation();
			setCurrentStation(this.vScrollBar1.Value);

			Trace.WriteLine("FStations: vScrollBar1_Scroll ended.");
		}

		private void saveCurrentStation()
		{
			Trace.WriteLine("FStations: saveCurrentStation on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				AppDomain.GetCurrentThreadId().ToString() + " )");

			if (CommonCode.GetCompetitions().Length == 0)
				return;

			foreach(Structs.Station station in stationList)
			{
				if (station.StationNr == int.Parse(this.lblStationNr.Text))
				{
					// Found station, so change it by creating a new and 
					// updating with that
					Structs.Station updStation = station;

					updStation.CompetitionId = station.CompetitionId;
					updStation.Figures = (int)this.numNumberOfFigures.Value;
					updStation.Shoots = (int)this.numNumberOfShoots.Value;
					updStation.Points = this.chkPoints.Checked;
					updStation.StationId = station.StationId;
					updStation.StationNr = station.StationNr;

					CommonCode.UpdateStation(updStation);
					Trace.WriteLine("FStations: saveCurrentStation ended.");
					return;
				}
			}
			Structs.Station newStation = new Structs.Station();
			newStation.StationId = int.Parse(this.lblStationNr.Text);
			newStation.CompetitionId = CommonCode.GetCompetitions()[0].CompetitionId;
			newStation.Figures = (int)this.numNumberOfFigures.Value;
			newStation.Points = this.chkPoints.Checked;
			newStation.Shoots = (int)this.numNumberOfShoots.Value;
			newStation.StationNr = int.Parse(this.lblStationNr.Text);
			CommonCode.NewStation(newStation);

			Trace.WriteLine("FStations: saveCurrentStation ended.");
		}

		private void setCurrentStation(int stnNr)
		{
			Trace.WriteLine("FStations.setCurrentStation(" + stnNr.ToString()
				+ ") started on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.AppDomain.GetCurrentThreadId().ToString() + " )");

			// Implement default values
			this.lblStationNr.Text = stnNr.ToString();
			this.numNumberOfFigures.Value = 3;
			this.numNumberOfShoots.Value = 6;
			this.vScrollBar1.Value = stnNr;
			this.chkPoints.Checked = false;

			// Find station and set those values
			foreach(Structs.Station station in stationList)
			{
				if (station.StationNr == stnNr)
				{
					Trace.WriteLine("FStations: setCurrentStation found " +
						"station " + stnNr.ToString());

					this.lblStationNr.Text = stnNr.ToString();
					this.numNumberOfFigures.Value = station.Figures;
					this.numNumberOfShoots.Value = station.Shoots;
					this.chkPoints.Checked = station.Points;

					Trace.WriteLine("FStations: setCurrentStation ended.");
					return;
				}

			}
			viewStationSample();
			lblPoints.Visible = chkPoints.Checked;
			txtPoints.Visible = chkPoints.Checked;

			Trace.WriteLine("FStations: setCurrentStation ended.");
		}


	
		//const int groupBoxStartX = 8;
		//const int groupBoxStartY = 80;
		//const int groupBoxNextY = 110;
		//const int groupBoxNextX = 140;
		Forms.SafeTextBox txtBox1 = new Forms.SafeTextBox();
		Forms.SafeTextBox txtBox2 = new Forms.SafeTextBox();
		Forms.SafeTextBox txtBox3 = new Forms.SafeTextBox();
		Forms.SafeTextBox txtBox4 = new Forms.SafeTextBox();
		Forms.SafeTextBox txtBox5 = new Forms.SafeTextBox();
		Forms.SafeTextBox txtBox6 = new Forms.SafeTextBox();

		private void viewStationSample()
		{
			Trace.WriteLine("FStations: viewStationSample on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				AppDomain.GetCurrentThreadId().ToString() + " )");

			txtBox1.Location = new System.Drawing.Point(16,24);
			txtBox2.Location = new System.Drawing.Point(16,48);
			txtBox3.Location = new System.Drawing.Point(16,72);
			txtBox4.Location = new System.Drawing.Point(60,24);
			txtBox5.Location = new System.Drawing.Point(60,48);
			txtBox6.Location = new System.Drawing.Point(60,72);

			this.groupBoxSample.Controls.Add(txtBox1);
			this.groupBoxSample.Controls.Add(txtBox2);
			this.groupBoxSample.Controls.Add(txtBox3);
			this.groupBoxSample.Controls.Add(txtBox4);
			this.groupBoxSample.Controls.Add(txtBox5);
			this.groupBoxSample.Controls.Add(txtBox6);

			txtBox1.Visible = false;
			txtBox2.Visible = false;
			txtBox3.Visible = false;
			txtBox4.Visible = false;
			txtBox5.Visible = false;
			txtBox6.Visible = false;

			txtBox1.Enabled = false;
			txtBox2.Enabled = false;
			txtBox3.Enabled = false;
			txtBox4.Enabled = false;
			txtBox5.Enabled = false;
			txtBox6.Enabled = false;

			// add new info to box
			switch((int)this.numNumberOfFigures.Value)
			{
				case 1:
					//txtBox1.Location = new System.Drawing.Point(16,24);
					txtBox1.Size = new Size(88,20);
					//this.groupBoxSample.Controls.Add(txtBox1);
					txtBox1.Visible = true;
					break;
				case 2:
					//txtBox1.Location = new System.Drawing.Point(16,24);
					txtBox1.Size = new Size(88,20);
					txtBox1.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox1);

					//txtBox2.Location = new System.Drawing.Point(16,48);
					txtBox2.Size = new Size(88,20);
					txtBox2.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox2);
					
					break;
				case 3:
					//txtBox1.Location = new System.Drawing.Point(16,24);
					txtBox1.Size = new Size(88,20);
					txtBox1.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox1);

					//txtBox2.Location = new System.Drawing.Point(16,48);
					txtBox2.Size = new Size(88,20);
					txtBox2.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox2);
					
					//txtBox3.Location = new System.Drawing.Point(16,72);
					txtBox3.Size = new Size(88,20);
					txtBox3.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox3);
					
					break;
				case 4:
					//txtBox1.Location = new System.Drawing.Point(16,24);
					txtBox1.Size = new Size(88,20);
					txtBox1.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox1);

					//txtBox2.Location = new System.Drawing.Point(16,48);
					txtBox2.Size = new Size(44,20);
					txtBox2.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox2);
					
					//txtBox3.Location = new System.Drawing.Point(16,72);
					txtBox3.Size = new Size(88,20);
					txtBox3.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox3);
					
					//txtBox4.Location = new System.Drawing.Point(60,48);
					txtBox5.Size = new Size(44,20);
					txtBox5.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox4);
					
					break;
				case 5:
					//txtBox1.Location = new System.Drawing.Point(16,24);
					txtBox1.Size = new Size(44,20);
					txtBox1.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox1);

					//txtBox2.Location = new System.Drawing.Point(16,48);
					txtBox2.Size = new Size(44,20);
					txtBox2.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox2);
					
					//txtBox3.Location = new System.Drawing.Point(16,72);
					txtBox3.Visible = true;
					txtBox3.Size = new Size(88,20);
					//this.groupBoxSample.Controls.Add(txtBox3);
					
					//txtBox4.Location = new System.Drawing.Point(60,24);
					txtBox4.Size = new Size(44,20);
					txtBox4.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox4);
					
					//txtBox5.Location = new System.Drawing.Point(60,48);
					txtBox5.Size = new Size(44,20);
					txtBox5.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox5);

					break;
				case 6:
					//txtBox1.Location = new System.Drawing.Point(16,24);
					txtBox1.Size = new Size(44,20);
					txtBox1.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox1);

					//txtBox2.Location = new System.Drawing.Point(16,48);
					txtBox2.Size = new Size(44,20);
					txtBox2.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox2);
					
					//txtBox3.Location = new System.Drawing.Point(16,72);
					txtBox3.Size = new Size(44,20);
					txtBox3.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox3);
					
					//txtBox4.Location = new System.Drawing.Point(60,24);
					txtBox4.Size = new Size(44,20);
					txtBox4.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox4);
					
					//txtBox5.Location = new System.Drawing.Point(60,48);
					txtBox5.Size = new Size(44,20);
					txtBox5.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox5);

					//txtBox6.Location = new System.Drawing.Point(60,72);
					txtBox6.Size = new Size(44,20);
					txtBox6.Visible = true;
					//this.groupBoxSample.Controls.Add(txtBox6);

					break;
				default:
					throw new ApplicationException("To high value for number of targets!");
			}

			Trace.WriteLine("FStations: viewStationSample ended.");
		}

		private void numNumberOfFigures_ValueChanged(object sender, System.EventArgs e)
		{
			viewStationSample();
		}
		private void numNumberOfFigures_KeyUp(object sender, KeyEventArgs e)
		{
			if ( this.numNumberOfFigures.Value > this.numNumberOfFigures.Maximum )
				this.numNumberOfFigures.Value = this.numNumberOfFigures.Maximum;

			viewStationSample();
		}

		private void chkPoints_CheckedChanged(object sender, System.EventArgs e)
		{
			this.lblPoints.Visible = this.chkPoints.Checked;
			this.txtPoints.Visible = this.chkPoints.Checked;
		}

		private void numNumberOfShoots_ValueChanged(object sender, System.EventArgs e)
		{
			checkNrOfShoots();
		}
		private void numNumberOfShoots_KeyUp(object sender, KeyEventArgs e)
		{
			checkNrOfShoots();
		}
		private void checkNrOfShoots()
		{
			if (this.numNumberOfShoots.Value > this.numNumberOfShoots.Maximum )
				this.numNumberOfShoots.Value = this.numNumberOfShoots.Maximum;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Dispose();
		}
	}
}
