using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

#if DEBUG
namespace Allberg.Shooter.WinShooterServerRemoting.UnitTests
{
	[TestFixture]
	public class TestsResultsReturn
	{
		private void SetupField(Structs.CompetitionTypeEnum CompetitionType, bool Norwegian,
			out ResultsReturn obj1, out ResultsReturn obj2,
			string testName,
			Structs.CompetitionChampionshipEnum ChampionShip)
		{
			obj1 = new ResultsReturn(
				CompetitionType, 
				Norwegian,
				ChampionShip);
			obj1.ClubId = "01-42";
			obj1.CompetitorId = 1;
			obj1.FigureHitsTotal = 12;
			obj1.FinalShootingPlace = 100;
			obj1.HitsTotal = 12;
			obj1.PointsTotal = 10;
			obj1.ShooterName = "First Shooter " + testName;
			obj1.HitsPerStn = new int[] {6, 6};
			obj1.FigureHitsPerStn = new int[] {2, 2};
			obj1.PointsPerStn = new int[] {0,11};

			obj2 = new ResultsReturn(
				CompetitionType, 
				Norwegian,
				ChampionShip);
			obj2.ClubId = "01-42";
			obj2.CompetitorId = 1;
			obj2.FigureHitsTotal = 12;
			obj2.FinalShootingPlace = 100;
			obj2.HitsTotal = 12;
			obj2.PointsTotal = 10;
			obj2.ShooterName = "Second Shooter " + testName;
			obj2.HitsPerStn = new int[] {6,6};
			obj2.FigureHitsPerStn = new int[] {2,2};
			obj2.PointsPerStn = new int[] {0,11};
		}

		private void runCheck(ResultsReturn obj1, ResultsReturn obj2)
		{
			List<ResultsReturn> list = new List<ResultsReturn>();
			list.Add(obj1);
			list.Add(obj2);
			list.Sort();

			Assert.AreEqual(obj1, list[0], "Sorting nr 1-1 failed!");
			Assert.AreEqual(obj2, list[1], "Sorting nr 1-2 failed!");

			list = new List<ResultsReturn>();
			list.Add(obj2);
			list.Add(obj1);
			list.Sort();

			Assert.AreEqual(obj1, list[0], "Sorting nr 2-1 failed!");
			Assert.AreEqual(obj2, list[1], "Sorting nr 2-2 failed!");
		}

		#region Field
		[Test]
		public void CompareField_Hits()
		{
			ResultsReturn obj1, obj2;
			SetupField(Structs.CompetitionTypeEnum.Field, false,
				out obj1, out obj2, "CompareFieldHits", 
				Structs.CompetitionChampionshipEnum.Club);

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Field;

			obj1.HitsTotal++;

			obj1.FigureHitsTotal = 11; // Should not be used
			obj2.FigureHitsTotal = 12; // Should not be used

			runCheck(obj1, obj2);
		}

