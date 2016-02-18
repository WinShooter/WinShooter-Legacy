// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CFileImport.cs" company="John Allberg">
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
//   Summary description for CFileImport.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading;

    using Allberg.Shooter.Common.Exceptions;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for CFileImport.
    /// </summary>
    [Serializable]
    internal class CFileImport
    {
        private enum ViewTableColumnNames
        {
            Skyttekort,
            Klubb,
            Fornamn,
            Efternamn,
            Klass,
            Patrull,
            Bana,
            Vapen,
            Kontrollera,
            Epost
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CFileImport"/> class.
        /// </summary>
        /// <param name="callerInterface">
        /// The caller interface.
        /// </param>
        internal CFileImport(Interface callerInterface)
        {
            this.myInterface = callerInterface;
        }

        private readonly Interface myInterface;

        /// <summary>
        /// The view datatable.
        /// </summary>
        internal DataTable ViewDatatable { get; set; }

        /// <summary>
        /// The view datatable check.
        /// </summary>
        internal DataTable ViewDatatableCheck { get; set; }

        /// <summary>
        /// The convert import file columns name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="Interface.ImportFileColumns"/>.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        private Interface.ImportFileColumns ConvertImportFileColumnsName(string name)
        {
            for (int i = 0; i < 20; i++)
            {
                var col = (Interface.ImportFileColumns)i;

                if (col.ToString() == name)
                {
                    return col;
                }
            }

            throw new ApplicationException("Cannot find Common.Interface.ImportFileColumns." + name +".");
        }

        private void CreateViewDatatable(SortedList columnOrder)
        {
            this.ViewDatatable = new DataTable();

            for (int i = 0; i < columnOrder.Count; i++)
            {
                ViewTableColumnNames viewColumnName;
                var temp = (string)columnOrder[columnOrder.IndexOfValue(i)];
                var dataColumnName = this.ConvertImportFileColumnsName(temp);

                switch(dataColumnName)
                {
                    case Interface.ImportFileColumns.ClubId:
                        viewColumnName = ViewTableColumnNames.Klubb;
                        break;
                    case Interface.ImportFileColumns.Email:
                        viewColumnName = ViewTableColumnNames.Epost;
                        break;
                    case Interface.ImportFileColumns.Givenname:
                        viewColumnName = ViewTableColumnNames.Fornamn;
                        break;
                    case Interface.ImportFileColumns.Lane:
                        viewColumnName = ViewTableColumnNames.Bana;
                        break;
                    case Interface.ImportFileColumns.Patrol:
                        viewColumnName = ViewTableColumnNames.Patrull;
                        break;
                    case Interface.ImportFileColumns.ShooterClass:
                        viewColumnName = ViewTableColumnNames.Klass;
                        break;
                    case Interface.ImportFileColumns.ShooterId:
                        viewColumnName = ViewTableColumnNames.Skyttekort;
                        break;
                    case Interface.ImportFileColumns.Surname:
                        viewColumnName = ViewTableColumnNames.Efternamn;
                        break;
                    case Interface.ImportFileColumns.WeaponId:
                        viewColumnName = ViewTableColumnNames.Vapen;
                        break;
                    default:
                        throw new ApplicationException("Unknown datacolumn");
                }
                try
                {
                    this.ViewDatatable.Columns.Add(viewColumnName.ToString(), typeof(string));
                }
                catch (Exception)
                {
                }
            }
        }

        internal string[] LoadFile(string filePath, Interface.ImportFileType fileType, SortedList columnOrder)
        {
            this.CreateViewDatatable(columnOrder);

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Write);

            var output = new StringBuilder { Length = 0 };

            Encoding encoding = Encoding.GetEncoding(1252);
            StreamReader r = new StreamReader(fs, encoding);
            r.BaseStream.Seek(0, SeekOrigin.Begin);

            while (r.Peek() > -1) 
            {
                string temp = r.ReadLine();
                if (temp.Replace(";", string.Empty).Replace(" ", string.Empty).Length != 0)
                {
                    output.Append(temp + "\n");
                }
            }

            r.Close();

            switch (fileType)
            {
                case Interface.ImportFileType.SKV:
                    this.parseSkvFile(output.ToString(), columnOrder);
                    break;
                default:
                    throw new ApplicationException("Not implemented");
            }

            return output.ToString().Replace("\r", string.Empty).Split('\n');
        }

        internal bool ignorePatrolErrors = false;
        internal bool ignoreLaneErrors = false;
        internal bool ignoreShooterErrors = false;

        Thread workerThread;
        
        internal void ValidateDataset()
        {
            ViewDatatableCheck = this.ViewDatatable.Copy();
            ViewDatatableCheck.Columns.Add(
                ViewTableColumnNames.Kontrollera.ToString());

            workerThread = 
                new System.Threading.Thread(
                new System.Threading.ThreadStart(this.ValidateDatasetThread));
            workerThread.Name = "CFileImport:workerThread";

            workerThread.Start();
        }

        internal void ValidateDatasetThread()
        {
            int maxRows = ViewDatatableCheck.Rows.Count;
            for(int i=0;i<maxRows;i++)
            {
                myInterface.updatedFileImportCount(
                    i,ViewDatatableCheck.Rows.Count);

                DataRow row = ViewDatatableCheck.Rows[i];

                row[ViewTableColumnNames.Kontrollera.ToString()] = string.Empty; 
                string[] resultStrings = new string[5];
                resultStrings[0] = checkRowClub(row);
                resultStrings[1] = checkRowWeapon(row);
                resultStrings[2] = checkRowShooter(row);
                resultStrings[3] = checkRowPatrol(row);
                resultStrings[4] = checkRowLane(row);

                bool errorAlreadyFound = false;
                string resultStringTot = string.Empty;
                foreach(string resultString in resultStrings)
                {
                    if (resultString != string.Empty & resultString != null)
                    {
                        if (errorAlreadyFound)
                        {
                            resultStringTot  += ", ";
                        }
                        resultStringTot += resultString;
                        errorAlreadyFound = true;
                    }
                }

                if (resultStringTot != string.Empty)
                {
                    // Some error occured
                    row[ViewTableColumnNames.Kontrollera.ToString()] = resultStringTot;
                }
                else
                {
                    // Ok with this row, remove from table
                    ViewDatatableCheck.Rows.Remove(row);
                    maxRows--;
                    i--;
                }
            }

            myInterface.updatedFileImportCount(
                ViewDatatableCheck.Rows.Count,
                ViewDatatableCheck.Rows.Count);
        }

        private void parseSkvFile(string fileContent, SortedList columnOrder)
        {
            this.ViewDatatable.Rows.Clear();
            fileContent = fileContent.Replace("\r", string.Empty);
            string[] lines = fileContent.Split('\n');

            foreach(string line in lines)
            {
                System.Data.DataRow row = ViewDatatable.NewRow();
                string[] thisLine = line.Split(';');
                
                foreach(string columnName in columnOrder.Keys)
                {
                    try
                    {
                        int thisColIndex = (int)columnOrder[columnName];
                        string column = thisLine[thisColIndex].Trim();
                        int colNr = (int)columnOrder[columnName];
                        row[colNr] = column;
                    }
                    catch(System.IndexOutOfRangeException)
                    {
                    }
                }

                if (thisLine.Length>=3)
                    this.ViewDatatable.Rows.Add(row);
            }
        }

        private string checkRowClub(DataRow row)
        {
            if (row.IsNull(ViewTableColumnNames.Klubb.ToString()))
                return "Klubb saknas i importfilen";

            string clubId = (string)row[ViewTableColumnNames.Klubb.ToString()];
            try
            {
                myInterface.GetClub(clubId);
            }
            catch(CannotFindIdException)
            {
                return "Kan inte hitta klubben i databasen";
            }
            return string.Empty;
        }
        private string checkRowWeapon(DataRow row)
        {
            if (row.IsNull(ViewTableColumnNames.Vapen.ToString()))
                return "VapenID saknas i importfilen";
            if ((string)row[ViewTableColumnNames.Vapen.ToString()] == string.Empty)
                return "VapenID saknas i importfilen";

            string weaponId = (string)row[ViewTableColumnNames.Vapen.ToString()];
            Structs.Weapon thisWeapon;
            try
            {
                thisWeapon = myInterface.GetWeapon(weaponId);
                if (myInterface.CompetitionCurrent.Type == Structs.CompetitionTypeEnum.MagnumField)
                {
                    // Check that this is a magnum.
                    try { 
                        CConvert.ConvertWeaponsClassToResultClass(
                            thisWeapon.WClass, 
                            Structs.CompetitionTypeEnum.MagnumField);
                    }
                    catch (Exception)
                    {
                        return "VapenID är inte ett Magnum-vapen";
                    }
                }
            }
            catch(CannotFindIdException)
            {
                return "VapenID " + weaponId + " saknas i databasen";
            }
            return string.Empty;
        }

        private string checkRowShooter(DataRow row)
        {
            if (row.IsNull(ViewTableColumnNames.Skyttekort.ToString()))
                return "Skyttekortsnr saknas i importfilen";

            string shooterCard = (string)row[ViewTableColumnNames.Skyttekort.ToString().Trim()];
            //int shooterId = -1;

            if (shooterCard == string.Empty)
                return string.Empty;

            try
            {
                int.Parse(shooterCard);
            }
            catch (System.FormatException)
            {
                return "Skyttekortet får endast innehålla siffror";
            }

            return string.Empty;
        }
        private string checkRowPatrol(DataRow row)
        {
            if (row.IsNull(ViewTableColumnNames.Patrull.ToString()))
                return string.Empty; // Patrol will be set later via GUI

            string patrullString = (string)row[ViewTableColumnNames.Patrull.ToString()];
            try
            {
                int patrolId = int.Parse(patrullString);
                myInterface.GetPatrol(patrolId);
            }
            catch(System.FormatException)
            {
                return "Patrullnummer var felaktigt: " + patrullString;
            }
            catch (CannotFindIdException)
            {
                myInterface.PatrolAddEmpty();
                return checkRowPatrol(row);
            }
            return string.Empty;
        }

        private string checkRowLane(DataRow row)
        {
            if (row.IsNull(ViewTableColumnNames.Bana.ToString())
                | row.IsNull(ViewTableColumnNames.Patrull.ToString()))
                return string.Empty; // Patrol will be set later via GUI

            if (checkRowPatrol(row) != string.Empty)
                return "Bana kunde inte kontrolleras på grund av fel i patrullen.";

            string laneString = (string)row[ViewTableColumnNames.Bana.ToString()];
            string patrullString = (string)row[ViewTableColumnNames.Patrull.ToString()];

            try
            {
                int patrolId = int.Parse(patrullString);
                Structs.Competitor[] competitors =
                    myInterface.GetCompetitors(myInterface.GetPatrol(patrolId));
                
                int laneId = int.Parse(laneString);

                foreach (Structs.Competitor competitor in competitors)
                {
                    if (competitor.Lane == laneId)
                        return "Bana " + laneId.ToString() + " i patrull " +
                            patrolId.ToString() + " är redan upptagen.";
                }

            }
            catch(System.FormatException)
            {
                return "Patrullnummer var felaktigt: " + laneString;
            }
            catch (CannotFindIdException)
            {
                return "Patrullen " + laneString + " saknas";
            }
            return string.Empty;
        }


        internal void ImportDataset()
        {
            workerThread = 
                new System.Threading.Thread(
                new System.Threading.ThreadStart(this.ImportDatasetThread));
            workerThread.Start();
        }
        private void ImportDatasetThread()
        {
            Trace.WriteLine("CFileImport: ImportDatasetThread started " +
                "on thread \"" + Thread.CurrentThread.Name + "\" ( " +
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            int i = -1;
            foreach(DataRow row in this.ViewDatatable.Rows)
            {
                i++;
                string classString = null;

                Trace.WriteLine("CFileImport: ImportDatasetThread calling " +
                    "updatedFileImportCount(" + i.ToString() + ", " +
                    ViewDatatable.Rows.Count.ToString() + ")");
                myInterface.updatedFileImportCount(
                    i,ViewDatatable.Rows.Count);

                // Get shooter
                string shooterCard = (string)row[ViewTableColumnNames.Skyttekort.ToString().Trim()];

                // Get shooter from DB
                Structs.Shooter shooter;
                try
                {
                    shooter = myInterface.GetShooter(shooterCard);

                    classString = (string)row[ViewTableColumnNames.Klass.ToString()];
                    if (classString.IndexOf("1") >= 0)
                        shooter.Class = Structs.ShootersClass.Klass1;
                    if (classString.IndexOf("2") >= 0)
                        shooter.Class = Structs.ShootersClass.Klass2;
                    if (classString.IndexOf("3") >= 0)
                        shooter.Class = Structs.ShootersClass.Klass3;

                    myInterface.UpdateShooter(shooter);
                }
                catch (CannotFindIdException)
                {
                    // Add user to DB with values from file.
                    shooter = new Structs.Shooter();
                    // What class?
                    classString = (string)row[ViewTableColumnNames.Klass.ToString()];
                    Structs.ShootersClass sclass = Structs.ShootersClass.Klass1;
                    if (classString.IndexOf("1") >= 0)
                        sclass = Structs.ShootersClass.Klass1;
                    if (classString.IndexOf("2") >= 0)
                        sclass = Structs.ShootersClass.Klass2;
                    if (classString.IndexOf("3") >= 0)
                        sclass = Structs.ShootersClass.Klass3;

                    if (shooterCard == string.Empty)
                    {
                        try
                        {
                            Random rnd = new Random();
                            while (true)
                            {
                                shooterCard = (-rnd.Next(100000)).ToString();
                                myInterface.GetShooter(shooterCard);
                            }
                        }
                        catch (CannotFindIdException)
                        {
                        }
                    }
                    // Create shooter and insert into database
                    shooter.Automatic = false;
                    shooter.CardNr = shooterCard.ToString();
                    shooter.Class = sclass;
                    shooter.ClubId = ((string)row[ViewTableColumnNames.Klubb.ToString()]).Trim();
                    shooter.Email = ((string)row[ViewTableColumnNames.Epost.ToString()]).Trim();
                    shooter.Givenname = ((string)row[ViewTableColumnNames.Efternamn.ToString()]).Trim();
                    shooter.Surname = ((string)row[ViewTableColumnNames.Fornamn.ToString()]).Trim();
                    shooter.Payed = 0;
                    shooter.ToAutomatic = false;

                    shooter.ShooterId = myInterface.NewShooter(shooter, false);
                    if (shooter.ShooterId < 0)
                        throw new ApplicationException("ShooterId is " + shooter.ShooterId);
                }

                // Ok, shooter is done. Create competitor
                classString = (string)row[ViewTableColumnNames.Klass.ToString().ToUpper()];
                Structs.ShootersClass cclass = shooter.Class;
                if (classString.IndexOf("D")>=0)
                {
                    cclass = (Structs.ShootersClass)(
                        (int)Structs.ShootersClass.Damklass1 - 1 +
                        (int)cclass);
                }
                if (classString.IndexOf("J")>=0)
                {
                    cclass = Structs.ShootersClass.Juniorklass;
                }
                if (classString.IndexOf("VY")>=0)
                {
                    cclass = Structs.ShootersClass.VeteranklassYngre;
                }
                if (classString.IndexOf("VÄ")>=0)
                {
                    cclass = Structs.ShootersClass.VeteranklassÄldre;
                }
                if (classString.IndexOf("Ö")>=0)
                {
                    cclass = Structs.ShootersClass.Öppen;
                }

                // Phu, class is done. Continue with competitor
                Structs.Competitor competitor = new Structs.Competitor();
                competitor.CompetitionId = myInterface.GetCompetitions()[0].CompetitionId;
                competitor.ShooterId = shooter.ShooterId;
                competitor.WeaponId = (string)row[ViewTableColumnNames.Vapen.ToString()];
                competitor.PatrolId = -1;
                competitor.Lane = -1;
                competitor.ShooterClass = cclass;

                if (!row.IsNull(ViewTableColumnNames.Patrull.ToString()) &&
                    ((string)row[ViewTableColumnNames.Patrull.ToString()]).Trim() != string.Empty)
                {
                    try
                    {
                        // Patrol defined in importfile
                        string patrolString = (string)row[ViewTableColumnNames.Patrull.ToString()];
                        int patrolId = -1;
                        patrolId = int.Parse(patrolString);
                        while (patrolId > myInterface.GetPatrolsCount())
                        {
                            myInterface.PatrolAddEmpty(false);
                        }
                        competitor.PatrolId = patrolId;
                        string laneString = null;
                        if (!row.IsNull(ViewTableColumnNames.Bana.ToString()))
                            laneString = (string)row[ViewTableColumnNames.Bana.ToString()];

                        if (laneString != null &&
                            laneString.Trim() != string.Empty)
                        {
                            competitor.Lane = int.Parse(laneString);
                        }
                        else
                        {
                            competitor.Lane = myInterface.PatrolGetNextLane(patrolId);
                        }
                    }
                    catch (System.FormatException)
                    {
                        // If this occurres, just ignore. It really shouldn't, since
                        // it has already been checked
                    }
                }

                myInterface.NewCompetitor(competitor, false);
            }

            myInterface.updatedFileImportCount(
                ViewDatatable.Rows.Count,
                ViewDatatable.Rows.Count);
            myInterface.updatedShooter(new Structs.Shooter());
            myInterface.updatedCompetitor(new Structs.Competitor());
            myInterface.updatedPatrol();
        }
    }
}
