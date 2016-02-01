// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenEffect.cs" company="John Allberg">
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
//   Defines the ScreenEffect type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System.Drawing;
    using System.Windows.Forms;

    class ScreenEffect
    {
        internal void RunEasterEgg()
        {
            //gets the screens width and height, use simple vars instead of calling
            //the GetBounds method all the time
            int screenWidth = Screen.GetBounds(new Point(0, 0)).Width;
            int screenHeight = Screen.GetBounds(new Point(0, 0)).Height;

            //create a new empty bitmap that will hold a screen shot of the whole screen
            Bitmap screenShot = new Bitmap(screenWidth, screenHeight);

            Graphics gfx = Graphics.FromImage((Image)screenShot);  //attach a Graphics object to the bitmap
            gfx.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));  //gets sreenshot of screen
            //and draws it into bitmap
            ScreenEffectForm fx = new ScreenEffectForm();   //create a new instance of the specialized form called Effect
            fx.Visible = true;

            gfx = fx.CreateGraphics();  //replace the gfx instance with a new graphics object from the form

            int x = 0, y = 0, width = screenWidth, height = screenHeight;    //set up mutator vars

            while (width >= 0)    //x and y get larger, width and height get smaller
            {
                //gfx.RotateTransform(1.0f); //also neat !enable the one below!
                gfx.DrawImage((Image)screenShot, x, y, width, height);
                x++;
                y++;
                width -= 2;
                height -= 2;
            }
            while (width <= screenWidth)  //x and y get smaller, width and height get larger
            {
                //gfx.RotateTransform(-1.0f);  //also neat !enable the one above!
                gfx.DrawImage((Image)screenShot, x, y, width, height);
                x--;
                y--;
                width += 2;
                height += 2;
            }

            fx.Visible = false;
        }
    }
}