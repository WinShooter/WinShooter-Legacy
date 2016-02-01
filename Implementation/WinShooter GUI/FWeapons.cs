// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FWeapons.cs" company="John Allberg">
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
//   Summary description for FWeapons.
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
    using Allberg.Shooter.Windows.Forms;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for FWeapons.
    /// </summary>
    public class FWeapons : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components;
        private SafeButton btnCancel;
        private SafeLabel SafeLabel1;
        private SafeButton btnDelete;
        private SafeLabel SafeLabel2;
        private SafeLabel SafeLabel3;
        private SafeLabel SafeLabel4;
        private System.Windows.Forms.LinkLabel linkWeaponsAutomatic;
        private SafeButton btnSave;
        private Allberg.Shooter.Windows.Forms.SafeComboBox ddWeapons;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtWeaponId;
        private Allberg.Shooter.Windows.Forms.SafeComboBox ddClass;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtManufacturer;
        private SafeLabel SafeLabel5;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtModel;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtCaliber;
        private SafeLabel SafeLabel6;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkInternet;

        public delegate void EnableMainHandler();
        public event EnableMainHandler EnableMain;

        internal FWeapons(ref Common.Interface newCommon)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            CommonCode = newCommon;

            Trace.WriteLine("FWeapons: Creating");
            try
            {
                height = this.Size.Height;
                width = this.Size.Width;
                this.Resize+=new EventHandler(FWeapons_Resize);

                // Create one DataTable to bind dropdown with.
                weaponTable = new DataTable();
                DataColumn colName = new DataColumn("Name",Type.GetType("System.String"));
                weaponTable.Columns.Add(colName);

                DataColumn colId = new DataColumn("Id",Type.GetType("System.String"));
                weaponTable.Columns.Add(colId);

                // Display the right things in weapons
                this.ddWeapons.ValueMember = "Id";
                this.ddWeapons.DisplayMember = "Name";

                // Initialize the class dropdown by creating a 
                // datatable, filling it with info and bind
                // the control to it
                classTable = new DataTable();
                DataColumn colClassName = new DataColumn("Name",Type.GetType("System.String"));
                classTable.Columns.Add(colClassName);

                DataColumn colClassId = new DataColumn("Id",Type.GetType("System.String"));
                classTable.Columns.Add(colClassId);

                // Insert data
                for(int i=1;i<=Structs.WeaponClassMax;i++)
                {
                    Structs.WeaponClass thisClass =
                        (Structs.WeaponClass)i;

                    if (thisClass.ToString() != i.ToString())
                    {
                        DataRow row = classTable.NewRow();
                        row["Name"] = thisClass.ToString();
                        row["Id"] = (int)thisClass;
                        classTable.Rows.Add(row);
                    }
                }

                // Display the right things in weapons
                this.ddClass.DataSource = classTable;
                this.ddClass.ValueMember = "Id";
                this.ddClass.DisplayMember = "Name";

                // Internet connection
                this.linkWeaponsAutomatic.Visible = CommonCode.EnableInternetConnections;
                this.chkInternet.Visible = CommonCode.EnableInternetConnections;
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FWeapons: Creating Exception:" + exc.ToString());
                throw;
            }
            finally
            {
                Trace.WriteLine("FWeapons: Created.");
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FWeapons: Dispose(" + disposing.ToString() + ")" +
                "from thread \"" + System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            this.Visible = false;
            try
            {
                if (!this.DisposeNow)
                    EnableMain();
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FImport: exception while disposing:" + exc.ToString());
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
            Trace.WriteLine("FWeapons: Dispose(" + disposing.ToString() + ")" +
                "ended " +
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FWeapons));
            this.btnCancel = new SafeButton();
            this.ddWeapons = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.SafeLabel1 = new SafeLabel();
            this.btnDelete = new SafeButton();
            this.SafeLabel2 = new SafeLabel();
            this.SafeLabel3 = new SafeLabel();
            this.SafeLabel4 = new SafeLabel();
            this.chkInternet = new System.Windows.Forms.CheckBox();
            this.txtWeaponId = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.txtManufacturer = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.ddClass = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.linkWeaponsAutomatic = new System.Windows.Forms.LinkLabel();
            this.btnSave = new SafeButton();
            this.SafeLabel5 = new SafeLabel();
            this.txtModel = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.txtCaliber = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.SafeLabel6 = new SafeLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(320, 176);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Stäng";
            this.toolTip1.SetToolTip(this.btnCancel, "Stäng fönstret");
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ddWeapons
            // 
            this.ddWeapons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddWeapons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddWeapons.Location = new System.Drawing.Point(112, 8);
            this.ddWeapons.Name = "ddWeapons";
            this.ddWeapons.Size = new System.Drawing.Size(204, 21);
            this.ddWeapons.TabIndex = 1;
            this.toolTip1.SetToolTip(this.ddWeapons, "Välj ett vapen att konfigurera");
            this.ddWeapons.SelectedIndexChanged += new System.EventHandler(this.ddWeapons_SelectedIndexChanged);
            // 
            // SafeLabel1
            // 
            this.SafeLabel1.Location = new System.Drawing.Point(8, 8);
            this.SafeLabel1.Name = "SafeLabel1";
            this.SafeLabel1.Size = new System.Drawing.Size(96, 23);
            this.SafeLabel1.TabIndex = 2;
            this.SafeLabel1.Text = "Vapen";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(320, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Radera";
            this.toolTip1.SetToolTip(this.btnDelete, "Raderar det valda vapnet");
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // SafeLabel2
            // 
            this.SafeLabel2.Location = new System.Drawing.Point(8, 32);
            this.SafeLabel2.Name = "SafeLabel2";
            this.SafeLabel2.Size = new System.Drawing.Size(96, 23);
            this.SafeLabel2.TabIndex = 4;
            this.SafeLabel2.Text = "Beteckning";
            // 
            // SafeLabel3
            // 
            this.SafeLabel3.Location = new System.Drawing.Point(8, 80);
            this.SafeLabel3.Name = "SafeLabel3";
            this.SafeLabel3.Size = new System.Drawing.Size(96, 23);
            this.SafeLabel3.TabIndex = 5;
            this.SafeLabel3.Text = "Modell";
            // 
            // SafeLabel4
            // 
            this.SafeLabel4.Location = new System.Drawing.Point(8, 128);
            this.SafeLabel4.Name = "SafeLabel4";
            this.SafeLabel4.Size = new System.Drawing.Size(96, 23);
            this.SafeLabel4.TabIndex = 6;
            this.SafeLabel4.Text = "Klass";
            // 
            // chkInternet
            // 
            this.chkInternet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkInternet.Enabled = false;
            this.chkInternet.Location = new System.Drawing.Point(256, 152);
            this.chkInternet.Name = "chkInternet";
            this.chkInternet.Size = new System.Drawing.Size(136, 24);
            this.chkInternet.TabIndex = 7;
            this.chkInternet.TabStop = false;
            this.chkInternet.Text = "Hämtat från Internet";
            this.chkInternet.Visible = false;
            // 
            // txtWeaponId
            // 
            this.txtWeaponId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWeaponId.Location = new System.Drawing.Point(112, 32);
            this.txtWeaponId.Name = "txtWeaponId";
            this.txtWeaponId.Size = new System.Drawing.Size(280, 20);
            this.txtWeaponId.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtWeaponId, "Fyll i vapnets beteckning, t.ex. SW32");
            this.txtWeaponId.LostFocus += new System.EventHandler(this.txtWeaponId_LostFocus);
            // 
            // txtManufacturer
            // 
            this.txtManufacturer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtManufacturer.Location = new System.Drawing.Point(112, 56);
            this.txtManufacturer.Name = "txtManufacturer";
            this.txtManufacturer.Size = new System.Drawing.Size(280, 20);
            this.txtManufacturer.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtManufacturer, "Fyll i vapnets fabrikat, t.ex. Smith & Wesson");
            this.txtManufacturer.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // ddClass
            // 
            this.ddClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddClass.Location = new System.Drawing.Point(112, 128);
            this.ddClass.Name = "ddClass";
            this.ddClass.Size = new System.Drawing.Size(280, 21);
            this.ddClass.TabIndex = 6;
            this.toolTip1.SetToolTip(this.ddClass, "Välj vapnets klass");
            // 
            // linkWeaponsAutomatic
            // 
            this.linkWeaponsAutomatic.Location = new System.Drawing.Point(8, 152);
            this.linkWeaponsAutomatic.Name = "linkWeaponsAutomatic";
            this.linkWeaponsAutomatic.Size = new System.Drawing.Size(240, 23);
            this.linkWeaponsAutomatic.TabIndex = 7;
            this.linkWeaponsAutomatic.TabStop = true;
            this.linkWeaponsAutomatic.Text = "Hämta vapen från Internet";
            this.linkWeaponsAutomatic.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(240, 176);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Spara";
            this.toolTip1.SetToolTip(this.btnSave, "Spara det nya eller ändrade vapnet.");
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // SafeLabel5
            // 
            this.SafeLabel5.Location = new System.Drawing.Point(8, 56);
            this.SafeLabel5.Name = "SafeLabel5";
            this.SafeLabel5.Size = new System.Drawing.Size(96, 23);
            this.SafeLabel5.TabIndex = 9;
            this.SafeLabel5.Text = "Fabrikat";
            // 
            // txtModel
            // 
            this.txtModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModel.Location = new System.Drawing.Point(112, 80);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(280, 20);
            this.txtModel.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtModel, "Fyll i vapnets modell");
            // 
            // txtCaliber
            // 
            this.txtCaliber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCaliber.Location = new System.Drawing.Point(112, 104);
            this.txtCaliber.Name = "txtCaliber";
            this.txtCaliber.Size = new System.Drawing.Size(280, 20);
            this.txtCaliber.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtCaliber, "Fyll i vapnets kaliber, t.ex. 9mm");
            // 
            // SafeLabel6
            // 
            this.SafeLabel6.Location = new System.Drawing.Point(8, 104);
            this.SafeLabel6.Name = "SafeLabel6";
            this.SafeLabel6.Size = new System.Drawing.Size(96, 23);
            this.SafeLabel6.TabIndex = 12;
            this.SafeLabel6.Text = "Kaliber";
            // 
            // FWeapons
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(400, 206);
            this.Controls.Add(this.SafeLabel6);
            this.Controls.Add(this.txtCaliber);
            this.Controls.Add(this.txtModel);
            this.Controls.Add(this.txtManufacturer);
            this.Controls.Add(this.txtWeaponId);
            this.Controls.Add(this.SafeLabel5);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.linkWeaponsAutomatic);
            this.Controls.Add(this.ddClass);
            this.Controls.Add(this.chkInternet);
            this.Controls.Add(this.SafeLabel4);
            this.Controls.Add(this.SafeLabel3);
            this.Controls.Add(this.SafeLabel2);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.SafeLabel1);
            this.Controls.Add(this.ddWeapons);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FWeapons";
            this.Text = "Vapen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        internal bool DisposeNow = false;
        Common.Interface CommonCode;
        int height;
        int width;

        Structs.Weapon[] weapons;
        DataTable weaponTable;
        DataTable classTable;

        const string NewWeaponValue = "AllbergNewWeapon"; 
        const string NewWeaponText = "<-- Nytt vapen -->";

        private void FWeapons_Resize(object sender, EventArgs e)
        {
            //Size size = new Size(this.width, this.height);
            Size size = new Size(this.Size.Width, this.height);
            this.Size = size;
        }
        internal void enableMe()
        {
            this.Visible = true;
            this.Focus();

            updatedWeapons(true);
            restoreWindow();
        }

        private bool useCachedWeapons = false;

        internal void updatedWeapons()
        {
            updatedWeapons(false);
        }
        private void updatedWeapons(bool guiOnly)
        {
            if (!guiOnly & !this.Visible)
            {
                useCachedWeapons = false;
                return;
            }

            if (useCachedWeapons & guiOnly)
                return;

            weapons = CommonCode.GetWeapons("Manufacturer, Model");

            weaponTable.Clear();
            DataRow newRow = weaponTable.NewRow();
            newRow["Id"] = NewWeaponValue;
            newRow["Name"] = NewWeaponText;
            weaponTable.Rows.Add(newRow);
            

            
            foreach(Structs.Weapon weapon in weapons)
            {
                newRow = weaponTable.NewRow();
                newRow["Name"] = weapon.Manufacturer + "," +
                    weapon.Model + ", " +
                    weapon.Caliber;
                newRow["Id"] = weapon.WeaponId;
                weaponTable.Rows.Add(newRow);
            }
            
            this.ddWeapons.DataSource = weaponTable;
            useCachedWeapons = true;
        }

        private void restoreWindow()
        {
            this.ddWeapons.SelectedValue = NewWeaponValue;

            this.txtWeaponId.Text = "";
            this.txtManufacturer.Text = "";
            this.txtModel.Text = "";
            this.txtCaliber.Text = "";
            this.ddClass.SelectedIndex = -1;
            this.chkInternet.Checked = false;

            this.btnDelete.Enabled = false;
            this.btnSave.Enabled = false;
            this.txtWeaponId.Enabled = true;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Visible = false;
            this.EnableMain();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (this.txtWeaponId.Text.Length == 0 |
                this.txtManufacturer.Text.Length == 0 |
                this.txtModel.Text.Length == 0)
            {
                MessageBox.Show("Du måste skriva något i vapnets tillverkare, modell och id");
                return;
            }
            if ((string)this.ddWeapons.SelectedValue == NewWeaponValue)
            {
                // New weapon

                Structs.Weapon weapon = new Structs.Weapon();
                weapon.WeaponId = this.txtWeaponId.Text;
                weapon.Manufacturer = this.txtManufacturer.Text;
                weapon.Model = this.txtModel.Text;
                weapon.Caliber = this.txtCaliber.Text;
                weapon.WClass = (Structs.WeaponClass)
                    Convert.ToInt32(
                    (string)this.ddClass.SelectedValue);
                weapon.Automatic = false;
                weapon.ToAutomatic = false;
                if (CommonCode.EnableInternetConnections)
                {
                    DialogResult res =
                        MessageBox.Show("Vill du lägga till vapnet \"" +
                        this.txtManufacturer.Text + ", " + 
                        this.txtModel.Text + 
                        "\" till Internet-databasen?",
                        "Internet-databasen", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);
                    if (DialogResult.Yes == res)
                        weapon.ToAutomatic = true;
                }

                CommonCode.NewWeapon(weapon);
            }
            else
            {
                Structs.Weapon updatedWeapon = CommonCode.GetWeapon((string)this.ddWeapons.SelectedValue);

                updatedWeapon.WClass = (Structs.WeaponClass)
                    Convert.ToInt32(
                    (string)this.ddClass.SelectedValue);
                updatedWeapon.Manufacturer = this.txtManufacturer.Text;
                updatedWeapon.Model = this.txtModel.Text;
                updatedWeapon.Caliber = this.txtCaliber.Text;
                updatedWeapon.Automatic = this.chkInternet.Checked;
                updatedWeapon.ToAutomatic = false;
                if (CommonCode.EnableInternetConnections)
                {
                    string text = "Vill du ändra vapnet \"" +
                        this.txtManufacturer.Text + "," +
                        this.txtModel.Text + 
                        "\" i Internet-databasen?";
                    if (this.chkInternet.Checked == false)
                        text = "Vill du lägga till vapnet \"" +
                            this.txtManufacturer.Text + "," +
                            this.txtModel.Text + "\" i Internet-databasen?";
                    DialogResult res = MessageBox.Show(text,
                        "Internet-databasen",
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);
                    if (DialogResult.Yes == res)
                        updatedWeapon.ToAutomatic = true;
                }
                CommonCode.UpdateWeapon(updatedWeapon);
            }

            restoreWindow();
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            CommonCode.DelWeapon(CommonCode.GetWeapon((string)this.ddWeapons.SelectedValue));
            this.restoreWindow();
        }

        private void ddWeapons_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((string)this.ddWeapons.SelectedValue == NewWeaponValue)
            {
                this.restoreWindow();
            }
            else
            {
                this.txtWeaponId.Enabled = false;
                this.btnDelete.Enabled = true;

                Structs.Weapon weapon = CommonCode.GetWeapon((string)this.ddWeapons.SelectedValue);
                this.txtWeaponId.Text = weapon.WeaponId;
                this.txtManufacturer.Text = weapon.Manufacturer;
                this.txtModel.Text = weapon.Model;
                this.txtCaliber.Text = weapon.Caliber;
                this.ddClass.SelectedValue = (int)weapon.WClass;
                this.chkInternet.Checked = weapon.Automatic;
            }
        }

        private void txtName_TextChanged(object sender, System.EventArgs e)
        {
            this.btnSave.Enabled = true;
        }

        private void txtWeaponId_LostFocus(object sender, EventArgs e)
        {
            if (txtWeaponId.Enabled == true)
            {
                try
                {
                    CommonCode.GetWeapon(txtWeaponId.Text);

                    // Ops, no exception, meaning the weapon already exist.
                    // Select from dropdown.
                    this.ddWeapons.SelectedValue = txtWeaponId.Text;
                }
                catch (CannotFindIdException)
                {
                    // This is where we get if the new weapon
                    // doesn't already exist.
                }
            }
        }
    }
}
