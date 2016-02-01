namespace Allberg.Shooter.Windows.Forms
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// Summary description for DataGridCustom.
    /// </summary>
    public class DataGridCustom : System.Windows.Forms.DataGrid
    {
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
