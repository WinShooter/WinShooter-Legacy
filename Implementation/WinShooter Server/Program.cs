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
// $Id: Program.cs 125 2011-05-28 16:37:29Z smuda $ 
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Allberg.UnhandledExceptionSender;

namespace Allberg.Shooter.WinShooterServer
{
	public class Program
	{
		private static string _traceFile = "\\AllbergWinShooterServer.log";
		private static TextWriterTraceListener _listener;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			_traceFile = Environment.GetFolderPath(
				Environment.SpecialFolder.DesktopDirectory) +
				_traceFile;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			AppDomain.CurrentDomain.UnhandledException +=
				CurrentDomainUnhandledException;
			Application.ThreadException +=
				ApplicationThreadException;
			try
			{
				foreach (var s in args)
				{
					if (s.ToLower() != "debug") 
						continue;

					if (File.Exists(_traceFile))
						File.Delete(_traceFile);

					_listener =
						new TextWriterTraceListener(_traceFile);

					Debug.Listeners.Add(_listener);

					Debug.AutoFlush = true;
					Trace.AutoFlush = true;

					Trace.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
					Trace.WriteLine("");
					Trace.WriteLine("Allberg WinShooter Server started on " + DateTime.Now);
					var ver = Assembly.GetExecutingAssembly().GetName().Version;
					Trace.WriteLine("Version: " + ver.Major + "." +
					                ver.Minor + "." +
					                ver.Build + "." +
					                ver.Revision);
					Trace.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId);
					Trace.WriteLine("");
					Trace.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
				}
				_app = new FMain();
				Application.Run(_app);
			}
			catch (Exception exc)
			{
				HandleUnhandledException(exc);
			}
		}

		static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			HandleUnhandledException((Exception)e.ExceptionObject);
		}
		static FMain _app;

		static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
		{
			HandleUnhandledException(e.Exception);
		}

		private static void HandleUnhandledException(Exception exc)
		{
			Trace.WriteLine("Unhandled Exception: " + exc);

			var handler = new FUnhandledExceptionHandler(exc)
			              	{
			              		ApplicationName = AssemblyTitle,
			              		ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version
			              	};
			if (_app != null)
			{
				_app.UnhandledExceptionOccurred = true;

				try
				{
					if (_app.CurrentFilename != null)
					{
						handler.CurrentFilename.Add(_app.CurrentFilename);
					}
				}
				catch (NullReferenceException)
				{
				}
			}
			if (_listener != null)
			{
				try
				{
					Debug.Listeners.Remove(_listener);
					_listener.Close();
					_listener.Dispose();
					handler.CurrentFilename.Add(_traceFile);
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

		public static string AssemblyTitle
		{
			get
			{
				// Get all Title attributes on this assembly
				var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				// If there is at least one Title attribute
				if (attributes.Length > 0)
				{
					// Select the first one
					var titleAttribute = (AssemblyTitleAttribute)attributes[0];
					// If it is not an empty string, return it
					if (titleAttribute.Title != "")
						return titleAttribute.Title;
				}
				// If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
				return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}
	}
}
