// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CInternetTextExport.cs" company="John Allberg">
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
//   Defines the CInternetTextExport type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System.Text;

    using Allberg.Shooter.WinShooterServerRemoting;

    class CInternetTextExport
    {
        internal CInternetTextExport(Interface callerInterface)
        {
            myInterface = callerInterface;
        }
        Interface myInterface;
        DatabaseDataset database;

        #region Export Results
        internal byte[] ExportResults()
        {
            database = myInterface.databaseClass.Database;

            StringBuilder toReturn = new StringBuilder();
            foreach (DatabaseDataset.CompetitorsRow competitor in
                database.Competitors)
            {
                toReturn.Append(createCompetitorLine(competitor));
            }

            Encoding encoding = Encoding.GetEncoding(1252);
            byte[] bytes = encoding.GetBytes(toReturn.ToString());

            return bytes;
        }

        /*
        2120;01-417;Fredriksson;Jonas;3J;jonas_fredriksson@email.com;Un (C)
        2120;01-417;Fredriksson;Jonas;3;jonas_fredriksson@email.com;Ad
        2120;01-417;Fredriksson;Jonas;3;jonas_fredriksson@email.com;_OkäntB
        2143;01-417;Selander;Martin;3;martin_selander@email.com;M40
        2143;01-417;Selander;Martin;3;martin_selander@email.com;C45 (C)
        
        Kolumn 1
            Pistolskyttekortsnummer
        Kolumn 2
            Klubb
        Kolumn 3
            Efternamn
        Kolumn 4
            Förnamn
        Kolumn 5
            Klass. Notera att man kan skriva t.ex. 3D för damklass.
        Kolumn 6
            E-postadress
        Kolumn 7
            Vapenförkortning. Notera att vapenförkortningen måste vara exakt så som programmet presenterar den.
        Kolumn 8
            Patrullnummer (måste inte anges)
        Kolumn 9
            Figur inom patrull (måste inte anges)
        */
        private string createCompetitorLine(DatabaseDataset.CompetitorsRow competitor)
        {
            DatabaseDataset.ShootersRow shooter =
                competitor.ShootersRow;
            //Allberg.Shooter.WinShooterServerRemoting.Structs.Shooter shooter =
            //    myInterface.GetShooter(competitor.ShooterId);

            StringBuilder toReturn = new StringBuilder();
            if (int.Parse(shooter.Cardnr) >= 0)
                toReturn.Append(shooter.Cardnr + ";");
            else
                toReturn.Append(";");
            toReturn.Append(shooter.ClubId + ";");
            toReturn.Append(shooter.Givenname + ";");
            toReturn.Append(shooter.Surname + ";");
            toReturn.Append(getShooterClassString(competitor.ShooterClass) + ";");
            toReturn.Append(shooter.Email + ";");
            toReturn.Append(competitor.WeaponId + ";");
            if (competitor.PatrolsRow != null)
            {
                DatabaseDataset.PatrolsRow patrol =
                    competitor.PatrolsRow;
                toReturn.Append(patrol.PatrolId.ToString() + ";");
                toReturn.Append(competitor.Lane.ToString() + ";");
            }
            else
            {
                toReturn.Append(";");
                toReturn.Append(";");
            }
            toReturn.Append("\r\n");
            
            return toReturn.ToString();
        }

        private string getShooterClassString(int classInt)
        {
            Structs.ShootersClassShort sclass =
                (Structs.ShootersClassShort)classInt;
            return sclass.ToString().Replace("Klass", "").Replace("ÖP", "Ö");
        }
#endregion

        #region ExportWeapons
        internal byte[] ExportWeapons()
        {
            database = myInterface.databaseClass.Database;

            StringBuilder toReturn = new StringBuilder();
            toReturn.Append("WeaponId;Tillverkare;Modell;Kaliber;\r\n");
            foreach (DatabaseDataset.WeaponsRow weapon in
                database.Weapons)
            {
                toReturn.Append(createWeaponsLine(weapon));
            }

            Encoding encoding = Encoding.GetEncoding(1252);
            byte[] bytes = encoding.GetBytes(toReturn.ToString());

            return bytes;

        }
        private string createWeaponsLine(DatabaseDataset.WeaponsRow weapon)
        {
            StringBuilder toReturn = new StringBuilder();

            toReturn.Append(weapon.WeaponId + ";");
            toReturn.Append(weapon.Manufacturer + ";");
            toReturn.Append(weapon.Model + ";");
            toReturn.Append(weapon.Caliber + ";");
            toReturn.Append("\r\n");

            return toReturn.ToString();
        }
        #endregion

        #region ExportClubs
        internal byte[] ExportClubs()
        {
            database = myInterface.databaseClass.Database;

            StringBuilder toReturn = new StringBuilder();
            toReturn.Append("Klubbid;Namn;Land;Epost;Bankgiro;Plusgiro;\r\n");
            foreach (DatabaseDataset.ClubsRow club in
                database.Clubs)
            {
                toReturn.Append(createClubsLine(club));
            }

            Encoding encoding = Encoding.GetEncoding(1252);
            byte[] bytes = encoding.GetBytes(toReturn.ToString());

            return bytes;
        }
        private string createClubsLine(DatabaseDataset.ClubsRow club)
        {
            StringBuilder toReturn = new StringBuilder();

            toReturn.Append(club.ClubId + ";");
            toReturn.Append(club.Name + ";");
            toReturn.Append(club.Country + ";");
            if (!club.IsEmailNull())
                toReturn.Append(club.Email + ";");
            else
                toReturn.Append(";");
            if (!club.IsBankgiroNull())
                toReturn.Append(club.Bankgiro + ";");
            else
                toReturn.Append(";");
            if (!club.IsPlusgiroNull())
                toReturn.Append(club.Plusgiro + ";");
            else
                toReturn.Append(";");
            toReturn.Append("\r\n");

            return toReturn.ToString();
        }
        #endregion
    }
}
