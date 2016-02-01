// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CPrintMirrorLabels.cs" company="John Allberg">
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
//   Summary description for CPrintPatrollist.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Printing;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for CPrintPatrollist.
    /// </summary>
    public class CPrintMirrorLabels : PrintDocument 
    {
        private readonly Common.Interface _commonCode;

        internal CPrintMirrorLabels(ref Common.Interface newCommon)
        {
            _labelCount = 0;
            _nrofseries = 0;
            _currentCompetitorSeries = 0;
            _currentCompetitor = 0;
            _commonCode = newCommon;
        }

        Font _printFont;
        int _currentCompetitor;
        int _currentCompetitorSeries;
        int _nrofseries;
        Structs.Competitor[] _competitors;
        PrintLabelDocument _labelDocument;
        int _labelCount;

        #region Init
        //Override OnBeginPrint to set up the font we are going to use
        protected override void OnBeginPrint(PrintEventArgs ev) 
        {
            base.OnBeginPrint(ev) ;

            _competitors = _commonCode.GetCompetitors();
            _nrofseries = _commonCode.GetStationsCount();
            _competition = _commonCode.GetCompetitions()[0];

            _labelDocument = _commonCode.Settings.PrinterSettings.LabelMirrorPrintDocument;
            _printFont = new Font(_labelDocument.FontName, _labelDocument.FontSize, 
                FontStyle.Regular);
        }

        //Override the OnPrintPage to provide the printing logic for the document
        protected override void OnPrintPage(PrintPageEventArgs ev) 
        {
            _labelDocument.DocumentSizeXPixels = ev.PageBounds.Width;
            _labelDocument.DocumentSizeYPixels = ev.PageBounds.Height;

            for(;_currentCompetitor<_competitors.Length;_currentCompetitor++)
            {
                for(;_currentCompetitorSeries<_nrofseries;_currentCompetitorSeries++)
                {
                    PrintLabel label;
                    try
                    {
                        label = _labelDocument.GetLabel(_labelCount);
                    }
                    catch(PrintLabelDoesNotExistException)
                    {
                        _labelCount = 0;
                        ev.HasMorePages = true;
                        return;
                    }
                    PrintCompetitor(ev,_competitors[_currentCompetitor], _currentCompetitorSeries, label);
                    _labelCount ++;
                }
                _currentCompetitorSeries = 0;
            }
            ev.HasMorePages = false;
        }
        #endregion

        Structs.Competition _competition;
        private void PrintCompetitor(PrintPageEventArgs ev,
            Structs.Competitor competitor, int serie, 
            PrintLabel label)
        {
            Trace.WriteLine("Competitor: " + _competitors[_currentCompetitor].CompetitorId + 
                "\r\nSerie: " + _currentCompetitorSeries);

            var shooter = _commonCode.GetShooter(competitor.ShooterId);

            var y = label.Y + label.MarginalTop;
            var x = label.X + label.MarginalLeft;

            ev.Graphics.DrawString(
                shooter.Surname + " " + shooter.Givenname + " (" + shooter.CardNr + ")",
                _printFont, Brushes.Black,
                x, y, new StringFormat());
            y += _printFont.Height;

            ev.Graphics.DrawString(
                "Lag " + competitor.PatrolId + ", Serie " + (serie+1),
                _printFont, Brushes.Black,
                x, y, new StringFormat());
            y += _printFont.Height;

            ev.Graphics.DrawString(
                _competition.Name + ", " + _competition.StartTime.ToShortDateString(),
                _printFont, Brushes.Black,
                x, y, new StringFormat());
            y += _printFont.Height;
        }
    }
}
