// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CPrintPatrollistByUser.cs" company="John Allberg">
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Threading;

    using Allberg.Shooter.Common;
    using Allberg.Shooter.Common.Exceptions;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for CPrintPatrollistByUser.
    /// </summary>
    internal class CPrintPatrollistByUser : PrintDocument
    {
        /// <summary>
        /// The print header font.
        /// </summary>
        private Font printHeaderFont = null;

        /// <summary>
        /// The print competitor font.
        /// </summary>
        private Font printCompetitorFont = null;

        /// <summary>
        /// The print allberg font.
        /// </summary>
        private Font printAllbergFont = null;

        /// <summary>
        /// The common code.
        /// </summary>
        private Interface CommonCode;

        /// <summary>
        /// The current shooter.
        /// </summary>
        private int currentShooter = 0;

        /// <summary>
        /// The left margin.
        /// </summary>
        private float LeftMargin = 50;

        // private float RightMargin = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="CPrintPatrollistByUser"/> class.
        /// </summary>
        /// <param name="newCommon">
        /// The new common.
        /// </param>
        internal CPrintPatrollistByUser(ref Interface newCommon)
            : base()
        {
            CommonCode = newCommon;
        }

        /// <summary>
        /// The shooter id to print.
        /// </summary>
        private int shooterIdToPrint = -1;

        /// <summary>
        /// Gets or sets the shooter id to print.
        /// </summary>
        internal int ShooterIdToPrint
        {
            get
            {
                return shooterIdToPrint;
            }

            set
            {
                this.shooterIdToPrint = value;
            }
        }

        // Override OnBeginPrint to set up the font we are going to use
        /// <summary>
        /// The on begin print.
        /// </summary>
        /// <param name="ev">
        /// The ev.
        /// </param>
        protected override void OnBeginPrint(PrintEventArgs ev)
        {
            base.OnBeginPrint(ev);
            printHeaderFont = new Font("Arial", 14, FontStyle.Bold);

            printCompetitorFont = new Font("Arial", 10, FontStyle.Regular);
            printAllbergFont = new Font("Arial", 8, FontStyle.Regular);
        }

        // Override the OnPrintPage to provide the printing logic for the document
        /// <summary>
        /// The on print page.
        /// </summary>
        /// <param name="ev">
        /// The ev.
        /// </param>
        protected override void OnPrintPage(PrintPageEventArgs ev)
        {
            Trace.WriteLine(
                "CPrintPatrollistByUser: OnPrintPage started" + " on thread \"" + Thread.CurrentThread.Name + "\" ( "
                + Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            base.OnPrintPage(ev);

            // Work out the number of lines per page 
            // Use the MarginBounds on the event to do this 
            // lpp = ev.MarginBounds.Height  / printFont.GetHeight(ev.Graphics) ;
            float topMargin = ev.MarginBounds.Top;
            topMargin = 25;
            float yPos = topMargin;

            // Now print what we want to print
            Structs.Shooter shooter = new Structs.Shooter();
            if (ShooterIdToPrint != -1)
            {
                shooter = CommonCode.GetShooter(ShooterIdToPrint);
            }
            else
            {
                string temp = null;
                while (temp == null & CommonCode.GetShooterHighestId() >= currentShooter)
                {
                    try
                    {
                        shooter = CommonCode.GetShooter(this.currentShooter);
                        temp = "found shooter";
                    }
                    catch (CannotFindIdException exc)
                    {
                        Trace.WriteLine("CPrintPatrollistByUser: OnPrintPage Exception " + exc.ToString());
                        temp = null;
                        currentShooter++;
                    }
                }
            }

            printHeader(ev, shooter, ref yPos);
            printShooter(ev, shooter, ref yPos);

            // If we have more patrols then print another page
            if (this.shooterIdToPrint == -1)
            {
                if (CommonCode.GetShooterHighestId() >= this.currentShooter)
                {
                    ev.HasMorePages = true;
                }
                else
                {
                    ev.HasMorePages = false;
                }
            }
            else
            {
                ev.HasMorePages = false;
            }
        }

        #region Header

        /// <summary>
        /// The print header.
        /// </summary>
        /// <param name="ev">
        /// The ev.
        /// </param>
        /// <param name="shooter">
        /// The shooter.
        /// </param>
        /// <param name="yPos">
        /// The y pos.
        /// </param>
        private void printHeader(PrintPageEventArgs ev, Structs.Shooter shooter, ref float yPos)
        {
            int tab = 125;

            // Print RegistrationsInfo
            ev.Graphics.DrawString(
                "WinShooter", 
                printAllbergFont, 
                Brushes.Black, 
                LeftMargin, 
                ev.PageBounds.Size.Height - 2 * printHeaderFont.GetHeight() - 20, 
                new StringFormat());

            /*ev.Graphics.DrawString("©John Allberg",
                printHeaderFont, Brushes.Black, ev.PageBounds.Right-180, 
                ev.PageBounds.Size.Height -2*printHeaderFont.GetHeight()-20,
                new StringFormat());*/

            // Print logo
            Image image = getLogo();

            ev.Graphics.DrawImage(image, ev.MarginBounds.Right - image.Width / 4, 20, image.Width / 4, image.Height / 4);

            // Print patrol header
            ev.Graphics.DrawString("Tävling: ", printHeaderFont, Brushes.Black, LeftMargin, yPos, new StringFormat());
            ev.Graphics.DrawString(
                CommonCode.GetCompetitions()[0].Name, 
                printHeaderFont, 
                Brushes.Black, 
                LeftMargin + tab, 
                yPos, 
                new StringFormat());
            yPos += printHeaderFont.GetHeight();

            ev.Graphics.DrawString("Namn: ", printHeaderFont, Brushes.Black, LeftMargin, yPos, new StringFormat());
            ev.Graphics.DrawString(
                shooter.Surname + " " + shooter.Givenname, 
                printHeaderFont, 
                Brushes.Black, 
                LeftMargin + tab, 
                yPos, 
                new StringFormat());
            yPos += printHeaderFont.GetHeight();

            ev.Graphics.DrawString("Skyttekort: ", printHeaderFont, Brushes.Black, LeftMargin, yPos, new StringFormat());
            ev.Graphics.DrawString(
                shooter.CardNr, 
                printHeaderFont, 
                Brushes.Black, 
                LeftMargin + tab, 
                yPos, 
                new StringFormat());
            yPos += printHeaderFont.GetHeight();

            ev.Graphics.DrawString(
                "Skytteklubb: ", 
                printHeaderFont, 
                Brushes.Black, 
                LeftMargin, 
                yPos, 
                new StringFormat());
            Structs.Club club = CommonCode.GetClub(shooter.ClubId);
            ev.Graphics.DrawString(
                club.Name, 
                printHeaderFont, 
                Brushes.Black, 
                LeftMargin + tab, 
                yPos, 
                new StringFormat());
            yPos += printHeaderFont.GetHeight();
            yPos += printHeaderFont.GetHeight();
            yPos += printHeaderFont.GetHeight();

            ev.Graphics.DrawString(
                "Starttid", 
                printHeaderFont, 
                Brushes.Black, 
                LeftMargin + tab1, 
                yPos, 
                new StringFormat());
            ev.Graphics.DrawString(
                "Patrull", 
                printHeaderFont, 
                Brushes.Black, 
                LeftMargin + tab2, 
                yPos, 
                new StringFormat());
            ev.Graphics.DrawString("Vapen", printHeaderFont, Brushes.Black, LeftMargin + tab3, yPos, new StringFormat());
            yPos += printHeaderFont.GetHeight();
        }

        /// <summary>
        /// The tab 1.
        /// </summary>
        private float tab1 = 0;

        /// <summary>
        /// The tab 2.
        /// </summary>
        private float tab2 = 100;

        /// <summary>
        /// The tab 3.
        /// </summary>
        private float tab3 = 200;

        /// <summary>
        /// The get logo.
        /// </summary>
        /// <returns>
        /// The <see cref="Image"/>.
        /// </returns>
        private Image getLogo()
        {
            return CommonCode.Settings.GetResizedLogo(400, 1200);
        }

        #endregion

        #region Shooter

        /// <summary>
        /// The print shooter.
        /// </summary>
        /// <param name="ev">
        /// The ev.
        /// </param>
        /// <param name="shooter">
        /// The shooter.
        /// </param>
        /// <param name="yPos">
        /// The y pos.
        /// </param>
        private void printShooter(PrintPageEventArgs ev, Structs.Shooter shooter, ref float yPos)
        {
            Structs.Competitor[] competitors = CommonCode.GetCompetitors(shooter.ShooterId, "PatrolId");

            foreach (Structs.Competitor comp in competitors)
            {
                if (comp.PatrolId > 0)
                {
                    Structs.Patrol patrol = CommonCode.GetPatrol(comp.PatrolId);
                    ev.Graphics.DrawString(
                        patrol.StartDateTimeDisplay.ToShortTimeString(), 
                        printCompetitorFont, 
                        Brushes.Black, 
                        LeftMargin + tab1, 
                        yPos, 
                        new StringFormat());

                    ev.Graphics.DrawString(
                        patrol.PatrolId.ToString(), 
                        printCompetitorFont, 
                        Brushes.Black, 
                        LeftMargin + tab2, 
                        yPos, 
                        new StringFormat());
                }

                Structs.Weapon weapon = CommonCode.GetWeapon(comp.WeaponId);
                ev.Graphics.DrawString(
                    weapon.Manufacturer + ", " + weapon.Model + "(" + comp.WeaponId + ")", 
                    printCompetitorFont, 
                    Brushes.Black, 
                    LeftMargin + tab3, 
                    yPos, 
                    new StringFormat());
                yPos += printHeaderFont.GetHeight();
            }

            currentShooter++;
        }

        #endregion
    }
}
