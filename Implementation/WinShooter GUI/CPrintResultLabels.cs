namespace Allberg.Shooter.Windows
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Printing;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for CPrintPatrollist.
    /// </summary>
    public class CPrintResultLabels : PrintDocument 
    {
        private Common.Interface CommonCode;
        private Structs.Patrol Patrol;

        internal CPrintResultLabels(ref Common.Interface newCommon, 
            Structs.Patrol PatrolToPrint, 
            int AlreadyPrintedLabels)
            : base()  
        {
            CommonCode = newCommon;
            Patrol = PatrolToPrint;
            labelCount = AlreadyPrintedLabels;
        }

        Font printFont;
        Structs.Competitor[] competitors;
        PrintLabelDocument labelDocument;
        int labelCount = 0;

        #region Init
        //Override OnBeginPrint to set up the font we are going to use
        protected override void OnBeginPrint(PrintEventArgs ev) 
        {
            base.OnBeginPrint(ev) ;

            competitors = CommonCode.GetCompetitors(Patrol);

            labelDocument = CommonCode.Settings.PrinterSettings.LabelResultDocument;
            printFont = new Font(labelDocument.FontName, labelDocument.FontSize, 
                System.Drawing.FontStyle.Regular);
        }

        //Override the OnPrintPage to provide the printing logic for the document
        protected override void OnPrintPage(PrintPageEventArgs ev) 
        {
            base.OnPrintPage(ev);
            labelDocument.DocumentSizeXPixels = ev.PageBounds.Width;
            labelDocument.DocumentSizeYPixels = ev.PageBounds.Height;

            foreach (Structs.Competitor competitor in competitors)
            {
                PrintLabel label;
                try
                {
                    label = labelDocument.GetLabel(labelCount);
                }
                catch (PrintLabelDoesNotExistException)
                {
                    labelCount = 0;
                    ev.HasMorePages = true;
                    return;
                }
                printCompetitor(ev, competitor, label);
                labelCount++;    
            }


            ev.HasMorePages = false;
        }
        #endregion

        private void printCompetitor(PrintPageEventArgs ev,
            Structs.Competitor competitor, 
            PrintLabel label)
        {
            Trace.WriteLine("Competitor: " + competitor.CompetitorId);

            ResultsReturn result = CommonCode.ResultsGetCompetitor(competitor);

            float y = label.Y + label.MarginalTop;
            float x = label.X + label.MarginalLeft;

            string shooterClass = getShooterClassString(
                CommonCode.GetCompetitor(result.CompetitorId));

            ev.Graphics.DrawString(
                shooterClass,
                printFont, Brushes.Black,
                x, y, new StringFormat());

            float indent = ev.Graphics.MeasureString("RES", printFont).Width;
            x = x + indent;

            ev.Graphics.DrawString(
                result.ShooterName,
                printFont, Brushes.Black,
                x, y, new StringFormat());
            y += printFont.Height;

            Structs.Club club = CommonCode.GetClub(
                result.ClubId);

            ev.Graphics.DrawString(
                club.Name,
                printFont, Brushes.Black,
                x, y, new StringFormat());
            y += printFont.Height;

            string resultString = "";
            switch (CommonCode.CompetitionCurrent.Type)
            {
                case Structs.CompetitionTypeEnum.Field:
                    resultString = getResultStringField(result);
                    break;
                case Structs.CompetitionTypeEnum.MagnumField:
                    resultString = getResultStringField(result);
                    break;
                case Structs.CompetitionTypeEnum.Precision:
                    resultString = result.HitsTotal.ToString() + " p";
                    break;
            }

            ev.Graphics.DrawString(
                resultString,
                printFont, Brushes.Black,
                x, y, new StringFormat());

            y += printFont.Height;
        }

        string getResultStringField(ResultsReturn result)
        {
            string toReturn;
            if (!CommonCode.CompetitionCurrent.NorwegianCount)
            {
                toReturn = result.HitsTotal.ToString() + "/" + result.FigureHitsTotal.ToString() +
                    " (" + result.PointsTotal.ToString() + " p)";
            }
            else
            {
                toReturn = (result.HitsTotal + result.FigureHitsTotal).ToString() +
                    " (" + result.PointsTotal.ToString() + " p)";
            }
            return toReturn;
        }

        private string getShooterClassString(Structs.Competitor comp)
        {
            string toReturn = "";
            Structs.Weapon weapon = CommonCode.GetWeapon(comp.WeaponId);
            toReturn += CommonCode.ConvertWeaponsClassToResultClass( weapon.WClass) .ToString();

            Structs.ShootersClassShort shortSC = (Structs.ShootersClassShort)comp.ShooterClass;
            toReturn += shortSC.ToString()
                .Replace("Klass", "");
            return toReturn;
        }
    }
}
