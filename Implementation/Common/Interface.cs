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
// $Id: Interface.cs 131 2011-05-28 17:38:19Z smuda $
using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Windows.Forms;
using Allberg.Shooter.WinShooterServerRemoting;
using System.Threading;

namespace Allberg.Shooter.Common
{
	/// <summary>
	/// This is the class that everything into the common code goes through.
	/// It implements the InterfaceDefinition
	/// </summary>
	//[NonSerialized]
	[CLSCompliant(false)]
	public class Interface : IWinshooterServer, IDisposable
	{
		/// <summary>
		/// Initiate the class
		/// </summary>
		public Interface()
		{
			databaseClass = new CDatabase(this);
			convertClass = new CConvert();
			patrolClass = new CPatrolManagement(this);
			resultClass = new CResultCache(this);
			resultTeamClass = new CResultTeam(this);
			internetHtmlExportClass = new CInternetHtmlExport(this);
			internetPdfExportClass = new CInternetPdfExport(this);
			internetExcelExportClass = new CInternetExcelExport(this);
			cacheClass = new Cache();
			fileImportClass = new CFileImport(this);
			internetXmlExportClass = new CInternetXmlExport(this);
			internetTextExportClass = new CInternetTextExport(this);

			
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (serverInterface != null)
				{
					Trace.WriteLine("Disposing server connection");
					serverInterface.UpdatedClub -= MethodInvokerUpdatedClub;
					serverInterface.UpdatedCompetition -= MethodInvokerUpdatedCompetition;
					serverInterface.UpdatedCompetitor -= MethodInvokerUpdatedCompetitor;
					serverInterface.UpdatedCompetitorResult -= MethodInvokerUpdatedCompetitorResult;
					serverInterface.UpdatedPatrol -= MethodInvokerUpdatedPatrol;
					serverInterface.UpdatedShooter -= MethodInvokerUpdatedShooter;
					serverInterface.UpdatedStation -= MethodInvokerUpdatedStation;
					serverInterface.UpdatedWeapon -= MethodInvokerUpdatedWeapon;
					serverInterface.UpdatedTeam -= MethodInvokerUpdatedTeam;
				}
			}
		}

		// TODO Enable this
		private bool enableInternetConnections = false;

		private const string minimumServerVersionString = "1.6.0";

		/// <summary>
		/// Determines if Internet Database should be enabled.
		/// </summary>
		public bool EnableInternetConnections
		{
			get
			{
				if (serverInterface != null)
					return serverInterface.EnableInternetConnections;
				else
					return enableInternetConnections;
			}
			set
			{
				if (serverInterface != null)
					serverInterface.EnableInternetConnections = value;
				else
					enableInternetConnections = value;
			}
		}

		/// <summary>
		/// Defines what type of patrols is to be used
		/// </summary>
		[CLSCompliant(false)]
		public Structs.PatrolConnectionTypeEnum PatrolConnectionType
		{
			get
			{
				return CompetitionCurrent.PatrolConnectionType;
			}
			set
			{
				Structs.Competition comp = CompetitionCurrent;
				comp.PatrolConnectionType = value;
				UpdateCompetition(comp);
			}
		}

		CSettings settings = CSettings.Instance;
		public Allberg.Shooter.WinShooterServerRemoting.ISettings Settings
		{
			get
			{
				return settings;
			}
			set
			{
				settings.Update(value);
			}
		}
		public string CurrentFilename
		{
			get
			{
				return currentFile;
			}
		}

		internal string currentFile = null;
		internal string connectionString = "";
		internal int debugLevel = 0;
		internal CDatabase databaseClass;
		internal CConvert convertClass;
		internal CPatrolManagement patrolClass;
		internal CResultCache resultClass;
		internal CResultTeam resultTeamClass;
		internal CInternetHtmlExport internetHtmlExportClass;
		internal CInternetPdfExport internetPdfExportClass;
		internal CInternetExcelExport internetExcelExportClass;
		internal CInternetXmlExport internetXmlExportClass;
		internal CInternetTextExport internetTextExportClass;
		internal Cache cacheClass;
		internal CFileImport fileImportClass;
		

		#region Database Methods
		#region Database Init

		/// <summary>
		/// Creates a new access database
		/// </summary>
		/// <param name="PathAndFilename">The filename with path to the new database</param>
		public void CreateAccessDatabase(string PathAndFilename)
		{
			if (serverInterface != null)
				throw new ApplicationException("Cannot create via server");
			else
				databaseClass.createAccessDatabase(PathAndFilename);
		}
		/// <summary>
		/// Creates the database content
		/// </summary>
		public void CreateDefaultDatabaseContent()
		{
			if (serverInterface != null)
				throw new ApplicationException("Cannot create via server");
			else
				databaseClass.CreateDefaultDatabaseContent();
		}
		/// <summary>
		/// Opens an Access database
		/// </summary>
		/// <param name="PathAndFilename">The filename</param>
		public void OpenAccessDatabase(string PathAndFilename)
		{
			if (serverInterface != null)
				throw new ApplicationException("Cannot create via server");
			else
				databaseClass.OpenAccessDatabase(PathAndFilename);
		}
		/// <summary>
		/// Opens a database
		/// </summary>
		public void OpenDatabase()
		{
			if (serverInterface != null)
			{
				serverInterface.Sync();
				serverInterface.OpenDatabase();
			}
			else
			{
				if (connectionString=="")
				{
					throw new ArgumentException("ConnectionString has not been " +
						"properly initialized.");
				}
				databaseClass.OpenDatabase();
			}
		}

		#endregion
		#region Get stuff
		/// <summary>
		/// Returns a clubs with a clubid
		/// </summary>
		/// <param name="ClubId">clubid</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Club GetClub(string ClubId)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetClub(ClubId);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to fetch Clubs");

