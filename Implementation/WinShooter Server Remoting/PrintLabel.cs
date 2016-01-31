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
// $Id: PrintLabel.cs 128 2011-05-28 17:07:54Z smuda $

namespace Allberg.Shooter.WinShooterServerRemoting
{
	/// <summary>
	/// Summary description for PrintLabel.
	/// </summary>
	public class PrintLabel
	{
		/// <summary>
		/// PrintLabel
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="sizeX"></param>
		/// <param name="sizeY"></param>
		/// <param name="marginalLeft"></param>
		/// <param name="marginalTop"></param>
		public PrintLabel(float x, float y, float sizeX, float sizeY, 
			float marginalLeft, float marginalTop)
		{
			X = x;
			Y = y;
			SizeX = sizeX;
			SizeY = sizeY;
			MarginalLeft = marginalLeft;
			MarginalTop = marginalTop;
		}

		/// <summary>
		/// x
		/// </summary>
		public float X;
		/// <summary>
		/// y
		/// </summary>
		public float Y;
		/// <summary>
		/// 
		/// </summary>
		public float SizeX;
		/// <summary>
		/// 
		/// </summary>
		public float SizeY;
		/// <summary>
		/// 
		/// </summary>
		public float MarginalLeft;
		/// <summary>
		/// 
		/// </summary>
		public float MarginalTop;
	}
}
