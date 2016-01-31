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
// $Id: ClientInterface.cs 127 2011-05-28 16:46:41Z smuda $ 
using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;
using Allberg.Shooter.Common;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.WinShooterServer
{
	public delegate void CompetitionChangedHandler();

	/// <summary>
	/// Summary description 
	/// </summary>
	[CLSCompliant(false)]
	public class ClientInterface : MarshalByRefObject,
		IWinshooterServer
	{
		private const string MinimumClientVersionString = "1.6.0";
		private const int minNumberOfWorkerThreads = 5;

		public ClientInterface()
		{
			CommonCode = new Interface();
			CommonCode.UpdatedClub += 
				updatedClub;
			CommonCode.UpdatedWeapon += 
				updatedWeapon;
			CommonCode.UpdatedCompetition += 
				updatedCompetition;
			CommonCode.UpdatedCompetitor += 
				updatedCompetitor;
			CommonCode.UpdatedCompetitorResult += 
				updatedCompetitorResult;
			CommonCode.UpdatedFileImportCount +=
				updatedFileImportCount;
			CommonCode.UpdatedPatrol += 
				updatedPatrol;
			CommonCode.UpdatedStation +=
				updatedStation;
			CommonCode.UpdatedShooter +=
				updatedShooter;
			CommonCode.UpdatedTeam +=
				updatedTeam;
			CommonCode.UpdatedPatrolAddAutomaticCompetitors += 
				updatedPatrolAddAutomaticCompetitors;

			CommonCode.SyncronizeEvent +=
				Sync;

			try
			{
				Thread.CurrentThread.Name = "Server Interface Thread";
			}
			catch(Exception exc)
			{
				Console.WriteLine(exc.ToString());
			}
		}

		Allberg.Shooter.Common.Interface CommonCode;


		#region Properties
		public bool EnableInternetConnections
		{
			get
			{
				return CommonCode.EnableInternetConnections;
			}
			set
			{
				CommonCode.EnableInternetConnections = value;
			}
		}
		public Structs.PatrolConnectionTypeEnum PatrolConnectionType
		{
			get
			{
				return CommonCode.PatrolConnectionType;
			}
			set
			{
				CommonCode.PatrolConnectionType = value;
			}
		}
		public ISettings Settings
		{
			get
			{
				return CommonCode.Settings;
			}
		}
		internal string CurrentFilename
		{
			get
			{
				if (CommonCode == null)
					return null;

				return CommonCode.CurrentFilename;
			}
		}
		#endregion
	
		#region Database Methods
		#region Database Init

		public void CreateAccessDatabase(string pathAndFilename)
		{
			CommonCode.CreateAccessDatabase(pathAndFilename);
		}
		public void CreateDefaultDatabaseContent()
		{
			CommonCode.CreateDefaultDatabaseContent();
		}
		public void OpenAccessDatabase(string pathAndFilename)
		{

			Trace.WriteLine("ClientInterface: OpenAccessDatabase started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
				DateTime.Now.ToLongTimeString());

			CommonCode.OpenAccessDatabase(pathAndFilename);

			Trace.WriteLine("ClientInterface: OpenAccessDatabase ended from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
				DateTime.Now.ToLongTimeString());

		}
		public void OpenDatabase()
		{
			Trace.WriteLine("ClientInterface: OpenDatabase started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
				DateTime.Now.ToLongTimeString());

			CommonCode.OpenDatabase();

			Trace.WriteLine("ClientInterface: OpenDatabase started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
				DateTime.Now.ToLongTimeString());
		}

		#endregion
		#region Get stuff
		public Structs.Club GetClub(string clubId)
		{
			try
			{
				return CommonCode.GetClub(clubId);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Club[] GetClubs()
		{
			try
			{
				return CommonCode.GetClubs();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Shooter[] GetShooters()
		{
			try
			{
				return CommonCode.GetShooters();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Shooter[] GetShooters(string sorting)
		{
			try
			{
				return CommonCode.GetShooters(sorting);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Shooter[] GetShooters(Structs.Club clubToFetch)
		{
			try
			{
				return CommonCode.GetShooters(clubToFetch);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Shooter[] GetShooters(Structs.Club clubToFetch, 
			Structs.ResultWeaponsClass wClass)
		{
			try
			{
				return CommonCode.GetShooters(clubToFetch, wClass);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Shooter GetShooter(int shooterId)
		{
			try
			{
				return CommonCode.GetShooter(shooterId);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetShooterHighestId()
		{
			try
			{
				return CommonCode.GetShooterHighestId();
			}
			catch(Exception exc)
			{
				Trace.WriteLine("GetShooterHighestId exception " +
					exc);
				throw;
			}
		}
		public Structs.Shooter GetShooter(string cardNr)
		{
			try
			{
				return CommonCode.GetShooter(cardNr);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Competitor[] GetCompetitors()
		{
			try
			{
				return CommonCode.GetCompetitors();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Competitor[] GetCompetitors(int shooterId, string sorting)
		{
			try
			{
				return CommonCode.GetCompetitors(shooterId, sorting);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Competitor[] GetCompetitors(int shooterId)
		{
			try
			{
				return CommonCode.GetCompetitors(shooterId);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Competitor[] GetCompetitors(Structs.Patrol patrolToFetch)
		{
			try
			{
				return CommonCode.GetCompetitors(patrolToFetch, "");
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Competitor[] GetCompetitors(Structs.Patrol patrolToFetch, string sorting)
		{
			try
			{
				return CommonCode.GetCompetitors(patrolToFetch, sorting);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Competitor[] GetCompetitorsWithNoPatrol()
		{
			try
			{
				return CommonCode.GetCompetitorsWithNoPatrol();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Competitor[] GetCompetitorsWithNoPatrol(Structs.PatrolClass thisClass)
		{
			try
			{
				return CommonCode.GetCompetitorsWithNoPatrol(thisClass);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Competitor[] GetCompetitors(Structs.Club clubToFetch, 
			Structs.ResultWeaponsClass wclass, string sorting)
		{
			try
			{
				return CommonCode.GetCompetitors(clubToFetch, wclass, sorting);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		public Structs.Competitor GetCompetitor(int competitorId)
		{
			try
			{
				return CommonCode.GetCompetitor(competitorId);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Weapon[] GetWeapons()
		{
			try
			{
				return CommonCode.GetWeapons();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Weapon[] GetWeapons(string sorting)
		{
			try
			{
				return CommonCode.GetWeapons(sorting);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Weapon GetWeapon(string weaponsId)
		{
			try
			{
				return CommonCode.GetWeapon(weaponsId);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Competition CompetitionCurrent
		{
			get
			{
				try
				{
					return CommonCode.CompetitionCurrent;
				}
				catch (Exception exc)
				{
					Trace.WriteLine(exc.ToString());
					throw;
				}
			}
		}
		public Structs.Competition[] GetCompetitions()
		{
			try
			{
				return CommonCode.GetCompetitions();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Patrol GetPatrol(int id)
		{
			try
			{
				return CommonCode.GetPatrol(id);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Patrol[] GetPatrols()
		{
			try
			{
				return CommonCode.GetPatrols();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Patrol[] GetPatrols(Structs.PatrolClass patrolClass, 
			bool alsoIncludeUnknownClass)
		{
			try
			{
				return CommonCode.GetPatrols(patrolClass, alsoIncludeUnknownClass);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Patrol[] GetPatrols(Structs.PatrolClass patrolClass, 
			bool alsoIncludeUnknownClass,
			bool onlyIncludePatrolsWithSpace,
			int patrolIdToAlwaysView)
		{
			try
			{
				return CommonCode.GetPatrols(patrolClass, 
					alsoIncludeUnknownClass, 
					onlyIncludePatrolsWithSpace,
					patrolIdToAlwaysView);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.CompetitorResult[] GetCompetitorResults()
		{
			try
			{
				return CommonCode.GetCompetitorResults();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.CompetitorResult[] GetCompetitorResults(int competitorsId)
		{
			try
			{
				return CommonCode.GetCompetitorResults(competitorsId);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.CompetitorResult GetCompetitorResult(int competitorsId, int stationId)
		{
			try
			{
				return CommonCode.GetCompetitorResult(competitorsId, stationId);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Station[] GetStations()
		{
			try
			{
				return CommonCode.GetStations();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Station[] GetStationsDistinguish()
		{
			try
			{
				return CommonCode.GetStationsDistinguish();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Station GetStation(int stationNr, bool distinguish)
		{
			try
			{
				return CommonCode.GetStation(stationNr, distinguish);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Team[] GetTeams()
		{
			try
			{
				return CommonCode.GetTeams();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.Team GetTeam(int teamId)
		{
			try
			{
				return CommonCode.GetTeam(teamId);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
						  
		#endregion

		#region Get stuff count
		public int GetClubsCount()
		{
			try
			{
				return CommonCode.GetClubsCount();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetClubsCountWithShooters()
		{
			try
			{
				return CommonCode.GetClubsCountWithShooters();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetShootersCount()
		{
			try
			{
				return CommonCode.GetShootersCount();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetCompetitorsCount()
		{
			try
			{
				return CommonCode.GetCompetitorsCount();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetCompetitorsCountPatrol(Structs.Patrol patrol)
		{
			try
			{
				return CommonCode.GetCompetitorsCountPatrol(patrol);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetCompetitorsWithResultCountPatrol(Structs.Patrol patrol)
		{
			try
			{
				return CommonCode.GetCompetitorsWithResultCountPatrol(patrol);
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetCompetitorsArrivedCountPatrol(Structs.Patrol patrol)
		{
			try
			{
				return CommonCode.GetCompetitorsArrivedCountPatrol(patrol);
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetWeaponsCount()
		{
			try
			{
				return CommonCode.GetWeaponsCount();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetCompetitionsCount()
		{
			try
			{
				return CommonCode.GetCompetitionsCount();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetPatrolsCount()
		{
			try
			{
				return CommonCode.GetPatrolsCount();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetCompetitorResultsCount()
		{
			try
			{
				return CommonCode.GetCompetitorResultsCount();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetCompetitorResultsCount(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass)
		{
			try
			{
				return CommonCode.GetCompetitorResultsCount(wclass, uclass);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetCompetitorResultsCount(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass, string clubId)
		{
			try
			{
				return CommonCode.GetCompetitorResultsCount(wclass, uclass, clubId);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public bool GetCompetitorResultsExist(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass)
		{
			try
			{
				return CommonCode.GetCompetitorResultsExist(wclass, uclass);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public bool GetCompetitorResultsExist(Structs.ResultWeaponsClass wclass)
		{
			try
			{
				return CommonCode.GetCompetitorResultsExist(wclass);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int GetStationsCount()
		{
			try
			{
				return CommonCode.GetStationsCount();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion


		#region Update Stuff
		public void UpdateClub(Structs.Club club)
		{
			try
			{
				CommonCode.UpdateClub(club);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public void UpdateShooter(Structs.Shooter shooter)
		{
			try
			{
				CommonCode.UpdateShooter(shooter);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public void UpdateCompetitor(Structs.Competitor competitor)
		{
			try
			{
				CommonCode.UpdateCompetitor(competitor);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public void UpdateWeapon(Structs.Weapon weapon)
		{
			try
			{
				CommonCode.UpdateWeapon(weapon);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public void UpdateCompetition(Structs.Competition competition)
		{
			try
			{
				CommonCode.UpdateCompetition(competition);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public void UpdatePatrol(Structs.Patrol patrol)
		{
			try
			{
				CommonCode.UpdatePatrol(patrol);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public void UpdateCompetitorResult(Structs.CompetitorResult 
			competitorResult,
			bool updateInterface)
		{
			try
			{
				CommonCode.UpdateCompetitorResult(competitorResult, 
					updateInterface);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		public void UpdateStation(Structs.Station station)
		{
			try
			{
				CommonCode.UpdateStation(station);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		public void UpdateTeam(Structs.Team team)
		{
			try
			{
				CommonCode.UpdateTeam(team);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		#endregion

		#region Add stuff
		public void NewClub(Structs.Club club)
		{
			try
			{
				CommonCode.NewClub(club);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int NewShooter(Structs.Shooter shooter)
		{
			try
			{
				return CommonCode.NewShooter(shooter);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public void NewCompetitor(Structs.Competitor competitor)
		{
			try
			{
				CommonCode.NewCompetitor(competitor);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public void NewWeapon(Structs.Weapon weapon)
		{
			try
			{
				CommonCode.NewWeapon(weapon);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public void NewCompetition(Structs.Competition comp)
		{
			try
			{
				CommonCode.NewCompetition(comp);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public int NewPatrol(Structs.Patrol patrol, bool notifyGui)
		{
			try
			{
				return CommonCode.NewPatrol(patrol, notifyGui);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public void NewCompetitorResult(Structs.CompetitorResult res)
		{
			try
			{
				CommonCode.NewCompetitorResult(res);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public void NewStation(Structs.Station station, bool distinguish)
		{
			try
			{
				CommonCode.NewStation(station, distinguish);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public void NewTeam(Structs.Team team)
		{
			try
			{
				CommonCode.NewTeam(team);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		#endregion

		#region Remove stuff
		[CLSCompliant(false)]
		public void DelClub(Structs.Club club)
		{
			try
			{
				CommonCode.DelClub(club);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public void DelShooter(Structs.Shooter shooter)
		{
			try
			{
				CommonCode.DelShooter(shooter);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public void DelCompetitor(Structs.Competitor competitor)
		{
			try
			{
				CommonCode.DelCompetitor(competitor);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public void DelWeapon(Structs.Weapon weapon)
		{
			try
			{
				CommonCode.DelWeapon(weapon);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public void DelCompetition(Structs.Competition competition)
		{
			try
			{
				CommonCode.DelCompetition(competition);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public void DelPatrol(Structs.Patrol patrol)
		{
			try
			{
				CommonCode.DelPatrol(patrol);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public void DelCompetitorResult(Structs.CompetitorResult 
			competitorResult)
		{
			try
			{
				CommonCode.DelCompetitorResult(competitorResult);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		[CLSCompliant(false)]
		public void DelStation(Structs.Station station)
		{
			try
			{
				CommonCode.DelStation(station);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		[CLSCompliant(false)]
		public void DelStation(Structs.Station station, bool forcibleDelete)
		{
			try
			{
				CommonCode.DelStation(station, forcibleDelete);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		[CLSCompliant(false)]
		public void DelTeam(Structs.Team team)
		{
			try
			{
				CommonCode.DelTeam(team);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion

		#region Handlers for updating GUI and database file

		#region UpdatedClub
		private readonly object _eventLock = new object();

		public event MethodInvoker UpdatedClub
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_updatedClub += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_updatedClub -= value;
				}
			}
		}
		private event MethodInvoker _updatedClub;
		[CLSCompliant(false)]
		public void updatedClub()
		{
			Trace.WriteLine("ClientInterface.updatedClub started");
			var start = DateTime.Now;

			try
			{
				var i = 0;
				foreach(MethodInvoker thisDelegate in _updatedClub.GetInvocationList())
				{
					try
					{
						int workerThreads, completionPortThreads;
						ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						while (workerThreads < minNumberOfWorkerThreads)
						{
							Trace.WriteLine("To low number of worker threads. Waiting 50 ms.");
							Thread.Sleep(50);
							ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						}
						var updatedClubClientThread =
							new Thread(
								runDelegate) {Name = "updatedClubClientThread " + i};
						updatedClubClientThread.Start(thisDelegate);
						i++;
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedClub.Removing Handler. ");
						/*UpdatedClub -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedClub.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedClub:" + 
					exc);
				throw;
			}
			var span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedClub ended (" + span.Milliseconds + " ms)");
		}
		#endregion

		#region UpdatedShooter
		public event MethodInvoker UpdatedShooter
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_UpdatedShooter += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_UpdatedShooter -= value;
				}
			}
		}
		private event MethodInvoker _UpdatedShooter;
		[CLSCompliant(false)]
		public void updatedShooter()
		{
			Trace.WriteLine("ClientInterface.updatedShooter started");
			var start = DateTime.Now;
			
			try
			{
				var i = 0;
				foreach(MethodInvoker thisDelegate in _UpdatedShooter.GetInvocationList())
				{
					try
					{
						int workerThreads, completionPortThreads;
						ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						while (workerThreads < minNumberOfWorkerThreads)
						{
							Trace.WriteLine("To low number of worker threads. Waiting 50 ms.");
							Thread.Sleep(50);
							ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						}

						var updatedShooterClientThread =
							new Thread(
								runDelegate)
								{
									Name = "updatedShooterClientThread " + i
								};
						updatedShooterClientThread.Start(thisDelegate);
						i++;
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedClub.Removing Handler. ");
						/*UpdatedShooter -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedShooter.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedShooter:" + 
					exc);
			}
			var span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedShooter ended (" + span.Milliseconds + " ms)");
		}
		#endregion

		#region UpdatedCompetitor
		public event MethodInvoker UpdatedCompetitor
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_UpdatedCompetitor += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_UpdatedCompetitor -= value;
				}
			}
		}
		private event MethodInvoker _UpdatedCompetitor;
		[CLSCompliant(false)]
		public void updatedCompetitor()
		{
			Trace.WriteLine("ClientInterface.updatedCompetitor started");
			var start = DateTime.Now;
			
			try
			{
				var i = 0;
				foreach(MethodInvoker thisDelegate in 
					_UpdatedCompetitor.GetInvocationList())
				{
					try
					{
						int workerThreads, completionPortThreads;
						ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						while (workerThreads < minNumberOfWorkerThreads)
						{
							Trace.WriteLine("To low number of worker threads. Waiting 50 ms.");
							Thread.Sleep(50);
							ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						}

						var updatedCompetitorClientThread =
							new Thread(
								runDelegate)
								{
									Name = "updatedCompetitorClientThread " + i
								};

						updatedCompetitorClientThread.Start(thisDelegate);
						i++;
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedClub.Removing Handler. ");
						/*UpdatedCompetitor -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedCompetitor.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetitor:" + 
					exc);
			}
			var span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedCompetitor ended (" + span.Milliseconds + " ms)");
		}
		#endregion

		#region UpdatedWeapon
		public event MethodInvoker UpdatedWeapon
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_updatedWeapon += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_updatedWeapon -= value;
				}
			}
		}
		private event MethodInvoker _updatedWeapon;
		[CLSCompliant(false)]
		public void updatedWeapon()
		{
			Trace.WriteLine("ClientInterface.updatedWeapon started");
			var start = DateTime.Now;
			
			try
			{
				var i = 0;
				foreach(MethodInvoker thisDelegate in 
					_updatedWeapon.GetInvocationList())
				{
					try
					{
						int workerThreads, completionPortThreads;
						ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						while (workerThreads < minNumberOfWorkerThreads)
						{
							Trace.WriteLine("To low number of worker threads. Waiting 50 ms.");
							Thread.Sleep(50);
							ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						}

						var updatedWeaponClientThread =
							new Thread(
								runDelegate) {Name = "updatedWeaponClientThread " + i};

						updatedWeaponClientThread.Start(thisDelegate);
						i++;
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedClub.Removing Handler. ");
						/*UpdatedWeapon -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedWeapon.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedWeapon:" + 
					exc);
			}
			var span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedWeapon ended (" + span.Milliseconds + " ms)");
		}
		#endregion

		#region UpdatedCompetition
		public event MethodInvoker UpdatedCompetition
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_UpdatedCompetition += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_UpdatedCompetition -= value;
				}
			}
		}
		private event MethodInvoker _UpdatedCompetition;
		[CLSCompliant(false)]
		public void updatedCompetition()
		{
			Trace.WriteLine("ClientInterface.updatedCompetition started");
			var start = DateTime.Now;
			
			try
			{
				int i = 0;
				foreach(MethodInvoker thisDelegate in 
					_UpdatedCompetition.GetInvocationList())
				{
					try
					{
						int workerThreads, completionPortThreads;
						ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						while (workerThreads < minNumberOfWorkerThreads)
						{
							Trace.WriteLine("To low number of worker threads. Waiting 50 ms.");
							Thread.Sleep(50);
							ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						}

						var updatedCompetitionClientThread =
							new Thread(
								runDelegate)
								{
									Name = "updatedCompetitionClientThread " + i
								};

						updatedCompetitionClientThread.Start(thisDelegate);
						i++;
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedClub.Removing Handler. ");
						/*UpdatedCompetition -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedCompetition.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetition:" + 
					exc);
			}
			var span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedCompetition ended (" + span.Milliseconds + " ms)");
		}
		#endregion

		#region UpdatedPatrol
		public event MethodInvoker UpdatedPatrol
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_UpdatedPatrol += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_UpdatedPatrol -= value;
				}
			}
		}
		private event MethodInvoker _UpdatedPatrol;
		[CLSCompliant(false)]
		public void updatedPatrol()
		{
			Trace.WriteLine("ClientInterface.updatedPatrol started");
			DateTime start = DateTime.Now;
			
			try
			{
				int i = 0;
				foreach(MethodInvoker thisDelegate in 
					_UpdatedPatrol.GetInvocationList())
				{
					try
					{
						int workerThreads, completionPortThreads;
						ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						while (workerThreads < minNumberOfWorkerThreads)
						{
							Trace.WriteLine("To low number of worker threads. Waiting 50 ms.");
							Thread.Sleep(50);
							ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						}

						var updatedPatrolClientThread =
							new Thread(
								runDelegate) {Name = "updatedPatrolClientThread " + i};

						updatedPatrolClientThread.Start(thisDelegate);
						i++;
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedClub.Removing Handler. ");
						/*UpdatedPatrol -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedPatrol.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedPatrol:" + 
					exc);
			}
			var span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedPatrol ended (" + span.Milliseconds + " ms)");
		}
		#endregion

		#region UpdatedCompetitorResult
		public event MethodInvoker UpdatedCompetitorResult
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_UpdatedCompetitorResult += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_UpdatedCompetitorResult -= value;
				}
			}
		}
		private event MethodInvoker _UpdatedCompetitorResult;
		[CLSCompliant(false)]
		public void updatedCompetitorResult()
		{
			Trace.WriteLine("ClientInterface.updatedCompetitorResult started");
			DateTime start = DateTime.Now;
			
			try
			{
				int i = 0;
				foreach(MethodInvoker thisDelegate in 
					_UpdatedCompetitorResult.GetInvocationList())
				{
					try
					{
						int workerThreads, completionPortThreads;
						ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						while (workerThreads < minNumberOfWorkerThreads)
						{
							Trace.WriteLine("To low number of worker threads. Waiting 50 ms.");
							Thread.Sleep(50);
							ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						}

						var updatedCompetitorResultClientThread =
							new Thread(
								runDelegate)
								{
									Name = "updatedCompetitorResultClientThread " + i
								};

						updatedCompetitorResultClientThread.Start(thisDelegate);
						i++;
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedClub.Removing Handler. ");
						/*UpdatedCompetitorResult -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedCompetitorResult.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetitorResult:" + 
					exc.ToString());
			}
			TimeSpan span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedCompetitorResult ended (" + span.Milliseconds.ToString() + " ms)");
		}
		#endregion

		#region UpdatedStation
		public event MethodInvoker UpdatedStation
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_UpdatedStation += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_UpdatedStation -= value;
				}
			}
		}
		private event MethodInvoker _UpdatedStation;
		[CLSCompliant(false)]
		public void updatedStation()
		{
			Trace.WriteLine("ClientInterface.updatedStation started");
			DateTime start = DateTime.Now;

			try
			{
				int i = 0;
				foreach(MethodInvoker thisDelegate in 
					_UpdatedStation.GetInvocationList())
				{
					try
					{
						int workerThreads, completionPortThreads;
						ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						while (workerThreads < minNumberOfWorkerThreads)
						{
							Trace.WriteLine("To low number of worker threads. Waiting 50 ms.");
							Thread.Sleep(50);
							ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						}

						var updatedStationClientThread =
							new Thread(
								runDelegate)
								{
									Name = "updatedStationClientThread " + i
								};

						updatedStationClientThread.Start(thisDelegate);
						i++;
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedClub.Removing Handler. ");
						/*UpdatedStation -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedStation.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedStation:" + 
					exc);
			}
			var span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedStation ended (" + span.Milliseconds + " ms)");
		}
		#endregion

		#region UpdatedTeam
		public event MethodInvoker UpdatedTeam
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_UpdatedTeam += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_UpdatedTeam -= value;
				}
			}
		}
		private event MethodInvoker _UpdatedTeam;
		[CLSCompliant(false)]
		public void updatedTeam()
		{
			Trace.WriteLine("ClientInterface.updatedTeam started");
			DateTime start = DateTime.Now;
			
			try
			{
				int i = 0;
				foreach(MethodInvoker thisDelegate in 
					_UpdatedTeam.GetInvocationList())
				{
					try
					{
						int workerThreads, completionPortThreads;
						ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						while (workerThreads < minNumberOfWorkerThreads)
						{
							Trace.WriteLine("To low number of worker threads. Waiting 50 ms.");
							Thread.Sleep(50);
							ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						}

						var updatedTeamClientThread =
							new Thread(
								runDelegate) {Name = "updatedTeamClientThread " + i};

						updatedTeamClientThread.Start(thisDelegate);
						i++;
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedTeam.Removing Handler. ");
						/*UpdatedStation -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedTeam.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedTeam:" + 
					exc);
			}
			var span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedTeam ended (" + span.Milliseconds + " ms)");
		}
		#endregion

		#region UpdatedFileImportCount
		public event UpdatedFileImportCountHandler UpdatedFileImportCount
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_UpdatedFileImportCount += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_UpdatedFileImportCount -= value;
				}
			}
		}
		private event UpdatedFileImportCountHandler _UpdatedFileImportCount;
		[CLSCompliant(false)]
		public void updatedFileImportCount(int count, int totalCount)
		{
			Trace.WriteLine("ClientInterface.updatedFileImportCount started");
			DateTime start = DateTime.Now;
			
			try
			{
				foreach(UpdatedFileImportCountHandler thisDelegate in 
					_UpdatedFileImportCount.GetInvocationList())
				{
					try
					{
						thisDelegate(count, totalCount);
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedClub.Removing Handler. ");
						/*UpdatedFileImportCount -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedFileImportCount.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface." + 
					"updatedFileImportCount:" + exc);
			}
			var span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedFileImportCount ended (" + span.Milliseconds + " ms)");
		}
		#endregion
		
		#region UpdatedPatrolAddAutomaticCompetitors
		public event UpdatedPatrolAddAutomaticCompetitorsHandler UpdatedPatrolAddAutomaticCompetitors
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add 
			{
				lock( _eventLock ) 
				{
					_UpdatedPatrolAddAutomaticCompetitors += value;
				}
			}
			remove 
			{
				lock( _eventLock ) 
				{
					_UpdatedPatrolAddAutomaticCompetitors -= value;
				}
			}
		}
		private event UpdatedPatrolAddAutomaticCompetitorsHandler _UpdatedPatrolAddAutomaticCompetitors;
		[CLSCompliant(false)]
		public void updatedPatrolAddAutomaticCompetitors(int current, int max)
		{
			Trace.WriteLine("ClientInterface.updatedPatrolAddAutomaticCompetitors started");
			DateTime start = DateTime.Now;
			
			try
			{
				foreach(UpdatedPatrolAddAutomaticCompetitorsHandler thisDelegate in 
					_UpdatedPatrolAddAutomaticCompetitors.GetInvocationList())
				{
					try
					{
						thisDelegate(current, max);
					}
					catch(WebException )
					{
						Trace.WriteLine("WebException occured during " +
							"callback of UpdatedPatrolAddAutomaticCompetitors.Removing Handler. ");
						/*UpdatedPatrolAddAutomaticCompetitors -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_UpdatedPatrolAddAutomaticCompetitors.GetInvocationList().Length);*/
					}
				}
			}
			catch(NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface." + 
					"UpdatedPatrolAddAutomaticCompetitors:" + exc);
			}
			var span = DateTime.Now - start;
			Trace.WriteLine("ClientInterface.updatedPatrolAddAutomaticCompetitors ended (" + span.Milliseconds + " ms)");
		}
		#endregion

		#endregion
		#endregion


		#region Cache methods
		[CLSCompliant(false)]
		public Structs.Shooter GetShooterFromCache(string cardNr)
		{
			try
			{
				return CommonCode.GetShooterFromCache(cardNr);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		/// <summary>
		/// Get the current number of shooters in the local cache
		/// </summary>
		/// <returns></returns>
		public int GetShooterCountInLocalCache()
		{
			try
			{
				return CommonCode.GetShooterCountInLocalCache();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		/// <summary>
		/// Get the current number of clubs in the local cache
		/// </summary>
		/// <returns></returns>
		public int GetClubCountInLocalCache()
		{
			try
			{
				return CommonCode.GetClubCountInLocalCache();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		/// <summary>
		/// Gets the content of the local xml cache file
		/// </summary>
		/// <returns></returns>
		public string GetCacheFileContent()
		{
			try
			{
				return CommonCode.GetCacheFileContent();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion

		
		#region Converters
		[CLSCompliant(false)]
		public Structs.PatrolClass ConvertWeaponsClassToPatrolClass(Structs.WeaponClass weapon)
		{
			try
			{
				return CommonCode.ConvertWeaponsClassToPatrolClass(weapon);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public Structs.ResultWeaponsClass ConvertWeaponsClassToResultClass(
			Structs.WeaponClass weapon)
		{
			try
			{
				return CommonCode.ConvertWeaponsClassToResultClass(weapon);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion


		#region Patrol Methods
		public void PatrolAddEmpty()
		{
			try
			{
				CommonCode.PatrolAddEmpty();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int PatrolGetNextLane(int patrolId)
		{
			try
			{
				return CommonCode.PatrolGetNextLane(patrolId);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int PatrolGetNextLane(int patrolId, int startlane)
		{
			try
			{
				return CommonCode.PatrolGetNextLane(patrolId, startlane);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public int PatrolGetNextLaneUp(int patrolId, int lane)
		{
			try
			{
				return CommonCode.PatrolGetNextLaneUp(patrolId, lane);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public void PatrolAddAutomaticCompetitors(bool cleanPatrols)
		{
			try
			{
				CommonCode.PatrolAddAutomaticCompetitors(cleanPatrols);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public void PatrolRemoveLast()
		{
			try
			{
				CommonCode.PatrolRemoveLast();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public bool CheckChangePatrolConnectionTypeIsPossible(
			Structs.Patrol patrol,
			Structs.PatrolConnectionTypeEnum newPatrolConnectionType)
		{
			try
			{
				return CommonCode.CheckChangePatrolConnectionTypeIsPossible(
					patrol,
					newPatrolConnectionType);
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		#endregion

		#region Result Methods
		[CLSCompliant(false)]
		public ResultsReturn ResultsGetCompetitor(Structs.Competitor competitor)
		{
			try
			{
				return CommonCode.ResultsGetCompetitor(competitor);
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		[CLSCompliant(false)]
		public ResultsReturn[] ResultsGet(Structs.ResultWeaponsClass wclass, 
			Structs.ShootersClass uclass,
			Structs.Competition competition,
			bool finalResults)
		{
			try
			{
				return CommonCode.ResultsGet(wclass, uclass, competition, finalResults);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public ResultsReturnTeam[] ResultsGetTeams(Structs.ResultWeaponsClass wclass,
			Structs.Competition competition)
		{
			try
			{
				return CommonCode.ResultsGetTeams(wclass, competition);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public Structs.ResultWeaponsClass[] ResultsGetWClasses()
		{
			try
			{
				return CommonCode.ResultsGetWClasses();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public Structs.ShootersClass[] ResultsGetUClasses(Structs.ResultWeaponsClass wclass)
		{
			try
			{
				return CommonCode.ResultsGetUClasses(wclass);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Structs.ResultWeaponsClass[] GetResultWeaponClassesWithResults()
		{
			try
			{
				return CommonCode.GetResultWeaponClassesWithResults();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion

		#region FollowUp
		public Structs.FollowUpReturn[] FollowUpByClub()
		{
			try
			{
				return FollowUpByClub();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion

		#region Internet Methods
		#region Html
		public string InternetHtmlExportPatrols()
		{
			try
			{
				return CommonCode.InternetHtmlExportPatrols();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public string InternetHtmlExportPatrolsByClub()
		{
			try
			{
				return CommonCode.InternetHtmlExportPatrolsByClub();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public string InternetHtmlExportResults(bool finalResults)
		{
			try
			{
				return CommonCode.InternetHtmlExportResults(finalResults);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public string InternetHtmlExportResults(Structs.ResultWeaponsClass wclass, 
			Structs.ShootersClass uclass, bool finalResults)
		{
			try
			{
				return CommonCode.InternetHtmlExportResults(wclass, uclass, finalResults);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion
		#region Pdf
		public byte[] InternetPdfExportPatrols()
		{
			try
			{
				return CommonCode.InternetPdfExportPatrols();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public byte[] InternetPdfExportPatrolsByClub()
		{
			try
			{
				return CommonCode.InternetPdfExportPatrolsByClub();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public byte[] InternetPdfExportResults(bool finalResults, bool allOnOnePage)
		{
			try
			{
				return CommonCode.InternetPdfExportResults(finalResults, allOnOnePage);
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public byte[] InternetPdfExportResults(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass, bool finalResults)
		{
			try
			{
				return CommonCode.InternetPdfExportResults(wclass, uclass, finalResults);
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion
		#region Excel
		public byte[] InternetExcelExportPatrols()
		{
			try
			{
				return CommonCode.InternetExcelExportPatrols();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public byte[] InternetExcelExportPatrolsByClub()
		{
			try
			{
				return CommonCode.InternetExcelExportPatrolsByClub();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public byte[] InternetExcelExportResults(bool finalResults )
		{
			try
			{
				return CommonCode.InternetExcelExportResults(finalResults);
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public byte[] InternetExcelExportResults(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass, bool finalResults)
		{
			try
			{
				return CommonCode.InternetExcelExportResults(wclass, uclass, finalResults);
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion
		#region Xml
		[CLSCompliant(false)]
		public byte[] InternetXmlExport()
		{
			try
			{
				return CommonCode.InternetXmlExport();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion
		#region Text
		[CLSCompliant(false)]
		public byte[] InternetTextExport()
		{
			try
			{
				return CommonCode.InternetTextExport();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public byte[] InternetWeaponsExport()
		{
			try
			{
				return CommonCode.InternetWeaponsExport();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		[CLSCompliant(false)]
		public byte[] InternetClubsExport()
		{
			try
			{
				return CommonCode.InternetClubsExport();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion
		#endregion

		#region Client Methods
		private event MethodInvoker _syncronizeEvent;
		public event MethodInvoker SyncronizeEvent
		{
			// The add/remove operations are performed while
			// holding the lock on the private _OnFoo field.
			//
			add
			{
				lock (_eventLock)
				{
					_syncronizeEvent += value;
				}
			}
			remove
			{
				lock (_eventLock)
				{
					_syncronizeEvent -= value;
				}
			}
		}

		[CLSCompliant(false)]
		public void RunSyncronizeEvent()
		{
			Trace.WriteLine("ClientInterface.runSyncronizeEvent started");
			try
			{
				int i = 0;
				foreach (MethodInvoker thisDelegate in
					_syncronizeEvent.GetInvocationList())
				{
					try
					{
						int workerThreads, completionPortThreads;
						ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						while (workerThreads < minNumberOfWorkerThreads)
						{
							Trace.WriteLine("To low number of worker threads. Waiting 50 ms.");
							Thread.Sleep(50);
							ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
						}

						var runSyncClientThread =
							new Thread(
							runDelegate) {Name = "runSyncClientThread " + i};
						runSyncClientThread.Start(thisDelegate);
						i++;
					}
					catch (WebException)
					{
						Trace.WriteLine("WebException occured during " +
							"callback of runSyncClientThread.Removing Handler. ");
						/*_syncronizeEvent -= thisDelegate;
						Trace.WriteLine("Number of eventhandlers is now: " + 
							_syncronizeEvent.GetInvocationList().Length);*/
					}
				}
			}
			catch (NullReferenceException)
			{
				// occurs when there is no delegate
			}
			catch (Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetitorResult:" +
					exc);
			}
			Trace.WriteLine("ClientInterface.updatedCompetitorResult ended");
		}

		private void runDelegate(object thisDelegateObject)
		{
			var thisDelegate = (MethodInvoker)thisDelegateObject;
			try
			{
				thisDelegate();
			}
			catch (Exception exc)
			{
				Trace.WriteLine("Exception in delegate " + 
					thisDelegate.GetType() + ": " + exc);
			}
		}

		readonly Hashtable clients = Hashtable.Synchronized(new Hashtable());
		int _ticker;
		public void Sync()
		{
			try
			{
				var ip = (IPAddress)CallContext.GetData("ClientIPAddress");
				if (ip != null)
				{
					if (clients.Contains(ip))
					{
						clients[ip] = DateTime.Now;
					}
					else
					{
						clients.Add(ip, DateTime.Now);
					}
					Trace.WriteLine("Sync from " + ip);
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine("Sync exception" + exc);
			}
			finally
			{
				try
				{
					_ticker++;
					if (_ticker > 100)
					{
						foreach(var obj in clients.Keys)
						{
							var time = (DateTime)clients[obj];
							var diff = DateTime.Now-time;
							if (diff.TotalSeconds > 3*60)
							{
								clients.Remove(obj);
							}
						}
						_ticker = 0;
					}
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.ToString());
				}
			}

			try
			{
				RunSyncronizeEvent();
			}
			catch (Exception exc)
			{
				Trace.WriteLine("Sync callback exception" + exc);
			}
		}
		public void Sync(bool local)
		{
			try
			{
				GetCompetitions();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Hashtable SyncClients()
		{
			try
			{
				return clients;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		public Version Sync(Version version)
		{
			Trace.WriteLine("ClientInterface: Sync(" + version + 
				") started from thread \"" + 
				Thread.CurrentThread.Name + "\" " +
				" ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
				DateTime.Now.ToLongTimeString());

			try
			{
				Trace.WriteLine("ClientInterface: SyncOpenAccessDatabase started from thread \"" + 
					Thread.CurrentThread.Name + "\" " +
					" ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
					DateTime.Now.ToLongTimeString());

				if (version< new Version(MinimumClientVersionString))
				{
					Trace.WriteLine("ClientInterface: Sync(\"" + version + 
						"\"): Client version is to low.");

					throw new ApplicationException("Klienten har version " + 
						version.ToString() + " vilket är för lågt för att denna " + 
						"klient ska kunna köra mot servern som har version " + 
						Assembly.GetExecutingAssembly().GetName().Version +
						". Uppdatera klienten med " + 
						"den senaste programvaran.");
				}
				Trace.WriteLine("ClientInterface: Sync(\"" + version +
					"\") returns " +
					Assembly.GetExecutingAssembly()
						.GetName().Version);

				try
				{
					var connId = (Int64)CallContext.GetData("ClientConnectionId");
					var newThreadName = "ClientThread " + connId;
					Trace.WriteLine("Changing thread name to \"" + newThreadName +
						"\"");
					Thread.CurrentThread.Name = newThreadName;
					Trace.WriteLine("ClientInterface: Sync: ThreadName changed successfully.");
				}
				catch(Exception exc)
				{
					Trace.WriteLine("ClientInterface: Sync: ThreadName change failed:" +
						exc);
				}

				return Assembly.GetExecutingAssembly().GetName().Version;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		/// <summary>
		/// Run a backup
		/// </summary>
		/// <param name="filename"></param>
		public void Backup(string filename)
		{
			try
			{
				CommonCode.Backup(filename);
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}
		#endregion



	}
}
