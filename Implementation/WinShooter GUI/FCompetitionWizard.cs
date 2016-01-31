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
// $Id: FCompetitionWizard.cs 130 2011-05-28 17:32:36Z smuda $ 
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Allberg.Shooter.Windows.Forms;
using Allberg.Shooter.Windows.Forms.Wizard;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Windows
{
	/// <summary>
	/// Summary description for FCompetitionWizard.
	/// </summary>
	public class FCompetitionWizard : System.Windows.Forms.Form
	{
		private Wizard wizardCtrl;
		private WizardPage wizardPageWelcome;
		private WizardPage wizardPageCompetitionType;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RadioButton radioBanskytte;
		private WizardPage wizardPageFinish;

		public event MethodInvoker EnableMain;
		public event MethodInvoker CancelMain;
		private Common.Interface CommonCode;
		private WizardPage wizardPageTimeDate;
		private Header header2;
		private Header headerCompetitionType;
		private System.Windows.Forms.DateTimePicker dateTimePickerStart;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtStartTime;
		private System.Windows.Forms.RadioButton radioFieldStandard;
		private System.Windows.Forms.RadioButton radioFieldNorwegian;
		private WizardPage wizardPageFieldPatrol;
		private Header header1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private WizardPage wizardPagePriceMoney;
		private Header header3;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.NumericUpDown numPriceRepay;
		private System.Windows.Forms.NumericUpDown numPriceShooters;
		private System.Windows.Forms.Label lblPriceRepay;
		private System.Windows.Forms.Label lblPriceShooters;
		private System.Windows.Forms.CheckBox chkPriceMoney;
		private System.Windows.Forms.NumericUpDown numPriceDeposit1;
		private System.Windows.Forms.Label lblPriceFirst;
		private System.Windows.Forms.NumericUpDown numPriceFirst;
		private System.Windows.Forms.Label lblPriceDeposit1;
		private WizardPage wizardPageName;
		private Header header4;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.CheckBox chkDoFinalShooting;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private InfoContainer infoContainerBegin;
		private InfoContainer infoContainerFinish;
		private Allberg.Shooter.Windows.Forms.Wizard.Header header5;
		private Allberg.Shooter.Windows.Forms.Wizard.WizardPage wizardPagePrecisionPatrol;
		private System.Windows.Forms.NumericUpDown numFieldPatrolRestTime;
		private System.Windows.Forms.NumericUpDown numFieldPatrolSize;
		private System.Windows.Forms.NumericUpDown numFieldPatrolTimeBetween;
		private System.Windows.Forms.NumericUpDown numFieldPatrolTime;
		private System.Windows.Forms.NumericUpDown numPrecisionPatrolSize;
		private System.Windows.Forms.NumericUpDown numPrecisionPatrolTimeBetween;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.RadioButton radioFieldMagnum;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddChampionshipType;
		private Allberg.Shooter.Windows.Forms.SafeComboBox ddCompetitionType;
		private SafeLabel safeLabel1;
		private SafeLabel safeLabel3;
		private SafeLabel safeLabel2;
		private Label label21;
		private Allberg.Shooter.Windows.Forms.SafeComboBox DDFieldPatrolConnectionType;
		private Label label20;
		private Label label22;
		private Allberg.Shooter.Windows.Forms.SafeComboBox DdPrecisionPatrolConnectionType;
		private Label label23;
		private NumericUpDown numPriceDeposit4;
		private Label lblPriceDeposit4;
		private NumericUpDown numPriceDeposit3;
		private Label lblPriceDeposit3;
		private NumericUpDown numPriceDeposit2;
		private Label lblPriceDeposit2;
		private Label lblPriceDeposit;
		private Label label28;
		private Label label30;
		private Label label31;
		private NumericUpDown numPriceDeposit;
		private NumericUpDown numericUpDown1;
		private NumericUpDown numericUpDown2;
		private NumericUpDown numericUpDown3;
		private Allberg.Shooter.Windows.Forms.SafeCheckBox chkPriceUseSameDeposit;
		private SafeLabel safeLabel4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FCompetitionWizard(ref Common.Interface newCommon)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			CommonCode = newCommon;

			Trace.WriteLine("FCompetitionWizard: Creating wizard.");
			// Handle images for begin, end
			int orgImageWidth = this.infoContainerBegin.Image.Width;
			int orgImageHeight = this.infoContainerBegin.Image.Height;

			// get image
			Bitmap newBitmap = (Bitmap)CommonCode.Settings.GetWinshooterLogo(orgImageHeight, orgImageWidth);

			this.infoContainerBegin.Image = newBitmap;
			this.infoContainerFinish.Image = newBitmap;

			// Handle images for icon
			orgImageWidth = 52;
			orgImageHeight = 52;
			newBitmap = (Bitmap)CommonCode.Settings.GetWinshooterLogo(orgImageHeight, orgImageWidth);
			
			this.header1.Image = newBitmap;
			this.header2.Image = newBitmap;
			this.header3.Image = newBitmap;
			this.header4.Image = newBitmap;
			this.header5.Image = newBitmap;
			this.headerCompetitionType.Image = newBitmap;

			// Handle buttons
			this.AcceptButton = this.wizardCtrl.btnNext;
			this.CancelButton = this.wizardCtrl.btnCancel;
			this.wizardCtrl.CancelEnabled = true;
			this.wizardCtrl.CloseFromCancel +=
				new CancelEventHandler(wizardCtrl_CloseFromCancel);

			// Name page
			this.wizardPageName.ShowFromNext += 
				new EventHandler(wizardPageName_ShowFromNext);

			// TimeDate Page
			dateTimePickerStart.Value = DateTime.Now.Date;
			this.wizardPageTimeDate.ShowFromNext += 
				new EventHandler(wizardPageTimeDate_ShowFromNext);
			this.wizardPageTimeDate.CloseFromNext += 
				new PageEventHandler(wizardPageTimeDate_CloseFromNext);

			// Competition Type Page
			this.wizardPageCompetitionType.ShowFromNext += 
				new EventHandler(wizardPageCompetitionType_ShowFromNext);
			this.wizardPageCompetitionType.CloseFromNext += 
				new PageEventHandler(
				wizardPageCompetitionType_CloseFromNext);
			ddChampionshipType.SelectedIndex = 0;

			// FieldPatrol Page
			wizardPageFieldPatrol.ShowFromNext += new EventHandler(wizardPageFieldPatrol_ShowFromNext);
			wizardPageFieldPatrol.CloseFromNext += new PageEventHandler(wizardPageFieldPatrol_CloseFromNext);
			DDFieldPatrolConnectionType.SelectedIndex = 0;

			// PrecisionPatrol Page
			wizardPagePrecisionPatrol.ShowFromNext += new EventHandler(wizardPagePrecisionPatrol_ShowFromNext);
			wizardPagePrecisionPatrol.CloseFromBack += new PageEventHandler(wizardPagePrecisionPatrol_CloseFromBack);
			DdPrecisionPatrolConnectionType.SelectedIndex = 0;

			// wizardPagePriceMoney
			chkPriceUseSameDeposit.CheckedChanged += new EventHandler(chkPriceUseSameDeposit_CheckedChanged);
			numPriceDeposit1.ValueChanged += new EventHandler(numPriceDeposit1_ValueChanged);
			wizardPagePriceMoney.ShowFromNext += new EventHandler(wizardPagePriceMoney_ShowFromNext);

			// Handle finish
			this.wizardPageFinish.CloseFromNext += new PageEventHandler(wizardPageFinish_CloseFromNext);

			Trace.WriteLine("FCompetitionWizard: Created wizard.");
		}



		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCompetitionWizard));
			this.wizardCtrl = new Allberg.Shooter.Windows.Forms.Wizard.Wizard();
			this.wizardPageFieldPatrol = new Allberg.Shooter.Windows.Forms.Wizard.WizardPage();
			this.label21 = new System.Windows.Forms.Label();
			this.DDFieldPatrolConnectionType = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.label20 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.numFieldPatrolRestTime = new System.Windows.Forms.NumericUpDown();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.numFieldPatrolSize = new System.Windows.Forms.NumericUpDown();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.numFieldPatrolTimeBetween = new System.Windows.Forms.NumericUpDown();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.numFieldPatrolTime = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.header1 = new Allberg.Shooter.Windows.Forms.Wizard.Header();
			this.wizardPageCompetitionType = new Allberg.Shooter.Windows.Forms.Wizard.WizardPage();
			this.safeLabel4 = new SafeLabel();
			this.safeLabel3 = new SafeLabel();
			this.safeLabel2 = new SafeLabel();
			this.safeLabel1 = new SafeLabel();
			this.ddChampionshipType = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.radioFieldMagnum = new System.Windows.Forms.RadioButton();
			this.chkDoFinalShooting = new System.Windows.Forms.CheckBox();
			this.radioFieldNorwegian = new System.Windows.Forms.RadioButton();
			this.headerCompetitionType = new Allberg.Shooter.Windows.Forms.Wizard.Header();
			this.radioBanskytte = new System.Windows.Forms.RadioButton();
			this.radioFieldStandard = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.wizardPageTimeDate = new Allberg.Shooter.Windows.Forms.Wizard.WizardPage();
			this.txtStartTime = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
			this.header2 = new Allberg.Shooter.Windows.Forms.Wizard.Header();
			this.wizardPageName = new Allberg.Shooter.Windows.Forms.Wizard.WizardPage();
			this.label19 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.header4 = new Allberg.Shooter.Windows.Forms.Wizard.Header();
			this.wizardPageWelcome = new Allberg.Shooter.Windows.Forms.Wizard.WizardPage();
			this.infoContainerBegin = new Allberg.Shooter.Windows.Forms.Wizard.InfoContainer();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.wizardPageFinish = new Allberg.Shooter.Windows.Forms.Wizard.WizardPage();
			this.infoContainerFinish = new Allberg.Shooter.Windows.Forms.Wizard.InfoContainer();
			this.label1 = new System.Windows.Forms.Label();
			this.wizardPagePriceMoney = new Allberg.Shooter.Windows.Forms.Wizard.WizardPage();
			this.chkPriceUseSameDeposit = new Allberg.Shooter.Windows.Forms.SafeCheckBox();
			this.numPriceDeposit4 = new System.Windows.Forms.NumericUpDown();
			this.lblPriceDeposit4 = new System.Windows.Forms.Label();
			this.numPriceDeposit3 = new System.Windows.Forms.NumericUpDown();
			this.lblPriceDeposit3 = new System.Windows.Forms.Label();
			this.numPriceDeposit2 = new System.Windows.Forms.NumericUpDown();
			this.lblPriceDeposit2 = new System.Windows.Forms.Label();
			this.numPriceFirst = new System.Windows.Forms.NumericUpDown();
			this.lblPriceFirst = new System.Windows.Forms.Label();
			this.numPriceDeposit1 = new System.Windows.Forms.NumericUpDown();
			this.lblPriceDeposit1 = new System.Windows.Forms.Label();
			this.numPriceShooters = new System.Windows.Forms.NumericUpDown();
			this.numPriceRepay = new System.Windows.Forms.NumericUpDown();
			this.lblPriceShooters = new System.Windows.Forms.Label();
			this.lblPriceRepay = new System.Windows.Forms.Label();
			this.chkPriceMoney = new System.Windows.Forms.CheckBox();
			this.label17 = new System.Windows.Forms.Label();
			this.header3 = new Allberg.Shooter.Windows.Forms.Wizard.Header();
			this.wizardPagePrecisionPatrol = new Allberg.Shooter.Windows.Forms.Wizard.WizardPage();
			this.label22 = new System.Windows.Forms.Label();
			this.DdPrecisionPatrolConnectionType = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.header5 = new Allberg.Shooter.Windows.Forms.Wizard.Header();
			this.numPrecisionPatrolSize = new System.Windows.Forms.NumericUpDown();
			this.numPrecisionPatrolTimeBetween = new System.Windows.Forms.NumericUpDown();
			this.ddCompetitionType = new Allberg.Shooter.Windows.Forms.SafeComboBox();
			this.lblPriceDeposit = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.numPriceDeposit = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
			this.wizardCtrl.SuspendLayout();
			this.wizardPageFieldPatrol.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numFieldPatrolRestTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFieldPatrolSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFieldPatrolTimeBetween)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFieldPatrolTime)).BeginInit();
			this.wizardPageCompetitionType.SuspendLayout();
			this.wizardPageTimeDate.SuspendLayout();
			this.wizardPageName.SuspendLayout();
			this.wizardPageWelcome.SuspendLayout();
			this.infoContainerBegin.SuspendLayout();
			this.wizardPageFinish.SuspendLayout();
			this.infoContainerFinish.SuspendLayout();
			this.wizardPagePriceMoney.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPriceDeposit4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceDeposit3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceDeposit2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceFirst)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceDeposit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceShooters)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceRepay)).BeginInit();
			this.wizardPagePrecisionPatrol.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPrecisionPatrolSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPrecisionPatrolTimeBetween)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceDeposit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
			this.SuspendLayout();
			// 
			// wizardCtrl
			// 
			this.wizardCtrl.Controls.Add(this.wizardPagePrecisionPatrol);
			this.wizardCtrl.Controls.Add(this.wizardPageFieldPatrol);
			this.wizardCtrl.Controls.Add(this.wizardPageCompetitionType);
			this.wizardCtrl.Controls.Add(this.wizardPageTimeDate);
			this.wizardCtrl.Controls.Add(this.wizardPageName);
			this.wizardCtrl.Controls.Add(this.wizardPageWelcome);
			this.wizardCtrl.Controls.Add(this.wizardPageFinish);
			this.wizardCtrl.Controls.Add(this.wizardPagePriceMoney);
			this.wizardCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardCtrl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.wizardCtrl.Location = new System.Drawing.Point(0, 0);
			this.wizardCtrl.Name = "wizardCtrl";
			this.wizardCtrl.Pages.AddRange(new Allberg.Shooter.Windows.Forms.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageName,
            this.wizardPageTimeDate,
            this.wizardPageCompetitionType,
            this.wizardPageFieldPatrol,
            this.wizardPagePrecisionPatrol,
            this.wizardPagePriceMoney,
            this.wizardPageFinish});
			this.wizardCtrl.Size = new System.Drawing.Size(496, 358);
			this.wizardCtrl.TabIndex = 0;
			// 
			// wizardPageFieldPatrol
			// 
			this.wizardPageFieldPatrol.Controls.Add(this.label21);
			this.wizardPageFieldPatrol.Controls.Add(this.DDFieldPatrolConnectionType);
			this.wizardPageFieldPatrol.Controls.Add(this.label20);
			this.wizardPageFieldPatrol.Controls.Add(this.label16);
			this.wizardPageFieldPatrol.Controls.Add(this.numFieldPatrolRestTime);
			this.wizardPageFieldPatrol.Controls.Add(this.label15);
			this.wizardPageFieldPatrol.Controls.Add(this.label14);
			this.wizardPageFieldPatrol.Controls.Add(this.numFieldPatrolSize);
			this.wizardPageFieldPatrol.Controls.Add(this.label13);
			this.wizardPageFieldPatrol.Controls.Add(this.label12);
			this.wizardPageFieldPatrol.Controls.Add(this.numFieldPatrolTimeBetween);
			this.wizardPageFieldPatrol.Controls.Add(this.label11);
			this.wizardPageFieldPatrol.Controls.Add(this.label10);
			this.wizardPageFieldPatrol.Controls.Add(this.numFieldPatrolTime);
			this.wizardPageFieldPatrol.Controls.Add(this.label9);
			this.wizardPageFieldPatrol.Controls.Add(this.label8);
			this.wizardPageFieldPatrol.Controls.Add(this.header1);
			this.wizardPageFieldPatrol.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardPageFieldPatrol.IsFinishPage = false;
			this.wizardPageFieldPatrol.Location = new System.Drawing.Point(0, 0);
			this.wizardPageFieldPatrol.Name = "wizardPageFieldPatrol";
			this.wizardPageFieldPatrol.Size = new System.Drawing.Size(496, 310);
			this.wizardPageFieldPatrol.TabIndex = 5;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(208, 251);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(280, 33);
			this.label21.TabIndex = 31;
			this.label21.Text = "Patrulltyp definierar vilka skytteklasser som kan \"samsas\" i samma patrull. Se ma" +
				"nualen för mer info.";
			// 
			// DDFieldPatrolConnectionType
			// 
			this.DDFieldPatrolConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DDFieldPatrolConnectionType.FormattingEnabled = true;
			this.DDFieldPatrolConnectionType.Items.AddRange(new object[] {
            "A,B,C,R,M",
            "A+R,B+C,M",
            "A+R,B,C,M",
            "A+R+B+C+M"});
			this.DDFieldPatrolConnectionType.Location = new System.Drawing.Point(99, 251);
			this.DDFieldPatrolConnectionType.Name = "DDFieldPatrolConnectionType";
			this.DDFieldPatrolConnectionType.Size = new System.Drawing.Size(96, 21);
			this.DDFieldPatrolConnectionType.TabIndex = 30;
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(40, 251);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(53, 13);
			this.label20.TabIndex = 29;
			this.label20.Text = "Patrulltyp";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(208, 192);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(280, 56);
			this.label16.TabIndex = 13;
			this.label16.Text = resources.GetString("label16.Text");
			// 
			// numFieldPatrolRestTime
			// 
			this.numFieldPatrolRestTime.Location = new System.Drawing.Point(152, 192);
			this.numFieldPatrolRestTime.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
			this.numFieldPatrolRestTime.Name = "numFieldPatrolRestTime";
			this.numFieldPatrolRestTime.Size = new System.Drawing.Size(40, 21);
			this.numFieldPatrolRestTime.TabIndex = 12;
			this.numFieldPatrolRestTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(208, 160);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(280, 32);
			this.label15.TabIndex = 11;
			this.label15.Text = "Det här är max antalet skyttar i en patrull.";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(40, 192);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(100, 23);
			this.label14.TabIndex = 10;
			this.label14.Text = "Vilotid";
			// 
			// numFieldPatrolSize
			// 
			this.numFieldPatrolSize.Location = new System.Drawing.Point(152, 160);
			this.numFieldPatrolSize.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numFieldPatrolSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numFieldPatrolSize.Name = "numFieldPatrolSize";
			this.numFieldPatrolSize.Size = new System.Drawing.Size(40, 21);
			this.numFieldPatrolSize.TabIndex = 9;
			this.numFieldPatrolSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numFieldPatrolSize.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(40, 160);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(100, 23);
			this.label13.TabIndex = 8;
			this.label13.Text = "Patrullstorlek";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(208, 128);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(280, 32);
			this.label12.TabIndex = 7;
			this.label12.Text = "Det här är tiden i minuter mellan patruller. ";
			// 
			// numFieldPatrolTimeBetween
			// 
			this.numFieldPatrolTimeBetween.Location = new System.Drawing.Point(152, 128);
			this.numFieldPatrolTimeBetween.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
			this.numFieldPatrolTimeBetween.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numFieldPatrolTimeBetween.Name = "numFieldPatrolTimeBetween";
			this.numFieldPatrolTimeBetween.Size = new System.Drawing.Size(40, 21);
			this.numFieldPatrolTimeBetween.TabIndex = 6;
			this.numFieldPatrolTimeBetween.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numFieldPatrolTimeBetween.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(40, 128);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(120, 23);
			this.label11.TabIndex = 5;
			this.label11.Text = "Tid mellan patruller";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(208, 96);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(280, 32);
			this.label10.TabIndex = 4;
			this.label10.Text = "Patrulltid är den tid i minuter du beräknar att det kommer att ta för en patrull " +
				"att gå runt ett varv.";
			// 
			// numFieldPatrolTime
			// 
			this.numFieldPatrolTime.Location = new System.Drawing.Point(152, 96);
			this.numFieldPatrolTime.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
			this.numFieldPatrolTime.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numFieldPatrolTime.Name = "numFieldPatrolTime";
			this.numFieldPatrolTime.Size = new System.Drawing.Size(40, 21);
			this.numFieldPatrolTime.TabIndex = 3;
			this.numFieldPatrolTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numFieldPatrolTime.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(40, 96);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(80, 23);
			this.label9.TabIndex = 2;
			this.label9.Text = "Patrulltid";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 72);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(480, 23);
			this.label8.TabIndex = 1;
			this.label8.Text = "Här anger du information om patrullerna";
			// 
			// header1
			// 
			this.header1.BackColor = System.Drawing.SystemColors.Control;
			this.header1.CausesValidation = false;
			this.header1.Description = "";
			this.header1.Dock = System.Windows.Forms.DockStyle.Top;
			this.header1.Location = new System.Drawing.Point(0, 0);
			this.header1.Name = "header1";
			this.header1.Size = new System.Drawing.Size(496, 64);
			this.header1.TabIndex = 0;
			this.header1.Title = "Fältskytte - Information om patrullerna";
			// 
			// wizardPageCompetitionType
			// 
			this.wizardPageCompetitionType.Controls.Add(this.safeLabel4);
			this.wizardPageCompetitionType.Controls.Add(this.safeLabel3);
			this.wizardPageCompetitionType.Controls.Add(this.safeLabel2);
			this.wizardPageCompetitionType.Controls.Add(this.safeLabel1);
			this.wizardPageCompetitionType.Controls.Add(this.ddChampionshipType);
			this.wizardPageCompetitionType.Controls.Add(this.radioFieldMagnum);
			this.wizardPageCompetitionType.Controls.Add(this.chkDoFinalShooting);
			this.wizardPageCompetitionType.Controls.Add(this.radioFieldNorwegian);
			this.wizardPageCompetitionType.Controls.Add(this.headerCompetitionType);
			this.wizardPageCompetitionType.Controls.Add(this.radioBanskytte);
			this.wizardPageCompetitionType.Controls.Add(this.radioFieldStandard);
			this.wizardPageCompetitionType.Controls.Add(this.label5);
			this.wizardPageCompetitionType.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardPageCompetitionType.IsFinishPage = false;
			this.wizardPageCompetitionType.Location = new System.Drawing.Point(0, 0);
			this.wizardPageCompetitionType.Name = "wizardPageCompetitionType";
			this.wizardPageCompetitionType.Size = new System.Drawing.Size(496, 310);
			this.wizardPageCompetitionType.TabIndex = 2;
			// 
			// safeLabel4
			// 
			this.safeLabel4.AutoSize = true;
			this.safeLabel4.Location = new System.Drawing.Point(227, 262);
			this.safeLabel4.Name = "safeLabel4";
			this.safeLabel4.Size = new System.Drawing.Size(235, 13);
			this.safeLabel4.TabIndex = 12;
			this.safeLabel4.Text = "Klubbtävlingen har inte några standardmedaljer";
			// 
			// safeLabel3
			// 
			this.safeLabel3.AutoSize = true;
			this.safeLabel3.Location = new System.Drawing.Point(227, 231);
			this.safeLabel3.Name = "safeLabel3";
			this.safeLabel3.Size = new System.Drawing.Size(45, 13);
			this.safeLabel3.TabIndex = 11;
			this.safeLabel3.Text = "och SM.";
			// 
			// safeLabel2
			// 
			this.safeLabel2.AutoSize = true;
			this.safeLabel2.Location = new System.Drawing.Point(226, 217);
			this.safeLabel2.Name = "safeLabel2";
			this.safeLabel2.Size = new System.Drawing.Size(262, 13);
			this.safeLabel2.TabIndex = 10;
			this.safeLabel2.Text = "annorlunda för \"normal\" tävling, landsdelsmästerskap";
			// 
			// safeLabel1
			// 
			this.safeLabel1.AutoSize = true;
			this.safeLabel1.Location = new System.Drawing.Point(226, 204);
			this.safeLabel1.Name = "safeLabel1";
			this.safeLabel1.Size = new System.Drawing.Size(245, 13);
			this.safeLabel1.TabIndex = 9;
			this.safeLabel1.Text = "Enligt SHB så är hanteringen av standardmedaljer";
			// 
			// ddChampionshipType
			// 
			this.ddChampionshipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddChampionshipType.FormattingEnabled = true;
			this.ddChampionshipType.Items.AddRange(new object[] {
            "Klubbtävling",
            "Nationellt tävling",
            "Kretsmästerskap",
            "Landsdelsmästerskap",
            "SM"});
			this.ddChampionshipType.Location = new System.Drawing.Point(56, 239);
			this.ddChampionshipType.Name = "ddChampionshipType";
			this.ddChampionshipType.Size = new System.Drawing.Size(160, 21);
			this.ddChampionshipType.TabIndex = 8;
			// 
			// radioFieldMagnum
			// 
			this.radioFieldMagnum.Location = new System.Drawing.Point(56, 152);
			this.radioFieldMagnum.Name = "radioFieldMagnum";
			this.radioFieldMagnum.Size = new System.Drawing.Size(160, 24);
			this.radioFieldMagnum.TabIndex = 7;
			this.radioFieldMagnum.Text = "Magnumfältskyttetävling";
			// 
			// chkDoFinalShooting
			// 
			this.chkDoFinalShooting.Location = new System.Drawing.Point(56, 208);
			this.chkDoFinalShooting.Name = "chkDoFinalShooting";
			this.chkDoFinalShooting.Size = new System.Drawing.Size(360, 24);
			this.chkDoFinalShooting.TabIndex = 6;
			this.chkDoFinalShooting.Text = "Särskjutning ska användas";
			// 
			// radioFieldNorwegian
			// 
			this.radioFieldNorwegian.Location = new System.Drawing.Point(56, 128);
			this.radioFieldNorwegian.Name = "radioFieldNorwegian";
			this.radioFieldNorwegian.Size = new System.Drawing.Size(160, 24);
			this.radioFieldNorwegian.TabIndex = 5;
			this.radioFieldNorwegian.Text = "Poängfältskyttetävling";
			// 
			// headerCompetitionType
			// 
			this.headerCompetitionType.BackColor = System.Drawing.SystemColors.Control;
			this.headerCompetitionType.CausesValidation = false;
			this.headerCompetitionType.Description = "";
			this.headerCompetitionType.Dock = System.Windows.Forms.DockStyle.Top;
			this.headerCompetitionType.Location = new System.Drawing.Point(0, 0);
			this.headerCompetitionType.Name = "headerCompetitionType";
			this.headerCompetitionType.Size = new System.Drawing.Size(496, 64);
			this.headerCompetitionType.TabIndex = 4;
			this.headerCompetitionType.Title = "Typ av tävling";
			// 
			// radioBanskytte
			// 
			this.radioBanskytte.Location = new System.Drawing.Point(56, 176);
			this.radioBanskytte.Name = "radioBanskytte";
			this.radioBanskytte.Size = new System.Drawing.Size(160, 24);
			this.radioBanskytte.TabIndex = 3;
			this.radioBanskytte.Text = "Precisionstävling";
			// 
			// radioFieldStandard
			// 
			this.radioFieldStandard.Location = new System.Drawing.Point(56, 104);
			this.radioFieldStandard.Name = "radioFieldStandard";
			this.radioFieldStandard.Size = new System.Drawing.Size(160, 24);
			this.radioFieldStandard.TabIndex = 2;
			this.radioFieldStandard.Text = "Fältskyttetävling";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(360, 24);
			this.label5.TabIndex = 1;
			this.label5.Text = "Välj vilken typ av tävling som du vill skapa och tryck på Nästa.";
			// 
			// wizardPageTimeDate
			// 
			this.wizardPageTimeDate.Controls.Add(this.txtStartTime);
			this.wizardPageTimeDate.Controls.Add(this.label7);
			this.wizardPageTimeDate.Controls.Add(this.label6);
			this.wizardPageTimeDate.Controls.Add(this.label4);
			this.wizardPageTimeDate.Controls.Add(this.dateTimePickerStart);
			this.wizardPageTimeDate.Controls.Add(this.header2);
			this.wizardPageTimeDate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardPageTimeDate.IsFinishPage = false;
			this.wizardPageTimeDate.Location = new System.Drawing.Point(0, 0);
			this.wizardPageTimeDate.Name = "wizardPageTimeDate";
			this.wizardPageTimeDate.Size = new System.Drawing.Size(496, 310);
			this.wizardPageTimeDate.TabIndex = 4;
			// 
			// txtStartTime
			// 
			this.txtStartTime.Location = new System.Drawing.Point(152, 136);
			this.txtStartTime.Name = "txtStartTime";
			this.txtStartTime.Size = new System.Drawing.Size(100, 21);
			this.txtStartTime.TabIndex = 5;
			this.txtStartTime.Text = "08:00";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(48, 136);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(100, 23);
			this.label7.TabIndex = 4;
			this.label7.Text = "Tid";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(48, 112);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 23);
			this.label6.TabIndex = 3;
			this.label6.Text = "Datum";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(48, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(312, 23);
			this.label4.TabIndex = 2;
			this.label4.Text = "Välj ett datum och en tid för när tävlingen ska starta";
			// 
			// dateTimePickerStart
			// 
			this.dateTimePickerStart.Location = new System.Drawing.Point(152, 112);
			this.dateTimePickerStart.Name = "dateTimePickerStart";
			this.dateTimePickerStart.Size = new System.Drawing.Size(200, 21);
			this.dateTimePickerStart.TabIndex = 1;
			// 
			// header2
			// 
			this.header2.BackColor = System.Drawing.SystemColors.Control;
			this.header2.CausesValidation = false;
			this.header2.Description = "";
			this.header2.Location = new System.Drawing.Point(0, 0);
			this.header2.Name = "header2";
			this.header2.Size = new System.Drawing.Size(496, 64);
			this.header2.TabIndex = 0;
			this.header2.Title = "Tävlingens datum och tid";
			// 
			// wizardPageName
			// 
			this.wizardPageName.Controls.Add(this.label19);
			this.wizardPageName.Controls.Add(this.txtName);
			this.wizardPageName.Controls.Add(this.label18);
			this.wizardPageName.Controls.Add(this.header4);
			this.wizardPageName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardPageName.IsFinishPage = false;
			this.wizardPageName.Location = new System.Drawing.Point(0, 0);
			this.wizardPageName.Name = "wizardPageName";
			this.wizardPageName.Size = new System.Drawing.Size(496, 310);
			this.wizardPageName.TabIndex = 7;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(32, 184);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(392, 23);
			this.label19.TabIndex = 3;
			this.label19.Text = "Detta namn kommer att synas t.ex. på utskrifter och exporter.";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(152, 96);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(280, 21);
			this.txtName.TabIndex = 2;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(24, 96);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(120, 32);
			this.label18.TabIndex = 1;
			this.label18.Text = "Ange tävlingens namn. ";
			// 
			// header4
			// 
			this.header4.BackColor = System.Drawing.SystemColors.Control;
			this.header4.CausesValidation = false;
			this.header4.Description = "";
			this.header4.Dock = System.Windows.Forms.DockStyle.Top;
			this.header4.Location = new System.Drawing.Point(0, 0);
			this.header4.Name = "header4";
			this.header4.Size = new System.Drawing.Size(496, 64);
			this.header4.TabIndex = 0;
			this.header4.Title = "Tävlingens namn";
			// 
			// wizardPageWelcome
			// 
			this.wizardPageWelcome.Controls.Add(this.infoContainerBegin);
			this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardPageWelcome.IsFinishPage = false;
			this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
			this.wizardPageWelcome.Name = "wizardPageWelcome";
			this.wizardPageWelcome.Size = new System.Drawing.Size(496, 310);
			this.wizardPageWelcome.TabIndex = 1;
			// 
			// infoContainerBegin
			// 
			this.infoContainerBegin.BackColor = System.Drawing.Color.White;
			this.infoContainerBegin.Controls.Add(this.label3);
			this.infoContainerBegin.Controls.Add(this.label2);
			this.infoContainerBegin.Dock = System.Windows.Forms.DockStyle.Fill;
			this.infoContainerBegin.Image = ((System.Drawing.Image)(resources.GetObject("infoContainerBegin.Image")));
			this.infoContainerBegin.Location = new System.Drawing.Point(0, 0);
			this.infoContainerBegin.Name = "infoContainerBegin";
			this.infoContainerBegin.PageTitle = "Välkommen till guiden Skapa ny tävling";
			this.infoContainerBegin.Size = new System.Drawing.Size(496, 310);
			this.infoContainerBegin.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(176, 272);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(200, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Tryck på Nästa för att fortsätta.";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(168, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(320, 112);
			this.label2.TabIndex = 8;
			this.label2.Text = "Den här guiden kommer att hjälpa dig att skapa en ny tävling genom att gå igenom " +
				"ett antal steg.";
			// 
			// wizardPageFinish
			// 
			this.wizardPageFinish.Controls.Add(this.infoContainerFinish);
			this.wizardPageFinish.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardPageFinish.IsFinishPage = true;
			this.wizardPageFinish.Location = new System.Drawing.Point(0, 0);
			this.wizardPageFinish.Name = "wizardPageFinish";
			this.wizardPageFinish.Size = new System.Drawing.Size(496, 310);
			this.wizardPageFinish.TabIndex = 3;
			// 
			// infoContainerFinish
			// 
			this.infoContainerFinish.BackColor = System.Drawing.Color.White;
			this.infoContainerFinish.Controls.Add(this.label1);
			this.infoContainerFinish.Dock = System.Windows.Forms.DockStyle.Fill;
			this.infoContainerFinish.Image = ((System.Drawing.Image)(resources.GetObject("infoContainerFinish.Image")));
			this.infoContainerFinish.Location = new System.Drawing.Point(0, 0);
			this.infoContainerFinish.Name = "infoContainerFinish";
			this.infoContainerFinish.PageTitle = "Slutför guiden Skapa ny tävling";
			this.infoContainerFinish.Size = new System.Drawing.Size(496, 310);
			this.infoContainerFinish.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(176, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(320, 88);
			this.label1.TabIndex = 8;
			this.label1.Text = "När du trycker på slutför så skapas tävlingen utifrån det värden som du nyss mata" +
				"t in.";
			// 
			// wizardPagePriceMoney
			// 
			this.wizardPagePriceMoney.Controls.Add(this.chkPriceUseSameDeposit);
			this.wizardPagePriceMoney.Controls.Add(this.numPriceDeposit4);
			this.wizardPagePriceMoney.Controls.Add(this.lblPriceDeposit4);
			this.wizardPagePriceMoney.Controls.Add(this.numPriceDeposit3);
			this.wizardPagePriceMoney.Controls.Add(this.lblPriceDeposit3);
			this.wizardPagePriceMoney.Controls.Add(this.numPriceDeposit2);
			this.wizardPagePriceMoney.Controls.Add(this.lblPriceDeposit2);
			this.wizardPagePriceMoney.Controls.Add(this.numPriceFirst);
			this.wizardPagePriceMoney.Controls.Add(this.lblPriceFirst);
			this.wizardPagePriceMoney.Controls.Add(this.numPriceDeposit1);
			this.wizardPagePriceMoney.Controls.Add(this.lblPriceDeposit1);
			this.wizardPagePriceMoney.Controls.Add(this.numPriceShooters);
			this.wizardPagePriceMoney.Controls.Add(this.numPriceRepay);
			this.wizardPagePriceMoney.Controls.Add(this.lblPriceShooters);
			this.wizardPagePriceMoney.Controls.Add(this.lblPriceRepay);
			this.wizardPagePriceMoney.Controls.Add(this.chkPriceMoney);
			this.wizardPagePriceMoney.Controls.Add(this.label17);
			this.wizardPagePriceMoney.Controls.Add(this.header3);
			this.wizardPagePriceMoney.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardPagePriceMoney.IsFinishPage = false;
			this.wizardPagePriceMoney.Location = new System.Drawing.Point(0, 0);
			this.wizardPagePriceMoney.Name = "wizardPagePriceMoney";
			this.wizardPagePriceMoney.Size = new System.Drawing.Size(496, 310);
			this.wizardPagePriceMoney.TabIndex = 6;
			// 
			// chkPriceUseSameDeposit
			// 
			this.chkPriceUseSameDeposit.AutoSize = true;
			this.chkPriceUseSameDeposit.Checked = true;
			this.chkPriceUseSameDeposit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkPriceUseSameDeposit.Location = new System.Drawing.Point(24, 209);
			this.chkPriceUseSameDeposit.Name = "chkPriceUseSameDeposit";
			this.chkPriceUseSameDeposit.Size = new System.Drawing.Size(242, 17);
			this.chkPriceUseSameDeposit.TabIndex = 17;
			this.chkPriceUseSameDeposit.Text = "Använd samma anmälningsavgift på alla varv";
			this.chkPriceUseSameDeposit.UseVisualStyleBackColor = true;
			this.chkPriceUseSameDeposit.CheckedChanged += new System.EventHandler(this.chkPriceUseSameDeposit_CheckedChanged);
			// 
			// numPriceDeposit4
			// 
			this.numPriceDeposit4.Enabled = false;
			this.numPriceDeposit4.Location = new System.Drawing.Point(338, 256);
			this.numPriceDeposit4.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numPriceDeposit4.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numPriceDeposit4.Name = "numPriceDeposit4";
			this.numPriceDeposit4.Size = new System.Drawing.Size(48, 21);
			this.numPriceDeposit4.TabIndex = 16;
			this.numPriceDeposit4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPriceDeposit4.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// lblPriceDeposit4
			// 
			this.lblPriceDeposit4.Location = new System.Drawing.Point(211, 256);
			this.lblPriceDeposit4.Name = "lblPriceDeposit4";
			this.lblPriceDeposit4.Size = new System.Drawing.Size(131, 23);
			this.lblPriceDeposit4.TabIndex = 15;
			this.lblPriceDeposit4.Text = "Anmälningsavgift varv 4";
			// 
			// numPriceDeposit3
			// 
			this.numPriceDeposit3.Enabled = false;
			this.numPriceDeposit3.Location = new System.Drawing.Point(151, 254);
			this.numPriceDeposit3.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numPriceDeposit3.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numPriceDeposit3.Name = "numPriceDeposit3";
			this.numPriceDeposit3.Size = new System.Drawing.Size(48, 21);
			this.numPriceDeposit3.TabIndex = 14;
			this.numPriceDeposit3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPriceDeposit3.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// lblPriceDeposit3
			// 
			this.lblPriceDeposit3.Location = new System.Drawing.Point(24, 254);
			this.lblPriceDeposit3.Name = "lblPriceDeposit3";
			this.lblPriceDeposit3.Size = new System.Drawing.Size(131, 23);
			this.lblPriceDeposit3.TabIndex = 13;
			this.lblPriceDeposit3.Text = "Anmälningsavgift varv 3";
			// 
			// numPriceDeposit2
			// 
			this.numPriceDeposit2.Enabled = false;
			this.numPriceDeposit2.Location = new System.Drawing.Point(338, 233);
			this.numPriceDeposit2.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numPriceDeposit2.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numPriceDeposit2.Name = "numPriceDeposit2";
			this.numPriceDeposit2.Size = new System.Drawing.Size(48, 21);
			this.numPriceDeposit2.TabIndex = 12;
			this.numPriceDeposit2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPriceDeposit2.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// lblPriceDeposit2
			// 
			this.lblPriceDeposit2.Location = new System.Drawing.Point(211, 233);
			this.lblPriceDeposit2.Name = "lblPriceDeposit2";
			this.lblPriceDeposit2.Size = new System.Drawing.Size(131, 23);
			this.lblPriceDeposit2.TabIndex = 11;
			this.lblPriceDeposit2.Text = "Anmälningsavgift varv 2";
			// 
			// numPriceFirst
			// 
			this.numPriceFirst.Location = new System.Drawing.Point(151, 183);
			this.numPriceFirst.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numPriceFirst.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numPriceFirst.Name = "numPriceFirst";
			this.numPriceFirst.Size = new System.Drawing.Size(48, 21);
			this.numPriceFirst.TabIndex = 10;
			this.numPriceFirst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPriceFirst.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
			// 
			// lblPriceFirst
			// 
			this.lblPriceFirst.Location = new System.Drawing.Point(24, 183);
			this.lblPriceFirst.Name = "lblPriceFirst";
			this.lblPriceFirst.Size = new System.Drawing.Size(100, 23);
			this.lblPriceFirst.TabIndex = 9;
			this.lblPriceFirst.Text = "Förstapris";
			// 
			// numPriceDeposit1
			// 
			this.numPriceDeposit1.Location = new System.Drawing.Point(151, 231);
			this.numPriceDeposit1.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numPriceDeposit1.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numPriceDeposit1.Name = "numPriceDeposit1";
			this.numPriceDeposit1.Size = new System.Drawing.Size(48, 21);
			this.numPriceDeposit1.TabIndex = 8;
			this.numPriceDeposit1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPriceDeposit1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// lblPriceDeposit1
			// 
			this.lblPriceDeposit1.Location = new System.Drawing.Point(24, 231);
			this.lblPriceDeposit1.Name = "lblPriceDeposit1";
			this.lblPriceDeposit1.Size = new System.Drawing.Size(131, 23);
			this.lblPriceDeposit1.TabIndex = 7;
			this.lblPriceDeposit1.Text = "Anmälningsavgift varv 1";
			// 
			// numPriceShooters
			// 
			this.numPriceShooters.Location = new System.Drawing.Point(151, 160);
			this.numPriceShooters.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numPriceShooters.Name = "numPriceShooters";
			this.numPriceShooters.Size = new System.Drawing.Size(48, 21);
			this.numPriceShooters.TabIndex = 6;
			this.numPriceShooters.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPriceShooters.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
			// 
			// numPriceRepay
			// 
			this.numPriceRepay.Location = new System.Drawing.Point(151, 136);
			this.numPriceRepay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numPriceRepay.Name = "numPriceRepay";
			this.numPriceRepay.Size = new System.Drawing.Size(48, 21);
			this.numPriceRepay.TabIndex = 5;
			this.numPriceRepay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPriceRepay.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
			// 
			// lblPriceShooters
			// 
			this.lblPriceShooters.Location = new System.Drawing.Point(24, 160);
			this.lblPriceShooters.Name = "lblPriceShooters";
			this.lblPriceShooters.Size = new System.Drawing.Size(120, 23);
			this.lblPriceShooters.TabIndex = 4;
			this.lblPriceShooters.Text = "Skytttar med pris (%)";
			// 
			// lblPriceRepay
			// 
			this.lblPriceRepay.Location = new System.Drawing.Point(24, 136);
			this.lblPriceRepay.Name = "lblPriceRepay";
			this.lblPriceRepay.Size = new System.Drawing.Size(120, 23);
			this.lblPriceRepay.TabIndex = 3;
			this.lblPriceRepay.Text = "Återbetalning (%)";
			// 
			// chkPriceMoney
			// 
			this.chkPriceMoney.Location = new System.Drawing.Point(24, 112);
			this.chkPriceMoney.Name = "chkPriceMoney";
			this.chkPriceMoney.Size = new System.Drawing.Size(416, 24);
			this.chkPriceMoney.TabIndex = 2;
			this.chkPriceMoney.Text = "Ja, jag vill använda Winshooters automatiska fördelning av prispengar";
			this.chkPriceMoney.CheckedChanged += new System.EventHandler(this.chkPriceMoney_CheckedChanged);
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(24, 80);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(416, 32);
			this.label17.TabIndex = 1;
			this.label17.Text = "Winshooter kan automatiskt fördela prispengar. Vill du använda automatisk prispen" +
				"gaberäkning?";
			// 
			// header3
			// 
			this.header3.BackColor = System.Drawing.SystemColors.Control;
			this.header3.CausesValidation = false;
			this.header3.Description = "";
			this.header3.Dock = System.Windows.Forms.DockStyle.Top;
			this.header3.Location = new System.Drawing.Point(0, 0);
			this.header3.Name = "header3";
			this.header3.Size = new System.Drawing.Size(496, 64);
			this.header3.TabIndex = 0;
			this.header3.Title = "Prispengar";
			// 
			// wizardPagePrecisionPatrol
			// 
			this.wizardPagePrecisionPatrol.Controls.Add(this.label22);
			this.wizardPagePrecisionPatrol.Controls.Add(this.DdPrecisionPatrolConnectionType);
			this.wizardPagePrecisionPatrol.Controls.Add(this.label23);
			this.wizardPagePrecisionPatrol.Controls.Add(this.label24);
			this.wizardPagePrecisionPatrol.Controls.Add(this.label26);
			this.wizardPagePrecisionPatrol.Controls.Add(this.label25);
			this.wizardPagePrecisionPatrol.Controls.Add(this.label27);
			this.wizardPagePrecisionPatrol.Controls.Add(this.label29);
			this.wizardPagePrecisionPatrol.Controls.Add(this.header5);
			this.wizardPagePrecisionPatrol.Controls.Add(this.numPrecisionPatrolSize);
			this.wizardPagePrecisionPatrol.Controls.Add(this.numPrecisionPatrolTimeBetween);
			this.wizardPagePrecisionPatrol.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardPagePrecisionPatrol.IsFinishPage = false;
			this.wizardPagePrecisionPatrol.Location = new System.Drawing.Point(0, 0);
			this.wizardPagePrecisionPatrol.Name = "wizardPagePrecisionPatrol";
			this.wizardPagePrecisionPatrol.Size = new System.Drawing.Size(496, 310);
			this.wizardPagePrecisionPatrol.TabIndex = 8;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(213, 164);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(280, 132);
			this.label22.TabIndex = 38;
			this.label22.Text = resources.GetString("label22.Text");
			// 
			// DdPrecisionPatrolConnectionType
			// 
			this.DdPrecisionPatrolConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DdPrecisionPatrolConnectionType.FormattingEnabled = true;
			this.DdPrecisionPatrolConnectionType.Items.AddRange(new object[] {
            "A,B,C,R,M",
            "A+R,B+C,M",
            "A+R,B,C,M",
            "A+R+B+C+M"});
			this.DdPrecisionPatrolConnectionType.Location = new System.Drawing.Point(104, 164);
			this.DdPrecisionPatrolConnectionType.Name = "DdPrecisionPatrolConnectionType";
			this.DdPrecisionPatrolConnectionType.Size = new System.Drawing.Size(96, 21);
			this.DdPrecisionPatrolConnectionType.TabIndex = 37;
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(40, 168);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(66, 13);
			this.label23.TabIndex = 36;
			this.label23.Text = "Skjutlagstyp";
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(208, 136);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(280, 32);
			this.label24.TabIndex = 35;
			this.label24.Text = "Det här är max antalet skyttar i ett lag.";
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(208, 104);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(280, 32);
			this.label26.TabIndex = 34;
			this.label26.Text = "Det här är tiden i minuter mellan start för olika lag.";
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(40, 136);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(100, 23);
			this.label25.TabIndex = 31;
			this.label25.Text = "Lagstorlek";
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(40, 104);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(104, 23);
			this.label27.TabIndex = 30;
			this.label27.Text = "Tid mellan skjutlag";
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(8, 72);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(472, 23);
			this.label29.TabIndex = 28;
			this.label29.Text = "Här anger du information om skjutlag";
			// 
			// header5
			// 
			this.header5.BackColor = System.Drawing.SystemColors.Control;
			this.header5.CausesValidation = false;
			this.header5.Description = "";
			this.header5.Dock = System.Windows.Forms.DockStyle.Top;
			this.header5.Location = new System.Drawing.Point(0, 0);
			this.header5.Name = "header5";
			this.header5.Size = new System.Drawing.Size(496, 64);
			this.header5.TabIndex = 27;
			this.header5.Title = "Precisionsskytte - Information om lagen";
			// 
			// numPrecisionPatrolSize
			// 
			this.numPrecisionPatrolSize.Location = new System.Drawing.Point(152, 136);
			this.numPrecisionPatrolSize.Maximum = new decimal(new int[] {
            350,
            0,
            0,
            0});
			this.numPrecisionPatrolSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numPrecisionPatrolSize.Name = "numPrecisionPatrolSize";
			this.numPrecisionPatrolSize.Size = new System.Drawing.Size(48, 21);
			this.numPrecisionPatrolSize.TabIndex = 22;
			this.numPrecisionPatrolSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPrecisionPatrolSize.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
			// 
			// numPrecisionPatrolTimeBetween
			// 
			this.numPrecisionPatrolTimeBetween.Location = new System.Drawing.Point(152, 104);
			this.numPrecisionPatrolTimeBetween.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.numPrecisionPatrolTimeBetween.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numPrecisionPatrolTimeBetween.Name = "numPrecisionPatrolTimeBetween";
			this.numPrecisionPatrolTimeBetween.Size = new System.Drawing.Size(48, 21);
			this.numPrecisionPatrolTimeBetween.TabIndex = 19;
			this.numPrecisionPatrolTimeBetween.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPrecisionPatrolTimeBetween.Value = new decimal(new int[] {
            105,
            0,
            0,
            0});
			// 
			// ddCompetitionType
			// 
			this.ddCompetitionType.FormattingEnabled = true;
			this.ddCompetitionType.Items.AddRange(new object[] {
            "Lokal tävling",
            "Kretsmästerskap",
            "SM"});
			this.ddCompetitionType.Location = new System.Drawing.Point(56, 239);
			this.ddCompetitionType.Name = "ddCompetitionType";
			this.ddCompetitionType.Size = new System.Drawing.Size(160, 21);
			this.ddCompetitionType.TabIndex = 8;
			// 
			// lblPriceDeposit
			// 
			this.lblPriceDeposit.Location = new System.Drawing.Point(24, 206);
			this.lblPriceDeposit.Name = "lblPriceDeposit";
			this.lblPriceDeposit.Size = new System.Drawing.Size(131, 23);
			this.lblPriceDeposit.TabIndex = 7;
			this.lblPriceDeposit.Text = "Anmälningsavgift varv 1";
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(24, 229);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(131, 23);
			this.label28.TabIndex = 11;
			this.label28.Text = "Anmälningsavgift varv 2";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(24, 252);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(131, 23);
			this.label30.TabIndex = 13;
			this.label30.Text = "Anmälningsavgift varv 3";
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(24, 275);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(131, 23);
			this.label31.TabIndex = 15;
			this.label31.Text = "Anmälningsavgift varv 4";
			// 
			// numPriceDeposit
			// 
			this.numPriceDeposit.Location = new System.Drawing.Point(151, 206);
			this.numPriceDeposit.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numPriceDeposit.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numPriceDeposit.Name = "numPriceDeposit";
			this.numPriceDeposit.Size = new System.Drawing.Size(48, 20);
			this.numPriceDeposit.TabIndex = 8;
			this.numPriceDeposit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPriceDeposit.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(151, 229);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(48, 20);
			this.numericUpDown1.TabIndex = 12;
			this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDown1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.Location = new System.Drawing.Point(151, 252);
			this.numericUpDown2.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numericUpDown2.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(48, 20);
			this.numericUpDown2.TabIndex = 14;
			this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDown2.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// numericUpDown3
			// 
			this.numericUpDown3.Location = new System.Drawing.Point(151, 275);
			this.numericUpDown3.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numericUpDown3.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDown3.Name = "numericUpDown3";
			this.numericUpDown3.Size = new System.Drawing.Size(48, 20);
			this.numericUpDown3.TabIndex = 16;
			this.numericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDown3.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// FCompetitionWizard
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(496, 358);
			this.Controls.Add(this.wizardCtrl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FCompetitionWizard";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Guiden Skapa ny tävling";
			this.wizardCtrl.ResumeLayout(false);
			this.wizardPageFieldPatrol.ResumeLayout(false);
			this.wizardPageFieldPatrol.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numFieldPatrolRestTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFieldPatrolSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFieldPatrolTimeBetween)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFieldPatrolTime)).EndInit();
			this.wizardPageCompetitionType.ResumeLayout(false);
			this.wizardPageCompetitionType.PerformLayout();
			this.wizardPageTimeDate.ResumeLayout(false);
			this.wizardPageTimeDate.PerformLayout();
			this.wizardPageName.ResumeLayout(false);
			this.wizardPageName.PerformLayout();
			this.wizardPageWelcome.ResumeLayout(false);
			this.infoContainerBegin.ResumeLayout(false);
			this.wizardPageFinish.ResumeLayout(false);
			this.infoContainerFinish.ResumeLayout(false);
			this.wizardPagePriceMoney.ResumeLayout(false);
			this.wizardPagePriceMoney.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPriceDeposit4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceDeposit3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceDeposit2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceFirst)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceDeposit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceShooters)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceRepay)).EndInit();
			this.wizardPagePrecisionPatrol.ResumeLayout(false);
			this.wizardPagePrecisionPatrol.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPrecisionPatrolSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPrecisionPatrolTimeBetween)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPriceDeposit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Page Name
		private void wizardPageName_ShowFromNext(object sender, EventArgs e)
		{
			this.txtName.Focus();
		}

		#endregion

		#region Competition Start date and time
		private void wizardPageTimeDate_ShowFromNext(object sender, EventArgs e)
		{
			dateTimePickerStart.Focus();
		}

		private void wizardPageTimeDate_CloseFromNext(object sender, PageEventArgs e)
		{
			string timeString = txtStartTime.Text;
			string[] timeStrings = timeString.Split(':');
			if (timeStrings.Length == 1)
			{
				MessageBox.Show("Det finns inte något kolon i tiden. Ange tiden på formatet HH:MM.", 
					"Felinmatning", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
				e.Page = wizardPageTimeDate;
				return;
			}
			if (timeStrings.Length > 2)
			{
				MessageBox.Show("Det finns fler än ett kolon i tiden. Ange tiden på formatet HH:MM.", 
					"Felinmatning", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
				e.Page = this.wizardPageTimeDate;
				return;
			}
			if (timeStrings[0].Length > 2 |
				timeStrings[0].Length < 1)
			{
				MessageBox.Show("Ange timmar med en eller två siffror. Ange tiden på formatet HH:MM.", 
					"Felinmatning", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
				e.Page = this.wizardPageTimeDate;
				return;
			}
			if (timeStrings[1].Length > 2 |
				timeStrings[1].Length < 1)
			{
				MessageBox.Show("Ange minuter med en eller två siffror. Ange tiden på formatet HH:MM.", 
					"Felinmatning", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
				e.Page = this.wizardPageTimeDate;
				return;
			}
			try
			{
				int.Parse(timeStrings[0]);
			}
			catch(System.FormatException)
			{
				MessageBox.Show("Ange timmar med en eller två siffror. Ange tiden på formatet HH:MM.", 
					"Felinmatning", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
				e.Page = this.wizardPageTimeDate;
				return;
			}
			try
			{
				int.Parse(timeStrings[1]);
			}
			catch(System.FormatException)
			{
				MessageBox.Show("Ange minuter med en eller två siffror. Ange tiden på formatet HH:MM.", 
					"Felinmatning", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);
				e.Page = this.wizardPageTimeDate;
				return;
			}
		}
		#endregion

		#region Competition Type
		private void wizardPageCompetitionType_ShowFromNext(object sender, EventArgs e)
		{
			this.radioFieldStandard.Focus();
		}

		private void wizardPageCompetitionType_CloseFromNext(object sender, 
			PageEventArgs e)
		{
			if (!this.radioBanskytte.Checked &
				!this.radioFieldStandard.Checked &
				!this.radioFieldNorwegian.Checked &
				!this.radioFieldMagnum.Checked)
			{
				MessageBox.Show("Du måste välja en typ av tävling", 
					"Val måste göras", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Exclamation);

				e.Page = this.wizardPageCompetitionType;
			}
			if (this.radioFieldStandard.Checked | 
				this.radioFieldNorwegian.Checked |
				this.radioFieldMagnum.Checked)
			{
				// Send user to field pages
				e.Page = this.wizardPageFieldPatrol;
			}
			else
			{
				// Send user to banskytte pages
				e.Page = this.wizardPagePrecisionPatrol;
			}
		}
		#endregion

		#region Field specific - Patrols
		private void wizardPageFieldPatrol_ShowFromNext(object sender, EventArgs e)
		{
			numFieldPatrolTime.Focus();
		}
		private void wizardPageFieldPatrol_CloseFromNext(object sender, PageEventArgs e)
		{
			e.Page = this.wizardPagePriceMoney;
		}
		#endregion

		#region Precision specifik - Patrols
		private void wizardPagePrecisionPatrol_ShowFromNext(object sender, EventArgs e)
		{
			// Set focus for usability
			this.numPrecisionPatrolTimeBetween.Focus();
		}
		private void wizardPagePrecisionPatrol_CloseFromBack(object sender, PageEventArgs e)
		{
			e.Page = wizardPageCompetitionType;
		}
		#endregion

		#region Pricemoney
		void chkPriceUseSameDeposit_CheckedChanged(object sender, EventArgs e)
		{
			if (chkPriceUseSameDeposit.Checked)
			{
				numPriceDeposit2.Enabled = false;
				numPriceDeposit3.Enabled = false;
				numPriceDeposit4.Enabled = false;

				numPriceDeposit2.Value = numPriceDeposit1.Value;
				numPriceDeposit3.Value = numPriceDeposit1.Value;
				numPriceDeposit4.Value = numPriceDeposit1.Value;
			}
			else
			{
				numPriceDeposit2.Enabled = true;
				numPriceDeposit3.Enabled = true;
				numPriceDeposit4.Enabled = true;
			}
		}

		void numPriceDeposit1_ValueChanged(object sender, EventArgs e)
		{
			if (chkPriceUseSameDeposit.Checked)
			{
				numPriceDeposit2.Value = numPriceDeposit1.Value;
				numPriceDeposit3.Value = numPriceDeposit1.Value;
				numPriceDeposit4.Value = numPriceDeposit1.Value;
			}
		}

		private void wizardPagePriceMoney_ShowFromNext(object sender, EventArgs e)
		{
			chkPriceMoney.Focus();
			checkPriceMoney();
		}

		private void chkPriceMoney_CheckedChanged(object sender, System.EventArgs e)
		{
			checkPriceMoney();
		}
		private void checkPriceMoney()
		{
			this.lblPriceFirst.Visible = this.chkPriceMoney.Checked;
			this.lblPriceRepay.Visible = this.chkPriceMoney.Checked;
			this.lblPriceShooters.Visible = this.chkPriceMoney.Checked;
 
			this.lblPriceDeposit1.Visible = this.chkPriceMoney.Checked;
			this.lblPriceDeposit2.Visible = this.chkPriceMoney.Checked;
			this.lblPriceDeposit3.Visible = this.chkPriceMoney.Checked;
			this.lblPriceDeposit4.Visible = this.chkPriceMoney.Checked;

			this.numPriceDeposit1.Visible = this.chkPriceMoney.Checked;
			this.numPriceDeposit2.Visible = this.chkPriceMoney.Checked;
			this.numPriceDeposit3.Visible = this.chkPriceMoney.Checked;
			this.numPriceDeposit4.Visible = this.chkPriceMoney.Checked;

			this.chkPriceUseSameDeposit.Visible = this.chkPriceMoney.Checked;
			
			this.numPriceFirst.Visible = this.chkPriceMoney.Checked;
			this.numPriceRepay.Visible = this.chkPriceMoney.Checked;
			this.numPriceShooters.Visible = this.chkPriceMoney.Checked;
		}
		#endregion
		
		#region Finished
		private void wizardPageFinish_CloseFromNext(object sender, PageEventArgs e)
		{
			Trace.WriteLine(e.ToString());
			Structs.Competition comp = new Structs.Competition();

			comp.DoFinalShooting = this.chkDoFinalShooting.Checked;
			comp.FirstPrice = (int)this.numPriceFirst.Value;
			comp.Name = this.txtName.Text;
			if (this.radioFieldStandard.Checked)
			{
				comp.Type = Structs.CompetitionTypeEnum.Field;
				comp.NorwegianCount = false;
			}
			else if (this.radioFieldNorwegian.Checked)
			{
				comp.Type = Structs.CompetitionTypeEnum.Field;
				comp.NorwegianCount = true;
			}
			else if (this.radioFieldMagnum.Checked)
			{
				comp.Type = Structs.CompetitionTypeEnum.MagnumField;
				comp.NorwegianCount = false;
			}
			else if (this.radioBanskytte.Checked)
			{
				comp.Type = Structs.CompetitionTypeEnum.Precision;
				comp.NorwegianCount = false;
			}
			switch(comp.Type)
			{
				case Structs.CompetitionTypeEnum.Field:
				{
					comp.PatrolSize = (int)this.numFieldPatrolSize.Value;
					comp.PatrolTime = (int)this.numFieldPatrolTime.Value;
					comp.PatrolTimeBetween = (int)this.numFieldPatrolTimeBetween.Value;
					comp.PatrolTimeRest = (int)this.numFieldPatrolRestTime.Value;
					comp.PatrolConnectionType = (Structs.PatrolConnectionTypeEnum)
						DDFieldPatrolConnectionType.SelectedIndex;
					break;
				}
				case Structs.CompetitionTypeEnum.MagnumField:
				{
					comp.PatrolSize = (int)this.numFieldPatrolSize.Value;
					comp.PatrolTime = (int)this.numFieldPatrolTime.Value;
					comp.PatrolTimeBetween = (int)this.numFieldPatrolTimeBetween.Value;
					comp.PatrolTimeRest = (int)this.numFieldPatrolRestTime.Value;
					comp.PatrolConnectionType = (Structs.PatrolConnectionTypeEnum)
						DDFieldPatrolConnectionType.SelectedIndex;
					break;
				}
				case Structs.CompetitionTypeEnum.Precision:
				{
					comp.PatrolSize = (int)this.numPrecisionPatrolSize.Value;
					//comp.PatrolTime = (int)this.numPrecisionPatrolTime.Value;
					comp.PatrolTimeBetween = (int)this.numPrecisionPatrolTimeBetween.Value;
					comp.PatrolTimeRest = 0;
					comp.PatrolConnectionType = (Structs.PatrolConnectionTypeEnum)
						DdPrecisionPatrolConnectionType.SelectedIndex;
					break;
				}
				default:
				{
					throw new ApplicationException("Not implemented yet");
				}
			}
			comp.PriceMoneyPercentToReturn = (int)this.numPriceRepay.Value;
			comp.PriceMoneyShooterPercent = (int)this.numPriceShooters.Value;
			comp.ShooterFee1 = (int)this.numPriceDeposit1.Value;
			comp.ShooterFee2 = (int)this.numPriceDeposit2.Value;
			comp.ShooterFee3 = (int)this.numPriceDeposit3.Value;
			comp.ShooterFee4 = (int)this.numPriceDeposit4.Value;

			DateTime starttime = this.dateTimePickerStart.Value;
			int hour = int.Parse(this.txtStartTime.Text.Split(':')[0]);
			int minute = int.Parse(this.txtStartTime.Text.Split(':')[1]);
			starttime = starttime.AddHours(hour);
			starttime = starttime.AddMinutes(minute);
			comp.StartTime = starttime;
			comp.UsePriceMoney = this.chkPriceMoney.Checked;

			comp.Championship = (Structs.CompetitionChampionshipEnum)
				ddChampionshipType.SelectedIndex;
			if (comp.Championship == Structs.CompetitionChampionshipEnum.Nationell ||
				comp.Championship == Structs.CompetitionChampionshipEnum.SM)
				comp.OneClass = true;
			else
				comp.OneClass = false;

			CommonCode.NewCompetition(comp);
			this.EnableMain();
		}
		#endregion


		private void wizardCtrl_CloseFromCancel(object sender, CancelEventArgs e)
		{	
			if (MessageBox.Show(this, 
				"Är du säker?",
				"Wizard Avbruts", MessageBoxButtons.YesNo
				) == DialogResult.Yes)
			{
				e.Cancel = true;
				try
				{
					CancelMain();
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.ToString());
				}
			}
			else
			{
				e.Cancel = false;
				Trace.WriteLine(sender.GetType().ToString());
			}
		}
	}
}
