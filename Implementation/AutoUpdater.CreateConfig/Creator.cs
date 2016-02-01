// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Creator.cs" company="John Allberg">
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
//   Summary description for Creator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.AutoUpdater.CreateConfig
{
    using System;
    using System.IO;
    using System.Text;

    using Allberg.AutoUpdater.UpdaterLib;

    /// <summary>
    /// Summary description for Creator.
    /// </summary>
    public class Creator
    {
        public Creator(string BaseUrl, string Directory)
        {
            if (!BaseUrl.EndsWith("/"))
                BaseUrl += "/";
            if (Directory.EndsWith("\\"))
                Directory = Directory.Substring(0,Directory.Length-1);

            baseUrl = BaseUrl;
            directory = Directory;
        }
        string baseUrl;
        string directory;

        public void Run()
        {
            StringBuilder content = new StringBuilder();
            foreach(string fileAndPath in Directory.GetFiles(directory))
            {
                if (!fileAndPath.EndsWith("CreateConfig.exe") &
                    !fileAndPath.EndsWith("updatefiles.txt"))
                {
                    content.Append(checkFile(fileAndPath));
                }
            }

            byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(content.ToString());
            Common.WriteFile(bytes, directory + "\\updatefiles.txt");

            Console.WriteLine(content.ToString());
        }

        private string checkFile(string fileAndPath)
        {
            string toReturn = baseUrl + Path.GetFileName(fileAndPath);

            byte[] bytes =
                Common.OpenFile(fileAndPath);

            toReturn += ";" + Common.CreateHash(bytes);
            toReturn += ";" + bytes.Length.ToString();

            return toReturn + "\r\n";
        }
    }
}
