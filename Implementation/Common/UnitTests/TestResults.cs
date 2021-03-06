#region copyright
/*
Copyright �2009 John Allberg

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
// $Id$
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Common.UnitTests
{
#if DEBUG
	[TestFixture]
	public class TestResultsField
	{
		const string filename = @"C:\temp\unittest.mdb";

		Interface intf = new Interface();

		[SetUp]
		public void SetupFixture()
		{
			if (System.IO.File.Exists(filename))
				System.IO.File.Delete(filename);

			intf = new Interface();
			intf.CreateAccessDatabase(filename);
			intf.CreateDefaultDatabaseContent();
		}


		[Test]
		public void TestStandardFieldTwoShooters()
		{
			Structs.Competition competition = new Structs.Competition();
			competition.Championship = Structs.CompetitionChampionshipEnum.Club;
			competition.DoFinalShooting = false;
			competition.FirstPrice = 400;
			competition.Name = "Unittest Field Two Shooters";
			competition.NorwegianCount = false;
			competition.PatrolConnectionType = Structs.PatrolConnectionTypeEnum.AR_B_C_M;
			competition.Type = Structs.CompetitionTypeEnum.Field;
			intf.NewCompetition(competition);

			createShooter(1);
			createShooter(2);
			createShooter(3);
			createShooter(4);
		}

		private void createClub()
		{
			Structs.Club club = new Structs.Club();
			club.Country = "SE";
			club.Name = "Club1";
			club.ClubId = "01-001";
			intf.NewClub(club);
		}

		private void createShooter(int cardnr)
		{
			Structs.Shooter shooter = new Structs.Shooter();
			shooter.CardNr = cardnr.ToString();
			shooter.Class = Structs.ShootersClass.Klass1;
			shooter.ClubId = "01-001";
			shooter.Givenname = cardnr.ToString();
			shooter.Surname = cardnr.ToString();
			intf.NewShooter(shooter);
		}
	}
#endif
}
