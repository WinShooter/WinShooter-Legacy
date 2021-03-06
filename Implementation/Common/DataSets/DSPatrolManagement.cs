﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace Allberg.Shooter.Common {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class DSPatrolManagement : DataSet {
        
        private PSortDataTable tablePSort;
        
        private CompetitorsDataTable tableCompetitors;
        
        public DSPatrolManagement() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected DSPatrolManagement(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["PSort"] != null)) {
                    this.Tables.Add(new PSortDataTable(ds.Tables["PSort"]));
                }
                if ((ds.Tables["Competitors"] != null)) {
                    this.Tables.Add(new CompetitorsDataTable(ds.Tables["Competitors"]));
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
        public PSortDataTable PSort {
            get {
                return this.tablePSort;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public CompetitorsDataTable Competitors {
            get {
                return this.tableCompetitors;
            }
        }
        
        public override DataSet Clone() {
            DSPatrolManagement cln = ((DSPatrolManagement)(base.Clone()));
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
            if ((ds.Tables["PSort"] != null)) {
                this.Tables.Add(new PSortDataTable(ds.Tables["PSort"]));
            }
            if ((ds.Tables["Competitors"] != null)) {
                this.Tables.Add(new CompetitorsDataTable(ds.Tables["Competitors"]));
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
            this.tablePSort = ((PSortDataTable)(this.Tables["PSort"]));
            if ((this.tablePSort != null)) {
                this.tablePSort.InitVars();
            }
            this.tableCompetitors = ((CompetitorsDataTable)(this.Tables["Competitors"]));
            if ((this.tableCompetitors != null)) {
                this.tableCompetitors.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "DSPatrolManagement";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/DSPatrolManagement.xsd";
            this.Locale = new System.Globalization.CultureInfo("sv");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tablePSort = new PSortDataTable();
            this.Tables.Add(this.tablePSort);
            this.tableCompetitors = new CompetitorsDataTable();
            this.Tables.Add(this.tableCompetitors);
        }
        
        private bool ShouldSerializePSort() {
            return false;
        }
        
        private bool ShouldSerializeCompetitors() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void PSortRowChangeEventHandler(object sender, PSortRowChangeEvent e);
        
        public delegate void CompetitorsRowChangeEventHandler(object sender, CompetitorsRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class PSortDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnShooterId;
            
            private DataColumn columnClubId;
            
            private DataColumn columnNrOfRounds;
            
            internal PSortDataTable() : 
                    base("PSort") {
                this.InitClass();
            }
            
            internal PSortDataTable(DataTable table) : 
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
            
            internal DataColumn ShooterIdColumn {
                get {
                    return this.columnShooterId;
                }
            }
            
            internal DataColumn ClubIdColumn {
                get {
                    return this.columnClubId;
                }
            }
            
            internal DataColumn NrOfRoundsColumn {
                get {
                    return this.columnNrOfRounds;
                }
            }
            
            public PSortRow this[int index] {
                get {
                    return ((PSortRow)(this.Rows[index]));
                }
            }
            
            public event PSortRowChangeEventHandler PSortRowChanged;
            
            public event PSortRowChangeEventHandler PSortRowChanging;
            
            public event PSortRowChangeEventHandler PSortRowDeleted;
            
            public event PSortRowChangeEventHandler PSortRowDeleting;
            
            public void AddPSortRow(PSortRow row) {
                this.Rows.Add(row);
            }
            
            public PSortRow AddPSortRow(int ShooterId, string ClubId, int NrOfRounds) {
                PSortRow rowPSortRow = ((PSortRow)(this.NewRow()));
                rowPSortRow.ItemArray = new object[] {
                        ShooterId,
                        ClubId,
                        NrOfRounds};
                this.Rows.Add(rowPSortRow);
                return rowPSortRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                PSortDataTable cln = ((PSortDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new PSortDataTable();
            }
            
            internal void InitVars() {
                this.columnShooterId = this.Columns["ShooterId"];
                this.columnClubId = this.Columns["ClubId"];
                this.columnNrOfRounds = this.Columns["NrOfRounds"];
            }
            
            private void InitClass() {
                this.columnShooterId = new DataColumn("ShooterId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnShooterId);
                this.columnClubId = new DataColumn("ClubId", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnClubId);
                this.columnNrOfRounds = new DataColumn("NrOfRounds", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnNrOfRounds);
            }
            
            public PSortRow NewPSortRow() {
                return ((PSortRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new PSortRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(PSortRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.PSortRowChanged != null)) {
                    this.PSortRowChanged(this, new PSortRowChangeEvent(((PSortRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.PSortRowChanging != null)) {
                    this.PSortRowChanging(this, new PSortRowChangeEvent(((PSortRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.PSortRowDeleted != null)) {
                    this.PSortRowDeleted(this, new PSortRowChangeEvent(((PSortRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.PSortRowDeleting != null)) {
                    this.PSortRowDeleting(this, new PSortRowChangeEvent(((PSortRow)(e.Row)), e.Action));
                }
            }
            
            public void RemovePSortRow(PSortRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class PSortRow : DataRow {
            
            private PSortDataTable tablePSort;
            
            internal PSortRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tablePSort = ((PSortDataTable)(this.Table));
            }
            
            public int ShooterId {
                get {
                    try {
                        return ((int)(this[this.tablePSort.ShooterIdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePSort.ShooterIdColumn] = value;
                }
            }
            
            public string ClubId {
                get {
                    try {
                        return ((string)(this[this.tablePSort.ClubIdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePSort.ClubIdColumn] = value;
                }
            }
            
            public int NrOfRounds {
                get {
                    try {
                        return ((int)(this[this.tablePSort.NrOfRoundsColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tablePSort.NrOfRoundsColumn] = value;
                }
            }
            
            public bool IsShooterIdNull() {
                return this.IsNull(this.tablePSort.ShooterIdColumn);
            }
            
            public void SetShooterIdNull() {
                this[this.tablePSort.ShooterIdColumn] = System.Convert.DBNull;
            }
            
            public bool IsClubIdNull() {
                return this.IsNull(this.tablePSort.ClubIdColumn);
            }
            
            public void SetClubIdNull() {
                this[this.tablePSort.ClubIdColumn] = System.Convert.DBNull;
            }
            
            public bool IsNrOfRoundsNull() {
                return this.IsNull(this.tablePSort.NrOfRoundsColumn);
            }
            
            public void SetNrOfRoundsNull() {
                this[this.tablePSort.NrOfRoundsColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class PSortRowChangeEvent : EventArgs {
            
            private PSortRow eventRow;
            
            private DataRowAction eventAction;
            
            public PSortRowChangeEvent(PSortRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public PSortRow Row {
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
        public class CompetitorsDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnCompetitorId;
            
            private DataColumn columnPatrolClass;
            
            internal CompetitorsDataTable() : 
                    base("Competitors") {
                this.InitClass();
            }
            
            internal CompetitorsDataTable(DataTable table) : 
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
            
            internal DataColumn CompetitorIdColumn {
                get {
                    return this.columnCompetitorId;
                }
            }
            
            internal DataColumn PatrolClassColumn {
                get {
                    return this.columnPatrolClass;
                }
            }
            
            public CompetitorsRow this[int index] {
                get {
                    return ((CompetitorsRow)(this.Rows[index]));
                }
            }
            
            public event CompetitorsRowChangeEventHandler CompetitorsRowChanged;
            
            public event CompetitorsRowChangeEventHandler CompetitorsRowChanging;
            
            public event CompetitorsRowChangeEventHandler CompetitorsRowDeleted;
            
            public event CompetitorsRowChangeEventHandler CompetitorsRowDeleting;
            
            public void AddCompetitorsRow(CompetitorsRow row) {
                this.Rows.Add(row);
            }
            
            public CompetitorsRow AddCompetitorsRow(int PatrolClass) {
                CompetitorsRow rowCompetitorsRow = ((CompetitorsRow)(this.NewRow()));
                rowCompetitorsRow.ItemArray = new object[] {
                        null,
                        PatrolClass};
                this.Rows.Add(rowCompetitorsRow);
                return rowCompetitorsRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                CompetitorsDataTable cln = ((CompetitorsDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new CompetitorsDataTable();
            }
            
            internal void InitVars() {
                this.columnCompetitorId = this.Columns["CompetitorId"];
                this.columnPatrolClass = this.Columns["PatrolClass"];
            }
            
            private void InitClass() {
                this.columnCompetitorId = new DataColumn("CompetitorId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCompetitorId);
                this.columnPatrolClass = new DataColumn("PatrolClass", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPatrolClass);
                this.columnCompetitorId.AutoIncrement = true;
                this.columnCompetitorId.AutoIncrementSeed = 1;
            }
            
            public CompetitorsRow NewCompetitorsRow() {
                return ((CompetitorsRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new CompetitorsRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(CompetitorsRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.CompetitorsRowChanged != null)) {
                    this.CompetitorsRowChanged(this, new CompetitorsRowChangeEvent(((CompetitorsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.CompetitorsRowChanging != null)) {
                    this.CompetitorsRowChanging(this, new CompetitorsRowChangeEvent(((CompetitorsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.CompetitorsRowDeleted != null)) {
                    this.CompetitorsRowDeleted(this, new CompetitorsRowChangeEvent(((CompetitorsRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.CompetitorsRowDeleting != null)) {
                    this.CompetitorsRowDeleting(this, new CompetitorsRowChangeEvent(((CompetitorsRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveCompetitorsRow(CompetitorsRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CompetitorsRow : DataRow {
            
            private CompetitorsDataTable tableCompetitors;
            
            internal CompetitorsRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableCompetitors = ((CompetitorsDataTable)(this.Table));
            }
            
            public int CompetitorId {
                get {
                    try {
                        return ((int)(this[this.tableCompetitors.CompetitorIdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCompetitors.CompetitorIdColumn] = value;
                }
            }
            
            public int PatrolClass {
                get {
                    try {
                        return ((int)(this[this.tableCompetitors.PatrolClassColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCompetitors.PatrolClassColumn] = value;
                }
            }
            
            public bool IsCompetitorIdNull() {
                return this.IsNull(this.tableCompetitors.CompetitorIdColumn);
            }
            
            public void SetCompetitorIdNull() {
                this[this.tableCompetitors.CompetitorIdColumn] = System.Convert.DBNull;
            }
            
            public bool IsPatrolClassNull() {
                return this.IsNull(this.tableCompetitors.PatrolClassColumn);
            }
            
            public void SetPatrolClassNull() {
                this[this.tableCompetitors.PatrolClassColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CompetitorsRowChangeEvent : EventArgs {
            
            private CompetitorsRow eventRow;
            
            private DataRowAction eventAction;
            
            public CompetitorsRowChangeEvent(CompetitorsRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public CompetitorsRow Row {
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
