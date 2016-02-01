// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FClubs.cs" company="John Allberg">
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
//   Summary description for FClubs.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    using Allberg.Shooter.Common.Exceptions;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for FClubs.
    /// </summary>
    public class FClubs : Form
    {
        private Forms.SafeLabel _safeLabel1;
        private Forms.SafeLabel _safeLabel2;
        private Forms.SafeTextBox _txtName;
        private Forms.SafeLabel _safeLabel3;
        private Forms.SafeLabel _safeLabel4;
        private Forms.SafeButton _btnSave;
        private Forms.SafeButton _btnCancel;
        private Forms.SafeComboBox _ddClubs;
        private Forms.SafeButton _btnDelete;
        private Forms.SafeTextBox _txtOfficialName;
        private Forms.SafeComboBox _ddCountry;
        private LinkLabel _linkFetchClubsAutomatic;
        private ToolTip _toolTip1;
        private System.ComponentModel.IContainer components;
        private TextBox _txtPgInfo;
        private TextBox _txtBgInfo;
        private Label _label1;
        private Label _label2;
        private PictureBox _picPgError;
        private PictureBox _picBgError;
        private readonly MethodInvoker _bindClubsTable;

        internal FClubs(ref Common.Interface newCommon)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            _commonCode = newCommon;

            Trace.WriteLine("FClubs: Creating");
            try
            {
                _height = Size.Height;
                _width = Size.Width;
                Resize += FClubsResize;

                // Create some things to hold clubs.
                _bindClubsTable += BindClubsTable;

                // Create one DataTable to bind dropdown with
                _clubTable = new DataTable();
                var colName = new DataColumn("Name",Type.GetType("System.String"));
                _clubTable.Columns.Add(colName);

                var colId = new DataColumn("Id",Type.GetType("System.String"));
                _clubTable.Columns.Add(colId);

                // Display the right things in clubs
                _ddClubs.ValueMember = "Id";
                _ddClubs.DisplayMember = "Name";

                // Countrys
                _countryTable = new DataTable();
                var colCountry = new DataColumn("Country", Type.GetType("System.String"));
                _countryTable.Columns.Add(colCountry);
                var colCId = new DataColumn("Id",Type.GetType("System.String"));
                _countryTable.Columns.Add(colCId);

                var seRow = _countryTable.NewRow();
                seRow["Country"] = "Sverige";
                seRow["Id"] = "SE";
                _countryTable.Rows.Add(seRow);

                _ddCountry.DataSource = _countryTable;
                _ddCountry.DisplayMember = "Country";
                _ddCountry.ValueMember = "Id";

                _ddClubs.DataSource = _clubTable;
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FClubs: Exception during creation:" + exc);
                throw;
            }
            finally
            {
                Trace.WriteLine("FClubs: Created.");
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FClubs: Dispose(" + disposing + ")" +
                "from thread \"" + Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                DateTime.Now.ToLongTimeString());
            
            Visible = false;
            try
            {
                if (!DisposeNow)
                    EnableMain();
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FClubs: exception while enabling Main:" + exc);
            }

            if(!DisposeNow)
                return;

            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
            Trace.WriteLine("FClubs: Dispose(" + disposing + ")" +
                "ended." +
                DateTime.Now.ToLongTimeString());
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FClubs));
            this._safeLabel1 = new Forms.SafeLabel();
            this._ddClubs = new Forms.SafeComboBox();
            this._btnDelete = new Forms.SafeButton();
            this._safeLabel2 = new Forms.SafeLabel();
            this._txtName = new Forms.SafeTextBox();
            this._safeLabel3 = new Forms.SafeLabel();
            this._safeLabel4 = new Forms.SafeLabel();
            this._txtOfficialName = new Forms.SafeTextBox();
            this._ddCountry = new Forms.SafeComboBox();
            this._btnSave = new Forms.SafeButton();
            this._btnCancel = new Forms.SafeButton();
            this._linkFetchClubsAutomatic = new System.Windows.Forms.LinkLabel();
            this._toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._txtPgInfo = new System.Windows.Forms.TextBox();
            this._txtBgInfo = new System.Windows.Forms.TextBox();
            this._label1 = new System.Windows.Forms.Label();
            this._label2 = new System.Windows.Forms.Label();
            this._picPgError = new System.Windows.Forms.PictureBox();
            this._picBgError = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // SafeLabel1
            // 
            this._safeLabel1.Location = new System.Drawing.Point(8, 8);
            this._safeLabel1.Name = "_safeLabel1";
            this._safeLabel1.TabIndex = 0;
            this._safeLabel1.Text = "Klubbar:";
            // 
            // ddClubs
            // 
            this._ddClubs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ddClubs.Location = new System.Drawing.Point(112, 8);
            this._ddClubs.Name = "_ddClubs";
            this._ddClubs.Size = new System.Drawing.Size(200, 21);
            this._ddClubs.TabIndex = 1;
            this._ddClubs.SelectedIndexChanged += new System.EventHandler(this.DdClubsSelectedIndexChanged);
            // 
            // btnDelete
            // 
            this._btnDelete.Location = new System.Drawing.Point(320, 8);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.TabIndex = 2;
            this._btnDelete.Text = "Radera";
            this._toolTip1.SetToolTip(this._btnDelete, "Här raderar du den valda klubben");
            this._btnDelete.Click += new System.EventHandler(this.BtnDeleteClick);
            // 
            // SafeLabel2
            // 
            this._safeLabel2.Location = new System.Drawing.Point(8, 56);
            this._safeLabel2.Name = "_safeLabel2";
            this._safeLabel2.TabIndex = 4;
            this._safeLabel2.Text = "Namn";
            // 
            // txtName
            // 
            this._txtName.Location = new System.Drawing.Point(112, 56);
            this._txtName.Name = "_txtName";
            this._txtName.Size = new System.Drawing.Size(280, 20);
            this._txtName.TabIndex = 4;
            this._txtName.Text = "";
            this._txtName.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // SafeLabel3
            // 
            this._safeLabel3.Location = new System.Drawing.Point(8, 80);
            this._safeLabel3.Name = "_safeLabel3";
            this._safeLabel3.TabIndex = 6;
            this._safeLabel3.Text = "Land";
            // 
            // SafeLabel4
            // 
            this._safeLabel4.Location = new System.Drawing.Point(8, 32);
            this._safeLabel4.Name = "_safeLabel4";
            this._safeLabel4.TabIndex = 7;
            this._safeLabel4.Text = "Officiell beteckning";
            // 
            // txtOfficialName
            // 
            this._txtOfficialName.Location = new System.Drawing.Point(112, 32);
            this._txtOfficialName.Name = "_txtOfficialName";
            this._txtOfficialName.Size = new System.Drawing.Size(280, 20);
            this._txtOfficialName.TabIndex = 3;
            this._txtOfficialName.Text = "";
            this._txtOfficialName.TextChanged += new System.EventHandler(this.TxtOfficialNameTextChanged);
            // 
            // ddCountry
            // 
            this._ddCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ddCountry.Location = new System.Drawing.Point(112, 80);
            this._ddCountry.Name = "_ddCountry";
            this._ddCountry.Size = new System.Drawing.Size(280, 21);
            this._ddCountry.TabIndex = 5;
            // 
            // btnSave
            // 
            this._btnSave.Enabled = false;
            this._btnSave.Location = new System.Drawing.Point(240, 160);
            this._btnSave.Name = "_btnSave";
            this._btnSave.TabIndex = 8;
            this._btnSave.Text = "Spara";
            this._toolTip1.SetToolTip(this._btnSave, "Spara den ändrade informationen");
            this._btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(320, 160);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.TabIndex = 9;
            this._btnCancel.Text = "Stäng";
            this._toolTip1.SetToolTip(this._btnCancel, "Stäng utan att spara");
            this._btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // linkFetchClubsAutomatic
            // 
            this._linkFetchClubsAutomatic.Location = new System.Drawing.Point(8, 160);
            this._linkFetchClubsAutomatic.Name = "_linkFetchClubsAutomatic";
            this._linkFetchClubsAutomatic.Size = new System.Drawing.Size(160, 23);
            this._linkFetchClubsAutomatic.TabIndex = 10;
            this._linkFetchClubsAutomatic.TabStop = true;
            this._linkFetchClubsAutomatic.Text = "Hämta klubbar från Internet";
            this._linkFetchClubsAutomatic.Visible = false;
            this._linkFetchClubsAutomatic.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkFetchClubsAutomaticLinkClicked);
            // 
            // txtPgInfo
            // 
            this._txtPgInfo.Location = new System.Drawing.Point(112, 104);
            this._txtPgInfo.Name = "_txtPgInfo";
            this._txtPgInfo.Size = new System.Drawing.Size(280, 20);
            this._txtPgInfo.TabIndex = 6;
            this._txtPgInfo.Text = "";
            this._txtPgInfo.TextChanged += new System.EventHandler(this.TxtPgInfoTextChanged);
            // 
            // txtBgInfo
            // 
            this._txtBgInfo.Location = new System.Drawing.Point(112, 128);
            this._txtBgInfo.Name = "_txtBgInfo";
            this._txtBgInfo.Size = new System.Drawing.Size(280, 20);
            this._txtBgInfo.TabIndex = 7;
            this._txtBgInfo.Text = "";
            this._txtBgInfo.TextChanged += new System.EventHandler(this.TxtBgInfoTextChanged);
            // 
            // label1
            // 
            this._label1.Location = new System.Drawing.Point(8, 104);
            this._label1.Name = "_label1";
            this._label1.TabIndex = 12;
            this._label1.Text = "Postgiro";
            // 
            // label2
            // 
            this._label2.Location = new System.Drawing.Point(8, 128);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(104, 23);
            this._label2.TabIndex = 13;
            this._label2.Text = "Bankgiro";
            // 
            // picPgError
            // 
            this._picPgError.Image = ((System.Drawing.Image)(resources.GetObject("picPgError.Image")));
            this._picPgError.Location = new System.Drawing.Point(96, 112);
            this._picPgError.Name = "_picPgError";
            this._picPgError.Size = new System.Drawing.Size(16, 16);
            this._picPgError.TabIndex = 14;
            this._picPgError.TabStop = false;
            this._picPgError.Visible = false;
            // 
            // picBgError
            // 
            this._picBgError.Image = ((System.Drawing.Image)(resources.GetObject("picBgError.Image")));
            this._picBgError.Location = new System.Drawing.Point(96, 136);
            this._picBgError.Name = "_picBgError";
            this._picBgError.Size = new System.Drawing.Size(16, 16);
            this._picBgError.TabIndex = 15;
            this._picBgError.TabStop = false;
            this._picBgError.Visible = false;
            // 
            // FClubs
            // 
            this.AcceptButton = this._btnSave;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(400, 190);
            this.Controls.Add(this._picBgError);
            this.Controls.Add(this._picPgError);
            this.Controls.Add(this._label2);
            this.Controls.Add(this._label1);
            this.Controls.Add(this._txtBgInfo);
            this.Controls.Add(this._txtPgInfo);
            this.Controls.Add(this._linkFetchClubsAutomatic);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnSave);
            this.Controls.Add(this._ddCountry);
            this.Controls.Add(this._txtOfficialName);
            this.Controls.Add(this._txtName);
            this.Controls.Add(this._safeLabel4);
            this.Controls.Add(this._safeLabel3);
            this.Controls.Add(this._safeLabel2);
            this.Controls.Add(this._btnDelete);
            this.Controls.Add(this._ddClubs);
            this.Controls.Add(this._safeLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FClubs";
            this.Text = "Klubbar";
            this.ResumeLayout(false);

        }
        #endregion

        internal bool DisposeNow;
        public delegate void EnableMainHandler();
        public event EnableMainHandler EnableMain;

        readonly Common.Interface _commonCode;
        readonly int _height;
        readonly int _width;

        Structs.Club[] _clubs;
        DataTable _clubTable;
        readonly DataTable _countryTable;

        const string NewClubValue = "AllbergNewClub"; 
        const string NewClubText = "<-- Ny klubb -->";

        internal void EnableMe()
        {
            Visible = true;
            Focus();
            UpdatedClubs();

            RestoreWindow();
        }

        private void RestoreWindow()
        {
            _txtOfficialName.Text = "";
            _txtOfficialName.Enabled = true;
            _txtName.Text = "";
            _ddCountry.SelectedValue = "SE";
            _btnDelete.Enabled = false;
            _ddClubs.SelectedValue = NewClubValue;
            _btnSave.Enabled = false;
            _txtPgInfo.Text = "";
            _txtBgInfo.Text = "";
            _ddClubs.Focus();
        }

        readonly object _updatedClubsThreadLock = new object();
        internal void UpdatedClubs()
        {
            try
            {
                Trace.WriteLine("FClubs.updatedClubs() started on thread " +
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId + " )");

                Trace.WriteLine("FClubs.updatedClubs()  locking \"updatedClubsThreadLock\"...");

                lock(_updatedClubsThreadLock)
                {
                    Trace.WriteLine("FClubs.updatedClubs()  locked \"updatedClubsThreadLock\".");
                    _clubs = _commonCode.GetClubs();

                    //clubTable.Clear();
                    // Create one DataTable to bind dropdown with
                    _clubTable = new DataTable();
                    var colName = new DataColumn("Name",Type.GetType("System.String"));
                    _clubTable.Columns.Add(colName);

                    var colId = new DataColumn("Id",Type.GetType("System.String"));
                    _clubTable.Columns.Add(colId);

                    // add default value
                    var newRow = _clubTable.NewRow();
                    newRow["Id"] = NewClubValue;
                    newRow["Name"] = NewClubText;
                    _clubTable.Rows.Add(newRow);
            
                    foreach(var club in _clubs)
                    {
                        newRow = _clubTable.NewRow();
                        newRow["Name"] = club.Name;
                        newRow["Id"] = club.ClubId;
                        _clubTable.Rows.Add(newRow);
                    }

                    if (InvokeRequired)
                        Invoke(_bindClubsTable);
                    else
                        _bindClubsTable();
                }
            }
            finally
            {
                Trace.WriteLine("FClubs.updatedClubs() unlocking \"updatedClubsThreadLock\".");
                Trace.WriteLine("FClubs.updatedClubs() ended.");
            }
        }

        private void BindClubsTable()
        {
            object selectedValue = _ddClubs.SelectedValue;
            _ddClubs.DataSource = _clubTable;
            _ddClubs.ValueMember = "Id";
            _ddClubs.DisplayMember = "Name";
            try
            {
                if (selectedValue != null)
                    _ddClubs.SelectedValue = selectedValue;
                else
                    _ddClubs.SelectedIndex = 0;
            }
            catch(Exception)
            {
                // Occurs when a club is deleted
                _ddClubs.SelectedIndex = 0;
            }
        }

        private void FClubsResize(object sender, EventArgs e)
        {
            var size = new Size(_width, _height);
            Size = size;
        }

        private void BtnDeleteClick(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FClubs: btnDelete_Click() started");
            var res =
                MessageBox.Show(
                "Detta kommer att radera klubben. Är du säker?", 
                "Bekräftelse", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning, 
                MessageBoxDefaultButton.Button2);

            if (res == DialogResult.Yes)
            {
                try
                {
                    Trace.WriteLine("FClubs: deleting club:" + 
                        (string)_ddClubs.SelectedValue);
                    _commonCode.DelClub(_commonCode.GetClub((string)_ddClubs.SelectedValue));
                    RestoreWindow();
                }
                catch(System.Data.OleDb.OleDbException exc)
                {
                    Trace.WriteLine("FClubs: Exception while deleting club:" + 
                        exc);
                    if (exc.Message.IndexOf("Shooter")>-1)
                    {
                        MessageBox.Show(
                            "Ett fel uppstod vid radering av klubb.\r\n" + 
                            "Detta beror på att det finns skyttar " +
                            "upplagda som tillhör den klubben.",
                            "Ett fel har uppstått",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Ett fel uppstod vid radering av klubb.\r\n" + 
                            "\r\n" + 
                            "Systemmeddelende: " + exc.Message,
                            "Ett fel har uppstått",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            Trace.WriteLine("FClubs: btnDelete_Click() ended");
        }

        private void DdClubsSelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)_ddClubs.SelectedValue == NewClubValue)
            {
                // Clean up
                RestoreWindow();
            }
            else
            {
                Structs.Club club =
                    _commonCode.GetClub((string)_ddClubs.SelectedValue);
                _txtName.Text = club.Name;
                _txtOfficialName.Text = club.ClubId;
                _txtOfficialName.Enabled = false;
                _txtPgInfo.Text = FormatGiro.FormatPlusgiro(club.Plusgiro);
                _txtBgInfo.Text = FormatGiro.FormatBankgiro(club.Bankgiro);
                _ddCountry.SelectedValue = club.Country;
                _btnDelete.Enabled = true;
            }
            _btnSave.Enabled = false;
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            // Validation
            if (_txtOfficialName.Text.Length == 0 |
                _txtName.Text.Length == 0)
            {
                MessageBox.Show("Varken den officiella beteckning eller " +
                    "namnet får vara blankt.",
                    "Inmatningsfel", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
                int.Parse(_txtOfficialName.Text.Replace("-",""));
            }
            catch(Exception)
            {
                MessageBox.Show("Den officella beteckningen består endast av tal" +
                    " och 1 minustecken.",
                    "Inmatningsfel", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (!PostgiroIsValid(_txtPgInfo.Text.Replace("-", "").Replace(" ", "")))
                {
                    MessageBox.Show("Postgiroinformationen verkar vara felaktig.",
                        "Inmatningsfel", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                if (!BankgiroIsValid(_txtBgInfo.Text.Replace("-", "").Replace(" ", "")))
                {
                    MessageBox.Show("Bankgiroinformationen verkar vara felaktig.",
                        "Inmatningsfel", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Postgiro- och/eller Bankgiroinformationen verkar vara felaktig.",
                    "Inmatningsfel", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }


            // Save
            if ((string)_ddClubs.SelectedValue == NewClubValue)
            {
                // New club
                // Check if club exists
                try
                {
                    var temp = _commonCode.GetClub(_txtOfficialName.Text);
                    MessageBox.Show("Det finns redan en klubb med det officiella namnet \"" +
                        _txtOfficialName.Text + 
                        "\". Den heter \"" +
                        temp.Name + 
                        "\"", "Felmeddelande", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (CannotFindIdException)
                {
                    // Do nothing, continue
                }
                
                var club = new Structs.Club
                            {
                                ClubId = _txtOfficialName.Text,
                                Name = _txtName.Text,
                                Country = (string) _ddCountry.SelectedValue,
                                Automatic = false,
                                ToAutomatic = false,
                                Plusgiro = _txtPgInfo.Text.Replace(" ", ""),
                                Bankgiro = _txtBgInfo.Text.Replace(" ", "")
                            };
                if (_commonCode.EnableInternetConnections)
                {
                    DialogResult res =
                        MessageBox.Show("Vill du lägga till klubben \"" +
                        _txtName.Text + "\" till Internet-databasen?",
                        "Internet-databasen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DialogResult.Yes == res)
                        club.ToAutomatic = true;
                }
                _commonCode.NewClub(club);
            }
            else
            {
                // Edit existing club
                var club = _commonCode.GetClub((string)_ddClubs.SelectedValue);
                club.Country = (string)_ddCountry.SelectedValue;
                club.Name = _txtName.Text;
                club.ToAutomatic = false;
                club.Plusgiro = _txtPgInfo.Text.Replace(" ", "");
                club.Bankgiro = _txtBgInfo.Text.Replace(" ", "");
                if (_commonCode.EnableInternetConnections)
                {
                    var res =
                        MessageBox.Show("Vill du uppdatera klubben \"" +
                        _txtName.Text + "\" i Internet-databasen?",
                        "Internet-databasen", MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);
                    if (DialogResult.Yes == res)
                        club.ToAutomatic = true;
                }
                _commonCode.UpdateClub(club);
            }

            RestoreWindow();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Visible = false;
            EnableMain();
        }

        private void LinkFetchClubsAutomaticLinkClicked(object sender, 
            LinkLabelLinkClickedEventArgs e)
        {
            // TODO Implement
            MessageBox.Show("Not implemented yet!", 
                "Implementation Status", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        private void TxtOfficialNameTextChanged(object sender, EventArgs e)
        {
            _btnSave.Enabled = true;
        }

        private void TxtNameTextChanged(object sender, EventArgs e)
        {
            _btnSave.Enabled = true;
        }

        private void TxtPgInfoTextChanged(object sender, EventArgs e)
        {
            // Remove all unwanted characters
            var temp = new System.Text.StringBuilder();
            for(var i=0 ;i<_txtPgInfo.Text.Length;i++)
            {
                try
                {
                    var thisChar = _txtPgInfo.Text.Substring(i,1);
                    if (thisChar == " " |
                        thisChar == "-")
                    {
                        temp.Append(thisChar);
                    }
                    else
                    {
                        // Check for integer
                        int.Parse(thisChar);
                        temp.Append(thisChar);
                    }
                }
                catch(FormatException)
                {
                }
            }
            if (temp.ToString() != _txtPgInfo.Text)
            {
                _txtPgInfo.Text = temp.ToString();
                return;
            }

            _picPgError.Visible = !PostgiroIsValid(_txtPgInfo.Text);
            if (_picPgError.Visible == false)
                _btnSave.Enabled = true;
        }

        private static bool PostgiroIsValid(string postgiro)
        {
            try
            {
                // Ok, if it's empty, return
                if (postgiro == "")
                {
                    return true;
                }

                // Check lengths
                var lengthCheck = postgiro.Replace(" ", "").Replace("-","");
                if (lengthCheck.Length < 6 |
                    lengthCheck.Length > 9)
                {
                    return false;
                }

                return CheckNumberIsOk(postgiro);
            }
            catch(Exception)
            {
                return false;
            }
        }


        private void TxtBgInfoTextChanged(object sender, EventArgs e)
        {
            // Remove all unwanted characters
            var temp = new System.Text.StringBuilder();
            for(var i=0 ;i<_txtPgInfo.Text.Length;i++)
            {
                try
                {
                    var thisChar = _txtPgInfo.Text.Substring(i,1);
                    if (thisChar == " " |
                        thisChar == "-")
                    {
                        temp.Append(thisChar);
                    }
                    else
                    {
                        // Check for integer
                        int.Parse(thisChar);
                        temp.Append(thisChar);
                    }
                }
                catch(FormatException)
                {
                }
            }
            if (temp.ToString() != _txtPgInfo.Text)
            {
                _txtPgInfo.Text = temp.ToString();
                return;
            }

            _picBgError.Visible = !BankgiroIsValid(
                _txtBgInfo.Text.Replace("-", "").Replace(" ", ""));
            if (_picBgError.Visible == false)
                _btnSave.Enabled = true;
        }

        private bool BankgiroIsValid(string bankgiro)
        {
            if (bankgiro == "")
            {
                return true;
            }

            if (bankgiro.Length < 7 |
                bankgiro.Length > 8)
            {
                return false;
            }

            return CheckNumberIsOk(_txtBgInfo.Text);
        }

        private static bool CheckNumberIsOk(string innr)
        {
            var nr = innr.Replace(" ", "").Replace("-", "");

            var sumString = new System.Text.StringBuilder();
            var multiplier = 2-nr.Length%2;
            for(var i=0;i<nr.Length-1;i++)
            {
                var j = int.Parse(nr.Substring(i,1));

                j = multiplier * j;

                multiplier = multiplier == 1 ? 2 : 1;
                sumString.Append(j.ToString());
            }

            var totalSum = 0;
            foreach(var thisChar in sumString.ToString())
            {
                totalSum += int.Parse(thisChar.ToString());
            }

            var checkNr = -1;
            for(var i=10;i<totalSum+10;i=i+10)
            {
                checkNr = i-totalSum;
            }

            return checkNr == int.Parse(nr.Substring(nr.Length-1, 1));
        }

    }
}