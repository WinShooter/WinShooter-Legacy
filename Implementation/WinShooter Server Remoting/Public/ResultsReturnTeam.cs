using System;
using System.Collections.Generic;
using System.Text;

namespace Allberg.Shooter.WinShooterServerRemoting
{
	/// <summary>
	/// ResultsReturn
	/// </summary>
	[Serializable]
	public class ResultsReturnTeam
	{
		/// <summary>
		/// 
		/// </summary>
		public int TeamId;
		/// <summary>
		/// 
		/// </summary>
		public string TeamName;
		/// <summary>
		/// 
		/// </summary>
		public string ClubId;
		/// <summary>
		/// 
		/// </summary>
		public int Hits;
		/// <summary>
		/// 
		/// </summary>
		public int FigureHits;
		/// <summary>
		/// 
		/// </summary>
		public string HitsPerStn;
		/// <summary>
		/// 
		/// </summary>
		public int Points;
		/// <summary>
		/// 
		/// </summary>
		public int PointsSort;
	}
}
