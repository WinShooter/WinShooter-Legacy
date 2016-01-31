#region copyright
/*
Copyright ©2009 John Allberg

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/
#endregion
// $Id: CPrintFollowup.cs 121 2011-05-28 16:02:14Z smuda $ 
using System;
using System.Drawing;
using System.Drawing.Printing;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	class CPrintFollowup : PrintDocument 
	{
		private readonly Common.Interface _commonCode;
		// Margins
		private float _marginTop = 20;
		private float _marginBottom = 20;
		private float _marginLeft = 20 + 30;
		private float _marginRight = 20;

		internal CPrintFollowup(ref Common.Interface newCommon)
		{
			Color = false;
			_commonCode = newCommon;
		}

		internal bool Color { get; set; }

		private Font _printFont;
		private Font _printAllbergFont;
		private Font _printFontClubHeader;
		private Structs.FollowUpReturn[] _followUp;

		#region Init
		//Override OnBeginPrint to set up the font we are going to use
		protected override void OnBeginPrint(PrintEventArgs ev)
		{
			base.OnBeginPrint(ev);

			_followUp = _commonCode.FollowUpByClub();
		}

		protected override void OnPrintPage(PrintPageEventArgs ev)
		{
			base.OnPrintPage(ev);
			PreparePrinting(ev);

			PrintPageHeader(ev);
			ev.HasMorePages = PrintPageContent(ev);
		}

		private void PreparePrinting(PrintPageEventArgs ev)
		{
			#region Fonts
			_printFont = new Font("Times New Roman", 12,
				FontStyle.Regular);
			_printAllbergFont = new Font("Arial", 8, FontStyle.Regular);
			_printFontClubHeader = new Font("Times New Roman", 14, FontStyle.Bold);
			#endregion

			#region Margins
			_marginLeft = ev.PageSettings.HardMarginX;
			_marginTop = ev.PageSettings.HardMarginY;

			if (_marginLeft < 70)
				_marginLeft = 70;
			if (_marginTop < 20)
				_marginTop = 20;

			_marginRight = ev.PageSettings.Bounds.Width - _marginLeft;
			_marginBottom = ev.PageSettings.Bounds.Height - _marginTop - ev.Graphics.MeasureString("A", _printAllbergFont).Height;
			#endregion

			#region Columns
			_shooterColumnStart = _marginLeft;
			_payedColumnStart = _shooterColumnStart + ShooterColumnWidth;
			_arrivedColumnStart = _payedColumnStart + PayedColumnWidth;
			_numberOfRoundsColumnStart = _arrivedColumnStart + ArrivedColumnWidth;
			#endregion

			_useShouldHavePayed = _commonCode.CompetitionCurrent.UsePriceMoney;
		}

		readonly string _printingTime = "Utskriven " + DateTime.Now.ToShortDateString() + " " +
				DateTime.Now.ToLongTimeString();
		private void PrintPageHeader(PrintPageEventArgs ev)
		{
			// Print logo
			var image = GetLogo();

			ev.Graphics.DrawImage(
				image,
				_marginRight - image.Width / 4 - 50,
				20,
				image.Width / 4,
				image.Height / 4);

			ev.Graphics.DrawString(
				"WinShooter",
				_printAllbergFont, Brushes.Black,
				_marginLeft, _marginBottom, new StringFormat());

			float x = _marginRight - 
				ev.Graphics.MeasureString(_printingTime, _printAllbergFont).Width;
			ev.Graphics.DrawString(
				_printingTime,
				_printAllbergFont, Brushes.Black,
				x, _marginBottom, new StringFormat());

		}
		private Image GetLogo()
		{
			return _commonCode.Settings.GetResizedLogo(400, 1200);
		}
		#endregion


		#region Printing
		private int _currentIndex;
		private string _lastClubName = "---------";
		/// <summary>
		/// Prints the pages
		/// </summary>
		/// <param name="ev">Printing</param>
		/// <returns>HasMorePages</returns>
		private bool PrintPageContent(PrintPageEventArgs ev)
		{
			var thisIndex = 0;
			float yPos = ev.MarginBounds.Top;
			foreach (var followUp in _followUp)
			{
				if (thisIndex <= _currentIndex & 
					!(thisIndex == 0 & _currentIndex == 0))
				{
					// Do nothing
				}
				else
				{
					if (followUp.ClubName != _lastClubName)
					{
						yPos = PrintClubHeader(ev, followUp.ClubName, yPos);
						_lastClubName = followUp.ClubName;
					}
					yPos = PrintFollowUp(ev, followUp, yPos);
					if (yPos > ev.MarginBounds.Bottom)
					{
						_currentIndex = thisIndex;
						return true;
					}
				}
				thisIndex++;
			}
			return false;
		}

		private float PrintFollowUp(PrintPageEventArgs ev, Structs.FollowUpReturn followUp, float yPos)
		{
			// Print shooter name
			while (ev.Graphics.MeasureString(followUp.ShooterName, _printFont).Width > ShooterColumnWidth)
			{
				followUp.ShooterName = followUp.ShooterName.Substring(0, followUp.ShooterName.Length - 1);
			}
			ev.Graphics.DrawString(followUp.ShooterName, _printFont, Brushes.Black, _shooterColumnStart, yPos);

			// Print payment
			string payedToPrint = followUp.Payed.ToString();
			if (_useShouldHavePayed)
			{
				payedToPrint += " / " + followUp.ShouldHavePayed;
			}
			ev.Graphics.DrawString(payedToPrint, _printFont, Brushes.Black, _payedColumnStart, yPos);


			// Print nr of rounds
			ev.Graphics.DrawString(followUp.ResultsExistForRounds + " / " + 
				followUp.Rounds, 
				_printFont, Brushes.Black, _numberOfRoundsColumnStart, yPos);
			DrawFyrkant(ev, yPos, "Ankommit", _printFont, _arrivedColumnStart, _printFont);
			if (followUp.Arrived)
				FillFyrkant(ev, yPos, "Ankommit", _printFont, _arrivedColumnStart, _printFont);

			return yPos + ev.Graphics.MeasureString(followUp.ShooterName, _printFont).Height;
		}
		private static void DrawFyrkant(PrintPageEventArgs ev, float yPos, string header, Font headerFont, float xLeftPos, Font font)
		{
			var size = ev.Graphics.MeasureString("a", font).Height * FyrkantRatio;
			var xPos = xLeftPos + CalculateFyrkantStart(ev, header, headerFont, size);
			ev.Graphics.DrawLine(new Pen(Brushes.Black), xPos, yPos, xPos, yPos + size);
			ev.Graphics.DrawLine(new Pen(Brushes.Black), xPos, yPos, xPos + size, yPos);
			ev.Graphics.DrawLine(new Pen(Brushes.Black), xPos + size, yPos, xPos + size, yPos + size);
			ev.Graphics.DrawLine(new Pen(Brushes.Black), xPos, yPos + size, xPos + size, yPos + size);
		}
		private static void FillFyrkant(PrintPageEventArgs ev, float yPos, string header, Font headerFont, float xLeftPos, Font font)
		{
			var size = ev.Graphics.MeasureString("a", font).Height * FyrkantRatio;
			var xPos = xLeftPos + CalculateFyrkantStart(ev, header, headerFont, size);
			ev.Graphics.DrawLine(new Pen(Brushes.Black), xPos, yPos, xPos + size, yPos + size);
			ev.Graphics.DrawLine(new Pen(Brushes.Black), xPos + size, yPos, xPos, yPos + size);
		}
		private static float CalculateFyrkantStart(PrintPageEventArgs ev, string header, Font headerFont, float fyrkantsize)
		{
			var textWidth = ev.Graphics.MeasureString(header, headerFont).Width;
			var xPos = textWidth / 2 - fyrkantsize / 2;
			return xPos;
		}
		private float PrintClubHeader(PrintPageEventArgs ev, string clubName, float yPos)
		{
			// Extra space between clubs
			yPos += ev.Graphics.MeasureString(clubName, _printFont).Height;

			// Print club name
			ev.Graphics.DrawString(clubName, _printFontClubHeader, Brushes.Black, _marginLeft, yPos);
			yPos += ev.Graphics.MeasureString(clubName, _printFont).Height;

			// print shooter header
			ev.Graphics.DrawString("Skytt", _printFont, Brushes.Black, _shooterColumnStart, yPos);
			ev.Graphics.DrawString("Betalat", _printFont, Brushes.Black, _payedColumnStart, yPos);
			ev.Graphics.DrawString("Ankommit", _printFont, Brushes.Black, _arrivedColumnStart, yPos);
			ev.Graphics.DrawString("Varv", _printFont, Brushes.Black, _numberOfRoundsColumnStart, yPos);

			// Draw line
			yPos += ev.Graphics.MeasureString(clubName, _printFont).Height;
			ev.Graphics.DrawLine(new Pen(Brushes.Black), _shooterColumnStart, yPos, _numberOfRoundsColumnStart + NumberOfRoundsColumnWidth, yPos);
			yPos += (float)0.1 * ev.Graphics.MeasureString(clubName, _printFont).Height;

			return yPos;
		}
		private float _shooterColumnStart = 40;
		private const float ShooterColumnWidth = 150;
		private float _payedColumnStart = 200;
		private const float PayedColumnWidth = 100;
		private float _arrivedColumnStart = 300;
		private const float ArrivedColumnWidth = 100;
		private float _numberOfRoundsColumnStart = 400;
		private const float NumberOfRoundsColumnWidth = 100;
		private const float FyrkantRatio = (float)0.8;
		bool _useShouldHavePayed = false;
		#endregion
	}
}
