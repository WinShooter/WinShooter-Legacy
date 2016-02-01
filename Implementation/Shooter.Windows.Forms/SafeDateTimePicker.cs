// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SafeDateTimePicker.cs" company="John Allberg">
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
//   Defines the SafeDateTimePicker type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows.Forms
{
    using System;
    using System.Windows.Forms;

    public class SafeDateTimePicker : DateTimePicker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeDateTimePicker"/> class.
        /// </summary>
        public SafeDateTimePicker() : base()
        {
            getValueHandler += new GetDateTimeDelegate(getValue);
            setValueHandler += new SetDateTimeDelegate(setValue);
        }

        private delegate DateTime GetDateTimeDelegate();
        private event GetDateTimeDelegate getValueHandler;
        private delegate void SetDateTimeDelegate(DateTime dateTime);
        private event SetDateTimeDelegate setValueHandler;

        public new DateTime Value
        {
            get
            {
                if (base.InvokeRequired)
                    return (DateTime)Invoke(getValueHandler);
                else
                    return base.Value;
            }
            set 
            {
                if (base.InvokeRequired)
                    Invoke(setValueHandler, new object[] { value });
                else
                    base.Value = value;
            }
        }

        private DateTime getValue()
        {
            return base.Value;
        }
        private void setValue(DateTime dateTime)
        {
            base.Value = dateTime;
        }
    }
}
