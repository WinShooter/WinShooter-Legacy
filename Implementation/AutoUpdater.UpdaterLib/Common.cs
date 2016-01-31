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
// $Id: Common.cs 105 2009-01-29 10:54:00Z smuda $
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;

namespace Allberg.AutoUpdater.UpdaterLib
{
	/// <summary>
	/// Summary description for Common.
	/// </summary>
	public class Common
	{
		public Common()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void WriteFile(byte[] bytes, string filename)
		{
			System.IO.FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
			
			fs.Write(bytes, 0, bytes.Length);

			fs.Flush();
			fs.Close();
		}

		public static byte[] OpenFile(string filename)
		{
			System.IO.MemoryStream mem = new MemoryStream();
			System.IO.FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
			
			byte[] read = new byte[256];
			int count = fs.Read(read, 0, 256);

			while (count > 0) 
			{
				mem.Write(read,0,count);

				count = fs.Read(read, 0, 256);
			}
			fs.Flush();
			fs.Close();
			return mem.ToArray();
		}

		public static bool CheckHashMatch(string url, byte[] bytes, string wantedHash)
		{
			string currentHash = CreateHash(bytes);

			return (currentHash == wantedHash);
		}

		public static string CreateHash(byte[] bytes)
		{
			SHA1Managed sha1 = new SHA1Managed();
			byte[] hashBytes = sha1.ComputeHash(bytes);
			return System.Convert.ToBase64String(hashBytes);
		}
	}
}
