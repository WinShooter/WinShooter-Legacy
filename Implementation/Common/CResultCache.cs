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
// $Id: CResultCache.cs 107 2009-02-01 06:25:33Z smuda $
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Common
{
	class CResultCache
	{
		internal CResultCache(Interface callerInterface)
		{
			myInterface = callerInterface;
			result = new CResult(callerInterface);
		}

		private Interface myInterface;
		private CResult result;

		internal ResultsReturn ResultsGetCompetitor(
			Structs.Competitor competitor)
		{
			return result.ResultsGetCompetitor(competitor);
		}

		internal Structs.ResultWeaponsClass[] ResultsGetWClasses()
		{
			return result.ResultsGetWClasses();
		}

		internal Structs.ShootersClass[] ResultsGetUClasses(
			Structs.ResultWeaponsClass wclass)
		{
			return result.ResultsGetUClasses(wclass);
		}

		internal Structs.ResultWeaponsClass[] GetResultWeaponClassesWithResults()
		{
			return result.GetResultWeaponClassesWithResults();
		}

		internal ResultsReturn[] GetResults(
			Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass,
			Structs.Competition competition)
		{
			return GetResults(
				wclass,
				uclass,
				competition,
				false);
		}

		internal ResultsReturn[] GetResults(
			Structs.ResultWeaponsClass wclass, 
			Structs.ShootersClass uclass,
			Structs.Competition competition,
			bool finalResults)
		{
			lock (resultHolders)
			{
				foreach (CResultHolder holder in resultHolders)
				{
					if (holder.WClass == wclass &&
						holder.UClass == uclass &&
						holder.NorwegianCount == competition.NorwegianCount &&
						holder.FinalResults == finalResults)
					{
						return holder.Results;
					}
				}

				ResultsReturn[] results = result.GetResults(wclass,
					uclass,
					competition,
					finalResults);

				CResultHolder newHolder = new CResultHolder(
					wclass,
					uclass,
					competition.NorwegianCount,
					finalResults,
					results);
				resultHolders.Add(newHolder);

				return results;
			}
		}

		List<CResultHolder> resultHolders = new List<CResultHolder>();

		internal void UpdatedShooter(Structs.Shooter shooter)
		{
			lock (resultHolders)
			{
				resultHolders.Clear();
			}
		}

		internal void UpdatedCompetitor(Structs.Competitor competitor)
		{
			lock (resultHolders)
			{
				resultHolders.Clear();
			}
		}

		internal void UpdatedCompetitorResult(Structs.CompetitorResult compResult)
		{
			lock (resultHolders)
			{
				resultHolders.Clear();
			}
		}
	}
}
