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
// $Id: CPrintPatrollistByClub.cs 107 2009-02-01 06:25:33Z smuda $ 
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;
using Allberg.Shooter.Common;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for CPrintPatrollistByUser.
	/// </summary>
	internal class CPrintPatrollistByClub : PrintDocument 
	{
		private Font printHeaderFont           = null;
		private Font printCompetitorFont       = null;
		private Font printAllbergFont          = null;
		private Common.Interface CommonCode;
		private int currentShooter = 0;
		private float LeftMargin = 50;
		//private float RightMargin = 50;

		internal CPrintPatrollistByClub(ref Common.Interface newCommon) : base ()  
		{
			CommonCode = newCommon;
		}

		
		private string clubIdToPrint = "-1";
		internal string ClubIdToPrint
		{
			set
			{
				clubIdToPrint = value;
			}
			get
			{
				return clubIdToPrint;
			}
		}

		//Override OnBeginPrint to set up the font we are going to use
		protected override void OnBeginPrint(PrintEventArgs ev) 
		{
			base.OnBeginPrint(ev) ;
			printHeaderFont = new Font("Arial", 14,System.Drawing.FontStyle.Bold);

			printCompetitorFont = new Font("Arial", 10, System.Drawing.FontStyle.Regular);
			printAllbergFont = new Font("Arial", 8, System.Drawing.FontStyle.Regular);

			if (clubIdToPrint == "-1")
				throw new ApplicationException("No club to print is specified");
		}

		//Override the OnPrintPage to provide the printing logic for the document
		protected override void OnPrintPage(PrintPageEventArgs ev) 
		{
			base.OnPrintPage(ev) ;
			LeftMargin = ev.MarginBounds.Left;

			//Work out the number of lines per page 
			//Use the MarginBounds on the event to do this 
			//lpp = ev.MarginBounds.Height  / printFont.GetHeight(ev.Graphics) ;
			float topMargin = ev.MarginBounds.Top;
			topMargin = 25;
			float yPos = topMargin;

			//Now print what we want to print
			Structs.Club club = CommonCode.GetClub(this.clubIdToPrint);
			Structs.Shooter shooter = CommonCode.GetShooters(club)[this.currentShooter];
			printHeader(ev, shooter, ref yPos);
			printShooter(ev, shooter, ref yPos);

			//If we have more patrols then print another page
			if (this.currentShooter < CommonCode.GetShooters(club).Length)
				ev.HasMorePages = true;
			else
				ev.HasMorePages = false;
		}

		#region Header
		private void printHeader(PrintPageEventArgs ev, 
			Structs.Shooter shooter, ref float yPos)
		{
			int tab = 125;

			// Print RegistrationsInfo
			ev.Graphics.DrawString(
				"WinShooter",
				printAllbergFont, Brushes.Black,
				ev.MarginBounds.Left, ev.PageBounds.Size.Height -2*printHeaderFont.GetHeight()-20, new StringFormat());
			/*ev.Graphics.DrawString("©John Allberg",
				printHeaderFont, Brushes.Black, ev.PageBounds.Right-180, 
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
				printHeaderFont, Brushes.Black, LeftMargin,	yPos, 
				new StringFormat());
			ev.Graphics.DrawString(CommonCode.GetCompetitions()[0].Name, 
				printHeaderFont, Brushes.Black, LeftMargin+tab, yPos, 
				new StringFormat());
			yPos += printHeaderFont.GetHeight();

			ev.Graphics.DrawString("Namn: ", 
				printHeaderFont, Brushes.Black, LeftMargin,	yPos, 
				new StringFormat());
			ev.Graphics.DrawString(shooter.Surname + " " + shooter.Givenname, 
				printHeaderFont, Brushes.Black, LeftMargin+tab, yPos, 
				new StringFormat());
			yPos += printHeaderFont.GetHeight();

			ev.Graphics.DrawString("Skyttekort: ", 
				printHeaderFont, Brushes.Black, LeftMargin, yPos, 
				new StringFormat());
			ev.Graphics.DrawString(shooter.CardNr, 
				printHeaderFont, Brushes.Black, LeftMargin+tab, yPos, 
				new StringFormat());
			yPos += printHeaderFont.GetHeight();

			ev.Graphics.DrawString("Skytteklubb: ", 
				printHeaderFont, Brushes.Black, LeftMargin, yPos, 
				new StringFormat());
			Structs.Club club = CommonCode.GetClub(shooter.ClubId);
			ev.Graphics.DrawString(club.Name, 
				printHeaderFont, Brushes.Black, LeftMargin+tab, yPos, 
				new StringFormat());
			yPos += printHeaderFont.GetHeight();
			yPos += printHeaderFont.GetHeight();
			yPos += printHeaderFont.GetHeight();

			ev.Graphics.DrawString("Starttid", 
				printHeaderFont, Brushes.Black, LeftMargin + tab1, yPos, 
				new StringFormat());
			ev.Graphics.DrawString("Patrull", 
				printHeaderFont, Brushes.Black, LeftMargin + tab2, yPos, 
				new StringFormat());
			ev.Graphics.DrawString("Vapen", 
				printHeaderFont, Brushes.Black, LeftMargin + tab3, yPos, 
				new StringFormat());
			yPos += printHeaderFont.GetHeight();
		}

		private float tab1 = 0;
		private float tab2 = 100;
		private float tab3 = 200;


		private System.Drawing.Image getLogo()
		{
			return CommonCode.Settings.GetResizedLogo(100, 300);
		}

		#endregion

		#region Shooter
		private void printShooter(PrintPageEventArgs ev, 
			Structs.Shooter shooter, ref float yPos)
		{
			Structs.Competitor[] competitors =
				CommonCode.GetCompetitors(shooter.ShooterId, "PatrolId");

			foreach(Structs.Competitor comp in competitors)
			{
				if (comp.PatrolId > 0)
				{
					Structs.Patrol patrol = CommonCode.GetPatrol(comp.PatrolId);
					ev.Graphics.DrawString(patrol.StartDateTimeDisplay.ToShortTimeString(), 
						printCompetitorFont, Brushes.Black, LeftMargin + tab1, yPos, 
						new StringFormat());

					ev.Graphics.DrawString(patrol.PatrolId.ToString(), 
						printCompetitorFont, Brushes.Black, LeftMargin + tab2, yPos, 
						new StringFormat());
				}
				Structs.Weapon weapon = CommonCode.GetWeapon(comp.WeaponId);
				ev.Graphics.DrawString(weapon.Manufacturer + ", " + 
					weapon.Model + "(" + comp.WeaponId + ")", 
					printCompetitorFont, Brushes.Black, LeftMargin + tab3, yPos, 
					new StringFormat());
				yPos += printHeaderFont.GetHeight();
			}

			currentShooter++;
		}

		#endregion

	}
}
