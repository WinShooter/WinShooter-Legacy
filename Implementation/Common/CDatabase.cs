#region copyright
/*
Copyright �2009 John Allberg

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
// $Id: CDatabase.cs 121 2011-05-28 16:02:14Z smuda $
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Diagnostics;
using System.Text;
using Allberg.Shooter.WinShooterServerRemoting;
using System.Threading;
using Allberg.Shooter.Common.DataSets;

namespace Allberg.Shooter.Common
{
	/// <summary>
	/// Summary description for Database.
	/// </summary>
	[Serializable]
	internal class CDatabase
	{
		internal CDatabase(Interface callerInterface)
		{
			MyInterface = callerInterface;
		}

		internal const string CurrentDbVersion = "1.6.2";   // This is the version of the database
		private const int MaxNrOfCompetitorsPerShooter = 4; // This is couple to the GUI
		internal Interface MyInterface;
		internal DatabaseDataset Database;
		[NonSerialized]
		internal OleDbConnection Conn;

		// This object is locked each time the database is updated
		private readonly object _databaseLocker = new object();


		#region Database init
		internal void initConnection()
		{
			Trace.WriteLine("CDatabase: Entering initConnection()");

			var testConnection =
				new OleDbConnection(MyInterface.connectionString);
			testConnection.Open();
			testConnection.Close();
		}

		
		private void checkDbForUpgrade(OleDbConnection dbconn)
		{
			var upgrade = new CDatabaseUpgrade(dbconn, this);
			upgrade.Upgrade();
		}

		internal void createAccessDatabase(string PathAndFilename)
		{
			Trace.WriteLine("CDatabase: Entering createAccessDatabase(" +
				PathAndFilename + ")");

			// Check path exists
			if (!Directory.Exists(Path.GetDirectoryName(PathAndFilename)))
			{
				throw new ArgumentException("Path does not exist");
			}

			// If file exists, delete file
			if (File.Exists(PathAndFilename))
				File.Delete(PathAndFilename);

			var cat = new Interop.ADOX.CatalogClass();

			cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Data Source=" + PathAndFilename + ";" +
				"Jet OLEDB:Engine Type=5;");// +
			//"Mode=Share Exclusive");

			cat = null;
			MyInterface.currentFile = PathAndFilename;

			// Ok, done.
			MyInterface.connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Data Source=" + PathAndFilename + ";" +
				"Jet OLEDB:Engine Type=5;";
			//+ "Mode=Share Exclusive";
		}

		internal void OpenAccessDatabase(string pathAndFilename)
		{
			Trace.WriteLine("CDatabase: Entering openAccessDatabase(" + 
				pathAndFilename + ")");

			if (!File.Exists(pathAndFilename))
			{
				throw new ArgumentException("File does not exist");
			}

			MyInterface.currentFile = pathAndFilename;

			// Ok, done.
			MyInterface.connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Data Source=" + pathAndFilename + ";" +
				"Jet OLEDB:Engine Type=5;" +
				"Mode=Share Exclusive";

			Conn = new OleDbConnection(MyInterface.connectionString);

			Thread.Sleep(50);

			Conn.Open();
			checkDbForUpgrade(Conn);
			Conn.Close();
		}

		internal void OpenDatabase()
		{
			Trace.WriteLine("CDatabase: Entering openDatabase()");

			Trace.WriteLine("CDatabase: openDatabase() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId + " )");
			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: openDatabase() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId + " )");

				Database = new DatabaseDataset();
				CreateDataAdapters();

				// Get data into dataset
				Trace.WriteLine("   Reading DbInfo from file.");
				_daDbInfo.Fill(Database, "DbInfo");

				Trace.WriteLine("   Reading Clubs from file.");
				DAClubs.Fill(Database, "Clubs");

				Trace.WriteLine("   Reading Shooters from file.");
				_daShooters.Fill(Database, "Shooters");

				Trace.WriteLine("   Reading Weapons from file.");
				_daWeapons.Fill(Database, "Weapons");

				Trace.WriteLine("   Reading Competition from file.");
				_daCompetition.Fill(Database, "Competition");

				Trace.WriteLine("   Reading patrols from file.");
				_daPatrols.Fill(Database, "Patrols");

				Trace.WriteLine("   Reading Stations from file.");
				_daStations.Fill(Database, "Stations");
			
				Trace.WriteLine("   Reading Competitors from file.");
				_daCompetitors.Fill(Database, "Competitors");
			
				Trace.WriteLine("   Reading CompetitorResults from file.");
				_daCompetitorResults.Fill(Database, "CompetitorResults");

				Trace.WriteLine("   Reading Teams from file.");
				_daTeams.Fill(Database, "Teams");

				// Tell gui data is updated
				Trace.WriteLine("   Done reading from file. Update GUI.");
				MyInterface.updatedClub();
				MyInterface.updatedShooter(new Structs.Shooter());
				MyInterface.updatedWeapon();
				MyInterface.updatedCompetition();
				MyInterface.updatedPatrol();
				MyInterface.updatedStation();
				MyInterface.updatedCompetitor(new Structs.Competitor());
				MyInterface.updatedCompetitorResult(new Structs.CompetitorResult());
				MyInterface.updatedTeam();

				Trace.WriteLine("CDatabase: openDatabase() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId + " )");
			}
		}


		internal void CreateDefaultDatabaseContent()
		{
			Trace.WriteLine("CDatabase: Entering createDefaultDatabaseContent()");
			Trace.WriteLine("CDatabase: createDefaultDatabaseContent() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: createDefaultDatabaseContent() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId + " )");

				OleDbCommand cmd = null;
				try
				{
					Database = new DatabaseDataset();
					Conn = new OleDbConnection(MyInterface.connectionString);
					Conn.Open();


					CreateTables(Database, Conn);
					CreateDatabaseConstraints(Database, Conn);

					cmd = new OleDbCommand(
						"insert into DbInfo (KeyName, KeyValue) values ('Version', '" + CurrentDbVersion + "')",
						Conn);
					cmd.ExecuteNonQuery();

					GetDefaultContent();

					Conn.Close();
				}
				catch (Exception exc)
				{
					Trace.WriteLine(exc.ToString());
					throw;
				}
				finally
				{
					if (cmd != null)
						cmd.Dispose();
				}
				CreateDataAdapters();
				MyInterface.updatedClub();
				MyInterface.updatedWeapon();

				Trace.WriteLine("CDatabase: createDefaultDatabaseContent() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId + " )");
			}
		}
		private void GetDefaultContent()
		{
			Trace.WriteLine("CDatabase: getDefaultContent() " + 
				" starting on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId + " )");

			// Read default content from xml file
			var startup =
				new DSStartupResources();
			startup.ReadXml(
				CEmbeddedResources.GetEmbeddedXmlFile(
					"Allberg.Shooter.Common.DSStartupResources.xml"));

			// Transfer default clubs to current database
			foreach (DSStartupResources.ClubsRow defaultRow in startup.Clubs)
			{
				var newRow = Database.Clubs.NewClubsRow();
				newRow.Automatic = true;
				newRow.ClubId = defaultRow.ClubId;
				newRow.Country  = defaultRow.Country;
				newRow.Name = defaultRow.Name;
				newRow.ToAutomatic = false;
				newRow.Bankgiro = defaultRow.Bankgiro;
				newRow.Plusgiro = defaultRow.Plusgiro;

				if (newRow.IsBankgiroNull())
					newRow.Bankgiro = "";

				if (newRow.IsPlusgiroNull())
					newRow.Plusgiro = "";

				Database.Clubs.AddClubsRow(newRow);
			}

			// Transfer default weapons to current database
			foreach (DSStartupResources.WeaponsRow defaultRow in startup.Weapons)
			{
				var newRow = Database.Weapons.NewWeaponsRow();
				newRow.Automatic = true;
				newRow.Class = defaultRow.Class;
				newRow.Manufacturer = defaultRow.Manufacturer;
				newRow.Model = defaultRow.Model;
				newRow.Caliber = defaultRow.Caliber;
				newRow.ToAutomatic = false;
				newRow.WeaponId = defaultRow.WeaponId;

				Database.Weapons.AddWeaponsRow(newRow);
			}
			Trace.WriteLine("CDatabase: getDefaultContent() " + 
				" ending.");
		}
		private void CreateTables(DatabaseDataset database, OleDbConnection conn)
		{
			Trace.WriteLine("CDatabase: Entering createDatabases()");

			Trace.WriteLine("CDatabase: createTables() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				Thread.CurrentThread.ManagedThreadId + " )");
			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: createTables() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId + " )");

				CreateTable(database.Tables["DbInfo"], conn);
				CreateTable(database.Tables["Clubs"], conn);
				CreateTable(database.Tables["shooters"], conn);
				CreateTable(database.Tables["Weapons"], conn);
				CreateTable(database.Tables["Competition"], conn);
				CreateTable(database.Tables["Patrols"], conn);
				UpdateTablePatrols();
				CreateTable(database.Tables["Competitors"], conn);
				CreateTable(database.Tables["Stations"], conn);
				CreateTable(database.Tables["CompetitorResults"], conn);
				CreateTable(database.Tables["Teams"], conn);

				Trace.WriteLine("CDatabase: createTables() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					Thread.CurrentThread.ManagedThreadId + " )");
			}
		}

		private void UpdateTablePatrols()
		{
			Trace.WriteLine("CDatabase: Entering updateTablePatrols");

			var sqlUpdate = "ALTER TABLE Patrols ADD unique ( PatrolId )";

			// Execute against database
			Trace.WriteLine("CDatabase: Running SQL: " + sqlUpdate);
			var SQL = new OleDbCommand(sqlUpdate,Conn);
			//int result = SQL.ExecuteNonQuery();
			SQL.ExecuteNonQuery();

			Trace.WriteLine("CDatabase: Exiting updateTablePatrols");
		}

		internal static void CreateTable(DataTable table, OleDbConnection dbconn)
		{
			Trace.WriteLine("CDatabase: Entering createTable(" + 
				table.TableName + ")");

			var sqlCreate = new StringBuilder("create table " + table.TableName + "(");
			var columnPrinted = false;
			var primaryKey = true;
			foreach(DataColumn col in table.Columns)
			{
				if (columnPrinted)
					sqlCreate.Append(", ");
				sqlCreate.Append(col.ColumnName + " ");
				switch(col.DataType.ToString())
				{
					case "System.String":
						sqlCreate.Append("varchar(150)");
						break;
					case "System.Integer":
						sqlCreate.Append(primaryKey ? "counter" : "integer");
						break;
					case "System.Int32":
						sqlCreate.Append(primaryKey ? "counter" : "integer");
						break;
					case "System.Boolean":
						sqlCreate.Append("bit");
						break;
					case "System.DateTime":
						sqlCreate.Append("datetime");
						break;
					default:
						throw new CannotFindIdException("Cannot find " + col.DataType);
				}

				sqlCreate.Append(col.AllowDBNull ? " NULL " : " NOT NULL ");

				if (primaryKey)
				{
					sqlCreate.Append(" PRIMARY KEY ");
					primaryKey = false;
				}
				columnPrinted = true;
			}
			sqlCreate.Append(");");

			Trace.WriteLine("CDatabase: Running SQL: " + sqlCreate);

			// Execute against database
			var SQL = new OleDbCommand(sqlCreate.ToString(),dbconn);
			//int result = SQL.ExecuteNonQuery();
			SQL.ExecuteNonQuery();
			SQL.Dispose();
			Trace.WriteLine("CDatabase: Exiting createTable(" + 
				table.TableName + ")");
		}

		private static void CreateDatabaseConstraints(DatabaseDataset database, OleDbConnection conn)
		{
			Trace.WriteLine("CDatabase: Entering createDatabaseConstraints()");

			var sqlCreate = "";

			foreach (DataRelation rel in database.Relations)
			{
				sqlCreate = "ALTER TABLE " + rel.ChildTable.TableName + " ADD " +
					"CONSTRAINT " + rel.RelationName + " FOREIGN KEY " +
					"(" + rel.ChildColumns[0].ColumnName + ")" +
					" REFERENCES " + rel.ParentTable + 
					" (" + rel.ParentColumns[0].ColumnName +
					")";

				// Execute against database
				Trace.WriteLine("CDatabase: Running SQL to create relation: " + sqlCreate);
				var SQL = new OleDbCommand(sqlCreate,conn);

				SQL.ExecuteNonQuery();
			}
		}
		#endregion

		#region Database reading and writing

		private OleDbDataAdapter _daDbInfo;
		private OleDbDataAdapter _daShooters;
		private OleDbDataAdapter _daWeapons;
		private OleDbDataAdapter _daCompetition;
		private OleDbDataAdapter _daPatrols;
		private OleDbDataAdapter _daCompetitors;
		private OleDbDataAdapter _daStations;
		private OleDbDataAdapter _daCompetitorResults;
		private OleDbDataAdapter _daTeams;

		private void CreateDataAdapters()
		{
			Trace.WriteLine("CDatabase: Entering createDatabaseAdapters()");

			CreateDataAdapterDbInfo();
			createDataAdapterClubs();
			createDataAdapterShooters();
			createDataAdapterWeapons();
			createDataAdapterCompetition();
			createDataAdapterPatrols();
			createDataAdapterCompetitors();
			createDataAdapterStations();
			createDataAdapterCompetitorResults();
			createDataAdapterTeams();
		}

		#region DADbInfo
		private OleDbCommand _selectCommandDaDbInfo;
		private OleDbCommand _insertCommandDaDbInfo;
		private OleDbCommand _updateCommandDaDbInfo;
		private OleDbCommand _deleteCommandDaDbInfo;
		private void CreateDataAdapterDbInfo()
		{
			Trace.WriteLine("CDatabase: Entering createDataAdapterClubs()");

			// First create the datadapter
			_daDbInfo = new OleDbDataAdapter();

			// Create all commands
			_selectCommandDaDbInfo = new OleDbCommand();
			_insertCommandDaDbInfo = new OleDbCommand();
			_updateCommandDaDbInfo = new OleDbCommand();
			_deleteCommandDaDbInfo = new OleDbCommand();
			// 
			// selectCommandDADbInfo
			// 
			_selectCommandDaDbInfo.CommandText = "SELECT KeyName, KeyValue FROM DbInfo";
			_selectCommandDaDbInfo.Connection = Conn;
			// 
			// insertCommandDADbInfo
			// 
			_insertCommandDaDbInfo.CommandText = "INSERT INTO DbInfo(KeyName, KeyValue) VALUES (?, ?)";
			_insertCommandDaDbInfo.Connection = Conn;
			_insertCommandDaDbInfo.Parameters.Add(new OleDbParameter("KeyName", OleDbType.VarWChar, 150, "KeyName"));
			_insertCommandDaDbInfo.Parameters.Add(new OleDbParameter("KeyValue", OleDbType.VarWChar, 150, "KeyValue"));
			// 
			// updateCommandDADbInfo
			// 
			_updateCommandDaDbInfo.CommandText = "UPDATE DbInfo SET KeyName = ?, KeyValue = ? WHERE (KeyName = ?) AND (KeyValue = ?" +
				" OR ? IS NULL AND KeyValue IS NULL)";
			_updateCommandDaDbInfo.Connection = Conn;
			_updateCommandDaDbInfo.Parameters.Add(new OleDbParameter("KeyName", OleDbType.VarWChar, 150, "KeyName"));
			_updateCommandDaDbInfo.Parameters.Add(new OleDbParameter("KeyValue", OleDbType.VarWChar, 150, "KeyValue"));
			_updateCommandDaDbInfo.Parameters.Add(new OleDbParameter("Original_KeyName", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "KeyName", System.Data.DataRowVersion.Original, null));
			_updateCommandDaDbInfo.Parameters.Add(new OleDbParameter("Original_KeyValue", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "KeyValue", System.Data.DataRowVersion.Original, null));
			_updateCommandDaDbInfo.Parameters.Add(new OleDbParameter("Original_KeyValue1", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "KeyValue", System.Data.DataRowVersion.Original, null));
			// 
			// deleteCommandDADbInfo
			// 
			_deleteCommandDaDbInfo.CommandText = "DELETE FROM DbInfo WHERE (KeyName = ?) AND (KeyValue = ? OR ? IS NULL AND KeyValu" +
				"e IS NULL)";
			_deleteCommandDaDbInfo.Connection = Conn;
			_deleteCommandDaDbInfo.Parameters.Add(new OleDbParameter("Original_KeyName", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "KeyName", System.Data.DataRowVersion.Original, null));
			_deleteCommandDaDbInfo.Parameters.Add(new OleDbParameter("Original_KeyValue", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "KeyValue", System.Data.DataRowVersion.Original, null));
			_deleteCommandDaDbInfo.Parameters.Add(new OleDbParameter("Original_KeyValue1", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "KeyValue", System.Data.DataRowVersion.Original, null));
			// 
			// oleDbDataAdapter1
			// 
			_daDbInfo.DeleteCommand = _deleteCommandDaDbInfo;
			_daDbInfo.InsertCommand = _insertCommandDaDbInfo;
			_daDbInfo.SelectCommand = _selectCommandDaDbInfo;
			_daDbInfo.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "DbInfo", new System.Data.Common.DataColumnMapping[] {
																																																				  new System.Data.Common.DataColumnMapping("KeyName", "KeyName"),
																																																				  new System.Data.Common.DataColumnMapping("KeyValue", "KeyValue")})});
			_daDbInfo.UpdateCommand = _updateCommandDaDbInfo;
		}
		#endregion

		#region Clubs
		private OleDbDataAdapter DAClubs;
		private OleDbCommand selectCommandClubs;
		private OleDbCommand insertCommandClubs;
		private OleDbCommand updateCommandClubs;
		private OleDbCommand deleteCommandClubs;
		private void createDataAdapterClubs()
		{
			Trace.WriteLine("CDatabase: Entering createDataAdapterClubs()");

			// First create the datadapter
			DAClubs = new OleDbDataAdapter();

			// Create all commands
			selectCommandClubs = new OleDbCommand();
			insertCommandClubs = new OleDbCommand();
			updateCommandClubs = new OleDbCommand();
			deleteCommandClubs = new OleDbCommand();

			selectCommandClubs.CommandText = "SELECT Automatic, ClubId, Country, Name, ToAutomatic, LastUpdate FROM Clubs";
			selectCommandClubs.Connection = Conn;
			// 
			// insertCommandClubs
			// 
			insertCommandClubs.CommandText = "INSERT INTO Clubs(Automatic, ClubId, Country, Name, ToAutomatic, LastUpdate) VALUES (?, ?, ?," +
				" ?, ?, ?)";
			insertCommandClubs.Connection = Conn;
			insertCommandClubs.Parameters.Add(new OleDbParameter("Automatic", OleDbType.Boolean, 2, "Automatic"));
			insertCommandClubs.Parameters.Add(new OleDbParameter("ClubId", OleDbType.VarWChar, 150, "ClubId"));
			insertCommandClubs.Parameters.Add(new OleDbParameter("Country", OleDbType.VarWChar, 150, "Country"));
			insertCommandClubs.Parameters.Add(new OleDbParameter("Name", OleDbType.VarWChar, 150, "Name"));
			insertCommandClubs.Parameters.Add(new OleDbParameter("ToAutomatic", OleDbType.Boolean, 2, "ToAutomatic"));
			insertCommandClubs.Parameters.Add(new OleDbParameter("LastUpdate", OleDbType.Date, 2, "LastUpdate"));
			// 
			// updateCommandClubs
			// 
			updateCommandClubs.CommandText = "UPDATE Clubs SET Automatic = ?, Country = ?, Name = ?, ToAutomatic = " +
				"?, LastUpdate = ? WHERE (ClubId = ?)";
			//updateCommandClubs.CommandText = "UPDATE Clubs SET Automatic = ?, ClubId = ?, Country = ?, Name = ?, ToAutomatic = " +
			//"? WHERE (ClubId = ?) AND (Automatic = ?) AND (Country = ? OR ? IS NULL AND Count" +
				//"ry IS NULL) AND (Name = ? OR ? IS NULL AND Name IS NULL) AND (ToAutomatic = ?)";
			updateCommandClubs.Connection = Conn;
			updateCommandClubs.Parameters.Add(new OleDbParameter("Automatic", OleDbType.Boolean, 2, "Automatic"));
			updateCommandClubs.Parameters.Add(new OleDbParameter("Country", OleDbType.VarWChar, 150, "Country"));
			updateCommandClubs.Parameters.Add(new OleDbParameter("Name", OleDbType.VarWChar, 150, "Name"));
			updateCommandClubs.Parameters.Add(new OleDbParameter("ToAutomatic", OleDbType.Boolean, 2, "ToAutomatic"));
			updateCommandClubs.Parameters.Add(new OleDbParameter("LastUpdate", OleDbType.Date, 2, "LastUpdate"));
			updateCommandClubs.Parameters.Add(new OleDbParameter("Original_ClubId", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null));
			// 
			// deleteCommandClubs
			// 
			deleteCommandClubs.CommandText = "DELETE FROM Clubs WHERE (ClubId = ?) AND (Automatic = ?) AND (Country = ? OR ? IS" +
				" NULL AND Country IS NULL) AND (Name = ? OR ? IS NULL AND Name IS NULL) AND (ToA" +
				"utomatic = ?) AND (LastUpdate = ?)";
			deleteCommandClubs.Connection = Conn;
			deleteCommandClubs.Parameters.Add(new OleDbParameter("Original_ClubId", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null));
			deleteCommandClubs.Parameters.Add(new OleDbParameter("Original_Automatic", OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Automatic", System.Data.DataRowVersion.Original, null));
			deleteCommandClubs.Parameters.Add(new OleDbParameter("Original_Country", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Country", System.Data.DataRowVersion.Original, null));
			deleteCommandClubs.Parameters.Add(new OleDbParameter("Original_Country1", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Country", System.Data.DataRowVersion.Original, null));
			deleteCommandClubs.Parameters.Add(new OleDbParameter("Original_Name", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name", System.Data.DataRowVersion.Original, null));
			deleteCommandClubs.Parameters.Add(new OleDbParameter("Original_Name1", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name", System.Data.DataRowVersion.Original, null));
			deleteCommandClubs.Parameters.Add(new OleDbParameter("Original_ToAutomatic", OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ToAutomatic", System.Data.DataRowVersion.Original, null));
			deleteCommandClubs.Parameters.Add(new OleDbParameter("Original_LastUpdate", OleDbType.Date, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "LastUpdate", System.Data.DataRowVersion.Original, null));
			// 
			// oleDbDataAdapter1
			// 
			DAClubs.DeleteCommand = deleteCommandClubs;
			DAClubs.InsertCommand = insertCommandClubs;
			DAClubs.SelectCommand = selectCommandClubs;
			DAClubs.UpdateCommand = updateCommandClubs;
			DAClubs.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "Clubs", new System.Data.Common.DataColumnMapping[] {
																																																				 new System.Data.Common.DataColumnMapping("Automatic", "Automatic"),
																																																				 new System.Data.Common.DataColumnMapping("ClubId", "ClubId"),
																																																				 new System.Data.Common.DataColumnMapping("Country", "Country"),
																																																				 new System.Data.Common.DataColumnMapping("Name", "Name"),
																																																				 new System.Data.Common.DataColumnMapping("ToAutomatic", "ToAutomatic"),
																																																				 new System.Data.Common.DataColumnMapping("LastUpdate", "LastUpdate")})});
		}
		#endregion

		#region Shooters
		private OleDbCommand selectCommandShooters;
		private OleDbCommand insertCommandShooters;
		private OleDbCommand updateCommandShooters;
		private OleDbCommand deleteCommandShooters;
		private OleDbCommand identityCommandShooters;
		private void createDataAdapterShooters()
		{
			Trace.WriteLine("CDatabase: Entering createDataAdapterShooters()");

			// Create dataadapter
			_daShooters = new OleDbDataAdapter();

			// Create commands
			selectCommandShooters = new OleDbCommand();
			insertCommandShooters = new OleDbCommand();
			updateCommandShooters = new OleDbCommand();
			deleteCommandShooters = new OleDbCommand();
			identityCommandShooters = new OleDbCommand();
			// 
			// selectCommandShooters
			// 
			selectCommandShooters.CommandText = "SELECT Automatic, Cardnr, Class, ClubId, Email, Givenname, Payed, ShooterId, Surn" +
				"ame, ToAutomatic, Arrived, LastUpdate FROM Shooters";
			selectCommandShooters.Connection = Conn;
			// 
			// insertCommandShooters
			// 
			insertCommandShooters.CommandText = "INSERT INTO Shooters(Automatic, Cardnr, Class, ClubId, Email, Givenname, Payed, S" +
				"urname, ToAutomatic, Arrived, LastUpdate) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
			insertCommandShooters.Connection = Conn;
			insertCommandShooters.Parameters.Add(new OleDbParameter("Automatic", OleDbType.Boolean, 2, "Automatic"));
			insertCommandShooters.Parameters.Add(new OleDbParameter("Cardnr", OleDbType.VarWChar, 150, "Cardnr"));
			insertCommandShooters.Parameters.Add(new OleDbParameter("Class", OleDbType.Integer, 0, "Class"));
			insertCommandShooters.Parameters.Add(new OleDbParameter("ClubId", OleDbType.VarWChar, 150, "ClubId"));
			insertCommandShooters.Parameters.Add(new OleDbParameter("Email", OleDbType.VarWChar, 150, "Email"));
			insertCommandShooters.Parameters.Add(new OleDbParameter("Givenname", OleDbType.VarWChar, 150, "Givenname"));
			insertCommandShooters.Parameters.Add(new OleDbParameter("Payed", OleDbType.Integer, 0, "Payed"));
			insertCommandShooters.Parameters.Add(new OleDbParameter("Surname", OleDbType.VarWChar, 150, "Surname"));
			insertCommandShooters.Parameters.Add(new OleDbParameter("ToAutomatic", OleDbType.Boolean, 2, "ToAutomatic"));
			insertCommandShooters.Parameters.Add(new OleDbParameter("Arrived", OleDbType.Boolean, 2, "Arrived"));
			insertCommandShooters.Parameters.Add(new OleDbParameter("LastUpdate", OleDbType.Date, 2, "LastUpdate"));
			// 
			// updateCommandShooters
			// 
			updateCommandShooters.CommandText = @"UPDATE Shooters SET Automatic = ?, Cardnr = ?, Class = ?, ClubId = ?, Email = ?, Givenname = ?, Payed = ?, Surname = ?, ToAutomatic = ?, Arrived = ?, LastUpdate = ? "+
				"WHERE ShooterId = ?";
			updateCommandShooters.Connection = Conn;
			updateCommandShooters.Parameters.Add(new OleDbParameter("Automatic", OleDbType.Boolean, 2, "Automatic"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("Cardnr", OleDbType.VarWChar, 150, "Cardnr"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("Class", OleDbType.Integer, 0, "Class"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("ClubId", OleDbType.VarWChar, 150, "ClubId"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("Email", OleDbType.VarWChar, 150, "Email"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("Givenname", OleDbType.VarWChar, 150, "Givenname"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("Payed", OleDbType.Integer, 0, "Payed"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("Surname", OleDbType.VarWChar, 150, "Surname"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("ToAutomatic", OleDbType.Boolean, 2, "ToAutomatic"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("Arrived", OleDbType.Boolean, 2, "Arrived"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("LastUpdate", OleDbType.Date, 2, "LastUpdate"));
			updateCommandShooters.Parameters.Add(new OleDbParameter("Original_ShooterId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ShooterId", System.Data.DataRowVersion.Original, null));
			// 
			// deleteCommandShooters
			// 
			deleteCommandShooters.CommandText = @"DELETE FROM Shooters WHERE (ShooterId = ?)";
			deleteCommandShooters.Connection = Conn;
			deleteCommandShooters.Parameters.Add(new OleDbParameter("Original_ShooterId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ShooterId", System.Data.DataRowVersion.Original, null));
			//
			// identityCommandShooters
			//
			identityCommandShooters.CommandText = "SELECT @@IDENTITY";
			identityCommandShooters.Connection = Conn;
			// 
			// oleDbDataAdapter1
			// 
			_daShooters.DeleteCommand = deleteCommandShooters;
			_daShooters.InsertCommand = insertCommandShooters;
			_daShooters.SelectCommand = selectCommandShooters;
			_daShooters.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "Shooters", new System.Data.Common.DataColumnMapping[] {
																																																					new System.Data.Common.DataColumnMapping("Automatic", "Automatic"),
																																																					new System.Data.Common.DataColumnMapping("Cardnr", "Cardnr"),
																																																					new System.Data.Common.DataColumnMapping("Class", "Class"),
																																																					new System.Data.Common.DataColumnMapping("ClubId", "ClubId"),
																																																					new System.Data.Common.DataColumnMapping("Email", "Email"),
																																																					new System.Data.Common.DataColumnMapping("Givenname", "Givenname"),
																																																					new System.Data.Common.DataColumnMapping("Payed", "Payed"),
																																																					new System.Data.Common.DataColumnMapping("ShooterId", "ShooterId"),
																																																					new System.Data.Common.DataColumnMapping("Surname", "Surname"),
																																																					new System.Data.Common.DataColumnMapping("ToAutomatic", "ToAutomatic"),
																																																					new System.Data.Common.DataColumnMapping("Arrived", "Arrived"),
																																																					new System.Data.Common.DataColumnMapping("LastUpdate", "LastUpdate")})});
			_daShooters.UpdateCommand = updateCommandShooters;

			// Handle fetching identity of new inserted rows.
			_daShooters.RowUpdated += new OleDbRowUpdatedEventHandler(DAShooters_RowUpdated);

		}
		private void DAShooters_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
		{
			if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert )
			{
				// Get the Identity column value
				int identity = Int32.Parse(identityCommandShooters.ExecuteScalar().ToString());
				e.Row["ShooterId"] = identity;
				e.Row.AcceptChanges();
			}
		}

		#endregion

		#region Weapons
		private OleDbCommand selectCommandWeapons;
		private OleDbCommand insertCommandWeapons;
		private OleDbCommand updateCommandWeapons;
		private OleDbCommand deleteCommandWeapons;
		private void createDataAdapterWeapons()
		{
			Trace.WriteLine("CDatabase: Entering createDataAdapterWeapons()");

			// Create DataAdapter
			_daWeapons = new OleDbDataAdapter();

			// Create commands
			selectCommandWeapons = new OleDbCommand();
			insertCommandWeapons = new OleDbCommand();
			updateCommandWeapons = new OleDbCommand();
			deleteCommandWeapons = new OleDbCommand();
			// 
			// selectCommandWeapons
			// 
			selectCommandWeapons.CommandText = "SELECT Automatic, Caliber, Class, Manufacturer, Model, ToAutomatic, WeaponId, LastUpdate FROM" +
				" Weapons";
			selectCommandWeapons.Connection = Conn;
			// 
			// insertCommandWeapons
			// 
			insertCommandWeapons.CommandText = "INSERT INTO Weapons(Automatic, Caliber, Class, Manufacturer, Model, ToAutomatic, " +
				"WeaponId, LastUpdate) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
			insertCommandWeapons.Connection = Conn;
			insertCommandWeapons.Parameters.Add(new OleDbParameter("Automatic", OleDbType.Boolean, 2, "Automatic"));
			insertCommandWeapons.Parameters.Add(new OleDbParameter("Caliber", OleDbType.VarWChar, 150, "Caliber"));
			insertCommandWeapons.Parameters.Add(new OleDbParameter("Class", OleDbType.Integer, 0, "Class"));
			insertCommandWeapons.Parameters.Add(new OleDbParameter("Manufacturer", OleDbType.VarWChar, 150, "Manufacturer"));
			insertCommandWeapons.Parameters.Add(new OleDbParameter("Model", OleDbType.VarWChar, 150, "Model"));
			insertCommandWeapons.Parameters.Add(new OleDbParameter("ToAutomatic", OleDbType.Boolean, 2, "ToAutomatic"));
			insertCommandWeapons.Parameters.Add(new OleDbParameter("WeaponId", OleDbType.VarWChar, 150, "WeaponId"));
			insertCommandWeapons.Parameters.Add(new OleDbParameter("LastUpdate", OleDbType.Date, 150, "LastUpdate"));
			// 
			// updateCommandWeapons
			// 
			updateCommandWeapons.CommandText = @"UPDATE Weapons SET Automatic = ?, Caliber = ?, Class = ?, Manufacturer = ?, Model = ?, ToAutomatic = ?, LastUpdate = ? " + 
				"WHERE WeaponId = ?";
			updateCommandWeapons.Connection = Conn;
			updateCommandWeapons.Parameters.Add(new OleDbParameter("Automatic", OleDbType.Boolean, 2, "Automatic"));
			updateCommandWeapons.Parameters.Add(new OleDbParameter("Caliber", OleDbType.VarWChar, 150, "Caliber"));
			updateCommandWeapons.Parameters.Add(new OleDbParameter("Class", OleDbType.Integer, 0, "Class"));
			updateCommandWeapons.Parameters.Add(new OleDbParameter("Manufacturer", OleDbType.VarWChar, 150, "Manufacturer"));
			updateCommandWeapons.Parameters.Add(new OleDbParameter("Model", OleDbType.VarWChar, 150, "Model"));
			updateCommandWeapons.Parameters.Add(new OleDbParameter("ToAutomatic", OleDbType.Boolean, 2, "ToAutomatic"));
			//updateCommandWeapons.Parameters.Add(new OleDbParameter("WeaponId", OleDbType.VarWChar, 150, "WeaponId"));
			updateCommandWeapons.Parameters.Add(new OleDbParameter("LastUpdate", OleDbType.Date, 150, "LastUpdate"));
			updateCommandWeapons.Parameters.Add(new OleDbParameter("Original_WeaponId", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "WeaponId", System.Data.DataRowVersion.Original, null));
			// 
			// deleteCommandWeapons
			// 
			deleteCommandWeapons.CommandText = @"DELETE FROM Weapons WHERE (WeaponId = ?)";
			deleteCommandWeapons.Connection = Conn;
			deleteCommandWeapons.Parameters.Add(new OleDbParameter("Original_WeaponId", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "WeaponId", System.Data.DataRowVersion.Original, null));
			// 
			// DAWeapons
			// 
			_daWeapons.DeleteCommand = deleteCommandWeapons;
			_daWeapons.InsertCommand = insertCommandWeapons;
			_daWeapons.SelectCommand = selectCommandWeapons;
			_daWeapons.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "Weapons", new System.Data.Common.DataColumnMapping[] {
																																																				   new System.Data.Common.DataColumnMapping("Automatic", "Automatic"),
																																																				   new System.Data.Common.DataColumnMapping("Caliber", "Caliber"),
																																																				   new System.Data.Common.DataColumnMapping("Class", "Class"),
																																																				   new System.Data.Common.DataColumnMapping("Manufacturer", "Manufacturer"),
																																																				   new System.Data.Common.DataColumnMapping("Model", "Model"),
																																																				   new System.Data.Common.DataColumnMapping("ToAutomatic", "ToAutomatic"),
																																																				   new System.Data.Common.DataColumnMapping("WeaponId", "WeaponId"),
																																																				   new System.Data.Common.DataColumnMapping("LastUpdate", "LastUpdate")})});
			_daWeapons.UpdateCommand = updateCommandWeapons;
		}
		#endregion

		#region Competition
		private OleDbCommand selectCommandCompetition;
		private OleDbCommand insertCommandCompetition;
		private OleDbCommand updateCommandCompetition;
		private OleDbCommand deleteCommandCompetition;
		private OleDbCommand identityCommandCompetition;
		private void createDataAdapterCompetition()
		{
			Trace.WriteLine("CDatabase: Entering createDataAdapterCompetition()");

			// Create the adapter
			_daCompetition = new OleDbDataAdapter();

			// Create the commands
			selectCommandCompetition = new OleDbCommand();
			insertCommandCompetition = new OleDbCommand();
			updateCommandCompetition = new OleDbCommand();
			deleteCommandCompetition = new OleDbCommand();
			identityCommandCompetition = new OleDbCommand();
			// 
			// selectCommandCompetition
			// 
			selectCommandCompetition.CommandText = "SELECT CompetitionId, DoFinalShooting, FirstPrice, Name, NorwegianCount, PatrolSi" +
				"ze, PatrolTime, PatrolTimeBetween, PatrolTimeRest, PriceMoneyPercentToReturn, Pr" +
				"iceMoneyShooterPercent, ShooterFee1, ShooterFee2, ShooterFee3, ShooterFee4, StartDate, StartTime, UsePriceMoney, Type, Championship, PatrolConnectionType, OneClass FROM Com" +
				"petition";
			selectCommandCompetition.Connection = Conn;
			// 
			// insertCommandCompetition
			// 
			insertCommandCompetition.CommandText = @"INSERT INTO Competition(DoFinalShooting, FirstPrice, Name, NorwegianCount, PatrolSize, PatrolTime, PatrolTimeBetween, PatrolTimeRest, PriceMoneyPercentToReturn, PriceMoneyShooterPercent, ShooterFee1, ShooterFee2, ShooterFee3, ShooterFee4, StartDate, StartTime, UsePriceMoney, Type, Championship, PatrolConnectionType, OneClass) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
			insertCommandCompetition.Connection = Conn;
			insertCommandCompetition.Parameters.Add(new OleDbParameter("DoFinalShooting", OleDbType.Boolean, 2, "DoFinalShooting"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("FirstPrice", OleDbType.Integer, 0, "FirstPrice"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("Name", OleDbType.VarWChar, 150, "Name"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("NorwegianCount", OleDbType.Boolean, 2, "NorwegianCount"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("PatrolSize", OleDbType.Integer, 0, "PatrolSize"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("PatrolTime", OleDbType.Integer, 0, "PatrolTime"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("PatrolTimeBetween", OleDbType.Integer, 0, "PatrolTimeBetween"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("PatrolTimeRest", OleDbType.Integer, 0, "PatrolTimeRest"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("PriceMoneyPercentToReturn", OleDbType.Integer, 0, "PriceMoneyPercentToReturn"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("PriceMoneyShooterPercent", OleDbType.Integer, 0, "PriceMoneyShooterPercent"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("ShooterFee1", OleDbType.Integer, 0, "ShooterFee1"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("ShooterFee2", OleDbType.Integer, 0, "ShooterFee2"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("ShooterFee3", OleDbType.Integer, 0, "ShooterFee3"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("ShooterFee4", OleDbType.Integer, 0, "ShooterFee4"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("StartDate", OleDbType.DBDate, 0, "StartDate"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("StartTime", OleDbType.Integer, 0, "StartTime"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("UsePriceMoney", OleDbType.Boolean, 2, "UsePriceMoney"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("Type", OleDbType.Integer, 0, "Type"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("Championship", OleDbType.Integer, 0, "Championship"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("PatrolConnectionType", OleDbType.Integer, 0, "PatrolConnectionType"));
			insertCommandCompetition.Parameters.Add(new OleDbParameter("OneClass", OleDbType.Boolean, 0, "OneClass"));
			// 
			// updateCommandCompetition
			// 
			updateCommandCompetition.CommandText = @"UPDATE Competition SET DoFinalShooting = ?, FirstPrice = ?, Name = ?, NorwegianCount = ?, PatrolSize = ?, PatrolTime = ?, PatrolTimeBetween = ?, PatrolTimeRest = ?, PriceMoneyPercentToReturn = ?, PriceMoneyShooterPercent = ?, ShooterFee1 = ?, ShooterFee2 = ?, ShooterFee3 = ?, ShooterFee4 = ?, StartDate = ?, StartTime = ?, UsePriceMoney = ?, Type = ?, Championship = ?, PatrolConnectionType = ?, OneClass = ? WHERE (CompetitionId = ?)";
			updateCommandCompetition.Connection = Conn;
			updateCommandCompetition.Parameters.Add(new OleDbParameter("DoFinalShooting", OleDbType.Boolean, 2, "DoFinalShooting"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("FirstPrice", OleDbType.Integer, 0, "FirstPrice"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("Name", OleDbType.VarWChar, 150, "Name"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("NorwegianCount", OleDbType.Boolean, 2, "NorwegianCount"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("PatrolSize", OleDbType.Integer, 0, "PatrolSize"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("PatrolTime", OleDbType.Integer, 0, "PatrolTime"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("PatrolTimeBetween", OleDbType.Integer, 0, "PatrolTimeBetween"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("PatrolTimeRest", OleDbType.Integer, 0, "PatrolTimeRest"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("PriceMoneyPercentToReturn", OleDbType.Integer, 0, "PriceMoneyPercentToReturn"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("PriceMoneyShooterPercent", OleDbType.Integer, 0, "PriceMoneyShooterPercent"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("ShooterFee1", OleDbType.Integer, 0, "ShooterFee1"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("ShooterFee2", OleDbType.Integer, 0, "ShooterFee2"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("ShooterFee3", OleDbType.Integer, 0, "ShooterFee3"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("ShooterFee4", OleDbType.Integer, 0, "ShooterFee4"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("StartDate", OleDbType.DBDate, 0, "StartDate"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("StartTime", OleDbType.Integer, 0, "StartTime"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("UsePriceMoney", OleDbType.Boolean, 2, "UsePriceMoney"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("Type", OleDbType.Integer, 0, "Type"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("Championship", OleDbType.Integer, 0, "Championship"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("PatrolConnectionType", OleDbType.Integer, 0, "PatrolConnectionType"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("OneClass", OleDbType.Boolean, 0, "OneClass"));
			updateCommandCompetition.Parameters.Add(new OleDbParameter("Original_CompetitionId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			// 
			// deleteCommandCompetition
			// 
			deleteCommandCompetition.CommandText = @"DELETE FROM Competition WHERE (CompetitionId = ?)";
			deleteCommandCompetition.Connection = Conn;
			deleteCommandCompetition.Parameters.Add(new OleDbParameter("Original_CompetitionId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			//
			// identityCommandCompetition
			//
			identityCommandCompetition.CommandText = "SELECT @@IDENTITY";
			identityCommandCompetition.Connection = Conn;
			// 
			// oleDbDataAdapter1
			// 
			_daCompetition.DeleteCommand = deleteCommandCompetition;
			_daCompetition.InsertCommand = insertCommandCompetition;
			_daCompetition.SelectCommand = selectCommandCompetition;
			_daCompetition.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "Competition", new System.Data.Common.DataColumnMapping[] {
																																																					   new System.Data.Common.DataColumnMapping("CompetitionId", "CompetitionId"),
																																																					   new System.Data.Common.DataColumnMapping("DoFinalShooting", "DoFinalShooting"),
																																																					   new System.Data.Common.DataColumnMapping("FirstPrice", "FirstPrice"),
																																																					   new System.Data.Common.DataColumnMapping("Name", "Name"),
																																																					   new System.Data.Common.DataColumnMapping("NorwegianCount", "NorwegianCount"),
																																																					   new System.Data.Common.DataColumnMapping("PatrolSize", "PatrolSize"),
																																																					   new System.Data.Common.DataColumnMapping("PatrolTime", "PatrolTime"),
																																																					   new System.Data.Common.DataColumnMapping("PatrolTimeBetween", "PatrolTimeBetween"),
																																																					   new System.Data.Common.DataColumnMapping("PatrolTimeRest", "PatrolTimeRest"),
																																																					   new System.Data.Common.DataColumnMapping("PriceMoneyPercentToReturn", "PriceMoneyPercentToReturn"),
																																																					   new System.Data.Common.DataColumnMapping("PriceMoneyShooterPercent", "PriceMoneyShooterPercent"),
																																																					   new System.Data.Common.DataColumnMapping("ShooterFee1", "ShooterFee1"),
																																																					   new System.Data.Common.DataColumnMapping("ShooterFee2", "ShooterFee2"),
																																																					   new System.Data.Common.DataColumnMapping("ShooterFee3", "ShooterFee3"),
																																																					   new System.Data.Common.DataColumnMapping("ShooterFee4", "ShooterFee4"),
																																																					   new System.Data.Common.DataColumnMapping("StartDate", "StartDate"),
																																																					   new System.Data.Common.DataColumnMapping("StartTime", "StartTime"),
																																																					   new System.Data.Common.DataColumnMapping("UsePriceMoney", "UsePriceMoney"),
																																																					   new System.Data.Common.DataColumnMapping("Type", "Type"),
																																																					   new System.Data.Common.DataColumnMapping("Championship", "Championship"),
																																																					   new System.Data.Common.DataColumnMapping("OneClass", "OneClass")})});
			_daCompetition.UpdateCommand = updateCommandCompetition;

			// Handle fetching identity of new inserted rows.
			_daCompetition.RowUpdated += new OleDbRowUpdatedEventHandler(DACompetition_RowUpdated);
		}
		private void DACompetition_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
		{
			if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert )
			{
				// Get the Identity column value
				int identity = Int32.Parse(identityCommandCompetition.ExecuteScalar().ToString());
				e.Row["CompetitionId"] = identity;
				e.Row.AcceptChanges();
			}
		}
		#endregion

		#region Patrols
		private OleDbCommand selectCommandPatrols;
		private OleDbCommand insertCommandPatrols;
		private OleDbCommand updateCommandPatrols;
		private OleDbCommand deleteCommandPatrols;
		private OleDbCommand identityCommandPatrols;

		private void createDataAdapterPatrols()
		{
			Trace.WriteLine("CDatabase: Entering createDataAdapterPatrols()");

			// Create adapter
			_daPatrols = new OleDbDataAdapter();

			// Create commands
			selectCommandPatrols = new OleDbCommand();
			insertCommandPatrols = new OleDbCommand();
			updateCommandPatrols = new OleDbCommand();
			deleteCommandPatrols = new OleDbCommand();
			identityCommandPatrols = new OleDbCommand();
			// 
			// selectCommandPatrols
			// 
			selectCommandPatrols.CommandText = "SELECT Automatic, CompetitionId, PatrolId, PClass, StartDateTime, PatrolDbId, StartDateTimeDisplay FROM Patrols";
			selectCommandPatrols.Connection = Conn;
			// 
			// oleDbInsertCommand1
			// 
			insertCommandPatrols.CommandText = "INSERT INTO Patrols(Automatic, CompetitionId, PClass, StartDateTime, PatrolId, StartDateTimeDisplay) VALUES (?, ?" +
				", ?, ?, ?, ?)";
			insertCommandPatrols.Connection = Conn;
			insertCommandPatrols.Parameters.Add(new OleDbParameter("Automatic", OleDbType.Boolean, 2, "Automatic"));
			insertCommandPatrols.Parameters.Add(new OleDbParameter("CompetitionId", OleDbType.Integer, 0, "CompetitionId"));
			insertCommandPatrols.Parameters.Add(new OleDbParameter("PClass", OleDbType.Integer, 0, "PClass"));
			insertCommandPatrols.Parameters.Add(new OleDbParameter("StartDateTime", OleDbType.Integer, 0, "StartDateTime"));
			insertCommandPatrols.Parameters.Add(new OleDbParameter("PatrolId", OleDbType.Integer, 0, "PatrolId"));
			insertCommandPatrols.Parameters.Add(new OleDbParameter("StartDateTimeDisplay", OleDbType.Integer, 0, "StartDateTimeDisplay"));
			// 
			// updateCommandPatrols
			// 
			/*updateCommandPatrols.CommandText = @"UPDATE Patrols SET Automatic = ?, CompetitionId = ?, PClass = ?, StartDateTime = ?, PatrolId = ? WHERE (PatrolId = ?) AND (Automatic = ?) AND (CompetitionId = ? OR ? IS NULL AND CompetitionId IS NULL) AND (PClass = ? OR ? IS NULL AND PClass IS NULL) AND (StartDateTime = ? OR ? IS NULL AND StartDateTime IS NULL) AND (PatrolDbId = ?)";
			updateCommandPatrols.Connection = conn;
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Automatic", OleDbType.Boolean, 2, "Automatic"));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("CompetitionId", OleDbType.Integer, 0, "CompetitionId"));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("PClass", OleDbType.Integer, 0, "PClass"));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("StartDateTime", OleDbType.Integer, 0, "StartDateTime"));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("PatrolId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolId", System.Data.DataRowVersion.Original, null));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_PatrolId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolId", System.Data.DataRowVersion.Original, null));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_Automatic", OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Automatic", System.Data.DataRowVersion.Original, null));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_CompetitionId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_CompetitionId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_PClass", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PClass", System.Data.DataRowVersion.Original, null));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_PClass1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PClass", System.Data.DataRowVersion.Original, null));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_StartDateTime", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StartDateTime", System.Data.DataRowVersion.Original, null));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_StartDateTime1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StartDateTime", System.Data.DataRowVersion.Original, null));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_PatrolDbId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolDbId", System.Data.DataRowVersion.Original, null));*/
			updateCommandPatrols.CommandText = @"UPDATE Patrols SET Automatic = ?, CompetitionId = ?, PClass = ?, StartDateTime = ?, StartDateTimeDisplay = ? WHERE (PatrolDbId = ?) AND (PatrolId = ?)";
			updateCommandPatrols.Connection = Conn;
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Automatic", OleDbType.Boolean, 2, "Automatic"));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("CompetitionId", OleDbType.Integer, 0, "CompetitionId"));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("PClass", OleDbType.Integer, 0, "PClass"));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("StartDateTime", OleDbType.Integer, 0, "StartDateTime"));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("StartDateTimeDisplay", OleDbType.Integer, 0, "StartDateTimeDisplay"));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_PatrolDbId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolDbId", System.Data.DataRowVersion.Original, null));
			updateCommandPatrols.Parameters.Add(new OleDbParameter("Original_PatrolId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolId", System.Data.DataRowVersion.Original, null));
			// 
			// deleteCommandPatrols
			// 
			deleteCommandPatrols.CommandText = "DELETE FROM Patrols WHERE (PatrolId = ?) AND (PatrolDbId = ?) AND (Automatic = ?) AND (CompetitionId =" +
				" ? OR ? IS NULL AND CompetitionId IS NULL) AND (PClass = ? OR ? IS NULL AND PCla" +
				"ss IS NULL) AND (StartDateTime = ? OR ? IS NULL AND StartDateTime IS NULL)";
			deleteCommandPatrols.Connection = Conn;
			deleteCommandPatrols.Parameters.Add(new OleDbParameter("Original_PatrolId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolId", System.Data.DataRowVersion.Original, null));
			deleteCommandPatrols.Parameters.Add(new OleDbParameter("Original_PatrolDbId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolDbId", System.Data.DataRowVersion.Original, null));
			deleteCommandPatrols.Parameters.Add(new OleDbParameter("Original_Automatic", OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Automatic", System.Data.DataRowVersion.Original, null));
			deleteCommandPatrols.Parameters.Add(new OleDbParameter("Original_CompetitionId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			deleteCommandPatrols.Parameters.Add(new OleDbParameter("Original_CompetitionId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			deleteCommandPatrols.Parameters.Add(new OleDbParameter("Original_PClass", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PClass", System.Data.DataRowVersion.Original, null));
			deleteCommandPatrols.Parameters.Add(new OleDbParameter("Original_PClass1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PClass", System.Data.DataRowVersion.Original, null));
			deleteCommandPatrols.Parameters.Add(new OleDbParameter("Original_StartDateTime", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StartDateTime", System.Data.DataRowVersion.Original, null));
			deleteCommandPatrols.Parameters.Add(new OleDbParameter("Original_StartDateTime1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StartDateTime", System.Data.DataRowVersion.Original, null));
			//
			// identityCommandPatrols
			//
			identityCommandPatrols.CommandText = "SELECT @@IDENTITY";
			identityCommandPatrols.Connection = Conn;
			// 
			// oleDbDataAdapter1
			// 
			_daPatrols.DeleteCommand = deleteCommandPatrols;
			_daPatrols.InsertCommand = insertCommandPatrols;
			_daPatrols.SelectCommand = selectCommandPatrols;
			_daPatrols.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "Patrols", new System.Data.Common.DataColumnMapping[] {
																																																				   new System.Data.Common.DataColumnMapping("Automatic", "Automatic"),
																																																				   new System.Data.Common.DataColumnMapping("CompetitionId", "CompetitionId"),
																																																				   new System.Data.Common.DataColumnMapping("PatrolId", "PatrolId"),
																																																				   new System.Data.Common.DataColumnMapping("PClass", "PClass"),
																																																				   new System.Data.Common.DataColumnMapping("StartDateTime", "StartDateTime"),
																																																				   new System.Data.Common.DataColumnMapping("PatrolDbId", "PatrolDbId"),
																																																				   new System.Data.Common.DataColumnMapping("StartDateTimeDisplay", "StartDateTimeDisplay")})});
			_daPatrols.UpdateCommand = updateCommandPatrols;

			// Handle fetching identity of new inserted rows.
			_daPatrols.RowUpdated += new OleDbRowUpdatedEventHandler(DAPatrols_RowUpdated);
		}
		private void DAPatrols_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
		{
			if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert )
			{
				// Get the Identity column value
				int identity = Int32.Parse(identityCommandPatrols.ExecuteScalar().ToString());
				e.Row["PatrolDbId"] = identity;
				e.Row.AcceptChanges();
			}
		}
		#endregion

		#region Competitors
		private OleDbCommand selectCommandCompetitors;
		private OleDbCommand insertCommandCompetitors;
		private OleDbCommand updateCommandCompetitors;
		private OleDbCommand deleteCommandCompetitors;
		private OleDbCommand identityCommandCompetitors;
		private void createDataAdapterCompetitors()
		{
			Trace.WriteLine("CDatabase: Entering createDataAdapterCompetitors()");

			// Create adapter
			_daCompetitors = new OleDbDataAdapter();

			// Create commands
			selectCommandCompetitors = new OleDbCommand();
			insertCommandCompetitors = new OleDbCommand();
			updateCommandCompetitors = new OleDbCommand();
			deleteCommandCompetitors = new OleDbCommand();
			identityCommandCompetitors = new OleDbCommand();
			// 
			// selectCommandCompetitors
			// 
			selectCommandCompetitors.CommandText = "SELECT CompetitionId, CompetitorId, Lane, PatrolId, ShooterId, WeaponId, ShooterClass, FinalShootingPlace FROM Comp" +
				"etitors";
			selectCommandCompetitors.Connection = Conn;
			// 
			// insertCommandCompetitors
			// 
			insertCommandCompetitors.CommandText = "INSERT INTO Competitors(CompetitionId, Lane, PatrolId, ShooterId, WeaponId, ShooterClass, FinalShootingPlace) VALUE" +
				"S (?, ?, ?, ?, ?, ?, ?)";
			insertCommandCompetitors.Connection = Conn;
			insertCommandCompetitors.Parameters.Add(new OleDbParameter("CompetitionId", OleDbType.Integer, 0, "CompetitionId"));
			insertCommandCompetitors.Parameters.Add(new OleDbParameter("Lane", OleDbType.Integer, 0, "Lane"));
			insertCommandCompetitors.Parameters.Add(new OleDbParameter("PatrolId", OleDbType.Integer, 0, "PatrolId"));
			insertCommandCompetitors.Parameters.Add(new OleDbParameter("ShooterId", OleDbType.Integer, 0, "ShooterId"));
			insertCommandCompetitors.Parameters.Add(new OleDbParameter("WeaponId", OleDbType.VarWChar, 150, "WeaponId"));
			insertCommandCompetitors.Parameters.Add(new OleDbParameter("ShooterClass", OleDbType.Integer, 0, "ShooterClass"));
			insertCommandCompetitors.Parameters.Add(new OleDbParameter("FinalShootingPlace", OleDbType.Integer, 0, "FinalShootingPlace"));
			// 
			// updateCommandCompetitors
			// 
			updateCommandCompetitors.CommandText = @"UPDATE Competitors SET CompetitionId = ?, Lane = ?, PatrolId = ?, ShooterId = ?, WeaponId = ?, ShooterClass = ?, FinalShootingPlace = ? WHERE (CompetitorId = ?) AND (CompetitionId = ? OR ? IS NULL AND CompetitionId IS NULL) AND (Lane = ? OR ? IS NULL AND Lane IS NULL) AND (PatrolId = ? OR ? IS NULL AND PatrolId IS NULL) AND (ShooterId = ? OR ? IS NULL AND ShooterId IS NULL) AND (WeaponId = ? OR ? IS NULL AND WeaponId IS NULL) AND (ShooterClass = ? OR ? IS NULL AND ShooterClass IS NULL)";
			updateCommandCompetitors.Connection = Conn;
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("CompetitionId", OleDbType.Integer, 0, "CompetitionId"));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Lane", OleDbType.Integer, 0, "Lane"));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("PatrolId", OleDbType.Integer, 0, "PatrolId"));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("ShooterId", OleDbType.Integer, 0, "ShooterId"));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("WeaponId", OleDbType.VarWChar, 150, "WeaponId"));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("ShooterClass", OleDbType.Integer, 0, "ShooterClass"));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("FinalShootingPlace", OleDbType.Integer, 0, "FinalShootingPlace"));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_CompetitorId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_CompetitionId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_CompetitionId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_Lane", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Lane", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_Lane1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Lane", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_PatrolId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_PatrolId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_ShooterId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ShooterId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_ShooterId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ShooterId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_WeaponId", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "WeaponId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_WeaponId1", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "WeaponId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_ShooterClass", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ShooterClass", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitors.Parameters.Add(new OleDbParameter("Original_ShooterClass1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ShooterClass", System.Data.DataRowVersion.Original, null));
			// 
			// deleteCommandCompetitors
			// 
			deleteCommandCompetitors.CommandText = @"DELETE FROM Competitors WHERE (CompetitorId = ?) AND (CompetitionId = ? OR ? IS NULL AND CompetitionId IS NULL) AND (Lane = ? OR ? IS NULL AND Lane IS NULL) AND (PatrolId = ? OR ? IS NULL AND PatrolId IS NULL) AND (ShooterId = ? OR ? IS NULL AND ShooterId IS NULL) AND (WeaponId = ? OR ? IS NULL AND WeaponId IS NULL) AND (ShooterClass = ? OR ? IS NULL AND ShooterClass IS NULL)";
			deleteCommandCompetitors.Connection = Conn;
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_CompetitorId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_CompetitionId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_CompetitionId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_Lane", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Lane", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_Lane1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Lane", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_PatrolId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_PatrolId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PatrolId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_ShooterId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ShooterId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_ShooterId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ShooterId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_WeaponId", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "WeaponId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_WeaponId1", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "WeaponId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_ShooterClass", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ShooterClass", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitors.Parameters.Add(new OleDbParameter("Original_ShooterClass1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ShooterClass", System.Data.DataRowVersion.Original, null));
			//
			// identityCommandCompetitors
			//
			identityCommandCompetitors.CommandText = "SELECT @@IDENTITY";
			identityCommandCompetitors.Connection = Conn;
			// 
			// DACompetitors
			// 
			_daCompetitors.DeleteCommand = deleteCommandCompetitors;
			_daCompetitors.InsertCommand = insertCommandCompetitors;
			_daCompetitors.SelectCommand = selectCommandCompetitors;
			_daCompetitors.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "Competitors", new System.Data.Common.DataColumnMapping[] {
																																																					   new System.Data.Common.DataColumnMapping("CompetitionId", "CompetitionId"),
																																																					   new System.Data.Common.DataColumnMapping("CompetitorId", "CompetitorId"),
																																																					   new System.Data.Common.DataColumnMapping("Lane", "Lane"),
																																																					   new System.Data.Common.DataColumnMapping("PatrolId", "PatrolId"),
																																																					   new System.Data.Common.DataColumnMapping("ShooterId", "ShooterId"),
																																																					   new System.Data.Common.DataColumnMapping("WeaponId", "WeaponId"),
																																																					   new System.Data.Common.DataColumnMapping("ShooterClass", "ShooterClass"),
																																																					   new System.Data.Common.DataColumnMapping("FinalShootingPlace", "FinalShootingPlace")})});
			_daCompetitors.UpdateCommand = updateCommandCompetitors;

			// Handle fetching identity of new inserted rows.
			_daCompetitors.RowUpdated += new OleDbRowUpdatedEventHandler(DACompetitors_RowUpdated);
		}
		private void DACompetitors_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
		{
			if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert )
			{
				// Get the Identity column value
				int identity = Int32.Parse(identityCommandCompetitors.ExecuteScalar().ToString());
				e.Row["CompetitorId"] = identity;
				e.Row.AcceptChanges();
			}
		}
		#endregion

		#region Stations
		private OleDbCommand selectCommandStations;
		private OleDbCommand insertCommandStations;
		private OleDbCommand updateCommandStations;
		private OleDbCommand deleteCommandStations;
		private OleDbCommand identityCommandStations;
		private void createDataAdapterStations()
		{
			Trace.WriteLine("CDatabase: Entering createDataAdapterStations()");

			// Create adapter
			_daStations = new OleDbDataAdapter();

			// Create the commands
			selectCommandStations = new OleDbCommand();
			insertCommandStations = new OleDbCommand();
			updateCommandStations = new OleDbCommand();
			deleteCommandStations = new OleDbCommand();
			identityCommandStations = new OleDbCommand();
			// 
			// selectCommandStations
			// 
			selectCommandStations.CommandText = "SELECT CompetitionId, Figures, Points, Shoots, StationId, StationNr, Distinguish FROM Stations" +
				"";
			selectCommandStations.Connection = Conn;
			// 
			// insertCommandStations
			// 
			insertCommandStations.CommandText = "INSERT INTO Stations(CompetitionId, Figures, Points, Shoots, StationNr, Distinguish) VALUES (?" +
				", ?, ?, ?, ?, ?)";
			insertCommandStations.Connection = Conn;
			insertCommandStations.Parameters.Add(new OleDbParameter("CompetitionId", OleDbType.Integer, 0, "CompetitionId"));
			insertCommandStations.Parameters.Add(new OleDbParameter("Figures", OleDbType.Integer, 0, "Figures"));
			insertCommandStations.Parameters.Add(new OleDbParameter("Points", OleDbType.Boolean, 2, "Points"));
			insertCommandStations.Parameters.Add(new OleDbParameter("Shoots", OleDbType.Integer, 0, "Shoots"));
			insertCommandStations.Parameters.Add(new OleDbParameter("StationNr", OleDbType.Integer, 0, "StationNr"));
			insertCommandStations.Parameters.Add(new OleDbParameter("Distinguish", OleDbType.Boolean, 0, "Distinguish"));
			// 
			// updateCommandStations
			// 
			updateCommandStations.CommandText = @"UPDATE Stations SET CompetitionId = ?, Figures = ?, Points = ?, Shoots = ?, StationNr = ?, Distinguish = ? WHERE (StationId = ?) AND (CompetitionId = ? OR ? IS NULL AND CompetitionId IS NULL) AND (Figures = ? OR ? IS NULL AND Figures IS NULL) AND (Points = ?) AND (Shoots = ? OR ? IS NULL AND Shoots IS NULL) AND (StationNr = ? OR ? IS NULL AND StationNr IS NULL) AND Distinguish = ?";
			updateCommandStations.Connection = Conn;
			updateCommandStations.Parameters.Add(new OleDbParameter("CompetitionId", OleDbType.Integer, 0, "CompetitionId"));
			updateCommandStations.Parameters.Add(new OleDbParameter("Figures", OleDbType.Integer, 0, "Figures"));
			updateCommandStations.Parameters.Add(new OleDbParameter("Points", OleDbType.Boolean, 2, "Points"));
			updateCommandStations.Parameters.Add(new OleDbParameter("Shoots", OleDbType.Integer, 0, "Shoots"));
			updateCommandStations.Parameters.Add(new OleDbParameter("StationNr", OleDbType.Integer, 0, "StationNr"));
			updateCommandStations.Parameters.Add(new OleDbParameter("Distinguish", OleDbType.Boolean, 0, "Distinguish"));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_StationId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationId", System.Data.DataRowVersion.Original, null));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_CompetitionId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_CompetitionId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_Figures", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Figures", System.Data.DataRowVersion.Original, null));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_Figures1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Figures", System.Data.DataRowVersion.Original, null));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_Points", OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Points", System.Data.DataRowVersion.Original, null));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_Shoots", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Shoots", System.Data.DataRowVersion.Original, null));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_Shoots1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Shoots", System.Data.DataRowVersion.Original, null));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_StationNr", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationNr", System.Data.DataRowVersion.Original, null));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_StationNr1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationNr", System.Data.DataRowVersion.Original, null));
			updateCommandStations.Parameters.Add(new OleDbParameter("Original_Distinguish", OleDbType.Boolean, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Distinguish", System.Data.DataRowVersion.Original, null));
			// 
			// deleteCommandStations
			// 
			deleteCommandStations.CommandText = @"DELETE FROM Stations WHERE (StationId = ?) AND (CompetitionId = ? OR ? IS NULL AND CompetitionId IS NULL) AND (Figures = ? OR ? IS NULL AND Figures IS NULL) AND (Points = ?) AND (Shoots = ? OR ? IS NULL AND Shoots IS NULL) AND (StationNr = ? OR ? IS NULL AND StationNr IS NULL)";
			deleteCommandStations.Connection = Conn;
			deleteCommandStations.Parameters.Add(new OleDbParameter("Original_StationId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationId", System.Data.DataRowVersion.Original, null));
			deleteCommandStations.Parameters.Add(new OleDbParameter("Original_CompetitionId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			deleteCommandStations.Parameters.Add(new OleDbParameter("Original_CompetitionId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitionId", System.Data.DataRowVersion.Original, null));
			deleteCommandStations.Parameters.Add(new OleDbParameter("Original_Figures", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Figures", System.Data.DataRowVersion.Original, null));
			deleteCommandStations.Parameters.Add(new OleDbParameter("Original_Figures1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Figures", System.Data.DataRowVersion.Original, null));
			deleteCommandStations.Parameters.Add(new OleDbParameter("Original_Points", OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Points", System.Data.DataRowVersion.Original, null));
			deleteCommandStations.Parameters.Add(new OleDbParameter("Original_Shoots", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Shoots", System.Data.DataRowVersion.Original, null));
			deleteCommandStations.Parameters.Add(new OleDbParameter("Original_Shoots1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Shoots", System.Data.DataRowVersion.Original, null));
			deleteCommandStations.Parameters.Add(new OleDbParameter("Original_StationNr", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationNr", System.Data.DataRowVersion.Original, null));
			deleteCommandStations.Parameters.Add(new OleDbParameter("Original_StationNr1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationNr", System.Data.DataRowVersion.Original, null));
			//
			// identityCommandStations
			//
			identityCommandStations.CommandText = "SELECT @@IDENTITY";
			identityCommandStations.Connection = Conn;
			// 
			// DAStations
			// 
			_daStations.DeleteCommand = deleteCommandStations;
			_daStations.InsertCommand = insertCommandStations;
			_daStations.SelectCommand = selectCommandStations;
			_daStations.UpdateCommand = updateCommandStations;
			_daStations.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							new System.Data.Common.DataTableMapping("Table", "Stations", new System.Data.Common.DataColumnMapping[] {
																																																		new System.Data.Common.DataColumnMapping("CompetitionId", "CompetitionId"),
																																																		new System.Data.Common.DataColumnMapping("Figures", "Figures"),
																																																		new System.Data.Common.DataColumnMapping("Points", "Points"),
																																																		new System.Data.Common.DataColumnMapping("Shoots", "Shoots"),
																																																		new System.Data.Common.DataColumnMapping("StationId", "StationId"),
																																																		new System.Data.Common.DataColumnMapping("StationNr", "StationNr")})});
			_daStations.RowUpdated += new OleDbRowUpdatedEventHandler(DAStations_RowUpdated);

		}
		private void DAStations_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
		{
			if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert )
			{
				// Get the Identity column value
				int identity = Int32.Parse(identityCommandStations.ExecuteScalar().ToString());
				e.Row["StationId"] = identity;
				e.Row.AcceptChanges();
			}
		}
		#endregion

		#region CompetitorResults
		private OleDbCommand selectCommandCompetitorResults;
		private OleDbCommand insertCommandCompetitorResults;
		private OleDbCommand updateCommandCompetitorResults;
		private OleDbCommand deleteCommandCompetitorResults;
		private OleDbCommand identityCommandCompetitorResults;
		private void createDataAdapterCompetitorResults()
		{
			Trace.WriteLine("CDatabase: Entering createDataAdapterCompetitorResults()");

			// Create adapter
			_daCompetitorResults = new OleDbDataAdapter();

			// Create the commands
			selectCommandCompetitorResults = new OleDbCommand();
			insertCommandCompetitorResults = new OleDbCommand();
			updateCommandCompetitorResults = new OleDbCommand();
			deleteCommandCompetitorResults = new OleDbCommand();
			identityCommandCompetitorResults = new OleDbCommand();
			// 
			// selectCommandCompetitorResults
			// 
			selectCommandCompetitorResults.CommandText = "SELECT CompetitorId, FigureHits, Hits, Points, ResultId, StationFigureHits, Stati" +
				"onId FROM CompetitorResults";
			selectCommandCompetitorResults.Connection = Conn;
			// 
			// oleDbInsertCommand1
			// 
			insertCommandCompetitorResults.CommandText = "INSERT INTO CompetitorResults(CompetitorId, FigureHits, Hits, Points, StationFigu" +
				"reHits, StationId) VALUES (?, ?, ?, ?, ?, ?)";
			insertCommandCompetitorResults.Connection = Conn;
			insertCommandCompetitorResults.Parameters.Add(new OleDbParameter("CompetitorId", OleDbType.Integer, 0, "CompetitorId"));
			insertCommandCompetitorResults.Parameters.Add(new OleDbParameter("FigureHits", OleDbType.Integer, 0, "FigureHits"));
			insertCommandCompetitorResults.Parameters.Add(new OleDbParameter("Hits", OleDbType.Integer, 0, "Hits"));
			insertCommandCompetitorResults.Parameters.Add(new OleDbParameter("Points", OleDbType.Integer, 0, "Points"));
			insertCommandCompetitorResults.Parameters.Add(new OleDbParameter("StationFigureHits", OleDbType.VarWChar, 150, "StationFigureHits"));
			insertCommandCompetitorResults.Parameters.Add(new OleDbParameter("StationId", OleDbType.Integer, 0, "StationId"));
			// 
			// updateCommandCompetitorResults
			// 
			updateCommandCompetitorResults.CommandText = @"UPDATE CompetitorResults SET CompetitorId = ?, FigureHits = ?, Hits = ?, Points = ?, StationFigureHits = ?, StationId = ? WHERE (ResultId = ?) AND (CompetitorId = ? OR ? IS NULL AND CompetitorId IS NULL) AND (FigureHits = ? OR ? IS NULL AND FigureHits IS NULL) AND (Hits = ? OR ? IS NULL AND Hits IS NULL) AND (Points = ? OR ? IS NULL AND Points IS NULL) AND (StationFigureHits = ? OR ? IS NULL AND StationFigureHits IS NULL) AND (StationId = ? OR ? IS NULL AND StationId IS NULL)";
			updateCommandCompetitorResults.Connection = Conn;
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("CompetitorId", OleDbType.Integer, 0, "CompetitorId"));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("FigureHits", OleDbType.Integer, 0, "FigureHits"));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Hits", OleDbType.Integer, 0, "Hits"));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Points", OleDbType.Integer, 0, "Points"));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("StationFigureHits", OleDbType.VarWChar, 150, "StationFigureHits"));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("StationId", OleDbType.Integer, 0, "StationId"));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_ResultId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ResultId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_CompetitorId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_CompetitorId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_FigureHits", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "FigureHits", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_FigureHits1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "FigureHits", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_Hits", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Hits", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_Hits1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Hits", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_Points", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Points", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_Points1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Points", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_StationFigureHits", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationFigureHits", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_StationFigureHits1", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationFigureHits", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_StationId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationId", System.Data.DataRowVersion.Original, null));
			updateCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_StationId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationId", System.Data.DataRowVersion.Original, null));
			// 
			// deleteCommandCompetitorResults
			// 
			deleteCommandCompetitorResults.CommandText = @"DELETE FROM CompetitorResults WHERE (ResultId = ?) AND (CompetitorId = ? OR ? IS NULL AND CompetitorId IS NULL) AND (FigureHits = ? OR ? IS NULL AND FigureHits IS NULL) AND (Hits = ? OR ? IS NULL AND Hits IS NULL) AND (Points = ? OR ? IS NULL AND Points IS NULL) AND (StationFigureHits = ? OR ? IS NULL AND StationFigureHits IS NULL) AND (StationId = ? OR ? IS NULL AND StationId IS NULL)";
			deleteCommandCompetitorResults.Connection = Conn;
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_ResultId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ResultId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_CompetitorId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_CompetitorId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_FigureHits", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "FigureHits", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_FigureHits1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "FigureHits", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_Hits", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Hits", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_Hits1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Hits", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_Points", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Points", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_Points1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Points", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_StationFigureHits", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationFigureHits", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_StationFigureHits1", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationFigureHits", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_StationId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationId", System.Data.DataRowVersion.Original, null));
			deleteCommandCompetitorResults.Parameters.Add(new OleDbParameter("Original_StationId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "StationId", System.Data.DataRowVersion.Original, null));
			//
			// identityCommandCompetitorResult
			//
			identityCommandCompetitorResults.CommandText = "SELECT @@IDENTITY";
			identityCommandCompetitorResults.Connection = Conn;
			// 
			// DACompetitorResults
			// 
			_daCompetitorResults.DeleteCommand = deleteCommandCompetitorResults;
			_daCompetitorResults.InsertCommand = insertCommandCompetitorResults;
			_daCompetitorResults.SelectCommand = selectCommandCompetitorResults;
			_daCompetitorResults.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										  new System.Data.Common.DataTableMapping("Table", "CompetitorResults", new System.Data.Common.DataColumnMapping[] {
																																																							   new System.Data.Common.DataColumnMapping("CompetitorId", "CompetitorId"),
																																																							   new System.Data.Common.DataColumnMapping("FigureHits", "FigureHits"),
																																																							   new System.Data.Common.DataColumnMapping("Hits", "Hits"),
																																																							   new System.Data.Common.DataColumnMapping("Points", "Points"),
																																																							   new System.Data.Common.DataColumnMapping("ResultId", "ResultId"),
																																																							   new System.Data.Common.DataColumnMapping("StationFigureHits", "StationFigureHits"),
																																																							   new System.Data.Common.DataColumnMapping("StationId", "StationId")})});
			_daCompetitorResults.UpdateCommand = updateCommandCompetitorResults;

			// Handle fetching identity of new inserted rows.
			_daCompetitorResults.RowUpdated += new OleDbRowUpdatedEventHandler(DACompetitorResults_RowUpdated);
		}
		private void DACompetitorResults_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
		{
			if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert )
			{
				// Get the Identity column value
				int identity = Int32.Parse(identityCommandCompetitorResults.ExecuteScalar().ToString());
				e.Row["ResultId"] = identity;
				e.Row.AcceptChanges();
			}
		}
		#endregion

		#region Teams
		private OleDbCommand selectCommandTeams;
		private OleDbCommand insertCommandTeams;
		private OleDbCommand updateCommandTeams;
		private OleDbCommand deleteCommandTeams;
		private OleDbCommand identityCommandTeams;
		private void createDataAdapterTeams()
		{
			Trace.WriteLine("CDatabase: Entering createDataAdapterTeams()");

			// Create adapter
			_daTeams = new OleDbDataAdapter();

			// Create the commands
			selectCommandTeams = new OleDbCommand();
			insertCommandTeams = new OleDbCommand();
			updateCommandTeams = new OleDbCommand();
			deleteCommandTeams = new OleDbCommand();
			identityCommandTeams = new OleDbCommand();
			// 
			// selectCommandTeams
			// 
			selectCommandTeams.CommandText = "SELECT ClubId, CompetitorId1, CompetitorId2, CompetitorId3, CompetitorId4, CompetitorId5, Name, " +
				"TeamId, WClass FROM Teams";
			selectCommandTeams.Connection = Conn;
			// 
			// insertCommandTeams
			// 
			insertCommandTeams.CommandText = "INSERT INTO Teams(ClubId, CompetitorId1, CompetitorId2, CompetitorId3, Competitor" +
				"Id4, CompetitorId5, Name, WClass) VALUES (?, ?, ?, ?, ?, ?, ?)";
			insertCommandTeams.Connection = Conn;
			insertCommandTeams.Parameters.Add(new OleDbParameter("ClubId", OleDbType.VarWChar, 150, "ClubId"));
			insertCommandTeams.Parameters.Add(new OleDbParameter("CompetitorId1", OleDbType.Integer, 0, "CompetitorId1"));
			insertCommandTeams.Parameters.Add(new OleDbParameter("CompetitorId2", OleDbType.Integer, 0, "CompetitorId2"));
			insertCommandTeams.Parameters.Add(new OleDbParameter("CompetitorId3", OleDbType.Integer, 0, "CompetitorId3"));
			insertCommandTeams.Parameters.Add(new OleDbParameter("CompetitorId4", OleDbType.Integer, 0, "CompetitorId4"));
			insertCommandTeams.Parameters.Add(new OleDbParameter("CompetitorId5", OleDbType.Integer, 0, "CompetitorId5"));
			insertCommandTeams.Parameters.Add(new OleDbParameter("Name", OleDbType.VarWChar, 150, "Name"));
			insertCommandTeams.Parameters.Add(new OleDbParameter("WClass", OleDbType.Integer, 0, "WClass"));
			// 
			// updateCommandTeams
			// 
			updateCommandTeams.CommandText = @"UPDATE Teams SET ClubId = ?, CompetitorId1 = ?, CompetitorId2 = ?, CompetitorId3 = ?, CompetitorId4 = ?, CompetitorId5 = ?, Name = ?, WClass = ? WHERE (TeamId = ?) AND (ClubId = ? OR ? IS NULL AND ClubId IS NULL) AND (CompetitorId1 = ? OR ? IS NULL AND CompetitorId1 IS NULL) AND (CompetitorId2 = ? OR ? IS NULL AND CompetitorId2 IS NULL) AND (CompetitorId3 = ? OR ? IS NULL AND CompetitorId3 IS NULL) AND (CompetitorId4 = ? OR ? IS NULL AND CompetitorId4 IS NULL)AND (CompetitorId5 = ? OR ? IS NULL AND CompetitorId5 IS NULL) AND (Name = ?) AND (WClass = ? OR ? IS NULL AND WClass IS NULL)";
			updateCommandTeams.Connection = Conn;
			updateCommandTeams.Parameters.Add(new OleDbParameter("ClubId", OleDbType.VarWChar, 150, "ClubId"));
			updateCommandTeams.Parameters.Add(new OleDbParameter("CompetitorId1", OleDbType.Integer, 0, "CompetitorId1"));
			updateCommandTeams.Parameters.Add(new OleDbParameter("CompetitorId2", OleDbType.Integer, 0, "CompetitorId2"));
			updateCommandTeams.Parameters.Add(new OleDbParameter("CompetitorId3", OleDbType.Integer, 0, "CompetitorId3"));
			updateCommandTeams.Parameters.Add(new OleDbParameter("CompetitorId4", OleDbType.Integer, 0, "CompetitorId4"));
			updateCommandTeams.Parameters.Add(new OleDbParameter("CompetitorId5", OleDbType.Integer, 0, "CompetitorId5"));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Name", OleDbType.VarWChar, 150, "Name"));
			updateCommandTeams.Parameters.Add(new OleDbParameter("WClass", OleDbType.Integer, 0, "WClass"));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_TeamId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TeamId", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_ClubId", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_ClubId1", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId1", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId11", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId1", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId2", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId2", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId21", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId2", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId3", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId3", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId31", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId3", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId4", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId4", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId41", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId4", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId5", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId5", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId51", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId5", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_Name", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_WClass", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "WClass", System.Data.DataRowVersion.Original, null));
			updateCommandTeams.Parameters.Add(new OleDbParameter("Original_WClass1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "WClass", System.Data.DataRowVersion.Original, null));
			// 
			// deleteCommandTeams
			// 
			deleteCommandTeams.CommandText = @"DELETE FROM Teams WHERE (TeamId = ?) AND (ClubId = ? OR ? IS NULL AND ClubId IS NULL) AND (CompetitorId1 = ? OR ? IS NULL AND CompetitorId1 IS NULL) AND (CompetitorId2 = ? OR ? IS NULL AND CompetitorId2 IS NULL) AND (CompetitorId3 = ? OR ? IS NULL AND CompetitorId3 IS NULL) AND (CompetitorId4 = ? OR ? IS NULL AND CompetitorId4 IS NULL) AND (CompetitorId5 = ? OR ? IS NULL AND CompetitorId5 IS NULL) AND (Name = ?) AND (WClass = ? OR ? IS NULL AND WClass IS NULL)";
			deleteCommandTeams.Connection = Conn;
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_TeamId", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TeamId", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_ClubId", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_ClubId1", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "ClubId", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId1", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId11", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId1", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId2", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId2", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId21", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId2", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId3", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId3", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId31", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId3", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId4", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId4", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId41", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId4", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId5", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId4", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_CompetitorId51", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CompetitorId4", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_Name", OleDbType.VarWChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Name", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_WClass", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "WClass", System.Data.DataRowVersion.Original, null));
			deleteCommandTeams.Parameters.Add(new OleDbParameter("Original_WClass1", OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "WClass", System.Data.DataRowVersion.Original, null));
			//
			// identityCommandTeams
			//
			identityCommandTeams.CommandText = "SELECT @@IDENTITY";
			identityCommandTeams.Connection = Conn;
			// 
			// DATeams
			// 
			_daTeams.DeleteCommand = deleteCommandTeams;
			_daTeams.InsertCommand = insertCommandTeams;
			_daTeams.SelectCommand = selectCommandTeams;
			_daTeams.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "Teams", new System.Data.Common.DataColumnMapping[] {
																																																				 new System.Data.Common.DataColumnMapping("ClubId", "ClubId"),
																																																				 new System.Data.Common.DataColumnMapping("CompetitorId1", "CompetitorId1"),
																																																				 new System.Data.Common.DataColumnMapping("CompetitorId2", "CompetitorId2"),
																																																				 new System.Data.Common.DataColumnMapping("CompetitorId3", "CompetitorId3"),
																																																				 new System.Data.Common.DataColumnMapping("CompetitorId4", "CompetitorId4"),
																																																				 new System.Data.Common.DataColumnMapping("Name", "Name"),
																																																				 new System.Data.Common.DataColumnMapping("TeamId", "TeamId"),
																																																				 new System.Data.Common.DataColumnMapping("WClass", "WClass")})});
			_daTeams.UpdateCommand = updateCommandTeams;

			// Handle fetching identity of new inserted rows.
			_daTeams.RowUpdated += new OleDbRowUpdatedEventHandler(DATeams_RowUpdated);
		}
		private void DATeams_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
		{
			if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert )
			{
				// Get the Identity column value
				int identity = Int32.Parse(identityCommandTeams.ExecuteScalar().ToString());
				e.Row["TeamId"] = identity;
				e.Row.AcceptChanges();
			}
		}
		#endregion

		// Init all threads
		internal void UpdateDatabaseFile()
		{
			Trace.WriteLine("CDatabase: Entering UpdateDatabaseFile()");

			Trace.WriteLine("CDatabase: UpdateDatabaseFile() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: UpdateDatabaseFile() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				try
				{
					if (Conn.State != ConnectionState.Open)
						Conn.Open();
					try
					{
						Trace.WriteLine("CDatabase.UpdateDatabaseFile: Updating Clubs");
						DAClubs.Update(Database, "Clubs");
					}
					catch(System.Data.DBConcurrencyException){}

					try
					{
						Trace.WriteLine("CDatabase.UpdateDatabaseFile: Updating Shooters");
						_daShooters.Update(Database, "Shooters");
					}
					catch(System.Data.DBConcurrencyException){}
				
					try
					{
						Trace.WriteLine("CDatabase.UpdateDatabaseFile: Updating Weapons");
						_daWeapons.Update(Database, "Weapons");
					}
					catch(System.Data.DBConcurrencyException){}
				
					try
					{
						Trace.WriteLine("CDatabase.UpdateDatabaseFile: Updating Competition");
						_daCompetition.Update(Database, "Competition");
					}
					catch(System.Data.DBConcurrencyException){}
				
					try
					{
						Trace.WriteLine("CDatabase.UpdateDatabaseFile: Updating Patrols");
						_daPatrols.Update(Database, "Patrols");
					}
					catch(System.Data.DBConcurrencyException){}
				
					try
					{
						Trace.WriteLine("CDatabase.UpdateDatabaseFile: Updating Stations");
						_daStations.Update(Database, "Stations");
					}
					catch(System.Data.DBConcurrencyException){}
				
					try
					{
						Trace.WriteLine("CDatabase.UpdateDatabaseFile: Updating Competitors");
						_daCompetitors.Update(Database, "Competitors");
					}
					catch(System.Data.DBConcurrencyException){}
				
					try
					{
						Trace.WriteLine("CDatabase.UpdateDatabaseFile: Updating CompetitorResults");
						_daCompetitorResults.Update(Database, "CompetitorResults");
					}
					catch(System.Data.DBConcurrencyException){}
				
					try
					{
						Trace.WriteLine("CDatabase.UpdateDatabaseFile: Updating Teams");
						_daTeams.Update(Database, "Teams");
					}
					catch(System.Data.DBConcurrencyException){}
				
					Database.AcceptChanges();
				}
				catch(System.InvalidOperationException exc)
				{
					Trace.WriteLine("Ett fel uppstod vid skrivning till databas:");
					Trace.WriteLine(exc.ToString());
					if (exc.Message.IndexOf("The connection is already Open")>=0)
						UpdateDatabaseFile();
					else
						throw;
				}
				catch(Exception exc)
				{
					Trace.WriteLine("Ett fel uppstod vid skrivning till databas:");
					Trace.WriteLine(exc.ToString());
					throw;
				}
				finally
				{
					Conn.Close();
					Trace.WriteLine("CDatabase: Leaving UpdateDatabaseFile()");
				}

				Trace.WriteLine("CDatabase: UpdateDatabaseFile() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
		}

		/// <summary>
		/// Run a backup
		/// </summary>
		/// <param name="filename"></param>
		internal void Backup(string filename)
		{
			Trace.WriteLine("CDatabase: Entering Backup(\"" + filename + "\")");

			Trace.WriteLine("CDatabase: Backup(\"" + filename + "\") " +
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			string partConnectionString = MyInterface.connectionString.Substring(MyInterface.connectionString.IndexOf("Data Source=") + "Data Source=".Length);
			string currentDb = partConnectionString.Substring(0, partConnectionString.IndexOf(";"));

			lock (_databaseLocker)
			{
				if (File.Exists(filename))
				{
					Trace.WriteLine("CDatabase: file \"" + filename + "\" alredy exist. Deleting.");
					File.Delete(filename);
				}
				Trace.WriteLine("CDatabase: copying \"" + currentDb + "\" to \"" + filename + "\"");
				System.IO.File.Copy(currentDb, filename);
			}

			Trace.WriteLine("CDatabase: Backup(\"" + filename + "\") " +
				" unlocking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
		}
		#endregion

		#region Get stuff
		internal Structs.Club[] getClubs()
		{
			Trace.WriteLine("CDatabase: Entering getClubs()");

			ArrayList clubs = new ArrayList();
			Structs.Club club = new Structs.Club();

			foreach(DatabaseDataset.ClubsRow row in Database.Clubs.Select("", "Name"))
			{
				club = new Structs.Club();
				club.ClubId = row.ClubId;
				club.Name = row.Name;
				club.Country = row.Country;
				club.Automatic = row.Automatic;
				club.ToAutomatic = row.ToAutomatic;
				if (row.IsPlusgiroNull())
					club.Plusgiro = "";
				else
					club.Plusgiro = row.Plusgiro;

				if (row.IsBankgiroNull())
					club.Bankgiro = "";
				else
					club.Bankgiro = row.Bankgiro;

				clubs.Add(club);
			}
				
			return (Structs.Club[])clubs.ToArray(club.GetType());
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Allberg.Shooter.Common.CannotFindIdException.#ctor(System.String)")]
		internal Structs.Club GetClub(string clubId)
		{
			Trace.WriteLine("CDatabase: Entering getClub(" + 
				clubId + ")");

			if (clubId == null)
			{
				throw new ArgumentNullException("clubId");
			}

			var club = new Structs.Club();

			foreach(DatabaseDataset.ClubsRow row in Database.Clubs.Select(
				"ClubId='" + clubId + "'", "Name"))
			{
				if (row.ClubId != clubId) 
					continue;

				club.ClubId = row.ClubId;
				club.Name = row.Name;
				club.Country = row.Country;
				club.Automatic = row.Automatic;
				club.ToAutomatic = row.ToAutomatic;
				club.Plusgiro = row.IsPlusgiroNull() ? "" : row.Plusgiro;

				club.Bankgiro = row.IsBankgiroNull() ? "" : row.Bankgiro;

				return club;
			}
			throw new CannotFindIdException("Could not find club" + clubId);
		}
		internal Structs.Shooter[] GetShooters(string sorting)
		{
			Trace.WriteLine("CDatabase: Entering getShooters()");

			var shooters = new ArrayList();
			var shooter = new Structs.Shooter();

			foreach(DatabaseDataset.ShootersRow row in Database.Shooters.Select("", sorting))
			{
				// Safety first...
				if (row.IsArrivedNull())
					row.Arrived = false;

				shooter = new Structs.Shooter();
				shooter.Arrived = row.Arrived;
				shooter.ClubId = row.ClubId;
				shooter.CardNr = row.Cardnr;
				shooter.Email = row.Email;
				shooter.Givenname = row.Givenname;
				shooter.Surname = row.Surname;
				shooter.Payed = row.Payed;
				shooter.ToAutomatic = row.ToAutomatic;
				shooter.ShooterId = row.ShooterId;
				shooter.Class = (Structs.ShootersClass)row.Class;
				shooters.Add(shooter);
			}
			return (Structs.Shooter[])shooters.ToArray(shooter.GetType());
		}
		internal Structs.Shooter[] GetShooters(Structs.Club club)
		{
			Trace.WriteLine("CDatabase: Entering getShooters(\"" + club.ClubId + "\")");

			var shooters = new ArrayList();
			var shooter = new Structs.Shooter();

			foreach(DatabaseDataset.ShootersRow row in Database.Shooters.Select("ClubId='" + club.ClubId + "'", "givenname, surname"))
			{
				shooter = new Structs.Shooter();
				shooter.Arrived = row.Arrived;
				shooter.ClubId = row.ClubId;
				shooter.CardNr = row.Cardnr;
				shooter.Email = row.Email;
				shooter.Givenname = row.Givenname;
				shooter.Surname = row.Surname;
				shooter.Payed = row.Payed;
				shooter.ToAutomatic = row.ToAutomatic;
				shooter.ShooterId = row.ShooterId;
				shooter.Class = (Structs.ShootersClass)row.Class;
				shooters.Add(shooter);
			}
			return (Structs.Shooter[])shooters.ToArray(shooter.GetType());
		}
		internal Structs.Shooter[] GetShooters(Structs.Club club, Structs.ResultWeaponsClass wclass)
		{
			Trace.WriteLine(string.Format("CDatabase: Entering getShooters(\"{0}\", {1})", club.ClubId, wclass));

			var shooters = new ArrayList();
			var shooter = new Structs.Shooter();

			foreach(DatabaseDataset.ShootersRow row in Database.Shooters.Select("ClubId='" + club.ClubId + "'", "givenname, surname"))
			{
				foreach(DatabaseDataset.CompetitorsRow comprow in row.GetChildRows("ShootersCompetitors"))
				{
					var weaponsrow = 
						(DatabaseDataset.WeaponsRow)comprow.GetParentRow("WeaponsCompetitors");
					var weaponclass = (Structs.WeaponClass)weaponsrow.Class;
					if (MyInterface.ConvertWeaponsClassToResultClass(weaponclass) != wclass) 
						continue;

					// Only choose those with the same wclass
					shooter = new Structs.Shooter();
					shooter.Arrived = row.Arrived;
					shooter.ClubId = row.ClubId;
					shooter.CardNr = row.Cardnr;
					shooter.Email = row.Email;
					shooter.Givenname = row.Givenname;
					shooter.Surname = row.Surname;
					shooter.Payed = row.Payed;
					shooter.ToAutomatic = row.ToAutomatic;
					shooter.ShooterId = row.ShooterId;
					shooter.Class = (Structs.ShootersClass)row.Class;
					shooters.Add(shooter);
				}
			}
			return (Structs.Shooter[])shooters.ToArray(shooter.GetType());
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Allberg.Shooter.Common.CannotFindIdException.#ctor(System.String)")]
		internal Structs.Shooter GetShooter(int shooterId)
		{
			Trace.WriteLine("CDatabase: Entering getShooter(" +
				shooterId.ToString() + ") on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			Structs.Shooter shooter = new Structs.Shooter();

			foreach(DatabaseDataset.ShootersRow row in 
				Database.Shooters/*.Select("shooterId=" + shooterId.ToString(), 
				"")*/)
			{
				if (row.ShooterId == shooterId)
				{
					shooter.Arrived = row.Arrived;
					shooter.ClubId = row.ClubId;
					shooter.CardNr = row.Cardnr;
					shooter.Email = row.Email;
					shooter.Givenname = row.Givenname;
					shooter.Surname = row.Surname;
					shooter.Payed = row.Payed;
					shooter.ToAutomatic = row.ToAutomatic;
					shooter.Automatic = row.Automatic;
					shooter.ShooterId = row.ShooterId;
					shooter.Class = (Structs.ShootersClass)row.Class;
					return shooter;
				}
			}
			throw new CannotFindIdException("Cannot find shooter id " + shooterId);
		}
		internal int GetShooterHighestId()
		{
			Trace.WriteLine("CDatabase: Entering getShooterHighestId(");

			var shooters =
				Database.Shooters.Select("", "ShooterId desc");
			return (int)shooters[0]["ShooterId"];
		}

		internal Structs.Shooter GetShooter(string cardNr)
		{
			Trace.WriteLine("CDatabase: Entering getShooter(" +
				cardNr + ")");

			var shooter = new Structs.Shooter();

			foreach(DatabaseDataset.ShootersRow row in Database.Shooters.Select("Cardnr='" + cardNr + "'"))
			{
				if (row.Cardnr != cardNr) 
					continue;

				shooter.Arrived = row.Arrived;
				shooter.ClubId = row.ClubId;
				shooter.CardNr = row.Cardnr;
				shooter.Email = row.Email;
				shooter.Givenname = row.Givenname;
				shooter.Surname = row.Surname;
				shooter.Payed = row.Payed;
				shooter.ToAutomatic = row.ToAutomatic;
				shooter.ShooterId = row.ShooterId;
				shooter.Class = (Structs.ShootersClass)row.Class;
				return shooter;
			}
			throw new CannotFindIdException("Cannot find shooter CardNr " + cardNr.ToString());
		}
		internal Structs.Competitor[] getCompetitors()
		{
			Trace.WriteLine("CDatabase: Entering getCompetitors()");

			ArrayList competitors = new ArrayList();
			Structs.Competitor competitor = new Structs.Competitor();

			foreach(DatabaseDataset.CompetitorsRow row in Database.Competitors)
			{
				competitor = new Structs.Competitor();
				competitor.CompetitionId = row.CompetitionId;
				competitor.CompetitorId = row.CompetitorId;
				competitor.ShooterClass = (Structs.ShootersClass)row.ShooterClass;
				if (row.IsPatrolIdNull())
					competitor.PatrolId = -1;
				else
					competitor.PatrolId = row.PatrolId;
				competitor.ShooterId = row.ShooterId;
				competitor.WeaponId = row.WeaponId;
				if (row.IsLaneNull())
					competitor.Lane = -1;
				else
					competitor.Lane = row.Lane;
				competitor.FinalShootingPlace = row.FinalShootingPlace;

				competitors.Add(competitor);
			}
			return (Structs.Competitor[])competitors.ToArray(competitor.GetType());
		}
		internal Structs.Competitor[] GetCompetitorsWithNoPatrol(
			Structs.PatrolClass thisClass)
		{
			Trace.WriteLine("CDatabase: Entering GetCompetitorsWithNoPatrol(" + 
				thisClass.ToString() + ")");

			ArrayList competitors = new ArrayList();
			Structs.Competitor competitor = new Structs.Competitor();

			foreach(DatabaseDataset.CompetitorsRow row in Database.Competitors.Select("PatrolId is null"))
			{
				if (thisClass == Structs.PatrolClass.Ok�nd |
					MyInterface.ConvertWeaponsClassToPatrolClass( 
					MyInterface.GetWeapon( row.WeaponId )
					.WClass) == thisClass)
				{
					competitor = new Structs.Competitor();
					competitor.CompetitionId = row.CompetitionId;
					competitor.CompetitorId = row.CompetitorId;
					competitor.ShooterClass = (Structs.ShootersClass)row.ShooterClass;
					if (row.IsPatrolIdNull())
						competitor.PatrolId = -1;
					else
						competitor.PatrolId = row.PatrolId;
					competitor.ShooterId = row.ShooterId;
					competitor.WeaponId = row.WeaponId;
					if (row.IsLaneNull())
						competitor.Lane = -1;
					else
						competitor.Lane = row.Lane;
					competitor.FinalShootingPlace = row.FinalShootingPlace;
					competitors.Add(competitor);
				}
			}
			return (Structs.Competitor[])competitors.ToArray(competitor.GetType());
		}
		internal Structs.Competitor[] getCompetitors(Structs.Patrol patrolSearch, string sortOrder)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitors(" + 
				patrolSearch.ToString() + ")");

			var competitors = new ArrayList();
			var competitor = new Structs.Competitor();

			foreach(DatabaseDataset.CompetitorsRow row in 
				Database.Competitors.Select(
				"PatrolId=" + patrolSearch.PatrolId, sortOrder))
			{
				if (!row.IsPatrolIdNull())
				{
					if (row.PatrolId == patrolSearch.PatrolId)
					{
						competitor = new Structs.Competitor();
						competitor.CompetitionId = row.CompetitionId;
						competitor.CompetitorId = row.CompetitorId;
						competitor.ShooterClass = (Structs.ShootersClass)row.ShooterClass;
						if (row.IsPatrolIdNull())
							competitor.PatrolId = -1;
						else
							competitor.PatrolId = row.PatrolId;
						competitor.ShooterId = row.ShooterId;
						competitor.WeaponId = row.WeaponId;
						if (row.IsLaneNull())
							competitor.Lane = -1;
						else
							competitor.Lane = row.Lane;
						competitor.FinalShootingPlace = row.FinalShootingPlace;
						competitors.Add(competitor);
					}
				}
			}
			return (Structs.Competitor[])competitors.ToArray(competitor.GetType());
		}

		internal Structs.Competitor[] getCompetitors(int shooterId)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitors(" + 
				shooterId.ToString() + ")");

			ArrayList competitors = new ArrayList();
			Structs.Competitor competitor = new Structs.Competitor();

			foreach(DatabaseDataset.CompetitorsRow row in
				Database.Competitors.Select("ShooterId=" + shooterId.ToString()))
			{
				try
				{
					if (row.ShooterId == shooterId)
					{
						competitor = new Structs.Competitor();
						competitor.CompetitionId = row.CompetitionId;
						competitor.CompetitorId = row.CompetitorId;
						competitor.ShooterClass = (Structs.ShootersClass)row.ShooterClass;
						if (row.IsPatrolIdNull())
							competitor.PatrolId = -1;
						else
							competitor.PatrolId = row.PatrolId;
						competitor.ShooterId = row.ShooterId;
						competitor.WeaponId = row.WeaponId;
						if (row.IsLaneNull())
							competitor.Lane = -1;
						else
							competitor.Lane = row.Lane;
						competitor.FinalShootingPlace = row.FinalShootingPlace;
						competitors.Add(competitor);
					}
				}
				catch(System.Data.DeletedRowInaccessibleException)
				{
				}
			}
			Trace.WriteLine("CDatabase: Exiting getCompetitors()");

			return (Structs.Competitor[])competitors.ToArray(competitor.GetType());
		}
		internal Structs.Competitor[] getCompetitors(int shooterId, string sorting)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitors(" + 
				shooterId.ToString() + ")");

			ArrayList competitors = new ArrayList();
			Structs.Competitor competitor = new Structs.Competitor();

			foreach(DatabaseDataset.CompetitorsRow row in 
				Database.Competitors.Select("ShooterId=" + shooterId.ToString(), sorting))
			{
				if (row.ShooterId == shooterId)
				{
					competitor = new Structs.Competitor();
					competitor.CompetitionId = row.CompetitionId;
					competitor.CompetitorId = row.CompetitorId;
					competitor.ShooterClass = (Structs.ShootersClass)row.ShooterClass;
					if (row.IsPatrolIdNull())
						competitor.PatrolId = -1;
					else
						competitor.PatrolId = row.PatrolId;
					competitor.ShooterId = row.ShooterId;
					competitor.WeaponId = row.WeaponId;
					if (row.IsLaneNull())
						competitor.Lane = -1;
					else
						competitor.Lane = row.Lane;
					competitor.FinalShootingPlace = row.FinalShootingPlace;
					competitors.Add(competitor);
				}
			}
			return (Structs.Competitor[])competitors.ToArray(competitor.GetType());
		}
		internal Structs.Competitor[] GetCompetitors(Structs.Club ClubToFetch, 
			Structs.ResultWeaponsClass wclass, string sorting)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitors(" + 
				ClubToFetch.ToString() + ", " + wclass + ")");

			Structs.CompetitionTypeEnum compType = getCompetitions()[0].Type;

			ArrayList competitors = new ArrayList();
			Structs.Competitor competitor = new Structs.Competitor();

			foreach(DatabaseDataset.ShootersRow shooter in 
				Database.Shooters.Select("ClubId='" + ClubToFetch.ClubId + "'", sorting))
			{
				DatabaseDataset.CompetitorsRow[] compRows = shooter.GetCompetitorsRows();
				foreach(DatabaseDataset.CompetitorsRow compRow in compRows)
				{
					Structs.Weapon weapon = 
						MyInterface.GetWeapon(compRow.WeaponId);

					if (CConvert.ConvertWeaponsClassToResultClass(weapon.WClass, compType) == wclass)
					{
						Structs.Competitor comp = MyInterface.GetCompetitor(compRow.CompetitorId);
						competitors.Add(comp);
					}
				}
			}
			return (Structs.Competitor[])competitors.ToArray(competitor.GetType());
		}
		internal Structs.Competitor getCompetitor(int competitorId)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitor(" + 
				competitorId.ToString() + ") on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			Structs.Competitor competitor = new Structs.Competitor();

			foreach(DatabaseDataset.CompetitorsRow row in 
				Database.Competitors.Select("CompetitorId=" + competitorId.ToString()))
			{
				if (row.CompetitorId == competitorId)
				{
					competitor.CompetitionId = row.CompetitionId;
					competitor.CompetitorId = row.CompetitorId;
					competitor.ShooterClass = (Structs.ShootersClass)row.ShooterClass;
					if (row.IsPatrolIdNull())
						competitor.PatrolId = -1;
					else
						competitor.PatrolId = row.PatrolId;
					competitor.ShooterId = row.ShooterId;
					competitor.WeaponId = row.WeaponId;
					if (row.IsLaneNull())
						competitor.Lane = -1;
					else
						competitor.Lane = row.Lane;
					competitor.FinalShootingPlace = row.FinalShootingPlace;
					return competitor;
				}
			}
			throw new CannotFindIdException("Could not find competitor with id " + competitorId.ToString());
		}
		internal Structs.Weapon[] getWeapons(string sorting)
		{
			Trace.WriteLine("CDatabase: Entering getWeapons()");

			Structs.CompetitionTypeEnum comptype = getCompetitions()[0].Type;

			ArrayList weapons = new ArrayList();
			Structs.Weapon weapon = new Structs.Weapon();

			foreach(DatabaseDataset.WeaponsRow row in Database.Weapons.Select("", sorting))
			{
				weapon = new Structs.Weapon();
				weapon.Manufacturer = row.Manufacturer;
				weapon.Model = row.Model;
				weapon.Caliber = row.Caliber;
				weapon.WClass = (Structs.WeaponClass)row.Class;
				weapon.WeaponId = row.WeaponId;
				weapon.Automatic = row.Automatic;
				weapon.ToAutomatic = row.ToAutomatic;

				if (comptype != Structs.CompetitionTypeEnum.MagnumField)
					weapons.Add(weapon);
				else
				{
					if (weapon.WClass.ToString().IndexOf("M") > -1 &
						weapon.WClass.ToString().Length > 1)
						weapons.Add(weapon);
				}
			}
			return (Structs.Weapon[])weapons.ToArray(weapon.GetType());
		}
		internal Structs.Weapon getWeapon(string weaponId)
		{
			Trace.WriteLine("CDatabase: Entering getWeapon(" + 
				weaponId + ") on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			DateTime start = DateTime.Now;

			Structs.Weapon weapon = new Structs.Weapon();

			foreach(DatabaseDataset.WeaponsRow row in Database.Weapons.Select("WeaponId='" + weaponId + "'"))
			{
				if (row.WeaponId.ToUpper() == weaponId.ToUpper())
				{
					weapon.Manufacturer = row.Manufacturer;
					weapon.Model = row.Model;
					weapon.Caliber = row.Caliber;
					weapon.WClass = (Structs.WeaponClass)row.Class;
					weapon.WeaponId = row.WeaponId;
					weapon.Automatic = row.Automatic;
					weapon.ToAutomatic = row.ToAutomatic;

					Trace.WriteLine("CDatabase: Ending getWeapon() after " +
						(DateTime.Now-start).TotalMilliseconds.ToString() +
						" ms.");
					return weapon;
				}
			}
			throw new CannotFindIdException("Could not find WeapondId " + weaponId.ToString());
		}
		internal Structs.Competition[] getCompetitions()
		{
			Trace.WriteLine("CDatabase: Entering getCompetitions() on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

			ArrayList competitions = new ArrayList();
			Structs.Competition competition = 
				new Structs.Competition();

			foreach(DatabaseDataset.CompetitionRow row in Database.Competition)
			{
				competition = new Structs.Competition();
				competition.CompetitionId = row.CompetitionId;
				competition.Name = row.Name;
				competition.NorwegianCount = row.NorwegianCount;
				competition.PatrolSize = row.PatrolSize;
				competition.PatrolTime = row.PatrolTime;
				competition.PatrolTimeBetween = row.PatrolTimeBetween;
				competition.PatrolTimeRest = row.PatrolTimeRest;
				competition.StartTime = row.StartDate.AddMinutes(row.StartTime);
				competition.DoFinalShooting = row.DoFinalShooting;
				competition.FirstPrice = row.FirstPrice;
				competition.PriceMoneyPercentToReturn = row.PriceMoneyPercentToReturn;
				competition.ShooterFee1 = row.ShooterFee1;
				competition.ShooterFee2 = row.ShooterFee2;
				competition.ShooterFee3 = row.ShooterFee3;
				competition.ShooterFee4 = row.ShooterFee4;
				competition.UsePriceMoney = row.UsePriceMoney;
				competition.PriceMoneyShooterPercent = row.PriceMoneyShooterPercent;
				competition.Type = (Structs.CompetitionTypeEnum)row.Type;
				competition.Championship = (Structs.CompetitionChampionshipEnum)row.Championship;
				competition.PatrolConnectionType = (Structs.PatrolConnectionTypeEnum)row.PatrolConnectionType;
				competition.OneClass = row.OneClass;
				competitions.Add(competition);
			}
			return (Structs.Competition[])competitions.ToArray(competition.GetType());
		}
		internal Structs.Patrol[] getPatrols()
		{
			Trace.WriteLine("CDatabase: Entering getPatrols()");

			ArrayList patrols = new ArrayList();
			Structs.Patrol patrol = new Structs.Patrol();

			DateTime compStart = getCompetitions()[0].StartTime;
			foreach(DatabaseDataset.PatrolsRow row in Database.Patrols.Select("", "PatrolId"))
			{
				patrol = new Structs.Patrol();
				patrol.CompetitionId = row.CompetitionId;
				patrol.PatrolId = row.PatrolId;
				patrol.StartDateTime = compStart.AddMinutes(row.StartDateTime);
				patrol.PClass = (Structs.PatrolClass)row.PClass;
				if (row.StartDateTimeDisplay > -1054800000)
					patrol.StartDateTimeDisplay = compStart.AddMinutes(row.StartDateTimeDisplay);
				else
					patrol.StartDateTimeDisplay = patrol.StartDateTime;
				patrol.LockedForAutomatic = row.Automatic;
				patrols.Add(patrol);
			}

			return (Structs.Patrol[])patrols.ToArray(patrol.GetType());
		}
		internal Structs.Patrol[] getPatrols(
			Structs.PatrolClass patrolClass, 
			bool alsoIncludeUnknownClass,
			bool OnlyIncludePatrolsWithSpace,
			int PatrolIdToAlwaysView)
		{
			Trace.WriteLine("CDatabase: Entering getPatrols(PatrolClass=" + 
				patrolClass.ToString() + ", alsoIncludeUnknownClass=" +
				alsoIncludeUnknownClass.ToString() + ")");

			string select = "PClass=" + ((int)patrolClass).ToString();
			if (alsoIncludeUnknownClass)
				select += " or PClass=" + 
					((int)Structs.PatrolClass.Ok�nd).ToString();

			ArrayList patrols = new ArrayList();
			Structs.Patrol patrol = new Structs.Patrol();
			
			Structs.Competition competition = getCompetitions()[0];
			DateTime compStart = competition.StartTime;
			foreach(DatabaseDataset.PatrolsRow row in Database.Patrols.Select(
				select, "PatrolId"))
			{
				patrol = new Structs.Patrol();
				patrol.CompetitionId = row.CompetitionId;
				patrol.PatrolId = row.PatrolId;
				patrol.StartDateTime = compStart.AddMinutes(row.StartDateTime);
				patrol.PClass = (Structs.PatrolClass)row.PClass;
				if (row.StartDateTimeDisplay > -1054800000)
					patrol.StartDateTimeDisplay = compStart.AddMinutes(row.StartDateTimeDisplay);
				else
					patrol.StartDateTimeDisplay = patrol.StartDateTime;
				patrol.LockedForAutomatic = row.Automatic;

				if (!OnlyIncludePatrolsWithSpace |
					Database.Competitors.Select("PatrolId=" + 
						patrol.PatrolId.ToString()).Length<competition.PatrolSize |
					patrol.PatrolId == PatrolIdToAlwaysView)
				{
					patrols.Add(patrol);
				}
			}
			return (Structs.Patrol[])patrols.ToArray(patrol.GetType());
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Allberg.Shooter.Common.CannotFindIdException.#ctor(System.String)")]
		internal Structs.Patrol getPatrol(int id)
		{
			Trace.WriteLine("CDatabase: Entering getPatrol(" +
				id.ToString() + ")");

			Structs.Patrol patrol = new Structs.Patrol();

			if (getCompetitions().Length == 0)
				throw new CannotFindIdException("Cannot find patrol with Id " + 
					id.ToString());

			DateTime compStart = getCompetitions()[0].StartTime;
			foreach(DatabaseDataset.PatrolsRow row in 
				Database.Patrols.Select("PatrolId=" + id.ToString()))
			{
				if (row.PatrolId == id)
				{
					patrol.CompetitionId = row.CompetitionId;
					patrol.PatrolId = row.PatrolId;
					patrol.StartDateTime = compStart.AddMinutes(row.StartDateTime);
					if (row.StartDateTimeDisplay < -1054800000)
						patrol.StartDateTimeDisplay = compStart.AddMinutes(row.StartDateTime);
					else
						patrol.StartDateTimeDisplay = compStart.AddMinutes(row.StartDateTimeDisplay);
					patrol.PClass = (Structs.PatrolClass)row.PClass;
					patrol.LockedForAutomatic = row.Automatic;
					return patrol;
				}
			}
			throw new CannotFindIdException("Cannot find patrol with Id " + 
				id.ToString());
		}
		internal Structs.CompetitorResult[] getCompetitorResults()
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorResults()");

			ArrayList compresults = new ArrayList();
			Structs.CompetitorResult compresult =
				new Structs.CompetitorResult();

			foreach(DatabaseDataset.CompetitorResultsRow row in
				Database.CompetitorResults)
			{
				DatabaseDataset.StationsRow stationsRow = 
					(DatabaseDataset.StationsRow)Database.Stations.Select("StationId=" + row.StationId.ToString())[0];

				compresult =
					new Structs.CompetitorResult();
				compresult.CompetitorId = row.CompetitorId;
				compresult.Hits = row.Hits;
				compresult.ResultId = row.ResultId;
				compresult.Station = stationsRow.StationNr;
				compresult.FigureHits = row.FigureHits;
				compresult.Points = row.Points;
				compresult.StationFigureHits = row.StationFigureHits;
				compresults.Add(compresult);
			}
			return (Structs.CompetitorResult[])compresults.ToArray(compresult.GetType());
		}
		internal Structs.CompetitorResult[] getCompetitorResults(int competitorsId)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorResults(" + 
				competitorsId.ToString() + ")");

			ArrayList compresults = new ArrayList();
			Structs.CompetitorResult compresult =
				new Structs.CompetitorResult();

			foreach(DatabaseDataset.CompetitorResultsRow row in
				Database.CompetitorResults.Select(
				"CompetitorId=" + competitorsId.ToString()))
			{
				if (row.CompetitorId == competitorsId)
				{
					DatabaseDataset.StationsRow stationsRow = 
						(DatabaseDataset.StationsRow)Database.Stations.Select("StationId=" + row.StationId.ToString())[0];

					compresult.CompetitorId = row.CompetitorId;
					compresult.Hits = row.Hits;
					compresult.ResultId = row.ResultId;
					compresult.Station = stationsRow.StationNr;
					compresult.FigureHits = row.FigureHits;
					compresult.Points = row.Points;
					compresult.StationFigureHits = row.StationFigureHits;
					compresults.Add(compresult);
				}
			}
			return (Structs.CompetitorResult[])compresults.ToArray(compresult.GetType());
		}
		internal Structs.CompetitorResult getCompetitorResult(int competitorsId, int stationNr)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorResults(" + 
				competitorsId.ToString() + "," + 
				stationNr.ToString() + ")");

			foreach(DatabaseDataset.CompetitorResultsRow row in
				Database.CompetitorResults)
			{
				DatabaseDataset.StationsRow stationsRow = 
					(DatabaseDataset.StationsRow)Database.Stations.Select("StationId=" + row.StationId.ToString())[0];

				if (row.CompetitorId == competitorsId & stationsRow.StationNr == stationNr)
				{
					Structs.CompetitorResult compresult =
						new Structs.CompetitorResult();
					compresult.CompetitorId = row.CompetitorId;
					compresult.Hits = row.Hits;
					compresult.ResultId = row.ResultId;
					compresult.Station = stationsRow.StationNr;
					compresult.FigureHits = row.FigureHits;
					compresult.Points = row.Points;
					compresult.StationFigureHits = row.StationFigureHits;

					return compresult;
				}
			}
			throw new CannotFindIdException("Could not find competitorid and station");
		}
		internal Structs.Station[] getStations()
		{
			Trace.WriteLine("CDatabase: Entering getStations()");

			List<Structs.Station> stations = new List<Structs.Station>();

			foreach(DatabaseDataset.StationsRow row in
				Database.Stations.Select("", "StationNr"))
			{
				if (!row.Distinguish)
				{
					Structs.Station station = new Structs.Station();
					station.CompetitionId = row.CompetitionId;
					station.Figures = row.Figures;
					station.Points = row.Points;
					station.Shoots = row.Shoots;
					station.StationNr = row.StationNr;
					station.StationId = row.StationId;
					station.Distinguish = row.Distinguish;
					stations.Add(station);
				}
			}
			return stations.ToArray();
		}
		internal Structs.Station[] getStationsDistinguish()
		{
			Trace.WriteLine("CDatabase: Entering getStationsDistinguish()");

			List<Structs.Station> stations = new List<Structs.Station>();

			foreach (DatabaseDataset.StationsRow row in
				Database.Stations.Select("", "StationNr"))
			{
				if (row.Distinguish)
				{
					Structs.Station station = new Structs.Station();
					station.CompetitionId = row.CompetitionId;
					station.Figures = row.Figures;
					station.Points = row.Points;
					station.Shoots = row.Shoots;
					station.StationNr = row.StationNr;
					station.StationId = row.StationId;
					station.Distinguish = row.Distinguish;
					stations.Add(station);
				}
			}

			if (stations.Count > 0)
				return stations.ToArray();
			else
			{
				Structs.Station stn = new Structs.Station();
				stn.CompetitionId = getCompetitions()[0].CompetitionId;
				stn.StationNr = 1;
				stn.Points = false;
				switch (getCompetitions()[0].Type)
				{
					case Structs.CompetitionTypeEnum.Field:
						stn.Figures = 6;
						stn.Shoots = 6;
						break;
					case Structs.CompetitionTypeEnum.MagnumField:
						stn.Figures = 6;
						stn.Shoots = 6;
						break;
					case Structs.CompetitionTypeEnum.Precision:
						stn.Figures = 1;
						stn.Shoots = 50;
						break;
					default:
						throw new NotImplementedException(
							getCompetitions()[0].Type.ToString() +
							" is not implemented");
				}
				
				newStation(stn, true);
				return getStationsDistinguish();
			}
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Allberg.Shooter.Common.CannotFindIdException.#ctor(System.String)")]
		internal Structs.Station getStation(int StationNr, bool Distinguish)
		{
			Trace.WriteLine("CDatabase: Entering getStation(" + 
				StationNr.ToString() + ")");

			foreach(DatabaseDataset.StationsRow row in
				Database.Stations.Select("StationNr=" +  StationNr.ToString() +
				" and Distinguish=" + Distinguish.ToString(), "StationId"))
			{
				if (row.StationNr == StationNr)
				{
					Structs.Station station = new Structs.Station();
					station.CompetitionId = row.CompetitionId;
					station.Figures = row.Figures;
					station.Points = row.Points;
					station.Shoots = row.Shoots;
					station.StationNr = row.StationNr;
					station.StationId = row.StationId;
					station.Distinguish = row.Distinguish;
					return station;
				}
			}
			throw new CannotFindIdException("Could not find stationnr " + StationNr.ToString());
		}


		internal Structs.Team[] getTeams()
		{
			Trace.WriteLine("CDatabase: Entering getTeams()");

			ArrayList list = new ArrayList();
			foreach(DatabaseDataset.TeamsRow row in
				Database.Teams)
			{
				Structs.Team team = new Structs.Team();
				team.CompetitorIds = new ArrayList();

				team.ClubId = row.ClubId;
				team.Name = row.Name;
				if (!row.IsCompetitorId1Null())
					team.CompetitorIds.Add(row.CompetitorId1);

				if (!row.IsCompetitorId2Null())
					team.CompetitorIds.Add(row.CompetitorId2);

				if (!row.IsCompetitorId3Null())
					team.CompetitorIds.Add(row.CompetitorId3);

				if (!row.IsCompetitorId4Null())
					team.CompetitorIds.Add(row.CompetitorId4);

				if (!row.IsCompetitorId5Null())
					team.CompetitorIds.Add(row.CompetitorId5);

				team.TeamId = row.TeamId;
				team.WClass = (Structs.ResultWeaponsClass)row.WClass;
				list.Add(team);
			}
			return (Structs.Team[])list.ToArray(typeof(Structs.Team));
		}


		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Allberg.Shooter.Common.CannotFindIdException.#ctor(System.String)")]
		internal Structs.Team getTeam(int teamId)
		{
			Trace.WriteLine("CDatabase: Entering getStation(" + 
				teamId.ToString() + ")");

			foreach(DatabaseDataset.TeamsRow row in
				Database.Teams.Select("TeamId=" + teamId))
			{
				Structs.Team team = new Structs.Team();
				team.ClubId = row.ClubId;
				team.Name = row.Name;

				if (team.CompetitorIds == null)
					team.CompetitorIds = new ArrayList();

				if (!row.IsCompetitorId1Null())
					team.CompetitorIds.Add( row.CompetitorId1);

				if (!row.IsCompetitorId2Null())
					team.CompetitorIds.Add( row.CompetitorId2);

				if (!row.IsCompetitorId3Null())
					team.CompetitorIds.Add( row.CompetitorId3);

				if (!row.IsCompetitorId4Null())
					team.CompetitorIds.Add(row.CompetitorId4);

				if (!row.IsCompetitorId5Null())
					team.CompetitorIds.Add(row.CompetitorId5);

				team.TeamId = row.TeamId;
				team.WClass = (Structs.ResultWeaponsClass)row.WClass;
				return team;
			}
			throw new CannotFindIdException("Could not find teamid " + teamId.ToString());
		}


		#endregion

		#region Get stuff count
		internal int getClubsCount()
		{
			Trace.WriteLine("CDatabase: Entering getClubsCount()");

			return Database.Clubs.Count;
		}
		internal int getClubsCountWithShooters()
		{
			Trace.WriteLine("CDatabase: Entering getClubsCountWithUsers()");
			try
			{
				int count = 0;

				foreach (DatabaseDataset.ClubsRow club in Database.Clubs)
				{
					if (Database.Shooters.Select("ClubId='" + club.ClubId + "'").Length > 0)
						count++;
				}

				Trace.WriteLine("CDatabase: Ending getClubsCountWithUsers()");
				return count;
			}
			catch (System.ArgumentOutOfRangeException)
			{
				Trace.WriteLine("CDatabase.getClubsCountWithUsers: ArgumentOutOfRangeException");
				return 0;
			}
			catch (InvalidOperationException exc)
			{
				Trace.WriteLine("CDatabase.getClubsCountWithUsers: " + exc.ToString());
				return 0;
			}
		}
		internal int getShootersCount()
		{
			Trace.WriteLine("CDatabase: Entering getShootersCount()");

			return Database.Shooters.Count;
		}
		internal int getCompetitorsCount()
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorsCount()");

			return Database.Competitors.Count;
		}
		internal int getCompetitorsCountPatrolId(Structs.Patrol patrol)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorsCountPatrolId(" +
				patrol.PatrolId.ToString() +
				")" +
				"on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() +
				" ) ");
			DateTime start = DateTime.Now;

			int count = Database.Competitors.Select(
				"PatrolId=" + patrol.PatrolId.ToString(), "PatrolId").Length;

			/*
			foreach(DatabaseDataset.CompetitorsRow row in database.Competitors.Select(
				"PatrolId=" + patrol.PatrolId.ToString(), "PatrolId"))
			{
				try
				{
					if(row.PatrolId == patrol.PatrolId)
						count++;
				}
				catch(Exception)
				{}
			}*/
			Trace.WriteLine("CDatabase: Exiting getCompetitorsCountPatrolId(" + 
				patrol.PatrolId.ToString() + ") - took " +
				(DateTime.Now-start).TotalMilliseconds.ToString() +
				" ms.");
			return count;
		}
		internal int getCompetitorsWithResultCountPatrol(Structs.Patrol patrol)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorsWithResultCountPatrol(" +
				patrol.PatrolId.ToString() +
				")" +
				"on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() +
				" ) ");
			DateTime start = DateTime.Now;

			DatabaseDataset.CompetitorsRow[] comps = 
				(DatabaseDataset.CompetitorsRow[])
				Database.Competitors.Select(
				"PatrolId=" + patrol.PatrolId.ToString(), "PatrolId");

			int count = 0;
			foreach (DatabaseDataset.CompetitorsRow row in comps)
			{
				if (row.GetCompetitorResultsRows().Length > 0)
					count++;
			}

			Trace.WriteLine("CDatabase: Exiting getCompetitorsWithResultCountPatrol(" +
				patrol.PatrolId.ToString() + ") - took " +
				(DateTime.Now - start).TotalMilliseconds.ToString() +
				" ms.");
			return count;
		}
		internal int GetCompetitorsArrivedCountPatrol(Structs.Patrol patrol)
		{
			Trace.WriteLine("CDatabase: Entering GetCompetitorsArrivedCountPatrol(" +
				patrol.PatrolId.ToString() +
				")" +
				"on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() +
				" ) ");
			DateTime start = DateTime.Now;

			DatabaseDataset.CompetitorsRow[] comps = 
				(DatabaseDataset.CompetitorsRow[])
				Database.Competitors.Select(
				"PatrolId=" + patrol.PatrolId.ToString(), "PatrolId");

			int count = 0;
			foreach (DatabaseDataset.CompetitorsRow row in comps)
			{
				int ShooterId = row.ShooterId;
				DatabaseDataset.ShootersRow[] shooters =
				(DatabaseDataset.ShootersRow[])
				Database.Shooters.Select(
				"ShooterId=" + ShooterId.ToString());

				if (shooters[0].Arrived)
					count++;
			}

			Trace.WriteLine("CDatabase: Exiting GetCompetitorsArrivedCountPatrol(" +
				patrol.PatrolId.ToString() + ") - took " +
				(DateTime.Now - start).TotalMilliseconds.ToString() +
				" ms.");
			return count;
		}
		
		internal int getWeaponsCount()
		{
			Trace.WriteLine("CDatabase: Entering getWeaponsCount()");

			return Database.Weapons.Count;
		}
		internal int getCompetitionsCount()
		{
			Trace.WriteLine("CDatabase: Entering getCompetitionsCount()");

			return Database.Competition.Count;
		}
		internal int getPatrolsCount()
		{
			Trace.WriteLine("CDatabase: Entering getPatrolsCount()");

			return Database.Patrols.Count;
		}
		internal int getCompetitorResultsCount()
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorResultsCount()");

			return Database.CompetitorResults.Count;
		}
		internal int getCompetitorResultsCount(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorResultsCount(" +
				wclass.ToString() + ", " + uclass.ToString() + ") on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			DateTime start = DateTime.Now;

			int returnValue = 0;
			try
			{
				Hashtable compsList = new Hashtable();
				Hashtable weaponList = new Hashtable();
				foreach(DatabaseDataset.CompetitorResultsRow row in
					Database.CompetitorResults)
				{
					Structs.Competitor comp;
					if (compsList.ContainsKey(row.CompetitorId))
						comp = (Structs.Competitor)compsList[row.CompetitorId];
					else
					{
						comp = 
							MyInterface.GetCompetitor(row.CompetitorId);
						compsList.Add(row.CompetitorId, comp);
					}
					Structs.Weapon weapon;
					if (weaponList.ContainsKey(comp.WeaponId))
						weapon = (Structs.Weapon)weaponList[comp.WeaponId];
					else
					{
						weapon = MyInterface.GetWeapon(comp.WeaponId);
						weaponList.Add(comp.WeaponId, weapon);
					}
					if ( MyInterface.ConvertWeaponsClassToResultClass(
						weapon.WClass) == wclass)
					{
						// Weaponsclass is correct. Check Shooters class
						//Interface.Shooter shooter = 
						//	myInterface.GetShooter(comp.ShooterId);

						if (comp.ShooterClass == uclass)
							returnValue++;
					}
				}
			}
			catch(System.InvalidOperationException)
			{
				// this occurs when the collection changed during testing.
				// try again
				Trace.WriteLine("CDatabase: getCompetitorResultsCount failed due to changed collection. Retrying.");
				return getCompetitorResultsCount(wclass, uclass);
			}

			TimeSpan span = DateTime.Now - start;
			Trace.WriteLine("CDatabase: Leaving getCompetitorResultsCount() after " +
				Common.Formatter.FormatTimeSpan(span) + "sec.");
			return returnValue;
		}
		internal int getCompetitorResultsCount(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass, string clubId)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorResultsCount(" +
				wclass.ToString() + ", " + uclass.ToString() + ") on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			int returnValue = 0;
			try
			{
				Hashtable compsList = new Hashtable();
				Hashtable weaponList = new Hashtable();
				foreach(DatabaseDataset.CompetitorResultsRow row in
					Database.CompetitorResults)
				{
					//Structs.Shooter shooter = myInterface.GetShooter(row.CompetitorId);
					DatabaseDataset.CompetitorsRow compRow = 
						(DatabaseDataset.CompetitorsRow)row.GetParentRow("CompetitorsCompetitorResults");
					DatabaseDataset.ShootersRow shooterRow = 
						(DatabaseDataset.ShootersRow)compRow.GetParentRow("ShootersCompetitors");

					if (shooterRow.ClubId == clubId)
					{
						Structs.Competitor comp;
						if (compsList.ContainsKey(row.CompetitorId))
							comp = (Structs.Competitor)compsList[row.CompetitorId];
						else
						{
							comp = 
								MyInterface.GetCompetitor(row.CompetitorId);
							compsList.Add(row.CompetitorId, comp);
						}
						Structs.Weapon weapon;
						if (weaponList.ContainsKey(comp.WeaponId))
							weapon = (Structs.Weapon)weaponList[comp.WeaponId];
						else
						{
							weapon = MyInterface.GetWeapon(comp.WeaponId);
							weaponList.Add(comp.WeaponId, weapon);
						}
						if ( MyInterface.ConvertWeaponsClassToResultClass(
							weapon.WClass) == wclass)
						{
							// Weaponsclass is correct. Check Shooters class
							//Interface.Shooter shooter = 
							//	myInterface.GetShooter(comp.ShooterId);

							if (comp.ShooterClass == uclass)
								returnValue++;
						}
					}
				}
			}
			catch(System.InvalidOperationException)
			{
				// this occurs when the collection changed during testing.
				// try again
				Trace.WriteLine("CDatabase: getCompetitorResultsCount failed due to changed collection. Retrying.");
				return getCompetitorResultsCount(wclass, uclass);
			}

			Trace.WriteLine("CDatabase: Leaving getCompetitorResultsCount()");
			return returnValue;
		}
		internal bool getCompetitorResultsExist(Structs.ResultWeaponsClass wclass)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorResultsExist(" +
				wclass.ToString() + ") on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

			try
			{
				Hashtable compsList = new Hashtable();
				Hashtable weaponsList = new Hashtable();
				foreach(DatabaseDataset.CompetitorResultsRow row in
					Database.CompetitorResults)
				{
					Structs.Competitor comp;
					if (compsList.ContainsKey(row.CompetitorId))
						comp = (Structs.Competitor)compsList[row.CompetitorId];
					else
					{
						comp = MyInterface.GetCompetitor(row.CompetitorId);
						compsList.Add(row.CompetitorId, comp);
					}
					Structs.Weapon weapon; 
					if (weaponsList.ContainsKey(comp.WeaponId))
						weapon = (Structs.Weapon)weaponsList[comp.WeaponId];
					else
					{
						weapon = MyInterface.GetWeapon(comp.WeaponId);
						weaponsList.Add(comp.WeaponId, weapon);
					}
					if ( MyInterface.ConvertWeaponsClassToResultClass(
						weapon.WClass) == wclass)
					{
						// Weaponsclass is correct. 
						return true;
					}
				}
			}
			catch(System.InvalidOperationException)
			{
				// this occurs when the collection changed during testing.
				// try again
				Trace.WriteLine("CDatabase: getCompetitorResultsCount failed to changed collection. Retrying.");
				return getCompetitorResultsExist(wclass);
			}

			Trace.WriteLine("CDatabase: Leaving getCompetitorResultsExist()");
			return false;
		}
		internal bool getCompetitorResultsExist(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass)
		{
			Trace.WriteLine("CDatabase: Entering getCompetitorResultsExist(" +
				wclass.ToString() + ", " + uclass.ToString() + ") on thread \"" +
				System.Threading.Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

			try
			{
				Hashtable compsList = new Hashtable();
				Hashtable weaponsList = new Hashtable();
				foreach(DatabaseDataset.CompetitorResultsRow row in
					Database.CompetitorResults)
				{
					Structs.Competitor comp;
					if (compsList.ContainsKey(row.CompetitorId))
						comp = (Structs.Competitor)compsList[row.CompetitorId];
					else
					{
						comp = MyInterface.GetCompetitor(row.CompetitorId);
						compsList.Add(row.CompetitorId, comp);
					}
					Structs.Weapon weapon;
					if (weaponsList.ContainsKey(comp.WeaponId))
						weapon = (Structs.Weapon)weaponsList[comp.WeaponId];
					else
					{
						weapon = MyInterface.GetWeapon(comp.WeaponId);
						weaponsList.Add(comp.WeaponId, weapon);
					}
					if ( MyInterface.ConvertWeaponsClassToResultClass(
						weapon.WClass) == wclass)
					{
						// Weaponsclass is correct. Check Shooters class
						//Interface.Shooter shooter = 
						//	myInterface.GetShooter(comp.ShooterId);

						if (comp.ShooterClass == uclass)
							return true;
					}
				}
			}
			catch(System.InvalidOperationException)
			{
				// this occurs when the collection changed during testing.
				// try again
				Trace.WriteLine("CDatabase: getCompetitorResultsCount failed to changed collection. Retrying.");
				return getCompetitorResultsExist(wclass, uclass);
			}

			Trace.WriteLine("CDatabase: Leaving getCompetitorResultsExist()");
			return false;
		}
		internal int getStationsCount()
		{
			Trace.WriteLine("CDatabase: Entering getStationsCount()");

			return Database.Stations.Select("Distinguish=false").Length;
		}
		#endregion

		#region Update stuff
		internal void updateClub(Structs.Club club)
		{
			Trace.WriteLine("CDatabase: Entering updateClub(" + 
				club.ToString() + ")");

			bool found = false;
			foreach(DatabaseDataset.ClubsRow row in Database.Clubs.Select(
				"ClubId='" + club.ClubId + "'"))
			{
				if (row.ClubId == club.ClubId)
				{
					row.Country = club.Country;
					row.Name = club.Name;
					row.Automatic = club.Automatic;
					row.ToAutomatic = club.ToAutomatic;
					row.Plusgiro = club.Plusgiro;
					row.Bankgiro = club.Bankgiro;
					row.LastUpdate = DateTime.Now;

					found = true;
				}
			}
			if (!found)
				throw new CannotFindIdException("Could not find ClubId " +
					club.ClubId.ToString() + " to update");
			else
				MyInterface.updatedClub();
		}
		internal void updateShooter(Structs.Shooter shooter)
		{
			Trace.WriteLine("CDatabase: Entering updateShooter(" + 
				shooter.ToString() + ")");
			
			bool found = false;
			foreach(DatabaseDataset.ShootersRow row in Database.Shooters.Select(
				"ShooterId=" + shooter.ShooterId.ToString()))
			{
				if (row.ShooterId == shooter.ShooterId)
				{
					row.Arrived = shooter.Arrived;
					row.ClubId = shooter.ClubId;
					row.Cardnr = shooter.CardNr;
					row.Email = shooter.Email;
					row.Givenname = shooter.Givenname;
					row.Payed = shooter.Payed;
					row.Surname = shooter.Surname;
					row.ToAutomatic = shooter.ToAutomatic;
					row.Automatic = shooter.Automatic;
					row.Class = (int)shooter.Class;
					row.LastUpdate = DateTime.Now;
					found = true;
				}
			}
			if (!found)
				throw new CannotFindIdException("Could not find shooterId " +
					shooter.ShooterId.ToString() + " to update");
			else
				MyInterface.updatedShooter(shooter);
		}
		internal void updateCompetitor(Structs.Competitor competitor, 
			bool updateGui)
		{
			Trace.WriteLine("CDatabase: Entering updateCompetitor(" + 
				competitor.ToString() + ")");

			if (competitor.WeaponId == null)
			{
				Trace.WriteLine("CDatabase: updateCompetitor " + 
					"competitor.WeaponId is null.");
				throw new CannotFindIdException("competitor.WeaponId is null.");
			}

			bool found = false;
			foreach(DatabaseDataset.CompetitorsRow row in 
				Database.Competitors.Select("CompetitorId=" + 
					competitor.CompetitorId.ToString()))
			{
				lock(_databaseLocker)
				{
					int oldPatrol = -1;
					int newPatrol = -1;
					try
					{
						// Check there isn't anyone already on that lane and patrol
						if (Database.Competitors.Select("PatrolId=" + competitor.PatrolId +
							" and Lane=" + competitor.Lane + 
							" and CompetitorId<>" + competitor.CompetitorId).Length>0)
						{
							throw new PatrolAndLaneAlreadyTakenException(
								"There already exist a competitor with patrol " + 
								competitor.PatrolId + " and lane " + 
								competitor.Lane + ". Error occured when updating " +
								"competitorId " + competitor.CompetitorId.ToString());
						}

						// Save new and old values for patrols
						if (!row.IsPatrolIdNull())
							oldPatrol = row.PatrolId;
						newPatrol = competitor.PatrolId;

						row.CompetitionId = competitor.CompetitionId;
						if (competitor.PatrolId == -1)
							row.SetPatrolIdNull();
						else
							row.PatrolId = competitor.PatrolId;
						row.ShooterId = competitor.ShooterId;
						row.WeaponId = competitor.WeaponId;
						row.ShooterClass = (int)competitor.ShooterClass;

						if (competitor.PatrolId != -1)
						{
							if (competitor.Lane == -1)
							{
								try
								{
									competitor.Lane = 
										MyInterface.patrolClass
										.GetNextLane(competitor.PatrolId);
								}
								catch(PatrolAlreadyFullException)
								{
									competitor.PatrolId = -1;
									row.SetPatrolIdNull();
									row.SetLaneNull();
									throw;
								}
							}
						}
						else
						{
							competitor.Lane = -1;
						}
						if (competitor.Lane == -1)
							row.SetLaneNull();
						else
							row.Lane = competitor.Lane;
						row.FinalShootingPlace = competitor.FinalShootingPlace;

						found = true;
					}
					finally
					{
						// Update patroltypes
						checkForPatrolClassUpdate(oldPatrol, updateGui);
						checkForPatrolClassUpdate(newPatrol, updateGui);
					}
				}
			}
			if (!found)
				throw new CannotFindIdException("Could not find CompetitorId " +
					competitor.CompetitorId.ToString() + " to update");
			else
				if (updateGui)
					MyInterface.updatedCompetitor(competitor);
		}
		internal void updateWeapon(Structs.Weapon weapon)
		{
			Trace.WriteLine("CDatabase: Entering updateWeapon(" + 
				weapon.ToString() + ")");

			bool found = false;
			foreach(DatabaseDataset.WeaponsRow row in Database.Weapons.Select(
				"WeaponId='" + weapon.WeaponId + "'"))
			{
				if (row.WeaponId == weapon.WeaponId)
				{
					row.Manufacturer = weapon.Manufacturer;
					row.Model = weapon.Model;
					row.Caliber = weapon.Caliber;
					row.Class = (int)weapon.WClass;
					row.Automatic = weapon.Automatic;
					row.ToAutomatic = weapon.ToAutomatic;
					row.LastUpdate = DateTime.Now;

					found = true;
				}
			}
			if (!found)
				throw new CannotFindIdException("Could not find WeaponId " +
					weapon.WeaponId.ToString() + " to update");
			else
				MyInterface.updatedWeapon();
		}
		internal void updateCompetition(Structs.Competition competition)
		{
			Trace.WriteLine("CDatabase: Entering updateCompetition(" + 
				competition.ToString() + ")");

			bool found = false;
			bool PatrolConnectionTypeChanged = false;
			foreach(DatabaseDataset.CompetitionRow row in Database.Competition.Select(
				"CompetitionId=" + competition.CompetitionId.ToString()))
			{
				if (row.CompetitionId == competition.CompetitionId)
				{					
					row.Name = competition.Name;
					row.NorwegianCount = competition.NorwegianCount;
					row.PatrolSize = competition.PatrolSize;
					row.PatrolTime = competition.PatrolTime;
					row.PatrolTimeBetween = competition.PatrolTimeBetween;
					row.PatrolTimeRest = competition.PatrolTimeRest;
					row.StartDate = competition.StartTime.Date;
					row.StartTime = (int)competition.StartTime.TimeOfDay.TotalMinutes;
					row.DoFinalShooting = competition.DoFinalShooting;
					row.FirstPrice = competition.FirstPrice;
					row.PriceMoneyPercentToReturn = competition.PriceMoneyPercentToReturn;
					row.ShooterFee1 = competition.ShooterFee1;
					row.ShooterFee2 = competition.ShooterFee2;
					row.ShooterFee3 = competition.ShooterFee3;
					row.ShooterFee4 = competition.ShooterFee4;
					row.UsePriceMoney = competition.UsePriceMoney;
					row.PriceMoneyShooterPercent = competition.PriceMoneyShooterPercent;
					row.Type = (int)competition.Type;
					row.Championship = (int)competition.Championship;
					row.OneClass = competition.OneClass;

					if (row.PatrolConnectionType != (int)competition.PatrolConnectionType)
					{
						PatrolConnectionTypeChanged = true;
						row.PatrolConnectionType = (int)competition.PatrolConnectionType;
					}

					found = true;
				}
			}

			if (!found)
				throw new CannotFindIdException("Could not find competitionId " +
					competition.CompetitionId.ToString() + " to update");
			else
				MyInterface.updatedCompetition();
			
			if (PatrolConnectionTypeChanged)
			{
				MyInterface.patrolClass.ChangePatrolConnectionType(competition.PatrolConnectionType);
			}
		}

		internal void updatePatrol(Structs.Patrol patrol, bool updateGui)
		{
			Trace.WriteLine("CDatabase: Entering updatePatrol(" + 
				patrol.ToString() + ", " + updateGui.ToString() + ")");

			DateTime compStart = getCompetitions()[0].StartTime;

			bool found = false;
			foreach(DatabaseDataset.PatrolsRow row in 
				Database.Patrols.Select("PatrolId=" + patrol.PatrolId.ToString())) 
			{
				if (row.PatrolId == patrol.PatrolId)
				{
					row.CompetitionId = patrol.CompetitionId;
					row.StartDateTime = (int)(patrol.StartDateTime-compStart).TotalMinutes;
					row.StartDateTimeDisplay = (int)(patrol.StartDateTimeDisplay-compStart).TotalMinutes;
					row.PClass = (int)patrol.PClass;
					row.Automatic = patrol.LockedForAutomatic;

					found = true;
				}
			}
			if (!found)
				throw new CannotFindIdException("Could not find PatrolId " +
					patrol.PatrolId.ToString() + " to update");
			else
				if (updateGui)
					MyInterface.updatedPatrol();
		}
		internal void updateCompetitorResult(Structs.CompetitorResult competitorResult,
			bool UpdateInterface)
		{
			Trace.WriteLine("CDatabase: Entering updateCompetitorResult(" + 
				competitorResult.ToString() + ")");

			foreach(DatabaseDataset.CompetitorResultsRow row in 
				Database.CompetitorResults.Select(
				"ResultId=" + competitorResult.ResultId))
			{
				DatabaseDataset.StationsRow stationsRow = 
					(DatabaseDataset.StationsRow)Database.Stations.Select("StationId=" + row.StationId.ToString())[0];

				if (row.ResultId == competitorResult.ResultId)
				{
					row.CompetitorId = competitorResult.CompetitorId;
					row.Hits = competitorResult.Hits;
					if (stationsRow.StationNr != competitorResult.Station)
						throw new ApplicationException(
							"Ops. Updating a competitorresult with a new stationnr is not supported");
					row.FigureHits = competitorResult.FigureHits;
					row.Points = competitorResult.Points;
					row.StationFigureHits = competitorResult.StationFigureHits;

					if (UpdateInterface)
						MyInterface.updatedCompetitorResult(competitorResult);

					return;
				}
			}
			throw new CannotFindIdException("Could not find competitorResultId " +
					competitorResult.ResultId.ToString() + " to update");
		}
		internal void updateStation(Structs.Station station)
		{
			Trace.WriteLine("CDatabase: Entering updateStation(" + 
				station.StationNr.ToString() + ")");

			bool found = false;
			foreach(DatabaseDataset.StationsRow row in 
				Database.Stations.Select("StationId=" + station.StationId.ToString() +
				" and Distinguish=" + station.Distinguish.ToString()))
			{
				if (row.StationId == station.StationId)
				{
					row.CompetitionId = station.CompetitionId;
					row.Figures = station.Figures;
					row.Points = station.Points;
					row.Shoots = station.Shoots;
					row.StationNr = station.StationNr;
					found = true;
				}
			}
			if (!found)
				throw new CannotFindIdException("Could not find stationId " +
					station.StationId.ToString() + " to update");
			else
				MyInterface.updatedStation();
		}
		internal void updateTeam(Structs.Team team)
		{
			Trace.WriteLine("CDatabase: Entering updateTeam(" + 
				team.TeamId.ToString() + ")");

			bool found = false;
			foreach(DatabaseDataset.TeamsRow row in 
				Database.Teams.Select("TeamId=" + team.TeamId.ToString()))
			{
				row.ClubId = team.ClubId;

				row.SetCompetitorId1Null();
				row.SetCompetitorId2Null();
				row.SetCompetitorId3Null();
				row.SetCompetitorId4Null();
				row.SetCompetitorId5Null();

				int i = 1;
				foreach (int competitorId in team.CompetitorIds)
				{
					switch (i)
					{
						case 1:
							row.CompetitorId1 = competitorId;
							break;
						case 2:
							row.CompetitorId2 = competitorId;
							break;
						case 3:
							row.CompetitorId3 = competitorId;
							break;
						case 4:
							row.CompetitorId4 = competitorId;
							break;
						case 5:
							row.CompetitorId5 = competitorId;
							break;
					}
					i++;
				}

				row.Name = team.Name;
				row.WClass = (int)team.WClass;
				found = true;
			}
			if (!found)
				throw new CannotFindIdException("Could not find teamId " +
					team.TeamId.ToString() + " to update");
			else
				MyInterface.updatedTeam();
		}
		#endregion

		#region Add stuff
		internal void newClub(Structs.Club club)
		{
			Trace.WriteLine("CDatabase: Entering newClub(" + 
				club.ToString() + ")");

			DatabaseDataset.ClubsRow row = Database.Clubs.NewClubsRow();
			row.ClubId = club.ClubId;
			row.Country = club.Country;
			row.Name = club.Name;
			row.Automatic = club.Automatic;
			row.ToAutomatic = club.ToAutomatic;
			row.Plusgiro = club.Plusgiro;
			row.Bankgiro = club.Bankgiro;
			row.LastUpdate = DateTime.Now;

			Trace.WriteLine("CDatabase: newClub() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: newClub() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				Database.Clubs.AddClubsRow(row);

				Trace.WriteLine("CDatabase: newClub() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			MyInterface.updatedClub();
		}
		internal int newShooter(Structs.Shooter shooter, bool updateGui)
		{
			Trace.WriteLine("CDatabase: Entering newShooter(" + 
				shooter.ToString() + ")");

			DatabaseDataset.ShootersRow row = 
				Database.Shooters.NewShootersRow();

			Random rnd = new Random();
			int shooterid = 0;
			try
			{
				while(true)
				{
					shooterid = -rnd.Next(20000)-200000;
					MyInterface.GetShooter(shooterid);
				}
			}
			catch(CannotFindIdException)
			{
				// Shooterid does not already exist. Very good.
			}
			row.ShooterId = shooterid; // Will be automaticly set when updating database file
			row.Arrived = shooter.Arrived;
			row.ClubId = shooter.ClubId;
			row.Cardnr = shooter.CardNr;
			row.Email = shooter.Email;
			row.Givenname = shooter.Givenname;
			row.Payed = shooter.Payed;
			row.Surname = shooter.Surname;
			row.ToAutomatic = shooter.ToAutomatic;
			row.Automatic = shooter.Automatic;
			row.Class = (int)shooter.Class;
			row.LastUpdate = DateTime.Now;

			Trace.WriteLine("CDatabase: newShooter() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: newShooter() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				Database.Shooters.AddShootersRow(row);

				Trace.WriteLine("CDatabase: newShooter() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			if (updateGui)
			{
				MyInterface.updatedShooter(shooter);
			}
			else
			{
				UpdateDatabaseFile();
			}
			return row.ShooterId;
		}
		internal void newCompetitor(Structs.Competitor competitor, bool updateGui)
		{
			Trace.WriteLine("CDatabase: Entering newCompetitor(" + 
				competitor.ToString() + ")");

			if (competitor.WeaponId == null)
			{
				Trace.WriteLine("CDatabase: newCompetitor WeaponId is null.");
				throw new CannotFindIdException("competitors WeaponId is null");
			}

			checkCompetitorIsNotFullOnCompetitors(competitor.ShooterId);

			DatabaseDataset.CompetitorsRow row = 
				Database.Competitors.NewCompetitorsRow();

			Random rnd = new Random();
			int competitorid = 0;
			try
			{
				while(true)
				{
					competitorid = -rnd.Next(20000)-200000;
					MyInterface.GetCompetitor(competitorid);
				}
			}
			catch(CannotFindIdException)
			{
				// Competitorid does not already exist. Very good.
			}

			row.CompetitorId = competitorid; 
				// CompetitorId will be automaticly set when updating database file,
				// but it's important that it is unique since it's not safe
				// to assume that database file will be updated before next
				// competitor is added.
			row.CompetitionId = competitor.CompetitionId;
			row.ShooterId = competitor.ShooterId;
			row.WeaponId = competitor.WeaponId;
			row.ShooterClass = (int)competitor.ShooterClass;

			// Patrol
			if (competitor.PatrolId < 1)
				row.SetPatrolIdNull();
			else
				row.PatrolId = competitor.PatrolId;

			// Lane
			if (competitor.Lane < 1)
				row.SetLaneNull();
			else
				row.Lane = competitor.Lane;
			row.FinalShootingPlace = competitor.FinalShootingPlace;

			Trace.WriteLine("CDatabase: newCompetitor() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: newCompetitor() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				if (!row.IsPatrolIdNull())
				{
					if (Database.Competitors.Select("PatrolId=" + row.PatrolId.ToString() +
						" and Lane=" + row.Lane.ToString()).Length>0)
					{
						throw new ApplicationException(
							"There already exist a competitor with this patrol and lane");
					}
				}

				Database.Competitors.AddCompetitorsRow(row);

				Trace.WriteLine("CDatabase: newCompetitor() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			if (!row.IsPatrolIdNull())
				checkForPatrolClassUpdate(row.PatrolId, updateGui);
			if (updateGui)
				MyInterface.updatedCompetitor(competitor);
		}
		private void checkCompetitorIsNotFullOnCompetitors(int shooterid)
		{
			if (MyInterface.GetCompetitors(shooterid).Length >= MaxNrOfCompetitorsPerShooter)
			{
				Structs.Shooter shooter = GetShooter(shooterid);
				throw new ShooterIsAlreadyFullWithCompetitors(
					"Shooterid " + shooterid.ToString() + 
					" (" + shooter.Surname + " " + shooter.Givenname + ") already has " +
					MaxNrOfCompetitorsPerShooter.ToString() + 
					" competitors.");
			}
		}
		internal void newWeapon(Structs.Weapon weapon)
		{
			Trace.WriteLine("CDatabase: Entering newWeapon(" + 
				weapon.ToString() + ")");

			DatabaseDataset.WeaponsRow row =
				Database.Weapons.NewWeaponsRow();

			row.Class = (int)weapon.WClass;
			row.Manufacturer = weapon.Manufacturer;
			row.Model = weapon.Model;
			row.Caliber = weapon.Caliber;
			row.WeaponId = weapon.WeaponId;
			row.Automatic = weapon.Automatic;
			row.ToAutomatic = weapon.ToAutomatic;
			row.LastUpdate = DateTime.Now;

			Trace.WriteLine("CDatabase: newWeapon() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: newWeapon() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				Database.Weapons.AddWeaponsRow(row);

				Trace.WriteLine("CDatabase: newWeapon() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			MyInterface.updatedWeapon();
		}
		internal void newCompetition(Structs.Competition comp)
		{
			Trace.WriteLine("CDatabase: Entering newCompetition(" + 
				comp.ToString() + ")");

			DatabaseDataset.CompetitionRow row =
				Database.Competition.NewCompetitionRow();

			row.CompetitionId = -1; // Will be automaticly set when updating database file
			row.Name = comp.Name;
			row.NorwegianCount = comp.NorwegianCount;
			row.PatrolSize = comp.PatrolSize;
			row.PatrolTime = comp.PatrolTime;
			row.PatrolTimeBetween = comp.PatrolTimeBetween;
			row.PatrolTimeRest = comp.PatrolTimeRest;
			row.StartDate = comp.StartTime.Date;
			row.StartTime = (int)comp.StartTime.TimeOfDay.TotalMinutes;
			row.DoFinalShooting = comp.DoFinalShooting;
			row.FirstPrice = comp.FirstPrice;
			row.PriceMoneyPercentToReturn = comp.PriceMoneyPercentToReturn;
			row.ShooterFee1 = comp.ShooterFee1;
			row.ShooterFee2 = comp.ShooterFee2;
			row.ShooterFee3 = comp.ShooterFee3;
			row.ShooterFee4 = comp.ShooterFee4;
			row.UsePriceMoney = comp.UsePriceMoney;
			row.PriceMoneyShooterPercent = comp.PriceMoneyShooterPercent;
			row.Type = (int)comp.Type;
			row.Championship = (int)comp.Championship;
			row.PatrolConnectionType = (int)comp.PatrolConnectionType;
			row.OneClass = comp.OneClass;

			Trace.WriteLine("CDatabase: newCompetition() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: newCompetition() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				Database.Competition.AddCompetitionRow(row);

				Trace.WriteLine("CDatabase: newCompetition() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			MyInterface.updatedCompetition();
		}
		internal int newPatrol(Structs.Patrol patrol, bool NotifyGui)
		{
			Trace.WriteLine("CDatabase: Entering newPatrol(" + 
				patrol.ToString() + ", " + NotifyGui.ToString() + ")");
			Trace.WriteLine("CDatabase newPatrol: StartDateTime = " +
				patrol.StartDateTime.ToString());

			DateTime compStart = getCompetitions()[0].StartTime;
			DatabaseDataset.PatrolsRow row =
				Database.Patrols.NewPatrolsRow();

			Trace.WriteLine("CDatabase: newPatrol() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: newPatrol() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				row.PatrolId = getHighestPatrolId() + 1;
				row.CompetitionId = patrol.CompetitionId;
				row.StartDateTime = (int)(patrol.StartDateTime-compStart).TotalMinutes;
				row.PClass = (int)patrol.PClass;
				row.Automatic = patrol.LockedForAutomatic;
				if (patrol.StartDateTimeDisplay == new DateTime(1,1,1,0,0,0,0))
					row.StartDateTimeDisplay = (int)(patrol.StartDateTimeDisplay-compStart).TotalMinutes;
				else
					row.StartDateTimeDisplay = row.StartDateTime;

				Database.Patrols.AddPatrolsRow(row);

				Trace.WriteLine("CDatabase: newPatrol() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			if (NotifyGui)
				MyInterface.updatedPatrol();
			return row.PatrolId;
		}
		private int getHighestPatrolId()
		{
			Trace.WriteLine("CDatabase: getHighestPatrolId() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: getHighestPatrolId() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				int highest = 0;
				foreach(DatabaseDataset.PatrolsRow ptrl in Database.Patrols)
				{
					if (ptrl.PatrolId > highest)
						highest = ptrl.PatrolId;
				}

				Trace.WriteLine("CDatabase: getHighestPatrolId() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				return highest;
			}
		}
		internal int newCompetitorResult(Structs.CompetitorResult res)
		{
			Trace.WriteLine("CDatabase: Entering newCompetitorResult(" + 
				res.ToString() + ")");

			DatabaseDataset.StationsRow stationsRow = 
				(DatabaseDataset.StationsRow)Database.Stations.Select("StationNr=" + res.Station.ToString())[0];

			DatabaseDataset.CompetitorResultsRow row =
				Database.CompetitorResults.NewCompetitorResultsRow();

			Random rnd = new Random();
			row.ResultId = 0 - rnd.Next(10000); // Will be automaticly set when updating database file
			row.CompetitorId = res.CompetitorId;
			row.Hits = res.Hits;
			row.StationId = stationsRow.StationId;
			row.FigureHits = res.FigureHits;
			row.Points = res.Points;
			row.StationFigureHits = res.StationFigureHits;

			Trace.WriteLine("CDatabase: newCompetitorResult() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: newCompetitorResult() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				// Check that there doesn't already exist a result
				// for this competitor and station
				if (Database.CompetitorResults.Select(
					"CompetitorId=" + row.CompetitorId + 
					" and StationId=" + row.StationId).Length > 0)
				{
					throw new ApplicationException(
						"Result for this competitor already exist!");
				}

				Database.CompetitorResults.AddCompetitorResultsRow(row);
				
				Trace.WriteLine("CDatabase: newCompetitorResult() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			Console.WriteLine("New competitorResult:" + row.ResultId);
			MyInterface.updatedCompetitor( getCompetitor(res.CompetitorId) );
			return row.ResultId;
		}
		internal void newStation(Structs.Station station, bool distinguish)
		{
			Trace.WriteLine("CDatabase: Entering newStation(" + 
				station.StationNr.ToString() + ")");

			if (Database.Stations.Select("StationNr=" + station.StationNr.ToString() + 
				" and Distinguish=" + distinguish.ToString() ).Length == 0)
			{
				DatabaseDataset.StationsRow row =
					Database.Stations.NewStationsRow();

				row.StationId = station.StationId; // Will be automaticly set when updating database file
				row.CompetitionId = station.CompetitionId;
				row.Figures = station.Figures;
				row.Points = station.Points;
				row.Shoots = station.Shoots;
				row.StationNr = station.StationNr;
				row.Distinguish = distinguish;

				Trace.WriteLine("CDatabase: newStation() " + 
					" locking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				lock(_databaseLocker)
				{
					Trace.WriteLine("CDatabase: newStation() " + 
						" locked \"DatabaseLocker\" on thread \"" +
						Thread.CurrentThread.Name + "\" ( " +
						System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

					Database.Stations.AddStationsRow(row);

					Trace.WriteLine("CDatabase: newStation() " + 
						" unlocking \"DatabaseLocker\" on thread \"" +
						Thread.CurrentThread.Name + "\" ( " +
						System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
				}
				MyInterface.updatedStation();
			}
			else
			{
				station.StationId = (int)Database.Stations.Select("StationNr=" + station.StationNr.ToString())[0]["StationId"];
				updateStation(station);
			}
		}
		internal void newTeam(Structs.Team team)
		{
			Trace.WriteLine("CDatabase: Entering newTeam(" + 
				team.TeamId.ToString() + ")");

			if (Database.Teams.Select("TeamId=" + team.TeamId.ToString()).Length == 0)
			{
				DatabaseDataset.TeamsRow row =
					Database.Teams.NewTeamsRow();

				row.ClubId = team.ClubId;
				row.SetCompetitorId1Null();
				row.SetCompetitorId2Null();
				row.SetCompetitorId3Null();
				row.SetCompetitorId4Null();

				int i=1;
				foreach(int competitorId in team.CompetitorIds)
				{
					switch (i)
					{
						case 1:
							row.CompetitorId1 = competitorId;
							break;
						case 2:
							row.CompetitorId2 = competitorId;
							break;
						case 3:
							row.CompetitorId3 = competitorId;
							break;
						case 4:
							row.CompetitorId4 = competitorId;
							break;
						case 5:
							row.CompetitorId5 = competitorId;
							break;
					}
					i++;
				}

				row.Name = team.Name;
				Random rnd = new Random();
				row.TeamId = 10000 + rnd.Next(1000);
				row.WClass = (int)team.WClass;

				Trace.WriteLine("CDatabase: newTeam() " + 
					" locking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				lock(_databaseLocker)
				{
					Trace.WriteLine("CDatabase: newTeam() " + 
						" locked \"DatabaseLocker\" on thread \"" +
						Thread.CurrentThread.Name + "\" ( " +
						System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

					Database.Teams.AddTeamsRow(row);

					Trace.WriteLine("CDatabase: newTeam() " + 
						" unlocking \"DatabaseLocker\" on thread \"" +
						Thread.CurrentThread.Name + "\" ( " +
						System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
				}
				MyInterface.updatedTeam();
			}
			else
			{
				updateTeam(team);
			}
		}
		#endregion

		#region Remove Stuff
		internal void delClub(Structs.Club club)
		{
			Trace.WriteLine("CDatabase: Entering delClub(" + 
				club.ToString() + ")");

			Trace.WriteLine("CDatabase: delClub() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: delClub() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				foreach(DatabaseDataset.ClubsRow row in Database.Clubs.Select(
					"ClubId='" + club.ClubId + "'"))
				{
					if(row.ClubId == club.ClubId)
					{
						row.Delete();
					}
				}
				Trace.WriteLine("CDatabase: delClub() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			MyInterface.updatedClub();
			return;
		}
		internal void delShooter(Structs.Shooter shooter)
		{
			Trace.WriteLine("CDatabase: Entering delShooter(" + 
				shooter.ToString() + ")");

			Trace.WriteLine("CDatabase: delShooter() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: delShooter() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				foreach(DatabaseDataset.ShootersRow row in Database.Shooters
					.Select("ShooterId=" + shooter.ShooterId.ToString()))
				{
					if(row.ShooterId == shooter.ShooterId)
					{
						row.Delete();
					}
				}

				Trace.WriteLine("CDatabase: delShooter() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			MyInterface.updatedShooter(shooter);
			return;
		}
		internal void delCompetitor(Structs.Competitor competitor, bool updateGui)
		{
			Trace.WriteLine("CDatabase: Entering delCompetitor(" + 
				competitor.ToString() + ")");

			Trace.WriteLine("CDatabase: delCompetitor() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: delCompetitor() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				foreach(DatabaseDataset.CompetitorsRow row in Database.Competitors
					.Select("CompetitorId=" + competitor.CompetitorId.ToString()))
				{
					if(row.CompetitorId == competitor.CompetitorId)
					{
						int patrolId = -1;
						if (!row.IsPatrolIdNull())
							patrolId = row.PatrolId;
						if (row.GetTeamsRowsByCompetitorsTeams1().Length > 0 ||
							row.GetTeamsRowsByCompetitorsTeams2().Length > 0 ||
							row.GetTeamsRowsByCompetitorsTeams3().Length > 0 ||
							row.GetTeamsRowsByCompetitorsTeams4().Length > 0 ||
							row.GetTeamsRowsByCompetitorsTeams5().Length > 0)
						{
							// Delete team membership in nr 1
							DatabaseDataset.TeamsRow[] teams1 =
								row.GetTeamsRowsByCompetitorsTeams1();
							foreach (DatabaseDataset.TeamsRow team in teams1)
							{
								team.SetCompetitorId1Null();
							}

							// Delete team membership in nr 2
							DatabaseDataset.TeamsRow[] teams2 =
								row.GetTeamsRowsByCompetitorsTeams2();
							foreach (DatabaseDataset.TeamsRow team in teams2)
							{
								team.SetCompetitorId2Null();
							}

							// Delete team membership in nr 3
							DatabaseDataset.TeamsRow[] teams3 =
								row.GetTeamsRowsByCompetitorsTeams3();
							foreach (DatabaseDataset.TeamsRow team in teams3)
							{
								team.SetCompetitorId3Null();
							}
							// Delete team membership in nr 4
							DatabaseDataset.TeamsRow[] teams4 =
								row.GetTeamsRowsByCompetitorsTeams4();
							foreach (DatabaseDataset.TeamsRow team in teams4)
							{
								team.SetCompetitorId4Null();
							}
							// Delete team membership in nr 5
							DatabaseDataset.TeamsRow[] teams5 =
								row.GetTeamsRowsByCompetitorsTeams5();
							foreach (DatabaseDataset.TeamsRow team in teams5)
							{
								team.SetCompetitorId5Null();
							}
							MyInterface.updatedTeam();
						}
						row.Delete();
						if (patrolId != -1)
							checkForPatrolClassUpdate(patrolId, updateGui);
					}
				}
				Trace.WriteLine("CDatabase: delCompetitor() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			if (updateGui)
				MyInterface.updatedCompetitor(competitor);
			return;
		}
		internal void delWeapon(Structs.Weapon weapon)
		{
			Trace.WriteLine("CDatabase: Entering delWeapon(" + 
				weapon.ToString() + ")");

			Trace.WriteLine("CDatabase: delWeapon() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: delWeapon() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				foreach(DatabaseDataset.WeaponsRow row in Database.Weapons.Select(
					"WeaponId='" + weapon.WeaponId + "'"))
				{
					if(row.WeaponId == weapon.WeaponId)
					{
						row.Delete();
					}
				}

				Trace.WriteLine("CDatabase: delWeapon() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			MyInterface.updatedWeapon();
			return;
		}
		internal void delCompetition(Structs.Competition comp)
		{
			Trace.WriteLine("CDatabase: Entering delCompetition(" + 
				comp.ToString() + ")");

			Trace.WriteLine("CDatabase: delCompetition() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: delCompetition() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				foreach(DatabaseDataset.CompetitionRow row in Database.Competition
					.Select("CompetitionId=" + comp.CompetitionId.ToString()))
				{
					if(row.CompetitionId == comp.CompetitionId)
					{
						row.Delete();
					}
				}

				Trace.WriteLine("CDatabase: delCompetition() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			MyInterface.updatedCompetition();
			return;
		}
		internal void delPatrol(Structs.Patrol patrol, bool updateGui)
		{
			Trace.WriteLine("CDatabase: Entering delPatrol(" + 
				patrol.ToString() + ")");

			// TODO Remove all shooters that depend on this patrol

			Trace.WriteLine("CDatabase: delPatrol() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: delPatrol() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				foreach(DatabaseDataset.PatrolsRow row in Database.Patrols.Select(
					"PatrolId=" + patrol.PatrolId.ToString()))
				{
					if(row.PatrolId == patrol.PatrolId)
					{
						//database.Patrols.RemovePatrolsRow(row);
						row.Delete();
					}
				}
				Trace.WriteLine("CDatabase: delPatrol() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			if (updateGui)
				MyInterface.updatedPatrol();
			return;
		}
		internal void delCompetitorResult(Structs.CompetitorResult res)
		{
			Trace.WriteLine("CDatabase: Entering delCompetitorResult(" + 
				res.ToString() + ")");

			Trace.WriteLine("CDatabase: delCompetitorResult() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: delCompetitorResult() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				foreach(DatabaseDataset.CompetitorResultsRow row in 
					Database.CompetitorResults.Select(
					"ResultId=" + res.ResultId.ToString()))
				{
					if(row.ResultId == res.ResultId)
					{
						row.Delete();
					}
				}

				Trace.WriteLine("CDatabase: delCompetitorResult() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			MyInterface.updatedCompetitorResult(res);
			return;
		}
		internal void delStation(Structs.Station station)
		{
			delStation(station, false);
		}
		internal void delStation(Structs.Station station, bool forcibleDelete)
		{
			Trace.WriteLine("CDatabase: Entering delStation(" + 
				station.ToString() + ")");

			Trace.WriteLine("CDatabase: delStation() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: delStation() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				string filter = "StationId=" + station.StationId.ToString();
				DatabaseDataset.CompetitorResultsRow[] compRows = 
					(DatabaseDataset.CompetitorResultsRow[])
					Database.CompetitorResults.Select(filter);

				if (forcibleDelete)
				{
					if (compRows.Length == 0)
					{
						foreach(DatabaseDataset.CompetitorResultsRow tstRow in
							Database.CompetitorResults.Rows)
						{
							if (tstRow.RowState != System.Data.DataRowState.Deleted)
								Trace.WriteLine("StationId: " + tstRow.StationId);
							else
								tstRow.Delete();
						}
					}
					foreach(DatabaseDataset.CompetitorResultsRow compRow in
						compRows)
					{
						compRow.Delete();
					}
					MyInterface.updatedCompetitorResult(new Structs.CompetitorResult());
				}
				else
				{
					if (compRows.Length != 0)
					{
						throw new ApplicationException("There is childrows in table CompetitorResultsRow");
					}
				}

				foreach(DatabaseDataset.StationsRow row in 
					Database.Stations.Select(
					"StationId=" + station.StationId.ToString()))
				{
					if(row.StationId == station.StationId)
					{
						row.Delete();
					}
				}

				Trace.WriteLine("CDatabase: delStation() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			MyInterface.updatedStation();
			return;
		}
		internal void delTeam(Structs.Team team)
		{
			Trace.WriteLine("CDatabase: Entering delTeam(" + 
				team.TeamId.ToString() + ")");

			Trace.WriteLine("CDatabase: delStation() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: delTeam() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				foreach(DatabaseDataset.TeamsRow row in 
					Database.Teams.Select(
					"TeamId=" + team.TeamId.ToString()))
				{
					if(row.TeamId == team.TeamId)
					{
						row.Delete();
					}
				}

				Trace.WriteLine("CDatabase: delTeam() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}
			MyInterface.updatedTeam();
			return;
		}
		#endregion









		internal void checkForPatrolClassUpdate(int patrolId, bool updateGui)
		{
			if (patrolId < 1)
				return;

			Structs.Patrol patrol;

			Trace.WriteLine("CDatabase: checkForPatrolClassUpdate() " + 
				" locking \"DatabaseLocker\" on thread \"" +
				Thread.CurrentThread.Name + "\" ( " +
				System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

			lock(_databaseLocker)
			{
				Trace.WriteLine("CDatabase: checkForPatrolClassUpdate() " + 
					" locked \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

				patrol = MyInterface.GetPatrol(patrolId);
				int count = MyInterface.GetCompetitorsCountPatrol(patrol);

				if ( count == 0 )
				{
					// set patrol class to unknown
					patrol.PClass = Structs.PatrolClass.Ok�nd;
					MyInterface.UpdatePatrol(patrol);
					return;
				}

				if ( count > 1 )
				{
					// Obviously the change wont matter
					return;
				}

				// Ok, there is exacly one competitor in patrol.
				// Set current patrol class to same as competitors weapon.
				Structs.Competitor competitor = MyInterface.GetCompetitors(patrol)[0];
				Structs.Weapon weapon = MyInterface.GetWeapon(competitor.WeaponId);
				patrol.PClass = MyInterface.ConvertWeaponsClassToPatrolClass(
					weapon.WClass);

				Trace.WriteLine("CDatabase: checkForPatrolClassUpdate() " + 
					" unlocking \"DatabaseLocker\" on thread \"" +
					Thread.CurrentThread.Name + "\" ( " +
					System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");
			}

			MyInterface.UpdatePatrol(patrol, updateGui);
		}

	}
}
