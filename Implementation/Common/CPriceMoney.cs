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
// $Id: CPriceMoney.cs 105 2009-01-29 10:54:00Z smuda $
using System;

namespace Allberg.Shooter.Common
{
	/// <summary>
	/// Class to calculate pricemoney
	/// </summary>
	internal class CPriceMoney
	{
		internal CPriceMoney(int nrOfShooterWithPrice,
			int minimi,
			int max,
			int totalAmount,
			int shooterFee)
		{
			NrOfShooterWithPrice = nrOfShooterWithPrice;
			Minimi = minimi;
			Max = max;
			TotalAmount = totalAmount;
			ShooterFee = shooterFee;
		}		
		
		private int NrOfShooterWithPrice = 0;
		private int Minimi = 0;
		private int Max = 0;
		private int TotalAmount = 0;
		private int ShooterFee = 0;

		internal int[] Calculate(double PercentMoneyToReturn)
		{
			int[] toReturn = new int[NrOfShooterWithPrice];
			int secondPrice = Max;

			double currentTotalSum = 1;
			while(currentTotalSum>PercentMoneyToReturn)
			{
				if (secondPrice > Minimi)
				{
					secondPrice = secondPrice-10;
					currentTotalSum = calculateTotalSum(ref toReturn, secondPrice);
				}
				else
				{
					Max = Max - 10;
					secondPrice = Max - 10;
					currentTotalSum = calculateTotalSum(ref toReturn, secondPrice);
				}
			}
			return toReturn;
		}

		private double calculateTotalSum(ref int[] toReturn, int secondPrice)
		{
			if (toReturn.Length== 0)
			{
				toReturn = new int[1];
				toReturn[0] = Max;
				return Max;
			}

			toReturn[0] = Max;
			int totalSum = Max;
			for (int i=0; i<toReturn.Length-1;i++)
			{
				int thisSum = (NrOfShooterWithPrice-i-2)*(secondPrice-ShooterFee)/
					(NrOfShooterWithPrice);
				thisSum+= ShooterFee;
				thisSum = 10*(thisSum/10);
				toReturn[i+1] = thisSum;
				totalSum = totalSum + thisSum;
			}

			double temp = (double)totalSum / (double)TotalAmount;
			return temp;
		}
	}
}
