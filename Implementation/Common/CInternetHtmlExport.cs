// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CInternetHtmlExport.cs" company="John Allberg">
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
//   Summary description for InternetExport to Html.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Web;

    using Allberg.Shooter.Common.DataSets;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for InternetExport to Html.
    /// </summary>
    [Serializable]
    public class CInternetHtmlExport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CInternetHtmlExport"/> class.
        /// </summary>
        /// <param name="callerInterface">
        /// The caller interface.
        /// </param>
        internal CInternetHtmlExport(Interface callerInterface)
        {
            myInterface = callerInterface;
            
            colPlaceWidth = 5;
            colNameWidth = 29;
            colClubWidth = 24;
            colResultWidth = 24;
            colPointsWidth = 9;
            colResultTotWidth = 9;
            colStdMedWidth = 6;
        }
        Interface myInterface;
        //DatabaseDataset database;
        // Columns
        private float colPlaceWidth;
        private float colNameWidth;
        private float colClubWidth;
        private float colResultWidth;
        //private float colResultMaxWidth;
        private float colPointsWidth;
        private float colResultTotWidth;
        private float colStdMedWidth;
        private bool useWidth = false;
        private int internalTableWidthcol1 = 70;

        private string[] colOrder = new string[] {
            "Place", 
            "Name", 
            "Club", 
            "Result", 
            "Points", 
            "ResultTot", 
            "StdMed",
            "PriceMoney"
        };

        #region Patrols
        internal string ExportPatrols()
        {
            //database = myInterface.databaseClass.database;
            StringBuilder html = new StringBuilder();
            html.Append(createHtmlHeader());
            html.Append(createCompetitionHeader());
            for(int patrolId=1;patrolId<=myInterface.GetPatrolsCount();patrolId++)
            {
                Structs.Patrol patrol = myInterface.GetPatrol(patrolId);

                html.Append(createPatrolHeader(patrol));
                html.Append(createPatrolHtmls(patrol));
            }
            html.Append(createHtmlFooter());

            return html.ToString();
        }

        private string createPatrolHeader(Structs.Patrol patrol)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<b>Patrull " + patrol.PatrolId + " - " + 
                patrol.StartDateTime.ToShortTimeString() + "</b>\r\n");
            return html.ToString();
        }
        private string createPatrolHtmls(Structs.Patrol patrol)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<table>\r\n");
            html.Append("<tr>");
            html.Append("<td>Figur</td>");
            html.Append("<td>Namn</td>");
            html.Append("<td>Klass</td>");
            html.Append("<td>Vapen</td>");
            html.Append("</tr>\r\n");
            Structs.Competitor[] comps = myInterface.GetCompetitors(patrol);

            for(int lane=1;lane<=myInterface.GetCompetitions()[0].PatrolSize;lane++)
            {
                bool found = false;

                foreach(Structs.Competitor comp in comps)
                {
                    if (comp.Lane == lane)
                    {
                        html.Append(createPatrolHtmlCompetitor(comp));
                        found = true;
                    }
                }

                if (!found)
                {
                    html.Append(createPatrolHtmlEmpty(lane));
                }
            }
            
            html.Append("</table>\r\n");
            return html.ToString();
        }
        private string createPatrolHtmlCompetitor(Structs.Competitor comp)
        {
            string tdclass = "";
            if (comp.Lane%2 == 0)
                tdclass = " class=\"resultevenline\" ";
            else
                tdclass = " class=\"resultoddline\" ";

            StringBuilder html = new StringBuilder();
            Structs.Shooter shooter = myInterface.GetShooter(comp.ShooterId);

            html.Append("<tr>\r\n");
            html.Append("<td " + tdclass + ">" + comp.Lane.ToString() + "</td>\r\n");
            html.Append("<td " + tdclass + ">" + shooter.Givenname + " " + shooter.Surname + "</td>\r\n");
            html.Append("<td " + tdclass + ">" + comp.ShooterClass.ToString() + "</td>\r\n");

            Structs.Weapon weapon = myInterface.GetWeapon(comp.WeaponId);
            html.Append("<td " + tdclass + ">" + weapon.Manufacturer + "</td>\r\n");
            html.Append("<td " + tdclass + ">" + weapon.Model + "</td>\r\n");
            html.Append("<td " + tdclass + ">" + weapon.Caliber + "</td>\r\n");
            html.Append("</tr>\r\n");
            return html.ToString();
        }
        private string createPatrolHtmlEmpty(int lane)
        {
            string tdclass = "";
            if (lane%2 == 0)
                tdclass = " class=\"resultevenline\" ";
            else
                tdclass = " class=\"resultoddline\" ";

            StringBuilder html = new StringBuilder();
            html.Append("<tr>\r\n");
            html.Append("<td" + tdclass + ">" + lane.ToString() + "</td>\r\n");
            html.Append("<td" + tdclass + ">&nbsp;</td>\r\n");
            html.Append("<td" + tdclass + ">&nbsp;</td>\r\n");
            html.Append("<td" + tdclass + ">&nbsp;</td>\r\n");
            html.Append("<td" + tdclass + ">&nbsp;</td>\r\n");
            html.Append("<td" + tdclass + ">&nbsp;</td>\r\n");
            html.Append("</tr>\r\n");
            return html.ToString();
        }
        #endregion

        #region PatrolsByClubs
        internal string ExportPatrolsByClub()
        {
            StringBuilder html = new StringBuilder();
            html.Append(createHtmlHeader());
            html.Append(createCompetitionHeader());
            DSInternetExport ds = getShootersForPatrolExportByClub();
            ds.Locale = new System.Globalization.CultureInfo("sv");

            string lastClubName = "";
            int clubPayed = 0;
            int line = 0;
            foreach(System.Data.DataRow rowTemp in ds.PatrolsByClub.Select("", "ClubName, ShooterName, PatrolId"))
            {
                DSInternetExport.PatrolsByClubRow row = (DSInternetExport.PatrolsByClubRow)rowTemp;
                if (lastClubName != row.ClubName)
                {
                    html.Append(createClubFooter(lastClubName, clubPayed));
                    html.Append(createClubHeader(row));
                    lastClubName = row.ClubName;
                    line = 0;
                }
                line++;
                html.Append("<tr>");
                html.Append("<td></td>");
                html.Append("<td" + getCssClassLine(line) + ">" + row.ShooterName + "</td>");
                html.Append("<td" + getCssClassLine(line) + ">" + row.ShooterClass + "</td>");
                html.Append("<td" + getCssClassLine(line) + ">" + row.PatrolId.ToString() + "</td>");
                html.Append("<td" + getCssClassLine(line) + ">" + row.Lane.ToString() + "</td>");
                html.Append("<td" + getCssClassLine(line) + ">" + row.Start.ToShortTimeString() + "</td>");
                html.Append("<td" + getCssClassLine(line) + ">" + row.Weapon + "</td>");
                html.Append("<td" + getCssClassLine(line) + ">" + row.WeaponCaliber + "</td>");
                html.Append("</tr>");
            }
            html.Append(createHtmlFooter());

            return html.ToString();
        }
        string getCssClassLine(int line)
        {
            if (line%2 == 0)
                return " class=\"resultevenline\" ";
            else
                return " class=\"resultoddline\" ";
        }
        DSInternetExport getShootersForPatrolExportByClub()
        {
            DSInternetExport toReturn = new DSInternetExport();
            Hashtable shooters = new Hashtable();
            Hashtable clubs = new Hashtable();
            Hashtable weapons = new Hashtable();
            foreach(Structs.Competitor comp in 
                myInterface.GetCompetitors())
            {
                if (comp.PatrolId != -1 &
                    comp.Lane != -1)
                {
                    DSInternetExport.PatrolsByClubRow newRow =
                        toReturn.PatrolsByClub.NewPatrolsByClubRow();

                    newRow.PatrolId = comp.PatrolId;
                    newRow.Lane = comp.Lane;

                    Structs.Shooter shooter;
                    if (shooters.Contains(comp.ShooterId))
                        shooter = (Structs.Shooter)shooters[comp.ShooterId];
                    else
                    {
                        shooter = myInterface.GetShooter(comp.ShooterId);
                        shooters.Add(comp.ShooterId, shooter);
                    }

                    newRow.ShooterName = shooter.Surname + " " + shooter.Givenname;
                    newRow.Payed = shooter.Payed;

                    Structs.Club club;
                    if (clubs.Contains(shooter.ClubId))
                        club = (Structs.Club)clubs[shooter.ClubId];
                    else
                    {
                        club = myInterface.GetClub(shooter.ClubId);
                        clubs.Add(shooter.ClubId, club);
                    }

                    newRow.ClubName = club.Name;
                    newRow.ClubId = shooter.ClubId;

                    Structs.Patrol patrol = myInterface.GetPatrol(comp.PatrolId);
                    newRow.Start = patrol.StartDateTimeDisplay;

                    Structs.Weapon weapon;
                    if (weapons.Contains(comp.WeaponId))
                        weapon = (Structs.Weapon)weapons[comp.WeaponId];
                    else
                    {
                        weapon = myInterface.GetWeapon(comp.WeaponId);
                        weapons.Add(comp.WeaponId, weapon);
                    }
                    newRow.Weapon = weapon.Manufacturer;
                    newRow.WeaponCaliber = weapon.Caliber;

                    Structs.ShootersClassShort shootershort = (Structs.ShootersClassShort)
                        (int)comp.ShooterClass;
                    newRow.ShooterClass = shootershort.ToString()
                        .Replace("Klass", "")
                        .Replace("Damklass", "D") +
                        weapon.WClass.ToString()
                        .Replace("1", "")
                        .Replace("2", "")
                        .Replace("3", "");

                    toReturn.PatrolsByClub.AddPatrolsByClubRow(newRow);
                }
            }
            return toReturn;
        }
        private string createClubHeader(DSInternetExport.PatrolsByClubRow row)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<!--- club header --->\r\n<table>\r\n");
            html.Append("<tr>");
            html.Append("<th>" + row.ClubId + "</th>");
            html.Append("<th>" + row.ClubName + "</th>");
            html.Append("<th>Klass</th>");
            html.Append("<th>Patr</th>");
            html.Append("<th>Fig</th>");
            html.Append("<th>Starttid</th>");
            html.Append("<th>Vapenfabrikat</th>");
            html.Append("<th>Kaliber</th>");
            html.Append("</tr>\r\n");
            return html.ToString();
        }
        private string createClubFooter(string lastClubName, int clubPayed)
        {
            StringBuilder html = new StringBuilder();
            if (lastClubName != "")
            {
                html.Append("<!--- club footer --->\r\n</table><p></p>\r\n");
            }
            return html.ToString();
        }
        #endregion

        #region Results
        internal string ExportResults(bool finalResults)
        {
            //database = myInterface.databaseClass.database;
            StringBuilder html = new StringBuilder();
            html.Length = 0;
            html.Append(createHtmlHeader());
            html.Append(createCompetitionHeader());

            html.Append("<h1>Individuella resultat</h1>");

            // Get results for the competitors
            for(int i=1;i<Structs.ResultWeaponsClassMax;i++)
            {
                Structs.ResultWeaponsClass wclass =
                    (Structs.ResultWeaponsClass)i;

                if (i.ToString() != wclass.ToString())
                {
                    // A real resultweaponsclass
                    for(int j=1;j<=Structs.ShootersClassMax;j++)
                    {
                        Structs.ShootersClass uclass =
                            (Structs.ShootersClass)j;

                        if( j.ToString() != uclass.ToString() )
                        {
                            // TODO fixa en bättre räknare
                            if (myInterface.GetCompetitorResultsExist(wclass, uclass))
                                html.Append(createCompetitorResults(wclass, 
                                    uclass, finalResults));
                        }
                    }
                }
            }

            // Get results for the teams
            for(int i=1;i<Structs.ResultWeaponsClassMax;i++)
            {
                Structs.ResultWeaponsClass wclass =
                    (Structs.ResultWeaponsClass)i;
                if (i.ToString() != wclass.ToString())
                {
                    // A real resultweaponsclass
                    ResultsReturnTeam[] teamresults=
                        myInterface.resultTeamClass.GetTeamResults(wclass, myInterface.GetCompetitions()[0]);
                    if (teamresults.Length > 0)
                    {
                        html.Append(createTeamResults(teamresults, wclass));
                    }
                }
            }
            html.Append(createHtmlFooter());
            return html.ToString();
        }

        internal string ExportResults(Structs.ResultWeaponsClass wclass, 
            Structs.ShootersClass uclass, bool finalResults)
        {
            StringBuilder html = new StringBuilder();
            html.Append(createHtmlHeader());
            html.Append(createCompetitionHeader());
            html.Append(createCompetitorResults(wclass, uclass, finalResults));
            html.Append(createHtmlFooter());
            return html.ToString();
        }

        private string createHtmlHeader()
        {
            string html = "<html>" +
                "<head>" +
                "<meta content=\"Allberg WinShooter\" name=\"GENERATOR\">" +
                "<title>WinShooter: " + 
                    HttpUtility.HtmlEncode(myInterface.GetCompetitions()[0].Name) + 
                    "</title>" +
                createHtmlCssScreen() +
                createHtmlCssPrinter() +
                "</head>" +
                "<body>";

            return html;
        }
        private string createHtmlCssScreen()
        {
            string html = "";
            html += "<STYLE TYPE=\"text/css\" MEDIA=screen>	\r\n";
            html += "<!--									\r\n";
            html += "body									\r\n";
            html += "{										\r\n";
            html += "	font-family: Arial;					\r\n";
            html += "}										\r\n";
            html += "td.resultevenline						\r\n";
            html += "{										\r\n";
            html += "	font-family: Arial;					\r\n";
            html += "	background-color: lightskyblue;		\r\n";
            html += "}										\r\n";
            html += "td.resultoddline						\r\n";
            html += "{										\r\n";
            html += "	font-family: Arial;					\r\n";
            html += "}										\r\n";
            html += "-->									\r\n";
            html += "</STYLE>								\r\n";

            return html;
        }

        private string createHtmlCssPrinter()
        {
            string html = "";
            html += "<STYLE TYPE=\"text/css\" MEDIA=print>	\r\n";
            html += "<!--									\r\n";
            html += "body									\r\n";
            html += "{										\r\n";
            html += "	font-family: Arial;					\r\n";
            html += "}										\r\n";
            html += "td.resultevenline						\r\n";
            html += "{										\r\n";
            html += "	font-family: Arial;					\r\n";
            html += "	border-bottom: black 1px solid;		\r\n";
            html += "}										\r\n";
            html += "td.resultoddline						\r\n";
            html += "{										\r\n";
            html += "	font-family: Arial;					\r\n";
            html += "	border-bottom: black 1px solid;		\r\n";
            html += "}										\r\n";
            html += "-->									\r\n";
            html += "</STYLE>								\r\n";

            return html;
        }

        private string createHtmlFooter()
        {
            string html = "</BODY>" +
                "</HTML>";

            return html;
        }

        private string createCompetitionHeader()
        {
            StringBuilder html = new StringBuilder();


            html.Append("<table width=100%>");
            html.Append("<tr valign=top><td>");

            html.Append("<table>");
            html.Append("<tr>");
            html.Append("<td width=" + 
                internalTableWidthcol1.ToString() + "><b>" + 
                HttpUtility.HtmlEncode("Tävling:") + "</b></td>");
            html.Append("<td>" + 
                HttpUtility.HtmlEncode(myInterface.GetCompetitions()[0].Name) + 
                "</td>");
            html.Append("</tr>");
            html.Append("</table>");

            html.Append("</td>");
            html.Append("<td align=Right><a href=\"http://www.allberg.se/WinShooter\"><img src=\"http://www.allberg.se/WinShooter/img/logga.jpg\" width=\"193px\" height=\"113\" border=0></a></td>");
            html.Append("</tr></table>");

            return html.ToString();
        }
        private string createCompetitorResults(Structs.ResultWeaponsClass wclass, 
            Structs.ShootersClass uclass, bool finalResults)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<p><table>");
            html.Append("<tr>");
            html.Append("<td width=" + internalTableWidthcol1.ToString() + 
                "><b>Vapengrupp:</b></td>");
            html.Append("<td>" + wclass.ToString() + "</td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td width=" + internalTableWidthcol1.ToString() + 
                "><b>Klass:</b></td>");
            html.Append("<td>" + uclass.ToString() + "</td>");
            html.Append("</tr>");

            html.Append("</table>");
            if (this.useWidth)
                html.Append("<table width=100% border=0 cellspacing=5 cellpadding=0>");
            else
                html.Append("<table border=0 cellspacing=0>");
            foreach(string columnName in this.colOrder)
            {
                html.Append("<th");
                if (useWidth)
                    html.Append(" width=");
                switch (columnName)
                {
                    case "Place":
                        if (useWidth)
                            html.Append(this.colPlaceWidth.ToString() + "%");
                        html.Append(" align=center>Pl");
                        break;
                    case "Name":
                        if (useWidth)
                            html.Append(this.colNameWidth.ToString() + "%");
                        html.Append(" align=left>Namn");
                        break;
                    case "Club":
                        if (useWidth)
                            html.Append(this.colClubWidth.ToString() + "%");
                        html.Append(" align=left>Klubb");
                        break;
                    case "Result":
                        if (useWidth)
                            html.Append(this.colResultWidth.ToString() + "%");
                        html.Append(" align=left colSpan=" + 
                            (3 + myInterface.GetStationsCount()).ToString() +
                            ">Result");
                        break;
                    case "ResultTot":
                        if (useWidth)
                            html.Append(this.colResultTotWidth.ToString() + "%");
                        html.Append(" align=center>Tot");
                        break;
                    case "Points":
                        if (myInterface.GetCompetitions()[0].Type == Structs.CompetitionTypeEnum.Field)
                        {
                            if (useWidth)
                                html.Append(this.colPointsWidth.ToString() + "%");
                            html.Append(" align=center>" + HttpUtility.HtmlEncode("Poäng"));
                        }
                        else
                        {
                            html.Append(" align=center>");
                        }
                        break;
                    case "StdMed":
                        if (useWidth)
                            html.Append(this.colStdMedWidth.ToString() + "%");
                        html.Append(" align=center>Stm");
                        break;
                    case "PriceMoney":
                        if (finalResults)
                        {
                            html.Append(" align=center>Pris");
                        }
                        else
                        {
                            html.Append(">");
                        }
                        break;

                }
                html.Append("</th>");
            }

            ResultsReturn[] results =
                myInterface.resultClass.GetResults(
                wclass, 
                uclass,
                myInterface.GetCompetitions()[0], finalResults);

            int place = 0;
            foreach(ResultsReturn result in results)
            {
                place++;
                html.Append("<tr>");
                foreach(string columnName in this.colOrder)
                {
                    html.Append("<td");
                    if (place%2 == 0)
                        html.Append(" class=\"resultevenline\" ");
                    else
                        html.Append(" class=\"resultoddline\" ");

                    if (useWidth)
                        html.Append(" width=");
                    switch (columnName)
                    {
                        case "Place":
                            if (useWidth)
                                html.Append(this.colPlaceWidth.ToString() + "%");
                            html.Append(" align=center>" + place.ToString());
                            break;
                        case "Name":
                            if (useWidth)
                                html.Append(this.colNameWidth.ToString() + "%");
                            html.Append(" align=left>" + 
                                HttpUtility.HtmlEncode(result.ShooterName));
                            break;
                        case "Club":
                            if (useWidth)
                                html.Append(this.colClubWidth.ToString() + "%");
                            html.Append(" align=left>" +  
                                HttpUtility.HtmlEncode(
                                myInterface.GetClub(result.ClubId).Name));
                            break;
                        case "Result":
                        {
                            if (useWidth)
                                html.Append(this.colResultWidth.ToString() + "%");
                            
                            switch(myInterface.CompetitionCurrent.Type)
                            {
                                case Structs.CompetitionTypeEnum.Field:
                                {
                                    if (myInterface.CompetitionCurrent.NorwegianCount)
                                    {
                                        html.Append(" align=left></td>");
                                        foreach(string str in result.HitsPerStnString.Split(';'))
                                        {
                                            if (str.Length>0)
                                            {
                                                html.Append("<td");
                                                if (place%2 == 0)
                                                    html.Append(" class=\"resultevenline\"");
                                                else
                                                    html.Append(" class=\"resultoddline\"");
    
                                                html.Append(" align=right>" +
                                                    str.ToString() + "</td>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        html.Append(" align=left></td>");
                                        foreach(string str in result.HitsPerStnString.Split(';'))
                                        {
                                            html.Append("<td");
                                            if (place%2 == 0)
                                                html.Append(" class=\"resultevenline\"");
                                            else
                                                html.Append(" class=\"resultoddline\"");
                                            html.Append(" align=right>" + str + "</td>");
                                        }
                                    }
                                    break;
                                }
                                case Structs.CompetitionTypeEnum.Precision:
                                {
                                    html.Append(" align=left></td>");
                                    foreach(string str in result.HitsPerStnString.Split(';'))
                                    {
                                        html.Append("<td");
                                        if (place%2 == 0)
                                            html.Append(" class=\"resultevenline\"");
                                        else
                                            html.Append(" class=\"resultoddline\"");
                                        html.Append(" align=right>" + str + "</td>");
                                    }
                                    break;
                                }
                            }
                            html.Append("<td");
                            if (place%2 == 0)
                                html.Append(" class=\"resultevenline\"");
                            else
                                html.Append(" class=\"resultoddline\"");

                            html.Append(" align=right>");
                            if (result.FinalShootingPlace < 100 &
                                result.FinalShootingPlace > 0)
                            {
                                html.Append("( " + result.FinalShootingPlace.ToString() +
                                    " )");
                            }
                            html.Append("</td>");

                            break;
                        }
                        case "Points":
                            if (myInterface.GetCompetitions()[0].Type == Structs.CompetitionTypeEnum.Field)
                            {
                                if (useWidth)
                                    html.Append(this.colPointsWidth.ToString() + "%");
                                html.Append(" align=center>" + result.PointsTotal);
                            }
                            else
                            {
                                html.Append(" align=center>");
                            }
                            break;
                        case "ResultTot":
                        {
                            if (useWidth)
                                html.Append(this.colResultTotWidth.ToString() + "%");
                            switch(myInterface.GetCompetitions()[0].Type)
                            {
                                case Structs.CompetitionTypeEnum.Field:
                                {
                                    if (myInterface.GetCompetitions()[0].NorwegianCount)
                                    {
                                        html.Append(" align=center>" + 
                                            (result.HitsTotal + result.FigureHitsTotal).ToString());
                                    }
                                    else
                                    {
                                        html.Append(" align=center>" + 
                                            result.HitsTotal.ToString() + "/" + 
                                            result.FigureHitsTotal.ToString());
                                    }
                                    break;
                                }
                                case Structs.CompetitionTypeEnum.Precision:
                                {
                                    html.Append(" align=center>" + 
                                        result.HitsTotal.ToString());
                                    break;
                                }
                            }
                            break;
                        }
                        case "StdMed":
                        {
                            if (useWidth)
                                html.Append(this.colStdMedWidth.ToString() + "%");
                            html.Append(" align=center>");
                            switch(result.Medal)
                            {
                                case Structs.Medal.StandardSilver:
                                    html.Append("S");
                                    break;
                                case Structs.Medal.StardardBrons:
                                    html.Append("B");
                                    break;
                            }
                            break;
                        }
                        case "PriceMoney":
                            if (finalResults)
                            {
                                html.Append(" align=right>");
                                if (result.PriceMoney > 0)
                                    html.Append(result.PriceMoney.ToString() + ":-");
                            }
                            else
                            {
                                html.Append(">");
                            }
                            break;
                    }
                    if (columnName != "Result")
                        html.Append("</td>");
                }
                html.Append("</tr>");
            }
            html.Append("</table></p>");
            return html.ToString();
        }

        private string createTeamResults(ResultsReturnTeam[] results, 
            Structs.ResultWeaponsClass wclass)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<h1>Lagtävling</h1>");
            html.Append("Vapengrupp " + wclass.ToString() + ":");
            html.Append("<table>\r\n");
            html.Append("<tr>" +
                "<td><b>Pl</b></td>" +
                "<td><b>Klubb</b></td>" +
                "<td><b>Lag</b></td>" +
                "<td><b>Resultat</b></td>" + 
                "</tr>\r\n");

            int place = 0;
            foreach(ResultsReturnTeam result in results)
            {
                place++;
                html.Append("<tr>");
                html.Append("<td class=\"resultevenline\">" + place.ToString() + "</td>");
                html.Append("<td class=\"resultevenline\">" + myInterface.GetClub( result.ClubId ).Name + "</td>");
                html.Append("<td class=\"resultevenline\">" + result.TeamName + "</td>");
                switch(myInterface.GetCompetitions()[0].Type)
                {
                    case Structs.CompetitionTypeEnum.Field:
                    {
                        if (myInterface.GetCompetitions()[0].NorwegianCount)
                            html.Append("<td class=\"resultevenline\">" + (result.Hits + result.FigureHits).ToString() + "</td>");
                        else
                            html.Append("<td class=\"resultevenline\">" + result.Hits.ToString() + "/" + result.FigureHits.ToString() + "</td>");
                        break;
                    }
                    case Structs.CompetitionTypeEnum.Precision:
                    {
                        html.Append("<td class=\"resultevenline\">" + result.Hits.ToString() + "</td>");
                        break;
                    }
                }
                html.Append("</tr>\r\n");

                // The total results
                html.Append("<tr><td colspan=2></td>");
                html.Append("<td colspan=2>" + result.HitsPerStn.Replace(";", " ") + "</td>");
                html.Append("</tr>\r\n");

                Structs.Team team = this.myInterface.GetTeam( result.TeamId );

                foreach (var compId in team.CompetitorIds.ToArray())
                {
                    Structs.Competitor comp = this.myInterface.GetCompetitor(compId);
                    html.Append("<tr><td colspan=2></td>");
                    html.Append("<td>" + this.GetNameForCompetitor(comp) + "</td>");
                    html.Append("<td>" + this.GetResultForCompetitor(comp) + "</td>");
                    html.Append("</tr>\r\n");
                }
            }
            html.Append("</table>");
            return html.ToString();
        }

        private string GetNameForCompetitor(Structs.Competitor comp)
        {
            Structs.Shooter shooter = this.myInterface.GetShooter(comp.ShooterId);
            return shooter.Surname + " " + shooter.Givenname;
        }
        private string GetResultForCompetitor(Structs.Competitor comp)
        {
            Structs.CompetitorResult[] results = myInterface.GetCompetitorResults(comp.CompetitorId);

            int hits = 0;
            int figurehits = 0;
            foreach (Structs.CompetitorResult result in results)
            {
                hits += result.Hits;
                figurehits += result.FigureHits;
            }

            switch(myInterface.GetCompetitions()[0].Type)
            {
                case Structs.CompetitionTypeEnum.Field:
                {
                    if (myInterface.GetCompetitions()[0].NorwegianCount)
                        return (hits + figurehits).ToString();
                    else
                        return hits.ToString() + "/" + figurehits.ToString();
                }
                case Structs.CompetitionTypeEnum.Precision:
                {
                    return hits.ToString();
                }
                default:
                    throw new ApplicationException("Unknown competition type: " + 
                        myInterface.GetCompetitions()[0].Type.ToString());
            }
        }
        #endregion


    }
}
