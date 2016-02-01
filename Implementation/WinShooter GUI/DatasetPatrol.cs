// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatasetPatrol.cs" company="John Allberg">
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
//   Defines the DatasetPatrolTemp type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows 
{
    using System;
    using System.Data;
    using System.Runtime.Serialization;
    using System.Xml;

    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class DatasetPatrolTemp : DataSet {
        
        private shootersDataTable tableshooters;
        
        private unassignedDataTable tableunassigned;
        
        public DatasetPatrolTemp() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected DatasetPatrolTemp(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["shooters"] != null)) {
                    this.Tables.Add(new shootersDataTable(ds.Tables["shooters"]));
                }
                if ((ds.Tables["unassigned"] != null)) {
                    this.Tables.Add(new unassignedDataTable(ds.Tables["unassigned"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.InitClass();
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public shootersDataTable shooters {
            get {
                return this.tableshooters;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public unassignedDataTable unassigned {
            get {
                return this.tableunassigned;
            }
        }
        
        public override DataSet Clone() {
            DatasetPatrol cln = ((DatasetPatrol)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["shooters"] != null)) {
                this.Tables.Add(new shootersDataTable(ds.Tables["shooters"]));
            }
            if ((ds.Tables["unassigned"] != null)) {
                this.Tables.Add(new unassignedDataTable(ds.Tables["unassigned"]));
            }
            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        internal void InitVars() {
            this.tableshooters = ((shootersDataTable)(this.Tables["shooters"]));
            if ((this.tableshooters != null)) {
                this.tableshooters.InitVars();
            }
            this.tableunassigned = ((unassignedDataTable)(this.Tables["unassigned"]));
            if ((this.tableunassigned != null)) {
                this.tableunassigned.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "DatasetPatrol";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/DatasetPatrol.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableshooters = new shootersDataTable();
            this.Tables.Add(this.tableshooters);
            this.tableunassigned = new unassignedDataTable();
            this.Tables.Add(this.tableunassigned);
        }
        
        private bool ShouldSerializeshooters() {
            return false;
        }
        
        private bool ShouldSerializeunassigned() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void shootersRowChangeEventHandler(object sender, shootersRowChangeEvent e);
        
        public delegate void unassignedRowChangeEventHandler(object sender, unassignedRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class shootersDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnMove;
            
            private DataColumn columncompetitorId;
            
            private DataColumn columnLane;
            
            private DataColumn columnName;
            
            private DataColumn columnClub;
            
            private DataColumn columnWeapon;
            
            private DataColumn columnWeaponClass;
            
            internal shootersDataTable() : 
                    base("shooters") {
                this.InitClass();
            }
            
            internal shootersDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn MoveColumn {
                get {
                    return this.columnMove;
                }
            }
            
            internal DataColumn competitorIdColumn {
                get {
                    return this.columncompetitorId;
                }
            }
            
            internal DataColumn LaneColumn {
                get {
                    return this.columnLane;
                }
            }
            
            internal DataColumn NameColumn {
                get {
                    return this.columnName;
                }
            }
            
            internal DataColumn ClubColumn {
                get {
                    return this.columnClub;
                }
            }
            
            internal DataColumn WeaponColumn {
                get {
                    return this.columnWeapon;
                }
            }
            
            internal DataColumn WeaponClassColumn {
                get {
                    return this.columnWeaponClass;
                }
            }
            
            public shootersRow this[int index] {
                get {
                    return ((shootersRow)(this.Rows[index]));
                }
            }
            
            public event shootersRowChangeEventHandler shootersRowChanged;
            
            public event shootersRowChangeEventHandler shootersRowChanging;
            
            public event shootersRowChangeEventHandler shootersRowDeleted;
            
            public event shootersRowChangeEventHandler shootersRowDeleting;
            
            public void AddshootersRow(shootersRow row) {
                this.Rows.Add(row);
            }
            
            public shootersRow AddshootersRow(bool Move, int competitorId, int Lane, string Name, string Club, string Weapon, string WeaponClass) {
                shootersRow rowshootersRow = ((shootersRow)(this.NewRow()));
                rowshootersRow.ItemArray = new object[] {
                        Move,
                        competitorId,
                        Lane,
                        Name,
                        Club,
                        Weapon,
                        WeaponClass};
                this.Rows.Add(rowshootersRow);
                return rowshootersRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                shootersDataTable cln = ((shootersDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new shootersDataTable();
            }
            
            internal void InitVars() {
                this.columnMove = this.Columns["Move"];
                this.columncompetitorId = this.Columns["competitorId"];
                this.columnLane = this.Columns["Lane"];
                this.columnName = this.Columns["Name"];
                this.columnClub = this.Columns["Club"];
                this.columnWeapon = this.Columns["Weapon"];
                this.columnWeaponClass = this.Columns["WeaponClass"];
            }
            
            private void InitClass() {
                this.columnMove = new DataColumn("Move", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnMove);
                this.columncompetitorId = new DataColumn("competitorId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columncompetitorId);
                this.columnLane = new DataColumn("Lane", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnLane);
                this.columnName = new DataColumn("Name", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnName);
                this.columnClub = new DataColumn("Club", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnClub);
                this.columnWeapon = new DataColumn("Weapon", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnWeapon);
                this.columnWeaponClass = new DataColumn("WeaponClass", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnWeaponClass);
            }
            
            public shootersRow NewshootersRow() {
                return ((shootersRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new shootersRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(shootersRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.shootersRowChanged != null)) {
                    this.shootersRowChanged(this, new shootersRowChangeEvent(((shootersRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.shootersRowChanging != null)) {
                    this.shootersRowChanging(this, new shootersRowChangeEvent(((shootersRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.shootersRowDeleted != null)) {
                    this.shootersRowDeleted(this, new shootersRowChangeEvent(((shootersRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.shootersRowDeleting != null)) {
                    this.shootersRowDeleting(this, new shootersRowChangeEvent(((shootersRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveshootersRow(shootersRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class shootersRow : DataRow {
            
            private shootersDataTable tableshooters;
            
            internal shootersRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableshooters = ((shootersDataTable)(this.Table));
            }
            
            public bool Move {
                get {
                    try {
                        return ((bool)(this[this.tableshooters.MoveColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableshooters.MoveColumn] = value;
                }
            }
            
            public int competitorId {
                get {
                    try {
                        return ((int)(this[this.tableshooters.competitorIdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableshooters.competitorIdColumn] = value;
                }
            }
            
            public int Lane {
                get {
                    try {
                        return ((int)(this[this.tableshooters.LaneColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableshooters.LaneColumn] = value;
                }
            }
            
            public string Name {
                get {
                    try {
                        return ((string)(this[this.tableshooters.NameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableshooters.NameColumn] = value;
                }
            }
            
            public string Club {
                get {
                    try {
                        return ((string)(this[this.tableshooters.ClubColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableshooters.ClubColumn] = value;
                }
            }
            
            public string Weapon {
                get {
                    try {
                        return ((string)(this[this.tableshooters.WeaponColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableshooters.WeaponColumn] = value;
                }
            }
            
            public string WeaponClass {
                get {
                    try {
                        return ((string)(this[this.tableshooters.WeaponClassColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableshooters.WeaponClassColumn] = value;
                }
            }
            
            public bool IsMoveNull() {
                return this.IsNull(this.tableshooters.MoveColumn);
            }
            
            public void SetMoveNull() {
                this[this.tableshooters.MoveColumn] = System.Convert.DBNull;
            }
            
            public bool IscompetitorIdNull() {
                return this.IsNull(this.tableshooters.competitorIdColumn);
            }
            
            public void SetcompetitorIdNull() {
                this[this.tableshooters.competitorIdColumn] = System.Convert.DBNull;
            }
            
            public bool IsLaneNull() {
                return this.IsNull(this.tableshooters.LaneColumn);
            }
            
            public void SetLaneNull() {
                this[this.tableshooters.LaneColumn] = System.Convert.DBNull;
            }
            
            public bool IsNameNull() {
                return this.IsNull(this.tableshooters.NameColumn);
            }
            
            public void SetNameNull() {
                this[this.tableshooters.NameColumn] = System.Convert.DBNull;
            }
            
            public bool IsClubNull() {
                return this.IsNull(this.tableshooters.ClubColumn);
            }
            
            public void SetClubNull() {
                this[this.tableshooters.ClubColumn] = System.Convert.DBNull;
            }
            
            public bool IsWeaponNull() {
                return this.IsNull(this.tableshooters.WeaponColumn);
            }
            
            public void SetWeaponNull() {
                this[this.tableshooters.WeaponColumn] = System.Convert.DBNull;
            }
            
            public bool IsWeaponClassNull() {
                return this.IsNull(this.tableshooters.WeaponClassColumn);
            }
            
            public void SetWeaponClassNull() {
                this[this.tableshooters.WeaponClassColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class shootersRowChangeEvent : EventArgs {
            
            private shootersRow eventRow;
            
            private DataRowAction eventAction;
            
            public shootersRowChangeEvent(shootersRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public shootersRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class unassignedDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnMove;
            
            private DataColumn columncompetitorId;
            
            private DataColumn columnName;
            
            private DataColumn columnClub;
            
            private DataColumn columnWeapon;
            
            private DataColumn columnWeaponClass;
            
            internal unassignedDataTable() : 
                    base("unassigned") {
                this.InitClass();
            }
            
            internal unassignedDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn MoveColumn {
                get {
                    return this.columnMove;
                }
            }
            
            internal DataColumn competitorIdColumn {
                get {
                    return this.columncompetitorId;
                }
            }
            
            internal DataColumn NameColumn {
                get {
                    return this.columnName;
                }
            }
            
            internal DataColumn ClubColumn {
                get {
                    return this.columnClub;
                }
            }
            
            internal DataColumn WeaponColumn {
                get {
                    return this.columnWeapon;
                }
            }
            
            internal DataColumn WeaponClassColumn {
                get {
                    return this.columnWeaponClass;
                }
            }
            
            public unassignedRow this[int index] {
                get {
                    return ((unassignedRow)(this.Rows[index]));
                }
            }
            
            public event unassignedRowChangeEventHandler unassignedRowChanged;
            
            public event unassignedRowChangeEventHandler unassignedRowChanging;
            
            public event unassignedRowChangeEventHandler unassignedRowDeleted;
            
            public event unassignedRowChangeEventHandler unassignedRowDeleting;
            
            public void AddunassignedRow(unassignedRow row) {
                this.Rows.Add(row);
            }
            
            public unassignedRow AddunassignedRow(bool Move, int competitorId, string Name, string Club, string Weapon, string WeaponClass) {
                unassignedRow rowunassignedRow = ((unassignedRow)(this.NewRow()));
                rowunassignedRow.ItemArray = new object[] {
                        Move,
                        competitorId,
                        Name,
                        Club,
                        Weapon,
                        WeaponClass};
                this.Rows.Add(rowunassignedRow);
                return rowunassignedRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                unassignedDataTable cln = ((unassignedDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new unassignedDataTable();
            }
            
            internal void InitVars() {
                this.columnMove = this.Columns["Move"];
                this.columncompetitorId = this.Columns["competitorId"];
                this.columnName = this.Columns["Name"];
                this.columnClub = this.Columns["Club"];
                this.columnWeapon = this.Columns["Weapon"];
                this.columnWeaponClass = this.Columns["WeaponClass"];
            }
            
            private void InitClass() {
                this.columnMove = new DataColumn("Move", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnMove);
                this.columncompetitorId = new DataColumn("competitorId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columncompetitorId);
                this.columnName = new DataColumn("Name", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnName);
                this.columnClub = new DataColumn("Club", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnClub);
                this.columnWeapon = new DataColumn("Weapon", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnWeapon);
                this.columnWeaponClass = new DataColumn("WeaponClass", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnWeaponClass);
            }
            
            public unassignedRow NewunassignedRow() {
                return ((unassignedRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new unassignedRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(unassignedRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.unassignedRowChanged != null)) {
                    this.unassignedRowChanged(this, new unassignedRowChangeEvent(((unassignedRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.unassignedRowChanging != null)) {
                    this.unassignedRowChanging(this, new unassignedRowChangeEvent(((unassignedRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.unassignedRowDeleted != null)) {
                    this.unassignedRowDeleted(this, new unassignedRowChangeEvent(((unassignedRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.unassignedRowDeleting != null)) {
                    this.unassignedRowDeleting(this, new unassignedRowChangeEvent(((unassignedRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveunassignedRow(unassignedRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class unassignedRow : DataRow {
            
            private unassignedDataTable tableunassigned;
            
            internal unassignedRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableunassigned = ((unassignedDataTable)(this.Table));
            }
            
            public bool Move {
                get {
                    try {
                        return ((bool)(this[this.tableunassigned.MoveColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableunassigned.MoveColumn] = value;
                }
            }
            
            public int competitorId {
                get {
                    try {
                        return ((int)(this[this.tableunassigned.competitorIdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableunassigned.competitorIdColumn] = value;
                }
            }
            
            public string Name {
                get {
                    try {
                        return ((string)(this[this.tableunassigned.NameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableunassigned.NameColumn] = value;
                }
            }
            
            public string Club {
                get {
                    try {
                        return ((string)(this[this.tableunassigned.ClubColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableunassigned.ClubColumn] = value;
                }
            }
            
            public string Weapon {
                get {
                    try {
                        return ((string)(this[this.tableunassigned.WeaponColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableunassigned.WeaponColumn] = value;
                }
            }
            
            public string WeaponClass {
                get {
                    try {
                        return ((string)(this[this.tableunassigned.WeaponClassColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableunassigned.WeaponClassColumn] = value;
                }
            }
            
            public bool IsMoveNull() {
                return this.IsNull(this.tableunassigned.MoveColumn);
            }
            
            public void SetMoveNull() {
                this[this.tableunassigned.MoveColumn] = System.Convert.DBNull;
            }
            
            public bool IscompetitorIdNull() {
                return this.IsNull(this.tableunassigned.competitorIdColumn);
            }
            
            public void SetcompetitorIdNull() {
                this[this.tableunassigned.competitorIdColumn] = System.Convert.DBNull;
            }
            
            public bool IsNameNull() {
                return this.IsNull(this.tableunassigned.NameColumn);
            }
            
            public void SetNameNull() {
                this[this.tableunassigned.NameColumn] = System.Convert.DBNull;
            }
            
            public bool IsClubNull() {
                return this.IsNull(this.tableunassigned.ClubColumn);
            }
            
            public void SetClubNull() {
                this[this.tableunassigned.ClubColumn] = System.Convert.DBNull;
            }
            
            public bool IsWeaponNull() {
                return this.IsNull(this.tableunassigned.WeaponColumn);
            }
            
            public void SetWeaponNull() {
                this[this.tableunassigned.WeaponColumn] = System.Convert.DBNull;
            }
            
            public bool IsWeaponClassNull() {
                return this.IsNull(this.tableunassigned.WeaponClassColumn);
            }
            
            public void SetWeaponClassNull() {
                this[this.tableunassigned.WeaponClassColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class unassignedRowChangeEvent : EventArgs {
            
            private unassignedRow eventRow;
            
            private DataRowAction eventAction;
            
            public unassignedRowChangeEvent(unassignedRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public unassignedRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}
