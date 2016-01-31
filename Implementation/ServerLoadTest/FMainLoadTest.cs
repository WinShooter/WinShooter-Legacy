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
// $Id: FMainLoadTest.cs 107 2009-02-01 06:25:33Z smuda $ 
using Allberg.Shooter.WinShooterServerRemoting;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Data;

namespace ServerLoadTest
{
	/// <summary>
	/// Summary description for FMain.
	/// </summary>
	public class FMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtServerAddress;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.CheckBox chkAddShooters;
		private System.Windows.Forms.CheckBox chkAddResults;
		private System.Windows.Forms.CheckBox chkViewResults;
		private System.Windows.Forms.Label lblShooters;
		private System.Windows.Forms.Label lblSetResults;
		private System.Windows.Forms.Label lblGetResults;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		public FMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CommonCode = new Allberg.Shooter.Common.Interface();
		}

		Allberg.Shooter.Common.Interface CommonCode;

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
			this.label1 = new System.Windows.Forms.Label();
			this.txtServerAddress = new System.Windows.Forms.TextBox();
			this.btnConnect = new System.Windows.Forms.Button();
			this.btnTest = new System.Windows.Forms.Button();
			this.chkAddShooters = new System.Windows.Forms.CheckBox();
			this.chkAddResults = new System.Windows.Forms.CheckBox();
			this.chkViewResults = new System.Windows.Forms.CheckBox();
			this.lblShooters = new System.Windows.Forms.Label();
			this.lblSetResults = new System.Windows.Forms.Label();
			this.lblGetResults = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Server";
			// 
			// txtServerAddress
			// 
			this.txtServerAddress.Location = new System.Drawing.Point(64, 8);
			this.txtServerAddress.Name = "txtServerAddress";
			this.txtServerAddress.Size = new System.Drawing.Size(224, 20);
			this.txtServerAddress.TabIndex = 1;
			this.txtServerAddress.Text = "localhost";
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(64, 32);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(75, 23);
			this.btnConnect.TabIndex = 2;
			this.btnConnect.Text = "Connect";
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// btnTest
			// 
			this.btnTest.Enabled = false;
			this.btnTest.Location = new System.Drawing.Point(144, 32);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(75, 23);
			this.btnTest.TabIndex = 3;
			this.btnTest.Text = "Run test";
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// chkAddShooters
			// 
			this.chkAddShooters.Location = new System.Drawing.Point(64, 64);
			this.chkAddShooters.Name = "chkAddShooters";
			this.chkAddShooters.Size = new System.Drawing.Size(224, 24);
			this.chkAddShooters.TabIndex = 4;
			this.chkAddShooters.Text = "Lägg till Skyttar";
			// 
			// chkAddResults
			// 
			this.chkAddResults.Location = new System.Drawing.Point(64, 88);
			this.chkAddResults.Name = "chkAddResults";
			this.chkAddResults.Size = new System.Drawing.Size(224, 24);
			this.chkAddResults.TabIndex = 5;
			this.chkAddResults.Text = "Fyll i resultat";
			// 
			// chkViewResults
			// 
			this.chkViewResults.Location = new System.Drawing.Point(64, 112);
			this.chkViewResults.Name = "chkViewResults";
			this.chkViewResults.Size = new System.Drawing.Size(224, 24);
			this.chkViewResults.TabIndex = 6;
			this.chkViewResults.Text = "Hämta resultat";
			// 
			// lblShooters
			// 
			this.lblShooters.Location = new System.Drawing.Point(184, 64);
			this.lblShooters.Name = "lblShooters";
			this.lblShooters.Size = new System.Drawing.Size(64, 23);
			this.lblShooters.TabIndex = 7;
			this.lblShooters.Text = "0";
			this.lblShooters.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblSetResults
			// 
			this.lblSetResults.Location = new System.Drawing.Point(192, 88);
			this.lblSetResults.Name = "lblSetResults";
			this.lblSetResults.Size = new System.Drawing.Size(56, 23);
			this.lblSetResults.TabIndex = 8;
			this.lblSetResults.Text = "0";
			this.lblSetResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblGetResults
			// 
			this.lblGetResults.Location = new System.Drawing.Point(216, 112);
			this.lblGetResults.Name = "lblGetResults";
			this.lblGetResults.Size = new System.Drawing.Size(32, 23);
			this.lblGetResults.TabIndex = 9;
			this.lblGetResults.Text = "0";
			this.lblGetResults.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// timer1
			// 
			this.timer1.Interval = 6000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// FMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 142);
			this.Controls.Add(this.lblGetResults);
			this.Controls.Add(this.lblSetResults);
			this.Controls.Add(this.lblShooters);
			this.Controls.Add(this.chkViewResults);
			this.Controls.Add(this.chkAddResults);
			this.Controls.Add(this.chkAddShooters);
			this.Controls.Add(this.btnTest);
			this.Controls.Add(this.btnConnect);
			this.Controls.Add(this.txtServerAddress);
			this.Controls.Add(this.label1);
			this.Name = "FMain";
			this.Text = "FMain";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FMain());
		}

		private void btnConnect_Click(object sender, System.EventArgs e)
		{
			try
			{
				CommonCode.ServerConnect("localhost");
				this.btnConnect.Enabled = false;
				this.btnTest.Enabled = true;
			}
			catch(Exception exc)
			{
				MessageBox.Show(exc.ToString());
			}
		}

		private void btnTest_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (!this.timer1.Enabled)
				{
					this.timer1.Enabled = true;
					this.btnTest.Text = "Stop test";
				}
				else
				{
					this.timer1.Enabled = false;
					this.btnTest.Text = "Start test";
				}
			}
			catch(Exception exc)
			{
				MessageBox.Show(exc.ToString());
			}
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			try
			{
				Thread.CurrentThread.Name = "Main Thread";
			}
			catch(Exception)
			{
			}

			Trace.WriteLine("timer1_Tick on Thread \" " + 
				Thread.CurrentThread.Name + "\"" +
				Thread.CurrentThread.ManagedThreadId.ToString());

			if (chkAddShooters.Checked)
			{
				Thread addShootersThread = new Thread(new ThreadStart(timerAddShooters));
				addShootersThread.IsBackground = true;
				addShootersThread.Name = "timerAddShooters";
				//addShootersThread.Start();
				timerAddShooters();
			}

			if (chkAddResults.Checked)
			{
				Thread addResultsThread = new Thread(new ThreadStart(timerAddResults));
				addResultsThread.IsBackground = true;
				addResultsThread.Name = "timerAddResults";
				addResultsThread.Start();
			}

			if (chkViewResults.Checked)
			{
				Thread viewResultsThread = new Thread(new ThreadStart(timerViewResults));
				viewResultsThread.IsBackground = true;
				viewResultsThread.Name = "timerViewResults";
				viewResultsThread.Start();
			}
		}

		Random rnd = new Random();

		#region RunningTest
		#region Add Shooters
		bool timerAddShootersRunning = false;
		readonly object timerAddShootersRunningLock = new object();
		private void timerAddShooters()
		{
			try
			{
				lock (timerAddShootersRunningLock)
				{
					if (timerAddShootersRunning)
						return;
					timerAddShootersRunning = true;
				}
				Trace.WriteLine("timerAddShooters_Tick on Thread \" " + 
					Thread.CurrentThread.Name + "\"" +
					Thread.CurrentThread.ManagedThreadId.ToString());

				Structs.Shooter shooter = new Structs.Shooter();
				shooter.Automatic = false;
				shooter.CardNr = rnd.Next(100000).ToString();
				int shooterClassInt = -1;
				while(((Structs.ShootersClass)shooterClassInt).ToString() ==
					shooterClassInt.ToString())
				{
					shooterClassInt = rnd.Next(2)+1;
				}
				shooter.Class = (Structs.ShootersClass)shooterClassInt;
				shooter.ClubId = "01-417";
				shooter.Email = "email@email.com";
				shooter.Givenname = shooter.CardNr;
				shooter.Payed = 0;
				shooter.Surname = shooter.CardNr;
				shooter.ToAutomatic = false;
				int shooterId = CommonCode.NewShooter(shooter);

				int nrOfCompetitors = 1 + rnd.Next(3);
				for(int i=0; i<nrOfCompetitors; i++)
				{
					Structs.Competitor comp = new Structs.Competitor();
					comp.CompetitionId = CommonCode.GetCompetitions()[0].CompetitionId;
					while(comp.Lane == 0)
					{
						try
						{
							comp.PatrolId = 1+rnd.Next(80);
							bool retry = true;
							while (retry)
							{
								try
								{
									CommonCode.GetPatrol(comp.PatrolId);
									retry = false;
								}
								catch (Allberg.Shooter.Common.CannotFindIdException)
								{
									CommonCode.PatrolAddEmpty();
								}
							}
							comp.Lane = CommonCode.PatrolGetNextLane(comp.PatrolId);
						}
						catch(Exception)
						{
						}
					}
					comp.ShooterClass = shooter.Class;
					comp.ShooterId = shooterId;
					comp.WeaponId = "G9-A1";
					CommonCode.NewCompetitor(comp);
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine("Exception while adding competitor:" + exc.ToString());
			}
			finally
			{
				timerAddShootersRunning = false;
			}
		}
		#endregion
		#region Add Results
		bool timerAddResultsRunning = false;
		readonly object timerAddResultsRunningLock = new object();
		private void timerAddResults()
		{
			try
			{
				lock(timerAddResultsRunningLock)
				{
					if (timerAddResultsRunning)
						return;
					timerAddResultsRunning = true;
				}
				Trace.WriteLine("timerAddResults_Tick on Thread \"" + 
					Thread.CurrentThread.Name + "\" " +
					Thread.CurrentThread.ManagedThreadId.ToString());

				bool retry = true;
				Structs.Competitor comp = new Structs.Competitor();
				while (retry)
				{
					Structs.Competitor[] comps = CommonCode.GetCompetitors();
					if (comps.Length == 0)
						return;
					comp = comps[rnd.Next(comps.Length)];
					Structs.CompetitorResult[] results;
					try
					{
						results = CommonCode.GetCompetitorResults(comp.CompetitorId);
						if (results.Length < 2)
							retry = false;
					}
					catch(Exception exc)
					{
						Trace.WriteLine(exc.ToString());
						retry = false;
					}
				}
				Structs.Station[] stations = CommonCode.GetStations();
				foreach(Structs.Station station in stations)
				{
					Structs.CompetitorResult res = new Structs.CompetitorResult();
					res.CompetitorId = comp.CompetitorId;
					res.FigureHits = rnd.Next(station.Figures);
					res.Hits = rnd.Next(station.Shoots);
					res.Points = 0;
					res.Station = station.StationId;
					res.StationFigureHits = res.Hits.ToString() + ";";
					CommonCode.NewCompetitorResult(res);
				}
			}
			finally
			{
				timerAddResultsRunning = false;
			}
		}
		#endregion
		#region View Results
		bool timerViewResultsRunning = false;
		readonly object timerViewResultsRunningLock = new object();
		private void timerViewResults()
		{
			try
			{
				lock(timerViewResultsRunningLock)
				{
					if (timerViewResultsRunning)
						return;
					timerViewResultsRunning = true;
				}
				Trace.WriteLine("timerAddResults_Tick on Thread \"" + 
					Thread.CurrentThread.Name + "\" " +
					Thread.CurrentThread.ManagedThreadId.ToString());

				ResultsReturn[] results =
					CommonCode.ResultsGet(Structs.ResultWeaponsClass.Unknown,
					Structs.ShootersClass.Öppen, CommonCode.GetCompetitions()[0], false);

				Trace.WriteLine("timerViewResults_Tick got " + results.Length.ToString() +
					" results.");
			}
			finally
			{
				timerViewResultsRunning = false;
			}
		}
		#endregion
		#endregion

	}
}
