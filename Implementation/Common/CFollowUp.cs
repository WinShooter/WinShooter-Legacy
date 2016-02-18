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
    using System.Collections.Generic;

    using Allberg.Shooter.WinShooterServerRemoting;

    internal class CFollowUp
    {
        private readonly Interface myInterface;

        private readonly DatabaseDataset database;

        /// <summary>
        /// Initializes a new instance of the <see cref="CFollowUp"/> class.
        /// </summary>
        /// <param name="callerInterface">
        /// The caller interface.
        /// </param>
        internal CFollowUp(Interface callerInterface)
        {
            this.myInterface = callerInterface;
            this.database = this.myInterface.databaseClass.Database;
        }

        internal enum SortingEnum
        {
            ByClub,
            ByLastnameFirstname
        }

        internal Structs.FollowUpReturn[] FollowUp(SortingEnum sorting)
        {
            var toReturn = new List<Structs.FollowUpReturn>();

            var clubs = (DatabaseDataset.ClubsRow[])this.database.Clubs.Select(string.Empty, "Name");
            foreach (var club in clubs)
            {
                toReturn.AddRange(this.checkClub(club));
            }

            return toReturn.ToArray();
        }

        private Structs.FollowUpReturn[] checkClub(DatabaseDataset.ClubsRow club)
        {
            var filter = "ClubId='" + club.ClubId + "'";

            var toReturn = new List<Structs.FollowUpReturn>();
            var shooters = (DatabaseDataset.ShootersRow[])
                this.database.Shooters.Select(filter, "GivenName, Surname");

            foreach (var shooter in shooters)
            {
                // This is for safety reasons
                if (shooter.IsArrivedNull())
                {
                    shooter.Arrived = false;
                }

                var followUp = new Structs.FollowUpReturn
                {
                    Arrived = shooter.Arrived,
                    CardNr = shooter.Cardnr,
                    ClubName = club.Name,
                    Payed = shooter.Payed,
                    Rounds = this.GetRoundsForShooter(shooter),
                    ResultsExistForRounds = this.GetResultExistForShooter(shooter),
                    ShooterName = shooter.Givenname + ", " + shooter.Surname,
                    ShouldHavePayed = this.GetShouldHavePayed(shooter)
                };

                toReturn.Add(followUp);
            }

            return toReturn.ToArray();
        }

        private int GetRoundsForShooter(DatabaseDataset.ShootersRow shooter)
        {
            return shooter.GetCompetitorsRows().Length;
        }

        private int GetResultExistForShooter(DatabaseDataset.ShootersRow shooter)
        {
            var competitors = shooter.GetCompetitorsRows();

            int count = 0;
            foreach (var competitor in competitors)
            {
                if (competitor.GetCompetitorResultsRows().Length > 0)
                {
                    count++;
                }
            }

            return count;
        }

        private int GetShouldHavePayed(DatabaseDataset.ShootersRow shooter)
        {
            int paying = 0;
            switch (shooter.GetCompetitorsRows().Length)
            {
                case 1:
                    paying = this.myInterface.CompetitionCurrent.ShooterFee1;
                    break;
                case 2:
                    paying = this.myInterface.CompetitionCurrent.ShooterFee1 +
                        this.myInterface.CompetitionCurrent.ShooterFee2;
                    break;
                case 3:
                    paying = this.myInterface.CompetitionCurrent.ShooterFee1 +
                        this.myInterface.CompetitionCurrent.ShooterFee2 +
                        this.myInterface.CompetitionCurrent.ShooterFee3;
                    break;
                case 4:
                    paying = this.myInterface.CompetitionCurrent.ShooterFee1 +
                        this.myInterface.CompetitionCurrent.ShooterFee2 +
                        this.myInterface.CompetitionCurrent.ShooterFee3 +
                        this.myInterface.CompetitionCurrent.ShooterFee4;
                    break;
            }

            return paying;
        }
    }
}
