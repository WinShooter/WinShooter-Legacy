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
// $Id: CInternetXmlExport.cs 121 2011-05-28 16:02:14Z smuda $
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Allberg.Shooter.Common
{
	class CInternetXmlExport
	{
		internal CInternetXmlExport(Interface callerInterface)
		{
			myInterface = callerInterface;
		}
		Interface myInterface;
		DatabaseDataset database;
		internal byte[] ExportResults()
		{
			database = myInterface.databaseClass.Database;

			MemoryStream memStream = new MemoryStream();
			database.WriteXml(memStream);
			return memStream.ToArray();
		}
	}
}
