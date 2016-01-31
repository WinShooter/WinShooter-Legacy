using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for FCompetition.
	/// </summary>
	public class FCompetition : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCancel;
		private Allberg.Shooter.Windows.Forms.SafeLabel lblName;
		private Allberg.Shooter.Windows.Forms.SafeTextBox txtName;
		private Allberg.Shooter.Windows.Forms.SafeLabel lblStartDate;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private Allberg.Shooter.Windows.Forms.SafeLabel lblStartTime;
		private Allberg.Shooter.Windows.Forms.SafeLabel lblPatrolTime;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel1;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel2;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel3;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.CheckBox chkNorwegianCount;
		private System.Windows.Forms.NumericUpDown numStartHour;
		private System.Windows.Forms.NumericUpDown numPatrolTime;
		private System.Windows.Forms.NumericUpDown numPatrolTimeBetween;
		private System.Windows.Forms.NumericUpDown numPatrolSize;
		private System.Windows.Forms.NumericUpDown numPatrolRest;
		private System.Windows.Forms.NumericUpDown numStartMinute;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel4;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel5;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel6;
		private Allberg.Shooter.Windows.Forms.SafeLabel SafeLabel7;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox chkFinal;
		private System.Windows.Forms.CheckBox chkUsePriceMoney;
		private System.Windows.Forms.NumericUpDown numPriceMoneyReturn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtShooterFee;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtFirstPrice;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numShoterPercentWithPrice;
		private System.ComponentModel.IContainer components;

		public delegate void EnableMainHandler();
		public event EnableMainHandler EnableMain;

		internal FCompetition(ref Common.Interface newCommon)
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
				" ( " + System.AppDomain.GetCurrentThreadId().ToString() + " ) " +
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FCompetition));
			this.lblName = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.txtName = new Allberg.Shooter.Windows.Forms.SafeTextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblStartDate = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.lblStartTime = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.numStartHour = new System.Windows.Forms.NumericUpDown();
			this.lblPatrolTime = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.numPatrolTime = new System.Windows.Forms.NumericUpDown();
			this.SafeLabel1 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.numPatrolTimeBetween = new System.Windows.Forms.NumericUpDown();
			this.numPatrolSize = new System.Windows.Forms.NumericUpDown();
			this.SafeLabel2 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.SafeLabel3 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.numPatrolRest = new System.Windows.Forms.NumericUpDown();
			this.btnSave = new System.Windows.Forms.Button();
			this.chkNorwegianCount = new System.Windows.Forms.CheckBox();
			this.numStartMinute = new System.Windows.Forms.NumericUpDown();
			this.SafeLabel4 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.SafeLabel5 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.SafeLabel6 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.SafeLabel7 = new Allberg.Shooter.Windows.Forms.SafeLabel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.chkFinal = new System.Windows.Forms.CheckBox();
			this.chkUsePriceMoney = new System.Windows.Forms.CheckBox();
			this.numPriceMoneyReturn = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.numShoterPercentWithPrice = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.txtFirstPrice = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtShooterFee = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
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
			this.lblName.Location = new System.Drawing.Point(8, 8);
			this.lblName.Name = "lblName";
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Namn";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(112, 8);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(296, 20);
			this.txtName.TabIndex = 1;
			this.txtName.Text = "";
			this.toolTip1.SetToolTip(this.txtName, "Fyll i namnet på tävlingen. Detta kommer att bl.a. visas på utskrifter.");
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(328, 192);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 17;
			this.btnCancel.Text = "Stäng";
			this.toolTip1.SetToolTip(this.btnCancel, "Stäng fönstret utan att spara.");
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblStartDate
			// 
			this.lblStartDate.Location = new System.Drawing.Point(8, 32);
			this.lblStartDate.Name = "lblStartDate";
			this.lblStartDate.TabIndex = 3;
			this.lblStartDate.Text = "Startdatum";
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(112, 32);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(152, 20);
			this.dateTimePicker1.TabIndex = 2;
			this.toolTip1.SetToolTip(this.dateTimePicker1, "Fyll i datum för tävlingen");
			// 
			// lblStartTime
			// 
			this.lblStartTime.Location = new System.Drawing.Point(8, 56);
			this.lblStartTime.Name = "lblStartTime";
			this.lblStartTime.TabIndex = 5;
			this.lblStartTime.Text = "Starttid";
			// 
			// numStartHour
			// 
			this.numStartHour.Location = new System.Drawing.Point(112, 56);
			this.numStartHour.Maximum = new System.Decimal(new int[] {
																		 23,
																		 0,
																		 0,
																		 0});
			this.numStartHour.Name = "numStartHour";
			this.numStartHour.Size = new System.Drawing.Size(40, 20);
			this.numStartHour.TabIndex = 3;
			this.numStartHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.numStartHour, "Fyll i första patrullens starttid (timme)");
			this.numStartHour.Value = new System.Decimal(new int[] {
																	   8,
																	   0,
																	   0,
																	   0});
			this.numStartHour.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numStartHour_KeyUp);
			// 
			// lblPatrolTime
			// 
			this.lblPatrolTime.Location = new System.Drawing.Point(8, 80);
			this.lblPatrolTime.Name = "lblPatrolTime";
			this.lblPatrolTime.TabIndex = 7;
			this.lblPatrolTime.Text = "Patrulltid";
			// 
			// numPatrolTime
			// 
			this.numPatrolTime.Location = new System.Drawing.Point(112, 80);
			this.numPatrolTime.Maximum = new System.Decimal(new int[] {
																		  240,
																		  0,
																		  0,
																		  0});
			this.numPatrolTime.Name = "numPatrolTime";
			this.numPatrolTime.Size = new System.Drawing.Size(40, 20);
			this.numPatrolTime.TabIndex = 5;
			this.numPatrolTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.numPatrolTime, "Fyll i hur lång tid en patrull beräknas ta på sig för att gå ett varv");
			this.numPatrolTime.Value = new System.Decimal(new int[] {
																		60,
																		0,
																		0,
																		0});
			this.numPatrolTime.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numPatrolTime_KeyUp);
			// 
			// SafeLabel1
			// 
			this.SafeLabel1.Location = new System.Drawing.Point(8, 104);
			this.SafeLabel1.Name = "SafeLabel1";
			this.SafeLabel1.Size = new System.Drawing.Size(104, 23);
			this.SafeLabel1.TabIndex = 9;
			this.SafeLabel1.Text = "Tid mellan patruller";
			// 
			// numPatrolTimeBetween
			// 
			this.numPatrolTimeBetween.Location = new System.Drawing.Point(112, 104);
			this.numPatrolTimeBetween.Name = "numPatrolTimeBetween";
			this.numPatrolTimeBetween.Size = new System.Drawing.Size(40, 20);
			this.numPatrolTimeBetween.TabIndex = 6;
			this.numPatrolTimeBetween.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.numPatrolTimeBetween, "Fyll i hur lång tid det ska vara mellan patrullerna");
			this.numPatrolTimeBetween.Value = new System.Decimal(new int[] {
																			   10,
																			   0,
																			   0,
																			   0});
			this.numPatrolTimeBetween.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numPatrolTimeBetween_KeyUp);
			// 
			// numPatrolSize
			// 
			this.numPatrolSize.Location = new System.Drawing.Point(112, 128);
			this.numPatrolSize.Maximum = new System.Decimal(new int[] {
																		  15,
																		  0,
																		  0,
																		  0});
			this.numPatrolSize.Minimum = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.numPatrolSize.Name = "numPatrolSize";
			this.numPatrolSize.Size = new System.Drawing.Size(40, 20);
			this.numPatrolSize.TabIndex = 7;
			this.numPatrolSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.numPatrolSize, "Fyll i patrullens maxstorlek");
			this.numPatrolSize.Value = new System.Decimal(new int[] {
																		8,
																		0,
																		0,
																		0});
			this.numPatrolSize.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numPatrolSize_KeyUp);
			// 
			// SafeLabel2
			// 
			this.SafeLabel2.Location = new System.Drawing.Point(8, 128);
			this.SafeLabel2.Name = "SafeLabel2";
			this.SafeLabel2.TabIndex = 12;
			this.SafeLabel2.Text = "Patrullstorlek";
			// 
			// SafeLabel3
			// 
			this.SafeLabel3.Location = new System.Drawing.Point(8, 152);
			this.SafeLabel3.Name = "SafeLabel3";
			this.SafeLabel3.TabIndex = 13;
			this.SafeLabel3.Text = "Vilotid";
			// 
			// numPatrolRest
			// 
			this.numPatrolRest.Location = new System.Drawing.Point(112, 152);
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
			this.btnSave.Location = new System.Drawing.Point(248, 192);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 16;
			this.btnSave.Text = "Spara";
			this.toolTip1.SetToolTip(this.btnSave, "Spara tävlingsinformation samt stäng fönstret");
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// chkNorwegianCount
			// 
			this.chkNorwegianCount.Location = new System.Drawing.Point(72, 176);
			this.chkNorwegianCount.Name = "chkNorwegianCount";
			this.chkNorwegianCount.Size = new System.Drawing.Size(136, 24);
			this.chkNorwegianCount.TabIndex = 9;
			this.chkNorwegianCount.Text = "Poängfältskjutning";
			this.toolTip1.SetToolTip(this.chkNorwegianCount, "Här väljer du om det ska vara poängfältskjutning, vilket också kallas för norsk r" +
				"äkning");
			// 
			// numStartMinute
			// 
			this.numStartMinute.Increment = new System.Decimal(new int[] {
																			 5,
																			 0,
																			 0,
																			 0});
			this.numStartMinute.Location = new System.Drawing.Point(160, 56);
			this.numStartMinute.Maximum = new System.Decimal(new int[] {
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
			this.SafeLabel4.Location = new System.Drawing.Point(160, 80);
			this.SafeLabel4.Name = "SafeLabel4";
			this.SafeLabel4.Size = new System.Drawing.Size(48, 23);
			this.SafeLabel4.TabIndex = 14;
			this.SafeLabel4.Text = "minuter";
			// 
			// SafeLabel5
			// 
			this.SafeLabel5.Location = new System.Drawing.Point(160, 104);
			this.SafeLabel5.Name = "SafeLabel5";
			this.SafeLabel5.Size = new System.Drawing.Size(48, 23);
			this.SafeLabel5.TabIndex = 15;
			this.SafeLabel5.Text = "minuter";
			// 
			// SafeLabel6
			// 
			this.SafeLabel6.Location = new System.Drawing.Point(160, 152);
			this.SafeLabel6.Name = "SafeLabel6";
			this.SafeLabel6.Size = new System.Drawing.Size(48, 23);
			this.SafeLabel6.TabIndex = 16;
			this.SafeLabel6.Text = "minuter";
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
			this.chkFinal.Location = new System.Drawing.Point(72, 200);
			this.chkFinal.Name = "chkFinal";
			this.chkFinal.Size = new System.Drawing.Size(136, 24);
			this.chkFinal.TabIndex = 10;
			this.chkFinal.Text = "Särskjutning";
			this.toolTip1.SetToolTip(this.chkFinal, "Här väljer du om det ska vara särskjutning");
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
			this.numPriceMoneyReturn.Increment = new System.Decimal(new int[] {
																				  5,
																				  0,
																				  0,
																				  0});
			this.numPriceMoneyReturn.Location = new System.Drawing.Point(136, 32);
			this.numPriceMoneyReturn.Minimum = new System.Decimal(new int[] {
																				5,
																				0,
																				0,
																				0});
			this.numPriceMoneyReturn.Name = "numPriceMoneyReturn";
			this.numPriceMoneyReturn.ReadOnly = true;
			this.numPriceMoneyReturn.Size = new System.Drawing.Size(48, 20);
			this.numPriceMoneyReturn.TabIndex = 12;
			this.numPriceMoneyReturn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPriceMoneyReturn.Value = new System.Decimal(new int[] {
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
			this.groupBox1.Controls.Add(this.numShoterPercentWithPrice);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.txtFirstPrice);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtShooterFee);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.chkUsePriceMoney);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.numPriceMoneyReturn);
			this.groupBox1.Location = new System.Drawing.Point(216, 56);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(192, 128);
			this.groupBox1.TabIndex = 22;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Prispengar";
			// 
			// numShoterPercentWithPrice
			// 
			this.numShoterPercentWithPrice.Increment = new System.Decimal(new int[] {
																						5,
																						0,
																						0,
																						0});
			this.numShoterPercentWithPrice.Location = new System.Drawing.Point(136, 56);
			this.numShoterPercentWithPrice.Minimum = new System.Decimal(new int[] {
																					  5,
																					  0,
																					  0,
																					  0});
			this.numShoterPercentWithPrice.Name = "numShoterPercentWithPrice";
			this.numShoterPercentWithPrice.ReadOnly = true;
			this.numShoterPercentWithPrice.Size = new System.Drawing.Size(48, 20);
			this.numShoterPercentWithPrice.TabIndex = 13;
			this.numShoterPercentWithPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numShoterPercentWithPrice.Value = new System.Decimal(new int[] {
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
			this.txtFirstPrice.Location = new System.Drawing.Point(120, 104);
			this.txtFirstPrice.Name = "txtFirstPrice";
			this.txtFirstPrice.ReadOnly = true;
			this.txtFirstPrice.Size = new System.Drawing.Size(64, 20);
			this.txtFirstPrice.TabIndex = 15;
			this.txtFirstPrice.Text = "250";
			this.txtFirstPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 20);
			this.label3.TabIndex = 24;
			this.label3.Text = "Förstapris";
			// 
			// txtShooterFee
			// 
			this.txtShooterFee.Location = new System.Drawing.Point(120, 80);
			this.txtShooterFee.Name = "txtShooterFee";
			this.txtShooterFee.ReadOnly = true;
			this.txtShooterFee.Size = new System.Drawing.Size(64, 20);
			this.txtShooterFee.TabIndex = 14;
			this.txtShooterFee.Text = "100";
			this.txtShooterFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.TabIndex = 22;
			this.label2.Text = "Anmälningsavgift";
			// 
			// FCompetition
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(416, 230);
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
			this.Name = "FCompetition";
			this.Text = "Tävlingsinfo";
			((System.ComponentModel.ISupportInitialize)(this.numStartHour)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPatrolTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPatrolTimeBetween)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPatrolSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPatrolRest)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numStartMinute)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceMoneyReturn)).EndInit();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numShoterPercentWithPrice)).EndInit();
			this.ResumeLayout(false);

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
				this.chkNorwegianCount.Checked = competition.NorwegianCount;
				this.numPatrolSize.Value = competition.PatrolSize;
				this.numPatrolTime.Value = competition.PatrolTime;
				this.numPatrolTimeBetween.Value = competition.PatrolTimeBetween;
				this.numPatrolRest.Value = competition.PatrolTimeRest;
				this.numStartHour.Value = competition.StartTime.Hour;
				this.numStartMinute.Value = competition.StartTime.Minute;
				this.dateTimePicker1.Value = competition.StartTime;
				this.chkFinal.Checked = competition.DoFinalShooting;
				this.chkUsePriceMoney.Checked = competition.UsePriceMoney;
				this.txtShooterFee.Text = competition.ShooterFee.ToString();
				this.txtFirstPrice.Text = competition.FirstPrice.ToString();
				this.numPriceMoneyReturn.Value = competition.PriceMoneyPercentToReturn;
			}
			else
			{
				/*competition = new Common.Interface.Competition();
				competition.Name = this.txtName.Text;
				competition.NorwegianCount = this.chkNorwegianCount.Checked;
				competition.PatrolSize = (int)this.numPatrolSize.Value;
				competition.PatrolTime = (int)this.numPatrolTime.Value;
				competition.PatrolTimeBetween = (int)this.numPatrolTimeBetween.Value;
				competition.PatrolTimeRest = (int)this.numPatrolRest.Value;
				competition.StartTime = this.dateTimePicker1.Value.Date;
				competition.StartTime =
					competition.StartTime.AddHours((double)this.numStartHour.Value)
					.AddMinutes((double)this.numStartMinute.Value);
				CommonCode.NewCompetition(competition);*/
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
				int.Parse(this.txtShooterFee.Text);
			}
			catch(System.FormatException)
			{
				MessageBox.Show("Anmälningsavgift verkar innehålla annat än siffror.", 
					"Inmatningsfel", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Warning);
				this.txtShooterFee.Focus();
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

			// ok, now save
			comps = CommonCode.GetCompetitions();

			//competition.CompetitionId = comps[0].CompetitionId;
			competition.Name = this.txtName.Text;
			competition.NorwegianCount = this.chkNorwegianCount.Checked;
			competition.PatrolSize = (int)this.numPatrolSize.Value;
			competition.PatrolTime = (int)this.numPatrolTime.Value;
			competition.PatrolTimeBetween = (int)this.numPatrolTimeBetween.Value;
			competition.PatrolTimeRest = (int)this.numPatrolRest.Value;
			competition.StartTime = this.dateTimePicker1.Value.Date;
			competition.StartTime =
				competition.StartTime.AddHours((double)this.numStartHour.Value)
				.AddMinutes((double)this.numStartMinute.Value);
			competition.DoFinalShooting = this.chkFinal.Checked;
			competition.UsePriceMoney = this.chkUsePriceMoney.Checked;
			competition.ShooterFee = int.Parse(this.txtShooterFee.Text);
			competition.FirstPrice = int.Parse(this.txtFirstPrice.Text);
			competition.PriceMoneyPercentToReturn = (int)this.numPriceMoneyReturn.Value;
			competition.PriceMoneyShooterPercent = (int)numShoterPercentWithPrice.Value;

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

		private void numPatrolTime_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.numPatrolTime.Value > this.numPatrolTime.Maximum)
				this.numPatrolTime.Value = this.numPatrolTime.Maximum;
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

		private void numPatrolRest_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.numPatrolRest.Value > this.numPatrolRest.Maximum)
				this.numPatrolRest.Value = this.numPatrolRest.Maximum;
		}

		private void chkUsePriceMoney_CheckedChanged(object sender, System.EventArgs e)
		{
			this.txtFirstPrice.ReadOnly = !this.chkUsePriceMoney.Checked;
			this.txtShooterFee.ReadOnly = !this.chkUsePriceMoney.Checked;
			this.numPriceMoneyReturn.ReadOnly = !this.chkUsePriceMoney.Checked;
			this.numShoterPercentWithPrice.ReadOnly = !this.chkUsePriceMoney.Checked;
		}
	}
}
