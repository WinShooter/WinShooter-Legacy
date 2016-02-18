// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CEmbeddedResources.cs" company="John Allberg">
//   Copyright ©2001-2016 John Allberg
//   
//   This program is free software; you can redistribute it and/or
//   modify it under the terms of the GNU General Public License
//   as published by the Free Software Foundation; either version 2
//   of the License, or (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
//   GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License
//   along with this program; if not, write to the Free Software
//   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
// <summary>
//   CEmbeddedResources handles the embedded resources.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// CEmbeddedResources handles the embedded resources.
    /// </summary>
    public class CEmbeddedResources
    {
        #region Common

        /// <summary>
        /// Get the resource reader.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <returns>
        /// The <see cref="System.IO.StreamReader"/>.
        /// </returns>
        private static System.IO.StreamReader GetResourceReader(string filename)
        {
            // If strIdentifier is local, get local assembly
            var ass = "Allberg.Shooter.Common." == filename.Substring(0, "Allberg.Shooter.Common.".Length) 
                ? Assembly.GetExecutingAssembly() 
                : Assembly.GetCallingAssembly();
#if DEBUG
            foreach (string resource in ass.GetManifestResourceNames())
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
        /// <returns>the bytes of an embedded resource</returns>
        public static byte[] GetEmbeddedResourceBytes(string filename)
        {
            try
            {
                // If strIdentifier is local, get local assembly
                var ass = "Allberg.Shooter.Common." == filename.Substring(0, "Allberg.Shooter.Common.".Length) 
                    ? Assembly.GetExecutingAssembly() 
                    : Assembly.GetCallingAssembly();
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
            catch (Exception exc)
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
        /// <returns>the bitmap of an embedded picture resource</returns>
        public static System.Drawing.Image GetEmbeddedPicture(string filename)
        {
            try
            {
                // get stream to resource
                System.IO.StreamReader reader = GetResourceReader(filename);
                System.Drawing.Image retValue = new System.Drawing.Bitmap(reader.BaseStream);
                System.Drawing.Image image = new System.Drawing.Bitmap(retValue);

                // close the stream
                reader.Close();

                return image;
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.ToString());

                throw;
            }
        }
        #endregion

        #region Xml
        /// <summary>
        /// Gets the stream reader of an embedded resource
        /// </summary>
        /// <param name="filename">Filename, including namespace</param>
        /// <returns>the stream reader of an embedded resource</returns>
        public static System.IO.StreamReader GetEmbeddedXmlFile(string filename)
        {
            try
            {
                // get stream to resource
                var reader = GetResourceReader(filename);
                return reader;
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.ToString());
                throw;
            }
        }
        #endregion
    }
}
