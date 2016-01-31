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
// $Id: CPrintResultlistByPatrol.cs 107 2009-02-01 06:25:33Z smuda $ 
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using Allberg.Shooter.Common;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for CPrintPatrollist.
	/// </summary>
	public class CPrintResultlistByPatrol : PrintDocument 
	{
		private Font printHeaderFont           = null;
		private Font printCompetitorHeaderFont = null;
		private Font printCompetitorFont       = null;
		private Font printAllbergFont          = null;
		private Common.Interface CommonCode;
		private bool printerSupportColor = false;

		// Margins
		private float LeftMargin = 50;
		private float RightMargin = 0;
		// Columns
		private float colLane;
		private float colName;
		private float colClub;
		private float colResult;
		private float colResultMaxWidth;
		private float colPoints;
		private float colResultTot;
		
		private bool NorwegianCount = false;
		private Structs.Patrol patrol;

		Structs.CompetitionTypeEnum CompetitionType;

		internal CPrintResultlistByPatrol(ref Common.Interface newCommon, 
			Structs.Patrol patrolToPrint) : base ()  
		{
			CommonCode = newCommon;
			patrol = patrolToPrint;

			Structs.Competition competition = CommonCode.GetCompetitions()[0];
			CompetitionType =
				competition.Type;
			this.NorwegianCount = competition.NorwegianCount;
		}

		internal bool Color
		{
			set 
			{
				printerSupportColor = value;
			}
			get
			{
				return printerSupportColor;
			}
		}


		#region Init
		//Override OnBeginPrint to set up the font we are going to use
		protected override void OnBeginPrint(PrintEventArgs ev) 
		{
			base.OnBeginPrint(ev) ;
			printHeaderFont = new Font("Arial", 25,System.Drawing.FontStyle.Bold);

			printCompetitorHeaderFont = new Font("Arial", 18, 
				System.Drawing.FontStyle.Bold & 
				System.Drawing.FontStyle.Underline);

			printCompetitorFont = new Font("Arial", 14,
				System.Drawing.FontStyle.Regular);
			printAllbergFont = new Font("Arial", 8,
				System.Drawing.FontStyle.Regular);
		}

		//Override the OnPrintPage to provide the printing logic for the document
		protected override void OnPrintPage(PrintPageEventArgs ev) 
		{
			this.RightMargin = ev.PageBounds.Right - 50;
			this.LeftMargin = 50;
			base.OnPrintPage(ev) ;

			float topMargin = ev.PageBounds.Top + 45; // Org = 25
			float yPos = topMargin;
			float width = this.RightMargin - this.LeftMargin;
			colLane = LeftMargin;
			colName = LeftMargin + width*8/165;
			colClub = LeftMargin + width*45/165;
			colResult = LeftMargin + width*85/165;
			colResultTot = LeftMargin + width*130/165;
			colPoints = LeftMargin + width*142/165;

			colResultMaxWidth = colResultTot;

			int tab = 140;

			printPageHeader(ref ev, ref yPos, tab);
			printPatrol(ref ev, ref yPos, tab);
			/*
			if (individualsPrintDone == false)
			{
				//printIndividual(ev, ref yPos, tab);

				if (ev.HasMorePages == false)
				{
					individualsPrintDone = true;
					if (CommonCode.ResultsGetTeams(this.wclass, this.NorwegianCount).Length> 0)
						ev.HasMorePages = true;
				}
			}
			else
			{
				// TODO Print teams.
				ev.HasMorePages = printTeamResults(ev, ref yPos, tab);
			}*/
		}
		#endregion

		#region Print Patrol Header
		private void printPageHeader(ref PrintPageEventArgs ev, ref float yPos, int tab) 
		{
			string strn;
			switch(CompetitionType)
			{
				case Structs.CompetitionTypeEnum.Field:
					strn = "Resultat patrull ";
					break;
				case Structs.CompetitionTypeEnum.MagnumField:
					strn = "Resultat patrull ";
					break;
				case Structs.CompetitionTypeEnum.Precision:
					strn = "Resultat skjutlag ";
					break;
				default:
					throw new ApplicationException("Unknown CompetitionType");
			}
			ev.Graphics.DrawString(strn + patrol.PatrolId.ToString() +
				" ( " + patrol.StartDateTimeDisplay.ToShortTimeString() + " )", 
				this.printHeaderFont, Brushes.Black, this.LeftMargin, yPos, new StringFormat());
			yPos += this.printCompetitorHeaderFont.GetHeight();

			ev.Graphics.DrawString("Klass: " + patrol.PClass.ToString(), 
				this.printHeaderFont, Brushes.Black, this.LeftMargin, yPos, new StringFormat());
			yPos += this.printHeaderFont.GetHeight();

			yPos += this.printHeaderFont.GetHeight();
		}
		#endregion
		#region Print Patrol
		private void printPatrolHeader(ref PrintPageEventArgs ev, ref float yPos, int tab) 
		{
			ev.Graphics.DrawString("Bana", 
				this.printCompetitorFont, Brushes.Black, this.colLane, yPos, new StringFormat());

			ev.Graphics.DrawString("Namn", 
				this.printCompetitorFont, Brushes.Black, this.colName, yPos, new StringFormat());

			ev.Graphics.DrawString("Klubb", 
				this.printCompetitorFont, Brushes.Black, this.colClub, yPos, new StringFormat());

			ev.Graphics.DrawString("Resultat (per stn)", 
				this.printCompetitorFont, Brushes.Black, this.colResult, yPos, new StringFormat());

			ev.Graphics.DrawString("Resultat", 
				this.printCompetitorFont, Brushes.Black, this.colResultTot, yPos, new StringFormat());

			if (CompetitionType != Structs.CompetitionTypeEnum.Precision)
			{
				ev.Graphics.DrawString("Poäng", 
					this.printCompetitorFont, Brushes.Black, this.colPoints, yPos, new StringFormat());
			}

			yPos += this.printCompetitorFont.GetHeight();

			ev.Graphics.DrawLine(new Pen(Brushes.Black, 2), this.LeftMargin, yPos, this.RightMargin, yPos);
			yPos += 2;
		}
		private void printPatrol(ref PrintPageEventArgs ev, ref float yPos, int tab) 
		{
			printPatrolHeader(ref ev, ref yPos, tab);
			Structs.Competitor[] comps = CommonCode.GetCompetitors(this.patrol, "Lane");

			foreach(Structs.Competitor comp in comps)
			{
				Structs.Shooter shooter = CommonCode.GetShooter(comp.ShooterId);
				Structs.Club club = CommonCode.GetClub(shooter.ClubId);

				ev.Graphics.DrawString(comp.Lane.ToString(), 
					printCompetitorFont, Brushes.Black, this.colLane, yPos, new StringFormat());


				string name = shooter.Givenname + ", " + shooter.Surname;
				while(ev.Graphics.MeasureString(name, printCompetitorFont).Width > (colClub - colName))
				{
					name = name.Substring(0, name.Length-1);
				}
				ev.Graphics.DrawString(name, 
					printCompetitorFont, Brushes.Black, this.colName, yPos, new StringFormat());


				string clubString = club.Name;
				while (ev.Graphics.MeasureString(clubString, printCompetitorFont).Width > (colResult - colClub))
				{
					clubString = clubString.Substring(0, clubString.Length - 1);
				}
				ev.Graphics.DrawString(clubString, 
					printCompetitorFont, Brushes.Black, colClub, yPos, new StringFormat());

				switch(CompetitionType)
				{
					case Structs.CompetitionTypeEnum.Field:
						printCompetitorField(comp, ref ev, ref yPos);
						break;
					case Structs.CompetitionTypeEnum.MagnumField:
						printCompetitorMagnumField(comp, ref ev, ref yPos);
						break;
					case Structs.CompetitionTypeEnum.Precision:
						printCompetitorPrecision(comp, ref ev, ref yPos);
						break;
					default: 
						throw new ApplicationException("Unknown CompetitionType");
				}


			}
		}

		private void printCompetitorField(Structs.Competitor comp, 
			ref PrintPageEventArgs ev, ref float yPos)
		{
			float xPosHitsPerStn = this.colResult;
			int nrOfLines = 1;
			int hits = 0;
			int figureHits = 0;
			int points = 0;
			Structs.CompetitorResult[] compresults = 
				CommonCode.GetCompetitorResults(comp.CompetitorId);

			foreach (Structs.CompetitorResult compres in compresults)
			{
				hits += compres.Hits;
				figureHits += compres.FigureHits;
				points += compres.Points;

				string strn = compres.Hits.ToString() + 
					"/" + compres.FigureHits.ToString();
				if (xPosHitsPerStn + 
					ev.Graphics.MeasureString(strn, printCompetitorFont).Width >
					colResultMaxWidth)
				{
					nrOfLines++;
					xPosHitsPerStn = this.colResult;
				}
				if (this.NorwegianCount &
					strn != "")
				{
					string[] parts = strn.Split('/');
					int thisHits = int.Parse(parts[0]);
					int figures = int.Parse(parts[1]);
					strn = (thisHits + figures).ToString();
				}
				ev.Graphics.DrawString(strn,
					printCompetitorFont, Brushes.Black, xPosHitsPerStn, 
					yPos + (nrOfLines-1) * 
					printCompetitorHeaderFont.GetHeight(),
					new StringFormat());
				xPosHitsPerStn += ev.Graphics.MeasureString(strn + "  ", 
					printCompetitorFont).Width;
			}

			if (!this.NorwegianCount)
			{
				ev.Graphics.DrawString(hits.ToString() + "/" + figureHits.ToString(), 
					printCompetitorFont, Brushes.Black, this.colResultTot, yPos, new StringFormat());
			}
			else
			{
				ev.Graphics.DrawString((hits + figureHits).ToString(), 
					printCompetitorFont, Brushes.Black, this.colResultTot, yPos, new StringFormat());
			}

			ev.Graphics.DrawString(points.ToString(), 
				printCompetitorFont, Brushes.Black, this.colPoints, yPos, new StringFormat());

			yPos += nrOfLines * this.printCompetitorFont.GetHeight();
		}

		private void printCompetitorMagnumField(Structs.Competitor comp, 
			ref PrintPageEventArgs ev, ref float yPos)
		{
			float xPosHitsPerStn = this.colResult;
			int nrOfLines = 1;
			int hits = 0;
			int figureHits = 0;
			int points = 0;
			Structs.CompetitorResult[] compresults = 
				CommonCode.GetCompetitorResults(comp.CompetitorId);

			foreach (Structs.CompetitorResult compres in compresults)
			{
				hits += compres.Hits;
				figureHits += compres.FigureHits;
				points += compres.Points;

				string strn = compres.Hits.ToString() + 
					"/" + compres.FigureHits.ToString();
				if (xPosHitsPerStn + 
					ev.Graphics.MeasureString(strn, printCompetitorFont).Width >
					colResultMaxWidth)
				{
					nrOfLines++;
					xPosHitsPerStn = this.colResult;
				}

				ev.Graphics.DrawString(strn,
					printCompetitorFont, Brushes.Black, xPosHitsPerStn, 
					yPos + (nrOfLines-1) * 
					printCompetitorHeaderFont.GetHeight(),
					new StringFormat());
				xPosHitsPerStn += ev.Graphics.MeasureString(strn + "  ", 
					printCompetitorFont).Width;
			}

			ev.Graphics.DrawString(hits.ToString() + "/" + figureHits.ToString(), 
				printCompetitorFont, Brushes.Black, this.colResultTot, yPos, new StringFormat());

			ev.Graphics.DrawString(points.ToString(), 
				printCompetitorFont, Brushes.Black, this.colPoints, yPos, new StringFormat());

			yPos += nrOfLines * this.printCompetitorFont.GetHeight();
		}

		private void printCompetitorPrecision(Structs.Competitor comp, 
			ref PrintPageEventArgs ev, ref float yPos)
		{
			float xPosHitsPerStn = this.colResult;
			int nrOfLines = 1;
			int hits = 0;

			Structs.CompetitorResult[] compresults = 
				CommonCode.GetCompetitorResults(comp.CompetitorId);

			foreach (Structs.CompetitorResult compres in compresults)
			{
				hits += compres.Hits;

				string strn = compres.Hits.ToString();
				if (xPosHitsPerStn + 
					ev.Graphics.MeasureString(strn, printCompetitorFont).Width >
					colResultMaxWidth)
				{
					nrOfLines++;
					xPosHitsPerStn = this.colResult;
				}
 
				ev.Graphics.DrawString(strn,
					printCompetitorFont, Brushes.Black, xPosHitsPerStn, 
					yPos + (nrOfLines-1) * 
					printCompetitorHeaderFont.GetHeight(),
					new StringFormat());
				xPosHitsPerStn += ev.Graphics.MeasureString(strn + "  ", 
					printCompetitorFont).Width;
			}

			ev.Graphics.DrawString(hits.ToString(), 
				printCompetitorFont, Brushes.Black, this.colResultTot, yPos, new StringFormat());

			yPos += nrOfLines * this.printCompetitorFont.GetHeight();
		}
		#endregion
	}
}
