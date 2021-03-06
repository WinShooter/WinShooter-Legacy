﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class CountryDataset : DataSet {
        
        private CountrysDataTable tableCountrys;
        
        public CountryDataset() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected CountryDataset(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["Countrys"] != null)) {
                    this.Tables.Add(new CountrysDataTable(ds.Tables["Countrys"]));
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
        public CountrysDataTable Countrys {
            get {
                return this.tableCountrys;
            }
        }
        
        public override DataSet Clone() {
            CountryDataset cln = ((CountryDataset)(base.Clone()));
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
            if ((ds.Tables["Countrys"] != null)) {
                this.Tables.Add(new CountrysDataTable(ds.Tables["Countrys"]));
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
            this.tableCountrys = ((CountrysDataTable)(this.Tables["Countrys"]));
            if ((this.tableCountrys != null)) {
                this.tableCountrys.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "CountryDataset";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/CountryDataset.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableCountrys = new CountrysDataTable();
            this.Tables.Add(this.tableCountrys);
        }
        
        private bool ShouldSerializeCountrys() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void CountrysRowChangeEventHandler(object sender, CountrysRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CountrysDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnIso;
            
            private DataColumn columnDisplayName;
            
            internal CountrysDataTable() : 
                    base("Countrys") {
                this.InitClass();
            }
            
            internal CountrysDataTable(DataTable table) : 
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
            
            internal DataColumn IsoColumn {
                get {
                    return this.columnIso;
                }
            }
            
            internal DataColumn DisplayNameColumn {
                get {
                    return this.columnDisplayName;
                }
            }
            
            public CountrysRow this[int index] {
                get {
                    return ((CountrysRow)(this.Rows[index]));
                }
            }
            
            public event CountrysRowChangeEventHandler CountrysRowChanged;
            
            public event CountrysRowChangeEventHandler CountrysRowChanging;
            
            public event CountrysRowChangeEventHandler CountrysRowDeleted;
            
            public event CountrysRowChangeEventHandler CountrysRowDeleting;
            
            public void AddCountrysRow(CountrysRow row) {
                this.Rows.Add(row);
            }
            
            public CountrysRow AddCountrysRow(string Iso, string DisplayName) {
                CountrysRow rowCountrysRow = ((CountrysRow)(this.NewRow()));
                rowCountrysRow.ItemArray = new object[] {
                        Iso,
                        DisplayName};
                this.Rows.Add(rowCountrysRow);
                return rowCountrysRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                CountrysDataTable cln = ((CountrysDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new CountrysDataTable();
            }
            
            internal void InitVars() {
                this.columnIso = this.Columns["Iso"];
                this.columnDisplayName = this.Columns["DisplayName"];
            }
            
            private void InitClass() {
                this.columnIso = new DataColumn("Iso", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnIso);
                this.columnDisplayName = new DataColumn("DisplayName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDisplayName);
            }
            
            public CountrysRow NewCountrysRow() {
                return ((CountrysRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new CountrysRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(CountrysRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.CountrysRowChanged != null)) {
                    this.CountrysRowChanged(this, new CountrysRowChangeEvent(((CountrysRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.CountrysRowChanging != null)) {
                    this.CountrysRowChanging(this, new CountrysRowChangeEvent(((CountrysRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.CountrysRowDeleted != null)) {
                    this.CountrysRowDeleted(this, new CountrysRowChangeEvent(((CountrysRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.CountrysRowDeleting != null)) {
                    this.CountrysRowDeleting(this, new CountrysRowChangeEvent(((CountrysRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveCountrysRow(CountrysRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CountrysRow : DataRow {
            
            private CountrysDataTable tableCountrys;
            
            internal CountrysRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableCountrys = ((CountrysDataTable)(this.Table));
            }
            
            public string Iso {
                get {
                    try {
                        return ((string)(this[this.tableCountrys.IsoColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCountrys.IsoColumn] = value;
                }
            }
            
            public string DisplayName {
                get {
                    try {
                        return ((string)(this[this.tableCountrys.DisplayNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tableCountrys.DisplayNameColumn] = value;
                }
            }
            
            public bool IsIsoNull() {
                return this.IsNull(this.tableCountrys.IsoColumn);
            }
            
            public void SetIsoNull() {
                this[this.tableCountrys.IsoColumn] = System.Convert.DBNull;
            }
            
            public bool IsDisplayNameNull() {
                return this.IsNull(this.tableCountrys.DisplayNameColumn);
            }
            
            public void SetDisplayNameNull() {
                this[this.tableCountrys.DisplayNameColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CountrysRowChangeEvent : EventArgs {
            
            private CountrysRow eventRow;
            
            private DataRowAction eventAction;
            
            public CountrysRowChangeEvent(CountrysRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public CountrysRow Row {
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
