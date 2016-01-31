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
// $Id: FCompetitionPrecision.cs 130 2011-05-28 17:32:36Z smuda $ 
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Allberg.Shooter.Windows.Forms;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for FCompetition.
	/// </summary>
	public class FCompetitionPrecision : System.Windows.Forms.Form
	{
		private SafeButton btnCancel;
		private SafeLabel lblName;
		private Allberg.Shooter.Windows.Forms.SafeTextBox txtName;
		private SafeLabel lblStartDate;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private SafeLabel lblStartTime;
		private SafeLabel SafeLabel1;
		private SafeLabel SafeLabel2;
		private SafeButton btnSave;
		private System.Windows.Forms.NumericUpDown numStartHour;
		private System.Windows.Forms.NumericUpDown numPatrolTimeBetween;
		private System.Windows.Forms.NumericUpDown numPatrolSize;
		private System.Windows.Forms.NumericUpDown numStartMinute;
		private SafeLabel SafeLabel5;
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
		private System.Windows.Forms.Label label6;
		private SafeLabel safeLabel8;
		private Allberg.Shooter.Windows.Forms.SafeComboBox DDChampionship;
		private Allberg.Shooter.Windows.Forms.SafeComboBox DDPatrolConnectionType;
		private Label label7;
		private TextBox txtShooterFee4;
		private Label label10;
		private TextBox txtShooterFee3;
		private Label label9;
		private TextBox txtShooterFee2;
		private Label label8;
		private Allberg.Shooter.Windows.Forms.SafeCheckBox chkOneClass;
		private System.ComponentModel.IContainer components;

		public delegate void EnableMainHandler();
		public event EnableMainHandler EnableMain;

		internal FCompetitionPrecision(ref Common.Interface newCommon)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			CommonCode = newCommon;

			height = this.Size.Height;
			width = this.Size.Width;
			this.Resize += new EventHandler(this.resize);
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

			this.Visible = false;
			try
			{
				if (!this.DisposeNow)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCompetitionPrecision));
			this.lblName = new SafeLabel();
			this.txtName = new Allberg.Shooter.Windows.Forms.SafeTextBox();
			this.btnCancel = new SafeButton();
			this.lblStartDate = new SafeLabel();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.lblStartTime = new SafeLabel();
			this.numStartHour = new System.Windows.Forms.NumericUpDown();
			this.SafeLabel1 = new SafeLabel();
			this.numPatrolTimeBetween = new System.Windows.Forms.NumericUpDown();
			this.numPatrolSize = new System.Windows.Forms.NumericUpDown();
			this.SafeLabel2 = new SafeLabel();
			this.btnSave = new SafeButton();
			this.numStartMinute = new System.Windows.Forms.NumericUpDown();
			this.SafeLabel5 = new SafeLabel();
			this.SafeLabel7 = new SafeLabel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.chkFinal = new System.Windows.Forms.CheckBox();
			this.DDPatrolConnectionType = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.chkUsePriceMoney = new System.Windows.Forms.CheckBox();
			this.numPriceMoneyReturn = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtShooterFee4 = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.txtShooterFee3 = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtShooterFee2 = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.numShoterPercentWithPrice = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.txtFirstPrice = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtShooterFee1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.safeLabel8 = new SafeLabel();
			this.DDChampionship = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.chkOneClass = new Allberg.Shooter.Windows.Forms.SafeCheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numStartHour)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPatrolTimeBetween)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPatrolSize)).BeginInit();
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
			this.btnCancel.Location = new System.Drawing.Point(329, 290);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 17;
			this.btnCancel.Text = "Stäng";
			this.toolTip1.SetToolTip(this.btnCancel, "Stäng fönstret utan att spara.");
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblStartDate
			// 
			this.lblStartDate.Location = new System.Drawing.Point(8, 55);
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
			// SafeLabel1
			// 
			this.SafeLabel1.Location = new System.Drawing.Point(8, 104);
			this.SafeLabel1.Name = "SafeLabel1";
			this.SafeLabel1.Size = new System.Drawing.Size(104, 23);
			this.SafeLabel1.TabIndex = 9;
			this.SafeLabel1.Text = "Tid mellan skjutlag";
			// 
			// numPatrolTimeBetween
			// 
			this.numPatrolTimeBetween.Location = new System.Drawing.Point(112, 104);
			this.numPatrolTimeBetween.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.numPatrolTimeBetween.Name = "numPatrolTimeBetween";
			this.numPatrolTimeBetween.Size = new System.Drawing.Size(40, 20);
			this.numPatrolTimeBetween.TabIndex = 6;
			this.numPatrolTimeBetween.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.numPatrolTimeBetween, "Fyll i hur lång tid det ska vara mellan patrullerna");
			this.numPatrolTimeBetween.Value = new decimal(new int[] {
            105,
            0,
            0,
            0});
			this.numPatrolTimeBetween.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numPatrolTimeBetween_KeyUp);
			// 
			// numPatrolSize
			// 
			this.numPatrolSize.Location = new System.Drawing.Point(112, 128);
			this.numPatrolSize.Maximum = new decimal(new int[] {
            300,
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
            30,
            0,
            0,
            0});
			this.numPatrolSize.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numPatrolSize_KeyUp);
			// 
			// SafeLabel2
			// 
			this.SafeLabel2.Location = new System.Drawing.Point(8, 128);
			this.SafeLabel2.Name = "SafeLabel2";
			this.SafeLabel2.Size = new System.Drawing.Size(100, 23);
			this.SafeLabel2.TabIndex = 12;
			this.SafeLabel2.Text = "Lagstorlek";
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(249, 290);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 16;
			this.btnSave.Text = "Spara";
			this.toolTip1.SetToolTip(this.btnSave, "Spara tävlingsinformation samt stäng fönstret");
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
			// SafeLabel5
			// 
			this.SafeLabel5.Location = new System.Drawing.Point(160, 104);
			this.SafeLabel5.Name = "SafeLabel5";
			this.SafeLabel5.Size = new System.Drawing.Size(48, 23);
			this.SafeLabel5.TabIndex = 15;
			this.SafeLabel5.Text = "minuter";
			// 
			// SafeLabel7
			// 
			this.SafeLabel7.Location = new System.Drawing.Point(160, 128);
			this.SafeLabel7.Name = "SafeLabel7";
			this.SafeLabel7.Size = new System.Drawing.Size(48, 23);
			this.SafeLabel7.TabIndex = 17;
			this.SafeLabel7.Text = "stycken";
			// 
			// chkFinal
			// 
			this.chkFinal.Location = new System.Drawing.Point(80, 204);
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
			this.DDPatrolConnectionType.Location = new System.Drawing.Point(112, 177);
			this.DDPatrolConnectionType.Name = "DDPatrolConnectionType";
			this.DDPatrolConnectionType.Size = new System.Drawing.Size(96, 21);
			this.DDPatrolConnectionType.TabIndex = 30;
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
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.txtShooterFee3);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.txtShooterFee2);
			this.groupBox1.Controls.Add(this.label8);
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
			this.groupBox1.Size = new System.Drawing.Size(192, 204);
			this.groupBox1.TabIndex = 22;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Prispengar";
			// 
			// txtShooterFee4
			// 
			this.txtShooterFee4.Location = new System.Drawing.Point(120, 177);
			this.txtShooterFee4.Name = "txtShooterFee4";
			this.txtShooterFee4.ReadOnly = true;
			this.txtShooterFee4.Size = new System.Drawing.Size(64, 20);
			this.txtShooterFee4.TabIndex = 31;
			this.txtShooterFee4.Text = "100";
			this.txtShooterFee4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 177);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(112, 23);
			this.label10.TabIndex = 32;
			this.label10.Text = "Anmälningsavgift v4";
			// 
			// txtShooterFee3
			// 
			this.txtShooterFee3.Location = new System.Drawing.Point(120, 154);
			this.txtShooterFee3.Name = "txtShooterFee3";
			this.txtShooterFee3.ReadOnly = true;
			this.txtShooterFee3.Size = new System.Drawing.Size(64, 20);
			this.txtShooterFee3.TabIndex = 29;
			this.txtShooterFee3.Text = "100";
			this.txtShooterFee3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 154);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(112, 23);
			this.label9.TabIndex = 30;
			this.label9.Text = "Anmälningsavgift v3";
			// 
			// txtShooterFee2
			// 
			this.txtShooterFee2.Location = new System.Drawing.Point(120, 131);
			this.txtShooterFee2.Name = "txtShooterFee2";
			this.txtShooterFee2.ReadOnly = true;
			this.txtShooterFee2.Size = new System.Drawing.Size(64, 20);
			this.txtShooterFee2.TabIndex = 27;
			this.txtShooterFee2.Text = "100";
			this.txtShooterFee2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 131);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(112, 23);
			this.label8.TabIndex = 28;
			this.label8.Text = "Anmälningsavgift v2";
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
			this.txtShooterFee1.Location = new System.Drawing.Point(120, 108);
			this.txtShooterFee1.Name = "txtShooterFee1";
			this.txtShooterFee1.ReadOnly = true;
			this.txtShooterFee1.Size = new System.Drawing.Size(64, 20);
			this.txtShooterFee1.TabIndex = 14;
			this.txtShooterFee1.Text = "100";
			this.txtShooterFee1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 108);
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
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(120, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 23);
			this.label6.TabIndex = 24;
			this.label6.Text = "Precisionsskytte";
			// 
			// safeLabel8
			// 
			this.safeLabel8.AutoSize = true;
			this.safeLabel8.Location = new System.Drawing.Point(8, 155);
			this.safeLabel8.Name = "safeLabel8";
			this.safeLabel8.Size = new System.Drawing.Size(62, 13);
			this.safeLabel8.TabIndex = 28;
			this.safeLabel8.Text = "Mästerskap";
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
			this.DDChampionship.Location = new System.Drawing.Point(112, 152);
			this.DDChampionship.Name = "DDChampionship";
			this.DDChampionship.Size = new System.Drawing.Size(96, 21);
			this.DDChampionship.TabIndex = 27;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(8, 177);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(64, 13);
			this.label7.TabIndex = 29;
			this.label7.Text = "Skjutlagstyp";
			// 
			// chkOneClass
			// 
			this.chkOneClass.AutoSize = true;
			this.chkOneClass.Location = new System.Drawing.Point(80, 231);
			this.chkOneClass.Name = "chkOneClass";
			this.chkOneClass.Size = new System.Drawing.Size(128, 17);
			this.chkOneClass.TabIndex = 31;
			this.chkOneClass.Text = "Slå samman klass 1-3";
			this.chkOneClass.UseVisualStyleBackColor = true;
			// 
			// FCompetitionPrecision
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(416, 320);
			this.Controls.Add(this.chkOneClass);
			this.Controls.Add(this.DDPatrolConnectionType);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.safeLabel8);
			this.Controls.Add(this.DDChampionship);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.chkFinal);
			this.Controls.Add(this.SafeLabel7);
			this.Controls.Add(this.SafeLabel5);
			this.Controls.Add(this.numStartMinute);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.SafeLabel2);
			this.Controls.Add(this.numPatrolSize);
			this.Controls.Add(this.numPatrolTimeBetween);
			this.Controls.Add(this.SafeLabel1);
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
			this.Name = "FCompetitionPrecision";
			this.Text = "Tävlingsinfo";
			((System.ComponentModel.ISupportInitialize)(this.numStartHour)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPatrolTimeBetween)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPatrolSize)).EndInit();
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
			this.Visible = true;
			this.Focus();
			comps = CommonCode.GetCompetitions();
			if (comps.GetUpperBound(0) >-1)
			{
				competition = comps[0];

				this.txtName.Text = competition.Name;
				this.numPatrolSize.Value = competition.PatrolSize;
				this.numPatrolTimeBetween.Value = competition.PatrolTimeBetween;
				this.numStartHour.Value = competition.StartTime.Hour;
				this.numStartMinute.Value = competition.StartTime.Minute;
				this.dateTimePicker1.Value = competition.StartTime;
				this.chkFinal.Checked = competition.DoFinalShooting;
				this.chkUsePriceMoney.Checked = competition.UsePriceMoney;
				this.txtShooterFee1.Text = competition.ShooterFee1.ToString();
				this.txtShooterFee2.Text = competition.ShooterFee2.ToString();
				this.txtShooterFee3.Text = competition.ShooterFee3.ToString();
				this.txtShooterFee4.Text = competition.ShooterFee4.ToString();
				this.txtFirstPrice.Text = competition.FirstPrice.ToString();
				this.numPriceMoneyReturn.Value = competition.PriceMoneyPercentToReturn;
				DDChampionship.SelectedIndex = (int)competition.Championship;
				DDPatrolConnectionType.SelectedIndex = (int)competition.PatrolConnectionType;
				chkOneClass.Checked = competition.OneClass;
			}
			else
			{
				DDChampionship.SelectedIndex = (int)competition.Championship;
				DDPatrolConnectionType.SelectedIndex = 0;
			}
		}

		Structs.Competition competition;

		private void resize(object sender, System.EventArgs e)
		{
			Size size = new Size(this.width, this.height);
			this.Size = size;
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			// Check input
			try
			{
				int.Parse(this.txtShooterFee1.Text);
			}
			catch(System.FormatException)
			{
				MessageBox.Show("Anmälningsavgift verkar innehålla annat än siffror.", 
					"Inmatningsfel", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Warning);
				this.txtShooterFee1.Focus();
				return;
			}
			try
			{
				int.Parse(this.txtFirstPrice.Text);
			}
			catch(System.FormatException)
			{
				MessageBox.Show("Förstapris verkar innehålla annat än siffror.", 
					"Inmatningsfel", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Warning);
				this.txtFirstPrice.Focus();
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

			if (comps.Length >= 1)
				competition = comps[0];

			competition.Name = this.txtName.Text;
			competition.Type = Structs.CompetitionTypeEnum.Field;
			competition.NorwegianCount = false;
			competition.PatrolSize = (int)this.numPatrolSize.Value;
			competition.PatrolTimeBetween = (int)this.numPatrolTimeBetween.Value;
			competition.StartTime = this.dateTimePicker1.Value.Date;
			competition.StartTime =
				competition.StartTime.AddHours((double)this.numStartHour.Value)
				.AddMinutes((double)this.numStartMinute.Value);
			competition.DoFinalShooting = this.chkFinal.Checked;
			competition.UsePriceMoney = this.chkUsePriceMoney.Checked;
			competition.ShooterFee1 = int.Parse(this.txtShooterFee1.Text);
			competition.ShooterFee2 = int.Parse(this.txtShooterFee2.Text);
			competition.ShooterFee3 = int.Parse(this.txtShooterFee3.Text);
			competition.ShooterFee4 = int.Parse(this.txtShooterFee4.Text);
			competition.FirstPrice = int.Parse(this.txtFirstPrice.Text);
			competition.PriceMoneyPercentToReturn = (int)this.numPriceMoneyReturn.Value;
			competition.PriceMoneyShooterPercent = (int)numShoterPercentWithPrice.Value;
			competition.Type = Structs.CompetitionTypeEnum.Precision;
			competition.Championship = 
				(Structs.CompetitionChampionshipEnum)DDChampionship.SelectedIndex;
			competition.PatrolConnectionType =
				(Structs.PatrolConnectionTypeEnum)DDPatrolConnectionType.SelectedIndex;
			competition.OneClass = chkOneClass.Checked;

			if (comps.Length>0)
			{
				competition.CompetitionId = comps[0].CompetitionId;
				CommonCode.UpdateCompetition(competition);
			}
			else
				CommonCode.NewCompetition(competition);
			
			this.Visible = false;
			this.EnableMain();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Visible = false;
			this.EnableMain();
		}

		private void numStartHour_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.numStartHour.Value > this.numStartHour.Maximum)
				this.numStartHour.Value = this.numStartHour.Maximum;
		}

		private void numStartMinute_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.numStartMinute.Value > this.numStartMinute.Maximum)
				this.numStartMinute.Value = this.numStartMinute.Maximum;
		}

		private void numPatrolTimeBetween_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.numPatrolTimeBetween.Value > this.numPatrolTimeBetween.Maximum)
				this.numPatrolTimeBetween.Value = this.numPatrolTimeBetween.Maximum;
		}

		private void numPatrolSize_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.numPatrolSize.Value > this.numPatrolSize.Maximum)
				this.numPatrolSize.Value = this.numPatrolSize.Maximum;
		}

		private void chkUsePriceMoney_CheckedChanged(object sender, System.EventArgs e)
		{
			this.txtFirstPrice.ReadOnly = !this.chkUsePriceMoney.Checked;
			this.txtShooterFee1.ReadOnly = !this.chkUsePriceMoney.Checked;
			this.txtShooterFee2.ReadOnly = !this.chkUsePriceMoney.Checked;
			this.txtShooterFee3.ReadOnly = !this.chkUsePriceMoney.Checked;
			this.txtShooterFee4.ReadOnly = !this.chkUsePriceMoney.Checked;
			this.numPriceMoneyReturn.ReadOnly = !this.chkUsePriceMoney.Checked;
			this.numShoterPercentWithPrice.ReadOnly = !this.chkUsePriceMoney.Checked;
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
