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
// $Id: Installer.cs 105 2009-01-29 10:54:00Z smuda $ 
using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.IO;

namespace Allberg.Shooter.WinShooterServer
{
	/// <summary>
	/// Summary description for Installer.
	/// </summary>
	[RunInstaller(true)]
	public class InstallerServer : System.Configuration.Install.Installer
	{
		public InstallerServer()
		{
		}

		public override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);

			Assembly callingAssembly = Assembly.GetExecutingAssembly();

			bool retry = true;
			int retrycount = 10;
			Exception excToPrint;
			while(retry && retrycount>=0)
			{
				try
				{
					FileInfo fi = new FileInfo(callingAssembly.Location.Replace("winshooterserver.exe", "winshooter.exe"));
					fi.LastWriteTime = DateTime.Now;
					retry = false;
					return;
				}
				catch(Exception exc)
				{
					excToPrint = exc;
					retrycount--;
					System.Threading.Thread.Sleep(50);
				}
			}
			//System.Windows.Forms.MessageBox.Show("Ett problem uppstod vid hantering av utvärderingsversion." + excToPrint.Message);
		}
	}
}
