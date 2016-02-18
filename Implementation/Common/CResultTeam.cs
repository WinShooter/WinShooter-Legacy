// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CResultTeam.cs" company="John Allberg">
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
//   Summary description for CResultTeam.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    using Allberg.Shooter.Common.DataSets;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for CResultTeam.
    /// </summary>
    internal class CResultTeam
    {
        internal CResultTeam(Interface callerInterface)
        {
            myInterface = callerInterface;
        }

        private Interface myInterface;
        private DatabaseDataset database = null;
        private bool useNorwegianCount = false;
        private Structs.CompetitionTypeEnum CompetitionType;
        readonly object GetResultsLock = new object();
        bool resultsAlreadyRunning = false;

        internal ResultsReturnTeam[] GetTeamResults(Structs.ResultWeaponsClass wclass, 
            Structs.Competition competition)
        {
            Trace.WriteLine("CResults.GetResults(" + wclass.ToString() +
                ") started on thread \"" +
                System.Threading.Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            while (resultsAlreadyRunning)
                System.Threading.Thread.Sleep(50);

            Trace.WriteLine("CResults: GetResults() " + 
                " locking \"GetResultsLock\" on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            lock(GetResultsLock)
            {
                Trace.WriteLine("CResults: GetResults() " + 
                    " locked \"GetResultsLock\" on thread \"" +
                    Thread.CurrentThread.Name + "\" ( " +
                    System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

                try
                {
                    resultsAlreadyRunning = true;
                    CompetitionType = myInterface.CompetitionCurrent.Type;
                    database = myInterface.databaseClass.Database;

                    useNorwegianCount = competition.NorwegianCount;

                    // Ok, now lets count the real ones
                    DSResults results = this.GetAllTeams(wclass);
                    results = this.SortTeams(results);

                    ResultsReturnTeam[] toReturn = 
                        ConvertIntoArray(results);

                    return toReturn;
                }
                finally
                {
                    Trace.WriteLine("CResultsTeam: GetResults() " + 
                        " unlocking \"GetResultsLock\" on thread \"" +
                        Thread.CurrentThread.Name + "\" ( " +
                        System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

                    Trace.WriteLine("CResults.GetResults ended.");
                    resultsAlreadyRunning = false;
                }
            }
        }


        private DSResults GetAllTeams(Structs.ResultWeaponsClass wclass)
        {
            DSResults results = new DSResults();

            // Add columns for each station (used later for sorting)
            int stationCount = this.myInterface.GetStationsCount();
            for (int stationNr = 1; stationNr <= stationCount; stationNr++)
            {
                results.TeamResults.Columns.Add("Station" + stationNr, typeof(int));
            }

            if (this.myInterface.CompetitionCurrent.Type == Structs.CompetitionTypeEnum.Precision)
            {
                for (int stationNr = this.myInterface.GetStationsCount(); stationNr >= 1; stationNr--)
                {
                    for (var i = 10; i > 0; i--)
                    {
                        results.TeamResults.Columns.Add("Station" + stationNr + "-" + i, typeof(int));
                    }
                }
            }

            foreach (DatabaseDataset.TeamsRow teamrow in this.database.Teams)
            {
                if (teamrow.WClass == (int)wclass)
                {
                    var comps = new List<DatabaseDataset.CompetitorsRow>();

                    if (!teamrow.IsCompetitorId1Null())
                    {
                        comps.Add(
                            (DatabaseDataset.CompetitorsRow)this.database.Competitors.Select("CompetitorId=" + teamrow.CompetitorId1)[0]);
                    }

                    if (!teamrow.IsCompetitorId2Null())
                    {
                        comps.Add(
                            (DatabaseDataset.CompetitorsRow)this.database.Competitors.Select("CompetitorId=" + teamrow.CompetitorId2)[0]);
                    }

                    if (!teamrow.IsCompetitorId3Null())
                    {
                        comps.Add(
                            (DatabaseDataset.CompetitorsRow)this.database.Competitors.Select("CompetitorId=" + teamrow.CompetitorId3)[0]);
                    }

                    if (!teamrow.IsCompetitorId4Null())
                    {
                        comps.Add(
                            (DatabaseDataset.CompetitorsRow)this.database.Competitors.Select("CompetitorId=" + teamrow.CompetitorId4)[0]);
                    }

                    if (!teamrow.IsCompetitorId5Null())
                    {
                        comps.Add(
                            (DatabaseDataset.CompetitorsRow)this.database.Competitors.Select("CompetitorId=" + teamrow.CompetitorId5)[0]);
                    }

                    var teamHits = new Dictionary<int, int>();
                    var teamFigureHits = new Dictionary<int, int>();
                    var teamCountNrOfTens = new Dictionary<string, int>();

                    int totPoints = 0;
                    foreach (var compsRow in comps)
                    {
                        foreach (var compresrow in (DatabaseDataset.CompetitorResultsRow[])
                            compsRow.GetChildRows("CompetitorsCompetitorResults"))
                        {
                            var stationsRow =
                                (DatabaseDataset.StationsRow)this.database.Stations.Select("StationId=" + compresrow.StationId.ToString())[0];
                            
                            int teamHitsThisStn = 0;
                            if (teamHits.ContainsKey(stationsRow.StationNr))
                            {
                                teamHitsThisStn = teamHits[stationsRow.StationNr];
                            }

                            var teamFigureHitsThisStn = 0;
                            if (teamFigureHits.ContainsKey(stationsRow.StationNr))
                            {
                                teamFigureHitsThisStn = teamFigureHits[stationsRow.StationNr];
                            }

                            teamHitsThisStn += compresrow.Hits;
                            teamFigureHitsThisStn += compresrow.FigureHits;
                            totPoints += compresrow.Points;

                            teamHits[stationsRow.StationNr] = teamHitsThisStn;
                            teamFigureHits[stationsRow.StationNr] = teamFigureHitsThisStn;
                            if (this.CompetitionType == Structs.CompetitionTypeEnum.Precision)
                            {
                                foreach (var strn in compresrow.StationFigureHits.Split(';'))
                                {
                                    if (strn != string.Empty && int.Parse(strn) != 0)
                                    {
                                        var columnName = "Station" + stationsRow.StationNr + "-" +
                                            strn;
                                        int thisValue = 0;
                                        if (teamCountNrOfTens.ContainsKey(columnName))
                                        {
                                            thisValue = teamCountNrOfTens[columnName];
                                        }

                                        thisValue++;
                                        teamCountNrOfTens[columnName] = thisValue;
                                    }
                                }
                            }
                        }
                    }

                    int totFigureHits = 0;
                    int totHits = 0;
                    string stnHits = string.Empty;
                    DSResults.TeamResultsRow newTeamRow = results.TeamResults.NewTeamResultsRow();

                    if (this.myInterface.GetCompetitions()[0].Type == Structs.CompetitionTypeEnum.Precision)
                    {
                        for (int stationNr = this.myInterface.GetStationsCount(); stationNr >= 1; stationNr--)
                        {
                            for (var i = 10; i > 0; i--)
                            {
                                newTeamRow["Station" + stationNr + "-" + i] = 0;
                            }
                        }
                    }

                    for (int stationNr = 1; stationNr <= stationCount; stationNr++)
                    {
                        int hitsThisStn = 0;
                        int figureHitsThisStn = 0;
                        if (teamHits.ContainsKey(stationNr))
                        {
                            hitsThisStn = (int)teamHits[stationNr];
                        }
                        if (teamFigureHits.ContainsKey(stationNr))
                        {
                            figureHitsThisStn = (int)teamFigureHits[stationNr];
                        }

                        totHits += hitsThisStn;
                        totFigureHits += figureHitsThisStn;
                        switch (this.CompetitionType)
                        {
                            case Structs.CompetitionTypeEnum.Field:
                            {
                                if (this.useNorwegianCount)
                                {
                                    stnHits += (hitsThisStn + figureHitsThisStn) + ";";
                                    newTeamRow["Station" + stationNr] = hitsThisStn + figureHitsThisStn;
                                }
                                else
                                {
                                    stnHits += hitsThisStn + "/" + figureHitsThisStn + ";";
                                    newTeamRow["Station" + stationNr] = hitsThisStn;
                                }

                                break;
                            }

                            case Structs.CompetitionTypeEnum.Precision:
                            {
                                stnHits += hitsThisStn + ";";
                                newTeamRow["Station" + stationNr] = hitsThisStn;
                                for (var i = 10; i >= 1; i--)
                                {
                                    var columnName = "Station" + stationNr + "-" + i;
                                    if (teamCountNrOfTens.ContainsKey(columnName))
                                    {
                                        newTeamRow[columnName] = (int)teamCountNrOfTens[columnName];
                                    }
                                    else
                                    {
                                        newTeamRow[columnName] = 0;
                                    }
                                }

                                break;
                            }
                        }
                    }

                    newTeamRow.ClubId = teamrow.ClubId;
                    newTeamRow.FigureHits = totFigureHits;
                    newTeamRow.Hits = totHits;
                    newTeamRow.NorwPoints = totFigureHits + totHits;
                    newTeamRow.Points = totPoints;
                    newTeamRow.TeamId = teamrow.TeamId;
                    newTeamRow.TeamName = teamrow.Name;
                    newTeamRow.HitsPerStn = stnHits;
                    results.TeamResults.AddTeamResultsRow(newTeamRow);
                }
            }

            return results;
        }

        private DSResults SortTeams(DSResults results)
        {
            string sortExpression;
            var stationsCount = myInterface.GetStationsCount();
            switch (this.CompetitionType)
            {
                case Structs.CompetitionTypeEnum.Field:
                    {
                        if (this.useNorwegianCount)
                        {
                            // Poängfäljtskjutning
                            sortExpression = "NorwPoints desc, Points desc, ";
                            // Add columns for each station
                            for (int stationNr = stationsCount; stationNr >= 1; stationNr--)
                            {
                                sortExpression += "Station" + stationNr + " desc, ";
                            }
                        }
                        else
                        {
                            // Fältskjutning
                            sortExpression = "Hits desc, FigureHits desc, Points desc, ";
                            for (var stationNr = stationsCount; stationNr >= 1; stationNr--)
                            {
                                sortExpression += "Station" + stationNr + " desc, ";
                            }

                        }

                        break;
                    }

                case Structs.CompetitionTypeEnum.MagnumField:
                    {
                        if (this.useNorwegianCount)
                        {
                            // Poängfältskjutning
                            sortExpression = "NorwPoints desc, Points desc, ";

                            // Add columns for each station
                            for (int stationNr = stationsCount; stationNr >= 1; stationNr--)
                            {
                                sortExpression += "Station" + stationNr + " desc, ";
                            }
                        }
                        else
                        {
                            // Fältskjutning
                            sortExpression = "Hits desc, FigureHits desc, Points desc, ";
                            for (int stationNr = stationsCount; stationNr >= 1; stationNr--)
                            {
                                sortExpression += "Station" + stationNr + " desc, ";
                            }

                        }
                        break;
                    }
                case Structs.CompetitionTypeEnum.Precision:
                {
                    sortExpression = "Hits desc, ";
                    for (int stationNr = stationsCount; stationNr >= 1; stationNr--)
                    {
                        sortExpression += "Station" + stationNr + " desc, ";
                    }

                    for (int stationNr = stationsCount; stationNr >= 1; stationNr--)
                    {
                        for (int i = 10; i >= 1; i--)
                        {
                            sortExpression += "Station" + stationNr + "-" + i + " desc, ";
                        }
                    }
                    break;
                }
                default:
                    {
                        throw new NotImplementedException(this.CompetitionType.ToString());
                    }
            }

            sortExpression = sortExpression.Trim();
            if (sortExpression.Substring(sortExpression.Length - 1, 1) == ",")
            {
                sortExpression = sortExpression.Substring(0, sortExpression.Length - 1);
            }

            var dataRows = results.TeamResults.Select(string.Empty, sortExpression);

            var newResults = new DSResults();
            foreach (var row in dataRows)
            {
                var newRow = newResults.TeamResults.NewTeamResultsRow();

                newRow.ClubId = (string)row["ClubId"];
                newRow.TeamId = (int)row["TeamId"];
                newRow.Hits = (int)row["Hits"];
                newRow.FigureHits = (int)row["FigureHits"];
                newRow.Points = (int)row["Points"];
                newRow.TeamName = (string)row["TeamName"];
                newRow.HitsPerStn = (string)row["HitsPerStn"];
                newRow.NorwPoints = (int)row["NorwPoints"];

                newResults.TeamResults.AddTeamResultsRow(newRow);
            }

            return newResults;
        }

        private static ResultsReturnTeam[] ConvertIntoArray(DSResults results)
        {
            var returnArray = new List<ResultsReturnTeam>();
            
            foreach (DSResults.TeamResultsRow row in results.TeamResults)
            {
                var result = new ResultsReturnTeam
                {
                    ClubId = row.ClubId,
                    TeamId = row.TeamId,
                    Hits = row.Hits,
                    Points = row.Points,
                    TeamName = row.TeamName,
                    FigureHits = row.FigureHits,
                    HitsPerStn = row.HitsPerStn
                };
                returnArray.Add(result);
            }

            return returnArray.ToArray();
        }

    }
}