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
// $Id$
using System;

namespace Allberg.Shooter.Windows.Forms
{
	/// <summary>
	/// Summary description for TextBoxColumnCustom.
	/// </summary>
	public class DataGridTextBoxColumnCustom : System.Windows.Forms.DataGridTextBoxColumn
	{
		public DataGridTextBoxColumnCustom()
		{
		}

		protected override void Edit(System.Windows.Forms.CurrencyManager source, 
			int rowNum, 
			System.Drawing.Rectangle bounds, 
			bool breadOnly, string instantText, bool cellsVisible)
		{
		}
	}
}
