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
// $Id: FormatGiro.cs 105 2009-01-29 10:54:00Z smuda $ 
using System;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for FormatGiro.
	/// </summary>
	internal class FormatGiro
	{
		private FormatGiro()
		{
		}

		internal static string FormatBankgiro(string bankgiro)
		{
			string bankgiroInternal = bankgiro.Replace(" ", "").Replace("-", "");
			switch(bankgiroInternal.Length)
			{
				case 0:
					return "";
				case 7:
					return bankgiroInternal.Substring(0,3) + "-" +
						bankgiroInternal.Substring(3, 4);

				case 8:
					return bankgiroInternal.Substring(0,4) + "-" +
						bankgiroInternal.Substring(4, 4);

				default:
					throw new ApplicationException("Okänd längt på bankgiro");
			}
		}

		internal static string FormatPlusgiro(string plusgiro)
		{
			string plusgiroInternal = plusgiro.Replace(" ", "").Replace("-", "");
			switch(plusgiroInternal.Length)
			{
				case 0:
					return "";
				case 2:
					return plusgiroInternal.Substring(0,1) + 
						"-" +
						plusgiroInternal.Substring(1,1);

				case 3:
					return plusgiroInternal.Substring(0,2) + 
						"-" +
						plusgiroInternal.Substring(2,1);
					
				case 4:
					return plusgiroInternal.Substring(0,3) + 
						"-" +
						plusgiroInternal.Substring(3,1);
					
				case 5:
					return plusgiroInternal.Substring(2,2) + 
						"-" +
						plusgiroInternal.Substring(4,1);
					
				case 6:
					return plusgiroInternal.Substring(0,2) + " " +
						plusgiroInternal.Substring(2,3) + "-" +
						plusgiroInternal.Substring(5,1);
					
				case 7:
					return plusgiroInternal.Substring(0,2) + " " +
						plusgiroInternal.Substring(2,2) + " " +
						plusgiroInternal.Substring(4,2) + "-" +
						plusgiroInternal.Substring(6,1);
					
				case 8:
					return plusgiroInternal.Substring(0,3) + " " +
						plusgiroInternal.Substring(3,2) + " " +
						plusgiroInternal.Substring(5,2) + "-" +
						plusgiroInternal.Substring(7,1);
					
				default:
					throw new ApplicationException("Okänd längt på plusgiro");
			}
		}

	}
}
