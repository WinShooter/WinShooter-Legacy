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
// $Id: SafeTextBox.cs 105 2009-01-29 10:54:00Z smuda $
using System;

namespace Allberg.Shooter.Windows.Forms
{
	/// <summary>
	/// Summary description for SafeTextBox.
	/// </summary>
	public class SafeTextBox : System.Windows.Forms.TextBox, IDisposable
	{
		public SafeTextBox() : base()
		{
            SetText += new SetStringMethodInvoker(SafeTextBox_SetText);
            GetText += new GetStringMethodInvoker(SafeTextBox_GetText);
		}

        string SafeTextBox_GetText()
        {
            return this.Text;
        }

        void SafeTextBox_SetText(string value)
        {
            this.Text = value;
        }

        private delegate void SetStringMethodInvoker(string value);
        private delegate string GetStringMethodInvoker();
        private event SetStringMethodInvoker SetText;
        private event GetStringMethodInvoker GetText;
        public override string Text
		{
			get
			{
                if (base.InvokeRequired)
                {
                    object obj = Invoke(GetText);
                    return (string)obj;
                }
                else
                    return base.Text;
			}
			set
			{
                if (base.InvokeRequired)
                {
                    Invoke(SetText, new object[] { value });
                }
                else
                    base.Text = value;
			}
		}

		protected override void Dispose(bool disposing )
		{
			base.Dispose(disposing);
		}
	}
}
