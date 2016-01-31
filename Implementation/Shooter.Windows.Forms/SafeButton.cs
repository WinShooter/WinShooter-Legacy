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
// $Id: SafeButton.cs 130 2011-05-28 17:32:36Z smuda $
using System;
using System.Windows.Forms;

namespace Allberg.Shooter.Windows.Forms
{
	/// <summary>
	/// Summary description for SafeButtonBox.
	/// </summary>
	public class SafeButton : Button
	{
		public SafeButton()
		{
			_getTextMethod += GetText;
			_setTextMethod += SetText;

			_getEnabledMethod += GetEnabled;
			_setEnabledMethod += SetEnabled;
		}

		#region Text
		private delegate string GetTextMethodInvoker();
		private readonly GetTextMethodInvoker _getTextMethod;
		private delegate void SetTextMethodInvoker(string text);
		private readonly SetTextMethodInvoker _setTextMethod;

		override public string Text
		{
			get
			{
				if (InvokeRequired)
				{
					return (string)Invoke(_getTextMethod);
				}
				return base.Text;
			}
			set
			{
				if (InvokeRequired)
					throw new Exception("Invoke Required on " + Name);
				base.Text = value;
			}
		}

		private string GetText()
		{
			return Text;
		}
		private void SetText(string text)
		{
			Text = text;
		}
		#endregion

		#region Enabled
		private delegate bool GetEnabledMethodInvoker();
		private readonly GetEnabledMethodInvoker _getEnabledMethod;
		private delegate void SetEnabledMethodInvoker(bool enabled);
		private readonly SetEnabledMethodInvoker _setEnabledMethod;

		public new bool Enabled
		{
			get
			{
				if (InvokeRequired)
				{
					return (bool)Invoke(_getEnabledMethod);
				}
				return base.Enabled;
			}
			set
			{
				if (InvokeRequired)
				{
					Invoke(_setEnabledMethod, new object[] { value });
				}
				base.Enabled = value;
			}
		}

		private bool GetEnabled()
		{
			return Enabled;
		}
		private void SetEnabled(bool enabled)
		{
			Enabled = enabled;
		}
		#endregion
	}
}
