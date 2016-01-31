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
// $Id: ISettings.cs 128 2011-05-28 17:07:54Z smuda $ 

namespace Allberg.Shooter.WinShooterServerRemoting
{
	/// <summary>
	/// Summary description for ISettings.
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Printer settings
		/// </summary>
		IPrinterSettings PrinterSettings
		{
			get;
		}

		/// <summary>
		/// The logo to display
		/// </summary>
		System.Drawing.Image Logo
		{
			get;
			set;
		}

		/// <summary>
		/// The logo to display with max height and width
		/// </summary>
		/// <param name="maxImageHeight"></param>
		/// <param name="maxImageWidth"></param>
		/// <returns></returns>
		System.Drawing.Image GetResizedLogo(int maxImageHeight, int maxImageWidth);

		/// <summary>
		/// The winshooter logo to display with max height and width
		/// </summary>
		/// <param name="maxImageHeight"></param>
		/// <param name="maxImageWidth"></param>
		/// <returns></returns>
		System.Drawing.Image GetWinshooterLogo(int maxImageHeight, int maxImageWidth);

	}
}
