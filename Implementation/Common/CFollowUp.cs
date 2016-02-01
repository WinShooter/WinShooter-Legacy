// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CFollowUp.cs" company="John Allberg">
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
//   Defines the CFollowUp type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System.Collections;
    using Allberg.Shooter.WinShooterServerRemoting;

    internal class CFollowUp
    {
        internal CFollowUp(Interface callerInterface)
        {
            myInterface = callerInterface;
            database = myInterface.databaseClass.Database;
        }

        internal enum SortingEnum
        {
            ByClub,
            ByLastnameFirstname
        }

        Interface myInterface;
        DatabaseDataset database;

        internal Structs.FollowUpReturn[] FollowUp(SortingEnum sorting)
        {
            ArrayList toReturn = new ArrayList();

            DatabaseDataset.ClubsRow[] clubs =
                (DatabaseDataset.ClubsRow[])
                database.Clubs.Select("", "Name");
            foreach (DatabaseDataset.ClubsRow club in clubs)
            {
                toReturn.AddRange(checkClub(club));
            }
            return (Structs.FollowUpReturn[])toReturn.ToArray(typeof(Structs.FollowUpReturn));
        }

        Structs.FollowUpReturn[] checkClub(DatabaseDataset.ClubsRow club)
        {
            string filter = "ClubId='" + club.ClubId + "'";
            //filter = "";
            ArrayList toReturn = new ArrayList();
            DatabaseDataset.ShootersRow[] shooters =
                (DatabaseDataset.ShootersRow[])
                //    club.GetShootersRows();
                database.Shooters.Select(filter, "GivenName, Surname");

            foreach (DatabaseDataset.ShootersRow shooter in shooters)
            {
                // This is for safety reasons
                if (shooter.IsArrivedNull())
                    shooter.Arrived = false;

                Structs.FollowUpReturn followUp = new Structs.FollowUpReturn();
                followUp.Arrived = shooter.Arrived;
                followUp.CardNr = shooter.Cardnr;
                followUp.ClubName = club.Name;
                followUp.Payed = shooter.Payed;
                followUp.Rounds = getRoundsForShooter(shooter);
                followUp.ResultsExistForRounds = getResultExistForShooter(shooter);
                followUp.ShooterName = shooter.Givenname + ", " + shooter.Surname;
                followUp.ShouldHavePayed = GetShouldHavePayed(shooter);
                toReturn.Add(followUp);
            }

            return (Structs.FollowUpReturn[])toReturn.ToArray(typeof(Structs.FollowUpReturn));
        }

        private int getRoundsForShooter(DatabaseDataset.ShootersRow shooter)
        {
            return shooter.GetCompetitorsRows().Length;
        }
        private int getResultExistForShooter(DatabaseDataset.ShootersRow shooter)
        {
            DatabaseDataset.CompetitorsRow[] competitors =
                (DatabaseDataset.CompetitorsRow[])
                shooter.GetCompetitorsRows();

            int count = 0;
            foreach (DatabaseDataset.CompetitorsRow competitor in
                competitors)
            {
                if (competitor.GetCompetitorResultsRows().Length > 0)
                    count++;
            }

            return count;
        }

        private int GetShouldHavePayed(DatabaseDataset.ShootersRow shooter)
        {
            int paying = 0;
            switch (shooter.GetCompetitorsRows().Length)
            {
                case 1:
                    paying = myInterface.CompetitionCurrent.ShooterFee1;
                    break;
                case 2:
                    paying = myInterface.CompetitionCurrent.ShooterFee1 +
                        myInterface.CompetitionCurrent.ShooterFee2;
                    break;
                case 3:
                    paying = myInterface.CompetitionCurrent.ShooterFee1 +
                        myInterface.CompetitionCurrent.ShooterFee2 +
                        myInterface.CompetitionCurrent.ShooterFee3;
                    break;
                case 4:
                    paying = myInterface.CompetitionCurrent.ShooterFee1 +
                        myInterface.CompetitionCurrent.ShooterFee2 +
                        myInterface.CompetitionCurrent.ShooterFee3 +
                        myInterface.CompetitionCurrent.ShooterFee4;
                    break;
            }
            return paying;
        }
    }
}
