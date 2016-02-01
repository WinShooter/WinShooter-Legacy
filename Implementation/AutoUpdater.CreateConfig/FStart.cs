// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FStart.cs" company="John Allberg">
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
//   Summary description for Class1.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.AutoUpdater.CreateConfig
{
    using System;
    using System.IO;

    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class FStart
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                showUsage();
                return;
            }

            string baseurl = args[0];
            string directory;
            if (args.Length == 1)
            {
                directory = Directory.GetCurrentDirectory();
            }
            else
            {
                directory = args[1];
            }
            Creator creator = new Creator(baseurl, directory);
            creator.Run();
        }

        static void showUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("CreateConfig <baseurl> <directory>");
        }

    }
}