		[Test]
		public void CompareField_Points()
		{
			ResultsReturn obj1, obj2;
			SetupField(Structs.CompetitionTypeEnum.Field, false,
				out obj1, out obj2, "CompareFieldPoints",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Field;

			obj1.PointsTotal++;

			runCheck(obj1, obj2);
		}

		[Test]
		public void CompareField_HitsPerStn()
		{
			ResultsReturn obj1, obj2;
			SetupField(Structs.CompetitionTypeEnum.Field, false,
				out obj1, out obj2, "CompareFieldHitsPerStn",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.HitsPerStn[0]--;
			obj1.HitsPerStn[1]++;

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Field;

			runCheck(obj1, obj2);
		}

		[Test]
		public void CompareField_FinalPlace()
		{
			ResultsReturn obj1, obj2;
			SetupField(Structs.CompetitionTypeEnum.Field, false,
				out obj1, out obj2, "CompareFieldFinalPlace",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.FinalShootingPlace--;

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Field;

			runCheck(obj1, obj2);
		}
		#endregion

		#region Field Norwegian
		[Test]
		public void CompareFieldNor_Hits()
		{
			ResultsReturn obj1, obj2;
			SetupField(Structs.CompetitionTypeEnum.Field, true,
				out obj1, out obj2, "CompareFieldHits",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Field;

			obj1.HitsTotal++;

			runCheck(obj1, obj2);
		}

		[Test]
		public void CompareFieldNor_Points()
		{
			ResultsReturn obj1, obj2;
			SetupField(Structs.CompetitionTypeEnum.Field, true,
				out obj1, out obj2, "CompareFieldPoints",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Field;

			obj1.PointsTotal++;

			runCheck(obj1, obj2);
		}

		[Test]
		public void CompareFieldNor_HitsPerStn()
		{
			ResultsReturn obj1, obj2;
			SetupField(Structs.CompetitionTypeEnum.Field, true,
				out obj1, out obj2, "CompareFieldHitsPerStn",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.HitsPerStn[0]--;
			obj1.HitsPerStn[1]++;

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Field;

			runCheck(obj1, obj2);
		}

		[Test]
		public void CompareFieldNor_FinalPlace()
		{
			ResultsReturn obj1, obj2;
			SetupField(Structs.CompetitionTypeEnum.Field, true,
				out obj1, out obj2, "CompareFieldFinalPlace",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.FinalShootingPlace--;

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Field;

			runCheck(obj1, obj2);
		}
		#endregion

		#region Precision
		private void SetupPrecision(Structs.CompetitionTypeEnum CompetitionType, bool Norwegian,
			out ResultsReturn obj1, out ResultsReturn obj2,
			string testName,
			Structs.CompetitionChampionshipEnum ChampionShip)
		{
			obj1 = new ResultsReturn(CompetitionType, 
				Norwegian, 
				ChampionShip);
			obj1.ClubId = "01-42";
			obj1.CompetitorId = 1;
			obj1.FigureHitsTotal = 12;
			obj1.FinalShootingPlace = 100;
			obj1.HitsTotal = 100;
			obj1.PointsTotal = 0;
			obj1.ShooterName = "First Shooter " + testName;
			obj1.HitsPerStn = new int[] {50,50};
			obj1.FigureHitsPerStn = new int[] {0,0};
			obj1.PointsPerStn = new int[] {0, 0};

			obj2 = new ResultsReturn(CompetitionType, Norwegian, 
				ChampionShip);
			obj2.ClubId = "01-42";
			obj2.CompetitorId = 1;
			obj2.FigureHitsTotal = 12;
			obj2.FinalShootingPlace = 100;
			obj2.HitsTotal = 100;
			obj2.PointsTotal = 0;
			obj2.ShooterName = "Second Shooter " + testName;
			obj2.HitsPerStn = new int[] {50, 50};
			obj2.FigureHitsPerStn = new int[] {0,0};
			obj2.PointsPerStn = new int[] {0,0};
		}

		[Test]
		public void ComparePrecision_Hits()
		{
			ResultsReturn obj1, obj2;
			SetupPrecision(Structs.CompetitionTypeEnum.Precision, false,
				out obj1, out obj2, "ComparePrecision_Hits",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Precision;

			obj1.HitsTotal++;

			runCheck(obj1, obj2);
		}

		[Test]
		public void ComparePrecision_Points()
		{
			ResultsReturn obj1, obj2;
			SetupPrecision(Structs.CompetitionTypeEnum.Precision, false,
				out obj1, out obj2, "ComparePrecision_Points",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Precision;

			obj1.PointsTotal++;

			runCheck(obj1, obj2);
		}

		[Test]
		public void ComparePrecision_HitsPerStn()
		{
			ResultsReturn obj1, obj2;
			SetupPrecision(Structs.CompetitionTypeEnum.Precision, false,
				out obj1, out obj2, "ComparePrecision_HitsPerStn",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.HitsPerStn[0]--;
			obj1.HitsPerStn[1]++;

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Precision;

			runCheck(obj1, obj2);
		}

		[Test]
		public void ComparePrecision_FinalPlace()
		{
			ResultsReturn obj1, obj2;
			SetupPrecision(Structs.CompetitionTypeEnum.Precision, false,
				out obj1, out obj2, "ComparePrecision_FinalPlace",
				Structs.CompetitionChampionshipEnum.Club);

			obj1.FinalShootingPlace--;

			obj1.CompetitionType = obj2.CompetitionType = Structs.CompetitionTypeEnum.Precision;

			runCheck(obj1, obj2);
		}
		#endregion
	}
}
#endif
