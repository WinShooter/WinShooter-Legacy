// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CInternetPdfExport.cs" company="John Allberg">
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
//   Summary description for InternetExport.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System;
    using System.Drawing;
    using System.IO;

    using Allberg.Shooter.WinShooterServerRemoting;

    using sharpPDF;
    using sharpPDF.Enumerators;
    using sharpPDF.Fonts;

    /// <summary>
    /// Summary description for InternetExport.
    /// </summary>
    public class CInternetPdfExport
    {
        internal CInternetPdfExport(Interface callerInterface)
        {
            myInterface = callerInterface;
            settings = CSettings.Instance;

            columnPatrolsPatrolPos = leftMargin;
            columnPatrolsNamePos = columnPatrolsPatrolPos + 25;
            columnPatrolsClubNamePos = columnPatrolsNamePos + 70;
            columnPatrolsWeaponPos = columnPatrolsClubNamePos + 50;

            columnPatrolsByClubClubIdPos = leftMargin;
            columnPatrolsByClubNamePos = columnPatrolsByClubClubIdPos + 15;
            columnPatrolsByClubPatrolPos = columnPatrolsByClubNamePos + 70;
            columnPatrolsByClubWeaponPos = columnPatrolsByClubPatrolPos + 20;
            columnPatrolsByClubClassPos = columnPatrolsByClubWeaponPos + 30;

            colResultNamePos = leftMargin + 10;
            colResultClubPos = colResultNamePos + 40;
            colResultResultPos = colResultClubPos + 45;
            colResultTotPos = colResultResultPos + 60; //40
            colResultPointsPos = colResultTotPos + 15;
            colResultStmPos = colResultPointsPos + 5;
            colResultPricePos = colResultStmPos + 10;
        }
        Interface myInterface;
        CSettings settings;

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

        pdfDocument document = null;
        pdfPage page = null;

        #region Common
        private const double conversionPixelsToMM = 0.353;
        private pdfPage getNewPage(ref double yPos)
        {
            yPos = topMargin;
            if (document == null)
            {
                document = new pdfDocument(myInterface.GetCompetitions()[0].Name,"Allberg Winshooter");
                font = document.getFontReference(predefinedFont.csTimes);
                fontCompetitionHeader = document.getFontReference(predefinedFont.csTimesBold);
                fontHeader = document.getFontReference(predefinedFont.csTimesBold);
            }

            //double xsize = 210/conversionPixelsToMM; // A4 = 210*297mm (35,3*35,3 mm)
            //double ysize = 297/conversionPixelsToMM;
            bottomMargin = (int)((297-20));
            //pdfPage myPage = document.addPage((int)ysize, (int)xsize);
            pdfPage myPage = document.addPage(predefinedPageSize.csA4Page);

            sharpPDF.Fonts.pdfAbstractFont headerFont = document.getFontReference(predefinedFont.csTimes);

            // Add copyright text
            addText(myPage, "WinShooter", 
                leftMargin, (int)((bottomMargin+3)), headerFont, 10);

            //addText(myPage, "©" + settings.PrinterSettings.Copyright, 
            //	leftMargin+155, (int)((bottomMargin+3)), headerFont, 10);

            // Add club logo
            Image image = settings.Logo;

            int logoHeight = image.Height;
            int logoWidth = image.Width;

            calculateLogoSize(image, 60, 200, out logoHeight, out logoWidth);

            int logoX = myPage.width - 20 - logoWidth;
            int logoY = myPage.height - 20 - logoHeight;

            try
            {
                sharpPDF.Elements.pdfImageReference imageRef = document.getImageReference("logo");
                myPage.addImage(imageRef, logoX, logoY, logoHeight, logoWidth);
            }
            catch (sharpPDF.Exceptions.pdfImageNotLoadedException)
            {
                document.addImageReference((System.Drawing.Image)image, "logo");
                sharpPDF.Elements.pdfImageReference imageRef = document.getImageReference("logo");
                myPage.addImage(imageRef, logoX, logoY, logoHeight, logoWidth);
            }

            // Add Winshooter logo
            image = settings.GetWinshooterLogo(1000, 1000);

            logoHeight = image.Height;
            logoWidth = image.Width;

            calculateLogoSize(image, 50, 200, out logoHeight, out logoWidth);

            logoX = myPage.width - 20 - logoWidth;
            logoY = 10;

            try
            {
                sharpPDF.Elements.pdfImageReference imageRef = document.getImageReference("WinShooterLogo");
                myPage.addImage(imageRef, logoX, logoY, logoHeight, logoWidth);
            }
            catch (sharpPDF.Exceptions.pdfImageNotLoadedException)
            {
                document.addImageReference((System.Drawing.Image)image, "WinShooterLogo");
                sharpPDF.Elements.pdfImageReference imageRef = document.getImageReference("WinShooterLogo");
                myPage.addImage(imageRef, logoX, logoY, logoHeight, logoWidth);
            }

            //
            TextStart = 20 + conversionPixelsToMM * logoHeight;

            return myPage;
        }
        double TextStart = 0;
        private void calculateLogoSize(Image image, int maxImageHeigh, int maxImageWidth,
            out int height, out int width)
        {
            double xfactor = (double)maxImageWidth / (double)image.Width;
            double yfactor = (double)maxImageHeigh / (double)image.Height;
            if (xfactor > yfactor)
                xfactor = yfactor;
            else
                yfactor = xfactor;

            height = (int)(yfactor * (double)image.Height);
            width = (int)(xfactor * (double)image.Width);
        }
        private byte[] returnPdfInBytes()
        {
            System.IO.MemoryStream stream = new MemoryStream();

            document.createPDF(stream);
            document = null; 
            page = null;

            return stream.ToArray();
        }
        
        sharpPDF.Fonts.pdfAbstractFont font;
        int fontSize = 12;
        int fontResultSize = 10;
        sharpPDF.Fonts.pdfAbstractFont fontHeader;
        int fontHeaderSize = 12;
        sharpPDF.Fonts.pdfAbstractFont fontCompetitionHeader;
        int fontCompetitionHeaderSize = 14;

        int leftMargin = 13;
        int topMargin = 13;
        int bottomMargin = 20;

        int columnPatrolsPatrolPos;
        int columnPatrolsNamePos;
        int columnPatrolsClubNamePos;
        int columnPatrolsWeaponPos;

        int columnPatrolsByClubClubIdPos;
        int columnPatrolsByClubNamePos;
        int columnPatrolsByClubPatrolPos;
        int columnPatrolsByClubWeaponPos;
        int columnPatrolsByClubClassPos;

        int colResultNamePos;
        int colResultClubPos;
        int colResultResultPos;
        int colResultTotPos;
        int colResultPointsPos;
        int colResultStmPos;
        int colResultPricePos;

        private double addText(pdfPage page, string text, double xPosInMM, double yPosInMM, pdfAbstractFont writeFont, int writeFontSize)
        {
            if (page == null)
                page = getNewPage(ref yPosInMM);

            int xInPoints = (int)(xPosInMM/conversionPixelsToMM);
            int yInPoints = page.height - (int)(yPosInMM/conversionPixelsToMM);
            page.addText(text,xInPoints,yInPoints,writeFont,writeFontSize);
            double newY = (double)(page.height-yInPoints+writeFontSize);
            newY = conversionPixelsToMM*newY;
            return newY;
        }

        System.Drawing.Graphics graphics = 
            System.Drawing.Graphics.FromImage(
            new System.Drawing.Bitmap(1000,1000));

        private double addText(pdfPage page, string text, 
            double xPosInMM, double yPosInMM, double maxXSizeInMM, 
            pdfAbstractFont writeFont, int writeFontSize)
        {
            int maxXSizeInPoints = (int)(maxXSizeInMM/conversionPixelsToMM);
            while (writeFont.getWordWidth(text, writeFontSize) > maxXSizeInPoints)
            {
                text = text.Substring(0, text.Length-1);
            }
            
            int xInPoints = (int)(xPosInMM/conversionPixelsToMM);
            int yInPoints = page.height - (int)(yPosInMM/conversionPixelsToMM);
            page.addText(text,xInPoints,yInPoints,writeFont,writeFontSize);
            double newY = (double)(page.height-yInPoints+writeFontSize);
            newY = conversionPixelsToMM*newY;
            return newY;
        }

        private void drawLine(pdfPage page, double x1, double y1, double x2, double y2, int lineSize)
        {
            int x1InPoints = (int)(x1/conversionPixelsToMM);
            int x2InPoints = (int)(x2/conversionPixelsToMM);
            int y1InPoints = page.height - (int)(y1/conversionPixelsToMM);
            int y2InPoints = page.height - (int)(y2/conversionPixelsToMM);

            page.drawLine(x1InPoints, y1InPoints, x2InPoints, y2InPoints, 
                predefinedLineStyle.csNormal, pdfColor.Black, lineSize);
        }
        #endregion

        #region Patrols
        internal byte[] ExportPatrols()
        {
            double yPos = topMargin;
            pdfPage page = getNewPage(ref yPos);
            yPos = writePatrolPageHeader(page, yPos);

            yPos += 10;
            yPos = writePatrolHeader(page, yPos);

            foreach(Structs.Patrol patrol in myInterface.GetPatrols())
            {
                if(myInterface.GetCompetitorsCountPatrol(patrol) > 0)
                {
                    writePatrol(ref page, patrol, ref yPos);
                }
            }
            return returnPdfInBytes();
        }

        private double writePatrolPageHeader(pdfPage page, double yPos)
        {
            Structs.Competition comp = myInterface.GetCompetitions()[0];
            
            addText(page, "Tävling", leftMargin, yPos, fontCompetitionHeader, fontCompetitionHeaderSize);
            yPos = addText(page, comp.Name, 
                leftMargin + 30, yPos , fontCompetitionHeader, fontCompetitionHeaderSize);

            addText(page, "Datum", leftMargin, yPos, fontCompetitionHeader, fontCompetitionHeaderSize);
            yPos = addText(page, comp.StartTime.ToShortDateString(), 
                leftMargin + 30, yPos , fontCompetitionHeader, fontCompetitionHeaderSize);
            return yPos;
        }

        private double writePatrol(ref pdfPage page, Structs.Patrol patrol, ref double yPos)
        {
            if (yPos + conversionPixelsToMM * fontSize > bottomMargin)
            {
                page = getNewPage(ref yPos);
                yPos = writePatrolHeader(page, yPos);
            }
            addText(page, patrol.PatrolId.ToString() + " ( " + patrol.StartDateTimeDisplay.ToShortTimeString() + " )", 
                columnPatrolsPatrolPos, yPos, font, fontSize);

            foreach(Structs.Competitor comp in myInterface.GetCompetitors(patrol))
            {
                if (yPos + conversionPixelsToMM * fontSize > bottomMargin)
                {
                    page = getNewPage(ref yPos);
                    yPos = writePatrolHeader(page, yPos);
                }
                Structs.Shooter shooter = myInterface.GetShooter(comp.ShooterId);
                Structs.Club club = myInterface.GetClub(shooter.ClubId);
                Structs.Weapon weapon = myInterface.GetWeapon(comp.WeaponId);
                string weaponString = getWeapon(weapon);

                addText(page, shooter.Givenname + ", " + shooter.Surname, 
                    columnPatrolsNamePos, yPos,
                    columnPatrolsClubNamePos - columnPatrolsNamePos,
                    font, fontSize);
                addText(page, club.Name,
                    columnPatrolsClubNamePos, yPos, 
                    columnPatrolsWeaponPos - columnPatrolsClubNamePos, 
                    font, fontSize);
                yPos = addText(page, weaponString,
                    columnPatrolsWeaponPos, yPos, font, fontSize);

                yPos += 1;
            }
            return yPos;
        }
        
        private const string Unknown = "_Okänt";

        private string getWeapon(Structs.Weapon weapon)
        {
            string weaponString = "";
            if (weapon.WeaponId.StartsWith(Unknown))
            {
                weaponString = weapon.WeaponId.Replace(Unknown,"");
            }
            else
            {
                weaponString = weapon.WeaponId;
                if (!weaponString.EndsWith(")"))
                {
                    weaponString += " (" + weapon.WClass + ")";
                }
            }

            return weaponString;
        }

        private double writePatrolHeader(pdfPage page, double yPos)
        {
            yPos += 1;
            drawLine(page, leftMargin, yPos, columnPatrolsWeaponPos + 25, yPos, 1);
            yPos -= 1;

            addText(page, "Patrull", columnPatrolsPatrolPos, yPos, fontHeader, fontHeaderSize);
            addText(page, "Namn", columnPatrolsNamePos, yPos, fontHeader, fontHeaderSize);
            addText(page, "Klubb", columnPatrolsClubNamePos, yPos, fontHeader, fontHeaderSize);
            yPos = addText(page, "Vapen", this.columnPatrolsWeaponPos, yPos, fontHeader, fontHeaderSize);
            yPos += 1;

            return yPos;
        }
        #endregion

        #region PatrolsByClubs
        internal byte[] ExportPatrolsByClub()
        {
            double yPos = topMargin;
            pdfPage page = getNewPage(ref yPos);
            yPos = writePatrolPageHeader(page, yPos);

            yPos += 10;
            yPos = writePatrolByClubHeader(page, yPos);

            foreach(Structs.Club club in myInterface.GetClubs())
            {
                Structs.Shooter[] shooters = myInterface.GetShooters(club);
                if(shooters.Length > 0)
                {
                    writePatrolByClub(ref page, shooters, ref yPos);
                }
            }
            return returnPdfInBytes();
        }
        private double writePatrolByClubHeader(pdfPage page, double yPos)
        {
            yPos += 1;
            drawLine(page, leftMargin, yPos, leftMargin + 120, yPos, 1);
            yPos -= 1;

            addText(page, "Klubb", this.leftMargin, yPos, fontHeader, fontHeaderSize);
            addText(page, "Namn", columnPatrolsByClubNamePos, yPos, fontHeader, fontHeaderSize);
            addText(page, "Patrull", columnPatrolsByClubPatrolPos, yPos, fontHeader, fontHeaderSize);
            addText(page, "Vapen", columnPatrolsByClubWeaponPos, yPos, fontHeader, fontHeaderSize);
            yPos = addText(page, "Klass", columnPatrolsByClubClassPos, yPos, fontHeader, fontHeaderSize);
            yPos += 1;

            return yPos;
        }
        private double writePatrolByClub(ref pdfPage page, Structs.Shooter[] shooters, ref double yPos)
        {
            Structs.Club club = myInterface.GetClub(shooters[0].ClubId);

            if (yPos + conversionPixelsToMM * (2*fontSize + fontHeaderSize) > bottomMargin)
            {
                // Paging
                page = getNewPage(ref yPos);
                yPos = writePatrolByClubHeader(page, yPos);
            }
            addText(page, club.ClubId, 
                columnPatrolsByClubClubIdPos, yPos, fontHeader, fontHeaderSize);
            yPos = addText(page, club.Name, 
                columnPatrolsByClubNamePos, yPos, fontHeader, fontHeaderSize);

            foreach(Structs.Shooter shooter in shooters)
            {
                if (yPos + 2 * conversionPixelsToMM * fontSize > bottomMargin)
                {
                    // Paging
                    page = getNewPage(ref yPos);
                    yPos = writePatrolByClubHeader(page, yPos);
                }
                foreach(Structs.Competitor comp in myInterface.GetCompetitors(shooter.ShooterId, "PatrolId"))
                {
                    if (yPos + 2 * conversionPixelsToMM * fontSize > bottomMargin)
                    {
                        // Paging
                        page = getNewPage(ref yPos);
                        yPos = writePatrolByClubHeader(page, yPos);
                    }

                    if (comp.PatrolId >= 1)
                    {
                        Structs.Patrol patrol = myInterface.GetPatrol(comp.PatrolId);
                        string weaponText = getWeapon(myInterface.GetWeapon(comp.WeaponId));

                        addText(page, shooter.Givenname + ", " + shooter.Surname, 
                            columnPatrolsByClubNamePos, yPos, font, fontSize);
                        addText(page, patrol.PatrolId.ToString() + " (" +
                            patrol.StartDateTimeDisplay.ToShortTimeString() + ")",
                            columnPatrolsByClubPatrolPos, yPos, font, fontSize);
                        addText(page, weaponText, 
                            columnPatrolsByClubWeaponPos, yPos, font, fontSize);

                        Structs.ShootersClassShort sc = (Structs.ShootersClassShort)(int)comp.ShooterClass;
                        string classText = sc.ToString().Replace("Klass", "").Replace("Damklass", "D");
                        classText += myInterface.GetWeapon(comp.WeaponId).WClass.ToString().Substring(0,1);

                        yPos = addText(page, classText,
                            columnPatrolsByClubClassPos, yPos, font, fontSize);

                        yPos += 1;
                    }
                }
            }

            return yPos+5;
        }
        #endregion

        #region Results
        internal byte[] ExportResults(bool finalResults, bool allOnOnePage)
        {
            if (finalResults != true)
                throw new ApplicationException("Not implemented yet");

            Structs.ResultWeaponsClass[] wclasses =
                myInterface.resultClass.ResultsGetWClasses();

            double yPos = topMargin;

            page = getNewPage(ref yPos);

            yPos = writeResultsPageHeader(page, yPos);

            foreach(Structs.ResultWeaponsClass wclass in wclasses)
            {
                foreach(Structs.ShootersClass uclass in myInterface.resultClass.ResultsGetUClasses(wclass))
                {
                    if (page == null | !allOnOnePage)
                    {
                        page = getNewPage(ref yPos);
                    }
                    ExportResults(wclass, uclass, finalResults, false, ref page, ref yPos);
                    yPos += 10;
                }
            }

            foreach(Structs.ResultWeaponsClass wclass in wclasses)
            {
                //foreach(Structs.ShootersClass uclass in myInterface.resultClass.ResultsGetUClasses(wclass))
                {
                    if (page == null | !allOnOnePage)
                    {
                        page = getNewPage(ref yPos);
                    }
                    ExportTeamResults(wclass, finalResults, false, ref page, ref yPos);
                    yPos += 10;
                }
            }

            return returnPdfInBytes();
        }
        internal byte[] ExportResults(Structs.ResultWeaponsClass wclass, 
            Structs.ShootersClass uclass, bool finalResults)
        {
            if (finalResults != true)
                throw new ApplicationException("Not implemented yet");

            double yPos = topMargin;

            sharpPDF.pdfPage page = null;

            return ExportResults(wclass, uclass, finalResults, true, ref page, ref yPos);
        }

        private byte[] ExportResults(Structs.ResultWeaponsClass wclass, 
            Structs.ShootersClass uclass, bool finalResults, bool returnDocument, 
            ref pdfPage page, ref double yPos)
        {
            if (yPos + conversionPixelsToMM * 3*fontSize >= bottomMargin)
            {
                page = this.getNewPage(ref yPos);
            }

            // Write headers
            yPos = writeResultsClassHeader(page, yPos, wclass, uclass);
            if (yPos < TextStart)
            {
                yPos = TextStart;
            }
            yPos = writeResultsHeader(page, yPos);

            ResultsReturn[] results = myInterface.resultClass.GetResults(wclass, uclass, 
                myInterface.CompetitionCurrent, true);

            int i=0;
            foreach(ResultsReturn result in results)
            {
                if (yPos + conversionPixelsToMM * fontSize >= bottomMargin)
                {
                    page = this.getNewPage(ref yPos);
                    yPos = writeResultsClassHeader(page, yPos, wclass, uclass);
                    if (yPos < TextStart)
                    {
                        yPos = TextStart;
                    }
                    yPos = writeResultsHeader(page, yPos);
                }
                i++;
                yPos = writeResultsUser(page, yPos, result, i);
            }
            
            // Return what we've created
            if (returnDocument)
            {
                return returnPdfInBytes();
            }
            else
            {
                return new byte[0];
            }
        }

        private double writeResultsPageHeader(pdfPage page, double yPos)
        {
            Structs.Competition comp = myInterface.GetCompetitions()[0];

            addText(page, "Tävling:", leftMargin, yPos, fontCompetitionHeader, fontCompetitionHeaderSize);
            yPos = addText(page, comp.Name + ", " + comp.StartTime.ToShortDateString(), 
                leftMargin + 30, yPos , fontCompetitionHeader, fontCompetitionHeaderSize);
            
            yPos = yPos + 4;
            return yPos;
        }
        private double writeResultsClassHeader(pdfPage page, double yPos,
            Structs.ResultWeaponsClass wclass, Structs.ShootersClass uclass)
        {
            Structs.ShootersClassShort uclassshort = (Structs.ShootersClassShort)(int)uclass;
            string klass = uclassshort.ToString().Replace("Klass", "") +
                wclass.ToString();
            addText(page, "Klass:", leftMargin, yPos, fontCompetitionHeader, fontCompetitionHeaderSize);
            yPos = addText(page, klass,
                leftMargin + 30, yPos, fontCompetitionHeader, fontCompetitionHeaderSize);

            yPos = yPos + 4;
            return yPos;
        }
        private double writeResultsPageTeamHeader(pdfPage page, double yPos, 
            Structs.ResultWeaponsClass wclass)
        {
            Structs.Competition comp = myInterface.GetCompetitions()[0];

            addText(page, "Tävling:", leftMargin, yPos, fontCompetitionHeader, fontCompetitionHeaderSize);
            yPos = addText(page, comp.Name + ", " + comp.StartTime.ToShortDateString(), 
                leftMargin + 30, yPos , fontCompetitionHeader, fontCompetitionHeaderSize);
            
            addText(page, "Vapen:", leftMargin, yPos, fontCompetitionHeader, fontCompetitionHeaderSize);
            yPos = addText(page, wclass.ToString(), 
                leftMargin + 30, yPos , fontCompetitionHeader, fontCompetitionHeaderSize);

            /*addText(page, "Klass:", leftMargin, yPos, fontCompetitionHeader, fontCompetitionHeaderSize);
            yPos = addText(page, uclass.ToString(), 
                leftMargin + 30, yPos , fontCompetitionHeader, fontCompetitionHeaderSize);*/

            yPos = yPos + 4;
            return yPos;
        }
        private double writeResultsHeader(pdfPage page, double yPos)
        {
            drawLine(page, this.leftMargin, yPos+1, 225, yPos+1, 1);

            double newYPos = addText(page, "Pl", leftMargin, yPos, fontHeader, fontCompetitionHeaderSize);
            addText(page, "Namn", colResultNamePos, yPos, fontHeader, fontCompetitionHeaderSize);
            addText(page, "Klubb", colResultClubPos, yPos, fontHeader, fontCompetitionHeaderSize);
            addText(page, "Resultat", colResultResultPos, yPos, fontHeader, fontCompetitionHeaderSize);
            addText(page, "Tot", colResultTotPos, yPos, fontHeader, fontCompetitionHeaderSize);
            addText(page, "P", colResultPointsPos, yPos, fontHeader, fontCompetitionHeaderSize);

            addText(page, "Stm", colResultStmPos, yPos, fontHeader, fontCompetitionHeaderSize);
            if (myInterface.CompetitionCurrent.UsePriceMoney)
            {
                addText(page, "Pris", colResultPricePos, yPos, fontHeader, fontCompetitionHeaderSize);
            }

            return newYPos;
        }		
        private double writeResultsTeamHeader(pdfPage page, double yPos)
        {
            drawLine(page, this.leftMargin, yPos+1, 225, yPos+1, 1);

            addText(page, "Pl", leftMargin, yPos, fontHeader, fontCompetitionHeaderSize);
            addText(page, "Lag", colResultNamePos, yPos, fontHeader, fontCompetitionHeaderSize);
            addText(page, "Klubb", colResultClubPos, yPos, fontHeader, fontCompetitionHeaderSize);
            addText(page, "Resultat", colResultResultPos, yPos, fontHeader, fontCompetitionHeaderSize);
            addText(page, "Tot", colResultTotPos, yPos, fontHeader, fontCompetitionHeaderSize);
            yPos = addText(page, "P", colResultPointsPos, yPos, fontHeader, fontCompetitionHeaderSize);

            //addText(page, "Stm", colResultStmPos, yPos, fontHeader, fontCompetitionHeaderSize);
            //addText(page, "Pris", colResultPricePos, yPos, fontHeader, fontCompetitionHeaderSize);

            return yPos;
        }
        Structs.Competition competition = new Allberg.Shooter.WinShooterServerRemoting.Structs.Competition();
        private double writeResultsUser(pdfPage page, double yPos, ResultsReturn result, int place)
        {
            double newYPos = yPos;
            Structs.Club club = myInterface.GetClub(result.ClubId);
            if (competition.CompetitionId == 0)
                competition = myInterface.GetCompetitions()[0];

            addText(page, place.ToString(), this.leftMargin, yPos, font, fontResultSize);
            addText(page, result.ShooterName, this.colResultNamePos, yPos, colResultClubPos-colResultNamePos,font, fontResultSize);
            addText(page, club.Name, this.colResultClubPos, yPos, colResultResultPos-colResultClubPos,font, fontResultSize);
            switch(competition.Type)
            {
                case Structs.CompetitionTypeEnum.Field:
                {
                    if (competition.NorwegianCount)
                    {
                        addText(page, (result.HitsTotal + result.FigureHitsTotal).ToString(), 
                            colResultTotPos, yPos, font, fontResultSize);
                    }
                    else
                    {
                        addText(page, result.HitsTotal.ToString() + "/" + result.FigureHitsTotal.ToString(),
                            colResultTotPos, yPos, font, fontResultSize);
                    }
                    addText(page, result.PointsTotal.ToString(), 
                        this.colResultPointsPos, yPos, font, fontResultSize);
                    break;
                }
                case Structs.CompetitionTypeEnum.Precision:
                {
                    addText(page, result.HitsTotal.ToString(), 
                        this.colResultPointsPos, yPos, font, fontResultSize);
                    break;
                }
                default:
                    throw new ApplicationException("Not implemented yet");
            }

            string medalText = "";
            switch((Structs.Medal)result.Medal)
            {
                case Structs.Medal.StandardSilver:
                    medalText = "S";
                    break;
                case Structs.Medal.StardardBrons:
                    medalText = "B";
                    break;
            }
            newYPos = addText(page, medalText, this.colResultStmPos, yPos, font, fontResultSize);

            if (competition.UsePriceMoney && 
                result.PriceMoney != 0)
            {
                newYPos = addText(page, result.PriceMoney.ToString() + ":-",
                    this.colResultPricePos, yPos, font, fontResultSize);
            }

            string[] HitsPerStn = result.HitsPerStnString.Split(';');
            double xPos = colResultResultPos;
            double xMaxPos = colResultTotPos;
            foreach(string thisStnPre in HitsPerStn)
            {
                if (thisStnPre != "")
                {
                    string thisStn = thisStnPre;

                    switch(competition.Type)
                    {
                        case Structs.CompetitionTypeEnum.Field:
                            {
                                if (competition.NorwegianCount)
                                {

                                }
                                else
                                {
                                    string[] parts = thisStn.Split('/');
                                    int hits = int.Parse(parts[0]);
                                    int figureHits;

                                    figureHits = int.Parse(parts[1]);

                                    thisStn = hits.ToString();
                                }
                                break;
                            }
                        case Structs.CompetitionTypeEnum.MagnumField:
                            {
                                string[] parts = thisStn.Split('/');
                                int hits = int.Parse(parts[0]);
                                int figureHits;
                                figureHits = int.Parse(parts[1]);

                                if (competition.NorwegianCount)
                                {
                                    thisStn = (hits + figureHits).ToString();
                                }
                                else
                                {
                                    thisStn = hits.ToString();
                                }
                                break;
                            }
                        case Structs.CompetitionTypeEnum.Precision:
                            {
                                break;
                            }
                        default:
                            throw new NotImplementedException();
                    }

                    int maxXSizeInPoints = (int)((xMaxPos - xPos) / conversionPixelsToMM);
                    if (font.getWordWidth(thisStn, fontSize) > maxXSizeInPoints)
                    {
                        xPos = colResultResultPos;
                        yPos = newYPos;
                    }
                    newYPos = addText(page, thisStn, xPos, yPos, font, fontResultSize);
                    xPos += conversionPixelsToMM * font.getWordWidth(thisStn, fontSize);
                }
            }
            if (result.FinalShootingPlace != 100)
            {
                addText(page, "(" + result.FinalShootingPlace.ToString() + ")", 
                    xPos, yPos, font, fontResultSize);
            }

            return newYPos;
        }

        private byte[] ExportTeamResults(Structs.ResultWeaponsClass wclass, 
            bool finalResults, bool returnDocument, 
            ref pdfPage page, ref double yPos)
        {
            if (yPos + conversionPixelsToMM * 3 * fontSize >= bottomMargin)
            {
                page = this.getNewPage(ref yPos);
            }

            ResultsReturnTeam[] results = myInterface.ResultsGetTeams(wclass, 
                myInterface.GetCompetitions()[0]);

            if (results.Length == 0)
            {
                if (returnDocument)
                {
                    return returnPdfInBytes();
                }
                else
                {
                    return new byte[0];
                }
            }

            // Write headers
            yPos = writeResultsPageTeamHeader(page, yPos, wclass);
            yPos = writeResultsTeamHeader(page, yPos);

            int i=0;
            foreach(ResultsReturnTeam result in results)
            {
                if (yPos + calculateTeamHeight(result) >= bottomMargin)
                {
                    page = this.getNewPage(ref yPos);
                    yPos = writeResultsPageTeamHeader(page, yPos, wclass);
                    if (yPos < TextStart)
                    {
                        yPos = TextStart;
                    }
                    yPos = writeResultsTeamHeader(page, yPos);
                }
                i++;
                yPos = writeResultsTeam(page, yPos, result, i);
            }
            
            // Return what we've created
            if (returnDocument)
            {
                return returnPdfInBytes();
            }
            else
            {
                return new byte[0];
            }
        }

        double calculateTeamHeight(ResultsReturnTeam result)
        {
            Structs.Team team = myInterface.GetTeam(result.TeamId);
            if (team.CompetitorIds == null)
            {
                return conversionPixelsToMM * fontSize;
            }
            else
            {
                return conversionPixelsToMM * fontSize * (1 + team.CompetitorIds.Count);
            }
        }


        private double writeResultsTeam(pdfPage page, double yPos, ResultsReturnTeam result, int place)
        {
            double newYPos = yPos;
            Structs.Club club = myInterface.GetClub(result.ClubId);
            if (competition.CompetitionId == 0)
                competition = myInterface.GetCompetitions()[0];

            addText(page, place.ToString(), this.leftMargin, yPos, font, fontResultSize);
            addText(page, result.TeamName, this.colResultNamePos, yPos, colResultClubPos-colResultNamePos,font, fontResultSize);
            addText(page, club.Name, this.colResultClubPos, yPos, colResultResultPos-colResultClubPos,font, fontResultSize);
            switch(competition.Type)
            {
                case Structs.CompetitionTypeEnum.Field:
                {
                    if (competition.NorwegianCount)
                    {
                        addText(page, (result.Hits + result.FigureHits).ToString(), 
                            colResultTotPos, yPos, font, fontResultSize);
                    }
                    else
                    {
                        addText(page, result.Hits.ToString() + "/" + result.FigureHits.ToString(), 
                            colResultTotPos, yPos, font, fontResultSize);
                    }
                    addText(page, result.Points.ToString(), 
                        this.colResultPointsPos, yPos, font, fontResultSize);
                    break;
                }
                case Structs.CompetitionTypeEnum.Precision:
                {
                    addText(page, result.Hits.ToString(), 
                        this.colResultPointsPos, yPos, font, fontResultSize);
                    break;
                }
                default:
                    throw new ApplicationException("Not implemented yet");
            }

            /*if (competition.UsePriceMoney && 
                result.PriceMoney != 0)
            {
                newYPos = addText(page, result.PriceMoney.ToString() + ":-",
                    this.colResultPricePos, yPos, font, fontResultSize);
            }*/

            string[] HitsPerStn = result.HitsPerStn.Split(';');
            double xPos = colResultResultPos;
            double xMaxPos = colResultTotPos;
            foreach(string thisStn in HitsPerStn)
            {
                int maxXSizeInPoints = (int)((xMaxPos-xPos)/conversionPixelsToMM);
                if (font.getWordWidth(thisStn, fontSize) > maxXSizeInPoints)
                {
                    xPos = colResultResultPos;
                    yPos = newYPos;
                }
                newYPos = addText(page, thisStn, xPos, yPos, font, fontResultSize);
                xPos += conversionPixelsToMM*font.getWordWidth(thisStn, fontSize);
            }

            Structs.Team team = myInterface.GetTeam(result.TeamId);

            foreach (int compid in team.CompetitorIds.ToArray())
            {
                newYPos = writeResultsTeam(page, newYPos, colResultResultPos, colResultTotPos, compid);
            }
            return newYPos;
        }

        private double writeResultsTeam(pdfPage page, double yPos, double xPos1, double xPos2, int competitorId)
        {
            double newYPos = yPos;

            Structs.Competitor competitor = myInterface.GetCompetitor(competitorId);
            Structs.Shooter shooter = myInterface.GetShooter(competitor.ShooterId);

            newYPos = addText(page, shooter.Surname + " " + shooter.Givenname, xPos1, yPos, font, fontResultSize);

            int hits = 0;
            int figurehits = 0;
            foreach (Structs.CompetitorResult res in myInterface.GetCompetitorResults(competitor.CompetitorId))
            {
                hits += res.Hits;
                figurehits += res.FigureHits;
            }

            switch (competition.Type)
            {
                case Structs.CompetitionTypeEnum.Field:
                    {
                        if (competition.NorwegianCount)
                        {
                            addText(page, (hits + figurehits).ToString(),
                                xPos2, yPos, font, fontResultSize);
                        }
                        else
                        {
                            addText(page, hits.ToString() + "/" + figurehits.ToString(),
                                xPos2, yPos, font, fontResultSize);
                        }
                        break;
                    }
                case Structs.CompetitionTypeEnum.MagnumField:
                    {
                        addText(page, hits.ToString() + "/" + figurehits.ToString(),
                            xPos2, yPos, font, fontResultSize);
                        break;
                    }
                case Structs.CompetitionTypeEnum.Precision:
                    {
                        addText(page, hits.ToString(),
                            xPos2, yPos, font, fontResultSize);
                        break;
                    }
                default:
                    throw new ApplicationException("Unknown CompetitionType");
            }

            return newYPos;
        }

        #endregion


    }
}
