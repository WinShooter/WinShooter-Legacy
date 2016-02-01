// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CallBackClass.cs" company="John Allberg">
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
//   Summary description for CallBackInterface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServerRemoting
{
    using System;
    using System.Security.Permissions;

    /// <summary>
    /// Summary description for CallBackInterface.
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    [SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    public abstract class CallBackClass : MarshalByRefObject
    {
        /// <summary>
        /// This is to insure that when created as a Singleton, the first instance never dies,
        /// regardless of the time between chat users.
        /// </summary>
        /// <returns>null</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Called to init update clubs
        /// </summary>
        public void LocalUpdatedClub()
        {
            InternalUpdatedClub();
        }
        /// <summary>
        /// Called to init update clubs
        /// </summary>
        protected abstract void InternalUpdatedClub();

        /// <summary>
        /// Called to init update Competition
        /// </summary>
        public void LocalUpdatedCompetition()
        {
            InternalUpdatedCompetition();
        }
        /// <summary>
        /// Called to init update Competition
        /// </summary>
        protected abstract void InternalUpdatedCompetition();

        /// <summary>
        /// Called to init update Competitor
        /// </summary>
        public void LocalUpdatedCompetitor()
        {
            InternalUpdatedCompetitor();
        }
        /// <summary>
        /// Called to init update Competitor
        /// </summary>
        protected abstract void InternalUpdatedCompetitor();

        /// <summary>
        /// Called to init update CompetitorResult
        /// </summary>
        public void LocalUpdatedCompetitorResult()
        {
            InternalUpdatedCompetitorResult();
        }
        /// <summary>
        /// Called to init update CompetitorResult
        /// </summary>
        protected abstract void InternalUpdatedCompetitorResult();

        /// <summary>
        /// Called to init update FileImportCount
        /// </summary>
        public void LocalUpdatedFileImportCount()
        {
            InternalUpdatedFileImportCount();
        }
        /// <summary>
        /// Called to init update FileImportCount
        /// </summary>
        protected abstract void InternalUpdatedFileImportCount();

        /// <summary>
        /// Called to init update Patrol
        /// </summary>
        public void LocalUpdatedPatrol()
        {
            InternalUpdatedPatrol();
        }
        /// <summary>
        /// Called to init update Patrol
        /// </summary>
        protected abstract void InternalUpdatedPatrol();

        /// <summary>
        /// Called to init update Shooter
        /// </summary>
        public void LocalUpdatedShooter()
        {
            InternalUpdatedShooter();
        }
        /// <summary>
        /// Called to init update Shooter
        /// </summary>
        protected abstract void InternalUpdatedShooter();

    }
}
