using System;
using System.Collections.Generic;
using System.Text;

namespace Allberg.Shooter.WinShooterServerRemoting
{
	/// <summary>
	/// ResultsReturn
	/// </summary>
	[Serializable]
	public class ResultsReturn : IComparable<ResultsReturn>
	{
		public ResultsReturn(
			Structs.CompetitionTypeEnum CompetitionType, 
			bool norwegian,
			Structs.CompetitionChampionshipEnum ChampionShip)
		{
			this.CompetitionType = CompetitionType;
			this.Norwegian = norwegian;
			this.ChampionShip = ChampionShip;
		}
		/// <summary>
		/// 
		/// </summary>
		public int CompetitorId;
		/// <summary>
		/// 
		/// </summary>
		public string ShooterName;
		/// <summary>
		/// 
		/// </summary>
		public string ClubId;
		/// <summary>
		/// 
		/// </summary>
		public int HitsTotal;
		/// <summary>
		/// 
		/// </summary>
		public int FigureHitsTotal;
		/// <summary>
		/// 
		/// </summary>
		public int[] FigureHitsPerStn = new int[0];
		/// <summary>
		/// 
		/// </summary>
		public int[] HitsPerStn = new int[0];
		/// <summary>
		/// 
		/// </summary>
		public string HitsPerStnString
		{
			get
			{
				StringBuilder toReturn = new StringBuilder();
				switch (this.CompetitionType)
				{
					case Structs.CompetitionTypeEnum.Field:
						if (!this.Norwegian)
						{
							for(int i=0;i<this.HitsPerStn.Length;i++)
							{
								toReturn.Append(HitsPerStn[i].ToString());
								toReturn.Append("/");
								toReturn.Append(FigureHitsPerStn[i].ToString());
								toReturn.Append(";");
							}
							return toReturn.ToString();
						}
						else
						{
							for (int i = 0; i < this.HitsPerStn.Length; i++)
							{
								toReturn.Append((HitsPerStn[i] + FigureHitsPerStn[i]).ToString());
								toReturn.Append(";");
							}
							return toReturn.ToString();
						}
					case Structs.CompetitionTypeEnum.Precision:
						for (int i = 0; i < this.HitsPerStn.Length; i++)
						{
							toReturn.Append(HitsPerStn[i].ToString());
							toReturn.Append(";");
						}
						return toReturn.ToString();
					default:
						throw new NotImplementedException();
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public int PointsTotal;
		/// <summary>
		/// 
		/// </summary>
		public int[] PointsPerStn = new int[0];
		/// <summary>
		/// 
		/// </summary>
		public int PointsSort;
		/// <summary>
		/// 
		/// </summary>
		public Structs.Medal Medal;
		/// <summary>
		/// 
		/// </summary>
		public int PriceMoney;
		/// <summary>
		/// 
		/// </summary>
		public int FinalShootingPlace = 100;
		public Structs.CompetitionTypeEnum CompetitionType = Structs.CompetitionTypeEnum.Field;
		public Structs.CompetitionChampionshipEnum ChampionShip = Structs.CompetitionChampionshipEnum.Club;
		public bool Norwegian = false;
		public int NorwegianPoints
		{
			get
			{
				return (this.HitsTotal + this.FigureHitsTotal);
			}
		}
		public int[] NorwegianPointsPerStn
		{
			get
			{
				List<int> toReturn = new List<int>();

				for (int i = 0; i < HitsPerStn.Length; i++)
				{
					toReturn.Add(HitsPerStn[i] + FigureHitsPerStn[i]);
				}

				return toReturn.ToArray();
			}
		}

		public void AddHitsPerStn(int hps)
		{
			int[] current = this.HitsPerStn;
			this.HitsPerStn = new int[current.Length + 1];
			Array.Copy(current, this.HitsPerStn, current.Length);
			HitsPerStn[HitsPerStn.Length - 1] = hps;
		}

		public void AddFigureHitsPerStn(int fhps)
		{
			int[] current = this.FigureHitsPerStn;
			this.FigureHitsPerStn = new int[current.Length + 1];
			Array.Copy(current, this.FigureHitsPerStn, current.Length);
			FigureHitsPerStn[FigureHitsPerStn.Length - 1] = fhps;
		}

		public void AddPointsPerStn(int pps)
		{
			int[] current = this.PointsPerStn;
			this.PointsPerStn = new int[current.Length + 1];
			Array.Copy(current, this.PointsPerStn, current.Length);
			PointsPerStn[PointsPerStn.Length - 1] = pps;
		}

		#region IComparable<CompetitorResult> Members
		private enum ReturnResult
		{
			OtherIsBefore = 1,
			Same = 0,
			ThisIsBefore = -1
		}
		/// <summary>
		/// Used to compare between CompetitorResults
		/// </summary>
		/// <param name="other">The one to compare to</param>
		/// <returns>Less than zero - This instance is less than obj.</returns>
		/// <returns>Zero - This instance is equal to obj.</returns>
		/// <returns>Greater than zero - This instance is greater than obj. </returns>
		int IComparable<ResultsReturn>.CompareTo(ResultsReturn other)
		{
			if (other == null)
				return (int)ReturnResult.ThisIsBefore;

			switch (CompetitionType)
			{
				case Structs.CompetitionTypeEnum.Field:
					if (!Norwegian)
						return (int)IComparableField(other);
					else
						return (int)IComparableFieldNorwegian(other);
				case Structs.CompetitionTypeEnum.Precision:
					return (int)IComparablePrecision(other);
				case Structs.CompetitionTypeEnum.MagnumField:
					return (int)IComparableMagnumField(other);
				default:
					throw new NotSupportedException("CompetitionType " + CompetitionType.ToString() +
						" is not supported.");
			}
		}

		/* 
		 * Resultat som beräknas här görs enligt B3.6.1.2 för fältskytte
		 * och B3.6.1.3 för poängfältskjutning (norsk räkning)
		 * i Svenska Pistolskytteförbundets skjuthandbok, 11 omarbetade 
		 * upplagan, uppdaterad 2004-03-02.
		 * 
		 * "B3.6.1.2 Fältskjutning
		 * Särskiljning vid lika antal träff äger rum enligt följande:
		 * 1. Största antalet träffade figurer
		 * 2. högsta poäng i eventuellt förekommande ringad figur
		 * 3. största antalet träff på sista stationen, näst sista stationen 
		 * o.s.v,
		 * 
		 * Kan särskiljning inte erhållas, skall antingen särskjutning ske 
		 * tills särsskiljning erhållits, eller då särskjutning inte kan 
		 * eller inte anses böra ske, lottning utföras."
		 * 
		 */

		private ReturnResult IComparableField(ResultsReturn other)
		{
			if (other.HitsTotal == 0 &&
				this.HitsTotal == 0)
				return ReturnResult.Same;

			// distinguish with HitsTotal
			if (other.HitsTotal > this.HitsTotal)
				return ReturnResult.OtherIsBefore;
			else if (other.HitsTotal < this.HitsTotal)
				return ReturnResult.ThisIsBefore;

			if (ChampionShip == Structs.CompetitionChampionshipEnum.Landsdel ||
				ChampionShip == Structs.CompetitionChampionshipEnum.Krets ||
				ChampionShip == Structs.CompetitionChampionshipEnum.Nationell ||
				ChampionShip == Structs.CompetitionChampionshipEnum.SM)
			{
				// FinalShootingPlace
				if (other.FinalShootingPlace < this.FinalShootingPlace)
					return ReturnResult.OtherIsBefore;
				else if (other.FinalShootingPlace > this.FinalShootingPlace)
					return ReturnResult.ThisIsBefore;
			}

			// distinguish with FigureHitsTotal
			if (other.FigureHitsTotal > this.FigureHitsTotal)
				return ReturnResult.OtherIsBefore;
			else if (other.FigureHitsTotal < this.FigureHitsTotal)
				return ReturnResult.ThisIsBefore;

			// distinguish with PointsTotal
			if (other.PointsTotal > this.PointsTotal)
				return ReturnResult.OtherIsBefore;
			else if (other.PointsTotal < this.PointsTotal)
				return ReturnResult.ThisIsBefore;

			// Couldn't distinguish with hits per station
			for (int i = HitsPerStn.Length-1; i >= 0; i--)
			{
				if (other.HitsPerStn[i] > this.HitsPerStn[i])
					return ReturnResult.OtherIsBefore;
				else if (other.HitsPerStn[i] < this.HitsPerStn[i])
					return ReturnResult.ThisIsBefore;
			}

			if (ChampionShip == Structs.CompetitionChampionshipEnum.Club)
			{
				// Lastly, FinalShootingPlace
				if (other.FinalShootingPlace < this.FinalShootingPlace)
					return ReturnResult.OtherIsBefore;
				else if (other.FinalShootingPlace > this.FinalShootingPlace)
					return ReturnResult.ThisIsBefore;
			}

			// Couldn't distinguish with StationHits
			return ReturnResult.Same;
		}


		/* "B3.6.1.3 Poängfältskjutning
* Särskiljning vid lika antal poäng äger rum enligt följande
* 1. högsta poäng i ringad figur.
* 2. högsta antal poäng på sista, näst sista stationen osv,
* 
* Kan särskiljning ändå inte erhållas, skall antingen särskjutning 
* ske till särskiljning erhållits, eller då särskjutning inte kan 
* eller inte anses böra ske, lottning utföras."
* 
* I detta program erhålls detta genom att sortera på NorwPoints 
* (summan träffade figurer och träff), FinalShootingPlace,
* StationHits och Points
		 */

		private ReturnResult IComparableFieldNorwegian(ResultsReturn other)
		{
			if (other.NorwegianPoints > this.NorwegianPoints)
				return ReturnResult.OtherIsBefore;
			else if (other.NorwegianPoints < this.NorwegianPoints)
				return ReturnResult.ThisIsBefore;

			if (ChampionShip == Structs.CompetitionChampionshipEnum.Landsdel ||
				ChampionShip == Structs.CompetitionChampionshipEnum.Nationell ||
				ChampionShip == Structs.CompetitionChampionshipEnum.SM)
			{
				// FinalShootingPlace
				if (other.FinalShootingPlace < this.FinalShootingPlace)
					return ReturnResult.OtherIsBefore;
				else if (other.FinalShootingPlace > this.FinalShootingPlace)
					return ReturnResult.ThisIsBefore;
			}

			if (other.PointsTotal > this.PointsTotal)
				return ReturnResult.OtherIsBefore;
			else if (other.PointsTotal < this.PointsTotal)
				return ReturnResult.ThisIsBefore;

			if (this.NorwegianPointsPerStn.Length > other.NorwegianPointsPerStn.Length)
				return ReturnResult.ThisIsBefore;
			else if (this.NorwegianPointsPerStn.Length < other.NorwegianPointsPerStn.Length)
				return ReturnResult.OtherIsBefore;

			for (int i = NorwegianPointsPerStn.Length - 1; i >= 0; i--)
			{
				if (other.NorwegianPointsPerStn[i] > this.NorwegianPointsPerStn[i])
					return ReturnResult.OtherIsBefore;
				else if (other.NorwegianPointsPerStn[i] < this.NorwegianPointsPerStn[i])
					return ReturnResult.ThisIsBefore;
			}

			// Lastly, FinalShootingPlace
			if (other.FinalShootingPlace < this.FinalShootingPlace)
				return ReturnResult.OtherIsBefore;
			else if (other.FinalShootingPlace > this.FinalShootingPlace)
				return ReturnResult.ThisIsBefore;

			return ReturnResult.Same;
		}

		/*
 * SHB 2005:
 * "B3.6.1.1 Precisionsskjutning
 * Särskiljning vid lika poängtal äger rum enligt följande:
 * 1. högsta poängtalet i sista serien, därefter näst sista serien o.s.v.
 * 2. Starta antalet 10:or därefter 9:or osv i  sista serien, 
 * därefter motsvarande i näst sista serien o.s.v.
 * 
 * Kan särskiljning ändå inte erhållas, skall antingen särskjutning ske med serie
 * (serier) tills särskiljning erhållits, eller då särskjutning inte kan eller 
 * inte anses böra ske, lottning utföras."
		 */
		private ReturnResult IComparablePrecision(ResultsReturn other)
		{
			if (other.HitsTotal > this.HitsTotal)
				return ReturnResult.OtherIsBefore;
			else if (other.HitsTotal < this.HitsTotal)
				return ReturnResult.ThisIsBefore;

			if (ChampionShip == Structs.CompetitionChampionshipEnum.Landsdel ||
				ChampionShip == Structs.CompetitionChampionshipEnum.Nationell ||
				ChampionShip == Structs.CompetitionChampionshipEnum.SM)
			{
				// FinalShootingPlace
				if (other.FinalShootingPlace < this.FinalShootingPlace)
					return ReturnResult.OtherIsBefore;
				else if (other.FinalShootingPlace > this.FinalShootingPlace)
					return ReturnResult.ThisIsBefore;
			}

			// distinguish with hits per station
			if (this.HitsPerStn.Length > other.HitsPerStn.Length)
				return ReturnResult.ThisIsBefore;
			else if (this.HitsPerStn.Length < other.HitsPerStn.Length)
				return ReturnResult.OtherIsBefore;

			if (this.HitsPerStn.Length > 0)
			{
				for (int i = this.HitsPerStn.Length - 1; i >= 0; i--)
				{
					if (this.HitsPerStn[i] > other.HitsPerStn[i])
						return ReturnResult.ThisIsBefore;

					if (this.HitsPerStn[i] < other.HitsPerStn[i])
						return ReturnResult.OtherIsBefore;
				}
			}

			// TODO Implement check 
			// 2. Starta antalet 10:or därefter 9:or osv i  sista serien, 
			// därefter motsvarande i näst sista serien o.s.v.

			// Lastly, FinalShootingPlace
			if (other.FinalShootingPlace < this.FinalShootingPlace)
				return ReturnResult.OtherIsBefore;
			else if (other.FinalShootingPlace > this.FinalShootingPlace)
				return ReturnResult.ThisIsBefore;

			return ReturnResult.Same;
		}

		/*
B.3.6.1.5 Magnumfältskjutning
Särskiljning vid lika antal träff sker enligt följande:
1. största antal träffade figurer
2. högsta antal sammanlagda poäng i poängmålen
3. högsta antal poäng på sista skjutstationen, näst sista o.s.v.
4. högsta antal träff på sista skjutstationen, näst sista o.s.v.
5. särskjutning
		 * */
		private ReturnResult IComparableMagnumField(ResultsReturn other)
		{
			// distinguish with HitsTotal
			if (other.HitsTotal > this.HitsTotal)
				return ReturnResult.OtherIsBefore;
			else if (other.HitsTotal < this.HitsTotal)
				return ReturnResult.ThisIsBefore;

			if (ChampionShip == Structs.CompetitionChampionshipEnum.Landsdel ||
				ChampionShip == Structs.CompetitionChampionshipEnum.Nationell ||
				ChampionShip == Structs.CompetitionChampionshipEnum.SM)
			{
				// FinalShootingPlace
				if (other.FinalShootingPlace < this.FinalShootingPlace)
					return ReturnResult.OtherIsBefore;
				else if (other.FinalShootingPlace > this.FinalShootingPlace)
					return ReturnResult.ThisIsBefore;
			}
	
			// distinguish with FigureHitsTotal
			if (other.FigureHitsTotal > this.FigureHitsTotal)
				return ReturnResult.OtherIsBefore;
			else if (other.FigureHitsTotal < this.FigureHitsTotal)
				return ReturnResult.ThisIsBefore;

			// distinguish with PointsTotal
			if (other.PointsTotal > this.PointsTotal)
				return ReturnResult.OtherIsBefore;
			else if (other.PointsTotal < this.PointsTotal)
				return ReturnResult.ThisIsBefore;

			// distinguish with Points per station
			if (this.PointsPerStn.Length > other.PointsPerStn.Length)
				return ReturnResult.ThisIsBefore;
			else if (this.PointsPerStn.Length < other.PointsPerStn.Length)
				return ReturnResult.OtherIsBefore;

			if (this.PointsPerStn.Length > 0)
			{
				for (int i = this.PointsPerStn.Length - 1; i >= 0; i--)
				{
					if (this.PointsPerStn[i] > other.PointsPerStn[i])
						return ReturnResult.ThisIsBefore;

					if (this.PointsPerStn[i] < other.PointsPerStn[i])
						return ReturnResult.OtherIsBefore;
				}
			}

			// distinguish with Hits per station
			if (this.HitsPerStn.Length > other.HitsPerStn.Length)
				return ReturnResult.ThisIsBefore;
			else if (this.HitsPerStn.Length < other.HitsPerStn.Length)
				return ReturnResult.OtherIsBefore;

			if (this.HitsPerStn.Length > 0)
			{
				for (int i = this.HitsPerStn.Length - 1; i >= 0; i--)
				{
					if (this.HitsPerStn[i] > other.HitsPerStn[i])
						return ReturnResult.ThisIsBefore;

					if (this.HitsPerStn[i] < other.HitsPerStn[i])
						return ReturnResult.OtherIsBefore;
				}
			}

			// Lastly, FinalShootingPlace
			if (other.FinalShootingPlace < this.FinalShootingPlace)
				return ReturnResult.OtherIsBefore;
			else if (other.FinalShootingPlace > this.FinalShootingPlace)
				return ReturnResult.ThisIsBefore;

			return ReturnResult.Same;
		}
		#endregion
	}
}