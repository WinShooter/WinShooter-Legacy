// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalSettings.cs" company="John Allberg">
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

namespace Allberg.Shooter.Common
{
    using System;

    using Microsoft.Win32;

    /// <summary>
    /// The local settings.
    /// </summary>
    public class LocalSettings
    {
        /// <summary>
        /// The registry place.
        /// </summary>
        internal const string RegistryPlace = @"SOFTWARE\Allberg\WinShooter";

        /// <summary>
        /// Gets or sets the data path.
        /// </summary>
        public static string DataPath
        {
            get
            {
                string result = regRead(Registry.CurrentUser, "DataPath");
                if (result == null)
                    return string.Empty;

                return result;
            }

            set
            {
                regWrite(Registry.CurrentUser, "DataPath", value);
            }
        }

        /// <summary>
        /// The reg read.
        /// </summary>
        /// <param name="rk">
        /// The rk.
        /// </param>
        /// <param name="valuename">
        /// The valuename.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string regRead(RegistryKey rk, string valuename)
        {
            RegistryKey winshooterKey = rk.OpenSubKey(RegistryPlace);
            if (winshooterKey == null)
            {
                return null;
            }

            object objvalue = winshooterKey.GetValue(valuename);
            if (objvalue == null || objvalue.GetType() != typeof(string))
            {
                return null;
            }

            return (string)objvalue;
        }

        /// <summary>
        /// The reg write.
        /// </summary>
        /// <param name="rk">
        /// The rk.
        /// </param>
        /// <param name="valuename">
        /// The valuename.
        /// </param>
        /// <param name="valuecontent">
        /// The valuecontent.
        /// </param>
        /// <exception cref="ApplicationException">
        /// </exception>
        private static void regWrite(RegistryKey rk, string valuename, string valuecontent)
        {
            RegistryKey winshooterKey = rk.CreateSubKey(RegistryPlace);
            if (winshooterKey == null)
            {
                throw new ApplicationException("ops");
            }

            winshooterKey.SetValue(valuename, valuecontent);
        }
    }
}
