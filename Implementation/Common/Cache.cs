// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cache.cs" company="John Allberg">
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
//   CCache implements the caching for shooters and clubs.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System;
    using System.IO;
    using System.Text;

    using Allberg.Shooter.Common.DataSets;
    using Allberg.Shooter.Common.Exceptions;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// CCache implements the caching for shooters and clubs.
    /// </summary>
    [Serializable]
    internal class Cache
    {
        /// <summary>
        /// The startup resources.
        /// </summary>
        private readonly DSStartupResources startupResources;

        /// <summary>
        /// The local cache filename.
        /// </summary>
        private readonly string localCacheFilename;

        /// <summary>
        /// The local cache.
        /// </summary>
        private readonly DSLocalCache localCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cache"/> class.
        /// </summary>
        internal Cache()
        {
            // myInterface = callerInterface;
            // Init cache from embedded file
            this.startupResources = new DSStartupResources();
            /* TODO: Enable reading file.
             * _startup.ReadXml(
                CEmbeddedResources.GetEmbeddedXmlFile(
                    "Allberg.Shooter.Common.DSStartupResources.xml"));*/

            // init cache from local file
            this.localCacheFilename = CreateLocalFilename();
            this.localCache = new DSLocalCache();
            if (File.Exists(this.localCacheFilename))
            {
                this.localCache.ReadXml(this.localCacheFilename);
            }
        }

        /// <summary>
        /// The create local filename.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string CreateLocalFilename()
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            dir += "\\Allberg\\WinShooter";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir + "\\LocalCache.xml";
        }

        /// <summary>
        /// Updates the actual file with data
        /// </summary>
        private void UpdateLocalCache()
        {
            lock (this.localCache)
            {
                this.localCache.WriteXml(this.localCacheFilename);
            }
        }

        /// <summary>
        /// Get the string representation of the local cache dataset
        /// </summary>
        /// <returns></returns>
        internal string GetCacheFileContent()
        {
            lock (this.localCache)
            {
                using (var stream = new MemoryStream())
                {
                    this.localCache.WriteXml(stream);

                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
        }

        /// <summary>
        /// Returns the number of shooters in the local cache
        /// </summary>
        /// <returns>The number of shooter entries in the local cache</returns>
        internal int GetShooterCountInLocalFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a shooter with card number.
        /// </summary>
        /// <param name="cardNr">Card number</param>
        /// <returns><see cref="Structs.Shooter"/></returns>
        internal Structs.Shooter GetShooter(string cardNr)
        {
            Structs.Shooter localFileShooter;
            try
            {
                localFileShooter = this.GetShooterFromLocalFile(cardNr);
            }
            catch (CannotFindIdException)
            {
                return this.GetShooterFromEmbeddedFile(cardNr);
            }

            Structs.Shooter embeddedShooter;
            try
            {
                embeddedShooter = this.GetShooterFromEmbeddedFile(cardNr);
            }
            catch (CannotFindIdException)
            {
                return localFileShooter;
            }

            if (localFileShooter.LastUpdate > embeddedShooter.LastUpdate)
            {
                return localFileShooter;
            }

            return embeddedShooter;
        }

        /// <summary>
        /// Updates a shooter in the local cache file
        /// </summary>
        /// <param name="shooter">The <see cref="Structs.Shooter"/></param>
        internal void UpdateShooterInCache(Structs.Shooter shooter)
        {
            if (int.Parse(shooter.CardNr) < 0)
            {
                return;
            }

            var rows =
                (DSLocalCache.ShootersRow[])
                this.localCache.Shooters.Select(
                "CardNr='" + shooter.CardNr + "'");

            DSLocalCache.ShootersRow row;
            if (rows.Length > 0)
            {
                row = rows[0];
            }
            else
            {
                row = this.localCache.Shooters.NewShootersRow();
                this.localCache.Shooters.AddShootersRow(row);
            }

            bool updated = false;

            // check all attributes
            if (row.IsCardnrNull() || row.Cardnr != shooter.CardNr)
            {
                row.Cardnr = shooter.CardNr;
                updated = true;
            } 

            if (row.IsClassNull() || row.Class != (int)shooter.Class)
            {
                row.Class = (int)shooter.Class;
                updated = true;
            }

            if (row.IsClubIdNull() || row.ClubId != shooter.ClubId)
            {
                row.ClubId = shooter.ClubId;
                updated = true;
            }

            if (row.IsEmailNull() || row.Email != shooter.Email)
            {
                row.Email = shooter.Email;
                updated = true;
            }

            if (row.IsGivennameNull() || row.Givenname != shooter.Givenname)
            {
                row.Givenname = shooter.Givenname;
                updated = true;
            }

            if (row.IsSurnameNull() || row.Surname != shooter.Surname)
            {
                row.Surname = shooter.Surname;
                updated = true;
            }

            if (updated)
            {
                row.LastUpdate = DateTime.Now;
                this.UpdateLocalCache();
            }
        }

        /// <summary>
        /// Get the shooter from local file.
        /// </summary>
        /// <param name="cardNr">
        /// The card nr.
        /// </param>
        /// <returns>
        /// The <see cref="Shooter"/>.
        /// </returns>
        private Structs.Shooter GetShooterFromLocalFile(string cardNr)
        {
            var rows =
                (DSLocalCache.ShootersRow[])
                this.localCache.Shooters.Select(
                "CardNr='" + cardNr + "'");
            if (rows.Length <= 0)
            {
                throw new CannotFindIdException("Could not find shooter");
            }

            var shooter = new Structs.Shooter
                            {
                                CardNr = rows[0].Cardnr,
                                ClubId = rows[0].ClubId,
                                Email = rows[0].Email,
                                Givenname = rows[0].Givenname,
                                Surname = rows[0].Surname,
                                Automatic = true,
                                Class = (Structs.ShootersClass)rows[0].Class,
                                LastUpdate = rows[0].LastUpdate
                            };

            return shooter;
        }

        /// <summary>
        /// Get the shooter from embedded file.
        /// </summary>
        /// <param name="cardNr">
        /// The card nr.
        /// </param>
        /// <returns>
        /// The <see cref="Shooter"/>.
        /// </returns>
        private Structs.Shooter GetShooterFromEmbeddedFile(string cardNr)
        {
            var rows =
                (DSStartupResources.ShootersRow[])
                this.startupResources.Shooters.Select(
                    "CardNr='" + cardNr + "'");
            if (rows.Length <= 0)
            {
                throw new CannotFindIdException("Could not find shooter");
            }

            var shooter = new Structs.Shooter
                            {
                                CardNr = rows[0].Cardnr,
                                ClubId = rows[0].ClubId,
                                Email = rows[0].Email,
                                Givenname = rows[0].Givenname,
                                Surname = rows[0].Surname,
                                Automatic = true,
                                Class = (Structs.ShootersClass)rows[0].Class,
                                LastUpdate = rows[0].LastUpdate
                            };

            return shooter;
        }

        /// <summary>
        /// Returns the number of clubs in the local cache
        /// </summary>
        /// <returns>The number of clubs entries in the local cache</returns>
        internal int GetClubCountInLocalFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates a club in cache
        /// </summary>
        /// <param name="club">The <see cref="Structs.Club"/></param>
        internal void UpdateClubInCache(Structs.Club club)
        {
            var rows =
                (DSLocalCache.ClubsRow[])
                this.localCache.Clubs.Select(
                "ClubId='" + club.ClubId + "'");

            DSLocalCache.ClubsRow row;
            if (rows.Length > 0)
            {
                row = rows[0];
            }
            else
            {
                row = this.localCache.Clubs.NewClubsRow();
                this.localCache.Clubs.AddClubsRow(row);
            }

            var updated = false;

            // check all attributes
            if (row.IsClubIdNull() || row.ClubId != club.ClubId)
            {
                row.ClubId = club.ClubId;
                updated = true;
            }

            if (row.IsCountryNull() || row.Country != club.Country)
            {
                row.Country = club.Country;
                updated = true;
            }

            if (row.IsEmailNull() || row.Email != club.Email)
            {
                row.Email = club.Email;
                updated = true;
            }

            if (row.IsNameNull() || row.Name != club.Name)
            {
                row.Name = club.Name;
                updated = true;
            }

            if (row.IsPlusgiroNull() || row.Plusgiro != club.Plusgiro)
            {
                row.Plusgiro = club.Plusgiro;
                updated = true;
            }

            if (row.IsBankgiroNull() || row.Bankgiro != club.Bankgiro)
            {
                row.Bankgiro = club.Bankgiro;
                updated = true;
            }

            if (updated)
            {
                row.LastUpdate = DateTime.Now;
                this.UpdateLocalCache();
            }
        }
    }
}
