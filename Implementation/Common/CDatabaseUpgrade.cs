// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CDatabaseUpgrade.cs" company="John Allberg">
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// The c database upgrade.
    /// </summary>
    internal class CDatabaseUpgrade
    {
        /// <summary>
        /// The database connection.
        /// </summary>
        private readonly OleDbConnection dbconn;

        /// <summary>
        /// Initializes a new instance of the <see cref="CDatabaseUpgrade"/> class.
        /// </summary>
        /// <param name="databaseConnection">
        /// The database connection.
        /// </param>
        internal CDatabaseUpgrade(OleDbConnection databaseConnection)
        {
            this.dbconn = databaseConnection;
        }

        /// <summary>
        /// The upgrade.
        /// </summary>
        [SuppressMessage("Microsoft.Security", 
            "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        internal void Upgrade()
        {
            Version version = new Version("1.0.0");

            try
            {
                OleDbCommand cmd = new OleDbCommand("select * from DbInfo", this.dbconn);
                OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

                while (reader.Read())
                {
                    if (reader.GetString(reader.GetOrdinal("KeyName")) == "Version")
                    {
                        version = new Version(reader.GetString(reader.GetOrdinal("KeyValue")));
                    }
                }

                // version = new Version("1.1.0");
                reader.Close();
            }
            catch (OleDbException exc)
            {
                // Older version than 1.1
                if (exc.Message.IndexOf("cannot find the input table") == -1)
                {
                    Trace.WriteLine("Ops, unknown OleDbException while checking for db upgrade:\r\n" +
                        exc.ToString());
                }
            }

            if (version < new Version("1.1.0"))
            {
                this.UpgradeVersion1_1();
            }

            if (version < new Version("1.2.0"))
            {
                this.UpgradeVersion1_2();
            }


            if (version < new Version("1.4.0"))
            {
                this.UpgradeVersion1_4();
            }

            if (version < new Version("1.5.6"))
            {
                this.UpgradeVersion1_5_6();
            }

            if (version < new Version("1.6.0"))
            {
                this.UpgradeVersion1_6_0();
            }

            if (version < new Version("1.6.2"))
            {
                this.UpgradeVersion1_6_2();
            }

            // Finished upgrading database
            OleDbCommand sqlCmd = null;
            try
            {
                if (new Version(CDatabase.CurrentDbVersion) > version)
                {
                    sqlCmd = new OleDbCommand(
                        "update DbInfo set KeyValue='" + 
                            CDatabase.CurrentDbVersion + 
                            "' where KeyName='Version'", 
                            this.dbconn);
                    int affected = sqlCmd.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        sqlCmd = new OleDbCommand(
                            "insert into DbInfo (KeyName, KeyValue) values ('Version', '" + 
                                CDatabase.CurrentDbVersion + "')", 
                            this.dbconn);
                        affected = sqlCmd.ExecuteNonQuery();
                        sqlCmd.Dispose();
                    }
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLine("Exception while updating/inserting the version value of the DbInfo table:" +
                    exc);
                throw;
            }
            finally
            {
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                }
            }
        }

        #region Upgraders
        #region Version 1.1.0

        /// <summary>
        /// The upgrade version 1_1.
        /// </summary>
        private void UpgradeVersion1_1()
        {
            if (CheckTableExist("Teams", this.dbconn))
            {
                DropTable("Teams", this.dbconn);
            }

            // Create table Teams
            CDatabase.CreateTable(new DatabaseDataset.TeamsDataTable(), this.dbconn);

            // Create restrictions for table Teams
            var dbtemp = new DatabaseDataset();
            foreach (DataRelation rel in dbtemp.Relations)
            {
                if (rel.RelationName.IndexOf("Teams") > -1)
                {
                    var sqlCreate = "ALTER TABLE " + rel.ChildTable.TableName + " ADD " + "CONSTRAINT " + rel.RelationName
                                       + " FOREIGN KEY " + "(" + rel.ChildColumns[0].ColumnName + ")" + " REFERENCES "
                                       + rel.ParentTable + " (" + rel.ParentColumns[0].ColumnName + ")";

                    // Execute against database
                    Trace.WriteLine("CDatabase: Running SQL to create relation: " + sqlCreate);
                    var sql = new OleDbCommand(sqlCreate, this.dbconn);
                    sql.ExecuteNonQuery();
                    sql.Dispose();
                }
            }

            // Create table DbInfo
            CDatabase.CreateTable(new DatabaseDataset.DbInfoDataTable(), this.dbconn);

            // Change shooter table to include "arrived" column
            AddColumnToTable("shooters", "Arrived", "bit null", this.dbconn);
            AddColumnToTable("shooters", "EmailResult", "bit null", this.dbconn);
        }

        #endregion
        #region Version 1.2.0

        /// <summary>
        /// The upgrade version 1_2.
        /// </summary>
        private void UpgradeVersion1_2()
        {
            OleDbCommand sqlCmd = null;
            try
            {
                // Add competition type
                AddColumnToTable("competition", "Type", "int null", this.dbconn);
                string sqlUpdate = "update competition set Type=0";

                Trace.WriteLine("CDatabase: Running SQL to set competition type: " + sqlUpdate);
                sqlCmd = new OleDbCommand(sqlUpdate, this.dbconn);
                sqlCmd.ExecuteNonQuery();

                // Add stationId to table CompetitorResults
                AddColumnToTable("CompetitorResults", "StationId", "int null", this.dbconn);
                sqlUpdate = "update CompetitorResults set StationId=StationNr";

                Trace.WriteLine("CDatabase: Running SQL to set StationId=StationNr: " + sqlUpdate);
                sqlCmd = new OleDbCommand(sqlUpdate, this.dbconn);
                sqlCmd.ExecuteNonQuery();

                AddConstraint("StationsCompetitorResults", new DatabaseDataset(), this.dbconn);
            }
            finally
            {
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                }
            }
        }

        #endregion
        #region Version 1.4.0

        /// <summary>
        /// The upgrade version 1_4.
        /// </summary>
        private void UpgradeVersion1_4()
        {
            // Add competition type
            AddColumnToTable("Teams", "CompetitorId5", "int null", this.dbconn);
            AddConstraint("CompetitorsTeams5", new DatabaseDataset(), this.dbconn);

            AddColumnToTable("Competition", "Championship", "int null", this.dbconn);
            const string SqlUpdate = "update competition set Championship=0";

            Trace.WriteLine("CDatabase: Running SQL to set competition Championship: " + SqlUpdate);
            var sqlCmd = new OleDbCommand(SqlUpdate, this.dbconn);
            sqlCmd.ExecuteNonQuery();
        }

        #endregion
        #region Version 1.5.6

        /// <summary>
        /// The upgrade version 1_5_6.
        /// </summary>
        private void UpgradeVersion1_5_6()
        {
            AddColumnToTable("Competition", "PatrolConnectionType", "int null", this.dbconn);
            const string SqlUpdate = "update competition set PatrolConnectionType=1";

            Trace.WriteLine("CDatabase: Running SQL to set competition patrolConnectionType: " + SqlUpdate);
            var sqlCmd = new OleDbCommand(SqlUpdate, this.dbconn);
            sqlCmd.ExecuteNonQuery();
        }

        #endregion
        #region Version 1.6.0

        /// <summary>
        /// The upgrade version 1_6_0.
        /// </summary>
        private void UpgradeVersion1_6_0()
        {
            string sqlUpdate;
            OleDbCommand sqlCmd;

            // Add shooterFee
            for (int i = 1; i <= 4; i++)
            {
                AddColumnToTable("Competition", "ShooterFee" + i, "int null", this.dbconn);

                sqlUpdate = "update competition set ShooterFee" + i + "=ShooterFee";
                Trace.WriteLine("CDatabase: Running SQL to set competition shooterFee: " + sqlUpdate);
                sqlCmd = new OleDbCommand(sqlUpdate, this.dbconn);
                sqlCmd.ExecuteNonQuery();
            }

            // Add lastupdate
            AddColumnToTable("Shooters", "LastUpdate", "DateTime null", this.dbconn);
            AddColumnToTable("Clubs", "LastUpdate", "DateTime null", this.dbconn);
            AddColumnToTable("Weapons", "LastUpdate", "DateTime null", this.dbconn);

            sqlUpdate = "update shooters set LastUpdate='2004-01-01 10:00:00'";
            Trace.WriteLine("CDatabase: Running SQL to set shooters lastupdate: " + sqlUpdate);
            sqlCmd = new OleDbCommand(sqlUpdate, this.dbconn);
            sqlCmd.ExecuteNonQuery();

            sqlUpdate = "update clubs set LastUpdate='2004-01-01 10:00:00'";
            Trace.WriteLine("CDatabase: Running SQL to set clubs lastupdate: " + sqlUpdate);
            sqlCmd = new OleDbCommand(sqlUpdate, this.dbconn);
            sqlCmd.ExecuteNonQuery();

            sqlUpdate = "update weapons set LastUpdate='2004-01-01 10:00:00'";
            Trace.WriteLine("CDatabase: Running SQL to set weapons lastupdate: " + sqlUpdate);
            sqlCmd = new OleDbCommand(sqlUpdate, this.dbconn);
            sqlCmd.ExecuteNonQuery();

            // Add one to competition championship type
            sqlUpdate = "update competition set Championship=Championship+1";
            Trace.WriteLine("CDatabase: Running SQL to update ChampionShipType: " + sqlUpdate);
            sqlCmd = new OleDbCommand(sqlUpdate, this.dbconn);
            sqlCmd.ExecuteNonQuery();
        }

        #endregion
        #region 1.6.2

        /// <summary>
        /// The upgrade version 1_6_2.
        /// </summary>
        private void UpgradeVersion1_6_2()
        {
            AddColumnToTable("Stations", "Distinguish", "bit null", this.dbconn);
            AddColumnToTable("Competition", "OneClass", "bit null", this.dbconn);

            // Set Distinguish=0
            var sqlUpdate = "update Stations set Distinguish=0";
            Trace.WriteLine("CDatabase: Running SQL to update Stations: " + sqlUpdate);
            var sqlCmd = new OleDbCommand(sqlUpdate, this.dbconn);
            sqlCmd.ExecuteNonQuery();

            // Set OneClass=0
            sqlUpdate = "update Competition set OneClass=0";
            Trace.WriteLine("CDatabase: Running SQL to update Competitions: " + sqlUpdate);
            sqlCmd = new OleDbCommand(sqlUpdate, this.dbconn);
            sqlCmd.ExecuteNonQuery();

        }

        #endregion
        #endregion

        #region Helpers

        /// <summary>
        /// The check table exist.
        /// </summary>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <param name="dbconn">
        /// The database connection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool CheckTableExist(string tableName, OleDbConnection dbconn)
        {
            try
            {
                var cmd = new OleDbCommand("select * from " + tableName, dbconn);
                var reader = cmd.ExecuteReader(CommandBehavior.Default);
                reader.Close();
                return true;
            }
            catch (OleDbException exc)
            {
                // Older version than 1.1
                if (exc.Message.IndexOf("cannot find the input table") == -1)
                {
                    Trace.WriteLine("Ops, unknown System.Data.OleDb.OleDbException while checking for db upgrade:\r\n" +
                        exc);
                }

                return false;
            }
        }

        /// <summary>
        /// The drop table.
        /// </summary>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <param name="dbconn">
        /// The dbconn.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool DropTable(string tableName, OleDbConnection dbconn)
        {
            try
            {
                var cmd = new OleDbCommand("drop table " + tableName, dbconn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (OleDbException exc)
            {
                // Older version than 1.1
                if (exc.Message.IndexOf("cannot find the input table") == -1)
                {
                    Trace.WriteLine("Ops, unknown System.Data.OleDb.OleDbException while checking for db upgrade:\r\n" +
                        exc);
                }

                return false;
            }
        }

        /// <summary>
        /// The add column to table.
        /// </summary>
        /// <param name="tablename">
        /// The table name.
        /// </param>
        /// <param name="columnname">
        /// The column name.
        /// </param>
        /// <param name="columntype">
        /// The column type.
        /// </param>
        /// <param name="dbconn">
        /// The database connection.
        /// </param>
        private static void AddColumnToTable(
            string tablename, 
            string columnname, 
            string columntype, 
            OleDbConnection dbconn)
        {
            try
            {
                var sqlCreate = "ALTER TABLE " + tablename + " " +
                    "ADD " + columnname + " " + columntype;

                // Execute against database
                Trace.WriteLine("CDatabase: Running SQL to add column \"" +
                    columnname + "\" to table \"" + tablename + "\": " + sqlCreate);
                var sql = new OleDbCommand(sqlCreate, dbconn);
                sql.ExecuteNonQuery();
                sql.Dispose();
            }
            catch (OleDbException exc)
            {
                if (exc.Message.IndexOf("already exists in table") == -1)
                {
                    throw exc;
                }
            }
        }

        /// <summary>
        /// The add constraint.
        /// </summary>
        /// <param name="constraintName">
        /// The constraint name.
        /// </param>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="conn">
        /// The conn.
        /// </param>
        private static void AddConstraint(string constraintName, DatabaseDataset database, OleDbConnection conn)
        {
            Trace.WriteLine("CDatabase: Entering createDatabaseConstraints()");

            foreach (DataRelation rel in database.Relations)
            {
                if (rel.RelationName == constraintName)
                {
                    var sqlCreate = new StringBuilder();
                    sqlCreate.Append("ALTER TABLE " + rel.ChildTable.TableName + " ADD " +
                        "CONSTRAINT " + rel.RelationName + " FOREIGN KEY " +
                        "(" + rel.ChildColumns[0].ColumnName + ")" +
                        " REFERENCES " + rel.ParentTable +
                        " (" + rel.ParentColumns[0].ColumnName +
                        ")");

                    // Execute against database
                    Trace.WriteLine("CDatabase: Running SQL to create relation: " + sqlCreate);
                    var sql = new OleDbCommand(sqlCreate.ToString(), conn);

                    sql.ExecuteNonQuery();
                    sql.Dispose();
                }
            }
        }
        #endregion
    }
}
