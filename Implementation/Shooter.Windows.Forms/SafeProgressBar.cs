// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SafeProgressBar.cs" company="John Allberg">
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
    using System.Windows.Forms;

    /// <summary>
    /// Summary description for SafeTextBox.
    /// </summary>
    public class SafeProgressBar : ProgressBar
    {
        public SafeProgressBar()
            : base()
        {
            getMaximumMethod += new getMaximumMethodInvoker(this.getMaximum);
            setMaximumMethod += new setMaximumMethodInvoker(this.setMaximum);

            getValueMethod += new getValueMethodInvoker(this.getValue);
            setValueMethod += new setValueMethodInvoker(this.setValue);

            getVisibleMethod += new getVisibleMethodInvoker(this.getVisible);
            setVisibleMethod += new setVisibleMethodInvoker(this.setVisible);
        }

        private delegate int getMaximumMethodInvoker();
        private getMaximumMethodInvoker getMaximumMethod;
        private delegate void setMaximumMethodInvoker(int maximum);
        private setMaximumMethodInvoker setMaximumMethod;

        private delegate int getValueMethodInvoker();
        private getValueMethodInvoker getValueMethod;
        private delegate void setValueMethodInvoker(int Value);
        private setValueMethodInvoker setValueMethod;

        private delegate bool getVisibleMethodInvoker();
        private getVisibleMethodInvoker getVisibleMethod;
        private delegate void setVisibleMethodInvoker(bool Value);
        private setVisibleMethodInvoker setVisibleMethod;

        public new int Maximum
        {
            get
            {
                if (base.InvokeRequired)
                {
                    return (int)this.Invoke(this.getMaximumMethod);
                }
                return base.Maximum;
            }
            set
            {
                if (base.InvokeRequired)
                    this.Invoke(this.setMaximumMethod, new object[] { value });
                else
                    base.Maximum = value;
            }
        }
        private int getMaximum()
        {
            return base.Maximum;
        }
        private void setMaximum(int maximum)
        {
            base.Maximum = maximum;
        }





        public new int Value
        {
            get
            {
                if (base.InvokeRequired)
                {
                    return (int)this.Invoke(this.getValueMethod);
                }
                return base.Value;
            }
            set
            {
                if (base.InvokeRequired)
                    this.Invoke(this.setValueMethod, new object[] { value });
                else
                    base.Value = value;
            }
        }
        private int getValue()
        {
            return base.Value;
        }
        private void setValue(int Value)
        {
            base.Value = Value;
        }

        public new bool Visible
        {
            get
            {
                if (base.InvokeRequired)
                {
                    return (bool)this.Invoke(this.getVisibleMethod);
                }
                return base.Visible;
            }
            set
            {
                if (base.InvokeRequired)
                    this.Invoke(this.setVisibleMethod, new object[] { value });
                else
                    base.Visible = value;
            }
        }

        private bool getVisible()
        {
            return base.Visible;
        }

        private void setVisible(bool Visible)
        {
            base.Visible = Visible;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose( disposing );
        }
    }
}
