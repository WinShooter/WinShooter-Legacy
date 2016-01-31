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
// $Id: DataGridCustom.cs 105 2009-01-29 10:54:00Z smuda $
using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace Allberg.Shooter.Windows.Forms
{
	/// <summary>
	/// Summary description for DataGridCustom.
	/// </summary>
	public class DataGridCustom : System.Windows.Forms.DataGrid
	{
		/// <summary>
		/// Creates an instance of the DataGridCustom class.
		/// </summary>
		public DataGridCustom()
		{
		}

		/// <summary>
		/// Sets the DataSource and DataMember properties at run time.
		/// </summary>
		/// <param name="dataSource">The data source for the System.Windows.Forms.DataGrid control. </param>
		/// <param name="dataMember">The DataMember string that specifies the table to bind to within the object returned by the DataSource property.</param>
		public new void SetDataBinding(object dataSource,
			string dataMember)
		{
			if (base.InvokeRequired)
			{
				Trace.WriteLine("DataGridCustom.SetDataBinding \"" + 
					this.Name + 
					"\" Invoke is required. Method was called on thread \"" +
					Thread.CurrentThread.Name + "\" threadId=" +
                    Thread.CurrentThread.ManagedThreadId.ToString());

				throw new ApplicationException("DataGridCustom.SetDataBinding \"" + 
					this.Name + 
					"\" Invoke is required. Method was called on thread \"" +
					Thread.CurrentThread.Name + "\" threadId=" +
					Thread.CurrentThread.ManagedThreadId.ToString());
			}

			try
			{
				// bind base
				//int vscroll = base.VertScrollBar.Value;
				base.SetDataBinding(dataSource, dataMember);

				// Make sure that the new datagrid is scrolled down to where it was.
				System.Windows.Forms.ScrollEventArgs se = 
					new System.Windows.Forms.ScrollEventArgs(
					System.Windows.Forms.ScrollEventType.EndScroll,
					base.VertScrollBar.Value);

				base.GridVScrolled(base.VertScrollBar, se);

			}
			catch(Exception exc)
			{
				Trace.WriteLine("DataGridCustom: Exception: " + exc.ToString());
				throw;
			}
			finally
			{
				// re-enabling
				this.Enabled = true;
			}
		}

	}
}
