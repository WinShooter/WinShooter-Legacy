// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SafeCheckBox.cs" company="John Allberg">
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
//   Defines the SafeCheckBox type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows.Forms
{
    using System.Windows.Forms;

    public class SafeCheckBox : CheckBox
    {
        public SafeCheckBox()
            : base()
        {
            getCheckedHandler += new GetCheckedDelegate(getChecked);
            setCheckedHandler += new SetCheckedDelegate(setChecked);
        }

        private delegate bool GetCheckedDelegate();
        private event GetCheckedDelegate getCheckedHandler;
        private delegate void SetCheckedDelegate(bool isChecked);
        private event SetCheckedDelegate setCheckedHandler;

        public new bool Checked
        {
            get
            {
                if (base.InvokeRequired)
                    return (bool)Invoke(getCheckedHandler);
                else
                    return base.Checked;
            }
            set 
            {
                if (base.InvokeRequired)
                    Invoke(setCheckedHandler, new object[] { value });
                else
                    base.Checked = value;
            }
        }

        private bool getChecked()
        {
            return base.Checked;
        }
        private void setChecked(bool isChecked)
        {
            base.Checked = isChecked;
        }
    }
}