				return databaseClass.GetClub(ClubId);
			}
		}
		/// <summary>
		/// Returns all clubs
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Club[] GetClubs()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetClubs();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to fetch Clubs");

				return databaseClass.getClubs();
			}
		}
		/// <summary>
		/// Returns all shooters, sorted by givenname, surname
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Shooter[] GetShooters()
		{
			return GetShooters("givenname, surname");
		}
		/// <summary>
		/// Returns all shooters with custom sorting
		/// </summary>
		/// <param name="sorting">sorting</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Shooter[] GetShooters(string sorting)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetShooters(sorting);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Shooters");

				return databaseClass.GetShooters(sorting);
			}
		}
		/// <summary>
		/// Returns all shooters for a certain club
		/// </summary>
		/// <param name="ClubToFetch">club</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Shooter[] GetShooters(Structs.Club ClubToFetch)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetShooters(ClubToFetch);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Shooters");

				return databaseClass.GetShooters(ClubToFetch);
			}
		}
		/// <summary>
		/// Returns all shooters for a certain club
		/// </summary>
		/// <param name="ClubToFetch">club</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Shooter[] GetShooters(Structs.Club ClubToFetch, Structs.ResultWeaponsClass wclass)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetShooters(ClubToFetch, wclass);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Shooters");

				return databaseClass.GetShooters(ClubToFetch, wclass);
			}
		}
		/// <summary>
		/// Returns a shooter with a certain shooterid
		/// </summary>
		/// <param name="ShooterId">shooterid</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Shooter GetShooter(int ShooterId)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetShooter(ShooterId);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Shooters");

				return databaseClass.GetShooter(ShooterId);
			}
		}
		/// <summary>
		/// Returns the highets shooterid
		/// </summary>
		/// <returns></returns>
		public int GetShooterHighestId()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetShooterHighestId();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Shooters");

				return databaseClass.GetShooterHighestId();
			}
		}
		/// <summary>
		/// Returns a shooter
		/// </summary>
		/// <param name="CardNr">Cardnr</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Shooter GetShooter(string CardNr)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetShooter(CardNr);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Shooters");

				return databaseClass.GetShooter(CardNr);
			}
		}
		/// <summary>
		/// Returns all competitors
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Competitor[] GetCompetitors()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitors();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.getCompetitors();
			}
		}
		/// <summary>
		/// GetCompetitors for a shooter with custom sorting
		/// </summary>
		/// <param name="ShooterId">shooter</param>
		/// <param name="Sorting">sortin</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Competitor[] GetCompetitors(int ShooterId, string Sorting)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitors(ShooterId, Sorting);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.getCompetitors(ShooterId, Sorting);
			}
		}
		/// <summary>
		/// returns all competitors for a shooter
		/// </summary>
		/// <param name="ShooterId">shooter</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Competitor[] GetCompetitors(int ShooterId)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitors(ShooterId);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.getCompetitors(ShooterId);
			}
		}
		/// <summary>
		/// Returns all competitors for a patrol
		/// </summary>
		/// <param name="PatrolToFetch">patrol</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Competitor[] GetCompetitors(Structs.Patrol PatrolToFetch)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitors(PatrolToFetch);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return GetCompetitors(PatrolToFetch, "");
			}
		}
		/// <summary>
		/// Returns competitors for a patrol with custom sorting
		/// </summary>
		/// <param name="PatrolToFetch">patrol</param>
		/// <param name="sorting">sorting</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Competitor[] GetCompetitors(Structs.Patrol PatrolToFetch, string sorting)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitors(PatrolToFetch, sorting);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.getCompetitors(PatrolToFetch, sorting);
			}
		}
		/// <summary>
		/// Returns competitors with no patrol
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Competitor[] GetCompetitorsWithNoPatrol()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitorsWithNoPatrol();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.GetCompetitorsWithNoPatrol(Structs.PatrolClass.Okänd);
			}
		}
		/// <summary>
		/// Returns competitors with no patrol for a class
		/// </summary>
		/// <param name="thisClass"></param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Competitor[] GetCompetitorsWithNoPatrol(Structs.PatrolClass thisClass)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitorsWithNoPatrol(thisClass);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.GetCompetitorsWithNoPatrol(thisClass);
			}
		}
		/// <summary>
		/// Returns all competitors for a certain club
		/// </summary>
		/// <param name="ClubToFetch">club</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Competitor[] GetCompetitors(Structs.Club ClubToFetch, Structs.ResultWeaponsClass wclass, string sorting)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitors(ClubToFetch, wclass, sorting);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Shooters");

				return databaseClass.GetCompetitors(ClubToFetch, wclass, sorting);
			}
		}
		/// <summary>
		/// Returns a competitors
		/// </summary>
		/// <param name="CompetitorId">competitorid</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Competitor GetCompetitor(int CompetitorId)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitor(CompetitorId);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.getCompetitor(CompetitorId);
			}
		}
		/// <summary>
		/// Returns all weapons
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Weapon[] GetWeapons()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetWeapons();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Weapons");

				return databaseClass.getWeapons("WeaponId");
			}
		}
		/// <summary>
		/// Returns all weapons with custom sorting
		/// </summary>
		/// <param name="sorting">sorting</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Weapon[] GetWeapons(string sorting)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetWeapons(sorting);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Weapons");

				return databaseClass.getWeapons(sorting);
			}
		}
		/// <summary>
		/// Returns weapon with weaponid
		/// </summary>
		/// <param name="WeaponsId">weaponid</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Weapon GetWeapon(string WeaponsId)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetWeapon(WeaponsId);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Weapons");

				return databaseClass.getWeapon(WeaponsId);
			}
		}
		/// <summary>
		/// CurrentCompetition
		/// </summary>
		public Structs.Competition CompetitionCurrent
		{
			get
			{
				return GetCompetitions()[0];
			}
		}
		/// <summary>
		/// Returns all competitions
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Competition[] GetCompetitions()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitions();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitions");

				return databaseClass.getCompetitions();
			}
		}
		/// <summary>
		/// Returns a patrol
		/// </summary>
		/// <param name="Id">patrolid</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Patrol GetPatrol(int Id)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetPatrol(Id);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Patrols");

				return databaseClass.getPatrol(Id);
			}
		}
		/// <summary>
		/// Returns all patrols
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Patrol[] GetPatrols()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetPatrols();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Patrols");

				return databaseClass.getPatrols();
			}
		}
		/// <summary>
		/// Returns all patrols for a class
		/// </summary>
		/// <param name="patrolClass">class</param>
		/// <param name="AlsoIncludeUnknownClass">if unknown (empty) patrols should be included</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Patrol[] GetPatrols(Structs.PatrolClass patrolClass, 
			bool AlsoIncludeUnknownClass)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetPatrols(patrolClass, AlsoIncludeUnknownClass);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Patrols");

				return databaseClass.getPatrols(patrolClass, 
					AlsoIncludeUnknownClass, 
					false,
					-1);
			}
		}
		/// <summary>
		/// Returns all patrols for a class
		/// </summary>
		/// <param name="patrolClass">class</param>
		/// <param name="AlsoIncludeUnknownClass">if unknown (empty) patrols should be included</param>
		/// <param name="OnlyIncludePatrolsWithSpace">If only patrols with space should be included</param>
		/// <param name="PatrolIdToAlwaysView">The current patrol, even </param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Patrol[] GetPatrols(Structs.PatrolClass patrolClass, 
			bool AlsoIncludeUnknownClass,
			bool OnlyIncludePatrolsWithSpace,
			int PatrolIdToAlwaysView)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetPatrols(patrolClass,
					AlsoIncludeUnknownClass, OnlyIncludePatrolsWithSpace,
					PatrolIdToAlwaysView);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Patrols");

				return databaseClass.getPatrols(patrolClass, 
					AlsoIncludeUnknownClass, OnlyIncludePatrolsWithSpace,
					PatrolIdToAlwaysView);
			}
		}
		/// <summary>
		/// Returns all competitorresults
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.CompetitorResult[] GetCompetitorResults()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitorResults();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Results");

				return databaseClass.getCompetitorResults();
			}
		}
		/// <summary>
		/// Returns competitorresults for a competitor
		/// </summary>
		/// <param name="CompetitorsId">CompetitorsId</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.CompetitorResult[] GetCompetitorResults(int CompetitorsId)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitorResults(CompetitorsId);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Results");

				return databaseClass.getCompetitorResults(CompetitorsId);
			}
		}
		/// <summary>
		/// Returns competitorresults for a competitor and a station
		/// </summary>
		/// <param name="CompetitorsId">CompetitorsId</param>
		/// <param name="StationId">StationId</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.CompetitorResult GetCompetitorResult(int CompetitorsId, int StationId)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetCompetitorResult(CompetitorsId, StationId);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Results");

				return databaseClass.getCompetitorResult(CompetitorsId, StationId);
			}
		}
		/// <summary>
		/// Returns all stations
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Station[] GetStations()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetStations();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Stations");

				return databaseClass.getStations();
			}
		}

				/// <summary>
		/// Returns all stations
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Station[] GetStationsDistinguish()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetStationsDistinguish();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Stations");

				return databaseClass.getStationsDistinguish();
			}
		}
						  
		/// <summary>
		/// Returns all stations
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Station GetStation(int StationNr, bool Distinguish)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetStation(StationNr, Distinguish);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Stations");

				return databaseClass.getStation(StationNr, Distinguish);
			}
		}
						  
		/// <summary>
		/// Returns all teams
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Team[] GetTeams()
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetTeams();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Teams");

				return databaseClass.getTeams();
			}
		}

		/// <summary>
		/// Returns one team
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public Structs.Team GetTeam(int TeamId)
		{
			if (serverInterface != null)
			{
				//serverInterface.Sync();
				return serverInterface.GetTeam(TeamId);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Team");

				return databaseClass.getTeam(TeamId);
			}
		}
						  

		#endregion

		#region Get stuff count
		/// <summary>
		/// Counts the clubs
		/// </summary>
		/// <returns></returns>
		public int GetClubsCount()
		{
			if (serverInterface != null)
				return serverInterface.GetClubsCount();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to fetch Clubs");

				return databaseClass.getClubsCount();
			}
		}
		/// <summary>
		/// Returns the number of clubs with shooters
		/// </summary>
		/// <returns></returns>
		public int GetClubsCountWithShooters()
		{
			if (serverInterface != null)
				return serverInterface.GetClubsCountWithShooters();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to fetch Clubs");

				return databaseClass.getClubsCountWithShooters();
			}
		}
		/// <summary>
		/// Counts the shooters
		/// </summary>
		/// <returns></returns>
		public int GetShootersCount()
		{
			if (serverInterface != null)
				return serverInterface.GetShootersCount();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Shooters");

				return databaseClass.getShootersCount();
			}
		}
		/// <summary>
		/// Counts the competitors
		/// </summary>
		/// <returns></returns>
		public int GetCompetitorsCount()
		{
			if (serverInterface != null)
				return serverInterface.GetCompetitorsCount();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.getCompetitorsCount();
			}
		}
		/// <summary>
		/// Counts the competitors in one patrol
		/// </summary>
		/// <param name="patrol">patrol</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public int GetCompetitorsCountPatrol(Structs.Patrol patrol)
		{
			if (serverInterface != null)
				return serverInterface.GetCompetitorsCountPatrol(patrol);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.getCompetitorsCountPatrolId(patrol);
			}
		}
		/// <summary>
		/// Counts the competitors in one patrol
		/// </summary>
		/// <param name="patrol">patrol</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public int GetCompetitorsWithResultCountPatrol(Structs.Patrol patrol)
		{
			if (serverInterface != null)
				return serverInterface.GetCompetitorsWithResultCountPatrol(patrol);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.getCompetitorsWithResultCountPatrol(patrol);
			}
		}
		/// <summary>
		/// Counts the competitors in one patrol
		/// </summary>
		/// <param name="patrol">patrol</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public int GetCompetitorsArrivedCountPatrol(Structs.Patrol patrol)
		{
			if (serverInterface != null)
				return serverInterface.GetCompetitorsArrivedCountPatrol(patrol);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.GetCompetitorsArrivedCountPatrol(patrol);
			}
		}
		
		/// <summary>
		/// Counts the weapons
		/// </summary>
		/// <returns></returns>
		public int GetWeaponsCount()
		{
			if (serverInterface != null)
				return serverInterface.GetWeaponsCount();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Weapons");

				return databaseClass.getWeaponsCount();
			}
		}
		/// <summary>
		/// Counts the competitions
		/// </summary>
		/// <returns></returns>
		public int GetCompetitionsCount()
		{
			if (serverInterface != null)
				return serverInterface.GetCompetitionsCount();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Competitors");

				return databaseClass.getCompetitionsCount();
			}
		}
		/// <summary>
		/// Counts the patrols
		/// </summary>
		/// <returns></returns>
		public int GetPatrolsCount()
		{
			if (serverInterface != null)
				return serverInterface.GetPatrolsCount();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Patrols");

				return databaseClass.getPatrolsCount();
			}
		}
		/// <summary>
		/// Counts the competitorresults
		/// </summary>
		/// <returns></returns>
		public int GetCompetitorResultsCount()
		{
			if (serverInterface != null)
				return serverInterface.GetCompetitorResultsCount();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Results");

				return databaseClass.getCompetitorResultsCount();
			}
		}
		/// <summary>
		/// Counts the competitorresults for a wclass and shooterclass
		/// </summary>
		/// <param name="wclass">wclass</param>
		/// <param name="uclass">uclass</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public int GetCompetitorResultsCount(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass)
		{
			if (serverInterface != null)
				return serverInterface.GetCompetitorResultsCount(wclass, uclass);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Results");

				return databaseClass.getCompetitorResultsCount(wclass, uclass);
			}
		}
		/// <summary>
		/// Counts the competitorresults for a wclass and shooterclass
		/// </summary>
		/// <param name="wclass">wclass</param>
		/// <param name="uclass">uclass</param>
		/// <param name="clubId">clubid</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public int GetCompetitorResultsCount(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass, string clubId)
		{
			if (serverInterface != null)
				return serverInterface.GetCompetitorResultsCount(wclass, uclass, clubId);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Results");

				return databaseClass.getCompetitorResultsCount(wclass, uclass, clubId);
			}
		}
		/// <summary>
		/// Checks for competitorsresults for a wclass
		/// </summary>
		/// <param name="wclass">wclass</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public bool GetCompetitorResultsExist(Structs.ResultWeaponsClass wclass)
		{
			if (serverInterface != null)
				return serverInterface.GetCompetitorResultsExist(wclass);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Results");

				return databaseClass.getCompetitorResultsExist(wclass);
			}
		}
		/// <summary>
		/// Checks for competitorsresults for a wclass/uclass
		/// </summary>
		/// <param name="wclass">wclass</param>
		/// <param name="uclass">uclass</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public bool GetCompetitorResultsExist(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass)
		{
			if (serverInterface != null)
				return serverInterface.GetCompetitorResultsExist(wclass, uclass);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Results");

				return databaseClass.getCompetitorResultsExist(wclass, uclass);
			}
		}
		/// <summary>
		/// Counts the number of stations
		/// </summary>
		/// <returns></returns>
		public int GetStationsCount()
		{
			if (serverInterface != null)
				return serverInterface.GetStationsCount();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to fetch Results");

				return databaseClass.getStationsCount();
			}
		}
		#endregion


		#region Update Stuff
		/// <summary>
		/// Updates a club
		/// </summary>
		/// <param name="club">club</param>
		[CLSCompliant(false)]
		public void UpdateClub(Structs.Club club)
		{
			if (serverInterface != null)
				serverInterface.UpdateClub(club);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Clubs");

				databaseClass.updateClub(club);
				cacheClass.UpdateClubInCache(club);
			}
		}
		/// <summary>
		/// Updates a shooter
		/// </summary>
		/// <param name="shooter">shooter</param>
		[CLSCompliant(false)]
		public void UpdateShooter(Structs.Shooter shooter)
		{
			if (serverInterface != null)
				serverInterface.UpdateShooter(shooter);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Shooters");

				databaseClass.updateShooter(shooter);
				cacheClass.UpdateShooterInCache(shooter);
			}
		}
		/// <summary>
		/// Updates a competitor
		/// </summary>
		/// <param name="competitor">competitor</param>
		[CLSCompliant(false)]
		public void UpdateCompetitor(Structs.Competitor competitor)
		{
			if (serverInterface != null)
				serverInterface.UpdateCompetitor(competitor);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update " +
						"Competitors");

				databaseClass.updateCompetitor(competitor, true);
			}
		}
		internal void UpdateCompetitor(Structs.Competitor competitor, bool updateGui)
		{
			if (serverInterface != null)
				throw new ApplicationException("Not supported while connected to server");
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update " +
						"Competitors");

				databaseClass.updateCompetitor(competitor, updateGui);
			}
		}
		/// <summary>
		/// Updates a weapon
		/// </summary>
		/// <param name="weapon">weapon</param>
		[CLSCompliant(false)]
		public void UpdateWeapon(Structs.Weapon weapon)
		{
			if (serverInterface != null)
				serverInterface.UpdateWeapon(weapon);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update " +
						"Weapons");

				databaseClass.updateWeapon(weapon);
			}
		}
		/// <summary>
		/// Updates a competition
		/// </summary>
		/// <param name="competition">competition</param>
		[CLSCompliant(false)]
		public void UpdateCompetition(Structs.Competition competition)
		{
			if (serverInterface != null)
				serverInterface.UpdateCompetition(competition);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update " +
						"Competitions");

				databaseClass.updateCompetition(competition);
			}
		}
		/// <summary>
		/// Updates a patrol
		/// </summary>
		/// <param name="patrol">patrol</param>
		[CLSCompliant(false)]
		public void UpdatePatrol(Structs.Patrol patrol)
		{
			if (serverInterface != null)
				serverInterface.UpdatePatrol(patrol);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update " +
						"Competitions");

				UpdatePatrol(patrol, true);
			}
		}
		internal void UpdatePatrol(Structs.Patrol patrol, bool updateGui)
		{
			if (serverInterface != null)
				throw new ApplicationException("Not supported while connected to server");
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update " +
						"Patrols");

				databaseClass.updatePatrol(patrol, updateGui);
			}
		}
		/// <summary>
		/// Updates a competitorResult
		/// </summary>
		/// <param name="competitorResult">competitorResult</param>
		/// <param name="UpdateInterface">if interface and database should be updated.</param>
		[CLSCompliant(false)]
		public void UpdateCompetitorResult(Structs.CompetitorResult 
			competitorResult, bool UpdateInterface)
		{
			if (serverInterface != null)
				serverInterface.UpdateCompetitorResult(competitorResult,
					UpdateInterface);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update " +
						"CompetitorResult");

				databaseClass.updateCompetitorResult(competitorResult,
					UpdateInterface);
			}
		}
		/// <summary>
		/// Updates a station
		/// </summary>
		/// <param name="station">station</param>
		[CLSCompliant(false)]
		public void UpdateStation(Structs.Station station)
		{
			if (serverInterface != null)
				serverInterface.UpdateStation(station);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update " +
						"CompetitorResult");

				databaseClass.updateStation(station);
			}
		}

		/// <summary>
		/// Updates a Team
		/// </summary>
		/// <param name="team">team</param>
		[CLSCompliant(false)]
		public void UpdateTeam(Structs.Team team)
		{
			if (serverInterface != null)
				serverInterface.UpdateTeam(team);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update " +
						"team");

				databaseClass.updateTeam(team);
			}
		}
		#endregion

		#region Add stuff
		/// <summary>
		/// Adds a club
		/// </summary>
		/// <param name="club">club</param>
		[CLSCompliant(false)]
		public void NewClub(Structs.Club club)
		{
			if (serverInterface != null)
				serverInterface.NewClub(club);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Clubs");

				cacheClass.UpdateClubInCache(club);
				databaseClass.newClub(club);
			}
		}
		/// <summary>
		/// Adds a shooter
		/// </summary>
		/// <param name="shooter">shooter</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public int NewShooter(Structs.Shooter shooter)
		{
			return NewShooter(shooter, true);
		}
		internal int NewShooter(Structs.Shooter shooter, bool updateGui)
		{
			if (serverInterface != null)
				return serverInterface.NewShooter(shooter);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Shooters");

				cacheClass.UpdateShooterInCache(shooter);
				return databaseClass.newShooter(shooter, updateGui);
			}
		}
		/// <summary>
		/// Adds a competitor
		/// </summary>
		/// <param name="competitor">competitor</param>
		[CLSCompliant(false)]
		public void NewCompetitor(Structs.Competitor competitor)
		{
			NewCompetitor(competitor, true);
		}
		internal void NewCompetitor(Structs.Competitor competitor, bool updateGui)
		{
			if (serverInterface != null)
				serverInterface.NewCompetitor(competitor);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Clubs");

				databaseClass.newCompetitor(competitor, updateGui);
			}
		}
		/// <summary>
		/// Adds a weapon
		/// </summary>
		/// <param name="weapon">weapon</param>
		[CLSCompliant(false)]
		public void NewWeapon(Structs.Weapon weapon)
		{
			if (serverInterface != null)
				serverInterface.NewWeapon(weapon);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Clubs");

				databaseClass.newWeapon(weapon);
			}
		}
		/// <summary>
		/// Adds competition
		/// </summary>
		/// <param name="comp">competition</param>
		[CLSCompliant(false)]
		public void NewCompetition(Structs.Competition comp)
		{
			if (serverInterface != null)
				serverInterface.NewCompetition(comp);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Clubs");

				databaseClass.newCompetition(comp);
			}
		}
		/// <summary>
		/// adds a patrol
		/// </summary>
		/// <param name="patrol">patrol</param>
		/// <param name="NotifyGui">If to notify gui</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public int NewPatrol(Structs.Patrol patrol, bool NotifyGui)
		{
			if (serverInterface != null)
				return serverInterface.NewPatrol(patrol, NotifyGui);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Clubs");

				return databaseClass.newPatrol(patrol, NotifyGui);
			}
		}
		/// <summary>
		/// Adds a competitorResult
		/// </summary>
		/// <param name="res">competitorResult</param>
		[CLSCompliant(false)]
		public void NewCompetitorResult(Structs.CompetitorResult res)
		{
			if (serverInterface != null)
				serverInterface.NewCompetitorResult(res);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Clubs");

				databaseClass.newCompetitorResult(res);
			}
		}
		/// <summary>
		/// Adds a station
		/// </summary>
		/// <param name="station">station</param>
		[CLSCompliant(false)]
		public void NewStation(Structs.Station station, bool distinguish)
		{
			if (serverInterface != null)
				serverInterface.NewStation(station, distinguish);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Stations");

				databaseClass.newStation(station, distinguish);
			}
		}
		/// <summary>
		/// Adds a team
		/// </summary>
		/// <param name="team">team</param>
		[CLSCompliant(false)]
		public void NewTeam(Structs.Team team)
		{
			if (serverInterface != null)
				serverInterface.NewTeam(team);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to update Stations");

				databaseClass.newTeam(team);
			}
		}
		#endregion

		#region Remove stuff
		/// <summary>
		/// Removes a club
		/// </summary>
		/// <param name="club">club</param>
		[CLSCompliant(false)]
		public void DelClub(Structs.Club club)
		{
			if (serverInterface != null)
				serverInterface.DelClub(club);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to remove Club");

				databaseClass.delClub(club);
			}
		}
		/// <summary>
		/// Removes a shooter
		/// </summary>
		/// <param name="shooter"></param>
		[CLSCompliant(false)]
		public void DelShooter(Structs.Shooter shooter)
		{
			if (serverInterface != null)
				serverInterface.DelShooter(shooter);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to remove Shooter");

				databaseClass.delShooter(shooter);
			}
		}
		/// <summary>
		/// Removes a competitor
		/// </summary>
		/// <param name="competitor">competitor</param>
		[CLSCompliant(false)]
		public void DelCompetitor(Structs.Competitor competitor)
		{
			DelCompetitor(competitor, true);
		}
		/// <summary>
		/// Removes a competitor
		/// </summary>
		/// <param name="competitor">competitor</param>
		/// <param name="UpdateGui">if to update gui</param>
		[CLSCompliant(false)]
		public void DelCompetitor(Structs.Competitor competitor, bool UpdateGui)
		{
			if (serverInterface != null)
				serverInterface.DelCompetitor(competitor);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to remove Competitor");

				databaseClass.delCompetitor(competitor, UpdateGui);
			}
		}
		/// <summary>
		/// Removes a weapon
		/// </summary>
		/// <param name="weapon">weapon</param>
		public void DelWeapon(Structs.Weapon weapon)
		{
			if (serverInterface != null)
				serverInterface.DelWeapon(weapon);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to remove Weapon");

				databaseClass.delWeapon(weapon);
			}
		}
		/// <summary>
		/// Removes a competition
		/// </summary>
		/// <param name="competition">competition</param>
		public void DelCompetition(Structs.Competition competition)
		{
			if (serverInterface != null)
				serverInterface.DelCompetition(competition);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to remove Competition");

				databaseClass.delCompetition(competition);
			}
		}
		/// <summary>
		/// Removes a patrol
		/// </summary>
		/// <param name="patrol">patrol</param>
		public void DelPatrol(Structs.Patrol patrol)
		{
			this.DelPatrol(patrol, true);
		}
		internal void DelPatrol(Structs.Patrol patrol, bool updateGui)
		{
			if (serverInterface != null)
				serverInterface.DelPatrol(patrol);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to remove Patrol");

				databaseClass.delPatrol(patrol, updateGui);
			}
		}
		/// <summary>
		/// Removes a competitorresult
		/// </summary>
		/// <param name="competitorResult">competitorresult</param>
		public void DelCompetitorResult(Structs.CompetitorResult 
			competitorResult)
		{
			if (serverInterface != null)
				serverInterface.DelCompetitorResult(competitorResult);
			else
			{
				if (databaseClass.Database == null)
				throw new ApplicationException(
					"Database not initialized when trying to remove " +
					"CompetitorResults");

				databaseClass.delCompetitorResult(competitorResult);
			}
		}
		/// <summary>
		/// Removes a station
		/// </summary>
		/// <param name="station">station</param>
		public void DelStation(Structs.Station station)
		{
			if (serverInterface != null)
				serverInterface.DelStation(station);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to remove " +
						"CompetitorResults");

				databaseClass.delStation(station);
			}
		}
		/// <summary>
		/// Removes a station
		/// </summary>
		/// <param name="station">station</param>
		public void DelStation(Structs.Station station, bool forcibleDelete)
		{
			if (serverInterface != null)
				serverInterface.DelStation(station, forcibleDelete);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to remove " +
						"CompetitorResults");

				databaseClass.delStation(station, forcibleDelete);
			}
		}
		/// <summary>
		/// Removes a team
		/// </summary>
		/// <param name="team">team</param>
		public void DelTeam(Structs.Team team)
		{
			if (serverInterface != null)
				serverInterface.DelTeam(team);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException(
						"Database not initialized when trying to remove " +
						"team");

				databaseClass.delTeam(team);
			}
		}
		#endregion

		#region Handlers for updating GUI and database file
		/// <summary>
		/// Event when clubs are updated.
		/// </summary>
		public event MethodInvoker UpdatedClub;
		internal void updatedClub()
		{
			if (this.serverInterface == null)
				databaseClass.UpdateDatabaseFile();

			try
			{
				Thread updatedClubThread =
					new Thread(
					new ThreadStart(this.UpdatedClub));
				updatedClubThread.Name = "Interface:updatedClubThread";
				updatedClubThread.Start();
				//this.UpdatedClub();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedClub:" + 
					exc.ToString());
			}
		}

		/// <summary>
		/// Event when shooters are updated
		/// </summary>
		public event MethodInvoker UpdatedShooter;
		internal void updatedShooter()
		{
			updatedShooter(new Structs.Shooter());
		}
		internal void updatedShooter(Structs.Shooter shooter)
		{
			if (this.serverInterface == null)
				databaseClass.UpdateDatabaseFile();

			try
			{
				resultClass.UpdatedShooter(shooter);
				Thread UpdatedShooterThread =
					new Thread(
					new ThreadStart(this.UpdatedShooter));
				UpdatedShooterThread.Name = "Interface:UpdatedShooterThread";
				UpdatedShooterThread.Start();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedShooter:" + 
					exc.ToString());
			}
		}

		/// <summary>
		/// Event when competitors are updated
		/// </summary>
		public event MethodInvoker UpdatedCompetitor;
		internal void updatedCompetitor()
		{
			updatedCompetitor(new Structs.Competitor());
		}
		internal void updatedCompetitor(Structs.Competitor competitor)
		{
			if (this.serverInterface == null)
				databaseClass.UpdateDatabaseFile();

			try
			{
				resultClass.UpdatedCompetitor(competitor);
				Thread UpdatedCompetitorThread =
					new Thread(
					new ThreadStart(this.UpdatedCompetitor));
				UpdatedCompetitorThread.Name = "Interface:UpdatedCompetitorThread";
				UpdatedCompetitorThread.Start();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetitor:" + 
					exc.ToString());
			}
		}

		/// <summary>
		/// Event when weapons are updated
		/// </summary>
		public event MethodInvoker UpdatedWeapon;
		internal void updatedWeapon()
		{
			if (this.serverInterface == null)
				databaseClass.UpdateDatabaseFile();

			try
			{
				Thread UpdatedWeaponThread =
					new Thread(
					new ThreadStart(this.UpdatedWeapon));
				UpdatedWeaponThread.Name = "Interface:UpdatedWeaponThread";
				UpdatedWeaponThread.Start();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedWeapon:" + 
					exc.ToString());
			}
		}

		/// <summary>
		/// Event when Competition are updated
		/// </summary>
		public event MethodInvoker UpdatedCompetition;
		internal void updatedCompetition()
		{
			if (this.serverInterface == null)
				databaseClass.UpdateDatabaseFile();

			try
			{
				Thread UpdatedCompetitionThread =
					new Thread(
					new ThreadStart(this.UpdatedCompetition));
				UpdatedCompetitionThread.Name = "Interface:UpdatedCompetitionThread";
				UpdatedCompetitionThread.Start();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetition:" + 
					exc.ToString());
			}
		}

		/// <summary>
		/// Event when patrols are updated
		/// </summary>
		public event MethodInvoker UpdatedPatrol;
		internal void updatedPatrol()
		{
			if (this.serverInterface == null)
				databaseClass.UpdateDatabaseFile();

			try
			{
				Thread UpdatedPatrolThread =
					new Thread(
					new ThreadStart(this.UpdatedPatrol));
				UpdatedPatrolThread.Name = "Interface:UpdatedPatrolThread";
				UpdatedPatrolThread.Start();
				//this.UpdatedPatrol();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedPatrol:" + 
					exc.ToString());
			}
		}

		/// <summary>
		/// Event when CompetitorResults are updated
		/// </summary>
		public event MethodInvoker UpdatedCompetitorResult;
		internal void updatedCompetitorResult()
		{
			updatedCompetitorResult(new Structs.CompetitorResult());
		}
		internal void updatedCompetitorResult(Structs.CompetitorResult compResult)
		{
			if (this.serverInterface == null)
				databaseClass.UpdateDatabaseFile();

			try
			{
				resultClass.UpdatedCompetitorResult(compResult);
				Thread UpdatedCompetitorResultThread =
					new Thread(
					new ThreadStart(this.UpdatedCompetitorResult));
				UpdatedCompetitorResultThread.Name = "Interface:UpdatedCompetitorResultThread";
				UpdatedCompetitorResultThread.Start();
				//this.UpdatedCompetitorResult();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedCompetitorResult:" + 
					exc.ToString());
			}
		}

		/// <summary>
		/// Event when stations are updated
		/// </summary>
		public event MethodInvoker UpdatedStation;
		internal void updatedStation()
		{
			if (this.serverInterface == null)
				databaseClass.UpdateDatabaseFile();

			try
			{
				Thread UpdatedStationThread =
					new Thread(
					new ThreadStart(this.UpdatedStation));
				UpdatedStationThread.Name = "Interface:UpdatedStationThread";
				UpdatedStationThread.Start();
				//this.UpdatedStation();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedStation:" + 
					exc.ToString());
			}
		}

		/// <summary>
		/// Event when stations are updated
		/// </summary>
		public event MethodInvoker UpdatedTeam;
		internal void updatedTeam()
		{
			if (this.serverInterface == null)
				databaseClass.UpdateDatabaseFile();

			try
			{
				Thread UpdatedTeamThread =
					new Thread(
					new ThreadStart(this.UpdatedTeam));
				UpdatedTeamThread.Name = "Interface:UpdatedTeamThread";
				UpdatedTeamThread.Start();
				//this.UpdatedTeam();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface.updatedTeam:" + 
					exc.ToString());
			}
		}

		/// <summary>
		/// Event used to inform gui of how much file import has been done
		/// </summary>
		public event UpdatedFileImportCountHandler UpdatedFileImportCount;
		internal void updatedFileImportCount(int count, int totalCount)
		{
			try
			{
				this.UpdatedFileImportCount(count, totalCount);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface." + 
					"updatedFileImportCount:" + exc.ToString());
			}
		}
		/// <summary>
		/// Event used to inform gui how mush automatic placement has been done
		/// </summary>
		public event UpdatedPatrolAddAutomaticCompetitorsHandler UpdatedPatrolAddAutomaticCompetitors;
		internal void updatedPatrolAddAutomaticCompetitors(int current, int max)
		{
			try
			{
				this.UpdatedPatrolAddAutomaticCompetitors(current, max);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(
					"Exception occured in Common.Interface." + 
					"updatedPatrolAddAutomaticCompetitors:" + exc.ToString());
			}
		}


		#endregion
		#endregion


		#region Cache methods
		/// <summary>
		/// Gets a shooter from cache
		/// </summary>
		/// <param name="CardNr">CardNr</param>
		/// <returns></returns>
		public Structs.Shooter GetShooterFromCache(string CardNr)
		{
			if (serverInterface != null)
				return serverInterface.GetShooterFromCache(CardNr);
			else
				return cacheClass.GetShooter(CardNr);
		}
		/// <summary>
		/// Get the current number of shooters in the local cache
		/// </summary>
		/// <returns></returns>
		public int GetShooterCountInLocalCache()
		{
			if (serverInterface != null)
				return serverInterface.GetShooterCountInLocalCache();
			else
				return cacheClass.GetShooterCountInLocalFile();
		}
		/// <summary>
		/// Get the current number of clubs in the local cache
		/// </summary>
		/// <returns></returns>
		public int GetClubCountInLocalCache()
		{
			if (serverInterface != null)
				return serverInterface.GetClubCountInLocalCache();
			else
				return cacheClass.GetClubCountInLocalFile();
		}
		/// <summary>
		/// Gets the content of the local xml cache file
		/// </summary>
		/// <returns></returns>
		public string GetCacheFileContent()
		{
			if (serverInterface != null)
				return serverInterface.GetCacheFileContent();
			else
				return cacheClass.GetCacheFileContent();
		}
		#endregion

		
		#region Converters
		/// <summary>
		/// Converts WeaponsClass to PatrolClass
		/// </summary>
		/// <param name="weapon">wclass</param>
		/// <returns></returns>
		public Structs.PatrolClass ConvertWeaponsClassToPatrolClass(Structs.WeaponClass weapon)
		{
			return CConvert.ConvertWeaponsClassToPatrolClass(weapon, PatrolConnectionType);
		}
		/// <summary>
		/// Converts WeaponsClass to PatrolClass
		/// </summary>
		/// <param name="weapon">wclass</param>
		/// <returns></returns>
		public Structs.PatrolClass ConvertWeaponsClassToPatrolClass(Structs.WeaponClass weapon,
			Structs.PatrolConnectionTypeEnum newPatrolConnectionType)
		{
			return CConvert.ConvertWeaponsClassToPatrolClass(weapon, newPatrolConnectionType);
		}
		
		/// <summary>
		/// Converts WeaponsClass to ResultClass
		/// </summary>
		/// <param name="weapon">Weapon</param>
		/// <returns></returns>
		public Structs.ResultWeaponsClass ConvertWeaponsClassToResultClass(
			Structs.WeaponClass weapon)
		{
			return CConvert.ConvertWeaponsClassToResultClass(weapon, CompetitionCurrent.Type);
		}
		#endregion


		#region Properties
		internal string ConnectionString
		{
			get
			{
				return connectionString;
			}
			set
			{
				connectionString = value;
				databaseClass.initConnection();
			}
		}

		/*public string DebugFileName
		{
			get
			{
				return debugFileName;
			}
			set
			{
				debugFileName = value;
			}
		}

		public int DebugLogLevel
		{
			get
			{
				return debugLevel;
			}
			set
			{
				debugLevel = value;
			}
		}*/
		#endregion


		#region Patrol Methods
		/// <summary>
		/// Adds an empty patrol
		/// </summary>
		public void PatrolAddEmpty()
		{
			PatrolAddEmpty(true);
		}
		internal void PatrolAddEmpty(bool UpdateGui)
		{
			if (serverInterface != null)
				serverInterface.PatrolAddEmpty();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to add patrol");

				patrolClass.PatrolAddEmpty(UpdateGui);
			}
		}
		/// <summary>
		///  Gets the next lane in a patrol
		/// </summary>
		/// <param name="patrolId"></param>
		/// <returns></returns>
		public int PatrolGetNextLane(int patrolId)
		{
			if (serverInterface != null)
				return serverInterface.PatrolGetNextLane(patrolId);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to add patrol");

				return patrolClass.GetNextLane(patrolId);
			}
		}
		/// <summary>
		///  Gets the next lane in a patrol after startlane
		/// </summary>
		/// <param name="patrolId"></param>
		/// <param name="startlane"></param>
		/// <returns></returns>
		public int PatrolGetNextLane(int patrolId, int startlane)
		{
			if (serverInterface != null)
				return serverInterface.PatrolGetNextLane(patrolId, startlane);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to add patrol");

				return patrolClass.GetNextLane(patrolId, startlane);
			}
		}
		/// <summary>
		///  Gets the previous empty lane in a patrol after startlane
		/// </summary>
		/// <param name="patrolId"></param>
		/// <param name="startlane"></param>
		/// <returns></returns>
		public int PatrolGetNextLaneUp(int patrolId, int startlane)
		{
			if (serverInterface != null)
				return serverInterface.PatrolGetNextLaneUp(patrolId, startlane);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to add patrol");

				return patrolClass.GetNextLaneUp(patrolId, startlane);
			}
		}
		/// <summary>
		/// Adds competitors to patrols automaticly
		/// </summary>
		/// <param name="CleanPatrols"></param>
		public void PatrolAddAutomaticCompetitors(bool CleanPatrols)
		{
			if (serverInterface != null)
				serverInterface.PatrolAddAutomaticCompetitors(CleanPatrols);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to add patrol");

				patrolClass.PatrolAddAutomaticCompetitors(CleanPatrols, true);

				updatedPatrol();
				updatedCompetitor(new Structs.Competitor());
			}
		}

		/// <summary>
		/// Removes the last patrol
		/// </summary>
		public void PatrolRemoveLast()
		{
			PatrolRemoveLast(true);
		}
		internal void PatrolRemoveLast(bool updateGui)
		{
			if (serverInterface != null)
				serverInterface.PatrolRemoveLast();
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to add patrol");
				patrolClass.PatrolRemoveLast();
				if (updateGui)
					updatedPatrol();
			}
		}
		public bool CheckChangePatrolConnectionTypeIsPossible(
			Structs.Patrol patrol,
			Structs.PatrolConnectionTypeEnum newPatrolConnectionType)
		{
			if (serverInterface != null)
				return serverInterface.CheckChangePatrolConnectionTypeIsPossible(
					patrol, 
					newPatrolConnectionType);
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to check patrol");
				return patrolClass.CheckChangePatrolConnectionTypeIsPossible(
					patrol,
					newPatrolConnectionType);
			}
		}

		#endregion

		#region Result Methods
		/// <summary>
		/// Calculates the results for a wclass and shooters class
		/// </summary>
		/// <param name="wclass">wclass</param>
		/// <param name="uclass">uclass</param>
		/// <param name="NorwegianCount">NorwegianCount</param>
		/// <param name="FinalResults"></param>
		/// <returns></returns>
		public ResultsReturn ResultsGetCompetitor(Structs.Competitor competitor)
		{
			if (serverInterface != null)
				try
				{
					return serverInterface.ResultsGetCompetitor(competitor);
				}
				catch (System.InvalidOperationException)
				{
					return this.ResultsGetCompetitor(competitor);
				}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to view results");

				return resultClass.ResultsGetCompetitor(competitor);
			}
		}
		/// <summary>
		/// Calculates the results for a wclass and shooters class
		/// </summary>
		/// <param name="wclass">wclass</param>
		/// <param name="uclass">uclass</param>
		/// <param name="NorwegianCount">NorwegianCount</param>
		/// <param name="FinalResults"></param>
		/// <returns></returns>
		public ResultsReturn[] ResultsGet(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass,
			Structs.Competition competition,
			bool FinalResults)
		{
			if (serverInterface != null)
				try
				{
					return serverInterface.ResultsGet(wclass, uclass, competition, FinalResults);
				}
				catch (System.InvalidOperationException)
				{
					return this.ResultsGet(wclass, uclass, competition, FinalResults);
				}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to view results");

				return resultClass.GetResults(wclass, uclass, competition, FinalResults);
			}
		}
		/// <summary>
		/// Calculates the results for a wclass and shooters class for team
		/// </summary>
		/// <param name="wclass">wclass</param>
		/// <param name="uclass">uclass</param>
		/// <param name="NorwegianCount">NorwegianCount</param>
		/// <param name="FinalResults"></param>
		/// <returns></returns>
		public ResultsReturnTeam[] ResultsGetTeams(Structs.ResultWeaponsClass wclass,
			Structs.Competition competition)
		{
			if (serverInterface != null)
				try
				{
					return serverInterface.ResultsGetTeams(wclass, competition);
				}
				catch(System.InvalidOperationException)
				{
					return this.ResultsGetTeams(wclass, competition);
				}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to view results");

				return this.resultTeamClass.GetTeamResults(wclass, competition);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Structs.ResultWeaponsClass[] ResultsGetWClasses()
		{
			if (serverInterface != null)
			{
				return serverInterface.ResultsGetWClasses();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to view results");

				return resultClass.ResultsGetWClasses();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="wclass"></param>
		/// <returns></returns>
		public Structs.ShootersClass[] ResultsGetUClasses(Structs.ResultWeaponsClass wclass)
		{
			if (serverInterface != null)
			{
				return serverInterface.ResultsGetUClasses(wclass);
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to view results");

				return resultClass.ResultsGetUClasses(wclass);
			}
		}

		public Structs.ResultWeaponsClass[] GetResultWeaponClassesWithResults()
		{
			if (serverInterface != null)
			{
				return serverInterface.GetResultWeaponClassesWithResults();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to view results");

				return resultClass.GetResultWeaponClassesWithResults();
			}
		}
		#endregion

		#region FollowUp Methods
		/// <summary>
		/// Used to follow up
		/// </summary>
		/// <returns>Array of follow up data to display/print</returns>
		public Structs.FollowUpReturn[] FollowUpByClub()
		{
			if (serverInterface != null)
			{
				return serverInterface.FollowUpByClub();
			}
			else
			{
				if (databaseClass.Database == null)
					throw new ApplicationException("Database not initialized when trying to view results");

				CFollowUp follow = new CFollowUp(this);
				return follow.FollowUp(CFollowUp.SortingEnum.ByClub);
			}
		}
		#endregion

		#region Internet Methods
		#region Html
		/// <summary>
		/// Creates an html-page for patrols
		/// </summary>
		/// <returns></returns>
		public string InternetHtmlExportPatrols()
		{
			if (serverInterface != null)
				return serverInterface.InternetHtmlExportPatrols();
			else
				return internetHtmlExportClass.ExportPatrols();
		}
		/// <summary>
		/// Exports the patrols sorted by club
		/// </summary>
		/// <returns>html</returns>
		public string InternetHtmlExportPatrolsByClub()
		{
			if (serverInterface != null)
				return serverInterface.InternetHtmlExportPatrolsByClub();
			else
				return internetHtmlExportClass.ExportPatrolsByClub();
		}
		/// <summary>
		/// Creates an html-page for results
		/// </summary>
		/// <param name="finalResults"></param>
		/// <returns></returns>
		public string InternetHtmlExportResults(bool finalResults)
		{
			if (serverInterface != null)
				return serverInterface.InternetHtmlExportResults(finalResults);
			else
				return internetHtmlExportClass.ExportResults(finalResults);
		}
		/// <summary>
		/// Creates an html-page for results for a wclass/uclass 
		/// </summary>
		/// <param name="wclass"></param>
		/// <param name="uclass"></param>
		/// <param name="finalResults"></param>
		/// <returns></returns>
		public string InternetHtmlExportResults(Structs.ResultWeaponsClass wclass, 
			Structs.ShootersClass uclass, bool finalResults)
		{
			if (serverInterface != null)
				return serverInterface.InternetHtmlExportResults(wclass, uclass, finalResults);
			else
				return internetHtmlExportClass.ExportResults(wclass, uclass, finalResults);
		}
		#endregion
		#region Pdf
		/// <summary>
		/// Creates an html-page for patrols
		/// </summary>
		/// <returns></returns>
		public byte[] InternetPdfExportPatrols()
		{
			if (serverInterface != null)
				return serverInterface.InternetPdfExportPatrols();
			else
				return internetPdfExportClass.ExportPatrols();
		}
		/// <summary>
		/// Exports the patrols sorted by club
		/// </summary>
		/// <returns>html</returns>
		public byte[] InternetPdfExportPatrolsByClub()
		{
			if (serverInterface != null)
				return serverInterface.InternetPdfExportPatrolsByClub();
			else
				return internetPdfExportClass.ExportPatrolsByClub();
		}
		/// <summary>
		/// Creates an html-page for results
		/// </summary>
		/// <param name="finalResults"></param>
		/// <returns></returns>
		public byte[] InternetPdfExportResults(bool finalResults, bool allOnOnePage)
		{
			if (serverInterface != null)
				return serverInterface.InternetPdfExportResults(finalResults, allOnOnePage);
			else
				return internetPdfExportClass.ExportResults(finalResults, allOnOnePage);
		}
		/// <summary>
		/// Creates an html-page for results for a wclass/uclass 
		/// </summary>
		/// <param name="wclass"></param>
		/// <param name="uclass"></param>
		/// <param name="finalResults"></param>
		/// <returns></returns>
		public byte[] InternetPdfExportResults(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass, bool finalResults)
		{
			if (serverInterface != null)
				return serverInterface.InternetPdfExportResults(wclass, uclass, finalResults);
			else
				return internetPdfExportClass.ExportResults(wclass, uclass, finalResults);
		}
		#endregion
		#region Excel
		/// <summary>
		/// Creates an html-page for patrols
		/// </summary>
		/// <returns></returns>
		public byte[] InternetExcelExportPatrols()
		{
			throw new NotImplementedException();
			/*if (serverInterface != null)
				return serverInterface.InternetExcelExportPatrols();
			else
				return internetExcelExportClass.ExportPatrols();*/
		}
		/// <summary>
		/// Exports the patrols sorted by club
		/// </summary>
		/// <returns>html</returns>
		public byte[] InternetExcelExportPatrolsByClub()
		{
			throw new NotImplementedException();
			/*if (serverInterface != null)
				return serverInterface.InternetExcelExportPatrolsByClub();
			else
				return internetExcelExportClass.ExportPatrolsByClub();*/
		}
		/// <summary>
		/// Creates an html-page for results
		/// </summary>
		/// <param name="finalResults"></param>
		/// <returns></returns>
		public byte[] InternetExcelExportResults(bool finalResults)
		{
			if (serverInterface != null)
				return serverInterface.InternetExcelExportResults(finalResults);
			else
				return internetExcelExportClass.ExportResults(finalResults);
		}
		/// <summary>
		/// Creates an html-page for results for a wclass/uclass 
		/// </summary>
		/// <param name="wclass"></param>
		/// <param name="uclass"></param>
		/// <param name="finalResults"></param>
		/// <returns></returns>
		public byte[] InternetExcelExportResults(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass, bool finalResults)
		{
			if (serverInterface != null)
				return serverInterface.InternetExcelExportResults(wclass, uclass, finalResults);
			else
				return internetExcelExportClass.ExportResults(wclass, uclass, finalResults);
		}
		#endregion
		#region Xml
		public byte[] InternetXmlExport()
		{
			if (serverInterface != null)
				return serverInterface.InternetXmlExport();
			else
				return internetXmlExportClass.ExportResults();
		}
		#endregion
		#region Text
		public byte[] InternetTextExport()
		{
			if (serverInterface != null)
				return serverInterface.InternetTextExport();
			else
				return internetTextExportClass.ExportResults();
		}
		public byte[] InternetWeaponsExport()
		{
			if (serverInterface != null)
				return serverInterface.InternetWeaponsExport();
			else
				return internetTextExportClass.ExportWeapons();
		}
		public byte[] InternetClubsExport()
		{
			if (serverInterface != null)
				return serverInterface.InternetClubsExport();
			else
				return internetTextExportClass.ExportClubs();
		}
		#endregion
		#endregion

		#region File import Methods
		/// <summary>
		/// ImportFileType
		/// </summary>
		public enum ImportFileType
		{
			/// <summary>
			/// Semicolon separated
			/// </summary>
			SKV,
			/// <summary>
			/// Length dependant
			/// </summary>
			LengthDependent
		}
		/// <summary>
		/// Import file columns
		/// </summary>
		public enum ImportFileColumns
		{
			/// <summary>
			/// ShooterId
			/// </summary>
			ShooterId,
			/// <summary>
			/// ClubId
			/// </summary>
			ClubId,
			/// <summary>
			/// Surname
			/// </summary>
			Surname,
			/// <summary>
			/// Givenname
			/// </summary>
			Givenname,
			/// <summary>
			/// ShooterClass
			/// </summary>
			ShooterClass,
			/// <summary>
			/// Patrols
			/// </summary>
			Patrol,
			/// <summary>
			/// Lane
			/// </summary>
			Lane,
			/// <summary>
			/// WeaponId
			/// </summary>
			WeaponId,
			/// <summary>
			/// Epost
			/// </summary>
			Email
		}
		/// <summary>
		/// Imports the file and loads it into a dataset
		/// </summary>
		/// <param name="FilePath">filename</param>
		/// <param name="ColumnOrder">order of columns</param>
		/// <returns></returns>
		public string[] ImportFileLoadFile(string FilePath, 
			System.Collections.SortedList ColumnOrder)
		{
			if (serverInterface != null)
				throw new ApplicationException("Not supported while connected to server");
			else
				return fileImportClass.LoadFile(
					FilePath, Interface.ImportFileType.SKV, ColumnOrder);
		}
		/// <summary>
		/// Tells to import the dataset into the database
		/// </summary>
		/// <param name="check">If it is supposed to check if it works</param>
		/// <returns></returns>
		public System.Data.DataTable ImportFileViewDataset(bool check)
		{
			if (serverInterface != null)
				throw new ApplicationException("Not supported while connected to server");
			else
			{
				if (check)
					return fileImportClass.ViewDatatableCheck;
				else
					return fileImportClass.ViewDatatable;
			}
		}
		/// <summary>
		/// ?
		/// </summary>
		public void ImportFileValidateDataset()
		{
			if (serverInterface != null)
				throw new ApplicationException("Not supported while connected to server");
			else
			{
				fileImportClass.ValidateDataset();
			}
		}
		/// <summary>
		/// ?
		/// </summary>
		public void ImportFileImportDataset()
		{
			if (serverInterface != null)
				throw new ApplicationException("Not supported while connected to server");
			else
			{
				fileImportClass.ImportDataset();
			}
		}
		/// <summary>
		/// Flag if shooters should be added
		/// </summary>
		public bool ImportFileFlagsAddShooters
		{
			set
			{
				if (serverInterface != null)
					throw new ApplicationException("Not supported while connected to server");
				else
					fileImportClass.ignoreShooterErrors = value;
			}
		}
		/// <summary>
		/// Flag if patrols should be automaticly added
		/// </summary>
		public bool ImportFileFlagsAddPatrols
		{
			set
			{
				if (serverInterface != null)
					throw new ApplicationException("Not supported while connected to server");
				else
					fileImportClass.ignorePatrolErrors = value;
			}
		}
		/// <summary>
		/// Flag if lanes should be automaticly added
		/// </summary>
		public bool ImportFileFlagsAddLanes
		{
			set
			{
				if (serverInterface != null)
					throw new ApplicationException("Not supported while connected to server");
				else
					fileImportClass.ignoreLaneErrors = value;
			}
		}
		#endregion

		#region Client Methods
		Allberg.Shooter.WinShooterServerRemoting.IWinshooterServer serverInterface = null;
		/// <summary>
		/// Connects to a WinShooter Server
		/// </summary>
		/// <param name="servername"></param>
		public void ServerConnect(string servername)
		{
			int serverPort = 90;
			if (servername.Contains(":"))
			{
				string port = servername.Substring(servername.IndexOf(":")+1);
				serverPort = int.Parse(port);
				servername = servername.Substring(0, servername.IndexOf(":"));
			}
			HttpChannel clientchan = null;
			foreach(IChannel channel in ChannelServices.RegisteredChannels)
			{
				if (channel.ChannelName == "WinShooterClientChan")
					clientchan = (HttpChannel)channel;
			}
			if (clientchan == null)
			{
				ListDictionary channelProperties = new ListDictionary();

				channelProperties.Add("port", 0);
				channelProperties.Add("name", "WinShooterClientChan");

				clientchan = new HttpChannel(
					channelProperties, 
					new SoapClientFormatterSinkProvider(),
					new SoapServerFormatterSinkProvider());

				ChannelServices.RegisterChannel(clientchan, false);
			}
			serverInterface = 
				(Allberg.Shooter.WinShooterServerRemoting.IWinshooterServer)
				Activator.GetObject(
				typeof(Allberg.Shooter.WinShooterServerRemoting.IWinshooterServer), 
				"http://" + servername + ":" + serverPort.ToString() + "/WinShooterServer");

			// Check versions
			System.Version version = serverInterface.Sync(
				System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
			if (version < new Version(minimumServerVersionString))
			{
				throw new ApplicationException("Servern har version " + 
					version.ToString() + " vilket är för lågt för att denna " + 
					"klient ska kunna köra mot den. Uppdatera servern med " + 
					"den senaste programvaran.");
			}

			// Ok, continue
			RemotingCallback callback = new RemotingCallback();

			callback.UpdatedClubEvent               += new MethodInvoker(this.updatedClub);
			callback.UpdatedCompetitionEvent        += new MethodInvoker(this.updatedCompetition);
			callback.UpdatedCompetitorEvent         += new MethodInvoker(this.updatedCompetitor);
			callback.UpdatedCompetitorResultEvent   += new MethodInvoker(this.updatedCompetitorResult);
			callback.UpdatedPatrolEvent             += new MethodInvoker(this.updatedPatrol);
			callback.UpdatedShooterEvent            += new MethodInvoker(this.updatedShooter);
			callback.UpdatedStationEvent            += new MethodInvoker(this.updatedStation);
			callback.UpdatedWeaponEvent             += new MethodInvoker(this.updatedWeapon);
			callback.UpdatedTeamEvent               += new MethodInvoker(this.updatedTeam);
			callback.SyncronizeEvent                += new MethodInvoker(this.SyncClient);

			MethodInvokerUpdatedClub                = new MethodInvoker(callback.UpdatedClub);
			serverInterface.UpdatedClub             += MethodInvokerUpdatedClub;

			MethodInvokerUpdatedCompetition         = new MethodInvoker(callback.UpdatedCompetition);
			serverInterface.UpdatedCompetition      += MethodInvokerUpdatedCompetition;

			MethodInvokerUpdatedCompetitor          = new MethodInvoker(callback.UpdatedCompetitor);
			serverInterface.UpdatedCompetitor       += MethodInvokerUpdatedCompetitor;

			MethodInvokerUpdatedCompetitorResult    = new MethodInvoker(callback.UpdatedCompetitorResult);
			serverInterface.UpdatedCompetitorResult += MethodInvokerUpdatedCompetitorResult;

			MethodInvokerUpdatedPatrol              = new MethodInvoker(callback.UpdatedPatrol);
			serverInterface.UpdatedPatrol           += MethodInvokerUpdatedPatrol;

			MethodInvokerUpdatedShooter             = new MethodInvoker(callback.UpdatedShooter);
			serverInterface.UpdatedShooter          += MethodInvokerUpdatedShooter;

			MethodInvokerUpdatedStation             = new MethodInvoker(callback.UpdatedStation);
			serverInterface.UpdatedStation          += MethodInvokerUpdatedStation;

			MethodInvokerUpdatedWeapon              = new MethodInvoker(callback.UpdatedWeapon);
			serverInterface.UpdatedWeapon           += MethodInvokerUpdatedWeapon;

			MethodInvokerUpdatedTeam                = new MethodInvoker(callback.UpdatedTeam);
			serverInterface.UpdatedTeam             += MethodInvokerUpdatedTeam;

			serverInterface.SyncronizeEvent         += new MethodInvoker(callback.Syncronize);

			serverInterface.Sync();
		}
		MethodInvoker MethodInvokerUpdatedClub;
		MethodInvoker MethodInvokerUpdatedCompetition;
		MethodInvoker MethodInvokerUpdatedCompetitor;
		MethodInvoker MethodInvokerUpdatedCompetitorResult;
		MethodInvoker MethodInvokerUpdatedPatrol;
		MethodInvoker MethodInvokerUpdatedShooter;
		MethodInvoker MethodInvokerUpdatedStation;
		MethodInvoker MethodInvokerUpdatedWeapon;
		MethodInvoker MethodInvokerUpdatedTeam;

		public event MethodInvoker SyncronizeEvent;

		/// <summary>
		/// Syncs a server connection
		/// </summary>
		public void Sync()
		{
			if (serverInterface != null)
				serverInterface.Sync();
		}
		public void SyncClient()
		{
			Trace.WriteLine("Sync Callback...");
		}
		/// <summary>
		/// Syncs a version. This is not implemented in common, only on serverside
		/// </summary>
		/// <param name="version"></param>
		/// <returns></returns>
		public System.Version Sync(System.Version version)
		{
			throw new ApplicationException("Not implemented in common");
		}

		/// <summary>
		/// Run a backup
		/// </summary>
		/// <param name="filename"></param>
		public void Backup(string filename)
		{
			if (serverInterface != null)
				throw new ApplicationException("Not supported while connected to server");
			else
				databaseClass.Backup(filename);
		}
		#endregion

	}
}
