// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Common.cs" company="John Allberg">
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.AutoUpdater.UpdaterLib
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// Summary description for Common.
    /// </summary>
    public class Common
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Common"/> class.
        /// </summary>
        public Common()
        {
            // TODO: Add constructor logic here
        }

        /// <summary>
        /// The write file.
        /// </summary>
        /// <param name="bytes">
        /// The bytes.
        /// </param>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public static void WriteFile(byte[] bytes, string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            
            fs.Write(bytes, 0, bytes.Length);

            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// The open file.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] OpenFile(string filename)
        {
            MemoryStream mem = new MemoryStream();
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            
            byte[] read = new byte[256];
            int count = fs.Read(read, 0, 256);

            while (count > 0) 
            {
                mem.Write(read, 0, count);

                count = fs.Read(read, 0, 256);
            }

            fs.Flush();
            fs.Close();
            return mem.ToArray();
        }

        /// <summary>
        /// The check hash match.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="bytes">
        /// The bytes.
        /// </param>
        /// <param name="wantedHash">
        /// The wanted hash.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool CheckHashMatch(string url, byte[] bytes, string wantedHash)
        {
            string currentHash = CreateHash(bytes);

            return currentHash == wantedHash;
        }

        /// <summary>
        /// The create hash.
        /// </summary>
        /// <param name="bytes">
        /// The bytes.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CreateHash(byte[] bytes)
        {
            SHA1Managed sha1 = new SHA1Managed();
            byte[] hashBytes = sha1.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
