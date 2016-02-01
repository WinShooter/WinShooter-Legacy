// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenEffectForm.cs" company="John Allberg">
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
//   Defines the ScreenEffectForm type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System.Drawing;
    using System.Windows.Forms;

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