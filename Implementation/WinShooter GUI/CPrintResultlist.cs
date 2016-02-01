namespace Allberg.Shooter.Windows
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Printing;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for CPrintPatrollist.
    /// </summary>
    public class CPrintResultlist : PrintDocument 
    {
        private Font printHeaderFont           = null;
        private Font printCompetitorHeaderFont = null;
        private Font printCompetitorFont       = null;
        private Font printAllbergFont          = null;
        private Common.Interface CommonCode;
        private int competitorsDone = -1;
        private ResultsReturn[] results;
        private bool individualsPrintDone = false;
        int currentUClass = 0;
        Structs.ResultWeaponsClass wclass;
        Structs.ShootersClass uclass;
        string clubId;

        // Margins
        private float LeftMargin = 50;
        private float RightMargin = 0;
        // Columns
        private float colIndividPlace;
        private float colIndividName;
        private float colIndividClub;
        private float colIndividResult;
        private float colIndividResultMaxWidth;
        private float colIndividPoints;
        private float colIndividResultTot;
        private float colIndividStdMed;
        private float colIndividPrice;
        bool printPrelResults = false;

        private float colTeamPlace;
        private float colTeamName;
        private float colTeamClub;
        private float colTeamResult;
        private float colTeamResultMaxWidth;
        private float colTeamPoints;
        private float colTeamResultTot;

        private Structs.Competition competition;
        private Structs.CompetitionTypeEnum CompetitionType;

        internal CPrintResultlist(ref Common.Interface newCommon, 
            Structs.ResultWeaponsClass wclasswanted,
            Structs.ShootersClass uclasswanted,
            bool prelResults,
            string clubIdwanted) : base ()  
        {
            CommonCode = newCommon;
            wclass = wclasswanted;
            uclass = uclasswanted;
            printPrelResults = prelResults;
            clubId = clubIdwanted;

            competition = CommonCode.GetCompetitions()[0];
            CompetitionType = competition.Type;
        }


        #region Init
        //Override OnBeginPrint to set up the font we are going to use
        protected override void OnBeginPrint(PrintEventArgs ev) 
        {
            base.OnBeginPrint(ev) ;
            printHeaderFont = new Font("Arial", 16,System.Drawing.FontStyle.Bold);

            PrintDocumentStd settings = CommonCode.Settings.PrinterSettings.PaperResultDocument;
            const int nrOfExpectedFonts = 3;

            if (settings.Fonts.Count < nrOfExpectedFonts)
                throw new ApplicationException("Not enough fonts in PaperResultDocument. It is " +
                    settings.Fonts.Count.ToString() + " and should be " + nrOfExpectedFonts.ToString());

            printCompetitorHeaderFont = settings.Fonts[0].PrintFont;
            printCompetitorFont = settings.Fonts[1].PrintFont;
            printAllbergFont = settings.Fonts[2].PrintFont;

            if (uclass != Structs.ShootersClass.Okänd)
            {
                currentUClass = (int)uclass;
                getCurrentResults();
            }
            else
            {
                currentUClass = 0;
                moreClassesPages();
            }
        }

        private void getCurrentResults()
        {
            Structs.ShootersClass temp = 
                (Structs.ShootersClass)this.currentUClass;

            Structs.Competition comp = CommonCode.GetCompetitions()[0];

            results =
                CommonCode.ResultsGet(wclass, temp, 
                comp,
                !printPrelResults);
        }
        //Override the OnPrintPage to provide the printing logic for the document
        protected override void OnPrintPage(PrintPageEventArgs ev) 
        {
            PrintDocumentStd printSettings = CommonCode.Settings.PrinterSettings.PaperResultDocument;
            printSettings.DocumentSizeXPixels = ev.PageBounds.Width;
            printSettings.DocumentSizeYPixels = ev.PageBounds.Height;

            this.RightMargin = ev.PageBounds.Right - 50;
            this.LeftMargin = 50;
            base.OnPrintPage(ev) ;

            float topMargin = ev.PageBounds.Top + 45; // Org = 25
            float yPos = topMargin;
            float width = this.RightMargin - this.LeftMargin;
            colIndividPlace = LeftMargin;
            colIndividName = colIndividPlace + printSettings.Columns[0].SizeDpi;
            colIndividClub = colIndividName + printSettings.Columns[1].SizeDpi;
            colIndividResult = colIndividClub + printSettings.Columns[2].SizeDpi;
            colIndividResultTot = colIndividResult + printSettings.Columns[3].SizeDpi;
            colIndividPoints = colIndividResultTot + printSettings.Columns[4].SizeDpi;
            colIndividStdMed = colIndividPoints + printSettings.Columns[5].SizeDpi;
            colIndividPrice = colIndividStdMed + printSettings.Columns[6].SizeDpi;

            colIndividResultMaxWidth = colIndividResultTot;

            PrintDocumentStd printTeamSettings = CommonCode.Settings.PrinterSettings.PaperResultTeamDocument;
            printTeamSettings.DocumentSizeXPixels = ev.PageBounds.Width;
            printTeamSettings.DocumentSizeYPixels = ev.PageBounds.Height;

            colTeamPlace = LeftMargin;
            colTeamClub = colTeamPlace + printTeamSettings.Columns[0].SizeDpi;
            colTeamName = colTeamClub + printTeamSettings.Columns[1].SizeDpi;
            colTeamResult = colTeamName + printTeamSettings.Columns[2].SizeDpi;
            colTeamResultTot = colTeamResult + printTeamSettings.Columns[3].SizeDpi;
            colTeamPoints = colTeamResultTot + printTeamSettings.Columns[4].SizeDpi;

            colTeamResultMaxWidth = colTeamResultTot;

            int tab = 140;

            if (individualsPrintDone == false)
            {
                printIndividual(ev, ref yPos, tab);

                if (ev.HasMorePages == false)
                {
                    individualsPrintDone = true;
                    if (CommonCode.ResultsGetTeams(this.wclass, this.competition).Length> 0)
                        ev.HasMorePages = true;
                }
            }
            else
            {
                // Print teams.
                ev.HasMorePages = printTeamResults(ev, ref yPos, tab);
            }
        }

        private void printIndividual(PrintPageEventArgs ev, ref float yPos, int tab) 
        {
            //Work out the number of lines per page 
            //Use the MarginBounds on the event to do this 
            //Now print what we want to print
            // Check if there is any results retreived
            if (results == null)
            {
                //ev.Cancel = true;
                ev.HasMorePages = false;
                return;
            }

            bool morePages = printResult(ev, ref yPos, tab);

            //If we have more patrols then print another page
            if (morePages)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
        #endregion

        #region MultipleClasses
        private bool moreClassesPages()
        {
            if ( uclass != Structs.ShootersClass.Okänd )
                return false;

            bool nextClass = false;

            // We're talking multiple classes here...
            for( int i=0;i<=Structs.ShootersClassMax; i++)
            {
                Structs.ShootersClass thisuclass = 
                    (Structs.ShootersClass)i;
                try
                {
                    int.Parse(thisuclass.ToString());
                }
                catch(Exception)
                {
                    // A class, since we couldn't parse int
                    if (nextClass)
                    {
                        if (CommonCode.GetCompetitorResultsCount(
                            wclass, thisuclass) > 0 )
                        {
                            competitorsDone = -1;
                            this.currentUClass = (int)thisuclass;
                            getCurrentResults();
                            return true;
                        }
                    }

                    // Check if next class should be checked
                    if ( (int)thisuclass == this.currentUClass )
                        nextClass = true;
                }
            }
            return false;
        }
        #endregion

        #region Header
        private void printHeader(PrintPageEventArgs ev, ref float yPos, int tab)
        {
            float leftMargin = 50;
            
            // Print watermark "Preliminär"
            if (printPrelResults)
            {
                ev.Graphics.RotateTransform(45);

                Font prelFont = new Font("Arial", 90, 
                    System.Drawing.FontStyle.Regular);

                ev.Graphics.DrawString("Preliminärresultat", 
                    prelFont, 
                    Brushes.LightGray, 
                    ev.MarginBounds.Left,
                    0,
                    new StringFormat());

                ev.Graphics.RotateTransform(-45);
            }

            // Print logo
            System.Drawing.Image image = getLogo();

            ev.Graphics.DrawImage( 
                image, 
                ev.MarginBounds.Right-image.Width/4, 
                20, 
                image.Width/4, 
                image.Height/4);

            // Print page header
            ev.Graphics.DrawString("Tävling: ", 
                printHeaderFont, Brushes.Black, leftMargin,	yPos, 
                new StringFormat());
            ev.Graphics.DrawString(CommonCode.GetCompetitions()[0].Name, 
                printHeaderFont, Brushes.Black, leftMargin+tab, yPos, 
                new StringFormat());
            yPos += printHeaderFont.GetHeight();

            ev.Graphics.DrawString("Vapengrupp: ", 
                printHeaderFont, Brushes.Black, leftMargin, yPos, 
                new StringFormat());
            ev.Graphics.DrawString(wclass.ToString(), 
                printHeaderFont, Brushes.Black, leftMargin+tab, yPos, 
                new StringFormat());
            yPos += printHeaderFont.GetHeight();

            if (this.clubId != null)
            {
                Structs.Club club = CommonCode.GetClub(clubId);
                ev.Graphics.DrawString("Skytteklubb: ", 
                    printHeaderFont, Brushes.Black, leftMargin, yPos, 
                    new StringFormat());

                ev.Graphics.DrawString(club.Name, 
                    printHeaderFont, Brushes.Black, leftMargin+tab, yPos, 
                    new StringFormat());
                yPos += printHeaderFont.GetHeight();
            }
            //

            // Print allberg on bottom
            ev.Graphics.DrawString("Utskriven " + DateTime.Now.ToShortDateString() + " " +
                    DateTime.Now.ToShortTimeString(),
                    printAllbergFont, Brushes.Black, ev.PageBounds.Right - 250,
                    ev.PageBounds.Size.Height - 2 * printHeaderFont.GetHeight() - 20,
                    new StringFormat());
        }

        private void printHeaderIndividual(PrintPageEventArgs ev, ref float yPos, int tab)
        {
            ev.Graphics.DrawString("Skytteklass: ", 
                printHeaderFont, Brushes.Black, LeftMargin, yPos, 
                new StringFormat());
            if (uclass == Structs.ShootersClass.Okänd)
            {
                ev.Graphics.DrawString(((Structs.ShootersClass)currentUClass).ToString(), 
                    printHeaderFont, Brushes.Black, LeftMargin+tab, yPos, 
                    new StringFormat());
            }
            else
            {
                ev.Graphics.DrawString(uclass.ToString(), 
                    printHeaderFont, Brushes.Black, LeftMargin+tab, yPos, 
                    new StringFormat());
            }
            yPos += 2*printHeaderFont.GetHeight();

            // Print result header
            ev.Graphics.DrawString("Pl",
                printCompetitorHeaderFont, Brushes.Black, this.colIndividPlace, yPos,
                new StringFormat());
            ev.Graphics.DrawString("Namn",
                printCompetitorHeaderFont, Brushes.Black, this.colIndividName, yPos,
                new StringFormat());
            ev.Graphics.DrawString("Klubb",
                printCompetitorHeaderFont, Brushes.Black, this.colIndividClub, yPos,
                new StringFormat());
            ev.Graphics.DrawString("Resultat",
                printCompetitorHeaderFont, Brushes.Black, this.colIndividResult, yPos,
                new StringFormat());
            if (CompetitionType == Structs.CompetitionTypeEnum.Field |
                CompetitionType == Structs.CompetitionTypeEnum.MagnumField)
            {
                ev.Graphics.DrawString("P",
                    printCompetitorHeaderFont, Brushes.Black, this.colIndividPoints, yPos,
                    new StringFormat());
            }
            ev.Graphics.DrawString("Tot",
                printCompetitorHeaderFont, Brushes.Black, this.colIndividResultTot, yPos,
                new StringFormat());
            ev.Graphics.DrawString("Stm",
                printCompetitorHeaderFont, Brushes.Black, this.colIndividStdMed, yPos,
                new StringFormat());
            this.colIndividStdMed += ev.Graphics.MeasureString("Stm", printCompetitorHeaderFont).Width/2;
            if (!printPrelResults & CommonCode.GetCompetitions()[0].UsePriceMoney)
            {
                ev.Graphics.DrawString("Pris",
                    printCompetitorHeaderFont, Brushes.Black, this.colIndividPrice, yPos,
                    new StringFormat());
            }
            yPos += printCompetitorHeaderFont.GetHeight();

            // Underline
            ev.Graphics.DrawLine(new System.Drawing.Pen(Brushes.Black, 2), 
                new System.Drawing.PointF(this.LeftMargin, yPos), 
                new System.Drawing.PointF(this.RightMargin, yPos));
            yPos += 2;
        }

        private System.Drawing.Image getLogo()
        {
            return CommonCode.Settings.GetResizedLogo(400, 1200);
        }

        #endregion

        #region Result individuals
        private bool printResult(PrintPageEventArgs ev, ref float yPos, int tab)
        {
            int nrOfLinesThisResult = 1;
            bool printThisCompetitor = true;
            printHeader(ev, ref yPos, tab);
            printHeaderIndividual(ev, ref yPos, tab);

            competitorsDone++;
            int place = competitorsDone;
            for(int i=competitorsDone;i<=results.GetUpperBound(0);i++)
            {
                if (this.clubId != null)
                {
                    if (this.clubId != results[i].ClubId)
                        printThisCompetitor = false;
                    else
                        printThisCompetitor = true;
                }
                if (printThisCompetitor)
                {
                    place++;
                    nrOfLinesThisResult = 1;
                    // If this competitor would print outsite margins, return
                    // and wait for next page.
                    if (yPos+2*printCompetitorFont.GetHeight() > ev.MarginBounds.Bottom)
                        return true;

                    // Print this competitor
                    ev.Graphics.DrawString((place).ToString(),
                        printCompetitorFont, Brushes.Black, this.colIndividPlace, yPos,
                        new StringFormat());
                    //ev.Graphics.DrawString(results[i].ShooterName,
                    //	printCompetitorFont, Brushes.Black, this.colIndividName, yPos,
                    //	new StringFormat());
                    printString(ev, results[i].ShooterName,
                        printCompetitorFont, this.colIndividName, yPos, colIndividResult - colIndividClub);
                    //ev.Graphics.DrawString(CommonCode.GetClub(results[i].ClubId).Name,
                    //	printCompetitorFont, Brushes.Black, this.colIndividClub, yPos,
                    //	new StringFormat());
                    printString(ev, CommonCode.GetClub(results[i].ClubId).Name,
                        printCompetitorFont, this.colIndividClub, yPos, colIndividResult-colIndividClub);

                    float xPosHitsPerStn = this.colIndividResult;
                    foreach(string strnTemp in results[i].HitsPerStnString.Split(';'))
                    {
                        string strn = strnTemp;
                        if (xPosHitsPerStn + 
                            ev.Graphics.MeasureString(strn, printCompetitorFont).Width >
                            colIndividResultMaxWidth)
                        {
                            nrOfLinesThisResult++;
                            xPosHitsPerStn = this.colIndividResult;
                        }
                        if (strn != "")
                        {
                            switch (CompetitionType)
                            {
                                case Structs.CompetitionTypeEnum.Field:
                                    if (!competition.NorwegianCount)
                                    {
                                        string[] parts = strn.Split('/');
                                        int hits = int.Parse(parts[0]);
                                        int figures = int.Parse(parts[1]);
                                        strn = hits.ToString();
                                    }
                                    else
                                    {
                                        /*string[] parts = strn.Split('/');
                                        int hits = int.Parse(parts[0]);
                                        int figures = int.Parse(parts[1]);
                                        strn = (hits + figures).ToString();*/
                                    }
                                    break;
                                case Structs.CompetitionTypeEnum.MagnumField:
                                    if (!competition.NorwegianCount)
                                    {
                                        string[] parts = strn.Split('/');
                                        int hits = int.Parse(parts[0]);
                                        int figures = int.Parse(parts[1]);
                                        strn = hits.ToString();
                                    }
                                    else
                                    {
                                        string[] parts = strn.Split('/');
                                        int hits = int.Parse(parts[0]);
                                        int figures = int.Parse(parts[1]);
                                        strn = (hits + figures).ToString();
                                    }
                                    break;
                                case Structs.CompetitionTypeEnum.Precision:
                                    break;
                            }
                        }
                        ev.Graphics.DrawString(strn,
                            printCompetitorFont, Brushes.Black, xPosHitsPerStn, 
                            yPos + (nrOfLinesThisResult-1) * 
                            printCompetitorHeaderFont.GetHeight(),
                            new StringFormat());
                        xPosHitsPerStn += ev.Graphics.MeasureString(strn + "  ", 
                            printCompetitorFont).Width;
                    }
                    if (results[i].FinalShootingPlace < 100 &
                        results[i].FinalShootingPlace > 0)
                    {
                        string strn ="(" + results[i].FinalShootingPlace.ToString() +")";
                        if (xPosHitsPerStn + 
                            ev.Graphics.MeasureString(strn, printCompetitorFont).Width >
                            colIndividResultMaxWidth)
                        {
                            nrOfLinesThisResult++;
                            xPosHitsPerStn = this.colIndividResult;
                        }
                        ev.Graphics.DrawString(strn,
                            printCompetitorFont, Brushes.Black, xPosHitsPerStn, 
                            yPos + (nrOfLinesThisResult-1) * 
                            printCompetitorHeaderFont.GetHeight(),
                            new StringFormat());
                    }

                    string resultString = "";
                    switch(CompetitionType)
                    {
                        case Structs.CompetitionTypeEnum.Field:
                        {
                            if (competition.NorwegianCount)
                                resultString = (results[i].HitsTotal + results[i].FigureHitsTotal).ToString();
                            else
                                resultString = results[i].HitsTotal + "/" + results[i].FigureHitsTotal;

                            ev.Graphics.DrawString(results[i].PointsTotal.ToString(),
                                printCompetitorFont, Brushes.Black, this.colIndividPoints, yPos,
                                new StringFormat());
                            break;
                        }
                        case Structs.CompetitionTypeEnum.MagnumField:
                        {
                            resultString = results[i].HitsTotal + "/" + results[i].FigureHitsTotal;

                            ev.Graphics.DrawString(results[i].PointsTotal.ToString(),
                                printCompetitorFont, Brushes.Black, this.colIndividPoints, yPos,
                                new StringFormat());
                            break;
                        }
                        case Structs.CompetitionTypeEnum.Precision:
                        {
                            resultString = results[i].HitsTotal.ToString();
                            break;
                        }
                        default:
                            throw new ApplicationException("Unknown CompetitionType");
                    }
                    ev.Graphics.DrawString(resultString,
                        printCompetitorFont, Brushes.Black, this.colIndividResultTot, yPos,
                        new StringFormat());

                    string medalToPrint = "";
                    if (results[i].Medal == Structs.Medal.StandardSilver)
                        medalToPrint = "S";
                    else
                        if (results[i].Medal == Structs.Medal.StardardBrons)
                        medalToPrint = "B";
                    else medalToPrint = "";

                    ev.Graphics.DrawString(medalToPrint,
                        printCompetitorFont, Brushes.Black, this.colIndividStdMed, 
                        yPos,
                        new StringFormat());

                    if (!printPrelResults & CommonCode.GetCompetitions()[0].UsePriceMoney &
                        results[i].PriceMoney > 0)
                    {
                        ev.Graphics.DrawString(results[i].PriceMoney.ToString() + ":-",
                            printCompetitorFont, Brushes.Black, this.colIndividPrice, yPos,
                            new StringFormat());
                    }

                    // Ok, done printing. Draw line beneath the shooter.
                    ev.Graphics.DrawLine(new Pen(Brushes.Black, 1), 
                        new PointF(this.LeftMargin, yPos-1), 
                        new PointF(this.RightMargin, yPos-1));

                    // Prepare for the next shooter
                    yPos += nrOfLinesThisResult * 
                        printCompetitorHeaderFont.GetHeight();
                    competitorsDone = i;

                    // If there is more shooters to print out and no more room
                    if (yPos + nrOfLinesThisResult * printCompetitorHeaderFont.GetHeight() >
                        ev.MarginBounds.Bottom)
                    {
                        competitorsDone++;
                        return true;
                    }
                }
            }
            return moreClassesPages();
        }
        private void printString(PrintPageEventArgs ev, string str, 
            Font printingFont, float x, float y, float maxWidth)
        {
            while(ev.Graphics.MeasureString(str, printingFont).Width > maxWidth)
            {
                str = str.Substring(0, str.Length-1);
            }

            ev.Graphics.DrawString(str,
                printingFont, Brushes.Black, x, y,
                new StringFormat());
        }
        #endregion

        #region Print Teams Result
        ResultsReturnTeam[] teamresults = null;
        int currentTeamCheck = 0;
        int currentTeamPlace = 0;
        private bool printTeamResults(PrintPageEventArgs ev, ref float yPos, int tab)
        {
            printHeader(ev, ref yPos, tab);

            this.printHeaderTeam(ev, ref yPos);

            if (teamresults == null)
            {
                teamresults = CommonCode.ResultsGetTeams(
                    this.wclass,
                    this.competition);
            }

            //int place=0;
            //foreach(ResultsReturnTeam result in teamresults)
            for(int i=currentTeamCheck; i<teamresults.Length;i++)
            {
                ResultsReturnTeam result = teamresults[i];
                bool printThisTeam = true;
                if (this.clubId != null)
                {
                    if (this.clubId != result.ClubId)
                        printThisTeam = false;
                    else
                        printThisTeam = true;
                }
                if (printThisTeam)
                {
                    if (yPos + calculateTeamHeight(result) > ev.MarginBounds.Bottom)
                    {
                        currentTeamCheck = i;
                        return true;
                    }
                    currentTeamPlace++;
                    printTeamResult(ev, ref yPos, currentTeamPlace, result);
                    printTeamMembersResults(ev, ref yPos, result);

                    // Ok, done printing. Draw line beneath the shooter.
                    yPos += 1;
                    ev.Graphics.DrawLine(new Pen(Brushes.Black, 1),
                        new PointF(this.LeftMargin, yPos - 1),
                        new PointF(this.RightMargin, yPos - 1));
                }
            }

            return false;
        }

        private float calculateTeamHeight(ResultsReturnTeam result)
        {
            float teamHeight = printCompetitorHeaderFont.GetHeight();
            Structs.Team team = CommonCode.GetTeam(result.TeamId);
            int nrCompetitors = team.CompetitorIds.Count;
            return teamHeight * (2 + nrCompetitors);
        }
        
        private void printHeaderTeam(PrintPageEventArgs ev, ref float yPos)
        {
            yPos += printHeaderFont.GetHeight();

            // Print result header
            ev.Graphics.DrawString("Pl",
                printCompetitorHeaderFont, Brushes.Black, this.colTeamPlace, yPos,
                new StringFormat());
            ev.Graphics.DrawString("Lag",
                printCompetitorHeaderFont, Brushes.Black, this.colTeamName, yPos,
                new StringFormat());
            ev.Graphics.DrawString("Klubb",
                printCompetitorHeaderFont, Brushes.Black, this.colTeamClub, yPos,
                new StringFormat());
            ev.Graphics.DrawString("Resultat",
                printCompetitorHeaderFont, Brushes.Black, this.colTeamResult, yPos,
                new StringFormat());
            ev.Graphics.DrawString("Tot",
                printCompetitorHeaderFont, Brushes.Black, this.colTeamResultTot, yPos,
                new StringFormat());
            if (CompetitionType == Structs.CompetitionTypeEnum.Field |
                CompetitionType == Structs.CompetitionTypeEnum.MagnumField)
            {
                ev.Graphics.DrawString("P",
                    printCompetitorHeaderFont, Brushes.Black, this.colTeamPoints, yPos,
                    new StringFormat());
            }
            yPos += printCompetitorHeaderFont.GetHeight();

            // Underline
            ev.Graphics.DrawLine(new System.Drawing.Pen(Brushes.Black, 2), 
                new System.Drawing.PointF(this.LeftMargin, yPos), 
                new System.Drawing.PointF(this.RightMargin, yPos));
            yPos += 2;
        }
        private void printTeamResult(PrintPageEventArgs ev, ref float yPos, int place, ResultsReturnTeam result)
        {
            int nrOfLinesThisResult = 1;
            printString(ev, place.ToString(), 
                printCompetitorFont, this.colTeamPlace, yPos, this.colTeamClub-this.colTeamPlace);
            
            printString(ev, CommonCode.GetClub(result.ClubId).Name,
                printCompetitorFont, this.colTeamClub, yPos, this.colTeamName-this.colTeamClub);

            printString(ev, result.TeamName,
                printCompetitorFont, this.colTeamName, yPos, this.colTeamResult-this.colTeamName);
            
            float xPosHitsPerStn = this.colTeamResult;
            foreach(string strnTemp in result.HitsPerStn.Split(';'))
            {
                string strn = strnTemp;
                if (xPosHitsPerStn + 
                    ev.Graphics.MeasureString(strn, printCompetitorFont).Width >
                    colTeamResultMaxWidth)
                {
                    nrOfLinesThisResult++;
                    xPosHitsPerStn = this.colTeamResult;
                }
                if (strn != "")
                {
                    switch (CompetitionType)
                    {
                        case Structs.CompetitionTypeEnum.Field:
                            if (!competition.NorwegianCount)
                            {
                                string[] parts = strn.Split('/');
                                int hits = int.Parse(parts[0]);
                                int figures = int.Parse(parts[1]);
                                strn = hits.ToString();
                            }
                            else
                            {
                            }
                            break;
                        case Structs.CompetitionTypeEnum.MagnumField:
                            if (!competition.NorwegianCount)
                            {
                                string[] parts = strn.Split('/');
                                int hits = int.Parse(parts[0]);
                                int figures = int.Parse(parts[1]);
                                strn = hits.ToString();
                            }
                            else
                            {
                            }
                            break;
                        case Structs.CompetitionTypeEnum.Precision:
                            break;
                    }
                }
                ev.Graphics.DrawString(strn,
                    printCompetitorFont, Brushes.Black, xPosHitsPerStn, 
                    yPos + (nrOfLinesThisResult-1) * 
                    printCompetitorHeaderFont.GetHeight(),
                    new StringFormat());
                xPosHitsPerStn += ev.Graphics.MeasureString(strn + "  ", 
                    printCompetitorFont).Width;
            }

            switch(CompetitionType)
            {
                case Structs.CompetitionTypeEnum.Field:
                {
                    if (competition.NorwegianCount)
                        printString(ev, (result.Hits+result.FigureHits).ToString(),
                            printCompetitorFont, this.colTeamResultTot, yPos, this.colTeamPoints-this.colTeamResultTot);
                    else
                        printString(ev, result.Hits.ToString() + "/" + result.FigureHits.ToString(),
                            printCompetitorFont, this.colTeamResultTot, yPos, this.colTeamPoints-this.colTeamResultTot);

                    printString(ev, result.Points.ToString(),
                        printCompetitorFont, this.colTeamPoints, yPos, this.RightMargin-this.colTeamPoints);

                    break;
                }
                case Structs.CompetitionTypeEnum.MagnumField:
                {
                    printString(ev, result.Hits.ToString() + "/" + result.FigureHits.ToString(),
                        printCompetitorFont, this.colTeamResultTot, yPos, this.colTeamPoints-this.colTeamResultTot);

                    printString(ev, result.Points.ToString(),
                        printCompetitorFont, this.colTeamPoints, yPos, this.RightMargin-this.colTeamPoints);

                    break;
                }
                case Structs.CompetitionTypeEnum.Precision:
                {
                    printString(ev, result.Hits.ToString(),
                        printCompetitorFont, this.colTeamResultTot, yPos, this.colTeamPoints-this.colTeamResultTot);
                    break;
                }

                default:
                    throw new ApplicationException("Unknown CompetitionType");
            }

            yPos += nrOfLinesThisResult * 
                printCompetitorHeaderFont.GetHeight();
        }
        private void printTeamMembersResults(PrintPageEventArgs ev, ref float yPos, ResultsReturnTeam result)
        {
            Structs.Team team = CommonCode.GetTeam(result.TeamId);

            foreach (int compid in (int[])team.CompetitorIds.ToArray(typeof(int)))
            {
                Structs.Competitor comp = CommonCode.GetCompetitor(compid);
                printTeamMemberResults(ev, ref yPos, comp);
                yPos += printCompetitorFont.GetHeight();
            }
        }
        private void printTeamMemberResults(PrintPageEventArgs ev, ref float yPos, Structs.Competitor competitor)
        {
            try
            {
                Structs.Shooter shooter = CommonCode.GetShooter(competitor.ShooterId);
                printString(ev, shooter.Surname + " " + shooter.Givenname,
                    printCompetitorFont, this.colTeamResult, yPos, this.colTeamResultTot-this.colTeamResult);

                int hits = 0;
                int figurehits = 0;
                foreach (Structs.CompetitorResult res in CommonCode.GetCompetitorResults(competitor.CompetitorId))
                {
                    hits += res.Hits;
                    figurehits += res.FigureHits;
                }

                switch(CompetitionType)
                {
                    case Structs.CompetitionTypeEnum.Field:
                    {
                        if (competition.NorwegianCount)
                            printString(ev, (hits+figurehits).ToString(),
                                printCompetitorFont, this.colTeamResultTot, yPos, this.colTeamPoints-this.colTeamResultTot);
                        else
                            printString(ev, hits.ToString() + "/" + figurehits.ToString(),
                                printCompetitorFont, this.colTeamResultTot, yPos, this.colTeamPoints-this.colTeamResultTot);
                        break;
                    }
                    case Structs.CompetitionTypeEnum.MagnumField:
                    {
                        printString(ev, hits.ToString() + "/" + figurehits.ToString(),
                            printCompetitorFont, this.colTeamResultTot, yPos, this.colTeamPoints-this.colTeamResultTot);
                        break;
                    }
                    case Structs.CompetitionTypeEnum.Precision:
                    {
                        printString(ev, hits.ToString(),
                            printCompetitorFont, this.colTeamResultTot, yPos, this.colTeamPoints-this.colTeamResultTot);
                        break;
                    }
                    default:
                        throw new ApplicationException("Unknown CompetitionType");
                }
            }
            catch(Exception exc)
            {
                Trace.WriteLine("CPrintResultlist.printTeamMemberResults Exception: " + exc.ToString());
            }
        }
        #endregion
    }
}
