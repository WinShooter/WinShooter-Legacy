// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsReturnTeam.cs" company="John Allberg">
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
//   ResultsReturn
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServerRemoting
{
    using System;

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
