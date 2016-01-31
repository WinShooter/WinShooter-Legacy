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
// $Id: IPrinterSettings.cs 128 2011-05-28 17:07:54Z smuda $ 
using System.Drawing;

namespace Allberg.Shooter.WinShooterServerRemoting
{
	/// <summary>
	/// Summary description for IPrinterSettings.
	/// </summary>
	public interface IPrinterSettings
	{
		/// <summary>
		/// Logo to print
		/// </summary>
		Image Logo
		{
			get;
			set;
		}

		/// <summary>
		/// The copyright text on print
		/// </summary>
		string Copyright
		{
			get;
		}

		/// <summary>
		/// The MirrorPrintLabelDocument...
		/// </summary>
		PrintLabelDocument LabelMirrorPrintDocument
		{
			get;
			set;
		}

		/// <summary>
		/// The ResultLabelDocument...
		/// </summary>
		PrintLabelDocument LabelResultDocument
		{
			get;
			set;
		}

		PrintDocumentStd PaperResultDocument
		{
			get;
			set;
		}

		PrintDocumentStd PaperResultTeamDocument
		{
			get;
			set;
		}

	}
}
