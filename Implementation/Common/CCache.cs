// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CCache.cs" company="John Allberg">
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
        /// Initiates the CCache
        /// </summary>
        internal Cache()
        {
            //myInterface = callerInterface;
            // Init cache from embedded file
            _startup = new DSStartupResources();
            /* TODO: Enable reading file.
             * _startup.ReadXml(
                CEmbeddedResources.GetEmbeddedXmlFile(
                    "Allberg.Shooter.Common.DSStartupResources.xml"));*/

            // init cache from local file
            _localCacheFilename = CreateLocalFilename();
            _localCache = new DSLocalCache();
            if (File.Exists(_localCacheFilename))
            {
                _localCache.ReadXml(_localCacheFilename);
            }
        }

        private readonly DSStartupResources _startup;
        private readonly string _localCacheFilename;
        private readonly DSLocalCache _localCache;

        #region Common
        private static string CreateLocalFilename()
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            dir += "\\Allberg\\WinShooter";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir + "\\LocalCache.xml";
        }

        /// <summary>
        /// Updates the actual file with data
        /// </summary>
        private void UpdateLocalCache()
        {
            lock (_localCache)
            {
                _localCache.WriteXml(_localCacheFilename);
            }
        }

        /// <summary>
        /// Get the string representation of the local cache dataset
        /// </summary>
        /// <returns></returns>
        internal string GetCacheFileContent()
        {
            lock (_localCache)
            {
                using (var stream = new MemoryStream())
                {
                    _localCache.WriteXml(stream);

                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
        }
        #endregion

        #region Shooter
        /// <summary>
        /// Returns the number of shooters in the local cache
        /// </summary>
        /// <returns>The number of shooter entries in the local cache</returns>
        internal int GetShooterCountInLocalFile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a shooter with cardnr.
        /// </summary>
        /// <param name="cardNr">Card number</param>
        /// <returns></returns>
        internal Structs.Shooter GetShooter(string cardNr)
        {
            Structs.Shooter localFileShooter;
            try
            {
                localFileShooter = GetShooterFromLocalFile(cardNr);
            }
            catch (CannotFindIdException)
            {
                return GetShooterFromEmbeddedFile(cardNr);
            }

            Structs.Shooter embeddedShooter;
            try
            {
                embeddedShooter = GetShooterFromEmbeddedFile(cardNr);
            }
            catch (CannotFindIdException)
            {
                return localFileShooter;
            }

            if (localFileShooter.LastUpdate > embeddedShooter.LastUpdate)
                return localFileShooter;
            return embeddedShooter;
        }

        /// <summary>
        /// Updates a shooter in the local cache file
        /// </summary>
        /// <param name="shooter"></param>
        internal void UpdateShooterInCache(Structs.Shooter shooter)
        {
            if (int.Parse(shooter.CardNr) < 0)
                return;

            var rows =
                (DSLocalCache.ShootersRow[])
                _localCache.Shooters.Select(
                "CardNr='" + shooter.CardNr + "'");

            DSLocalCache.ShootersRow row;
            if (rows.Length > 0)
                row = rows[0];
            else
            {
                row = _localCache.Shooters.NewShootersRow();
                _localCache.Shooters.AddShootersRow(row);
            }

            bool updated = false;

            #region check all attributes
            if (row.IsCardnrNull() ||
                row.Cardnr != shooter.CardNr)
            {
                row.Cardnr = shooter.CardNr;
                updated = true;
            } 
            if (row.IsClassNull() ||
                row.Class != (int)shooter.Class)
            {
                row.Class = (int)shooter.Class;
                updated = true;
            }
            if (row.IsClubIdNull() ||
                row.ClubId != shooter.ClubId)
            {
                row.ClubId = shooter.ClubId;
                updated = true;
            }
            if (row.IsEmailNull() ||
                row.Email != shooter.Email)
            {
                row.Email = shooter.Email;
                updated = true;
            }
            if (row.IsGivennameNull() ||
                row.Givenname != shooter.Givenname)
            {
                row.Givenname = shooter.Givenname;
                updated = true;
            }
            if (row.IsSurnameNull() ||
                row.Surname != shooter.Surname)
            {
                row.Surname = shooter.Surname;
                updated = true;
            }
            #endregion

            if (updated)
            {
                row.LastUpdate = DateTime.Now;
                UpdateLocalCache();
            }
        }

        private Structs.Shooter GetShooterFromLocalFile(string cardNr)
        {
            var rows =
                (DSLocalCache.ShootersRow[])
                _localCache.Shooters.Select(
                "CardNr='" + cardNr + "'");
            if (rows.Length <= 0)
                throw new CannotFindIdException("Could not find shooter");

            var shooter = new Structs.Shooter
                            {
                                CardNr = rows[0].Cardnr,
                                ClubId = rows[0].ClubId,
                                Email = rows[0].Email,
                                Givenname = rows[0].Givenname,
                                Surname = rows[0].Surname,
                                Automatic = true,
                                Class = (Structs.ShootersClass) rows[0].Class,
                                LastUpdate = rows[0].LastUpdate
                            };

            return shooter;
        }

        private Structs.Shooter GetShooterFromEmbeddedFile(string cardNr)
        {
            var rows =
                (DSStartupResources.ShootersRow[])
                _startup.Shooters.Select(
                    "CardNr='" + cardNr + "'");
            if (rows.Length <= 0)
                throw new CannotFindIdException("Could not find shooter");

            var shooter = new Structs.Shooter
                            {
                                CardNr = rows[0].Cardnr,
                                ClubId = rows[0].ClubId,
                                Email = rows[0].Email,
                                Givenname = rows[0].Givenname,
                                Surname = rows[0].Surname,
                                Automatic = true,
                                Class = (Structs.ShootersClass) rows[0].Class,
                                LastUpdate = rows[0].LastUpdate
                            };

            return shooter;
        }
        #endregion

        #region Weapon
        /*/// <summary>
        /// Returns a weapon with weaponid.
        /// </summary>
        /// <param name="WeaponId">Weapon id</param>
        /// <returns></returns>
        internal Structs.Weapon GetWeapon(string WeaponId)
        {
            Structs.Weapon localFileWeapon;
            try
            {
                localFileWeapon = getWeaponFromLocalFile(WeaponId);
            }
            catch (CannotFindIdException)
            {
                return getWeaponFromEmbeddedFile(WeaponId);
            }

            Structs.Weapon embeddedWeapon;
            try
            {
                embeddedWeapon = getWeaponFromEmbeddedFile(WeaponId);
            }
            catch (CannotFindIdException)
            {
                return localFileWeapon;
            }

            if (localFileWeapon.LastUpdate > embeddedWeapon.LastUpdate)
                return localFileWeapon;
            else
                return embeddedWeapon;
        }

        internal void UpdateWeaponInCache(Structs.Weapon weapon)
        {
            DSLocalCache.WeaponsRow[] rows =
                (DSLocalCache.WeaponsRow[])
                localCache.Weapons.Select(
                "CardNr='" + Weapon.CardNr + "'");

            DSLocalCache.WeaponsRow row;
            if (rows.Length > 0)
                row = rows[0];
            else
            {
                row = localCache.Weapons.NewWeaponsRow();
                localCache.Weapons.AddWeaponsRow(row);
            }

            bool updated = false;

            #region check all attributes
            if (row.IsCardnrNull() ||
                row.Cardnr != Weapon.CardNr)
            {
                row.Cardnr = Weapon.CardNr;
                updated = true;
            }
            if (row.IsClassNull() ||
                row.Class != (int)Weapon.Class)
            {
                row.Class = (int)Weapon.Class;
                updated = true;
            }
            if (row.IsClubIdNull() ||
                row.ClubId != Weapon.ClubId)
            {
                row.ClubId = Weapon.ClubId;
                updated = true;
            }
            if (row.IsEmailNull() ||
                row.Email != Weapon.Email)
            {
                row.Email = Weapon.Email;
                updated = true;
            }
            if (row.IsGivennameNull() ||
                row.Givenname != Weapon.Givenname)
            {
                row.Givenname = Weapon.Givenname;
                updated = true;
            }
            if (row.IsSurnameNull() ||
                row.Surname != Weapon.Surname)
            {
                row.Surname = Weapon.Surname;
                updated = true;
            }
            #endregion

            if (updated)
            {
                row.LastUpdate = DateTime.Now;
                updateLocalCache();
            }
        }


        private Structs.Weapon getWeaponFromLocalFile(string CardNr)
        {
            DSLocalCache.WeaponsRow[] rows =
                (DSLocalCache.WeaponsRow[])
                localCache.Weapons.Select(
                "CardNr='" + CardNr + "'");
            if (rows.Length <= 0)
                throw new CannotFindIdException("Could not find Weapon");

            Structs.Weapon Weapon = new Structs.Weapon();
            Weapon.CardNr = rows[0].Cardnr;
            Weapon.ClubId = rows[0].ClubId;
            Weapon.Email = rows[0].Email;
            Weapon.Givenname = rows[0].Givenname;
            Weapon.Surname = rows[0].Surname;
            Weapon.Automatic = true;
            Weapon.Class = (Structs.WeaponsClass)rows[0].Class;
            Weapon.LastUpdate = rows[0].LastUpdate;

            return Weapon;
        }

        private Structs.Weapon getWeaponFromEmbeddedFile(string CardNr)
        {
            DSStartupResources.WeaponsRow[] rows =
                (DSStartupResources.WeaponsRow[])
                startup.Weapons.Select(
                    "CardNr='" + CardNr + "'");
            if (rows.Length <= 0)
                throw new CannotFindIdException("Could not find Weapon");

            Structs.Weapon Weapon = new Structs.Weapon();
            shooter.CardNr = rows[0].Cardnr;
            shooter.ClubId = rows[0].ClubId;
            shooter.Email = rows[0].Email;
            shooter.Givenname = rows[0].Givenname;
            shooter.Surname = rows[0].Surname;
            shooter.Automatic = true;
            shooter.Class = (Structs.WeaponsClass)rows[0].Class;
            shooter.LastUpdate = rows[0].LastUpdate;

            return shooter;
        }*/
        #endregion
        
        #region Club
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
        /// <param name="club"></param>
        internal void UpdateClubInCache(Structs.Club club)
        {
            var rows =
                (DSLocalCache.ClubsRow[])
                _localCache.Clubs.Select(
                "ClubId='" + club.ClubId + "'");

            DSLocalCache.ClubsRow row;
            if (rows.Length > 0)
                row = rows[0];
            else
            {
                row = _localCache.Clubs.NewClubsRow();
                _localCache.Clubs.AddClubsRow(row);
            }

            bool updated = false;

            #region check all attributes
            if (row.IsClubIdNull() ||
                row.ClubId != club.ClubId)
            {
                row.ClubId = club.ClubId;
                updated = true;
            }
            if (row.IsCountryNull() ||
                row.Country != club.Country)
            {
                row.Country = club.Country;
                updated = true;
            }
            if (row.IsEmailNull() ||
                row.Email != club.Email)
            {
                row.Email = club.Email;
                updated = true;
            }
            if (row.IsNameNull() ||
                row.Name != club.Name)
            {
                row.Name = club.Name;
                updated = true;
            }
            if (row.IsPlusgiroNull() ||
                row.Plusgiro != club.Plusgiro)
            {
                row.Plusgiro = club.Plusgiro;
                updated = true;
            }
            if (row.IsBankgiroNull() ||
                row.Bankgiro != club.Bankgiro)
            {
                row.Bankgiro = club.Bankgiro;
                updated = true;
            }
            #endregion

            if (updated)
            {
                row.LastUpdate = DateTime.Now;
                UpdateLocalCache();
            }
        }
        #endregion
    }
}
