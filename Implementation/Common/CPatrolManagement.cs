// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CPatrolManagement.cs" company="John Allberg">
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
//   Summary description for PatrolClass.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    using Allberg.Shooter.Common.DataSets;
    using Allberg.Shooter.Common.Exceptions;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for PatrolClass.
    /// </summary>
    [Serializable]
    internal class CPatrolManagement
    {
        internal CPatrolManagement(Interface callerInterface)
        {
            myInterface = callerInterface;
        }
        internal Interface myInterface;
        internal DatabaseDataset database = null;

        internal void PatrolAddEmpty()
        {
            PatrolAddEmpty(true);
        }
        internal void PatrolAddEmpty(bool notifyGui)
        {
            database = myInterface.databaseClass.Database;

            DateTime compStart = myInterface.GetCompetitions()[0].StartTime;

            int timeBetweenPatrols = database.Competition[0].PatrolTimeBetween;
            int lastStart = 0 - timeBetweenPatrols;
            int patrolId = 0;

            foreach(DatabaseDataset.PatrolsRow row in database.Patrols)
            {
                if (row.StartDateTime > lastStart)
                {
                    lastStart = row.StartDateTime;
                }
                if (row.PatrolId > patrolId)
                    patrolId = row.PatrolId;
            }

            Structs.Patrol patrol = new Structs.Patrol();
            patrol.PatrolId = patrolId + 1;
            patrol.StartDateTime = compStart.AddMinutes((int)(lastStart + timeBetweenPatrols));
            patrol.CompetitionId = database.Competition[0].CompetitionId;

            myInterface.NewPatrol(patrol, notifyGui);
        }

        internal int GetNextLane(int patrolId)
        {
            return GetNextLane(patrolId, 0);
        }

        internal int GetNextLane(int patrolId, int startlane)
        {
            if ( patrolId == -1 )
                return -1;

            int MaxCompetitorsInPatrol = myInterface.GetCompetitions()[0].PatrolSize;

            Hashtable hash = new Hashtable();

            Structs.Competitor[] competitors =
                myInterface.GetCompetitors(myInterface.GetPatrol(patrolId));

            foreach(Structs.Competitor competitor in competitors)
            {
                if (!hash.ContainsKey(competitor.Lane))
                    hash.Add(competitor.Lane, competitor);
            }

            for (int lane=startlane+1;lane<=MaxCompetitorsInPatrol;lane++)
            {
                if (!hash.ContainsKey(lane))
                    return lane;
            }		
            throw new PatrolAlreadyFullException(patrolId);
        }

        internal int GetNextLaneUp(int patrolId, int startlane)
        {
            if ( patrolId == -1 )
                return -1;

            int MaxCompetitorsInPatrol = myInterface.GetCompetitions()[0].PatrolSize;

            Hashtable hash = new Hashtable();

            Structs.Competitor[] competitors =
                myInterface.GetCompetitors(myInterface.GetPatrol(patrolId));

            foreach(Structs.Competitor competitor in competitors)
            {
                hash.Add(competitor.Lane, competitor);
            }

            for (int lane = startlane - 1; lane>0; lane--)
            {
                if (!hash.ContainsKey(lane))
                    return lane;
            }		
            throw new ApplicationException("There is no more room in this patrol");
        }


        internal void PatrolAddAutomaticCompetitors(bool CleanPatrols, bool preserveCompetitorOrder)
        {
            Trace.WriteLine("CPatrolManagement: PatrolAddAutomaticCompetitors(" +
                CleanPatrols.ToString() + ") started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() +
                " )");

            database = myInterface.databaseClass.Database;

            // First clean patrols if instructed
            if (CleanPatrols)
            {
                cleanPatrols();
                myInterface.updatedPatrol();
            }

            // Check there is any competitors
            if (this.database.Competitors
                .Select("PatrolId is null").Length == 0)
                return;

            this.myInterface.updatedPatrolAddAutomaticCompetitors(
                0, 3*database.Shooters.Count);
            
            // Create list
            DSPatrolManagement dblist = createList();

            // Sort list
            dblist = sortList(dblist);

            // Spread list evenly
            spreadList(dblist, preserveCompetitorOrder);

            // Update interface
            this.myInterface.updatedPatrolAddAutomaticCompetitors(
                3*database.Shooters.Count, 
                3*database.Shooters.Count);

            Trace.WriteLine("CPatrolManagement: PatrolAddAutomaticCompetitors ended.");
        }
        internal bool CheckChangePatrolConnectionTypeIsPossible(
            Structs.Patrol patrol,
            Structs.PatrolConnectionTypeEnum newPatrolConnectionType)
        {
            Structs.Competitor[] competitors = myInterface.GetCompetitors(patrol);
            if (competitors.Length == 0)
                return true; // Since there are no competitors, patrol is changeable

            Structs.Weapon weapon = myInterface.GetWeapon(competitors[0].WeaponId);
            Structs.PatrolClass patrolClass = myInterface.ConvertWeaponsClassToPatrolClass(
                weapon.WClass, newPatrolConnectionType);

            foreach (Structs.Competitor competitor in competitors)
            {
                weapon = myInterface.GetWeapon(competitor.WeaponId);

                if (patrolClass != myInterface.ConvertWeaponsClassToPatrolClass(
                    weapon.WClass,
                    newPatrolConnectionType))
                    return false;
            }

            return true;
        }
        internal void ChangePatrolConnectionType(
            Structs.PatrolConnectionTypeEnum newPatrolConnectionType)
        {
            Structs.Patrol[] patrols = myInterface.GetPatrols();
            foreach (Structs.Patrol patrol in patrols)
            {
                if (!CheckChangePatrolConnectionTypeIsPossible(patrol, newPatrolConnectionType))
                {
                    throw new ApplicationException("Trying to change a patrolconnectiontype which isn't possiblen on PatrolId " + patrol.PatrolId);
                }
            }
            foreach (Structs.Patrol patrol in patrols)
            {
                ChangePatrolConnectionType(newPatrolConnectionType, patrol);
            }
        }
        private void ChangePatrolConnectionType(
            Structs.PatrolConnectionTypeEnum newPatrolConnectionType,
            Structs.Patrol patrol)
        {
            Structs.Competitor[] competitors = myInterface.GetCompetitors(patrol);

            if (competitors.Length == 0)
                return;

            Structs.Weapon weapon = myInterface.GetWeapon(competitors[0].WeaponId);
            Structs.PatrolClass patrolClass = myInterface.ConvertWeaponsClassToPatrolClass(
                weapon.WClass, newPatrolConnectionType);

            if (patrolClass != patrol.PClass)
            {
                patrol.PClass = patrolClass;
                myInterface.UpdatePatrol(patrol);
            }
        }

        private void cleanPatrols()
        {
            foreach(DatabaseDataset.CompetitorsRow row in database.Competitors)
            {
                if (!row.IsPatrolIdNull())
                {
                    row.SetPatrolIdNull();
                }
            }

            // Update database
            myInterface.updatedCompetitor(new Structs.Competitor());

            // Remove those patrols!
            try
            {
                while(myInterface.GetPatrolsCount()>0)
                {
                    myInterface.PatrolRemoveLast(false);
                }
            }
            catch(Exception exc)
            {
                Trace.WriteLine("CPatrolManagement cleanPatrols: Exception: " + 
                    exc.ToString());
            }

            // Update database
            myInterface.updatedPatrol();
        }

        private DSPatrolManagement createList()
        {
            Trace.WriteLine("CPatrolManagement: createList() started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() +
                " )");

            try
            {
                DSPatrolManagement dblist = new DSPatrolManagement();

                int i = 0;
                foreach(DatabaseDataset.ShootersRow shooter in database.Shooters)
                {
                    DSPatrolManagement.PSortRow newRow = dblist.PSort.NewPSortRow();

                    newRow.ClubId = shooter.ClubId;
                    int rounds = 0;
                    foreach(System.Data.DataRow row in shooter.GetChildRows("ShootersCompetitors"))
                    {
                        if (row.IsNull("PatrolId"))
                            rounds++;
                    }
                    if (rounds > 0)
                    {
                        newRow.NrOfRounds = rounds;
                        newRow.ShooterId = shooter.ShooterId;

                        dblist.PSort.AddPSortRow(newRow);
                    }
                    i++;

                    this.myInterface.updatedPatrolAddAutomaticCompetitors(i, 3*database.Shooters.Count);
                }

                return dblist;
            }
            finally
            {
                Trace.WriteLine("CPatrolManagement: createList() ended.");
            }
        }

        private DSPatrolManagement sortList(DSPatrolManagement dblist)
        {	
            Trace.WriteLine("CPatrolManagement: sortList() started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() +
                " )");

            try
            {
                DSPatrolManagement dblistnew = new DSPatrolManagement();
                int i = 0;
                foreach (DSPatrolManagement.PSortRow row in
                    dblist.PSort.Select("", "NrOfRounds desc, ClubId"))
                {
                    DSPatrolManagement.PSortRow newRow = dblistnew.PSort.NewPSortRow();
                    newRow.ShooterId = row.ShooterId;
                    newRow.ClubId = row.ClubId;
                    newRow.NrOfRounds = row.NrOfRounds;
                    dblistnew.PSort.AddPSortRow(newRow);
                    i++;
                    this.myInterface.updatedPatrolAddAutomaticCompetitors(
                        database.Shooters.Count + i, 
                        3*database.Shooters.Count);
                    
                }

                return dblistnew;
            }
            finally
            {
                Trace.WriteLine("CPatrolManagement: sortList() ended.");
            }
        }


        private void spreadList(DSPatrolManagement dblist, bool preserveCompetitorOrder)
        {
            Trace.WriteLine("CPatrolManagement: spreadList() started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() +
                " )");

            try
            {
                // Spread that list
                int i = 0;
                foreach (DSPatrolManagement.PSortRow row in dblist.PSort)
                {
                    // For each shooter, add competitors to patrols,
                    // sorting on weaponsclass
                    int shooterId = row.ShooterId;

                    // First get those competitors, sorted on weaponsclass
                    DSPatrolManagement comps = getCompetitors(shooterId, preserveCompetitorOrder);

                    // Assign competitors to patrols with regards to minimum wait
                    assignPatrolsToCompetitors(comps, preserveCompetitorOrder);
                    i++;
                    this.myInterface.updatedPatrolAddAutomaticCompetitors(
                        2*database.Shooters.Count + i, 
                        3*database.Shooters.Count);
                }
            }
            finally
            {
                Trace.WriteLine("CPatrolManagement: spreadList() ended.");
            }
        }

        private DSPatrolManagement getCompetitors(int shooterId, bool preserveShooterOrder)
        {
            Trace.WriteLine("CPatrolManagement: getCompetitors() started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() +
                " )");

            try
            {
                // Get the competitors from the database
                Structs.Competitor[] temp = this.myInterface.GetCompetitors(shooterId);
                var competitorsUnfiltered = new List<Structs.Competitor>(temp);

                for (var i = 0; i < competitorsUnfiltered.Count;)
                {
                    var comp = competitorsUnfiltered[i];
                    if (comp.PatrolId != -1)
                    {
                        competitorsUnfiltered.Remove(comp);
                    }

                    i++;
                }

                Structs.Competitor[] competitors = competitorsUnfiltered.ToArray();

                // Insert them into a temporary table
                DSPatrolManagement dblistunsorted = new DSPatrolManagement();
                foreach (Structs.Competitor row in competitors)
                {
                    DSPatrolManagement.CompetitorsRow newRow = 
                        dblistunsorted.Competitors.NewCompetitorsRow();

                    newRow.CompetitorId = row.CompetitorId;
                    newRow.PatrolClass = 
                        (int)myInterface.ConvertWeaponsClassToPatrolClass(
                        myInterface.GetWeapon(row.WeaponId).WClass);

                    dblistunsorted.Competitors.AddCompetitorsRow(newRow);
                }

                // Sort the table into a new table
                string sorting = "PatrolClass desc";
                if (preserveShooterOrder)
                {
                    sorting = "CompetitorId," + sorting;
                }
                else
                {
                    sorting += ", CompetitorId";
                }

                DSPatrolManagement.CompetitorsRow[] rows = (DSPatrolManagement.CompetitorsRow[])
                    dblistunsorted.Competitors.Select("", sorting);

                DSPatrolManagement dblistsorted = new DSPatrolManagement();
                foreach (DSPatrolManagement.CompetitorsRow row in rows)
                {
                    DSPatrolManagement.CompetitorsRow newRow =
                        dblistsorted.Competitors.NewCompetitorsRow();

                    newRow.CompetitorId = row.CompetitorId;
                    newRow.PatrolClass = row.PatrolClass;

                    dblistsorted.Competitors.AddCompetitorsRow(newRow);
                }

                return dblistsorted;
            }
            finally
            {
                Trace.WriteLine("CPatrolManagement: getCompetitors() ended.");
            }
        }

        private void assignPatrolsToCompetitors(DSPatrolManagement comps, bool preserveOrder)
        {
            Trace.WriteLine("CPatrolManagement: assignPatrolsToCompetitors() started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() +
                " )");

            try
            {
                int patrolTime = myInterface.CompetitionCurrent.PatrolTime;
                int patrolRest = myInterface.CompetitionCurrent.PatrolTimeRest;
                int maxInPatrol = myInterface.CompetitionCurrent.PatrolSize;

                int starttime = 0;

                string sorting = "";
                if (preserveOrder)
                    sorting = "CompetitorId";
                else
                    sorting = "PatrolClass";

                foreach (DSPatrolManagement.CompetitorsRow comp in 
                    comps.Competitors.Select("", sorting))
                {
                    Structs.Competitor competitor = 
                        myInterface.GetCompetitor(comp.CompetitorId);
                    starttime = assignPatrolToCompetitor(competitor, 
                        comp.PatrolClass, 
                        starttime, maxInPatrol);
                    starttime += patrolTime + patrolRest;
                }
            }
            finally
            {
                Trace.WriteLine("CPatrolManagement: assignPatrolsToCompetitors() ended.");
            }
        }

        /// <summary>
        /// Assigns a singe competitor to a patrol with regards to
        /// weaponsclass and startime.
        /// </summary>
        /// <param name="competitor">CompetitorId</param>
        /// <param name="patrolClass">PatrolClass</param>
        /// <param name="starttime">starttime</param>
        /// <param name="maxInPatrol">Max number of Shooters in patrol</param>
        /// <returns>The number of minutes of the patrol assigned from competition start</returns>
        private int assignPatrolToCompetitor(Structs.Competitor competitor, 
            int patrolClass, int starttime, int maxInPatrol)
        {
            //DateTime compStart = myInterface.GetCompetitions()[0].StartTime;
            //int starttime = (int)(startDateTime-compStart).TotalMinutes;
            
            foreach(DatabaseDataset.PatrolsRow patrol in database.Patrols)
            {
                if(patrol.StartDateTime>=starttime & !patrol.Automatic)
                {
                    // Ok, this patrols time is OK
                    if (patrol.PClass == patrolClass | 
                        patrol.PClass == (int)Structs.PatrolClass.Okänd)
                    {
                        // Ok, this patrols class is OK
                        if (patrol.GetChildRows("PatrolsCompetitors").Length 
                            < maxInPatrol)
                        {
                            // Yes, we've got a winner!
                            competitor.PatrolId = patrol.PatrolId;
                            competitor.Lane = GetNextLane(patrol.PatrolId);
                            myInterface.UpdateCompetitor(competitor, false);

                            if (patrol.PClass == (int)Structs.PatrolClass.Okänd)
                            {
                                patrol.PClass = (int)myInterface.ConvertWeaponsClassToPatrolClass(
                                    myInterface.GetWeapon(competitor.WeaponId).WClass);
                            }
                            return patrol.StartDateTime;
                        }
                    }
                }
            }

            // Oh oh, we didn't get a patrol.
            //Add patrol and try again
            PatrolAddEmpty(false);

            return assignPatrolToCompetitor(competitor, 
                patrolClass, starttime, maxInPatrol);
        }


        readonly object PatrolRemoveLastLock = new object();
        internal void PatrolRemoveLast()
        {
            Trace.WriteLine("CPatrolManagement: patrolRemoveLast() started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() +
                " )");

            try
            {
                database = myInterface.databaseClass.Database;

                if (database.Patrols.Count == 0)
                    return;

                Trace.WriteLine("CPatrolManagement: PatrolRemoveLast() " + 
                    " locking \"PatrolRemoveLastLock\" on thread \"" +
                    Thread.CurrentThread.Name + "\" ( " +
                    System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

                lock(PatrolRemoveLastLock)
                {
                    Trace.WriteLine("CPatrolManagement: PatrolRemoveLast() " + 
                        " locked \"PatrolRemoveLastLock\" on thread \"" +
                        Thread.CurrentThread.Name + "\" ( " +
                        System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

                    if (database.Patrols.Count == 0)
                        return;

                    int pid = getHighestPatrolId();
                    if (myInterface.GetCompetitorsCountPatrol(myInterface.GetPatrol(pid)) > 0)
                        throw new ApplicationException("CompetitorsExist");

                    Structs.Patrol patrol =
                        myInterface.GetPatrol(pid);
                    myInterface.DelPatrol(patrol, false);

                    Trace.WriteLine("CPatrolManagement: PatrolRemoveLast() " + 
                        " locked \"PatrolRemoveLastLock\" on thread \"" +
                        Thread.CurrentThread.Name + "\" ( " +
                        System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
                }
            }
            catch(System.Data.DeletedRowInaccessibleException)
            {
                Trace.WriteLine("CPatrolManagement: PatrolRemoveLast DeletedRowInaccessibleException. Retrying.");

                PatrolRemoveLast();
            }
            finally
            {
                Trace.WriteLine("CPatrolManagement: PatrolRemoveLast ended.");
            }
        }

        private int getHighestPatrolId()
        {
            int highest = 0;
            foreach(DatabaseDataset.PatrolsRow ptrl in database.Patrols)
            {
                if (ptrl.RowState != System.Data.DataRowState.Deleted)
                {
                    if (ptrl.PatrolId > highest)
                        highest = ptrl.PatrolId;
                }
            }
            return highest;
        }
    }
}
