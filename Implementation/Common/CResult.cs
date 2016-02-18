// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CResult.cs" company="John Allberg">
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
//   Summary description for CResult.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Threading;

    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for CResult.
    /// </summary>
    [Serializable]
    internal class CResult
    {
        /* 
         * Resultat som beräknas här görs enligt B3.6.1.2 för fältskytte
         * och B3.6.1.3 för poängfältskjutning (norsk räkning)
         * i Svenska Pistolskytteförbundets skjuthandbok, 11 omarbetade 
         * upplagan, uppdaterad 2004-03-02.
         * 
         * "B3.6.1.2 Fältskjutning
         * Särskiljning vid lika antal träff äger rum enligt följande:
         * 1. Största antalet träffade figurer
         * 2. högsta poäng i eventuellt förekommande ringad figur
         * 3. största antalet träff på sista stationen, näst sista stationen 
         * o.s.v,
         * 
         * Kan särskiljning inte erhållas, skall antingen särskjutning ske 
         * tills särsskiljning erhållits, eller då särskjutning inte kan 
         * eller inte anses böra ske, lottning utföras."
         * 
         * I detta program tillämpas det genom att sortera på FigureHits, 
         * FinalShootingPlace, StationHits, Points.
         * 
         * "B3.6.1.3 Poängfältskjutning
         * Särskiljning vid lika antal poäng äger rum enligt följande
         * 1. högsta poäng i ringad figur.
         * 2. högsta antal poäng på sista, näst sista stationen osv,
         * 
         * Kan särskiljning ändå inte erhållas, skall antingen särskjutning 
         * ske till särskiljning erhållits, eller då särskjutning inte kan 
         * eller inte anses böra ske, lottning utföras."
         * 
         * I detta program erhålls detta genom att sortera på NorwPoints 
         * (summan träffade figurer och träff), FinalShootingPlace,
         * StationHits och Points
         * 
         * SHB 2005:
         * "B3.6.1.1 Precisionsskjutning
         * Särskiljning vid lika poängtal äger rum enligt följande:
         * 1. högsta poängtalet i sista serien, därefter näst sista serien o.s.v.
         * 2. Starta antalet 10:or därefter 9:or osv i  sista serien, 
         * därefter motsvarande i näst sista serien o.s.v.
         * 
         * Kan särskiljning ändå inte erhållas, skall antingen särskjutning ske med serie
         * (serier) tills särskiljning erhållits, eller då särskjutning inte kan eller 
         * inte anses böra ske, lottning utföras."
         * 
         * Standardmedaljer
         * ================
         * "D8.1.1 Beräkning av antalet standardmedaljer
         * Vid SM och landsdlesmästerskap skall alltid standardmedalj-
         * beräkning ske spearat för varje mästerskap i vapengrupp C 
         * (Öppen C, Dam C, Vet(Y+Ä), och Jun C) trots vas som sägs i 
         * D8.1.2.1 nedan)"
         * 
         * "D8.1.1.3 Fältskjutning
         * Samtliga deltagare som fullföljt tävlingen, sammanförs oavsett 
         * klasstillhörighet vapengruppsvis (A, B, C och R), för SM och 
         * landsdelsmästerskap gäller dock D8.1.1 ovan."
         * 
         * "D8.1.2 Beräkningsmetod
         * Sammanslagning av klasser får inte ske om inte förutsättningen 
         * (svårighetsgrad) varit densamma för alla i gruppen, undantag VÄ 
         * som i fältskjutning får skjuta med stödhand. För SM och 
         * landsdelsmästerskap se även D8.1.1 ovan.
         * 
         * Berättigad till standardmedaljen i silver och brons är den bästa 
         * niondelen resp. bästa tredjedelen av antalet skyttar i varje av 
         * ovanstående grupper (för skidskytte- och springskyttetävling i 
         * klasser) samt de som uppnått samma poängtal som dessa.
         * - Vid fältskjutning gäller samma träffantal.
         * - Vid poängfältskjutning gäller samma antal poäng.
         * - Vid magnumfältskjutning fäller samma antal träff, träffade figurer och poäng.
         * - Vid beräkningen avrundas till närmast lägre hela tal."
         * 
        */
        internal CResult(Interface callerInterface)
        {
            myInterface = callerInterface;
        }
        private Interface myInterface;
        private DatabaseDataset database = null;
        private bool useNorwegianCount = false;
        private int standardBronsMedal = defaultStandardMedalLimit;
        private int standardSilverMedal = defaultStandardMedalLimit;
        private const int defaultStandardMedalLimit = 1000;

        readonly object GetResultsLock = new object();
        bool resultsAlreadyRunning = false;
        internal ResultsReturn[] GetResults(
            Structs.ResultWeaponsClass wclass, 
            Structs.ShootersClass uclass,
            Structs.Competition competition)
        {
            return GetResults(
                wclass, 
                uclass,
                competition,
                false);
        }

        internal ResultsReturn ResultsGetCompetitor(
            int CompetitorId)
        {
            Structs.Competitor comp = myInterface.GetCompetitor(CompetitorId);
            return ResultsGetCompetitor(comp);
        }
        internal ResultsReturn ResultsGetCompetitor(
            Structs.Competitor competitor)
        {
            Structs.Competition competition = myInterface.CompetitionCurrent;
            ResultsReturn toReturn = 
                new ResultsReturn(
                    competition.Type, 
                    competition.NorwegianCount,
                    competition.Championship);
            
            Structs.Shooter shooter = myInterface.GetShooter(competitor.ShooterId);

            Structs.CompetitorResult[] results = 
                myInterface.GetCompetitorResults(competitor.CompetitorId);

            int figureHits = 0;
            int hits = 0;
            int points = 0;

            foreach (Structs.CompetitorResult result in results)
            {
                figureHits += result.FigureHits;
                hits += result.Hits;
                points += result.Points;

                toReturn.AddHitsPerStn(result.Hits);
                toReturn.AddFigureHitsPerStn(result.FigureHits);
                toReturn.AddPointsPerStn(result.Points);
            }

            toReturn.ClubId = shooter.ClubId;
            toReturn.CompetitorId = competitor.CompetitorId;
            toReturn.FigureHitsTotal = figureHits;
            toReturn.HitsTotal = hits;
            toReturn.PointsTotal = points;
            toReturn.ShooterName = shooter.Surname + " " + shooter.Givenname;
            toReturn.FinalShootingPlace = competitor.FinalShootingPlace;
            return toReturn;
        }
        internal ResultsReturn[] GetResults(
            Structs.ResultWeaponsClass wclass, 
            Structs.ShootersClass uclass,
            Structs.Competition competition,
            bool finalResults)
        {
            Trace.WriteLine("CResults.GetResults(" + wclass.ToString() 
                + ", " + uclass.ToString() + 
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

                    database = myInterface.databaseClass.Database;

                    useNorwegianCount = competition.NorwegianCount;

                    // First find out about the standard medals
                    calculateStandardMedals(wclass, uclass);

                    // Ok, now lets count the real ones
                    if (competition.OneClass)
                    {
                        switch (uclass)
                        {
                            case Structs.ShootersClass.Damklass:
                                uclass = Structs.ShootersClass.Damklass;
                                break;
                            case Structs.ShootersClass.Damklass1:
                                uclass = Structs.ShootersClass.Damklass;
                                break;
                            case Structs.ShootersClass.Damklass2:
                                uclass = Structs.ShootersClass.Damklass;
                                break;
                            case Structs.ShootersClass.Damklass3:
                                uclass = Structs.ShootersClass.Damklass;
                                break;
                            case Structs.ShootersClass.Juniorklass:
                                break;
                            case Structs.ShootersClass.Klass:
                                uclass = Structs.ShootersClass.Klass;
                                break;
                            case Structs.ShootersClass.Klass1:
                                uclass = Structs.ShootersClass.Klass;
                                break;
                            case Structs.ShootersClass.Klass2:
                                uclass = Structs.ShootersClass.Klass;
                                break;
                            case Structs.ShootersClass.Klass3:
                                uclass = Structs.ShootersClass.Klass;
                                break;
                            case Structs.ShootersClass.VeteranklassYngre:
                                break;
                            case Structs.ShootersClass.VeteranklassÄldre:
                                break;
                            case Structs.ShootersClass.Öppen:
                                break;
                            default:
                                throw new NotSupportedException("Structs.ShootersClass." +
                                    uclass.ToString() + " is not supported");
                        }
                    }
                    List<ResultsReturn> results = getAllCompetitors(wclass, uclass, false);
                    results.Sort();
                    if (myInterface.CompetitionCurrent.Championship !=
                        Structs.CompetitionChampionshipEnum.Club)
                    {
                        results = markMedals(results);
                    }
                    else
                    {
                        // Mark all as not having medals
                        foreach (ResultsReturn row in results)
                        {
                            row.Medal = Structs.Medal.None;
                        }
                    }

                    if (finalResults)
                    {
                        results = this.MarkPriceMoney(results);
                    }

                    return results.ToArray();
                }
                finally
                {
                    Trace.WriteLine("CResults: GetResults() " + 
                        " unlocking \"GetResultsLock\" on thread \"" +
                        Thread.CurrentThread.Name + "\" ( " +
                        System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

                    Trace.WriteLine("CResults.GetResults ended.");
                    resultsAlreadyRunning = false;
                }
            }
        }

        #region Standard Medals
        private void calculateStandardMedals(Structs.ResultWeaponsClass wclass,
            Structs.ShootersClass uclass)
        {
            List<ResultsReturn> results;
            Structs.Competition competition = myInterface.CompetitionCurrent;
            results = getAllCompetitors(
                wclass,
                uclass, 
                true);
            
            results.Sort();
            calculateMedals(results);
        }

        private void calculateMedals(List<ResultsReturn> results)
        {
            int i = 1;
            standardSilverMedal = defaultStandardMedalLimit;
            standardBronsMedal = defaultStandardMedalLimit;
            int nrOfSilverMedalShooters = (int)results.Count/9;
            int nrOfBronsMedalShooters = (int)results.Count/3;
            foreach(ResultsReturn row in results)
            {
                if (i <= nrOfSilverMedalShooters)
                {
                    if (useNorwegianCount)
                        standardSilverMedal = row.HitsTotal + row.FigureHitsTotal;
                    else
                        standardSilverMedal = row.HitsTotal;
                }
                if (i <= nrOfBronsMedalShooters)
                {
                    if (useNorwegianCount)
                        standardBronsMedal = row.HitsTotal + row.FigureHitsTotal;
                    else
                        standardBronsMedal = row.HitsTotal;
                }
                i++;
            }
            Trace.WriteLine("NrOfSilverMedalShooters(theory) = " + 
                nrOfSilverMedalShooters.ToString());
            Trace.WriteLine("NrOfBronsMedalShooters(theory) = " + 
                nrOfBronsMedalShooters.ToString());
            Trace.WriteLine("Limit Silver = " + 
                standardSilverMedal.ToString());
            Trace.WriteLine("Limit Brons = " + 
                standardBronsMedal.ToString());
        }

        private List<ResultsReturn> markMedals(List<ResultsReturn> results)
        {
            int i = 0;
            Structs.Competition competition = myInterface.CompetitionCurrent;
            switch(competition.Type)
            {
                case Structs.CompetitionTypeEnum.Field:
                {
                    if (useNorwegianCount)
                        foreach (ResultsReturn row in results)
                        {
                            i++;
                            if (row.NorwegianPoints >= standardSilverMedal)
                                row.Medal = Structs.Medal.StandardSilver;
                            else if (row.NorwegianPoints >= standardBronsMedal)
                                row.Medal = Structs.Medal.StardardBrons;
                            else 
                                row.Medal = Structs.Medal.None;
                        }
                    else
                        foreach (ResultsReturn row in results)
                        {
                            i++;
                            if (row.HitsTotal>=standardSilverMedal)
                                row.Medal = Structs.Medal.StandardSilver;
                            else if (row.HitsTotal>= standardBronsMedal)
                                row.Medal = Structs.Medal.StardardBrons;
                            else 
                                row.Medal = Structs.Medal.None;
                        }
                    break;
                }
                case Structs.CompetitionTypeEnum.MagnumField:
                {
                    foreach (ResultsReturn row in results)
                    {
                        i++;
                        if (row.HitsTotal>=standardSilverMedal)
                            row.Medal = Structs.Medal.StandardSilver;
                        else if (row.HitsTotal>= standardBronsMedal)
                            row.Medal = Structs.Medal.StardardBrons;
                        else row.Medal = Structs.Medal.None;
                    }
                    break;
                }
                case Structs.CompetitionTypeEnum.Precision:
                {
                    int series = myInterface.GetStationsCount();
                    foreach (ResultsReturn row in results)
                    {
                        i++;
                        if (row.HitsTotal>=standardSilverMedal)
                            row.Medal = Structs.Medal.StandardSilver;
                        else if (row.HitsTotal>= standardBronsMedal)
                            row.Medal = Structs.Medal.StardardBrons;
                        else row.Medal = Structs.Medal.None;

                        Structs.Competitor competitor = myInterface.GetCompetitor(row.CompetitorId);
                        Structs.Weapon weapon = myInterface.GetWeapon(competitor.WeaponId);
                        Structs.ResultWeaponsClass wclass =
                            CConvert.ConvertWeaponsClassToResultClass(
                                weapon.WClass, competition.Type);
                        switch(series)
                        {
                            case 6:
                            {
                                switch(wclass)
                                {
                                    case Structs.ResultWeaponsClass.A:
                                    {
                                        if (row.HitsTotal >= 277)
                                        {
                                            row.Medal = Structs.Medal.StandardSilver;
                                        }
                                        else if (row.HitsTotal >= 267 & row.Medal != Structs.Medal.StandardSilver)
                                        {
                                            row.Medal = Structs.Medal.StardardBrons;
                                        }
                                        break;
                                    }
                                    case Structs.ResultWeaponsClass.B:
                                    {
                                        if (row.HitsTotal >= 282)
                                        {
                                            row.Medal = Structs.Medal.StandardSilver;
                                        }
                                        else if (row.HitsTotal >= 273 & row.Medal != Structs.Medal.StandardSilver)
                                        {
                                            row.Medal = Structs.Medal.StardardBrons;
                                        }
                                        break;
                                    }
                                    case Structs.ResultWeaponsClass.C:
                                    {
                                        if (row.HitsTotal >= 283)
                                        {
                                            row.Medal = Structs.Medal.StandardSilver;
                                        }
                                        else if (row.HitsTotal >= 276 & row.Medal != Structs.Medal.StandardSilver)
                                        {
                                            row.Medal = Structs.Medal.StardardBrons;
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                            case 7:
                            {
                                switch(wclass)
                                {
                                    case Structs.ResultWeaponsClass.A:
                                    {
                                        if (row.HitsTotal >= 323)
                                        {
                                            row.Medal = Structs.Medal.StandardSilver;
                                        }
                                        else if (row.HitsTotal >= 312 & row.Medal != Structs.Medal.StandardSilver)
                                        {
                                            row.Medal = Structs.Medal.StardardBrons;
                                        }
                                        break;
                                    }
                                    case Structs.ResultWeaponsClass.B:
                                    {
                                        if (row.HitsTotal >= 329)
                                        {
                                            row.Medal = Structs.Medal.StandardSilver;
                                        }
                                        else if (row.HitsTotal >= 319 & row.Medal != Structs.Medal.StandardSilver)
                                        {
                                            row.Medal = Structs.Medal.StardardBrons;
                                        }
                                        break;
                                    }
                                    case Structs.ResultWeaponsClass.C:
                                    {
                                        if (row.HitsTotal >= 330)
                                        {
                                            row.Medal = Structs.Medal.StandardSilver;
                                        }
                                        else if (row.HitsTotal >= 322 & row.Medal != Structs.Medal.StandardSilver)
                                        {
                                            row.Medal = Structs.Medal.StardardBrons;
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                            case 10:
                            {
                                switch(wclass)
                                {
                                    case Structs.ResultWeaponsClass.A:
                                    {
                                        if (row.HitsTotal >= 461)
                                        {
                                            row.Medal = Structs.Medal.StandardSilver;
                                        }
                                        else if (row.HitsTotal>= 445 & row.Medal != Structs.Medal.StandardSilver)
                                        {
                                            row.Medal = Structs.Medal.StardardBrons;
                                        }
                                        break;
                                    }
                                    case Structs.ResultWeaponsClass.B:
                                    {
                                        if (row.HitsTotal >= 470)
                                        {
                                            row.Medal = Structs.Medal.StandardSilver;
                                        }
                                        else if (row.HitsTotal >= 455 & row.Medal != Structs.Medal.StandardSilver)
                                        {
                                            row.Medal = Structs.Medal.StardardBrons;
                                        }
                                        break;
                                    }
                                    case Structs.ResultWeaponsClass.C:
                                    {
                                        if (row.HitsTotal >= 471)
                                        {
                                            row.Medal = Structs.Medal.StandardSilver;
                                        }
                                        else if (row.HitsTotal>= 460 & row.Medal != Structs.Medal.StandardSilver)
                                        {
                                            row.Medal = Structs.Medal.StardardBrons;
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    break;
                }
                default:
                    throw new ApplicationException("Unknown competition type");
            }

            return results;
        }

        #endregion


        private List<ResultsReturn> getAllCompetitors(Structs.ResultWeaponsClass wclass, 
            Structs.ShootersClass uclass, bool standardMedalsCalculation)
        {
            List<ResultsReturn> results = new List<ResultsReturn>();
            Structs.Competition competition = this.myInterface.CompetitionCurrent;

            var weapons = new Dictionary<string, Structs.Weapon>();

            string selectString;
            string standardClass = "ShooterClass=" + ((int)Structs.ShootersClass.Klass1) +
                            " or ShooterClass=" + ((int)Structs.ShootersClass.Klass2) +
                            " or ShooterClass=" + ((int)Structs.ShootersClass.Klass3) +
                            " or ShooterClass=" + ((int)Structs.ShootersClass.Öppen);
            string damClass = "ShooterClass=" + ((int)Structs.ShootersClass.Damklass1) +
                            " or ShooterClass=" + ((int)Structs.ShootersClass.Damklass2) +
                            " or ShooterClass=" + ((int)Structs.ShootersClass.Damklass3);
            string extraClasses = "ShooterClass=" + ((int)Structs.ShootersClass.Juniorklass) +
                            " or ShooterClass=" + ((int)Structs.ShootersClass.VeteranklassYngre) +
                            " or ShooterClass=" + ((int)Structs.ShootersClass.VeteranklassÄldre);

            if (standardMedalsCalculation)
            {
                if (wclass == Structs.ResultWeaponsClass.C &&
                    (competition.Championship == Structs.CompetitionChampionshipEnum.SM |
                    competition.Championship == Structs.CompetitionChampionshipEnum.Landsdel))
                {
                    switch (uclass)
                    {
                        case Structs.ShootersClass.Klass:
                            selectString = standardClass;
                            break;
                        case Structs.ShootersClass.Klass1:
                            selectString = standardClass;
                            break;
                        case Structs.ShootersClass.Klass2:
                            selectString = standardClass;
                            break;
                        case Structs.ShootersClass.Klass3:
                            selectString = standardClass;
                            break;
                        case Structs.ShootersClass.Öppen:
                            selectString = "ShooterClass=" + ((int)uclass);
                            break;
                        case Structs.ShootersClass.Damklass:
                            selectString = damClass;
                            break;
                        case Structs.ShootersClass.Damklass1:
                            selectString = damClass;
                            break;
                        case Structs.ShootersClass.Damklass2:
                            selectString = damClass;
                            break;
                        case Structs.ShootersClass.Damklass3:
                            selectString = damClass;
                            break;
                        case Structs.ShootersClass.Juniorklass:
                            selectString = "ShooterClass=" + ((int)Structs.ShootersClass.Juniorklass);
                            break;
                        case Structs.ShootersClass.VeteranklassYngre:
                            selectString = "ShooterClass=" + ((int)Structs.ShootersClass.VeteranklassYngre);
                            break;
                        case Structs.ShootersClass.VeteranklassÄldre:
                            selectString = "ShooterClass=" + ((int)Structs.ShootersClass.VeteranklassÄldre);
                            break;
                        default:
                            throw new NotImplementedException("uclass: " + uclass);
                    }
                }
                else
                {
                    selectString = string.Empty; // Everyone in one calculation
                }
            }
            else
            {
                // Fetch the shooters
                switch (uclass)
                {
                    case Structs.ShootersClass.Damklass:
                        selectString = damClass;
                        break;
                    case Structs.ShootersClass.Klass:
                        selectString = standardClass;
                        break;
                    case Structs.ShootersClass.Okänd:
                        selectString = string.Empty;
                        break;
                    default:
                        selectString = "ShooterClass=" + ((int)uclass);
                        break;
                }
            }


            foreach (var dataRow in this.database.Competitors.Select(selectString))
            {
                var competitorsRow = (DatabaseDataset.CompetitorsRow)dataRow;

                // Setup a cache for weapons.
                Structs.Weapon weapon;
                if (weapons.ContainsKey(competitorsRow.WeaponId))
                {
                    weapon = weapons[competitorsRow.WeaponId];
                }
                else
                {
                    weapon = this.myInterface.GetWeapon(competitorsRow.WeaponId);
                    weapons.Add(competitorsRow.WeaponId, weapon);
                }

                // For each competitor, find the result (competitorresults)
                // and add together
                if (this.myInterface.ConvertWeaponsClassToResultClass(
                    weapon.WClass) == wclass |
                    wclass == Structs.ResultWeaponsClass.Unknown) 
                {
                    ResultsReturn thisResult = this.ResultsGetCompetitor(competitorsRow.CompetitorId);
                    if (thisResult.HitsTotal > 0)
                    {
                        results.Add(thisResult);
                    }
                }
            }

            return results;
        }

        private List<ResultsReturn> MarkPriceMoney(List<ResultsReturn> results)
        {
            var comp = this.myInterface.CompetitionCurrent;

            var numberOfShooterWithPrice = (int)(((double)comp.PriceMoneyShooterPercent) / 100 * results.Count);
            var totalAmount = this.getTotalAmountOfMoney();
            var prices = new CPriceMoney(
                numberOfShooterWithPrice,
                comp.ShooterFee1,
                comp.FirstPrice,
                totalAmount,
                comp.ShooterFee1);

            var priceArray = prices.Calculate(((double)comp.PriceMoneyPercentToReturn) / 100);
            for (int i = 0; i < priceArray.Length & i < results.Count; i++)
            {
                results[i].PriceMoney = priceArray[i];
            }

            return results;
        }

        private int getTotalAmountOfMoney()
        {
            int total = 0;
            int fee1 = myInterface.CompetitionCurrent.ShooterFee1;
            int fee2 = myInterface.CompetitionCurrent.ShooterFee2;
            int fee3 = myInterface.CompetitionCurrent.ShooterFee3;
            int fee4 = myInterface.CompetitionCurrent.ShooterFee4;

            foreach (Structs.Shooter shooter in myInterface.GetShooters())
            {
                int nrOfRounds = 0;;
                foreach(Structs.Competitor comp in myInterface.GetCompetitors(shooter.ShooterId))
                {
                    if (myInterface.GetCompetitorResults(comp.CompetitorId).Length > 1)
                        nrOfRounds++;
                }
                
                switch (nrOfRounds)
                {
                    case 1:
                        total += fee1;
                        break;
                    case 2:
                        total += fee1 +
                            fee2;
                        break;
                    case 3:
                        total += fee1 +
                            fee2 +
                            fee3;
                        break;
                    case 4:
                        total += fee1 +
                            fee2 +
                            fee3 +
                            fee4;
                        break;
                }
            }
            return total;
        }

        /*private ResultsReturn[] convertIntoArray(ResultsReturn results)
        {
            ResultsReturn[] returnArray =
                new ResultsReturn[results.Count];
            int i = 0;
            Structs.Competition competition = myInterface.GetCompetitions()[0];
            foreach(DSResults.ResultsRow row in results)
            {
                returnArray[i] = new ResultsReturn(competition.Type, competition.NorwegianCount);
                returnArray[i].ClubId = row.ClubId;
                returnArray[i].CompetitorId = row.CompetitorId;
                returnArray[i].HitsTotal = row.Hits;
                returnArray[i].Medal = row.Medal;
                returnArray[i].PointsTotal = row.Points;
                returnArray[i].ShooterName = row.ShooterName;
                returnArray[i].FigureHitsTotal = row.FigureHits;
                returnArray[i].HitsPerStnString = row.HitsPerStn;
                returnArray[i].FinalShootingPlace = row.FinalShootingPlace;
                returnArray[i].PriceMoney = row.PriceMoney;
                i++;
            }
            return returnArray;
        }*/

        internal Structs.ResultWeaponsClass[] ResultsGetWClasses()
        {
            this.database = this.myInterface.databaseClass.Database;

            List<Structs.ResultWeaponsClass> list = new List<Structs.ResultWeaponsClass>();

            for (int i = 1; i <= Structs.ResultWeaponsClassMax; i++)
            {
                Structs.ResultWeaponsClass thisClass = (Structs.ResultWeaponsClass)i;

                try
                {
                    int.Parse(thisClass.ToString());
                }
                catch(Exception)
                {
                    // thisClass is a string, which means it is a valid class
                    if (this.myInterface.GetCompetitorResultsExist(thisClass))
                    {
                        list.Add(thisClass);
                    }
                }
            }

            return list.ToArray();
        }

        internal Structs.ShootersClass[] ResultsGetUClasses(
            Structs.ResultWeaponsClass wclass)
        {
            Trace.WriteLine("CResult.ResultsGetUClasses(\"" + 
                wclass.ToString() + "\") " +
                "started on thread \"" +
                System.Threading.Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
            DateTime start = DateTime.Now;

            database = myInterface.databaseClass.Database;
            List<int> sClassesIntTable = new List<int>();
            List<Structs.ShootersClass> shooterClasses = new List<Structs.ShootersClass>();

            foreach(DatabaseDataset.CompetitorsRow row in
                database.Competitors)
            {
                Structs.Weapon weapon = myInterface.GetWeapon(row.WeaponId);
                if (myInterface.ConvertWeaponsClassToResultClass(weapon.WClass) == 
                    wclass)
                {
                    if (row.GetChildRows("CompetitorsCompetitorResults").Length > 0)
                    {
                        if (!sClassesIntTable.Contains(row.ShooterClass))
                        {
                            sClassesIntTable.Add(row.ShooterClass);
                            Structs.ShootersClass uclass =
                                (Structs.ShootersClass)row.ShooterClass;
                            shooterClasses.Add(uclass);
                        }
                    }
                }
            }

            if (myInterface.CompetitionCurrent.OneClass)
            {
                if (shooterClasses.Contains(Structs.ShootersClass.Klass1) ||
                    shooterClasses.Contains(Structs.ShootersClass.Klass2) ||
                    shooterClasses.Contains(Structs.ShootersClass.Klass3))
                {
                    shooterClasses.Add(Structs.ShootersClass.Klass);
                    shooterClasses.Remove(Structs.ShootersClass.Klass1);
                    shooterClasses.Remove(Structs.ShootersClass.Klass2);
                    shooterClasses.Remove(Structs.ShootersClass.Klass3);
                }

                if (shooterClasses.Contains(Structs.ShootersClass.Damklass1) ||
                    shooterClasses.Contains(Structs.ShootersClass.Damklass2) ||
                    shooterClasses.Contains(Structs.ShootersClass.Damklass3))
                {
                    shooterClasses.Add(Structs.ShootersClass.Damklass);
                    shooterClasses.Remove(Structs.ShootersClass.Damklass1);
                    shooterClasses.Remove(Structs.ShootersClass.Damklass2);
                    shooterClasses.Remove(Structs.ShootersClass.Damklass3);
                }
            }

            /*for(int i=0 ; i<=Structs.ShootersClassMax ; i++)
            {
                try
                {
                    if (sClassesIntTable.Contains(i))
                    {
                        int.Parse(((Structs.ShootersClass)i).ToString());
                    }
                }
                catch(System.FormatException)
                {
                    // Ok, we got a class
                    Structs.ShootersClass uclass = 
                        (Structs.ShootersClass)i;
                    shooterClasses.Add(uclass);
                }
            }*/

            shooterClasses.Sort();

            Trace.WriteLine("CResult.ResultsGetUClasses(\"" + 
                wclass.ToString() + "\") " +
                "ending after " + 
                (DateTime.Now-start).TotalMilliseconds.ToString() + 
                " ms.");

            return shooterClasses.ToArray();
        }

        internal Structs.ResultWeaponsClass[] GetResultWeaponClassesWithResults()
        {
            string select = "SELECT DISTINCT weapons.class " +
                "FROM (weapons RIGHT JOIN Competitors ON weapons.WeaponId = Competitors.WeaponId) " +
                "INNER JOIN CompetitorResults ON Competitors.CompetitorId = CompetitorResults.CompetitorId;";

            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(
                select, myInterface.databaseClass.Conn);
            
            if (myInterface.databaseClass.Conn.State != ConnectionState.Open)
                myInterface.databaseClass.Conn.Open();

            System.Data.OleDb.OleDbDataReader r = cmd.ExecuteReader();

            List<Structs.WeaponClass> wclasses = new List<Structs.WeaponClass>();

            while (r.Read())
            {
                wclasses.Add((Structs.WeaponClass)r.GetInt32(0));
            }

            r.Dispose();
            cmd.Dispose();

            List<Structs.ResultWeaponsClass> rwclasses = new List<Structs.ResultWeaponsClass>();

            foreach (Structs.WeaponClass wclass in wclasses)
            {
                // Check for duplicates
                Structs.ResultWeaponsClass rwc=
                    CConvert.ConvertWeaponsClassToResultClass(
                        wclass, 
                        myInterface.CompetitionCurrent.Type);
                if (!rwclasses.Contains(rwc))
                    rwclasses.Add(rwc);
            }

            // Sort
            rwclasses.Sort();

            return rwclasses.ToArray();
        }
    }
}
