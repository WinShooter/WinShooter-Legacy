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
// $Id: CPrintPatrollistByPatrol.cs 107 2009-02-01 06:25:33Z smuda $ 
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;
using Allberg.Shooter.Common;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for CPrintPatrollistByPatrol.
	/// </summary>
	internal class CPrintPatrollistByPatrol : PrintDocument 
	{
		private Font printHeaderFont           = null;
		private Font printCompetitorHeaderFont = null;
		private Font printCompetitorFont       = null;
		private Font printAllbergFont          = null;
		private Font printCompetitorFontBold   = null;
		private Common.Interface CommonCode;
		private int currentPatrol = 0;

		internal CPrintPatrollistByPatrol(ref Common.Interface newCommon) : base ()  
		{
			CommonCode = newCommon;
			currentPatrol = CommonCode.GetPatrols()[0].PatrolId;
			competition = CommonCode.GetCompetitions()[0];
		}

		Structs.Competition competition;
		private int maxLanes = 8;
		internal int MaxLanes
		{
			set
			{
				maxLanes = value;
			}
			get
			{
				return maxLanes;
			}
		}

		private string club = "";
		internal string Club
		{
			set
			{
				club = value;
			}
			get
			{
				return club;
			}
		}

		private int patrolIdToPrint = -1;
		internal int PatrolIdToPrint
		{
			set
			{
				patrolIdToPrint = value;
				if (value != -1)
				{
					this.currentPatrol = value;
				}
			}
			get
			{
				return patrolIdToPrint;
			}
		}


		//Override OnBeginPrint to set up the font we are going to use
		protected override void OnBeginPrint(PrintEventArgs ev) 
		{
			base.OnBeginPrint(ev) ;
			printHeaderFont = new Font("Arial", 14,System.Drawing.FontStyle.Bold);

			printCompetitorHeaderFont = new Font("Arial", 12,
				System.Drawing.FontStyle.Bold &
				System.Drawing.FontStyle.Underline);

			printAllbergFont = new Font("Arial", 8,
				System.Drawing.FontStyle.Bold &
				System.Drawing.FontStyle.Underline);

			printCompetitorFont = new Font("Arial", 10, System.Drawing.FontStyle.Regular);
			printCompetitorFontBold = new Font("Arial", 10,System.Drawing.FontStyle.Bold);
		}

		//Override the OnPrintPage to provide the printing logic for the document
		protected override void OnPrintPage(PrintPageEventArgs ev) 
		{
			base.OnPrintPage(ev) ;

			//Work out the number of lines per page 
			//Use the MarginBounds on the event to do this 
			//lpp = ev.MarginBounds.Height  / printFont.GetHeight(ev.Graphics) ;
			float topMargin = ev.MarginBounds.Top;
			topMargin = 25;
			float yPos = topMargin;
			leftMargin = base.DefaultPageSettings.HardMarginX;
			if (leftMargin < 20)
				leftMargin = 20;

			//Now print what we want to print
			Structs.Patrol patrol = CommonCode.GetPatrol(this.currentPatrol);
			printHeader(ev, patrol, ref yPos);
			yPos += 20;
			printPatrol(ev, patrol, ref yPos, maxLanes);

			// Increase current control for the next one
			

			//If we have more patrols then print another page
			if (this.patrolIdToPrint == -1)
			{
				if (CommonCode.GetPatrolsCount() >= this.currentPatrol)
					ev.HasMorePages = true;
				else
					ev.HasMorePages = false;
			}
			else
			{
				if (this.currentPatrol == this.patrolIdToPrint)
					ev.HasMorePages = true;
				else
					ev.HasMorePages = false;
			}
		}

		float leftMargin = 50;
		
		#region Header
		private void printHeader(PrintPageEventArgs ev, 
			Structs.Patrol patrol, ref float yPos)
		{
			//leftMargin = ev.MarginBounds.Left;
			int tab = 100;

			// Print RegistrationsInfo
			ev.Graphics.DrawString(
				"WinShooter",
				printAllbergFont, Brushes.Black,
				leftMargin, ev.PageBounds.Size.Height -2*printHeaderFont.GetHeight()-20, new StringFormat());
			/*ev.Graphics.DrawString("©John Allberg",
				printAllbergFont, Brushes.Black, ev.PageBounds.Right-180, 
				ev.PageBounds.Size.Height -2*printHeaderFont.GetHeight()-20,
				new StringFormat());*/

			// Print logo
			System.Drawing.Image image = getLogo();

			ev.Graphics.DrawImage(
				image,
				ev.MarginBounds.Right-image.Width, 
				20, 
				image.Width, 
				image.Height);

			// Print patrol header
			ev.Graphics.DrawString("Tävling: ", 
				printHeaderFont, Brushes.Black, leftMargin,	yPos, 
				new StringFormat());
			ev.Graphics.DrawString(CommonCode.GetCompetitions()[0].Name, 
				printHeaderFont, Brushes.Black, leftMargin+tab, yPos, 
				new StringFormat());
			yPos += printHeaderFont.GetHeight();

			string patrolHeader = "Patrull: ";
			switch (CommonCode.CompetitionCurrent.Type)
			{
				case Structs.CompetitionTypeEnum.Field:
					patrolHeader = "Patrull: ";
					break;
				case Structs.CompetitionTypeEnum.MagnumField:
					patrolHeader = "Patrull";
					break;
				case Structs.CompetitionTypeEnum.Precision:
					patrolHeader = "Lag: ";
					break;
				default:
					throw new ApplicationException("Unkown CompetitionType");
			}
			ev.Graphics.DrawString(patrolHeader, 
				printHeaderFont, Brushes.Black, leftMargin,	yPos, 
				new StringFormat());
			ev.Graphics.DrawString(patrol.PatrolId.ToString(), 
				printHeaderFont, Brushes.Black, leftMargin+tab, yPos, 
				new StringFormat());
			yPos += printHeaderFont.GetHeight();

			ev.Graphics.DrawString("Starttid: ", 
				printHeaderFont, Brushes.Black, leftMargin, yPos, 
				new StringFormat());
			ev.Graphics.DrawString(patrol.StartDateTimeDisplay.ToShortTimeString(), 
				printHeaderFont, Brushes.Black, leftMargin+tab, yPos, 
				new StringFormat());
			yPos += printHeaderFont.GetHeight();
		}

		private System.Drawing.Image getLogo()
		{
			return CommonCode.Settings.GetResizedLogo(80, 400);
		}

		#endregion

		#region Patrol
		private float placeNameX;
		//private float placeClubX;
		private float placeLeftHeaderX;
		private int lastPrintedLane = 0;
		private void printPatrol(PrintPageEventArgs ev, Structs.Patrol patrol, ref float yPos, int maxLanes)
		{
			placeLeftHeaderX = ev.PageBounds.Left + 10;
			placeLeftHeaderX = leftMargin;

			//placeClubX = (ev.PageBounds.Right - ev.PageBounds.Left)/2 + ev.PageBounds.Left;
			placeNameX = 80 + ev.PageBounds.Left;
			placeNameX = placeLeftHeaderX  + 70;

			// Print competitorheader
			//ev.Graphics.DrawString("Namn", 
			//	printCompetitorHeaderFont, Brushes.Black, placeNameX, yPos, 
			//	new StringFormat());

			//yPos += printHeaderFont.GetHeight();

			// Print competitors
			for(int thisLane=lastPrintedLane+1;thisLane<=maxLanes;thisLane++)
			{
				bool laneFound = false;
				lastPrintedLane = thisLane;
				foreach(Structs.Competitor comp in CommonCode.GetCompetitors(patrol, "Lane"))
				{
					if (comp.Lane == thisLane)
					{
						laneFound = true;
						printThisCompetitor(ev, comp, ref yPos);
						yPos += this.printCompetitorFont.GetHeight()/2;
					}
				}
				if (!laneFound)
				{
					printEmptyCompetitor(ev, ref yPos, thisLane);
					yPos += this.printCompetitorFont.GetHeight()/2;
				}

				if (yPos + heightOfOneCompetitor > (ev.PageBounds.Height-2*printHeaderFont.GetHeight()-20) &
					thisLane < maxLanes)
				{
					return;
				}
			}
			currentPatrol++;
			lastPrintedLane = 0;
		}
		
		private void printThisCompetitor(PrintPageEventArgs ev, 
			Structs.Competitor comp, ref float yPos)
		{
			Structs.Shooter shooter = CommonCode.GetShooter(comp.ShooterId);
			Structs.Club club = CommonCode.GetClub(shooter.ClubId);
			Structs.Weapon weapon = CommonCode.GetWeapon(comp.WeaponId);

			// Print name
			string shooterName = shooter.Surname + " " + shooter.Givenname +
				", " + club.Name;
			ev.Graphics.DrawString(shooterName,
				printCompetitorFont, Brushes.Black, placeNameX, yPos, 
				new StringFormat());

			// Print Club
			/*ev.Graphics.DrawString(club.Name,
				printCompetitorFont, Brushes.Black, placeClubX, yPos, 
				new StringFormat());*/

			// Print weaponsgroup
			ev.Graphics.DrawString(weapon.WClass.ToString(),
				printCompetitorFont, Brushes.Black, placeLeftHeaderX, yPos + 2*printCompetitorFont.Height, 
				new StringFormat());

			// Print shooterclass
			Structs.ShootersClassShort sclassshort = (Structs.ShootersClassShort)
				(int)comp.ShooterClass;
			ev.Graphics.DrawString(sclassshort.ToString(),
				printCompetitorFont, Brushes.Black, placeLeftHeaderX, yPos + 4*printCompetitorFont.Height, 
				new StringFormat());		

			// Print weapon
			if (comp.WeaponId.ToLower().IndexOf("unknown") == -1)
			{
				ev.Graphics.DrawString(comp.WeaponId.Replace("Unknown", ""),
					printCompetitorFont, Brushes.Black, placeLeftHeaderX, yPos + 6*printCompetitorFont.Height, 
					new StringFormat());
			}

			// Draw "arrived" checkbox
			float size = printCompetitorFont.Height;
			float left = placeNameX + ev.Graphics.MeasureString(shooterName, printCompetitorFont).Width + 10;
			float height = yPos;

			ev.Graphics.DrawString("Ankommit",
				printCompetitorFont, Brushes.Black, left + size, height,
				new StringFormat());
			Pen pen = new Pen(Brushes.Black, 2);
			ev.Graphics.DrawLine(pen, left, height,
				left + size, height);
			ev.Graphics.DrawLine(pen, left, height + size,
				left + size, height + size);
			ev.Graphics.DrawLine(pen, left, height,
				left, height + size);
			ev.Graphics.DrawLine(pen, left + size, height,
				left + size, height + size);
			if (shooter.Arrived)
			{
				ev.Graphics.DrawLine(pen, left, height,
					left + size, height + size);
				ev.Graphics.DrawLine(pen, left + size, height,
					left, height + size);
			}

			printEmptyCompetitor(ev, ref yPos, comp.Lane);
		}

		private void printEmptyCompetitor(PrintPageEventArgs ev, 
			ref float yPos, int thisLane)
		{
			string laneString = "";
			switch (competition.Type)
			{
				case Structs.CompetitionTypeEnum.Field:
					laneString = "Figur ";
					break;
				case Structs.CompetitionTypeEnum.MagnumField:
					laneString = "Figur ";
					break;
				case Structs.CompetitionTypeEnum.Precision:
					laneString = "Tavla ";
					break;
			}

			// Print lane
			ev.Graphics.DrawString(laneString + thisLane.ToString(),
				printCompetitorFontBold, Brushes.Black, placeLeftHeaderX, yPos, 
				new StringFormat());

			// Print Weaponsgroup header
			ev.Graphics.DrawString("Vgrp:",
				printCompetitorFontBold, Brushes.Black, placeLeftHeaderX, yPos + printCompetitorFont.Height, 
				new StringFormat());

			// Print Weaponsgroup header
			ev.Graphics.DrawString("Klass:",
				printCompetitorFontBold, Brushes.Black, placeLeftHeaderX, yPos + 3*printCompetitorFont.Height, 
				new StringFormat());

			// Print Weaponsgroup header
			ev.Graphics.DrawString("Vapen:",
				printCompetitorFontBold, Brushes.Black, placeLeftHeaderX, yPos + 5*printCompetitorFont.Height, 
				new StringFormat());

			// Print the stations
			System.Drawing.Pen pen = new Pen(Brushes.Black, 2);
			float thisY = yPos + 3*printCompetitorFont.Height/2;
			float thisX = placeNameX;
			float maxX = 0;
			float rectangleXSize = 3* printCompetitorFont.Height; // Adjust
			float rectangleYSize = 2* printCompetitorFont.Height;
			bool anyStationHasPoints = false;
			for(int currentStation=1;
				currentStation<=CommonCode.GetStationsCount();
				currentStation++)
			{
				maxX = thisX + rectangleXSize;
				
				// Print station nr
				ev.Graphics.DrawString(currentStation.ToString(),
					printCompetitorFont, Brushes.Black, 
					thisX + rectangleXSize/2-10, thisY-printCompetitorFont.Height/2, 
					new StringFormat());

				// Top lines
				ev.Graphics.DrawLine(pen, 
					new PointF(thisX, thisY), 
					new PointF(thisX+rectangleXSize/2-12, thisY));

				ev.Graphics.DrawLine(pen, 
					new PointF(thisX+rectangleXSize, thisY), 
					new PointF(thisX+rectangleXSize-(rectangleXSize/2-12), thisY));

				// Övre strecket
				//ev.Graphics.DrawLine(pen, 
				//	new PointF(thisX, thisY), 
				//	new PointF(thisX, thisY+rectangleYSize));

				int nrOfFiguresInThisStation = 0;
				bool pointsInThisStation = false;
				foreach(Structs.Station station in CommonCode.GetStations())
				{
					if (station.StationNr == currentStation)
					{
						nrOfFiguresInThisStation = station.Figures;
						pointsInThisStation = station.Points;
					}
				}

				if (pointsInThisStation)
					anyStationHasPoints = true;

				if (nrOfFiguresInThisStation > 0)
					printResultRectangeles(ev, pen, thisX, thisY, 
						rectangleXSize, rectangleYSize, nrOfFiguresInThisStation, 
						pointsInThisStation);
				else
					throw new ApplicationException(
						"Could not find station " + currentStation.ToString());

				thisX += rectangleXSize;
			}

			heightOfOneCompetitor = 7*printCompetitorFont.Height + 3*printCompetitorFont.Height/2;
			if (anyStationHasPoints)
				heightOfOneCompetitor += printCompetitorFont.Height;

			yPos += heightOfOneCompetitor ;

			// Draw line beneith competitor
			Pen underLinePen = new Pen(Brushes.Black, 2);
			underLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
			ev.Graphics.DrawLine(underLinePen,
				new PointF(leftMargin, yPos + 2 ),
				new PointF(maxX, yPos + 2));
		}
		float heightOfOneCompetitor = 0;
		private void printResultRectangeles(PrintPageEventArgs ev, System.Drawing.Pen pen, float xPos, 
			float yPos, float rectangleXSize, float rectangleYSize, 
			int nrOfFigures, bool pointsInThisStation)
		{
			// Vänstra övre strecket
			ev.Graphics.DrawLine(pen, 
				new PointF(xPos, yPos), 
				new PointF(xPos, yPos+rectangleYSize));

			// Högra övre strecket
			ev.Graphics.DrawLine(pen, 
				new PointF(xPos+rectangleXSize, yPos), 
				new PointF(xPos+rectangleXSize, yPos+rectangleYSize));

			// övre inre strecket
			ev.Graphics.DrawLine(pen, 
				new PointF(xPos, yPos+rectangleYSize), 
				new PointF(xPos+rectangleXSize, yPos+rectangleYSize));

			if (nrOfFigures>1)
			{
				printResultRectangle(ev, pen, xPos, yPos+rectangleYSize, 
					rectangleXSize, rectangleYSize);
			}
			if (nrOfFigures>2)
			{
				printResultRectangle(ev, pen, xPos, yPos+rectangleYSize*2,
					rectangleXSize, rectangleYSize);
			}
			switch(nrOfFigures)
			{
				case 4:
					ev.Graphics.DrawLine(pen, 
						new PointF(xPos + rectangleXSize/2, yPos+1*rectangleYSize), 
						new PointF(xPos + rectangleXSize/2, yPos+2*rectangleYSize));
					break;
				case 5:
					ev.Graphics.DrawLine(pen, 
						new PointF(xPos + rectangleXSize/2, yPos+0*rectangleYSize), 
						new PointF(xPos + rectangleXSize/2, yPos+1*rectangleYSize));
					ev.Graphics.DrawLine(pen, 
						new PointF(xPos + rectangleXSize/2, yPos+2*rectangleYSize), 
						new PointF(xPos + rectangleXSize/2, yPos+3*rectangleYSize));
					break;
				case 6:
					ev.Graphics.DrawLine(pen, 
						new PointF(xPos + rectangleXSize/2, yPos+0*rectangleYSize), 
						new PointF(xPos + rectangleXSize/2, yPos+1*rectangleYSize));
					ev.Graphics.DrawLine(pen, 
						new PointF(xPos + rectangleXSize/2, yPos+1*rectangleYSize), 
						new PointF(xPos + rectangleXSize/2, yPos+2*rectangleYSize));
					ev.Graphics.DrawLine(pen, 
						new PointF(xPos + rectangleXSize/2, yPos+2*rectangleYSize), 
						new PointF(xPos + rectangleXSize/2, yPos+3*rectangleYSize));
					break;
			}

			if (pointsInThisStation)
			{
				printResultRectangle(ev, pen, xPos, yPos+rectangleYSize*3,
					rectangleXSize, rectangleYSize);

				ev.Graphics.DrawLine(pen, 
					new PointF(xPos, yPos+3*rectangleYSize), 
					new PointF(xPos + rectangleXSize, yPos+3*rectangleYSize));
			}
		}
		private void printResultRectangle(PrintPageEventArgs ev, System.Drawing.Pen pen, float xPos, 
			float yPos, float rectangleXSize, float rectangleYSize)
		{
			// Vänstra övre strecket
			ev.Graphics.DrawLine(pen, 
				new PointF(xPos, yPos), 
				new PointF(xPos, yPos+rectangleYSize));

			// Högra övre strecket
			ev.Graphics.DrawLine(pen, 
				new PointF(xPos+rectangleXSize, yPos), 
				new PointF(xPos+rectangleXSize, yPos+rectangleYSize));

			// övre inre strecket
			ev.Graphics.DrawLine(pen, 
				new PointF(xPos, yPos+rectangleYSize), 
				new PointF(xPos+rectangleXSize, yPos+rectangleYSize));
		}
		#endregion
	}
}
