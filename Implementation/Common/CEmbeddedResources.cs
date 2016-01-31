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
// $Id: CEmbeddedResources.cs 105 2009-01-29 10:54:00Z smuda $
using System;
using System.Diagnostics;

namespace Allberg.Shooter.Common
{
	/// <summary>
	/// CEmbeddedResources handles the embedded resources.
	/// </summary>
	public class CEmbeddedResources
	{
		/// <summary>
		/// Creates the new class.
		/// </summary>
		public CEmbeddedResources()
		{
		}

		#region Common
		private static System.IO.StreamReader getResourceReader(string filename)
		{
			System.Reflection.Assembly ass;

			// If strIdentifier is local, get local assembly
			if ("Allberg.Shooter.Common." == 
				filename.Substring(0, 
				"Allberg.Shooter.Common.".Length))
			{
				// Get local assembly
				ass = System.Reflection.Assembly.GetExecutingAssembly();
			}
			else
			{
				// Get calling assembly
				ass = System.Reflection.Assembly.GetCallingAssembly();
			}
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
		#endregion

		#region Bytes
		/// <summary>
		/// Gets the bytes of an embedded resource
		/// </summary>
		/// <param name="filename">Filename, including namespace</param>
		/// <returns></returns>
		public static byte[] GetEmbeddedResourceBytes(string filename)
		{
			try
			{
				System.Reflection.Assembly ass;

				// If strIdentifier is local, get local assembly
				if ("Allberg.Shooter.Common." == 
					filename.Substring(0, 
					"Allberg.Shooter.Common.".Length))
				{
					// Get local assembly
					ass = System.Reflection.Assembly.GetExecutingAssembly();
				}
				else
				{
					// Get calling assembly
					ass = System.Reflection.Assembly.GetCallingAssembly();
				}
#if DEBUG
				foreach(string resource in ass.GetManifestResourceNames())
				{
					Trace.WriteLine("Found object in assembly: " + resource);
				}
#endif
				System.IO.Stream stream = ass.GetManifestResourceStream( filename );
				System.IO.BinaryReader reader =
					new System.IO.BinaryReader( stream );

				byte[] bytes = reader.ReadBytes((int)stream.Length);
				return bytes;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion

		#region Picture
		/// <summary>
		/// Gets the bitmap of an embedded picture resource
		/// </summary>
		/// <param name="filename">Filename, including namespace</param>
		/// <returns></returns>
		public static System.Drawing.Image GetEmbeddedPicture(string filename)
		{
			try
			{
				// get stream to resource
				System.IO.StreamReader reader = getResourceReader(filename);
				// read the resource from the returned stream
				//System.Drawing.Image retValue = System.Drawing.Bitmap.FromStream(reader.BaseStream, true, true);
				System.Drawing.Image retValue = new System.Drawing.Bitmap(reader.BaseStream);
				System.Drawing.Image image = new System.Drawing.Bitmap(retValue);
				//retValue.Save(new System.IO.MemoryStream(), System.Drawing.Imaging.ImageFormat.Jpeg);

				// close the stream
				reader.Close();

				//retValue.Save(new System.IO.MemoryStream(), System.Drawing.Imaging.ImageFormat.Jpeg);

				return image;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());

				throw;
				//return new System.Drawing.Bitmap(20,20);
			}
		}
		#endregion

		#region Xml
		/// <summary>
		/// Gets the streamreader of an embedded resource
		/// </summary>
		/// <param name="filename">Filename, including namespace</param>
		/// <returns></returns>
		public static System.IO.StreamReader GetEmbeddedXmlFile(string filename)
		{
			try
			{
				// get stream to resource
				System.IO.StreamReader reader = getResourceReader(filename);
				return reader;
				// read the resource from the returned stream
				//System.Drawing.Bitmap retValue = new System.Drawing.Bitmap(reader.BaseStream);

				// close the stream
				//reader.Close();
				//return retValue;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion


	}
}
