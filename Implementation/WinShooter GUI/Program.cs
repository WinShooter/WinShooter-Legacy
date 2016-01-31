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
// $Id: Program.cs 107 2009-02-01 06:25:33Z smuda $ 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Allberg.UnhandledExceptionSender;

namespace Allberg.Shooter.Windows
{
	public class Program
	{
		private static string traceFile = "\\AllbergWinShooter.log";
		static TextWriterTraceListener listener = null;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			traceFile = Environment.GetFolderPath(
				Environment.SpecialFolder.DesktopDirectory) + 
				traceFile;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			AppDomain.CurrentDomain.UnhandledException += 
				new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			Application.ThreadException += 
				new ThreadExceptionEventHandler(Application_ThreadException);
			//Application.DoEvents();

			try
			{
				string filename = null;
				foreach (string s in args)
				{
					switch (s.ToLower())
					{
						case "debug":
							{
								Trace.WriteLine("Debug was entered from command-line.");
								if (System.IO.File.Exists(traceFile))
									System.IO.File.Delete(traceFile);

								Trace.WriteLine("Creating file listener.");
								listener =
									new TextWriterTraceListener(traceFile);
								Debug.Listeners.Add(listener);

								Debug.AutoFlush = true;
								Trace.AutoFlush = true;

								Trace.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
								Trace.WriteLine("");
								Trace.WriteLine("Allberg WinShooter started on " + System.DateTime.Now);
								System.Version ver = Assembly.GetExecutingAssembly().GetName().Version;
								Trace.Write("Version: " + ver.Major.ToString() + "." +
									ver.Minor.ToString() + "." +
									ver.Build.ToString() + "." +
									ver.Revision.ToString());
#if DEBUG
								Trace.WriteLine(" Beta");
#else
								Trace.WriteLine("");
#endif
								Trace.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
								System.OperatingSystem os = Environment.OSVersion;
								Trace.WriteLine("OS Platform: " + os.Platform.ToString());
								Trace.WriteLine("OS Version: " + os.Version.ToString());
								Trace.WriteLine("");
								Trace.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
								Trace.WriteLine("Done.");
								break;
							}
						default:
							{
								if (s.ToLower().StartsWith("/u="))
								{
									string guid = s.Split('=')[1];
									string path = Environment.GetFolderPath(System.Environment.SpecialFolder.System);
									string str = path + "\\msiexec.exe";
									ProcessStartInfo pi = new ProcessStartInfo(str);
									pi.Arguments = "/i " + guid;
									pi.UseShellExecute = false;
									Process.Start(pi);

									return;
								}
								else if (System.IO.File.Exists(s))
								{
									filename = s;
								}
								else
								{
									Trace.WriteLine("Unknown argument: \"" + s + "\"");
								}
								break;
							}
					}
				}
				// Start application
				app = new FMain(filename);
				Application.Run(app);
			}
			catch (System.ObjectDisposedException)
			{
				// This occurs when disposing self
			}
			catch (Exception exc)
			{
				handleUnhandledException(exc);
			}
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			handleUnhandledException((Exception)e.ExceptionObject);
		}
		static FMain app = null;

		static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			handleUnhandledException(e.Exception);
		}

		private static void handleUnhandledException(Exception exc)
		{
			bool useHandler = true;
			Trace.WriteLine(exc.ToString());
			if (useHandler)
			{
				FUnhandledExceptionHandler handler = new FUnhandledExceptionHandler(exc);
				handler.ApplicationName = AssemblyTitle;
				handler.ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version;
				if (app != null)
				{
					try
					{
						if (app.CommonCode.CurrentFilename != null)
						{
							handler.CurrentFilename.Add(app.CommonCode.CurrentFilename);
						}
					}
					catch (NullReferenceException)
					{
					}
				}
				if (listener != null)
				{
					try
					{
						Debug.Listeners.Remove(listener);
						listener.Close();
						listener.Dispose();
						handler.CurrentFilename.Add(traceFile);
					}
					catch (Exception excLocal)
					{
						Trace.WriteLine(excLocal.ToString());
					}
				}
				if (Application.MessageLoop)
				{
					handler.Show();
				}
				else
				{
					Application.Run(handler);
				}
			}
			else
			{
				MessageBox.Show("Ett allvarligt fel har uppstått: \"" + exc.ToString() + "\"",
					"Major Failure",
					MessageBoxButtons.OK,
					MessageBoxIcon.Stop);
			}
			//Application.Exit();
		}
		public static string AssemblyTitle
		{
			get
			{
				// Get all Title attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				// If there is at least one Title attribute
				if (attributes.Length > 0)
				{
					// Select the first one
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					// If it is not an empty string, return it
					if (titleAttribute.Title != "")
						return titleAttribute.Title;
				}
				// If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}
	}
}
