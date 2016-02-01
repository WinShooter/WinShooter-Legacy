// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterfaceDefinition.cs" company="John Allberg">
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
// <summary>
//   Delegate for FileImportCount
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServerRemoting
{
    using System.Windows.Forms;

    /// <summary>
    /// Delegate for FileImportCount
    /// </summary>
    public delegate void UpdatedFileImportCountHandler(int count, int totalCount);

    /// <summary>
    /// Delegate for UpdatedPatrolAddAutomatic
    /// </summary>
    public delegate void UpdatedPatrolAddAutomaticCompetitorsHandler(int current, int total);

    /// <summary>
    /// Summary description for TcpInterfaceDefinition.
    /// </summary>
    public interface IWinshooterServer
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        bool EnableInternetConnections
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        Structs.PatrolConnectionTypeEnum PatrolConnectionType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        ISettings Settings
        {
            get;
        }
        #endregion
        
        #region Database Methods
        #region Database Init

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathAndFileName"></param>
        void CreateAccessDatabase(string pathAndFileName);
        /// <summary>
        /// 
        /// </summary>
        void CreateDefaultDatabaseContent();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathAndFilename"></param>
        void OpenAccessDatabase(string pathAndFilename);
        /// <summary>
        /// 
        /// </summary>
        void OpenDatabase();

        #endregion
        #region Get stuff
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clubId"></param>
        /// <returns></returns>
        Structs.Club GetClub(string clubId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.Club[] GetClubs();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sorting"></param>
        /// <returns></returns>
        Structs.Shooter[] GetShooters(string sorting);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clubToFetch"></param>
        /// <returns></returns>
        Structs.Shooter[] GetShooters(Structs.Club clubToFetch);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clubToFetch"></param>
        /// <param name="wclass">wclass</param>
        /// <returns></returns>
        Structs.Shooter[] GetShooters(Structs.Club clubToFetch, Structs.ResultWeaponsClass wclass);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shooterId"></param>
        /// <returns></returns>
        Structs.Shooter GetShooter(int shooterId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetShooterHighestId();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNr"></param>
        /// <returns></returns>
        Structs.Shooter GetShooter(string cardNr);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.Competitor[] GetCompetitors();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shooterId"></param>
        /// <param name="sorting"></param>
        /// <returns></returns>
        Structs.Competitor[] GetCompetitors(int shooterId, string sorting);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shooterId"></param>
        /// <returns></returns>
        Structs.Competitor[] GetCompetitors(int shooterId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrolToFetch"></param>
        /// <returns></returns>
        Structs.Competitor[] GetCompetitors(Structs.Patrol patrolToFetch);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrolToFetch"></param>
        /// <param name="sorting"></param>
        /// <returns></returns>
        Structs.Competitor[] GetCompetitors(Structs.Patrol patrolToFetch, string sorting);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.Competitor[] GetCompetitorsWithNoPatrol();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisClass"></param>
        /// <returns></returns>
        Structs.Competitor[] GetCompetitorsWithNoPatrol(Structs.PatrolClass thisClass);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clubToFetch"></param>
        /// <param name="wclass"></param>
        /// <param name="sorting"></param>
        /// <returns></returns>
        Structs.Competitor[] GetCompetitors(Structs.Club clubToFetch, Structs.ResultWeaponsClass wclass, string sorting);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitorId"></param>
        /// <returns></returns>
        Structs.Competitor GetCompetitor(int competitorId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sorting"></param>
        /// <returns></returns>
        Structs.Weapon[] GetWeapons(string sorting);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.Weapon[] GetWeapons();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weaponsId"></param>
        /// <returns></returns>
        Structs.Weapon GetWeapon(string weaponsId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.Competition CompetitionCurrent { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.Competition[] GetCompetitions();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Structs.Patrol GetPatrol(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.Patrol[] GetPatrols();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrolClass"></param>
        /// <param name="alsoIncludeUnknownClass"></param>
        /// <returns></returns>
        Structs.Patrol[] GetPatrols(Structs.PatrolClass patrolClass, 
            bool alsoIncludeUnknownClass);		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrolClass"></param>
        /// <param name="alsoIncludeUnknownClass"></param>
        /// <param name="onlyIncludePatrolsWithSpace"></param>
        /// <param name="patrolIdToAlwaysView"></param>
        /// <returns></returns>
        Structs.Patrol[] GetPatrols(Structs.PatrolClass patrolClass, 
            bool alsoIncludeUnknownClass,
            bool onlyIncludePatrolsWithSpace,
            int patrolIdToAlwaysView);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.CompetitorResult[] GetCompetitorResults();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitorsId"></param>
        /// <returns></returns>
        Structs.CompetitorResult[] GetCompetitorResults(int competitorsId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitorsId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        Structs.CompetitorResult GetCompetitorResult(int competitorsId, int stationId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.Station[] GetStations();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.Station[] GetStationsDistinguish();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationNr"></param>
        /// <param name="distinguish"></param>
        /// <returns></returns>
        Structs.Station GetStation(int stationNr, bool distinguish);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.Team[] GetTeams();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        Structs.Team GetTeam(int teamId);
                          
        #endregion

        #region Get stuff count
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetClubsCount();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetClubsCountWithShooters();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetShootersCount();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetCompetitorsCount();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrol"></param>
        /// <returns></returns>
        int GetCompetitorsCountPatrol(Structs.Patrol patrol);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrol"></param>
        /// <returns></returns>
        int GetCompetitorsWithResultCountPatrol(Structs.Patrol patrol);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrol"></param>
        /// <returns></returns>
        int GetCompetitorsArrivedCountPatrol(Structs.Patrol patrol);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetWeaponsCount();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetCompetitionsCount();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetPatrolsCount();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetCompetitorResultsCount();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wclass"></param>
        /// <param name="uclass"></param>
        /// <returns></returns>
        int GetCompetitorResultsCount(Structs.ResultWeaponsClass wclass,
            Structs.ShootersClass uclass);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wclass"></param>
        /// <param name="uclass"></param>
        /// <param name="clubId"></param>
        /// <returns></returns>
        int GetCompetitorResultsCount(Structs.ResultWeaponsClass wclass,
            Structs.ShootersClass uclass, string clubId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wclass"></param>
        /// <returns></returns>
        bool GetCompetitorResultsExist(Structs.ResultWeaponsClass wclass);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wclass"></param>
        /// <param name="uclass"></param>
        /// <returns></returns>
        bool GetCompetitorResultsExist(Structs.ResultWeaponsClass wclass,
            Structs.ShootersClass uclass);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetStationsCount();
        #endregion


        #region Update Stuff
        /// <summary>
        /// 
        /// </summary>
        /// <param name="club"></param>
        void UpdateClub(Structs.Club club);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shooter"></param>
        void UpdateShooter(Structs.Shooter shooter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitor"></param>
        void UpdateCompetitor(Structs.Competitor competitor);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weapon"></param>
        void UpdateWeapon(Structs.Weapon weapon);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competition"></param>
        void UpdateCompetition(Structs.Competition competition);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrol"></param>
        void UpdatePatrol(Structs.Patrol patrol);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitorResult"></param>
        /// <param name="updateInterface"></param>
        void UpdateCompetitorResult(Structs.CompetitorResult 
            competitorResult,
            bool updateInterface);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="station"></param>
        void UpdateStation(Structs.Station station);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        void UpdateTeam(Structs.Team team);

        #endregion

        #region Add stuff
        /// <summary>
        /// 
        /// </summary>
        /// <param name="club"></param>
        void NewClub(Structs.Club club);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shooter"></param>
        /// <returns></returns>
        int NewShooter(Structs.Shooter shooter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitor"></param>
        void NewCompetitor(Structs.Competitor competitor);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weapon"></param>
        void NewWeapon(Structs.Weapon weapon);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comp"></param>
        void NewCompetition(Structs.Competition comp);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrol"></param>
        /// <param name="notifyGui"></param>
        /// <returns></returns>
        int NewPatrol(Structs.Patrol patrol, bool notifyGui);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        void NewCompetitorResult(Structs.CompetitorResult res);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="station"></param>
        /// <param name="distinguish"></param>
        void NewStation(Structs.Station station, bool distinguish);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        void NewTeam(Structs.Team team);
        #endregion

        #region Remove stuff
        /// <summary>
        /// 
        /// </summary>
        /// <param name="club"></param>
        void DelClub(Structs.Club club);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shooter"></param>
        void DelShooter(Structs.Shooter shooter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitor"></param>
        void DelCompetitor(Structs.Competitor competitor);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weapon"></param>
        void DelWeapon(Structs.Weapon weapon);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competition"></param>
        void DelCompetition(Structs.Competition competition);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrol"></param>
        void DelPatrol(Structs.Patrol patrol);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitorResult"></param>
        void DelCompetitorResult(Structs.CompetitorResult 
            competitorResult);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="station"></param>
        void DelStation(Structs.Station station);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="station"></param>
        /// <param name="forciblyDelete"></param>
        void DelStation(Structs.Station station, bool forciblyDelete);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        void DelTeam(Structs.Team team);
        #endregion

        #region Handlers for updating GUI and database file
        /// <summary>
        /// 
        /// </summary>
        event MethodInvoker UpdatedClub;
        /// <summary>
        /// 
        /// </summary>
        event MethodInvoker UpdatedShooter;
        /// <summary>
        /// 
        /// </summary>
        event MethodInvoker UpdatedCompetitor;
        /// <summary>
        /// 
        /// </summary>
        event MethodInvoker UpdatedWeapon;
        /// <summary>
        /// 
        /// </summary>
        event MethodInvoker UpdatedCompetition;
        /// <summary>
        /// 
        /// </summary>
        event MethodInvoker UpdatedPatrol;
        /// <summary>
        /// 
        /// </summary>
        event MethodInvoker UpdatedCompetitorResult;
        /// <summary>
        /// 
        /// </summary>
        event MethodInvoker UpdatedStation;
        /// <summary>
        /// 
        /// </summary>
        event MethodInvoker UpdatedTeam;
        /// <summary>
        /// 
        /// </summary>
        event UpdatedFileImportCountHandler UpdatedFileImportCount;
        /// <summary>
        /// 
        /// </summary>
        event UpdatedPatrolAddAutomaticCompetitorsHandler UpdatedPatrolAddAutomaticCompetitors;
        #endregion
        #endregion

        #region Cache methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNr"></param>
        /// <returns></returns>
        Structs.Shooter GetShooterFromCache(string cardNr);
        /// <summary>
        /// Get the current number of shooters in the local cache
        /// </summary>
        /// <returns></returns>
        int GetShooterCountInLocalCache();
        /// <summary>
        /// Get the current number of clubs in the local cache
        /// </summary>
        /// <returns></returns>
        int GetClubCountInLocalCache();
        /// <summary>
        /// Gets the content of the local xml cache file
        /// </summary>
        /// <returns></returns>
        string GetCacheFileContent();
        #endregion

        #region Converters
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        Structs.PatrolClass ConvertWeaponsClassToPatrolClass(Structs.WeaponClass weapon);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        Structs.ResultWeaponsClass ConvertWeaponsClassToResultClass(
            Structs.WeaponClass weapon);
        #endregion
        
        #region Patrol Methods
        /// <summary>
        /// 
        /// </summary>
        void PatrolAddEmpty();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrolId"></param>
        /// <returns></returns>
        int PatrolGetNextLane(int patrolId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrolId"></param>
        /// <param name="lane"></param>
        /// <returns></returns>
        int PatrolGetNextLane(int patrolId, int lane);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patrolId"></param>
        /// <param name="lane"></param>
        /// <returns></returns>
        int PatrolGetNextLaneUp(int patrolId, int lane);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cleanPatrols"></param>
        void PatrolAddAutomaticCompetitors(bool cleanPatrols);
        /// <summary>
        /// 
        /// </summary>
        void PatrolRemoveLast();
        /// <summary>
        /// Checks if the patrol is able to be converted to the new PatrolConnectionType
        /// </summary>
        /// <param name="patrol"></param>
        /// <param name="newPatrolConnectionType"></param>
        /// <returns></returns>
        bool CheckChangePatrolConnectionTypeIsPossible(Structs.Patrol patrol, Structs.PatrolConnectionTypeEnum newPatrolConnectionType);
        #endregion

        #region Result Methods

        /// <summary>
        /// Return the results for a competitor
        /// </summary>
        /// <param name="competitor">The competitor asked for</param>
        /// <returns></returns>
        ResultsReturn ResultsGetCompetitor(Structs.Competitor competitor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wclass"></param>
        /// <param name="uclass"></param>
        /// <param name="competition"></param>
        /// <param name="finalResults"></param>
        /// <returns></returns>
        ResultsReturn[] ResultsGet(Structs.ResultWeaponsClass wclass, 
            Structs.ShootersClass uclass,
            Structs.Competition competition,
            bool finalResults);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wclass"></param>
        /// <param name="competition"></param>
        /// <returns></returns>
        ResultsReturnTeam[] ResultsGetTeams(Structs.ResultWeaponsClass wclass, 
            Structs.Competition competition);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.ResultWeaponsClass[] ResultsGetWClasses();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wclass"></param>
        /// <returns></returns>
        Structs.ShootersClass[] ResultsGetUClasses(Structs.ResultWeaponsClass wclass);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Structs.ResultWeaponsClass[] GetResultWeaponClassesWithResults();
        #endregion

        #region FollowUp Methods
        /// <summary>
        /// Used to follow up
        /// </summary>
        /// <returns>Array of follow up data to display/print</returns>
        Structs.FollowUpReturn[] FollowUpByClub();
        #endregion

        #region Internet Methods
        #region Html
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string InternetHtmlExportPatrols();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The Html</returns>
        string InternetHtmlExportPatrolsByClub();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finalResults">If the export is for the final results (including pricemoney)</param>
        /// <returns></returns>
        string InternetHtmlExportResults(bool finalResults);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wclass"></param>
        /// <param name="uclass"></param>
        /// <param name="finalResults">If the export is for the final results (including pricemoney)</param>
        /// <returns></returns>
        string InternetHtmlExportResults(Structs.ResultWeaponsClass wclass, 
            Structs.ShootersClass uclass, bool finalResults);
        #endregion
        #region Pdf
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] InternetPdfExportPatrols();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The Html</returns>
        byte[] InternetPdfExportPatrolsByClub();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finalResults">If the export is for the final results (including pricemoney)</param>
        /// <param name="allOnOnePage">If all exports should be on one page (if it will fit)</param>
        /// <returns></returns>
        byte[] InternetPdfExportResults(bool finalResults, bool allOnOnePage);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wclass"></param>
        /// <param name="uclass"></param>
        /// <param name="finalResults">If the export is for the final results (including pricemoney)</param>
        /// <returns></returns>
        byte[] InternetPdfExportResults(Structs.ResultWeaponsClass wclass,
            Structs.ShootersClass uclass, bool finalResults);
        #endregion
        #region Excel
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] InternetExcelExportPatrols();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>The Html</returns>
        byte[] InternetExcelExportPatrolsByClub();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="finalResults">If the export is for the final results (including pricemoney)</param>
        /// <returns></returns>
        byte[] InternetExcelExportResults(bool finalResults);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wclass"></param>
        /// <param name="uclass"></param>
        /// <param name="finalResults">If the export is for the final results (including pricemoney)</param>
        /// <returns></returns>
        byte[] InternetExcelExportResults(Structs.ResultWeaponsClass wclass,
            Structs.ShootersClass uclass, bool finalResults);
        #endregion
        #region Xml
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] InternetXmlExport();
        #endregion
        #region Text
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] InternetTextExport();
        byte[] InternetWeaponsExport();
        byte[] InternetClubsExport();
        #endregion
        #endregion

        #region ClientServer
        event MethodInvoker SyncronizeEvent;

        /// <summary>
        /// 
        /// </summary>
        void Sync();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        System.Version Sync(System.Version version);
        /// <summary>
        /// Run a backup
        /// </summary>
        /// <param name="filename"></param>
        void Backup(string filename);
        #endregion


    }
}
