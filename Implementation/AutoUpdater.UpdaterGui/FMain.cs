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
// $Id: FMain.cs 107 2009-02-01 06:25:33Z smuda $
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace Allberg.AutoUpdater.UpdaterGui
{
	/// <summary>
	/// Summary description for FMain.
	/// </summary>
	public class FMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ProgressBar progressBarTotal;
		private System.Windows.Forms.ProgressBar progressBarCurrentOperation;
		private System.ComponentModel.IContainer components;

		public FMain(string[] args)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			if (args.Length < 3)
			{
				MessageBox.Show("There is not enough arguments.");
				return;
			}

			ProcessName = args[0];
			SourceDir = args[1];
			TargetDir = args[2];

			if (args.Length>3)
			{
				Startup = args[3];

				int i = 4;
				while (args.Length > i)
				{
					if (args[i].IndexOf(" ") > -1)
						StartupArguments += "\"";
					
					StartupArguments += args[i];

					if (args[i].IndexOf(" ") > -1)
						StartupArguments += "\"";

					StartupArguments += " ";
					i++;
				}

				StartupArguments = StartupArguments.Trim();
			}

			timer1.Enabled = true;
		}

		private string ProcessName = "notepad";
		private string SourceDir = "C:\\Temp\\Gurka"; 
		private string TargetDir = "C:\\Temp\\Gurka2";
		private string Startup = "";
		private string StartupArguments = "";

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
			this.label1 = new System.Windows.Forms.Label();
			this.progressBarTotal = new System.Windows.Forms.ProgressBar();
			this.progressBarCurrentOperation = new System.Windows.Forms.ProgressBar();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(232, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Väntar på att programmet ska avslutas";
			// 
			// progressBarTotal
			// 
			this.progressBarTotal.Location = new System.Drawing.Point(8, 64);
			this.progressBarTotal.Name = "progressBarTotal";
			this.progressBarTotal.Size = new System.Drawing.Size(280, 23);
			this.progressBarTotal.TabIndex = 1;
			// 
			// progressBarCurrentOperation
			// 
			this.progressBarCurrentOperation.Location = new System.Drawing.Point(8, 40);
			this.progressBarCurrentOperation.Name = "progressBarCurrentOperation";
			this.progressBarCurrentOperation.Size = new System.Drawing.Size(280, 23);
			this.progressBarCurrentOperation.TabIndex = 2;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// FMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 94);
			this.Controls.Add(this.progressBarCurrentOperation);
			this.Controls.Add(this.progressBarTotal);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FMain";
			this.Text = "AutoUpdater";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.Run(new FMain(args));
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			this.timer1.Enabled = false;

			try
			{
				if (progressBarCurrentOperation.Value < progressBarCurrentOperation.Maximum)
					progressBarCurrentOperation.Value++;
				else
				{
					MessageBox.Show("Programmet som ska uppdateras stängdes inte ner i tid.");
					this.Dispose(true);
				}
				if (!processExist())
				{
					runUpdate();
					if (Startup != "")
					{
						try
						{
							Process.Start(Startup, StartupArguments);
						}
						catch(Exception exc)
						{
							MessageBox.Show(
								"Ett fel uppstod vid start av \"" + 
								Startup + "\": " + exc.ToString());
						}
					}
					Application.Exit();
					return;
				}
			}
			catch(Exception exc)
			{
				MessageBox.Show("Ett fel uppstod under uppdatering: " + exc.ToString());
				Dispose(true);
			}
			finally
			{
				this.timer1.Enabled = true;
			}
		}

		private bool processExist()
		{
			Process[] processes = Process.GetProcessesByName(this.ProcessName);
			if (processes.Length > 0)
				return true;
			else
				return false;
		}
		private void runUpdate()
		{
			this.label1.Text = "Uppdaterar...";
			foreach(string fileAndPath in System.IO.Directory.GetFiles(SourceDir))
			{
				Trace.WriteLine(fileAndPath);
				string newFileName = TargetDir + "\\" + 
					System.IO.Path.GetFileName(fileAndPath);
				if (File.Exists(newFileName))
				{
					int oldVersions = 1;
					string tempFileName = newFileName + "." + oldVersions.ToString();
					while (File.Exists(tempFileName))
					{
						oldVersions++;
						tempFileName = newFileName + "." + oldVersions.ToString();
					}

					bool test = true;
					int retries = 40;
					Exception excSave = null;
					while(test & retries > 0)
					{
						try
						{
							File.Move(newFileName, tempFileName);
							test = false;
						}
						catch(System.IO.IOException exc)
						{
							excSave = exc;
							if (exc.Message.IndexOf("used by another process") > -1)
							{
								System.Threading.Thread.Sleep(250);
							}
							else
								test = false;

							retries--;
						}
					}

					if (retries == 0)
					{
						MessageBox.Show("Ett fel uppstod: \r\n" + excSave.ToString());
						Application.Exit();
					}
				}

				try
				{
					if (fileAndPath.EndsWith("UpdaterGui.exe"))
						File.Copy(fileAndPath, newFileName);
					else
						File.Move(fileAndPath, newFileName);
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.ToString());
					MessageBox.Show(exc.ToString());
				}
			}
		}

	}
}
