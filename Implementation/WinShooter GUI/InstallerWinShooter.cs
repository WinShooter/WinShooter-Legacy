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
// $Id: InstallerWinShooter.cs 105 2009-01-29 10:54:00Z smuda $ 
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;
//using System.Reflection;


namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for Installer.
	/// </summary>
	[RunInstaller(true)]
	public class Installer : System.Configuration.Install.Installer
	{
		public Installer()
		{
		}

		public override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);

			if (!CheckPDFInstalled())
			{
				System.Windows.Forms.MessageBox.Show("Du har inte Acrobat Reader installerat." +
					"Det gör att du inte kan läsa manualen eller titta på " +
					"t.ex. exporterade resultat. Gå till http://www.adobe.se " +
					"för att ladda ner och installera Acrobat Reader.",
					"Acrobat Reader saknas",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
			}

			writeInstallDateToRegistry();
		}

		#region PDF
		private bool CheckPDFInstalled()
		{
			string RegistryPlace = @".pdf";
			RegistryKey rk = Registry.ClassesRoot;
			RegistryKey pdfKey = rk.OpenSubKey(RegistryPlace);
			if (pdfKey == null)
				return false;

			object defaultValue = pdfKey.GetValue("");
			if (defaultValue == null)
				return false;

			return true;
		}
		#endregion

		private string readRegistry(string path, string val)
		{
			// Opening the registry key
			RegistryKey rk = Registry.LocalMachine;
			// Open a subKey as read-only
			RegistryKey sk1 = rk.OpenSubKey(path);
			// If the RegistrySubKey doesn't exist -> (null)
			if ( sk1 == null )
			{
				throw new ApplicationException();
			}
			else
			{
				try 
				{
					// If the RegistryKey exists I get its value
					// or null is returned.
					return (string)sk1.GetValue(val.ToString());
				}
				catch (Exception exc)
				{
					// AAAAAAAAAAARGH, an error!
					Trace.WriteLine("Exception while writing to registry: " + 
						exc.ToString());
					throw new ApplicationException();
				}
			}
		}

		private bool checkDirForFile(DirectoryInfo dirInfo, string filename)
		{
			Trace.WriteLine("Checking directory \"" + dirInfo.FullName + "\"");
			foreach(FileInfo file in dirInfo.GetFiles())
			{
				Trace.WriteLine(file.ToString());
				if (file.ToString().ToLower() == filename.ToLower())
					return true;
			}

			foreach(DirectoryInfo dir in dirInfo.GetDirectories())
			{
				if (checkDirForFile(dir, filename))
					return true;
			}
			return false;
		}

		private void writeInstallDateToRegistry()
		{
			// TODO Implement
		}
	}
}
