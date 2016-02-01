// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CSettings.cs" company="John Allberg">
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
//   Summary description for Settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System;
    using System.Drawing;
    using System.IO;

    using Allberg.Shooter.Common.DataSets;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for Settings.
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public class CSettings  : Allberg.Shooter.WinShooterServerRemoting.ISettings
    {

        private static object locker = new object();
        private static CSettings myInstance = null;
        internal static CSettings Instance
        {
            get
            {
                if (myInstance == null)
                {
                    lock (locker)
                    {
                        if (myInstance == null)
                        {
                            myInstance = new CSettings();
                        }
                    }
                }
                return myInstance;
            }
        }
        private CSettings()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            dir += "\\Allberg\\WinShooter";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            fileName = dir + "\\LocalCache.xml";
            
            ds = new DSSettings();
            if (File.Exists(fileName))
            {
                ds.ReadXml(fileName);
            }
            readSettings();
            printerSettings = new CPrinterSettings(this);
        }

        private void readSettings()
        {
            // Logo
            string logoString = ReadSettingString(SettingsNameEnum.Logo);
            if (logoString != null && logoString != "")
            {
                MemoryStream stream = new MemoryStream();
                byte[] logobytes = Convert.FromBase64String(logoString);
                stream.Write(logobytes,0,logobytes.Length);
                logo = new Bitmap(stream);
            }
            else
                logo = null;
        }

        internal enum SettingsNameEnum
        {
            Logo,
            LabelMirrorPrintDocument,
            LabelStickPrintDocument,
            LabelResultPrintDocument,
            PaperResultDocument,
            PaperResultTeamDocument
        }

        private CPrinterSettings printerSettings;
        public IPrinterSettings PrinterSettings
        {
            get
            {
                return printerSettings;
            }
            set
            {
                printerSettings = new CPrinterSettings(value);
                printerSettings.SetValuesToRegistry();
            }
        }

        private Image logo = null;
        public Image Logo
        {
            get
            {
                readSettings();
                if (logo == null)
                {
                    Image image =
                        CEmbeddedResources.GetEmbeddedPicture(
                        "Allberg.Shooter.Common.WinShooterLogga.jpg");
                    return image;
                }
                else
                {
                    return logo;
                }
            }
            set
            {
                logo = value;
                updateSettingsInRegistry();
            }
        }

        // Get the resized logo
        public Image GetResizedLogo(int maxImageHeight, int maxImageWidth)
        {
            return getLogo(Logo, maxImageHeight, maxImageWidth);
        }
        public Image GetWinshooterLogo(int maxImageHeight, int maxImageWidth)
        {
            Image image =
                CEmbeddedResources.GetEmbeddedPicture(
                "Allberg.Shooter.Common.WinShooterLogga.jpg");
            return getLogo(image, maxImageHeight, maxImageWidth);
        }
        private Image getLogo(Image image, int maxImageHeight, int maxImageWidth)
        {
            double xfactor = (double)maxImageWidth / (double)image.Width;
            double yfactor = (double)maxImageHeight / (double)image.Height;
            if (xfactor > yfactor)
                xfactor = yfactor;
            else
                yfactor = xfactor;

            int height = (int)(yfactor * (double)image.Height);
            int width = (int)(xfactor * (double)image.Width);

            Image logoToReturn = new Bitmap(image, new Size(width, height));
            return logoToReturn;
        }


        internal void Update(ISettings newSettings)
        {
            if (newSettings.Logo != Logo)
                Logo = newSettings.Logo;
        }

        private void updateSettingsInRegistry()
        {
            // Logo
            if (logo != null)
            {
                MemoryStream stream = new MemoryStream();
                logo.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Flush();
                byte[] logoBytes = stream.ToArray();
                WriteSetting(SettingsNameEnum.Logo, 
                    System.Convert.ToBase64String(logoBytes));
            }
            else
            {
                WriteSetting(SettingsNameEnum.Logo, 
                    "");
            }
        }

        #region Registry handling
        private string fileName = "";
        private DSSettings ds;
        internal void WriteSetting(SettingsNameEnum valuename, string valuecontent)
        {
            DSSettings.SettingsRow[] rows = (DSSettings.SettingsRow[])
                ds.Settings.Select("Name='" + valuename.ToString() + "'");

            if (rows.Length == 0)
            {
                DSSettings.SettingsRow row = (DSSettings.SettingsRow)
                    ds.Settings.NewRow();
                row.Name = valuename.ToString();
                row.Content = valuecontent;
                ds.Settings.AddSettingsRow(row);
            }
            else
            {
                DSSettings.SettingsRow row = rows[0];
                row.Content = valuecontent;
            }

            ds.WriteXml(fileName);
        }

        internal string ReadSettingString(SettingsNameEnum valuename)
        {
            DSSettings.SettingsRow[] rows = (DSSettings.SettingsRow[])
                ds.Settings.Select("Name='" + valuename.ToString() + "'");

            if (rows.Length == 0)
            {
                return null;
            }
            else
            {
                return rows[0].Content;
            }
        }

        /*const string RegistryPlace = @"SOFTWARE\Allberg\WinShooter";
        static RegistryKey rk = Registry.CurrentUser;
        static internal void WriteSetting(SettingsNameEnum valuename, string valuecontent)
        {
            RegistryKey winshooterKey = rk.CreateSubKey(RegistryPlace);
            if (winshooterKey == null)
            {
                throw new ApplicationException("ops");
            }
            winshooterKey.SetValue(valuename.ToString(), valuecontent);
        }
        static internal string ReadSettingString(SettingsNameEnum valuename)
        {
            RegistryKey winshooterKey = rk.OpenSubKey(RegistryPlace);
            if (winshooterKey == null)
            {
                return null;
            }
            object objvalue = winshooterKey.GetValue(valuename.ToString());
            if (objvalue == null || objvalue.GetType() != typeof(string))
            {
                return null;
            }
            return (string)objvalue;
        }*/
        #endregion

    }
}