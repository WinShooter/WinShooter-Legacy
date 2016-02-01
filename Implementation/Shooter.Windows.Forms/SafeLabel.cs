// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SafeLabel.cs" company="John Allberg">
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
//   Summary description for SafeTextBox.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows.Forms
{
    /// <summary>
    /// Summary description for SafeTextBox.
    /// </summary>
    public class SafeLabel : System.Windows.Forms.Label
    {
        public SafeLabel() : base()
        {
            SetText += new SetTextHandler(SafeLabel_SetText);
            GetText += new GetTextHandler(SafeLabel_GetText);
        }

        string SafeLabel_GetText()
        {
            return base.Text;
        }

        void SafeLabel_SetText(string newText)
        {
            base.Text = newText;
        }

        public override string Text
        {
            get
            {
                if (base.InvokeRequired)
                {
                    return (string)Invoke(GetText);
                }
                else
                {
                    return base.Text;
                }
            }
            set
            {
                if (base.InvokeRequired)
                {
                    Invoke(SetText, new object[] { value });
                }
                else
                {
                    base.Text = value;
                }
            }
        }

        private delegate void SetTextHandler(string newText);
        private event SetTextHandler SetText;
        private delegate string GetTextHandler();
        private event GetTextHandler GetText;

        protected override void Dispose(bool disposing )
        {
            base.Dispose(disposing);
        }
    }
}
