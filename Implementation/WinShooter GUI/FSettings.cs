// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FSettings.cs" company="John Allberg">
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
//   Summary description for FSettings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using Allberg.Shooter.Windows.Forms;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for FSettings.
    /// </summary>
    public class FSettings : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageLogo;
        private SafeButton btnCancel;
        private SafeButton btnApply;
        private SafeButton btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private SafeButton btnLogoChooseFile;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.TabPage tabPageLabels;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddLabelUsage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddLabelType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numLabelsLeftMargin;
        private System.Windows.Forms.NumericUpDown numLabelsTopMargin;
        private System.Windows.Forms.NumericUpDown numLabelsLabelSizeX;
        private System.Windows.Forms.NumericUpDown numLabelsLabelSizeY;
        private System.Windows.Forms.NumericUpDown numLabelsNrOfLabelsX;
        private System.Windows.Forms.NumericUpDown numLabelsNrOfLabelsY;
        private System.Windows.Forms.ComboBox ddLabelsFont;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numLabelsFontSize;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numLabelsInnerMarginHorisontal;
        private System.Windows.Forms.NumericUpDown numLabelsInnerMarginVertical;
        private System.Windows.Forms.Label label5;
        private SafeButton btnLogoRemoveFile;
        private TabPage tabPagePrints;
        private Label label15;
        private ComboBox DDPrintoutType;
        private Panel panelPrintResults;
        private Label lblFontSample;
        private Label label16;
        private FontDialog fontDialog1;
        private Button btnPrintsFontChange;
        private Allberg.Shooter.Windows.Forms.SafeComboBox DDPrintoutFont;
        private Label label17;
        private Allberg.Shooter.Windows.Forms.SafeComboBox DDPrintoutColumn;
        private NumericUpDown numericPrintoutColumnWidth;
        private Label label18;
        private Button btnPrintsColsChange;
        private Button btnPrintsOrientationChange;
        private Allberg.Shooter.Windows.Forms.SafeComboBox DDPrintoutOrientation;
        private Label label19;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public FSettings(ref Common.Interface newCommon)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            height = Height;
            width = Width;
            CommonCode = newCommon;
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "tabPagePrints":
                    DDPrintoutType.SelectedIndex = 0;
                    break;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FSettings: Dispose(" + disposing.ToString() + ")" +
                "from thread \"" + Thread.CurrentThread.Name + "\" " +
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

            Trace.WriteLine("FSettings: Dispose(" + disposing.ToString() + ")" +
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSettings));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageLabels = new System.Windows.Forms.TabPage();
            this.numLabelsInnerMarginVertical = new System.Windows.Forms.NumericUpDown();
            this.numLabelsInnerMarginHorisontal = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.numLabelsFontSize = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ddLabelsFont = new System.Windows.Forms.ComboBox();
            this.numLabelsNrOfLabelsY = new System.Windows.Forms.NumericUpDown();
            this.numLabelsNrOfLabelsX = new System.Windows.Forms.NumericUpDown();
            this.numLabelsLabelSizeY = new System.Windows.Forms.NumericUpDown();
            this.numLabelsLabelSizeX = new System.Windows.Forms.NumericUpDown();
            this.numLabelsLeftMargin = new System.Windows.Forms.NumericUpDown();
            this.numLabelsTopMargin = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ddLabelType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ddLabelUsage = new System.Windows.Forms.ComboBox();
            this.tabPagePrints = new System.Windows.Forms.TabPage();
            this.panelPrintResults = new System.Windows.Forms.Panel();
            this.btnPrintsOrientationChange = new System.Windows.Forms.Button();
            this.DDPrintoutOrientation = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.btnPrintsColsChange = new System.Windows.Forms.Button();
            this.numericPrintoutColumnWidth = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.DDPrintoutColumn = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.DDPrintoutFont = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.btnPrintsFontChange = new System.Windows.Forms.Button();
            this.lblFontSample = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.DDPrintoutType = new System.Windows.Forms.ComboBox();
            this.tabPageLogo = new System.Windows.Forms.TabPage();
            this.btnLogoRemoveFile = new SafeButton();
            this.btnLogoChooseFile = new SafeButton();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.btnCancel = new SafeButton();
            this.btnApply = new SafeButton();
            this.btnOK = new SafeButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.tabControl1.SuspendLayout();
            this.tabPageLabels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsInnerMarginVertical)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsInnerMarginHorisontal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsNrOfLabelsY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsNrOfLabelsX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsLabelSizeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsLabelSizeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsLeftMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsTopMargin)).BeginInit();
            this.tabPagePrints.SuspendLayout();
            this.panelPrintResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPrintoutColumnWidth)).BeginInit();
            this.tabPageLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageLabels);
            this.tabControl1.Controls.Add(this.tabPagePrints);
            this.tabControl1.Controls.Add(this.tabPageLogo);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(512, 264);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageLabels
            // 
            this.tabPageLabels.Controls.Add(this.numLabelsInnerMarginVertical);
            this.tabPageLabels.Controls.Add(this.numLabelsInnerMarginHorisontal);
            this.tabPageLabels.Controls.Add(this.label14);
            this.tabPageLabels.Controls.Add(this.label13);
            this.tabPageLabels.Controls.Add(this.numLabelsFontSize);
            this.tabPageLabels.Controls.Add(this.label12);
            this.tabPageLabels.Controls.Add(this.label11);
            this.tabPageLabels.Controls.Add(this.ddLabelsFont);
            this.tabPageLabels.Controls.Add(this.numLabelsNrOfLabelsY);
            this.tabPageLabels.Controls.Add(this.numLabelsNrOfLabelsX);
            this.tabPageLabels.Controls.Add(this.numLabelsLabelSizeY);
            this.tabPageLabels.Controls.Add(this.numLabelsLabelSizeX);
            this.tabPageLabels.Controls.Add(this.numLabelsLeftMargin);
            this.tabPageLabels.Controls.Add(this.numLabelsTopMargin);
            this.tabPageLabels.Controls.Add(this.label10);
            this.tabPageLabels.Controls.Add(this.label9);
            this.tabPageLabels.Controls.Add(this.label8);
            this.tabPageLabels.Controls.Add(this.label7);
            this.tabPageLabels.Controls.Add(this.label6);
            this.tabPageLabels.Controls.Add(this.label5);
            this.tabPageLabels.Controls.Add(this.label4);
            this.tabPageLabels.Controls.Add(this.ddLabelType);
            this.tabPageLabels.Controls.Add(this.label3);
            this.tabPageLabels.Controls.Add(this.label2);
            this.tabPageLabels.Controls.Add(this.ddLabelUsage);
            this.tabPageLabels.Location = new System.Drawing.Point(4, 22);
            this.tabPageLabels.Name = "tabPageLabels";
            this.tabPageLabels.Size = new System.Drawing.Size(504, 238);
            this.tabPageLabels.TabIndex = 1;
            this.tabPageLabels.Text = "Etiketter";
            this.tabPageLabels.UseVisualStyleBackColor = true;
            // 
            // numLabelsInnerMarginVertical
            // 
            this.numLabelsInnerMarginVertical.Location = new System.Drawing.Point(200, 208);
            this.numLabelsInnerMarginVertical.Name = "numLabelsInnerMarginVertical";
            this.numLabelsInnerMarginVertical.Size = new System.Drawing.Size(40, 20);
            this.numLabelsInnerMarginVertical.TabIndex = 24;
            this.numLabelsInnerMarginVertical.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLabelsInnerMarginVertical.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numLabelsInnerMarginHorisontal
            // 
            this.numLabelsInnerMarginHorisontal.Location = new System.Drawing.Point(200, 184);
            this.numLabelsInnerMarginHorisontal.Name = "numLabelsInnerMarginHorisontal";
            this.numLabelsInnerMarginHorisontal.Size = new System.Drawing.Size(40, 20);
            this.numLabelsInnerMarginHorisontal.TabIndex = 23;
            this.numLabelsInnerMarginHorisontal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLabelsInnerMarginHorisontal.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 208);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(192, 23);
            this.label14.TabIndex = 22;
            this.label14.Text = "Avstånd mellan etiketter (vertikalt)";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(8, 184);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(192, 23);
            this.label13.TabIndex = 21;
            this.label13.Text = "Avstånd mellan etiketter (horisontellt)";
            // 
            // numLabelsFontSize
            // 
            this.numLabelsFontSize.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numLabelsFontSize.Location = new System.Drawing.Point(456, 136);
            this.numLabelsFontSize.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numLabelsFontSize.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numLabelsFontSize.Name = "numLabelsFontSize";
            this.numLabelsFontSize.Size = new System.Drawing.Size(40, 20);
            this.numLabelsFontSize.TabIndex = 20;
            this.numLabelsFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLabelsFontSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(264, 136);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 23);
            this.label12.TabIndex = 19;
            this.label12.Text = "Fontstorlek";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(264, 160);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 23);
            this.label11.TabIndex = 18;
            this.label11.Text = "Font";
            // 
            // ddLabelsFont
            // 
            this.ddLabelsFont.Items.AddRange(new object[] {
            "Arial"});
            this.ddLabelsFont.Location = new System.Drawing.Point(376, 160);
            this.ddLabelsFont.Name = "ddLabelsFont";
            this.ddLabelsFont.Size = new System.Drawing.Size(121, 21);
            this.ddLabelsFont.TabIndex = 17;
            // 
            // numLabelsNrOfLabelsY
            // 
            this.numLabelsNrOfLabelsY.Location = new System.Drawing.Point(200, 160);
            this.numLabelsNrOfLabelsY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLabelsNrOfLabelsY.Name = "numLabelsNrOfLabelsY";
            this.numLabelsNrOfLabelsY.Size = new System.Drawing.Size(40, 20);
            this.numLabelsNrOfLabelsY.TabIndex = 16;
            this.numLabelsNrOfLabelsY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLabelsNrOfLabelsY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numLabelsNrOfLabelsX
            // 
            this.numLabelsNrOfLabelsX.Location = new System.Drawing.Point(200, 136);
            this.numLabelsNrOfLabelsX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLabelsNrOfLabelsX.Name = "numLabelsNrOfLabelsX";
            this.numLabelsNrOfLabelsX.Size = new System.Drawing.Size(40, 20);
            this.numLabelsNrOfLabelsX.TabIndex = 15;
            this.numLabelsNrOfLabelsX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLabelsNrOfLabelsX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numLabelsLabelSizeY
            // 
            this.numLabelsLabelSizeY.Location = new System.Drawing.Point(456, 112);
            this.numLabelsLabelSizeY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLabelsLabelSizeY.Name = "numLabelsLabelSizeY";
            this.numLabelsLabelSizeY.Size = new System.Drawing.Size(40, 20);
            this.numLabelsLabelSizeY.TabIndex = 14;
            this.numLabelsLabelSizeY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLabelsLabelSizeY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numLabelsLabelSizeX
            // 
            this.numLabelsLabelSizeX.Location = new System.Drawing.Point(456, 88);
            this.numLabelsLabelSizeX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLabelsLabelSizeX.Name = "numLabelsLabelSizeX";
            this.numLabelsLabelSizeX.Size = new System.Drawing.Size(40, 20);
            this.numLabelsLabelSizeX.TabIndex = 13;
            this.numLabelsLabelSizeX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLabelsLabelSizeX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numLabelsLeftMargin
            // 
            this.numLabelsLeftMargin.Location = new System.Drawing.Point(200, 112);
            this.numLabelsLeftMargin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLabelsLeftMargin.Name = "numLabelsLeftMargin";
            this.numLabelsLeftMargin.Size = new System.Drawing.Size(40, 20);
            this.numLabelsLeftMargin.TabIndex = 12;
            this.numLabelsLeftMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLabelsLeftMargin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numLabelsTopMargin
            // 
            this.numLabelsTopMargin.Location = new System.Drawing.Point(200, 88);
            this.numLabelsTopMargin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLabelsTopMargin.Name = "numLabelsTopMargin";
            this.numLabelsTopMargin.Size = new System.Drawing.Size(40, 20);
            this.numLabelsTopMargin.TabIndex = 11;
            this.numLabelsTopMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLabelsTopMargin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(8, 160);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(184, 23);
            this.label10.TabIndex = 10;
            this.label10.Text = "Antal etiketter per sida (vertikalt)";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(184, 23);
            this.label9.TabIndex = 9;
            this.label9.Text = "Antal etiketter per sida (horisontellt)";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(264, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(184, 23);
            this.label8.TabIndex = 8;
            this.label8.Text = "Etikettstorlek (vertikalt)";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(264, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(136, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "Etikettstorlek (horisontellt)";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 23);
            this.label6.TabIndex = 6;
            this.label6.Text = "Marginal vänsterkant";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(160, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(176, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "Samtliga mått är i millimeter";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Marginal överkant";
            // 
            // ddLabelType
            // 
            this.ddLabelType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddLabelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddLabelType.Items.AddRange(new object[] {
            "Avery5160",
            "Anpassat"});
            this.ddLabelType.Location = new System.Drawing.Point(112, 32);
            this.ddLabelType.Name = "ddLabelType";
            this.ddLabelType.Size = new System.Drawing.Size(384, 21);
            this.ddLabelType.TabIndex = 3;
            this.ddLabelType.SelectedIndexChanged += new System.EventHandler(this.ddLabelType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Etiketttyp";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Etiketter till";
            // 
            // ddLabelUsage
            // 
            this.ddLabelUsage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddLabelUsage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddLabelUsage.Items.AddRange(new object[] {
            "Speglar",
            "Stickor",
            "Resultat"});
            this.ddLabelUsage.Location = new System.Drawing.Point(112, 8);
            this.ddLabelUsage.Name = "ddLabelUsage";
            this.ddLabelUsage.Size = new System.Drawing.Size(384, 21);
            this.ddLabelUsage.TabIndex = 0;
            this.ddLabelUsage.SelectedIndexChanged += new System.EventHandler(this.ddLabelUsage_SelectedIndexChanged);
            // 
            // tabPagePrints
            // 
            this.tabPagePrints.Controls.Add(this.panelPrintResults);
            this.tabPagePrints.Controls.Add(this.label15);
            this.tabPagePrints.Controls.Add(this.DDPrintoutType);
            this.tabPagePrints.Location = new System.Drawing.Point(4, 22);
            this.tabPagePrints.Name = "tabPagePrints";
            this.tabPagePrints.Size = new System.Drawing.Size(504, 238);
            this.tabPagePrints.TabIndex = 2;
            this.tabPagePrints.Text = "Utskrifter";
            this.tabPagePrints.UseVisualStyleBackColor = true;
            // 
            // panelPrintResults
            // 
            this.panelPrintResults.Controls.Add(this.btnPrintsOrientationChange);
            this.panelPrintResults.Controls.Add(this.DDPrintoutOrientation);
            this.panelPrintResults.Controls.Add(this.label19);
            this.panelPrintResults.Controls.Add(this.btnPrintsColsChange);
            this.panelPrintResults.Controls.Add(this.numericPrintoutColumnWidth);
            this.panelPrintResults.Controls.Add(this.label18);
            this.panelPrintResults.Controls.Add(this.DDPrintoutColumn);
            this.panelPrintResults.Controls.Add(this.DDPrintoutFont);
            this.panelPrintResults.Controls.Add(this.label17);
            this.panelPrintResults.Controls.Add(this.btnPrintsFontChange);
            this.panelPrintResults.Controls.Add(this.lblFontSample);
            this.panelPrintResults.Controls.Add(this.label16);
            this.panelPrintResults.Location = new System.Drawing.Point(11, 40);
            this.panelPrintResults.Name = "panelPrintResults";
            this.panelPrintResults.Size = new System.Drawing.Size(490, 195);
            this.panelPrintResults.TabIndex = 2;
            // 
            // btnPrintsOrientationChange
            // 
            this.btnPrintsOrientationChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintsOrientationChange.Location = new System.Drawing.Point(410, 147);
            this.btnPrintsOrientationChange.Name = "btnPrintsOrientationChange";
            this.btnPrintsOrientationChange.Size = new System.Drawing.Size(75, 23);
            this.btnPrintsOrientationChange.TabIndex = 12;
            this.btnPrintsOrientationChange.Text = "Ändra";
            this.btnPrintsOrientationChange.UseVisualStyleBackColor = true;
            this.btnPrintsOrientationChange.Click += new System.EventHandler(this.btnPrintsOrientationChange_Click);
            // 
            // DDPrintoutOrientation
            // 
            this.DDPrintoutOrientation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DDPrintoutOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DDPrintoutOrientation.FormattingEnabled = true;
            this.DDPrintoutOrientation.Items.AddRange(new object[] {
            "Stående",
            "Liggande"});
            this.DDPrintoutOrientation.Location = new System.Drawing.Point(67, 120);
            this.DDPrintoutOrientation.Name = "DDPrintoutOrientation";
            this.DDPrintoutOrientation.Size = new System.Drawing.Size(418, 21);
            this.DDPrintoutOrientation.TabIndex = 11;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 120);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(58, 13);
            this.label19.TabIndex = 10;
            this.label19.Text = "Orientering";
            // 
            // btnPrintsColsChange
            // 
            this.btnPrintsColsChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintsColsChange.Location = new System.Drawing.Point(410, 92);
            this.btnPrintsColsChange.Name = "btnPrintsColsChange";
            this.btnPrintsColsChange.Size = new System.Drawing.Size(75, 23);
            this.btnPrintsColsChange.TabIndex = 9;
            this.btnPrintsColsChange.Text = "Ändra";
            this.btnPrintsColsChange.UseVisualStyleBackColor = true;
            this.btnPrintsColsChange.Click += new System.EventHandler(this.btnPrintsColsChange_Click);
            // 
            // numericPrintoutColumnWidth
            // 
            this.numericPrintoutColumnWidth.Location = new System.Drawing.Point(132, 90);
            this.numericPrintoutColumnWidth.Name = "numericPrintoutColumnWidth";
            this.numericPrintoutColumnWidth.Size = new System.Drawing.Size(70, 20);
            this.numericPrintoutColumnWidth.TabIndex = 8;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(67, 92);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 13);
            this.label18.TabIndex = 7;
            this.label18.Text = "Bredd i mm";
            // 
            // DDPrintoutColumn
            // 
            this.DDPrintoutColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DDPrintoutColumn.FormattingEnabled = true;
            this.DDPrintoutColumn.Location = new System.Drawing.Point(67, 64);
            this.DDPrintoutColumn.Name = "DDPrintoutColumn";
            this.DDPrintoutColumn.Size = new System.Drawing.Size(418, 21);
            this.DDPrintoutColumn.TabIndex = 6;
            this.DDPrintoutColumn.SelectedIndexChanged += new System.EventHandler(this.DDPrintoutColumn_SelectedIndexChanged);
            // 
            // DDPrintoutFont
            // 
            this.DDPrintoutFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DDPrintoutFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DDPrintoutFont.FormattingEnabled = true;
            this.DDPrintoutFont.Location = new System.Drawing.Point(67, 6);
            this.DDPrintoutFont.Name = "DDPrintoutFont";
            this.DDPrintoutFont.Size = new System.Drawing.Size(418, 21);
            this.DDPrintoutFont.TabIndex = 5;
            this.DDPrintoutFont.SelectedIndexChanged += new System.EventHandler(this.DDPrintoutFont_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 72);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(51, 13);
            this.label17.TabIndex = 4;
            this.label17.Text = "Kolumner";
            // 
            // btnPrintsFontChange
            // 
            this.btnPrintsFontChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintsFontChange.Location = new System.Drawing.Point(410, 33);
            this.btnPrintsFontChange.Name = "btnPrintsFontChange";
            this.btnPrintsFontChange.Size = new System.Drawing.Size(75, 23);
            this.btnPrintsFontChange.TabIndex = 3;
            this.btnPrintsFontChange.Text = "Ändra";
            this.btnPrintsFontChange.UseVisualStyleBackColor = true;
            this.btnPrintsFontChange.Click += new System.EventHandler(this.btnPrintsFontChange_Click);
            // 
            // lblFontSample
            // 
            this.lblFontSample.AutoSize = true;
            this.lblFontSample.Location = new System.Drawing.Point(64, 30);
            this.lblFontSample.Name = "lblFontSample";
            this.lblFontSample.Size = new System.Drawing.Size(102, 13);
            this.lblFontSample.TabIndex = 2;
            this.lblFontSample.Text = "Detta är ett exempel";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 11);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(37, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "Fonter";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 13);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(59, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Utskriftstyp";
            // 
            // DDPrintoutType
            // 
            this.DDPrintoutType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DDPrintoutType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DDPrintoutType.FormattingEnabled = true;
            this.DDPrintoutType.Items.AddRange(new object[] {
            "Resultatutskrift"});
            this.DDPrintoutType.Location = new System.Drawing.Point(114, 13);
            this.DDPrintoutType.Name = "DDPrintoutType";
            this.DDPrintoutType.Size = new System.Drawing.Size(382, 21);
            this.DDPrintoutType.TabIndex = 0;
            this.DDPrintoutType.SelectedIndexChanged += new System.EventHandler(this.DDPrintoutType_SelectedIndexChanged);
            // 
            // tabPageLogo
            // 
            this.tabPageLogo.Controls.Add(this.btnLogoRemoveFile);
            this.tabPageLogo.Controls.Add(this.btnLogoChooseFile);
            this.tabPageLogo.Controls.Add(this.label1);
            this.tabPageLogo.Controls.Add(this.pictureBoxLogo);
            this.tabPageLogo.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogo.Name = "tabPageLogo";
            this.tabPageLogo.Size = new System.Drawing.Size(504, 238);
            this.tabPageLogo.TabIndex = 0;
            this.tabPageLogo.Text = "Logga";
            this.tabPageLogo.UseVisualStyleBackColor = true;
            // 
            // btnLogoRemoveFile
            // 
            this.btnLogoRemoveFile.Location = new System.Drawing.Point(88, 48);
            this.btnLogoRemoveFile.Name = "btnLogoRemoveFile";
            this.btnLogoRemoveFile.Size = new System.Drawing.Size(75, 23);
            this.btnLogoRemoveFile.TabIndex = 3;
            this.btnLogoRemoveFile.Text = "Ta bort";
            this.btnLogoRemoveFile.Click += new System.EventHandler(this.btnLogoRemoveFile_Click);
            // 
            // btnLogoChooseFile
            // 
            this.btnLogoChooseFile.Location = new System.Drawing.Point(8, 48);
            this.btnLogoChooseFile.Name = "btnLogoChooseFile";
            this.btnLogoChooseFile.Size = new System.Drawing.Size(75, 23);
            this.btnLogoChooseFile.TabIndex = 2;
            this.btnLogoChooseFile.Text = "Välj fil";
            this.btnLogoChooseFile.Click += new System.EventHandler(this.btnLogoChooseFile_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(488, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "På utskrifter och exporter finns det en logga med. Här kan du välja att använda d" +
                "in egen logga istället för Winshooters.";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxLogo.Location = new System.Drawing.Point(8, 80);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(488, 152);
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(432, 272);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Avbryt";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(352, 272);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Verkställ";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(272, 272);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(512, 302);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FSettings";
            this.Text = "Inställningar";
            this.tabControl1.ResumeLayout(false);
            this.tabPageLabels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsInnerMarginVertical)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsInnerMarginHorisontal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsNrOfLabelsY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsNrOfLabelsX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsLabelSizeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsLabelSizeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsLeftMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLabelsTopMargin)).EndInit();
            this.tabPagePrints.ResumeLayout(false);
            this.tabPagePrints.PerformLayout();
            this.panelPrintResults.ResumeLayout(false);
            this.panelPrintResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPrintoutColumnWidth)).EndInit();
            this.tabPageLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        public delegate void EnableMainHandler();
        public event EnableMainHandler EnableMain;
        public bool DisposeNow = false;
        Common.Interface CommonCode;
        int height;
        int width;

        public void EnableMe()
        {
            Visible = true;
            Focus();
            displaySettings();
            btnApply.Enabled = false;
            tabControl1.TabIndex = 0;
        }

        private void displaySettings()
        {
            ISettings settings = CommonCode.Settings;
            displayLogoSettings(settings);
            displayLabelsSettings(settings);
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            saveSettings();
            Visible = false;
            EnableMain();
        }
        private void btnApply_Click(object sender, System.EventArgs e)
        {
            saveSettings();
            btnApply.Enabled = false;
        }
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Visible = false;
            EnableMain();
        }
        private void saveSettings()
        {
            ISettings settings = CommonCode.Settings;
            try
            {
                if (newLogo != null)
                    settings.Logo = newLogo;
            }
            catch(System.IO.IOException exc)
            {
                Trace.WriteLine("FSettings: Exception while saving picture: " + exc.ToString());
                MessageBox.Show("Misslyckades med att spara ny logo. " +
                    "Troligen är bilden du försöker välja för stor.", 
                    "Fel", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FSettings: Exception while saving picture: " + exc.ToString());
                MessageBox.Show("Misslyckades med att spara ny logo: " + exc.ToString(),
                    "Fel", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

            }

            if (labelsChanged)
                saveSettingsLabels(settings);

            if (printoutsSettingsChanged)
                saveSettingsPrintouts();
        }


        #region Logo
        Image newLogo = null;
        private void displayLogoSettings(ISettings settings)
        {
            newLogo = null;
            Image image = settings.Logo;
            displayLogo(image);
        }
        private void displayLogo(Image image)
        {
            int logoHeight = image.Height;
            int logoWidth = image.Width;
            
            calculateLogoSize(image, pictureBoxLogo.Height, pictureBoxLogo.Width, 
                out logoHeight, out logoWidth);

            System.Drawing.Image logo = new Bitmap(image, new Size(logoWidth, logoHeight));
            pictureBoxLogo.Image = logo;
        }
        private void calculateLogoSize(Image image, int maxImageHeigh, int maxImageWidth,
            out int height, out int width)
        {
            double xfactor = (double)maxImageWidth / (double)image.Width;
            double yfactor = (double)maxImageHeigh / (double)image.Height;
            if (xfactor > yfactor)
                xfactor = yfactor;
            else
                yfactor = xfactor;

            height = (int)(yfactor * (double)image.Height);
            width = (int)(xfactor * (double)image.Width);
        }
        private void btnLogoChooseFile_Click(object sender, System.EventArgs e)
        {
            openFileDialog1.Filter = "Bildfiler (*.gif,*.bmp,*.jpg)|*.gif;*.bmp;*.jpg";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                try
                {
                    Image image = new Bitmap(this.openFileDialog1.FileName);
                    displayLogo(image);
                    newLogo = image;
                    this.btnApply.Enabled = true;
                }
                catch(Exception exc)
                {
                    Trace.WriteLine("FSettings: Exception occured while reading new logo: " + 
                        exc.ToString());
                    MessageBox.Show("Kunde inte skapa en bild utifrån den fil som du angivit");
                }
            }
        }
        private void btnLogoRemoveFile_Click(object sender, System.EventArgs e)
        {
            CommonCode.Settings.Logo = null;
            //settings.Logo = null;
            displayLogo(CommonCode.Settings.Logo);
        }
        #endregion

        #region Labels
        private void displayLabelsSettings(ISettings settings)
        {
            var labelUsage = new List<string>();
            labelUsage.Add("Speglar");
            labelUsage.Add("Resultat");

            ddLabelUsage.DataSource = labelUsage;
            ddLabelUsage.SelectedIndex = 0;

            List<PrintLabelDocument.PrintLabelDocumentTypeEnum> labelTypes = new List<PrintLabelDocument.PrintLabelDocumentTypeEnum>();
            labelTypes.Add(PrintLabelDocument.PrintLabelDocumentTypeEnum.Avery6150);
            labelTypes.Add(PrintLabelDocument.PrintLabelDocumentTypeEnum.Anpassad);
            ddLabelType.DataSource = labelTypes;
            ddLabelType.SelectedIndex = 0;

            PrintLabelDocument doc =  settings.PrinterSettings.LabelMirrorPrintDocument;

            ddLabelType.SelectedItem = doc.PrintLabelDocumentType;
            if (doc.PrintLabelDocumentType == PrintLabelDocument.PrintLabelDocumentTypeEnum.Anpassad)
            {
                numLabelsTopMargin.Value = doc.TopMarginMm;
                numLabelsLeftMargin.Value = doc.LeftMarginMm;
                numLabelsNrOfLabelsX.Value = doc.NrOfLabelsX;
                numLabelsNrOfLabelsY.Value = doc.NrOfLabelsY;
                numLabelsInnerMarginHorisontal.Value = doc.HorizontalInnerMarginMm;
                numLabelsInnerMarginVertical.Value = doc.VerticalInnerMarginMm;
                numLabelsLabelSizeX.Value = doc.LabelXSizeMm;
                numLabelsLabelSizeY.Value = doc.LabelYSizeMm;
                numLabelsFontSize.Value = doc.FontSize;
                ddLabelsFont.SelectedValue = doc.FontName;
            }
            labelsChanged = false;
        }
        private void ddLabelUsage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Trace.WriteLine("ddLabelUsage_SelectedIndexChanged");
        }

        private bool labelsChanged = false;
        /*
        1" = 25.4 mm

        Några Etikettark

        Avery 5160
        Length: 2.5935"  Height: 1" 
        Top Margin: 0.5" Bottom Margin: 0.5" 
        Left Margin: 0.1875" Right Margin: 0.1875" 
        Hor. Spacing (gutter): 0.15625" Vert. Spacing  (gutter): 0" 
         */
        private const float InchToMM = (float)(25.4);
        private void ddLabelType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ddLabelType_SelectedIndexChanged();
        }
        private void ddLabelType_SelectedIndexChanged()
        {
            PrintLabelDocument.PrintLabelDocumentTypeEnum type =
                (PrintLabelDocument.PrintLabelDocumentTypeEnum)
                ddLabelType.SelectedItem;
            switch(type)
            {
                case PrintLabelDocument.PrintLabelDocumentTypeEnum.Avery6150:
                {
                    numLabelsTopMargin.Value = (int)(0.5 * InchToMM);
                    numLabelsLeftMargin.Value = (int)(0.1875 * InchToMM);
                    numLabelsLabelSizeX.Value = (int)(63.5);
                    numLabelsLabelSizeY.Value = (int)(46.6);
                    numLabelsNrOfLabelsX.Value = 3;
                    numLabelsNrOfLabelsY.Value = 9;
                    numLabelsInnerMarginHorisontal.Value = (int)(0.15625 * InchToMM);
                    numLabelsInnerMarginVertical.Value = (int)(0 * InchToMM);
                    ddLabelsFont.SelectedIndex = 0;
                    numLabelsFontSize.Value = 10;
                    setLabelMesauresVisibillity(false);
                    break;
                }
                case PrintLabelDocument.PrintLabelDocumentTypeEnum.Anpassad:
                {
                    ddLabelsFont.SelectedIndex = 0;
                    numLabelsFontSize.Value = 10;
                    setLabelMesauresVisibillity(true);
                    break;
                }
            }
            this.btnApply.Enabled = true;
            labelsChanged = true;
        }
        private void setLabelMesauresVisibillity(bool visibillity)
        {
            numLabelsTopMargin.Enabled = visibillity;
            numLabelsLeftMargin.Enabled = visibillity;
            numLabelsLabelSizeX.Enabled = visibillity;
            numLabelsLabelSizeY.Enabled = visibillity;
            numLabelsNrOfLabelsX.Enabled = visibillity;
            numLabelsNrOfLabelsY.Enabled = visibillity;
            numLabelsInnerMarginHorisontal.Enabled = visibillity;
            numLabelsInnerMarginVertical.Enabled = visibillity;
        }
        private void saveSettingsLabels(ISettings settings)
        {
            //PrintLabelDocument doc = settings.PrinterSettings.MirrorPrintLabelDocument;
            int horizontalInnerMarginMm = (int)numLabelsInnerMarginHorisontal.Value;
            int labelXSizeMm = (int)numLabelsLabelSizeX.Value;
            int labelYSizeMm = (int)numLabelsLabelSizeY.Value;
            int leftMarginMm = (int)numLabelsLeftMargin.Value;
            int nrOfLabelsX = (int)numLabelsNrOfLabelsX.Value;
            int nrOfLabelsY = (int)numLabelsNrOfLabelsY.Value;
            int topMarginMm = (int)numLabelsTopMargin.Value;
            int verticalInnerMarginMm = (int)numLabelsInnerMarginVertical.Value;

            string fontName = ddLabelsFont.SelectedItem.ToString();
            int fontSize = (int)numLabelsFontSize.Value;
            PrintLabelDocument.PrintLabelDocumentTypeEnum printLabelDocumentType = 
                (PrintLabelDocument.PrintLabelDocumentTypeEnum)
                ddLabelType.SelectedItem;

            PrintLabelDocument doc = new PrintLabelDocument(
                printLabelDocumentType, 
                210, 
                297, 
                nrOfLabelsX, 
                nrOfLabelsY, 
                labelXSizeMm, 
                labelYSizeMm, 
                leftMarginMm, 
                topMarginMm, 
                horizontalInnerMarginMm, 
                verticalInnerMarginMm, 
                fontName, 
                fontSize);
            switch (ddLabelUsage.SelectedIndex)
            {
                case 0: // Speglar
                    settings.PrinterSettings.LabelMirrorPrintDocument = doc;
                    break;
                case 1: // Resultat
                    settings.PrinterSettings.LabelResultDocument = doc;
                    break;
            }

        }
        #endregion

        #region Printouts
        bool printoutsSettingsChanged = false;
        private void DDPrintoutType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("FSettings:DDPrintoutType_SelectedIndexChanged()");
            panelPrintResults.Visible = false;
            PrintDocumentStd doc = getCurrentSettings();

            switch (DDPrintoutType.SelectedIndex)
            {
                case 0:
                    panelPrintResults.Visible = true;
                    DDPrintoutFont.Items.Clear();
                    foreach (FontInfo font in doc.Fonts)
                    {
                        if (font.PrintName != null)
                            DDPrintoutFont.Items.Add(font.PrintName);
                    }

                    DDPrintoutColumn.Items.Clear();
                    foreach (PrintColumnInfo col in doc.Columns)
                    {
                        DDPrintoutColumn.Items.Add(col.Name);
                    }

                    if (DDPrintoutFont.Items.Count > 0)
                        DDPrintoutFont.SelectedIndex = 0;
                    if (DDPrintoutColumn.Items.Count > 0)
                        DDPrintoutColumn.SelectedIndex = 0;

                    if (doc.Landscape)
                        DDPrintoutOrientation.SelectedIndex = 1;
                    else
                        DDPrintoutOrientation.SelectedIndex = 0;
                    break;
                default:
                    throw new NotImplementedException(
                        "Index " + DDPrintoutType.SelectedIndex + " is not implemented");
            }
        }

        private PrintDocumentStd getCurrentSettings()
        {
            Trace.WriteLine("FSettings:getCurrentSettings()");
            switch (DDPrintoutType.SelectedIndex)
            {
                case 0:
                    return CommonCode.Settings.PrinterSettings.PaperResultDocument;
                default:
                    throw new NotImplementedException(
                        "Index " + DDPrintoutType.SelectedIndex + " is not implemented");
            }
        }

        private void setCurrentSettings(PrintDocumentStd doc)
        {
            Trace.WriteLine("FSettings:setCurrentSettings(" + doc.ToString() + ")");
            switch (DDPrintoutType.SelectedIndex)
            {
                case 0:
                    CommonCode.Settings.PrinterSettings.PaperResultDocument = doc;
                    break;
                default:
                    throw new NotImplementedException(
                        "Index " + DDPrintoutType.SelectedIndex + " is not implemented");
            }
        }

        #region Font changes
        private void DDPrintoutFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("FSettings:DDPrintoutFont_SelectedIndexChanged()");
            PrintDocumentStd doc = getCurrentSettings();

            System.Drawing.Font currentfont = doc.Fonts[DDPrintoutFont.SelectedIndex].PrintFont;
            this.lblFontSample.Font = currentfont;
        }

        private void btnPrintsFontChange_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("FSettings:btnPrintsFontChange_Click()");
            PrintDocumentStd doc = getCurrentSettings();

            System.Drawing.Font currentfont = doc.Fonts[DDPrintoutFont.SelectedIndex].PrintFont;
            this.lblFontSample.Font = currentfont;

            this.fontDialog1.Font = currentfont;
            DialogResult res = fontDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                doc.Fonts[DDPrintoutFont.SelectedIndex].PrintFont = fontDialog1.Font;
                setCurrentSettings(doc);
                lblFontSample.Font = fontDialog1.Font;
            }
        }
        #endregion

        #region Column changes
        private void DDPrintoutColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("FSettings:DDPrintoutColumn_SelectedIndexChanged() started");
            PrintDocumentStd doc = getCurrentSettings();
            PrintColumnInfo currentColumn = doc.Columns[DDPrintoutColumn.SelectedIndex];

            numericPrintoutColumnWidth.Value = (decimal)currentColumn.SizeMm;
            Trace.WriteLine("FSettings:DDPrintoutColumn_SelectedIndexChanged() ended");
        }
        private void btnPrintsColsChange_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("FSettings:btnPrintsColsChange_Click() started");

            PrintDocumentStd doc = getCurrentSettings();
            PrintColumnInfo currentColumn = doc.Columns[DDPrintoutColumn.SelectedIndex];

            currentColumn.SizeMm = (float)numericPrintoutColumnWidth.Value;
            setCurrentSettings(doc);
            Trace.WriteLine("FSettings:btnPrintsColsChange_Click() ended");
        }
        #endregion

        #region Orientation
        private void btnPrintsOrientationChange_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("FSettings:btnPrintsOrientationChange_Click() started");

            PrintDocumentStd doc = getCurrentSettings();
            if (DDPrintoutOrientation.SelectedIndex == 0)
                doc.Landscape = false;
            else
                doc.Landscape = true;

            setCurrentSettings(doc);

            Trace.WriteLine("FSettings:btnPrintsOrientationChange_Click() ended");
        }
        #endregion


        private void saveSettingsPrintouts()
        {
            Trace.WriteLine("FSettings:saveSettingsPrintouts()");

        }
        #endregion
    }
}
