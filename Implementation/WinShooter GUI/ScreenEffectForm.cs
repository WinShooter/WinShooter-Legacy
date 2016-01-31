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
// $Id: ScreenEffectForm.cs 107 2009-02-01 06:25:33Z smuda $ 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Allberg.Shooter.Windows
{
	public partial class ScreenEffectForm : Form
	{
		public ScreenEffectForm()
		{
			InitializeComponent();

			this.Cursor = Cursors.VSplit;

			this.WindowState = FormWindowState.Maximized;  //*take the whole screen 
			this.BackColor = Color.Black;
			this.FormBorderStyle = FormBorderStyle.None;   //*take the whole screen
			this.TopMost = true;  //make it seem like its actually manipulating the screen itself
			this.Show();
		}
	}
}