// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Structs.cs" company="John Allberg">
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
//   Summary description for Structs.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServerRemoting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for Structs.
    /// </summary>
    public class Structs
    {
        #region Structs to work with
        /// <summary>
        /// Clubs
        /// </summary>
        [Serializable]
        public struct Club
        {
            /// <summary>
            /// 
            /// </summary>
            public string ClubId;
            /// <summary>
            /// 
            /// </summary>
            public string Name;
            /// <summary>
            /// 
            /// </summary>
            public string Country;
            /// <summary>
            /// 
            /// </summary>
            public string Email;
            /// <summary>
            /// 
            /// </summary>
            public string Plusgiro;
            /// <summary>
            /// 
            /// </summary>
            public string Bankgiro;
            /// <summary>
            /// 
            /// </summary>
            public bool Automatic;
            /// <summary>
            /// 
            /// </summary>
            public bool ToAutomatic;
        }
        /// <summary>
        /// Shooter
        /// </summary>
        [Serializable]
        public struct Shooter
        {
            /// <summary>
            /// 
            /// </summary>
            public int ShooterId;
            /// <summary>
            /// 
            /// </summary>
            public string CardNr;
            /// <summary>
            /// 
            /// </summary>
            public string Surname;
            /// <summary>
            /// 
            /// </summary>
            public string Givenname;
            /// <summary>
            /// 
            /// </summary>
            public string Email;
            /// <summary>
            /// 
            /// </summary>
            public string ClubId;
            /// <summary>
            /// 
            /// </summary>
            public int Payed;
            /// <summary>
            /// 
            /// </summary>
            public Structs.ShootersClass Class;
            /// <summary>
            /// 
            /// </summary>
            public bool ToAutomatic;
            /// <summary>
            /// 
            /// </summary>
            public bool Automatic;
            /// <summary>
            /// 
            /// </summary>
            public bool Arrived;
            /// <summary>
            /// 
            /// </summary>
            public DateTime LastUpdate;
        }
        /// <summary>
        /// Competitor
        /// </summary>
        [Serializable]
        public struct Competitor
        {
            /// <summary>
            /// 
            /// </summary>
            public int CompetitorId;
            /// <summary>
            /// 
            /// </summary>
            public int ShooterId;
            /// <summary>
            /// 
            /// </summary>
            public string WeaponId;
            /// <summary>
            /// 
            /// </summary>
            public int CompetitionId;
            /// <summary>
            /// 
            /// </summary>
            public int PatrolId;
            /// <summary>
            /// 
            /// </summary>
            public int Lane;
            /// <summary>
            /// 
            /// </summary>
            public Structs.ShootersClass ShooterClass;
            /// <summary>
            /// 
            /// </summary>
            public int FinalShootingPlace;
        }
        /// <summary>
        /// Team
        /// </summary>
        [Serializable]
        public struct Team
        {
            /// <summary>
            /// 
            /// </summary>
            public int TeamId;
            /// <summary>
            /// 
            /// </summary>
            public string ClubId;
            /// <summary>
            /// 
            /// </summary>
            public string Name;
            /// <summary>
            /// 
            /// </summary>
            public Structs.ResultWeaponsClass WClass;
            /// <summary>
            /// 
            /// </summary>
            public List<int> CompetitorIds;
        }
        /// <summary>
        /// Weapon
        /// </summary>
        [Serializable]
            public struct Weapon
        {
            /// <summary>
            /// 
            /// </summary>
            public string WeaponId;
            /// <summary>
            /// 
            /// </summary>
            public string Manufacturer;
            /// <summary>
            /// 
            /// </summary>
            public string Model;
            /// <summary>
            /// 
            /// </summary>
            public string Caliber;
            /// <summary>
            /// 
            /// </summary>
            public WeaponClass WClass;
            /// <summary>
            /// 
            /// </summary>
            public bool Automatic;
            /// <summary>
            /// 
            /// </summary>
            public bool ToAutomatic;
            /// <summary>
            /// 
            /// </summary>
            public DateTime LastUpdate;
        }
        /// <summary>
        /// Competition
        /// </summary>
        [Serializable]
        public struct Competition
        {
            /// <summary>
            /// 
            /// </summary>
            public int CompetitionId;
            /// <summary>
            /// 
            /// </summary>
            public string Name;
            /// <summary>
            /// 
            /// </summary>
            public DateTime StartTime;
            /// <summary>
            /// 
            /// </summary>
            public bool NorwegianCount;
            /// <summary>
            /// 
            /// </summary>
            public int PatrolTime;
            /// <summary>
            /// 
            /// </summary>
            public int PatrolTimeBetween;
            /// <summary>
            /// 
            /// </summary>
            public int PatrolTimeRest;
            /// <summary>
            /// 
            /// </summary>
            public int PatrolSize;
            /// <summary>
            /// 
            /// </summary>
            public bool DoFinalShooting;
            /// <summary>
            /// 
            /// </summary>
            public bool UsePriceMoney;
            /// <summary>
            /// 
            /// </summary>
            public int ShooterFee1;
            /// <summary>
            /// 
            /// </summary>
            public int ShooterFee2;
            /// <summary>
            /// 
            /// </summary>
            public int ShooterFee3;
            /// <summary>
            /// 
            /// </summary>
            public int ShooterFee4;
            /// <summary>
            /// 
            /// </summary>
            public int FirstPrice;
            /// <summary>
            /// 
            /// </summary>
            public int PriceMoneyPercentToReturn;
            /// <summary>
            /// 
            /// </summary>
            public int PriceMoneyShooterPercent;
            /// <summary>
            /// 
            /// </summary>
            public CompetitionTypeEnum Type;
            /// <summary>
            /// 
            /// </summary>
            public CompetitionChampionshipEnum Championship;
            /// <summary>
            /// 
            /// </summary>
            public PatrolConnectionTypeEnum PatrolConnectionType;
            /// <summary>
            /// 
            /// </summary>
            public bool OneClass;
        }
        /// <summary>
        /// Patrol
        /// </summary>
        [Serializable]
        public struct Patrol
        {
            /// <summary>
            /// 
            /// </summary>
            public int PatrolId;
            /// <summary>
            /// 
            /// </summary>
            public DateTime StartDateTime;
            /// <summary>
            /// 
            /// </summary>
            public DateTime StartDateTimeDisplay;
            /// <summary>
            /// 
            /// </summary>
            public int CompetitionId;
            /// <summary>
            /// 
            /// </summary>
            public PatrolClass PClass;
            /// <summary>
            /// 
            /// </summary>
            public bool LockedForAutomatic;
        }
        /// <summary>
        /// CompetitorResult
        /// </summary>
        [Serializable]
            public struct CompetitorResult
        {
            /// <summary>
            /// 
            /// </summary>
            public int ResultId;
            /// <summary>
            /// 
            /// </summary>
            public int CompetitorId;
            /// <summary>
            /// 
            /// </summary>
            public int Station;
            /// <summary>
            /// 
            /// </summary>
            public int FigureHits;
            /// <summary>
            /// 
            /// </summary>
            public int Hits;
            /// <summary>
            /// 
            /// </summary>
            public int Points;
            /// <summary>
            /// 
            /// </summary>
            public string StationFigureHits;
        }
        /// <summary>
        /// Station
        /// </summary>
        [Serializable]
            public struct Station
        {
            /// <summary>
            /// 
            /// </summary>
            public int StationId;
            /// <summary>
            /// 
            /// </summary>
            public int StationNr;
            /// <summary>
            /// 
            /// </summary>
            public int Figures;
            /// <summary>
            /// 
            /// </summary>
            public int Shoots;
            /// <summary>
            /// 
            /// </summary>
            public bool Points;
            /// <summary>
            /// 
            /// </summary>
            public int CompetitionId;
            /// <summary>
            /// 
            /// </summary>
            public bool Distinguish;
        }
        
        /// <summary>
        /// Used for following up for example payment.
        /// </summary>
        [Serializable]
        public struct FollowUpReturn
        {
            /// <summary>
            /// The shooters cardnr
            /// </summary>
            public string CardNr;
            /// <summary>
            /// The name of the shooter
            /// </summary>
            public string ShooterName;
            /// <summary>
            /// The shooters clubs name
            /// </summary>
            public string ClubName;
            /// <summary>
            /// How much the shooter have payed
            /// </summary>
            public int Payed;
            /// <summary>
            /// How much the shooter should have payed
            /// </summary>
            public int ShouldHavePayed;
            /// <summary>
            /// If the shooter has arrived
            /// </summary>
            public bool Arrived;
            /// <summary>
            /// How many rounds there is results for the shooter.
            /// </summary>
            public int ResultsExistForRounds;
            /// <summary>
            /// How many rounds the shooter should go
            /// </summary>
            public int Rounds;
        }
        #endregion

        #region Enums to work with
        /// <summary>
        /// WeaponClass
        /// </summary>
        [Serializable]
        public enum WeaponClass
        {
            /// <summary>
            /// 
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// 
            /// </summary>
            A1 = 1,
            /// <summary>
            /// 
            /// </summary>
            A2 = 2,
            /// <summary>
            /// 
            /// </summary>
            A3 = 3,
            /// <summary>
            /// 
            /// </summary>
            B = 11,
            /// <summary>
            /// 
            /// </summary>
            C = 21,
            /// <summary>
            /// 
            /// </summary>
            R = 31,
            /// <summary>
            /// 
            /// </summary>
            M = 41,
            /// <summary>
            /// 
            /// </summary>
            M1 = 42,
            /// <summary>
            /// 
            /// </summary>
            M2 = 43,
            /// <summary>
            /// 
            /// </summary>
            M3 = 44,
            /// <summary>
            /// 
            /// </summary>
            M4 = 45,
            /// <summary>
            /// 
            /// </summary>
            M5 = 46,
            /// <summary>
            /// 
            /// </summary>
            M6 = 47,
            /// <summary>
            /// 
            /// </summary>
            M7 = 48,
            /// <summary>
            /// 
            /// </summary>
            M8 = 49,
            /// <summary>
            /// 
            /// </summary>
            M9 = 50
        }
        /// <summary>
        /// ResultWeaponsClass
        /// </summary>
        [Serializable]
        public enum ResultWeaponsClass
        {
            /// <summary>
            /// 
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// 
            /// </summary>
            A =  1,
            /// <summary>
            /// 
            /// </summary>
            B = 11,
            /// <summary>
            /// 
            /// </summary>
            C = 21,
            /// <summary>
            /// 
            /// </summary>
            R = 31,
            /// <summary>
            /// 
            /// </summary>
            M = 41,
            /// <summary>
            /// 
            /// </summary>
            M1 = 42,
            /// <summary>
            /// 
            /// </summary>
            M2 = 43,
            /// <summary>
            /// 
            /// </summary>
            M3 = 44,
            /// <summary>
            /// 
            /// </summary>
            M4 = 45,
            /// <summary>
            /// 
            /// </summary>
            M5 = 46,
            /// <summary>
            /// 
            /// </summary>
            M6 = 47,
            /// <summary>
            /// 
            /// </summary>
            M7 = 48,
            /// <summary>
            /// 
            /// </summary>
            M8 = 49,
            /// <summary>
            /// 
            /// </summary>
            M9 = 50
        }
        /// <summary>
        /// MaxWeaponsClass
        /// </summary>
        public const int ResultWeaponsClassMax = 51;
        /// <summary>
        /// WeaponClassMax
        /// </summary>
        public const int WeaponClassMax = 51;
        /// <summary>
        /// PatrolClass
        /// </summary>
        [Serializable]
        public enum PatrolClass
        {
            /// <summary>
            /// 
            /// </summary>
            Okänd = 0,
            /// <summary>
            /// 
            /// </summary>
            A = 1,
            /// <summary>
            /// 
            /// </summary>
            B = 2,
            /// <summary>
            /// 
            /// </summary>
            C = 3,
            /// <summary>
            /// 
            /// </summary>
            R = 4,
            /// <summary>
            /// 
            /// </summary>
            M = 5,
            /// <summary>
            /// 
            /// </summary>
            AR = 11,
            /// <summary>
            /// 
            /// </summary>
            BC = 12,
            /// <summary>
            /// 
            /// </summary>
            ABCRM = 99
        }
        /// <summary>
        /// PatrolConnectionType
        /// </summary>
        [Serializable]
        public enum PatrolConnectionTypeEnum
        {
            /// <summary>
            /// 
            /// </summary>
            Disparate = 0,
            /// <summary>
            /// 
            /// </summary>
            AR_BC = 1,
            /// <summary>
            /// 
            /// </summary>
            AR_B_C_M = 2,
            /// <summary>
            /// 
            /// </summary>
            ABCRM = 3
        }
        /// <summary>
        /// ShootersClass
        /// </summary>
        [Serializable]
        public enum ShootersClass
        {
            /// <summary>
            /// 
            /// </summary>
            Okänd = 0,
            /// <summary>
            /// 
            /// </summary>
            Klass1 = 1,
            /// <summary>
            /// 
            /// </summary>
            Klass2 = 2,
            /// <summary>
            /// 
            /// </summary>
            Klass3 = 3,
            /// <summary>
            /// 
            /// </summary>
            Klass = 4,
            /// <summary>
            /// 
            /// </summary>
            Damklass = 10,
            /// <summary>
            /// 
            /// </summary>
            Damklass1 = 11,
            /// <summary>
            /// 
            /// </summary>
            Damklass2 = 12,
            /// <summary>
            /// 
            /// </summary>
            Damklass3 = 13,
            /// <summary>
            /// 
            /// </summary>
            Juniorklass = 20,
            /// <summary>
            /// 
            /// </summary>
            VeteranklassYngre = 30,
            /// <summary>
            /// 
            /// </summary>
            VeteranklassÄldre = 40,
            /// <summary>
            /// 
            /// </summary>
            Öppen = 50
        }
        /// <summary>
        /// ShootersClassMax
        /// </summary>
        public const int ShootersClassMax = 50;

        /// <summary>
        /// ShooterClassShort
        /// </summary>
        [Serializable]
        public enum ShootersClassShort
        {
            /// <summary>
            /// 
            /// </summary>
            _ = 0,
            /// <summary>
            /// 
            /// </summary>
            Klass1 = 1,
            /// <summary>
            /// 
            /// </summary>
            Klass2 = 2,
            /// <summary>
            /// 
            /// </summary>
            Klass3 = 3,
            /// <summary>
            /// 
            /// </summary>
            G = 4,
            /// <summary>
            /// 
            /// </summary>
            D = 10,
            /// <summary>
            /// 
            /// </summary>
            D1 = 11,
            /// <summary>
            /// 
            /// </summary>
            D2 = 12,
            /// <summary>
            /// 
            /// </summary>
            D3 = 13,
            /// <summary>
            /// 
            /// </summary>
            J = 20,
            /// <summary>
            /// 
            /// </summary>
            VY = 30,
            /// <summary>
            /// 
            /// </summary>
            VÄ = 40,
            /// <summary>
            /// 
            /// </summary>
            ÖP = 50
        }
        /// <summary>
        /// Medal
        /// </summary>
        [Serializable]
        public enum Medal
        {
            /// <summary>
            /// 
            /// </summary>
            None = 0,
            /// <summary>
            /// 
            /// </summary>
            StandardSilver = 1,
            /// <summary>
            /// 
            /// </summary>
            StardardBrons = 2
        }
        /// <summary>
        /// CompetitionTypeEnum
        /// </summary>
        [Serializable]
        public enum CompetitionTypeEnum
        {
            /// <summary>
            /// 
            /// </summary>
            Field = 0,
            /// <summary>
            /// 
            /// </summary>
            Precision = 1,
            /// <summary>
            /// 
            /// </summary>
            MagnumField = 2
        }
        /// <summary>
        /// CompetitionChampionship
        /// </summary>
        public enum CompetitionChampionshipEnum
        {
            /// <summary>
            /// Nationell a championship
            /// </summary>
            Club = 0,
            /// <summary>
            /// Nationell a championship
            /// </summary>
            Nationell = 1,
            /// <summary>
            /// Landsdelsmästerskap
            /// </summary>
            Krets = 2,
            /// <summary>
            /// Kretsmästerskap
            /// </summary>
            Landsdel = 3,
            /// <summary>
            /// SM
            /// </summary>
            SM = 4
        }
        #endregion
    }
}
