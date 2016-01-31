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
using System;
using System.Diagnostics;

namespace Allberg.Shooter.Windows.Forms.Wizard
{
	/// <summary>
	/// Summary description for Common.
	/// </summary>
	internal class Common
	{
		private Common()
		{
		}

		internal static System.Drawing.Bitmap GetResourceBitmap(string filename)
		{
			System.IO.StreamReader reader = Common.GetResourceReader(filename);
			System.Drawing.Bitmap toReturn = new System.Drawing.Bitmap(reader.BaseStream);
			reader.Close();
			return toReturn;
		}
		internal static System.IO.StreamReader GetResourceReader(string filename)
		{
			filename = "Allberg.Shooter.Windows.Forms." + filename;
			System.Reflection.Assembly ass;

			// Get local assembly
			ass = System.Reflection.Assembly.GetExecutingAssembly();
#if DEBUG
			foreach(string resource in ass.GetManifestResourceNames())
			{
				Trace.WriteLine("Found object in assembly: " + resource);
			}
#endif
			System.IO.Stream stream = ass.GetManifestResourceStream( filename );
			System.IO.StreamReader reader =
				new System.IO.StreamReader( stream );

			return reader;
		}

	}
}
