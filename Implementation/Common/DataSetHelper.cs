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
// $Id: DataSetHelper.cs 105 2009-01-29 10:54:00Z smuda $
using System;
using System.Data;

namespace Allberg.Shooter.Common
{
	/// <summary>
	/// Summary description for DataSetHelper.
	/// </summary>
	internal class DataSetHelper
	{
		internal DataSet ds;
		internal DataSetHelper(ref DataSet DataSet)
		{
			ds = DataSet;
		}
		internal DataSetHelper()
		{
			ds = null;
		}

		private bool ColumnEqual(object A, object B)
		{

			// Compares two values to see if they are equal. Also compares DBNULL.Value.
			// Note: If your DataTable contains object fields, then you must extend this
			// function to handle them in a meaningful way if you intend to group on them.

			if ( A == DBNull.Value && B == DBNull.Value ) //  both are DBNull.Value
				return true;
			if ( A == DBNull.Value || B == DBNull.Value ) //  only one is DBNull.Value
				return false;
			return ( A.Equals(B) );  // value type standard comparison
		}

		public DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
		{
			DataTable dt = new DataTable(TableName);
			dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

			object LastValue = null;
			foreach (DataRow dr in SourceTable.Select("", FieldName))
			{
				if (  LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])) )
				{
					LastValue = dr[FieldName];
					dt.Rows.Add(new object[]{LastValue});
				}
			}
			if (ds != null)
				ds.Tables.Add(dt);
			return dt;
		}
	}
}
