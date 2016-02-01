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
    using System.Collections;
    using System.Data;
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
                    DSResults results = getAllTeams(wclass);
                    results = sortTeams(results);

                    ResultsReturnTeam[] toReturn = 
                        convertIntoArray(results);

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


        private DSResults getAllTeams(Structs.ResultWeaponsClass wclass)
        {
            DSResults results = new DSResults();

            // Add columns for each station (used later for sorting)
            int stationCount = myInterface.GetStationsCount();
            for(int stationNr=1;stationNr<=stationCount;stationNr++)
            {
                results.TeamResults.Columns.Add("Station" + stationNr.ToString(), typeof(int));
            }
            if (myInterface.CompetitionCurrent.Type == Structs.CompetitionTypeEnum.Precision)
            {
                for(int stationNr=myInterface.GetStationsCount();stationNr>=1;stationNr--)
                {
                    for(int i=10; i>0; i--)
                    {
                        results.TeamResults.Columns.Add("Station" + stationNr.ToString() + "-" + i.ToString(), typeof(int));
                    }
                }
            }

            foreach(DatabaseDataset.TeamsRow teamrow in database.Teams)
            {
                if (teamrow.WClass == (int)wclass)
                {
                    ArrayList comps = new ArrayList();

                    if (!teamrow.IsCompetitorId1Null())
                        comps.Add((DatabaseDataset.CompetitorsRow)
                            database.Competitors.Select("CompetitorId=" + teamrow.CompetitorId1.ToString())[0]);
                    if (!teamrow.IsCompetitorId2Null())
                        comps.Add((DatabaseDataset.CompetitorsRow)
                            database.Competitors.Select("CompetitorId=" + teamrow.CompetitorId2.ToString())[0]);
                    if (!teamrow.IsCompetitorId3Null())
                        comps.Add((DatabaseDataset.CompetitorsRow)
                            database.Competitors.Select("CompetitorId=" + teamrow.CompetitorId3.ToString())[0]);
                    if (!teamrow.IsCompetitorId4Null())
                        comps.Add((DatabaseDataset.CompetitorsRow)
                            database.Competitors.Select("CompetitorId=" + teamrow.CompetitorId4.ToString())[0]);
                    if (!teamrow.IsCompetitorId5Null())
                        comps.Add((DatabaseDataset.CompetitorsRow)
                            database.Competitors.Select("CompetitorId=" + teamrow.CompetitorId5.ToString())[0]);

                    Hashtable teamHits = new Hashtable();
                    Hashtable teamFigureHits = new Hashtable();
                    Hashtable teamCountNrOfTens = new Hashtable();

                    int totPoints = 0;
                    foreach(DatabaseDataset.CompetitorsRow compsRow in
                        (DatabaseDataset.CompetitorsRow[])
                        comps.ToArray(typeof(DatabaseDataset.CompetitorsRow)))
                    {
                        foreach(DatabaseDataset.CompetitorResultsRow compresrow in
                            (DatabaseDataset.CompetitorResultsRow[])
                            compsRow.GetChildRows("CompetitorsCompetitorResults"))
                        {
                            DatabaseDataset.StationsRow stationsRow =
                                (DatabaseDataset.StationsRow)database.Stations.Select("StationId=" + compresrow.StationId.ToString())[0];
                            
                            int teamHitsThisStn = 0;
                            if (teamHits.ContainsKey(stationsRow.StationNr))
                                teamHitsThisStn = (int)teamHits[stationsRow.StationNr];

                            int teamFigureHitsThisStn = 0;
                            if (teamFigureHits.ContainsKey(stationsRow.StationNr))
                                teamFigureHitsThisStn = (int)teamFigureHits[stationsRow.StationNr];

                            teamHitsThisStn += compresrow.Hits;
                            teamFigureHitsThisStn += compresrow.FigureHits;
                            totPoints += compresrow.Points;

                            teamHits[stationsRow.StationNr] = teamHitsThisStn;
                            teamFigureHits[stationsRow.StationNr] = teamFigureHitsThisStn;
                            if (CompetitionType == Structs.CompetitionTypeEnum.Precision)
                            {
                                foreach(string strn in compresrow.StationFigureHits.Split(';'))
                                {
                                    if (strn != "" && int.Parse(strn) != 0)
                                    {
                                        string columnName = "Station" + stationsRow.StationNr.ToString() + "-" +
                                            strn;
                                        int thisValue = 0;
                                        if (teamCountNrOfTens.ContainsKey(columnName))
                                            thisValue = (int)teamCountNrOfTens[columnName];
                                        thisValue++;
                                        teamCountNrOfTens[columnName] = thisValue;
                                    }
                                }
                            }
                        }
                    }

                    int totFigureHits = 0;
                    int totHits = 0;
                    string stnHits = "";
                    DSResults.TeamResultsRow newTeamRow = 
                        results.TeamResults.NewTeamResultsRow();

                    if (myInterface.GetCompetitions()[0].Type == Structs.CompetitionTypeEnum.Precision)
                    {
                        for(int stationNr=myInterface.GetStationsCount();stationNr>=1;stationNr--)
                        {
                            for(int i=10; i>0; i--)
                            {
                                newTeamRow["Station" + stationNr.ToString() + "-" + i.ToString()] = 0;
                            }
                        }
                    }

                    for(int stationNr=1;stationNr<=stationCount;stationNr++)
                    {
                        int hitsThisStn = 0;
                        int figureHitsThisStn = 0;
                        if (teamHits.ContainsKey(stationNr))
                            hitsThisStn =(int)teamHits[stationNr];
                        if (teamFigureHits.ContainsKey(stationNr))
                            figureHitsThisStn = (int)teamFigureHits[stationNr];

                        totHits += hitsThisStn;
                        totFigureHits += figureHitsThisStn;
                        switch(CompetitionType)
                        {
                            case Structs.CompetitionTypeEnum.Field:
                            {
                                if (this.useNorwegianCount)
                                {
                                    stnHits += (hitsThisStn + figureHitsThisStn).ToString() + ";";
                                    newTeamRow["Station" + stationNr.ToString()] = hitsThisStn + figureHitsThisStn;
                                }
                                else
                                {
                                    stnHits += hitsThisStn.ToString() + "/" + figureHitsThisStn.ToString() + ";";
                                    newTeamRow["Station" + stationNr.ToString()] = hitsThisStn;
                                }
                                break;
                            }
                            case Structs.CompetitionTypeEnum.Precision:
                            {
                                stnHits += hitsThisStn.ToString() + ";";
                                newTeamRow["Station" + stationNr.ToString()] = hitsThisStn;
                                for(int i=10;i>=1;i--)
                                {
                                    string columnName = "Station" + stationNr.ToString() + "-" +
                                        i.ToString();
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

        private DSResults sortTeams(DSResults results)
        {
            string sortExpression = "";
            int StationsCount = myInterface.GetStationsCount();
            switch(CompetitionType)
            {
                case Structs.CompetitionTypeEnum.Field:
                    {
                        if (this.useNorwegianCount)
                        {
                            // Poängfäljtskjutning
                            sortExpression = "NorwPoints desc, Points desc, ";
                            // Add columns for each station
                            for (int stationNr = StationsCount; stationNr >= 1; stationNr--)
                            {
                                sortExpression += "Station" + stationNr.ToString() + " desc, ";
                            }
                        }
                        else
                        {
                            // Fältskjutning
                            sortExpression = "Hits desc, FigureHits desc, Points desc, ";
                            for (int stationNr = StationsCount; stationNr >= 1; stationNr--)
                            {
                                sortExpression += "Station" + stationNr.ToString() + " desc, ";
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
                            for (int stationNr = StationsCount; stationNr >= 1; stationNr--)
                            {
                                sortExpression += "Station" + stationNr.ToString() + " desc, ";
                            }
                        }
                        else
                        {
                            // Fältskjutning
                            sortExpression = "Hits desc, FigureHits desc, Points desc, ";
                            for (int stationNr = StationsCount; stationNr >= 1; stationNr--)
                            {
                                sortExpression += "Station" + stationNr.ToString() + " desc, ";
                            }

                        }
                        break;
                    }
                case Structs.CompetitionTypeEnum.Precision:
                {
                    sortExpression = "Hits desc, ";
                    for(int stationNr=StationsCount;stationNr>=1;stationNr--)
                    {
                        sortExpression += "Station" + stationNr.ToString() + " desc, ";
                    }
                    for(int stationNr=StationsCount;stationNr>=1;stationNr--)
                    {
                        for(int i=10;i>=1;i--)
                        {
                            sortExpression += "Station" + stationNr.ToString() + "-" + i.ToString() + " desc, ";
                        }
                    }
                    break;
                }
                default:
                    throw new NotImplementedException(CompetitionType.ToString());
            }
            sortExpression = sortExpression.Trim();
            if (sortExpression.Substring(sortExpression.Length-1, 1) == ",")
            {
                sortExpression = sortExpression.Substring(0,sortExpression.Length-1);
            }

            DataRow[] dataRows = results.TeamResults.Select("", sortExpression);

            DSResults newResults = new DSResults();
            foreach(DataRow row in dataRows)
            {
                DSResults.TeamResultsRow newRow =
                    newResults.TeamResults.NewTeamResultsRow();

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

        private ResultsReturnTeam[] convertIntoArray(DSResults results)
        {
            ResultsReturnTeam[] returnArray =
                new ResultsReturnTeam[results.TeamResults.Count];
            int i = 0;
            foreach(DSResults.TeamResultsRow row in results.TeamResults)
            {
                returnArray[i] = new ResultsReturnTeam();
                returnArray[i].ClubId = row.ClubId;
                returnArray[i].TeamId = row.TeamId;
                returnArray[i].Hits = row.Hits;
                returnArray[i].Points = row.Points;
                returnArray[i].TeamName = row.TeamName;
                returnArray[i].FigureHits = row.FigureHits;
                returnArray[i].HitsPerStn = row.HitsPerStn;
                i++;
            }
            return returnArray;
        }

    }
}