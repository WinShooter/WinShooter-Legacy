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
// $Id: CConvert.cs 121 2011-05-28 16:02:14Z smuda $
using System;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Common
{
	/// <summary>
	/// Summary description for CConvert.
	/// </summary>
	[Serializable]
	internal class CConvert
	{
		static internal Structs.ResultWeaponsClass ConvertWeaponsClassToResultClass(
			Structs.WeaponClass weapon, Structs.CompetitionTypeEnum compType)
		{
			switch(compType)
			{
				case Structs.CompetitionTypeEnum.MagnumField:
				{
					switch(weapon)
					{
						case Structs.WeaponClass.M1:
							return Structs.ResultWeaponsClass.M1;
						case Structs.WeaponClass.M2:
							return Structs.ResultWeaponsClass.M2;
						case Structs.WeaponClass.M3:
							return Structs.ResultWeaponsClass.M3;
						case Structs.WeaponClass.M4:
							return Structs.ResultWeaponsClass.M4;
						case Structs.WeaponClass.M5:
							return Structs.ResultWeaponsClass.M5;
						case Structs.WeaponClass.M6:
							return Structs.ResultWeaponsClass.M6;
						case Structs.WeaponClass.M7:
							return Structs.ResultWeaponsClass.M7;
						case Structs.WeaponClass.M8:
							return Structs.ResultWeaponsClass.M8;
						case Structs.WeaponClass.M9:
							return Structs.ResultWeaponsClass.M9;
						default:
							throw new CannotFindIdException("WeaponClass " + 
								weapon + " is not used in MagnumField");
					}
				}
				default:
				{
					switch(weapon)
					{
						case Structs.WeaponClass.A1:
							return Structs.ResultWeaponsClass.A;
						case Structs.WeaponClass.A2:
							return Structs.ResultWeaponsClass.A;
						case Structs.WeaponClass.A3:
							return Structs.ResultWeaponsClass.A;
						case Structs.WeaponClass.B:
							return Structs.ResultWeaponsClass.B;
						case Structs.WeaponClass.C:
							return Structs.ResultWeaponsClass.C;
						case Structs.WeaponClass.M:
							return Structs.ResultWeaponsClass.M;
						case Structs.WeaponClass.R:
							return Structs.ResultWeaponsClass.R;
						default:
							throw new CannotFindIdException("Weaponclass " + 
								weapon + " could not be found.");
					}
				}
			}
		}

		static internal Structs.PatrolClass ConvertWeaponsClassToPatrolClass(
			Structs.WeaponClass weapon,
			Structs.PatrolConnectionTypeEnum type)
		{
			Structs.PatrolClass returnClass = Structs.PatrolClass.Okänd;

			switch(type)
			{
				case Structs.PatrolConnectionTypeEnum.Disparate:
				{
					switch(weapon)
					{
						case Structs.WeaponClass.Unknown:
							returnClass = Structs.PatrolClass.Okänd;
							break;
						case Structs.WeaponClass.A1:
							returnClass = Structs.PatrolClass.A;
							break;
						case Structs.WeaponClass.A2:
							returnClass = Structs.PatrolClass.A;
							break;
						case Structs.WeaponClass.A3:
							returnClass = Structs.PatrolClass.A;
							break;
						case Structs.WeaponClass.B:
							returnClass = Structs.PatrolClass.B;
							break;
						case Structs.WeaponClass.C:
							returnClass = Structs.PatrolClass.C;
							break;
						case Structs.WeaponClass.R:
							returnClass = Structs.PatrolClass.R;
							break;
						case Structs.WeaponClass.M:
							returnClass = Structs.PatrolClass.M;
							break;
					}
					break;
				}
				case Structs.PatrolConnectionTypeEnum.AR_BC:
				{
					switch(weapon)
					{
						case Structs.WeaponClass.Unknown:
							returnClass = Structs.PatrolClass.Okänd;
							break;
						case Structs.WeaponClass.A1:
							returnClass = Structs.PatrolClass.AR;
							break;
						case Structs.WeaponClass.A2:
							returnClass = Structs.PatrolClass.AR;
							break;
						case Structs.WeaponClass.A3:
							returnClass = Structs.PatrolClass.AR;
							break;
						case Structs.WeaponClass.B:
							returnClass = Structs.PatrolClass.BC;
							break;
						case Structs.WeaponClass.C:
							returnClass = Structs.PatrolClass.BC;
							break;
						case Structs.WeaponClass.R:
							returnClass = Structs.PatrolClass.AR;
							break;
						case Structs.WeaponClass.M:
							returnClass = Structs.PatrolClass.M;
							break;
					}
					break;
				}
				case Structs.PatrolConnectionTypeEnum.AR_B_C_M:
				{
					switch (weapon)
					{
						case Structs.WeaponClass.Unknown:
							returnClass = Structs.PatrolClass.Okänd;
							break;
						case Structs.WeaponClass.A1:
							returnClass = Structs.PatrolClass.AR;
							break;
						case Structs.WeaponClass.A2:
							returnClass = Structs.PatrolClass.AR;
							break;
						case Structs.WeaponClass.A3:
							returnClass = Structs.PatrolClass.AR;
							break;
						case Structs.WeaponClass.B:
							returnClass = Structs.PatrolClass.B;
							break;
						case Structs.WeaponClass.C:
							returnClass = Structs.PatrolClass.C;
							break;
						case Structs.WeaponClass.R:
							returnClass = Structs.PatrolClass.AR;
							break;
						case Structs.WeaponClass.M:
							returnClass = Structs.PatrolClass.M;
							break;
					}
					break;
				}
				case Structs.PatrolConnectionTypeEnum.ABCRM:
				{
					switch (weapon)
					{
						case Structs.WeaponClass.Unknown:
							returnClass = Structs.PatrolClass.Okänd;
							break;
						case Structs.WeaponClass.A1:
							returnClass = Structs.PatrolClass.ABCRM;
							break;
						case Structs.WeaponClass.A2:
							returnClass = Structs.PatrolClass.ABCRM;
							break;
						case Structs.WeaponClass.A3:
							returnClass = Structs.PatrolClass.ABCRM;
							break;
						case Structs.WeaponClass.B:
							returnClass = Structs.PatrolClass.ABCRM;
							break;
						case Structs.WeaponClass.C:
							returnClass = Structs.PatrolClass.ABCRM;
							break;
						case Structs.WeaponClass.R:
							returnClass = Structs.PatrolClass.ABCRM;
							break;
						case Structs.WeaponClass.M:
							returnClass = Structs.PatrolClass.ABCRM;
							break;
					}
					break;
				}
				default:
				{
					throw new NotImplementedException(type.ToString());
				}
			}
			return returnClass;
		}

	}
}
