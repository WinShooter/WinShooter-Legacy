// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FCompetitionField.cs" company="John Allberg">
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
//   Summary description for FCompetition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using Allberg.Shooter.Windows.Forms;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for FCompetition.
    /// </summary>
    public class FCompetitionField : System.Windows.Forms.Form
    {
        private SafeButton btnCancel;
        private SafeLabel lblName;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtName;
        private SafeLabel lblStartDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private SafeLabel lblStartTime;
        private SafeLabel lblPatrolTime;
        private SafeLabel SafeLabel1;
        private SafeLabel SafeLabel2;
        private SafeLabel SafeLabel3;
        private SafeButton btnSave;
        private System.Windows.Forms.CheckBox chkNorwegianCount;
        private System.Windows.Forms.NumericUpDown numStartHour;
        private System.Windows.Forms.NumericUpDown numPatrolTime;
        private System.Windows.Forms.NumericUpDown numPatrolTimeBetween;
        private System.Windows.Forms.NumericUpDown numPatrolSize;
        private System.Windows.Forms.NumericUpDown numPatrolRest;
        private System.Windows.Forms.NumericUpDown numStartMinute;
        private SafeLabel SafeLabel4;
        private SafeLabel SafeLabel5;
        private SafeLabel SafeLabel6;
        private SafeLabel SafeLabel7;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkFinal;
        private System.Windows.Forms.CheckBox chkUsePriceMoney;
        private System.Windows.Forms.NumericUpDown numPriceMoneyReturn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtShooterFee1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFirstPrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numShoterPercentWithPrice;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCompetitionType;
        private Allberg.Shooter.Windows.Forms.SafeComboBox DDChampionship;
        private SafeLabel safeLabel8;
        private Label label6;
        private Allberg.Shooter.Windows.Forms.SafeComboBox DDPatrolConnectionType;
        private TextBox txtShooterFee4;
        private Label label9;
        private TextBox txtShooterFee3;
        private Label label8;
        private TextBox txtShooterFee2;
        private Label label7;
        private Allberg.Shooter.Windows.Forms.SafeCheckBox chkOneClass;
        private System.ComponentModel.IContainer components;

        public delegate void EnableMainHandler();
        public event EnableMainHandler EnableMain;

        internal FCompetitionField(ref Common.Interface newCommon)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            CommonCode = newCommon;

            height = Size.Height;
            width = Size.Width;
            Resize += new EventHandler(resize);
        }

        Common.Interface CommonCode;
        int height;
        int width;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FCompetition: Dispose(" + disposing.ToString() + ")" +
                "from thread \"" + System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            Visible = false;
            try
            {
                if (!DisposeNow)
                    EnableMain();
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FCompetition: exception while disposing:" + exc.ToString());
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
            Trace.WriteLine("FCompetition: Dispose(" + disposing.ToString() + ")" +
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCompetitionField));
            this.lblName = new SafeLabel();
            this.txtName = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.btnCancel = new SafeButton();
            this.lblStartDate = new SafeLabel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.lblStartTime = new SafeLabel();
            this.numStartHour = new System.Windows.Forms.NumericUpDown();
            this.lblPatrolTime = new SafeLabel();
            this.numPatrolTime = new System.Windows.Forms.NumericUpDown();
            this.SafeLabel1 = new SafeLabel();
            this.numPatrolTimeBetween = new System.Windows.Forms.NumericUpDown();
            this.numPatrolSize = new System.Windows.Forms.NumericUpDown();
            this.SafeLabel2 = new SafeLabel();
            this.SafeLabel3 = new SafeLabel();
            this.numPatrolRest = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new SafeButton();
            this.chkNorwegianCount = new System.Windows.Forms.CheckBox();
            this.numStartMinute = new System.Windows.Forms.NumericUpDown();
            this.SafeLabel4 = new SafeLabel();
            this.SafeLabel5 = new SafeLabel();
            this.SafeLabel6 = new SafeLabel();
            this.SafeLabel7 = new SafeLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkFinal = new System.Windows.Forms.CheckBox();
            this.DDPatrolConnectionType = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.chkUsePriceMoney = new System.Windows.Forms.CheckBox();
            this.numPriceMoneyReturn = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtShooterFee4 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtShooterFee3 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtShooterFee2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numShoterPercentWithPrice = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFirstPrice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtShooterFee1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCompetitionType = new System.Windows.Forms.Label();
            this.DDChampionship = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.safeLabel8 = new SafeLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.chkOneClass = new Allberg.Shooter.Windows.Forms.SafeCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numStartHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatrolTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatrolTimeBetween)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatrolSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatrolRest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPriceMoneyReturn)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShoterPercentWithPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(8, 32);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Namn";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(112, 32);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(296, 20);
            this.txtName.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtName, "Fyll i namnet på tävlingen. Detta kommer att bl.a. visas på utskrifter.");
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(329, 288);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Stäng";
            this.toolTip1.SetToolTip(this.btnCancel, "Stäng fönstret utan att spara.");
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStartDate
            // 
            this.lblStartDate.Location = new System.Drawing.Point(8, 56);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(100, 23);
            this.lblStartDate.TabIndex = 3;
            this.lblStartDate.Text = "Startdatum";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(112, 56);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(152, 20);
            this.dateTimePicker1.TabIndex = 2;
            this.toolTip1.SetToolTip(this.dateTimePicker1, "Fyll i datum för tävlingen");
            // 
            // lblStartTime
            // 
            this.lblStartTime.Location = new System.Drawing.Point(8, 80);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(100, 23);
            this.lblStartTime.TabIndex = 5;
            this.lblStartTime.Text = "Starttid";
            // 
            // numStartHour
            // 
            this.numStartHour.Location = new System.Drawing.Point(112, 80);
            this.numStartHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numStartHour.Name = "numStartHour";
            this.numStartHour.Size = new System.Drawing.Size(40, 20);
            this.numStartHour.TabIndex = 3;
            this.numStartHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.numStartHour, "Fyll i första patrullens starttid (timme)");
            this.numStartHour.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numStartHour.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numStartHour_KeyUp);
            // 
            // lblPatrolTime
            // 
            this.lblPatrolTime.Location = new System.Drawing.Point(8, 104);
            this.lblPatrolTime.Name = "lblPatrolTime";
            this.lblPatrolTime.Size = new System.Drawing.Size(100, 23);
            this.lblPatrolTime.TabIndex = 7;
            this.lblPatrolTime.Text = "Patrulltid";
            // 
            // numPatrolTime
            // 
            this.numPatrolTime.Location = new System.Drawing.Point(112, 104);
            this.numPatrolTime.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.numPatrolTime.Name = "numPatrolTime";
            this.numPatrolTime.Size = new System.Drawing.Size(40, 20);
            this.numPatrolTime.TabIndex = 5;
            this.numPatrolTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.numPatrolTime, "Fyll i hur lång tid en patrull beräknas ta på sig för att gå ett varv");
            this.numPatrolTime.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numPatrolTime.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numPatrolTime_KeyUp);
            // 
            // SafeLabel1
            // 
            this.SafeLabel1.Location = new System.Drawing.Point(8, 128);
            this.SafeLabel1.Name = "SafeLabel1";
            this.SafeLabel1.Size = new System.Drawing.Size(104, 23);
            this.SafeLabel1.TabIndex = 9;
            this.SafeLabel1.Text = "Tid mellan patruller";
            // 
            // numPatrolTimeBetween
            // 
            this.numPatrolTimeBetween.Location = new System.Drawing.Point(112, 128);
            this.numPatrolTimeBetween.Name = "numPatrolTimeBetween";
            this.numPatrolTimeBetween.Size = new System.Drawing.Size(40, 20);
            this.numPatrolTimeBetween.TabIndex = 6;
            this.numPatrolTimeBetween.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.numPatrolTimeBetween, "Fyll i hur lång tid det ska vara mellan patrullerna");
            this.numPatrolTimeBetween.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPatrolTimeBetween.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numPatrolTimeBetween_KeyUp);
            // 
            // numPatrolSize
            // 
            this.numPatrolSize.Location = new System.Drawing.Point(112, 152);
            this.numPatrolSize.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numPatrolSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPatrolSize.Name = "numPatrolSize";
            this.numPatrolSize.Size = new System.Drawing.Size(40, 20);
            this.numPatrolSize.TabIndex = 7;
            this.numPatrolSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.numPatrolSize, "Fyll i patrullens maxstorlek");
            this.numPatrolSize.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numPatrolSize.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numPatrolSize_KeyUp);
            // 
            // SafeLabel2
            // 
            this.SafeLabel2.Location = new System.Drawing.Point(8, 152);
            this.SafeLabel2.Name = "SafeLabel2";
            this.SafeLabel2.Size = new System.Drawing.Size(100, 23);
            this.SafeLabel2.TabIndex = 12;
            this.SafeLabel2.Text = "Patrullstorlek";
            // 
            // SafeLabel3
            // 
            this.SafeLabel3.Location = new System.Drawing.Point(8, 176);
            this.SafeLabel3.Name = "SafeLabel3";
            this.SafeLabel3.Size = new System.Drawing.Size(100, 23);
            this.SafeLabel3.TabIndex = 13;
            this.SafeLabel3.Text = "Vilotid";
            // 
            // numPatrolRest
            // 
            this.numPatrolRest.Location = new System.Drawing.Point(112, 176);
            this.numPatrolRest.Name = "numPatrolRest";
            this.numPatrolRest.Size = new System.Drawing.Size(40, 20);
            this.numPatrolRest.TabIndex = 8;
            this.numPatrolRest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.numPatrolRest, "Fyll i vilotiden. Det är tiden från att en patrull ska vara tillbaka tills en sky" +
                    "tt kan planeras in i nästa patrull");
            this.numPatrolRest.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numPatrolRest_KeyUp);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(249, 288);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Spara";
            this.toolTip1.SetToolTip(this.btnSave, "Spara tävlingsinformation samt stäng fönstret");
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkNorwegianCount
            // 
            this.chkNorwegianCount.Location = new System.Drawing.Point(11, 268);
            this.chkNorwegianCount.Name = "chkNorwegianCount";
            this.chkNorwegianCount.Size = new System.Drawing.Size(120, 24);
            this.chkNorwegianCount.TabIndex = 9;
            this.chkNorwegianCount.Text = "Poängfältskjutning";
            this.toolTip1.SetToolTip(this.chkNorwegianCount, "Här väljer du om det ska vara poängfältskjutning, vilket också kallas för norsk r" +
                    "äkning");
            // 
            // numStartMinute
            // 
            this.numStartMinute.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numStartMinute.Location = new System.Drawing.Point(160, 80);
            this.numStartMinute.Maximum = new decimal(new int[] {
            55,
            0,
            0,
            0});
            this.numStartMinute.Name = "numStartMinute";
            this.numStartMinute.Size = new System.Drawing.Size(40, 20);
            this.numStartMinute.TabIndex = 4;
            this.numStartMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.numStartMinute, "Fyll i första patrullens starttid (minut)");
            this.numStartMinute.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numStartMinute_KeyUp);
            // 
            // SafeLabel4
            // 
            this.SafeLabel4.Location = new System.Drawing.Point(160, 104);
            this.SafeLabel4.Name = "SafeLabel4";
            this.SafeLabel4.Size = new System.Drawing.Size(48, 23);
            this.SafeLabel4.TabIndex = 14;
            this.SafeLabel4.Text = "minuter";
            // 
            // SafeLabel5
            // 
            this.SafeLabel5.Location = new System.Drawing.Point(160, 128);
            this.SafeLabel5.Name = "SafeLabel5";
            this.SafeLabel5.Size = new System.Drawing.Size(48, 23);
            this.SafeLabel5.TabIndex = 15;
            this.SafeLabel5.Text = "minuter";
            // 
            // SafeLabel6
            // 
            this.SafeLabel6.Location = new System.Drawing.Point(160, 176);
            this.SafeLabel6.Name = "SafeLabel6";
            this.SafeLabel6.Size = new System.Drawing.Size(48, 23);
            this.SafeLabel6.TabIndex = 16;
            this.SafeLabel6.Text = "minuter";
            // 
            // SafeLabel7
            // 
            this.SafeLabel7.Location = new System.Drawing.Point(160, 152);
            this.SafeLabel7.Name = "SafeLabel7";
            this.SafeLabel7.Size = new System.Drawing.Size(48, 23);
            this.SafeLabel7.TabIndex = 17;
            this.SafeLabel7.Text = "stycken";
            // 
            // chkFinal
            // 
            this.chkFinal.Location = new System.Drawing.Point(11, 248);
            this.chkFinal.Name = "chkFinal";
            this.chkFinal.Size = new System.Drawing.Size(88, 24);
            this.chkFinal.TabIndex = 10;
            this.chkFinal.Text = "Särskjutning";
            this.toolTip1.SetToolTip(this.chkFinal, "Här väljer du om det ska vara särskjutning");
            // 
            // DDPatrolConnectionType
            // 
            this.DDPatrolConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DDPatrolConnectionType.FormattingEnabled = true;
            this.DDPatrolConnectionType.Items.AddRange(new object[] {
            "A,B,C,R,M",
            "A+R,B+C,M",
            "A+R,B,C,M",
            "A+R+B+C+M"});
            this.DDPatrolConnectionType.Location = new System.Drawing.Point(112, 227);
            this.DDPatrolConnectionType.Name = "DDPatrolConnectionType";
            this.DDPatrolConnectionType.Size = new System.Drawing.Size(96, 21);
            this.DDPatrolConnectionType.TabIndex = 28;
            this.toolTip1.SetToolTip(this.DDPatrolConnectionType, "Tillsammans innebär att klass B och C respektive A och R kan läggas i samma patru" +
                    "ll. Enskild innebär att en patrull enbart kan innehålla en klass i taget.");
            this.DDPatrolConnectionType.SelectedIndexChanged += new System.EventHandler(this.DDPatrolConnectionType_SelectedIndexChanged);
            // 
            // chkUsePriceMoney
            // 
            this.chkUsePriceMoney.Location = new System.Drawing.Point(8, 16);
            this.chkUsePriceMoney.Name = "chkUsePriceMoney";
            this.chkUsePriceMoney.Size = new System.Drawing.Size(88, 24);
            this.chkUsePriceMoney.TabIndex = 11;
            this.chkUsePriceMoney.Text = "Prispengar";
            this.chkUsePriceMoney.CheckedChanged += new System.EventHandler(this.chkUsePriceMoney_CheckedChanged);
            // 
            // numPriceMoneyReturn
            // 
            this.numPriceMoneyReturn.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numPriceMoneyReturn.Location = new System.Drawing.Point(136, 32);
            this.numPriceMoneyReturn.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numPriceMoneyReturn.Name = "numPriceMoneyReturn";
            this.numPriceMoneyReturn.ReadOnly = true;
            this.numPriceMoneyReturn.Size = new System.Drawing.Size(48, 20);
            this.numPriceMoneyReturn.TabIndex = 12;
            this.numPriceMoneyReturn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPriceMoneyReturn.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 23);
            this.label1.TabIndex = 21;
            this.label1.Text = "Återbetalning (%)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtShooterFee4);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtShooterFee3);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtShooterFee2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.numShoterPercentWithPrice);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtFirstPrice);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtShooterFee1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkUsePriceMoney);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numPriceMoneyReturn);
            this.groupBox1.Location = new System.Drawing.Point(216, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 202);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Prispengar";
            // 
            // txtShooterFee4
            // 
            this.txtShooterFee4.Location = new System.Drawing.Point(120, 176);
            this.txtShooterFee4.Name = "txtShooterFee4";
            this.txtShooterFee4.ReadOnly = true;
            this.txtShooterFee4.Size = new System.Drawing.Size(64, 20);
            this.txtShooterFee4.TabIndex = 31;
            this.txtShooterFee4.Text = "100";
            this.txtShooterFee4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 23);
            this.label9.TabIndex = 32;
            this.label9.Text = "Anmälningsavgift v4";
            // 
            // txtShooterFee3
            // 
            this.txtShooterFee3.Location = new System.Drawing.Point(120, 153);
            this.txtShooterFee3.Name = "txtShooterFee3";
            this.txtShooterFee3.ReadOnly = true;
            this.txtShooterFee3.Size = new System.Drawing.Size(64, 20);
            this.txtShooterFee3.TabIndex = 29;
            this.txtShooterFee3.Text = "100";
            this.txtShooterFee3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 23);
            this.label8.TabIndex = 30;
            this.label8.Text = "Anmälningsavgift v3";
            // 
            // txtShooterFee2
            // 
            this.txtShooterFee2.Location = new System.Drawing.Point(120, 130);
            this.txtShooterFee2.Name = "txtShooterFee2";
            this.txtShooterFee2.ReadOnly = true;
            this.txtShooterFee2.Size = new System.Drawing.Size(64, 20);
            this.txtShooterFee2.TabIndex = 27;
            this.txtShooterFee2.Text = "100";
            this.txtShooterFee2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 23);
            this.label7.TabIndex = 28;
            this.label7.Text = "Anmälningsavgift v2";
            // 
            // numShoterPercentWithPrice
            // 
            this.numShoterPercentWithPrice.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numShoterPercentWithPrice.Location = new System.Drawing.Point(136, 56);
            this.numShoterPercentWithPrice.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numShoterPercentWithPrice.Name = "numShoterPercentWithPrice";
            this.numShoterPercentWithPrice.ReadOnly = true;
            this.numShoterPercentWithPrice.Size = new System.Drawing.Size(48, 20);
            this.numShoterPercentWithPrice.TabIndex = 13;
            this.numShoterPercentWithPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numShoterPercentWithPrice.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 16);
            this.label4.TabIndex = 26;
            this.label4.Text = "Skyttar med pris (%)";
            // 
            // txtFirstPrice
            // 
            this.txtFirstPrice.Location = new System.Drawing.Point(120, 82);
            this.txtFirstPrice.Name = "txtFirstPrice";
            this.txtFirstPrice.ReadOnly = true;
            this.txtFirstPrice.Size = new System.Drawing.Size(64, 20);
            this.txtFirstPrice.TabIndex = 15;
            this.txtFirstPrice.Text = "250";
            this.txtFirstPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 24;
            this.label3.Text = "Förstapris";
            // 
            // txtShooterFee1
            // 
            this.txtShooterFee1.Location = new System.Drawing.Point(120, 107);
            this.txtShooterFee1.Name = "txtShooterFee1";
            this.txtShooterFee1.ReadOnly = true;
            this.txtShooterFee1.Size = new System.Drawing.Size(64, 20);
            this.txtShooterFee1.TabIndex = 14;
            this.txtShooterFee1.Text = "100";
            this.txtShooterFee1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 23);
            this.label2.TabIndex = 22;
            this.label2.Text = "Anmälningsavgift v1";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 23;
            this.label5.Text = "Tävlingstyp";
            // 
            // lblCompetitionType
            // 
            this.lblCompetitionType.Location = new System.Drawing.Point(120, 8);
            this.lblCompetitionType.Name = "lblCompetitionType";
            this.lblCompetitionType.Size = new System.Drawing.Size(100, 23);
            this.lblCompetitionType.TabIndex = 24;
            this.lblCompetitionType.Text = "Fälttävlan";
            // 
            // DDChampionship
            // 
            this.DDChampionship.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DDChampionship.FormattingEnabled = true;
            this.DDChampionship.Items.AddRange(new object[] {
            "Klubbtävling",
            "Nationellt tävling",
            "Kretsmästerskap",
            "Landsdelsmästerskap",
            "SM"});
            this.DDChampionship.Location = new System.Drawing.Point(112, 200);
            this.DDChampionship.Name = "DDChampionship";
            this.DDChampionship.Size = new System.Drawing.Size(96, 21);
            this.DDChampionship.TabIndex = 25;
            // 
            // safeLabel8
            // 
            this.safeLabel8.AutoSize = true;
            this.safeLabel8.Location = new System.Drawing.Point(10, 203);
            this.safeLabel8.Name = "safeLabel8";
            this.safeLabel8.Size = new System.Drawing.Size(62, 13);
            this.safeLabel8.TabIndex = 26;
            this.safeLabel8.Text = "Mästerskap";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Patrulltyp";
            // 
            // chkOneClass
            // 
            this.chkOneClass.AutoSize = true;
            this.chkOneClass.Location = new System.Drawing.Point(11, 289);
            this.chkOneClass.Name = "chkOneClass";
            this.chkOneClass.Size = new System.Drawing.Size(128, 17);
            this.chkOneClass.TabIndex = 29;
            this.chkOneClass.Text = "Slå samman klass 1-3";
            this.chkOneClass.UseVisualStyleBackColor = true;
            // 
            // FCompetitionField
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(416, 316);
            this.Controls.Add(this.chkOneClass);
            this.Controls.Add(this.DDPatrolConnectionType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.safeLabel8);
            this.Controls.Add(this.DDChampionship);
            this.Controls.Add(this.lblCompetitionType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkFinal);
            this.Controls.Add(this.SafeLabel7);
            this.Controls.Add(this.SafeLabel6);
            this.Controls.Add(this.SafeLabel5);
            this.Controls.Add(this.SafeLabel4);
            this.Controls.Add(this.numStartMinute);
            this.Controls.Add(this.chkNorwegianCount);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.numPatrolRest);
            this.Controls.Add(this.SafeLabel3);
            this.Controls.Add(this.SafeLabel2);
            this.Controls.Add(this.numPatrolSize);
            this.Controls.Add(this.numPatrolTimeBetween);
            this.Controls.Add(this.SafeLabel1);
            this.Controls.Add(this.numPatrolTime);
            this.Controls.Add(this.lblPatrolTime);
            this.Controls.Add(this.numStartHour);
            this.Controls.Add(this.lblStartTime);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCompetitionField";
            this.Text = "Tävlingsinfo";
            ((System.ComponentModel.ISupportInitialize)(this.numStartHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatrolTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatrolTimeBetween)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatrolSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatrolRest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPriceMoneyReturn)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShoterPercentWithPrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        internal bool DisposeNow = false;
        Structs.Competition[] comps;

        internal void enableMe()
        {
            Visible = true;
            Focus();
            comps = CommonCode.GetCompetitions();
            if (comps.GetUpperBound(0) >-1)
            {
                competition = comps[0];

                txtName.Text = competition.Name;
                chkNorwegianCount.Checked = competition.NorwegianCount;
                numPatrolSize.Value = competition.PatrolSize;
                numPatrolTime.Value = competition.PatrolTime;
                numPatrolTimeBetween.Value = competition.PatrolTimeBetween;
                numPatrolRest.Value = competition.PatrolTimeRest;
                numStartHour.Value = competition.StartTime.Hour;
                numStartMinute.Value = competition.StartTime.Minute;
                dateTimePicker1.Value = competition.StartTime;
                chkFinal.Checked = competition.DoFinalShooting;
                chkUsePriceMoney.Checked = competition.UsePriceMoney;
                txtShooterFee1.Text = competition.ShooterFee1.ToString();
                txtShooterFee2.Text = competition.ShooterFee2.ToString();
                txtShooterFee3.Text = competition.ShooterFee3.ToString();
                txtShooterFee4.Text = competition.ShooterFee4.ToString();
                txtFirstPrice.Text = competition.FirstPrice.ToString();
                numPriceMoneyReturn.Value = competition.PriceMoneyPercentToReturn;
                if (competition.Type == Structs.CompetitionTypeEnum.MagnumField)
                {
                    lblCompetitionType.Text = "Magnumfälttävlan";
                    chkNorwegianCount.Visible = false;
                }
                DDChampionship.SelectedIndex = (int)competition.Championship;
                DDPatrolConnectionType.SelectedIndex = (int)competition.PatrolConnectionType;
                chkOneClass.Checked = competition.OneClass;
            }
            else
            {
                DDChampionship.SelectedIndex = 0;
                DDPatrolConnectionType.SelectedIndex = 0;
            }
        }

        Structs.Competition competition;

        private void resize(object sender, System.EventArgs e)
        {
            Size size = new Size(width, height);
            Size = size;
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            // Check input
            try
            {
                int.Parse(txtShooterFee1.Text);
            }
            catch(System.FormatException)
            {
                MessageBox.Show("Anmälningsavgift verkar innehålla annat än siffror.", 
                    "Inmatningsfel", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                txtShooterFee1.Focus();
                return;
            }
            try
            {
                int.Parse(txtFirstPrice.Text);
            }
            catch(System.FormatException)
            {
                MessageBox.Show("Förstapris verkar innehålla annat än siffror.", 
                    "Inmatningsfel", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                txtFirstPrice.Focus();
                return;
            }

            if ((Structs.PatrolConnectionTypeEnum)DDPatrolConnectionType.SelectedIndex !=
                competition.PatrolConnectionType)
            {
                if (!checkChangePatrolConnectionTypeIsPossible())
                    return;
            }

            // ok, now save
            comps = CommonCode.GetCompetitions();

            //competition.CompetitionId = comps[0].CompetitionId;
            competition.Name = txtName.Text;
            competition.Type = Structs.CompetitionTypeEnum.Field;
            competition.NorwegianCount = chkNorwegianCount.Checked;
            competition.PatrolSize = (int)numPatrolSize.Value;
            competition.PatrolTime = (int)numPatrolTime.Value;
            competition.PatrolTimeBetween = (int)numPatrolTimeBetween.Value;
            competition.PatrolTimeRest = (int)numPatrolRest.Value;
            competition.StartTime = dateTimePicker1.Value.Date;
            competition.StartTime =
                competition.StartTime.AddHours((double)numStartHour.Value)
                .AddMinutes((double)numStartMinute.Value);
            competition.DoFinalShooting = chkFinal.Checked;
            competition.UsePriceMoney = chkUsePriceMoney.Checked;
            competition.ShooterFee1 = int.Parse(txtShooterFee1.Text);
            competition.ShooterFee2 = int.Parse(txtShooterFee2.Text);
            competition.ShooterFee3 = int.Parse(txtShooterFee3.Text);
            competition.ShooterFee4 = int.Parse(txtShooterFee4.Text);
            competition.FirstPrice = int.Parse(txtFirstPrice.Text);
            competition.PriceMoneyPercentToReturn = (int)numPriceMoneyReturn.Value;
            competition.PriceMoneyShooterPercent = (int)numShoterPercentWithPrice.Value;
            competition.Championship = 
                (Structs.CompetitionChampionshipEnum)DDChampionship.SelectedIndex;
            competition.PatrolConnectionType =
                (Structs.PatrolConnectionTypeEnum)DDPatrolConnectionType.SelectedIndex;
            competition.OneClass = chkOneClass.Checked;

            if (comps.Length>0)
            {
                competition.Type = comps[0].Type;
                competition.CompetitionId = comps[0].CompetitionId;
                CommonCode.UpdateCompetition(competition);
            }
            else
                CommonCode.NewCompetition(competition);
            
            Visible = false;
            EnableMain();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Visible = false;
            EnableMain();
        }

        private void numStartHour_KeyUp(object sender, KeyEventArgs e)
        {
            if (numStartHour.Value > numStartHour.Maximum)
                numStartHour.Value = numStartHour.Maximum;
        }

        private void numStartMinute_KeyUp(object sender, KeyEventArgs e)
        {
            if (numStartMinute.Value > numStartMinute.Maximum)
                numStartMinute.Value = numStartMinute.Maximum;
        }

        private void numPatrolTime_KeyUp(object sender, KeyEventArgs e)
        {
            if (numPatrolTime.Value > numPatrolTime.Maximum)
                numPatrolTime.Value = numPatrolTime.Maximum;
        }

        private void numPatrolTimeBetween_KeyUp(object sender, KeyEventArgs e)
        {
            if (numPatrolTimeBetween.Value > numPatrolTimeBetween.Maximum)
                numPatrolTimeBetween.Value = numPatrolTimeBetween.Maximum;
        }

        private void numPatrolSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (numPatrolSize.Value > numPatrolSize.Maximum)
                numPatrolSize.Value = numPatrolSize.Maximum;
        }

        private void numPatrolRest_KeyUp(object sender, KeyEventArgs e)
        {
            if (numPatrolRest.Value > numPatrolRest.Maximum)
                numPatrolRest.Value = numPatrolRest.Maximum;
        }

        private void chkUsePriceMoney_CheckedChanged(object sender, System.EventArgs e)
        {
            txtFirstPrice.ReadOnly = !chkUsePriceMoney.Checked;
            txtShooterFee1.ReadOnly = !chkUsePriceMoney.Checked;
            txtShooterFee2.ReadOnly = !chkUsePriceMoney.Checked;
            txtShooterFee3.ReadOnly = !chkUsePriceMoney.Checked;
            txtShooterFee4.ReadOnly = !chkUsePriceMoney.Checked;
            numPriceMoneyReturn.ReadOnly = !chkUsePriceMoney.Checked;
            numShoterPercentWithPrice.ReadOnly = !chkUsePriceMoney.Checked;
        }

        private bool checkChangePatrolConnectionTypeIsPossible()
        {
            Structs.PatrolConnectionTypeEnum newPatrolConnectionType =
                (Structs.PatrolConnectionTypeEnum)DDPatrolConnectionType.SelectedIndex;
            Structs.Patrol[] patrols = CommonCode.GetPatrols();

            string listOfPatrolsThatFailedCheck = "";
            foreach (Structs.Patrol patrol in patrols)
            {
                if (!CommonCode.CheckChangePatrolConnectionTypeIsPossible(patrol, newPatrolConnectionType))
                {
                    listOfPatrolsThatFailedCheck += patrol.PatrolId.ToString() + ",";
                }
            }
            if (listOfPatrolsThatFailedCheck.Length > 0)
            {
                listOfPatrolsThatFailedCheck = 
                    listOfPatrolsThatFailedCheck.Substring(0, listOfPatrolsThatFailedCheck.Length - 1);

                MessageBox.Show("Du kan inte ändra hur patruller kopplas samman " +
                    "eftersom befintliga patruller inte uppfyller det nya kravet. " +
                    "Patruller med detta problem är:\r\n" + listOfPatrolsThatFailedCheck, 
                    "Kan inte spara ändrad tävling", MessageBoxButtons.OK);
                return false;
            }
            else
                return true;
        }

        private void DDPatrolConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkChangePatrolConnectionTypeIsPossible();
        }
    }
}
