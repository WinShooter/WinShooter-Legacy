// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SafeComboBox.cs" company="John Allberg">
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
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    /// <summary>
    /// Summary description for SafeTextBox.
    /// </summary>
    public class SafeComboBox : ComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeComboBox"/> class.
        /// </summary>
        public SafeComboBox() : base()
        {
            getSelectedValueMethod += new getSelectedValueMethodInvoker(this.getSelectedValue);
            getSelectedIndexMethod += new getSelectedIndexMethodInvoker(this.getSelectedIndex);
            getTextMethod += new getTextMethodInvoker(this.getText);
            getDataSourceMethod += new getDataSourceMethodInvoker(this.getDataSource);

            setSelectedValueMethod += new setSelectedValueMethodInvoker(this.setSelectedValue);
            setSelectedIndexMethod += new setSelectedIndexMethodInvoker(this.setSelectedIndex);
            setDataSourceMethod += new setDataSourceMethodInvoker(this.setDataSource);

        }

        private delegate object getSelectedValueMethodInvoker();
        private getSelectedValueMethodInvoker getSelectedValueMethod;
        private delegate int getSelectedIndexMethodInvoker();
        private getSelectedIndexMethodInvoker getSelectedIndexMethod;
        private delegate string getTextMethodInvoker();
        private getTextMethodInvoker getTextMethod;
        private delegate object getDataSourceMethodInvoker();
        private getDataSourceMethodInvoker getDataSourceMethod;

        private delegate void setSelectedValueMethodInvoker(object obj);
        private setSelectedValueMethodInvoker setSelectedValueMethod;
        private delegate void setSelectedIndexMethodInvoker(int index);
        private setSelectedIndexMethodInvoker setSelectedIndexMethod;
        private delegate void setDataSourceMethodInvoker(object obj);
        private setDataSourceMethodInvoker setDataSourceMethod;

        public override string Text
        {
            get
            {
                if (base.InvokeRequired)
                {
                    return (string)this.Invoke(this.getTextMethod);
                }
                return base.Text;
            }
            set
            {
                if (base.InvokeRequired)
                    throw new Exception("Invoke Required on " + base.Name);
                base.Text = value;
            }
        }

        private string getText()
        {
            return this.Text;
        }

        public override int SelectedIndex
        {
            get
            {
                if (base.InvokeRequired)
                {
                    return (int)this.Invoke(this.getSelectedIndexMethod);
                }
                return base.SelectedIndex;
            }
            set
            {
                if (base.InvokeRequired)
                {
                    this.Invoke(setSelectedValueMethod, new object[] { value } );

                    return;
                }

                try
                {
                    try
                    {
                        base.SelectedIndex = value;
                    }
                    catch(System.ArgumentOutOfRangeException)
                    {
                    }
                    catch(System.ArgumentNullException)
                    {
                    }
                    catch(System.NullReferenceException)
                    {
                    }
                }
                catch(Exception exc)
                {
                    Trace.WriteLine("Exception occured in SafeComboBox (" + this.Name + "):" + exc.ToString());
                    this.SelectedIndex = -1;
                    throw;
                }
            }
        }

        private int getSelectedIndex()
        {
            return this.SelectedIndex;
        }
        private void setSelectedIndex(int index)
        {
            base.SelectedIndex = index;
        }


        public new object SelectedValue
        {
            get
            {
                if (base.InvokeRequired)
                {
                    return this.Invoke(this.getSelectedValueMethod);
                }
                return base.SelectedValue;
            }
            set
            {
                if (base.InvokeRequired)
                {
                    if (value != null)
                    {
                        this.Invoke(setSelectedValueMethod, new object[] { value });
                        //throw new Exception("Invoke Required while setting value on " + base.Name);
                    }
                }
                try
                {
                    if (value != null)
                        base.SelectedValue = value;
                    else
                        base.SelectedIndex = -1;
                }
                catch(Exception exc)
                {
                    Trace.WriteLine("Exception: " + exc.ToString());
                    base.SelectedIndex = -1;
                }
            }
        }

        private object getSelectedValue()
        {
            return base.SelectedValue;
        }

        private void setSelectedValue(object obj)
        {
            if (obj != null)
                base.SelectedValue = obj;
        }



        public new object DataSource
        {
            get
            {
                if (base.InvokeRequired)
                {
                    return this.Invoke(getDataSourceMethod);
                }
                return base.DataSource;
            }
            set
            {
                if (base.InvokeRequired)
                {
                    if (value != null)
                    {
                        this.Invoke(setDataSourceMethod, new object[] { value });
                        //throw new Exception("Invoke Required while setting value on " + base.Name);
                    }
                }
                try
                {
                    base.DataSource = value;
                }
                catch(Exception)
                {
                    base.SelectedIndex = -1;
                    throw;
                }
            }
        }

        private object getDataSource()
        {
            return base.DataSource;
        }

        private void setDataSource(object val)
        {
            base.DataSource = val;
        }

        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );
        }

        #region Entering values and selecting
        DateTime lastType = DateTime.Now;
        string typed = "";
        public int MilliSecondsBetweenTypes = 2000;

        protected override void OnKeyUp(KeyEventArgs e)
        {
            //base.OnKeyUp (e);

            int index;

            if (Char.IsControl((char)e.KeyData))
                return;

            // Do nothing for certain keys such as navigation keys
            if ((e.KeyCode == Keys.Back) | 
                (e.KeyCode == Keys.Left) |
                (e.KeyCode == Keys.Right) |
                (e.KeyCode == Keys.Up) |
                (e.KeyCode == Keys.Delete) |
                (e.KeyCode == Keys.Down) |
                (e.KeyCode == Keys.Prior) |
                (e.KeyCode == Keys.PageUp) |
                (e.KeyCode == Keys.PageDown) |
                (e.KeyCode == Keys.Next) |
                (e.KeyCode == Keys.Home) |
                (e.KeyCode == Keys.ShiftKey) |
                (e.KeyCode == Keys.End)) 
            {
                return;
            }

            DroppedDown = true;

            if ((DateTime.Now-lastType)
                .TotalMilliseconds > MilliSecondsBetweenTypes)
            {
                typed = "";
            }

            lastType = DateTime.Now;
            typed += e.KeyData.ToString();

            // Find the first match for the typed value
            index = FindString(typed);

            // Get the text of the first match
            if (index > -1) 
            {
                // Select this item from the list
                SelectedIndex = index;
            }
            e.Handled = true;
        }
        #endregion
    }
}
