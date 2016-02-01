// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FMain.cs" company="John Allberg">
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
//   Summary description for FMain.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#define VisualStudio

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using Allberg.Shooter.Windows.Forms;
    using Allberg.Shooter.WinShooterServerRemoting;
    using Microsoft.Win32;

    /// <summary>
    /// Summary description for FMain.
    /// </summary>
    public class FMain : Form
    {
        private MainMenu mainMenu1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuHelpAbout;
        private MenuItem menuFileExit;
        private MenuItem menuFileNew;
        private MenuItem menuFileOpen;
        private SaveFileDialog saveFileDialog1;
        private SafeLabel SafeLabel1;
        private SafeLabel SafeLabel2;
        private Forms.SafeLabel lblPatrolCountHeader;
        private SafeButton btnCompetition;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private SafeButton btnClubs;
        //private SafeButton btnCompetitors;
        private SafeButton btnCompetitors;
        private SafeButton btnPatrols;
        private SafeButton btnWeapons;
        private MenuItem menuFile;
        private SafeLabel lblShooterCount;
        private SafeLabel lblCompetitorCount;
        private SafeLabel lblPatrolCount;
        private SafeLabel SafeLabel4;
        private SafeLabel lblClubCount;
        private SafeButton btnStations;
        private SafeButton btnResults;
        private SafeButton btnViewResults;
        private MenuItem menuPrint;
        private MenuItem menuPrintResultA;
        private MenuItem menuPrintResultB;
        private MenuItem menuPrintResultC;
        private MenuItem menuPrintResults;
        private MenuItem menuInternet;
        private MenuItem menuInternetPatrollist;
        private MenuItem menuInternetResults;
        private System.Windows.Forms.SaveFileDialog saveFileDialogHtml;
        private MenuItem menuPrintStatistics;

        private MenuItem menuPrintResultR;
        private MenuItem menuPrintResultM;
        private MenuItem menuImport;
        private MenuItem menuImportText;
        private MenuItem menuPrintPatrollist;
        private MenuItem menuPrintPatrollistByPatrol;
        private MenuItem menuPrintPatrollistByClub;
        private MenuItem menuPrintPatrollistByShooter;
        private System.Windows.Forms.ToolTip toolTip1;
        private MenuItem menuItem1;
        private MenuItem menuItem4;
        private MenuItem menuHelpInternet;
        private MenuItem menuHelpInternetSupport;
        private MenuItem menuHelpInternetAllberg;
        private MenuItem menuFileConnect;
        private MenuItem menuItem6;
        private MenuItem menuPrintResultAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private MenuItem menuPrintPrelResultAll;
        private MenuItem menuPrintPrelResultA;
        private MenuItem menuPrintPrelResultB;
        private MenuItem menuPrintPrelResultC;
        private MenuItem menuPrintPrelResultR;
        private MenuItem menuPrintPrelResultM;
        private MenuItem menuPrintPrelResults;
        private MenuItem menuPrintPrelResultAllPreview;
        private MenuItem menuPrintResultAllPreview;
        private MenuItem menuFileLine1;
        private SafeButton btnTeamManagement;
        private MenuItem menuInternetPatrollistByClub;
        private MenuItem menuPrintResultClub;
        private MenuItem menuHelpManual;
        private MenuItem menuPrintPrelResultPatrol;
        private MenuItem menuPdf;
        private MenuItem menuPdfPatrollist;
        private MenuItem menuPdfPatrollistByClub;
        private MenuItem menuPdfResults;
        private MenuItem menuSettings;
        private MenuItem menuPrintLabels;
        private MenuItem menuPrintLabelsMirrors;
        private MenuItem menuFileUpdate;
        private MenuItem menuItem7;
        private MenuItem menuFileUpdateBeta;
        private Allberg.Shooter.Windows.Forms.SafeProgressBar progressBar1;
        //private System.Windows.Forms.ProgressBar progressBar1;
        private MenuItem menuExport;
        private MenuItem menuExportXml;
        private MenuItem menuExportText;
        private MenuItem menuPrintFollowup;
        private MenuItem menuPrintFollowupGroupByClub;
        private MenuItem menuExcel;
        private MenuItem menuExcelResults;
        private System.ComponentModel.IContainer components;


        string FilenameToOpen = null;
        private delegate void StringMethodInvoker(string thisValue);
        private StringMethodInvoker SetlblPatrolCount;
        private StringMethodInvoker SetlblShooterCount;
        private StringMethodInvoker SetlblCompetitorCount;
        private StringMethodInvoker SetlblClubCount;
        private MenuItem menuTestErrorHandling;
        private MenuItem menuExportWeapons;
        private MenuItem menuExportClubs;
        private MenuItem menuItem5;
        private MenuItem menuFileUpdateShooters;
        private MenuItem menuPrintLabelsResultPatrol;
        private System.Windows.Forms.Timer timerEasterEgg;
        private MenuItem menuPrintPrinterTest;
        private MethodInvoker SetHeaderHandler;
        private MethodInvoker UpdateDisplayStringsEvent;

        #region Startup
        public FMain(string filename)
        {
            try
            {
                Trace.WriteLine("FMain started.");
                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();

                Trace.WriteLine("FMain: Creating FMain.");
                if (filename != null)
                {
                    Trace.WriteLine("FMain: Filename = \"" + filename + "\"");
                    FilenameToOpen = filename;
                }

                Thread.CurrentThread.Name = "Main Thread";

                // Setup a few handlers
                SetlblPatrolCount += new StringMethodInvoker(setlblPatrolCount);
                SetlblShooterCount += new StringMethodInvoker(setlblShooterCount);
                SetlblCompetitorCount += new StringMethodInvoker(setlblCompetitorCount);
                SetlblClubCount += new StringMethodInvoker(setlblClubCount);
                SetHeaderHandler += new MethodInvoker(setHeader);
                MenuItemEnabledInvoker += new MenuItemEnabledHandler(MenuItemEnabled);
                MenuItemVisibleInvoker += new MenuItemVisibleHandler(MenuItemVisible);
                UpdateWinShooterNowEvent += new UpdateWinShooterNowDelegate(updateWinShooterNow);
                UpdateDisplayStringsEvent += new MethodInvoker(updateDisplayStrings);


                // If mono, no splash
#if VisualStudio
                // Open SplashScreen
                Trace.WriteLine("FMain: Viewing Splash");
                FSplash.ShowSplash();
                while(FSplash.theInstance == null)
                    Thread.Sleep(50);
                FSplash splash = FSplash.theInstance;
#endif

                EnableMain += new MethodInvoker(enableMain);

                FMainStartupMethodInvoker += new MethodInvoker(FMainStartup);
                Thread FMainStartupThread =
                    new Thread(
                    new ThreadStart(FMainStartupDummy));
                FMainStartupThread.Name = "FMain.FMainStartupThread";
                FMainStartupThread.IsBackground = true;
                FMainStartupThread.Start();
                Trace.WriteLine("FMain: FMain started successfully from thread \"" + 
                    Thread.CurrentThread.Name + "\" " +
                    " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                    DateTime.Now.ToLongTimeString());
            }
            finally
            {
                Trace.WriteLine("FMain created.");

            }
        }
        private MethodInvoker FMainStartupMethodInvoker;
        private void FMainStartupDummy()
        {
            Trace.WriteLine("FMain: FMainStartupDummy started from thread \"" + 
                Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());
            Trace.WriteLine("FMainStartupDummy started.");
            while(!IsHandleCreated)
                Thread.Sleep(50);

            BeginInvoke(FMainStartupMethodInvoker);
        }

        private void FMainStartup()
        {
            Trace.WriteLine("FMain: FMainStartup started from thread \"" + 
                Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());
            Trace.WriteLine("FMainStartup started.");

            try
            {
                getAssemblyVersion();

                // Let's rock'n roll!
                setHeader();

                // Common code
                CommonCode = new Common.Interface();

                // About window
                Trace.WriteLine("Creating about window");
                myAboutWindow = new FAbout();
                myAboutWindow.EnableMain += 
                    new MethodInvoker(enableMain);

                // Competition window field
                Trace.WriteLine("Creating Competition field window");
                myCompetitionFieldWindow = new FCompetitionField(ref CommonCode);
                myCompetitionFieldWindow.EnableMain += 
                    new FCompetitionField.EnableMainHandler(enableMain);

                // Competition window precision
                Trace.WriteLine("Creating Competition precision window");
                myCompetitionPrecisionWindow = new FCompetitionPrecision(ref CommonCode);
                myCompetitionPrecisionWindow.EnableMain += 
                    new FCompetitionPrecision.EnableMainHandler(enableMain);

                // Stations Field window
                Trace.WriteLine("Creating StationField window");
                myStationsFieldWindow = new FStationsField(ref CommonCode);
                myStationsFieldWindow.EnableMain += 
                    new FStationsField.EnableMainHandler(enableMain);

                // Stations Ban window
                Trace.WriteLine("Creating StationPrecision window");
                myStationsPrecisionWindow = new FStationsPrecision(ref CommonCode);
                myStationsPrecisionWindow.EnableMain += 
                    new FStationsPrecision.EnableMainHandler(enableMain);

                // Clubs window
                Trace.WriteLine("Creating Clubs window");
                myClubsWindow = new FClubs(ref CommonCode);
                myClubsWindow.EnableMain += 
                    new FClubs.EnableMainHandler(enableMain);

                // Competitors Window
                Trace.WriteLine("Creating Competitors window");
                myCompetitorsWindow = new FCompetitors(ref CommonCode);
                myCompetitorsWindow.EnableMain+=
                    new FCompetitors.EnableMainHandler(enableMain);

                // Weapons Window
                Trace.WriteLine("Creating Weapons window");
                myWeaponsWindow = new FWeapons(ref CommonCode);
                myWeaponsWindow.EnableMain +=
                    new FWeapons.EnableMainHandler(enableMain);

                // Patrols Window
                Trace.WriteLine("Creating Patrols window");
                myPatrolsWindow = new FPatrols(ref CommonCode);
                myPatrolsWindow.EnableMain +=
                    new FPatrols.EnableMainHandler(enableMain);

                // Results Window
                Trace.WriteLine("Creating Results window");
                myResultsWindow = new FResults(ref CommonCode);
                myResultsWindow.EnableMain +=
                    new FResults.EnableMainHandler(enableMain);
                myResultsWindow.PrintPatrolResult +=
                    new FResults.PrintPatrolResultHandler(menuPrintPrelResultPatrol_Print);

                // ResultsView Window
                Trace.WriteLine("Creating ResultsView window");
                myResultsViewWindow = new FResultsView(ref CommonCode);
                myResultsViewWindow.EnableMain +=
                    new FResultsView.EnableMainHandler(enableMain);

                // Import Window
                Trace.WriteLine("Creating Import window");
                myImportWindow = new FImport(ref CommonCode);
                myImportWindow.EnableMain +=
                    new FImport.EnableMainHandler(enableMain);

                // ServerConnect Window
                Trace.WriteLine("Creating ServerConnect window");
                myServerConnectWindow = new FServerConnect();
                myServerConnectWindow.EnableMain +=
                    new FServerConnect.EnableMainHandler(enableMain);
                myServerConnectWindow.ConnectToServer +=
                    new FServerConnect.ConnectToServerHandler(ConnectToServer);

                // Teams Window
                Trace.WriteLine("Create Teams Window");
                myTeamsWindow = new FTeams(ref CommonCode);
                myTeamsWindow.EnableMain +=
                    new FTeams.EnableMainHandler(enableMain);

                // Settings window
                Trace.WriteLine("Create settings window");
                mySettingsWindow = new FSettings(ref CommonCode);
                mySettingsWindow.EnableMain +=
                    new FSettings.EnableMainHandler(enableMain);

                // Updating handlers
                Trace.WriteLine("Updating Handlers.");
                CommonCode.UpdatedCompetition += 
                    new MethodInvoker(
                    CommonCode_UpdatedCompetition);
                CommonCode.UpdatedStation +=
                    new MethodInvoker(
                    CommonCode_UpdatedStation);
                CommonCode.UpdatedClub +=
                    new MethodInvoker(
                    CommonCode_UpdatedClub);
                CommonCode.UpdatedWeapon +=
                    new MethodInvoker(
                    CommonCode_UpdatedWeapon);
                CommonCode.UpdatedShooter+=
                    new MethodInvoker(
                    CommonCode_UpdatedShooter);
                CommonCode.UpdatedCompetitor +=
                    new MethodInvoker(
                    CommonCode_UpdatedCompetitor);
                CommonCode.UpdatedCompetitorResult +=
                    new MethodInvoker(
                    CommonCode_UpdatedCompetitorResult);
                CommonCode.UpdatedPatrol +=
                    new MethodInvoker(
                    CommonCode_UpdatedPatrol);
                CommonCode.UpdatedTeam +=
                    new MethodInvoker(
                    CommonCode_UpdatedTeam);

                //assemblyDir =
                //	System.IO.Directory.GetCurrentDirectory();

                // Activate main window
                Trace.WriteLine("Activating main window");
                menuFile.Enabled = true;
                Activate();
#if VisualStudio	
                // If mono, no splash
                FSplash.RemoveSplash();
#endif

                // Autoopen
                if (FilenameToOpen != null)
                {
                    FileOpen(FilenameToOpen);
                }

#if !VisualStudio
                // If this is mono, ADOX is not supported
                menuFileNew.Visible = false;
                menuFileOpen.Visible = false;
                menuFileLine1.Visible = false;
                
                // If this is mono, printing is not supported
                menuPrint.Visible = false;
                
                // If this is linux, IE is not supported. This is possible with mono
                menuHelpInternet.Visible = false;
                menuHelpInternetSupport.Visible = false;
                menuHelpInternetAllberg.Visible = false;
#endif

#if DEBUG
                menuFileUpdateBeta.Visible = true;
#endif

#if DEBUG
                checkForNewerVersion(false);
#else
                // Check if there is a newer version
                Thread updateVersionThread = 
                    new Thread(
                    new ThreadStart(checkForNewerVersion));
                updateVersionThread.Name = "updateVersionThread";
                updateVersionThread.IsBackground = true;
                updateVersionThread.Start();
#endif

#if DEBUG
                menuTestErrorHandling.Visible=true;
#endif

                if (DateTime.Now.Month == 4 &&
                    DateTime.Now.Day == 1)
                {
                    timerEasterEgg.Enabled = true;
                    timerEasterEgg.Start();
                }
                Trace.WriteLine("FMain: Successfully created from thread.");
            }
            catch(Exception exc)
            {
#if VisualStudio
                // If mono, no splash
                FSplash.RemoveSplash();
#endif
                Trace.WriteLine("Exception during init of FMain:" + exc.ToString());
                MessageBox.Show("Ett fel uppstod vid start av WinShooter:\r\n" + exc.ToString());
            }
            finally
            {
                Trace.WriteLine("FMain: (Finally) created from thread \"" + 
                    Thread.CurrentThread.Name + "\" " +
                    " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                    DateTime.Now.ToLongTimeString());
            }
        }
        #endregion

        internal Common.Interface CommonCode;
        private FAbout myAboutWindow;
        private FCompetitionField myCompetitionFieldWindow;
        private FCompetitionPrecision myCompetitionPrecisionWindow;
        private FStationsField myStationsFieldWindow;
        private FStationsPrecision myStationsPrecisionWindow;
        private FClubs myClubsWindow;
        private FWeapons myWeaponsWindow;
        private FCompetitors myCompetitorsWindow;
        private FPatrols myPatrolsWindow;
        private FResults myResultsWindow;
        private FResultsView myResultsViewWindow;
        private FImport myImportWindow;
        private FServerConnect myServerConnectWindow;
        private FTeams myTeamsWindow;
        private FSettings mySettingsWindow;
        //private string assemblyDir;

        #region Disposing
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FMain: Dispose(" + disposing.ToString() + ")" +
                "from thread \"" + Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            try
            {
                Trace.WriteLine("Disposing FAbout");
                myAboutWindow.DisposeNow = true;
                myAboutWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FClubs");
                myClubsWindow.DisposeNow = true;
                myClubsWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FCompetitionField");
                myCompetitionFieldWindow.DisposeNow = true;
                myCompetitionFieldWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FCompetitionBan");
                myCompetitionPrecisionWindow.DisposeNow = true;
                myCompetitionPrecisionWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FCompetitors");
                myCompetitorsWindow.DisposeNow = true;
                myCompetitorsWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FPatrols");
                myPatrolsWindow.DisposeNow = true;
                myPatrolsWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FStationsField");
                myStationsFieldWindow.DisposeNow = true;
                myStationsFieldWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FStationsBan");
                myStationsPrecisionWindow.DisposeNow = true;
                myStationsPrecisionWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FWeapons");
                myWeaponsWindow.DisposeNow = true;
                myWeaponsWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FResults");
                myResultsWindow.DisposeNow = true;
                myResultsWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FResultsView");
                myResultsViewWindow.DisposeNow = true;
                myResultsViewWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FTeams");
                myTeamsWindow.DisposeNow = true;
                myTeamsWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
            {
                Trace.WriteLine("Disposing FSettings");
                mySettingsWindow.DisposeNow = true;
                mySettingsWindow.Dispose();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }
            try
            {
                CommonCode.Dispose();
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }

            try
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
            catch(Exception)
            {
            }

            try
            {
                Trace.WriteLine("FMain: Dispose(" + disposing.ToString() + ")" +
                    "ended " +
                    DateTime.Now.ToLongTimeString());
            }
            catch(Exception)
            {
            }
        }
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuFileNew = new System.Windows.Forms.MenuItem();
            this.menuFileOpen = new System.Windows.Forms.MenuItem();
            this.menuFileLine1 = new System.Windows.Forms.MenuItem();
            this.menuFileConnect = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuPrint = new System.Windows.Forms.MenuItem();
            this.menuPrintPatrollist = new System.Windows.Forms.MenuItem();
            this.menuPrintPatrollistByPatrol = new System.Windows.Forms.MenuItem();
            this.menuPrintPatrollistByClub = new System.Windows.Forms.MenuItem();
            this.menuPrintPatrollistByShooter = new System.Windows.Forms.MenuItem();
            this.menuPrintPrelResults = new System.Windows.Forms.MenuItem();
            this.menuPrintPrelResultAllPreview = new System.Windows.Forms.MenuItem();
            this.menuPrintPrelResultAll = new System.Windows.Forms.MenuItem();
            this.menuPrintPrelResultA = new System.Windows.Forms.MenuItem();
            this.menuPrintPrelResultB = new System.Windows.Forms.MenuItem();
            this.menuPrintPrelResultC = new System.Windows.Forms.MenuItem();
            this.menuPrintPrelResultR = new System.Windows.Forms.MenuItem();
            this.menuPrintPrelResultM = new System.Windows.Forms.MenuItem();
            this.menuPrintPrelResultPatrol = new System.Windows.Forms.MenuItem();
            this.menuPrintResults = new System.Windows.Forms.MenuItem();
            this.menuPrintResultAllPreview = new System.Windows.Forms.MenuItem();
            this.menuPrintResultAll = new System.Windows.Forms.MenuItem();
            this.menuPrintResultA = new System.Windows.Forms.MenuItem();
            this.menuPrintResultB = new System.Windows.Forms.MenuItem();
            this.menuPrintResultC = new System.Windows.Forms.MenuItem();
            this.menuPrintResultR = new System.Windows.Forms.MenuItem();
            this.menuPrintResultM = new System.Windows.Forms.MenuItem();
            this.menuPrintResultClub = new System.Windows.Forms.MenuItem();
            this.menuPrintStatistics = new System.Windows.Forms.MenuItem();
            this.menuPrintLabels = new System.Windows.Forms.MenuItem();
            this.menuPrintLabelsMirrors = new System.Windows.Forms.MenuItem();
            this.menuPrintLabelsResultPatrol = new System.Windows.Forms.MenuItem();
            this.menuPrintFollowup = new System.Windows.Forms.MenuItem();
            this.menuPrintFollowupGroupByClub = new System.Windows.Forms.MenuItem();
            this.menuPrintPrinterTest = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuFileUpdate = new System.Windows.Forms.MenuItem();
            this.menuFileUpdateBeta = new System.Windows.Forms.MenuItem();
            this.menuFileUpdateShooters = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuFileExit = new System.Windows.Forms.MenuItem();
            this.menuImport = new System.Windows.Forms.MenuItem();
            this.menuImportText = new System.Windows.Forms.MenuItem();
            this.menuExport = new System.Windows.Forms.MenuItem();
            this.menuPdf = new System.Windows.Forms.MenuItem();
            this.menuPdfPatrollist = new System.Windows.Forms.MenuItem();
            this.menuPdfPatrollistByClub = new System.Windows.Forms.MenuItem();
            this.menuPdfResults = new System.Windows.Forms.MenuItem();
            this.menuInternet = new System.Windows.Forms.MenuItem();
            this.menuInternetPatrollist = new System.Windows.Forms.MenuItem();
            this.menuInternetPatrollistByClub = new System.Windows.Forms.MenuItem();
            this.menuInternetResults = new System.Windows.Forms.MenuItem();
            this.menuExcel = new System.Windows.Forms.MenuItem();
            this.menuExcelResults = new System.Windows.Forms.MenuItem();
            this.menuExportXml = new System.Windows.Forms.MenuItem();
            this.menuExportText = new System.Windows.Forms.MenuItem();
            this.menuExportWeapons = new System.Windows.Forms.MenuItem();
            this.menuExportClubs = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuSettings = new System.Windows.Forms.MenuItem();
            this.menuTestErrorHandling = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuHelpManual = new System.Windows.Forms.MenuItem();
            this.menuHelpInternet = new System.Windows.Forms.MenuItem();
            this.menuHelpInternetSupport = new System.Windows.Forms.MenuItem();
            this.menuHelpInternetAllberg = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuHelpAbout = new System.Windows.Forms.MenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SafeLabel1 = new SafeLabel();
            this.lblClubCount = new SafeLabel();
            this.SafeLabel4 = new SafeLabel();
            this.lblPatrolCount = new SafeLabel();
            this.lblCompetitorCount = new SafeLabel();
            this.lblShooterCount = new SafeLabel();
            this.lblPatrolCountHeader = new SafeLabel();
            this.SafeLabel2 = new SafeLabel();
            this.btnCompetition = new SafeButton();
            this.btnStations = new SafeButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnClubs = new SafeButton();
            this.btnCompetitors = new SafeButton();
            this.btnPatrols = new SafeButton();
            this.btnWeapons = new SafeButton();
            this.btnResults = new SafeButton();
            this.btnViewResults = new SafeButton();
            this.saveFileDialogHtml = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnTeamManagement = new SafeButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new Allberg.Shooter.Windows.Forms.SafeProgressBar();
            this.timerEasterEgg = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuImport,
            this.menuExport,
            this.menuItem4,
            this.menuItem2});
            // 
            // menuFile
            // 
            this.menuFile.Enabled = false;
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFileNew,
            this.menuFileOpen,
            this.menuFileLine1,
            this.menuFileConnect,
            this.menuItem6,
            this.menuPrint,
            this.menuItem1,
            this.menuItem5,
            this.menuItem7,
            this.menuFileExit});
            this.menuFile.Text = "&Arkiv";
            // 
            // menuFileNew
            // 
            this.menuFileNew.Index = 0;
            this.menuFileNew.Text = "&Ny tävling";
            this.menuFileNew.Click += new System.EventHandler(this.menuFileNew_Click);
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Index = 1;
            this.menuFileOpen.Text = "&Öppna tävling";
            this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // menuFileLine1
            // 
            this.menuFileLine1.Index = 2;
            this.menuFileLine1.Text = "-";
            // 
            // menuFileConnect
            // 
            this.menuFileConnect.Index = 3;
            this.menuFileConnect.Text = "A&nslut till server";
            this.menuFileConnect.Click += new System.EventHandler(this.menuFileConnect_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 4;
            this.menuItem6.Text = "-";
            this.menuItem6.Visible = false;
            // 
            // menuPrint
            // 
            this.menuPrint.Enabled = false;
            this.menuPrint.Index = 5;
            this.menuPrint.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPrintPatrollist,
            this.menuPrintPrelResults,
            this.menuPrintResults,
            this.menuPrintStatistics,
            this.menuPrintLabels,
            this.menuPrintFollowup,
            this.menuPrintPrinterTest});
            this.menuPrint.Text = "&Skriv ut";
            // 
            // menuPrintPatrollist
            // 
            this.menuPrintPatrollist.Enabled = false;
            this.menuPrintPatrollist.Index = 0;
            this.menuPrintPatrollist.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPrintPatrollistByPatrol,
            this.menuPrintPatrollistByClub,
            this.menuPrintPatrollistByShooter});
            this.menuPrintPatrollist.Text = "Patrull";
            // 
            // menuPrintPatrollistByPatrol
            // 
            this.menuPrintPatrollistByPatrol.Index = 0;
            this.menuPrintPatrollistByPatrol.Text = "Efter Patrull (Patrullpärm)";
            this.menuPrintPatrollistByPatrol.Click += new System.EventHandler(this.menuPrintPatrollistByPatrol_Click);
            // 
            // menuPrintPatrollistByClub
            // 
            this.menuPrintPatrollistByClub.Index = 1;
            this.menuPrintPatrollistByClub.Text = "Efter Klubb";
            this.menuPrintPatrollistByClub.Click += new System.EventHandler(this.menuPrintPatrollistByClub_Click);
            // 
            // menuPrintPatrollistByShooter
            // 
            this.menuPrintPatrollistByShooter.Index = 2;
            this.menuPrintPatrollistByShooter.Text = "Efter Skytt";
            this.menuPrintPatrollistByShooter.Click += new System.EventHandler(this.menuPrintPatrollistByShooter_Click);
            // 
            // menuPrintPrelResults
            // 
            this.menuPrintPrelResults.Enabled = false;
            this.menuPrintPrelResults.Index = 1;
            this.menuPrintPrelResults.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPrintPrelResultAllPreview,
            this.menuPrintPrelResultAll,
            this.menuPrintPrelResultA,
            this.menuPrintPrelResultB,
            this.menuPrintPrelResultC,
            this.menuPrintPrelResultR,
            this.menuPrintPrelResultM,
            this.menuPrintPrelResultPatrol});
            this.menuPrintPrelResults.Text = "Resultatlista (preliminär)";
            // 
            // menuPrintPrelResultAllPreview
            // 
            this.menuPrintPrelResultAllPreview.Index = 0;
            this.menuPrintPrelResultAllPreview.Text = "Alla (Förhandsgranskning)";
            this.menuPrintPrelResultAllPreview.Click += new System.EventHandler(this.menuPrintPrelResultAllPreview_Click);
            // 
            // menuPrintPrelResultAll
            // 
            this.menuPrintPrelResultAll.Index = 1;
            this.menuPrintPrelResultAll.Text = "Alla";
            this.menuPrintPrelResultAll.Click += new System.EventHandler(this.menuPrintPrelResultAll_Click);
            // 
            // menuPrintPrelResultA
            // 
            this.menuPrintPrelResultA.Index = 2;
            this.menuPrintPrelResultA.Text = "Vapengrupp A";
            this.menuPrintPrelResultA.Click += new System.EventHandler(this.menuPrintPrelResultA_Click);
            // 
            // menuPrintPrelResultB
            // 
            this.menuPrintPrelResultB.Index = 3;
            this.menuPrintPrelResultB.Text = "Vapengrupp B";
            this.menuPrintPrelResultB.Click += new System.EventHandler(this.menuPrintPrelResultB_Click);
            // 
            // menuPrintPrelResultC
            // 
            this.menuPrintPrelResultC.Index = 4;
            this.menuPrintPrelResultC.Text = "Vapengrupp C";
            this.menuPrintPrelResultC.Click += new System.EventHandler(this.menuPrintPrelResultC_Click);
            // 
            // menuPrintPrelResultR
            // 
            this.menuPrintPrelResultR.Index = 5;
            this.menuPrintPrelResultR.Text = "Vapengrupp R";
            this.menuPrintPrelResultR.Click += new System.EventHandler(this.menuPrintPrelResultR_Click);
            // 
            // menuPrintPrelResultM
            // 
            this.menuPrintPrelResultM.Index = 6;
            this.menuPrintPrelResultM.Text = "Vapengrupp M";
            this.menuPrintPrelResultM.Click += new System.EventHandler(this.menuPrintPrelResultM_Click);
            // 
            // menuPrintPrelResultPatrol
            // 
            this.menuPrintPrelResultPatrol.Index = 7;
            this.menuPrintPrelResultPatrol.Text = "Enskild patrull";
            this.menuPrintPrelResultPatrol.Click += new System.EventHandler(this.menuPrintPrelResultPatrol_Click);
            // 
            // menuPrintResults
            // 
            this.menuPrintResults.Enabled = false;
            this.menuPrintResults.Index = 2;
            this.menuPrintResults.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPrintResultAllPreview,
            this.menuPrintResultAll,
            this.menuPrintResultA,
            this.menuPrintResultB,
            this.menuPrintResultC,
            this.menuPrintResultR,
            this.menuPrintResultM,
            this.menuPrintResultClub});
            this.menuPrintResults.Text = "Resultatlista";
            // 
            // menuPrintResultAllPreview
            // 
            this.menuPrintResultAllPreview.Index = 0;
            this.menuPrintResultAllPreview.Text = "Alla (Förhandsgranska)";
            this.menuPrintResultAllPreview.Click += new System.EventHandler(this.menuPrintResultAllPreview_Click);
            // 
            // menuPrintResultAll
            // 
            this.menuPrintResultAll.Index = 1;
            this.menuPrintResultAll.Text = "Alla";
            this.menuPrintResultAll.Click += new System.EventHandler(this.menuPrintResultAll_Click);
            // 
            // menuPrintResultA
            // 
            this.menuPrintResultA.Index = 2;
            this.menuPrintResultA.Text = "Vapengrupp A";
            this.menuPrintResultA.Click += new System.EventHandler(this.menuPrintResultA_Click);
            // 
            // menuPrintResultB
            // 
            this.menuPrintResultB.Index = 3;
            this.menuPrintResultB.Text = "Vapengrupp B";
            this.menuPrintResultB.Click += new System.EventHandler(this.menuPrintResultB_Click);
            // 
            // menuPrintResultC
            // 
            this.menuPrintResultC.Index = 4;
            this.menuPrintResultC.Text = "Vapengrupp C";
            this.menuPrintResultC.Click += new System.EventHandler(this.menuPrintResultC_Click);
            // 
            // menuPrintResultR
            // 
            this.menuPrintResultR.Index = 5;
            this.menuPrintResultR.Text = "Vapengrupp R";
            this.menuPrintResultR.Click += new System.EventHandler(this.menuPrintResultR_Click);
            // 
            // menuPrintResultM
            // 
            this.menuPrintResultM.Index = 6;
            this.menuPrintResultM.Text = "Vapengrupp M";
            this.menuPrintResultM.Click += new System.EventHandler(this.menuPrintResultM_Click);
            // 
            // menuPrintResultClub
            // 
            this.menuPrintResultClub.Index = 7;
            this.menuPrintResultClub.Text = "Klubbmästerskap inom tävling";
            this.menuPrintResultClub.Click += new System.EventHandler(this.menuPrintResultClub_Click);
            // 
            // menuPrintStatistics
            // 
            this.menuPrintStatistics.Index = 3;
            this.menuPrintStatistics.Text = "Statistik";
            this.menuPrintStatistics.Visible = false;
            // 
            // menuPrintLabels
            // 
            this.menuPrintLabels.Index = 4;
            this.menuPrintLabels.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPrintLabelsMirrors,
            this.menuPrintLabelsResultPatrol});
            this.menuPrintLabels.Text = "Etiketter";
            // 
            // menuPrintLabelsMirrors
            // 
            this.menuPrintLabelsMirrors.Index = 0;
            this.menuPrintLabelsMirrors.Text = "För speglar";
            this.menuPrintLabelsMirrors.Visible = false;
            this.menuPrintLabelsMirrors.Click += new System.EventHandler(this.menuPrintLabelsMirrors_Click);
            // 
            // menuPrintLabelsResultPatrol
            // 
            this.menuPrintLabelsResultPatrol.Enabled = false;
            this.menuPrintLabelsResultPatrol.Index = 1;
            this.menuPrintLabelsResultPatrol.Text = "Resultat för patrull";
            this.menuPrintLabelsResultPatrol.Click += new System.EventHandler(this.menuPrintLabelsResultPatrol_Click);
            // 
            // menuPrintFollowup
            // 
            this.menuPrintFollowup.Index = 5;
            this.menuPrintFollowup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPrintFollowupGroupByClub});
            this.menuPrintFollowup.Text = "Uppföljningslista";
            // 
            // menuPrintFollowupGroupByClub
            // 
            this.menuPrintFollowupGroupByClub.Index = 0;
            this.menuPrintFollowupGroupByClub.Text = "Efter klubb";
            this.menuPrintFollowupGroupByClub.Click += new System.EventHandler(this.menuPrintFollowupGroupByClub_Click);
            // 
            // menuPrintPrinterTest
            // 
            this.menuPrintPrinterTest.Index = 6;
            this.menuPrintPrinterTest.Text = "Testa din skrivare";
            this.menuPrintPrinterTest.Click += new System.EventHandler(this.menuPrintPrinterTest_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 6;
            this.menuItem1.Text = "-";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 7;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFileUpdate,
            this.menuFileUpdateBeta,
            this.menuFileUpdateShooters});
            this.menuItem5.Text = "Uppdatering";
            // 
            // menuFileUpdate
            // 
            this.menuFileUpdate.Index = 0;
            this.menuFileUpdate.Text = "&Uppdatera WinShooter";
            this.menuFileUpdate.Click += new System.EventHandler(this.menuFileUpdate_Click);
            // 
            // menuFileUpdateBeta
            // 
            this.menuFileUpdateBeta.Index = 1;
            this.menuFileUpdateBeta.Text = "Uppdatera WinShooter BETA";
            this.menuFileUpdateBeta.Visible = false;
            this.menuFileUpdateBeta.Click += new System.EventHandler(this.menuFileUpdateBeta_Click);
            // 
            // menuFileUpdateShooters
            // 
            this.menuFileUpdateShooters.Index = 2;
            this.menuFileUpdateShooters.Text = "Uppdatera skyttar i WinShooter";
            this.menuFileUpdateShooters.Click += new System.EventHandler(this.menuFileUpdateShooters_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 8;
            this.menuItem7.Text = "-";
            // 
            // menuFileExit
            // 
            this.menuFileExit.Index = 9;
            this.menuFileExit.Text = "A&vsluta";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            // 
            // menuImport
            // 
            this.menuImport.Enabled = false;
            this.menuImport.Index = 1;
            this.menuImport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuImportText});
            this.menuImport.Text = "&Importera";
            // 
            // menuImportText
            // 
            this.menuImportText.Index = 0;
            this.menuImportText.Text = "Skyttar från textfil";
            this.menuImportText.Click += new System.EventHandler(this.menuImportText_Click);
            // 
            // menuExport
            // 
            this.menuExport.Enabled = false;
            this.menuExport.Index = 2;
            this.menuExport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPdf,
            this.menuInternet,
            this.menuExcel,
            this.menuExportXml,
            this.menuExportText,
            this.menuExportWeapons,
            this.menuExportClubs});
            this.menuExport.Text = "&Exportera";
            // 
            // menuPdf
            // 
            this.menuPdf.Enabled = false;
            this.menuPdf.Index = 0;
            this.menuPdf.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPdfPatrollist,
            this.menuPdfPatrollistByClub,
            this.menuPdfResults});
            this.menuPdf.Text = "Exportera till PDF";
            // 
            // menuPdfPatrollist
            // 
            this.menuPdfPatrollist.Index = 0;
            this.menuPdfPatrollist.Text = "Patrullista";
            this.menuPdfPatrollist.Click += new System.EventHandler(this.menuPdfPatrollist_Click);
            // 
            // menuPdfPatrollistByClub
            // 
            this.menuPdfPatrollistByClub.Index = 1;
            this.menuPdfPatrollistByClub.Text = "Patrullista efter klubb";
            this.menuPdfPatrollistByClub.Click += new System.EventHandler(this.menuPdfPatrollistByClub_Click);
            // 
            // menuPdfResults
            // 
            this.menuPdfResults.Index = 2;
            this.menuPdfResults.Text = "Resultat";
            this.menuPdfResults.Click += new System.EventHandler(this.menuPdfResults_Click);
            // 
            // menuInternet
            // 
            this.menuInternet.Enabled = false;
            this.menuInternet.Index = 1;
            this.menuInternet.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuInternetPatrollist,
            this.menuInternetPatrollistByClub,
            this.menuInternetResults});
            this.menuInternet.Text = "Exportera till Webbsida";
            this.menuInternet.Visible = false;
            // 
            // menuInternetPatrollist
            // 
            this.menuInternetPatrollist.Enabled = false;
            this.menuInternetPatrollist.Index = 0;
            this.menuInternetPatrollist.Text = "Patrullista";
            this.menuInternetPatrollist.Click += new System.EventHandler(this.menuInternetPatrollist_Click);
            // 
            // menuInternetPatrollistByClub
            // 
            this.menuInternetPatrollistByClub.Enabled = false;
            this.menuInternetPatrollistByClub.Index = 1;
            this.menuInternetPatrollistByClub.Text = "Patrullista efter klubb";
            this.menuInternetPatrollistByClub.Click += new System.EventHandler(this.menuInternetPatrollistByClub_Click);
            // 
            // menuInternetResults
            // 
            this.menuInternetResults.Enabled = false;
            this.menuInternetResults.Index = 2;
            this.menuInternetResults.Text = "Resultat";
            this.menuInternetResults.Click += new System.EventHandler(this.menuInternetResults_Click);
            // 
            // menuExcel
            // 
            this.menuExcel.Enabled = false;
            this.menuExcel.Index = 2;
            this.menuExcel.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuExcelResults});
            this.menuExcel.Text = "Exportera till Excel";
            // 
            // menuExcelResults
            // 
            this.menuExcelResults.Index = 0;
            this.menuExcelResults.Text = "Resultat";
            this.menuExcelResults.Click += new System.EventHandler(this.menuExcelResults_Click);
            // 
            // menuExportXml
            // 
            this.menuExportXml.Index = 3;
            this.menuExportXml.Text = "Exportera till XML-fil";
            this.menuExportXml.Click += new System.EventHandler(this.menuExportXml_Click);
            // 
            // menuExportText
            // 
            this.menuExportText.Index = 4;
            this.menuExportText.Text = "Exportera till text-fil";
            this.menuExportText.Click += new System.EventHandler(this.menuExportText_Click);
            // 
            // menuExportWeapons
            // 
            this.menuExportWeapons.Index = 5;
            this.menuExportWeapons.Text = "Exportera vapen till textfil";
            this.menuExportWeapons.Click += new System.EventHandler(this.menuExportWeapons_Click);
            // 
            // menuExportClubs
            // 
            this.menuExportClubs.Index = 6;
            this.menuExportClubs.Text = "Exportera klubbar till textfil";
            this.menuExportClubs.Click += new System.EventHandler(this.menuExportClubs_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSettings,
            this.menuTestErrorHandling});
            this.menuItem4.Text = "&Verktyg";
            // 
            // menuSettings
            // 
            this.menuSettings.Index = 0;
            this.menuSettings.Text = "Inställningar";
            this.menuSettings.Click += new System.EventHandler(this.menuSettings_Click);
            // 
            // menuTestErrorHandling
            // 
            this.menuTestErrorHandling.Index = 1;
            this.menuTestErrorHandling.Text = "TestErrorHandling";
            this.menuTestErrorHandling.Visible = false;
            this.menuTestErrorHandling.Click += new System.EventHandler(this.menuTestErrorHandling_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 4;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuHelpManual,
            this.menuHelpInternet,
            this.menuItem3,
            this.menuHelpAbout});
            this.menuItem2.Text = "&Hjälp";
            // 
            // menuHelpManual
            // 
            this.menuHelpManual.Index = 0;
            this.menuHelpManual.Text = "&Manual";
            this.menuHelpManual.Click += new System.EventHandler(this.menuHelpManual_Click);
            // 
            // menuHelpInternet
            // 
            this.menuHelpInternet.Index = 1;
            this.menuHelpInternet.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuHelpInternetSupport,
            this.menuHelpInternetAllberg});
            this.menuHelpInternet.Text = "Internet";
            // 
            // menuHelpInternetSupport
            // 
            this.menuHelpInternetSupport.Index = 0;
            this.menuHelpInternetSupport.Text = "WinShooter Support";
            this.menuHelpInternetSupport.Click += new System.EventHandler(this.menuHelpSupport_Click);
            // 
            // menuHelpInternetAllberg
            // 
            this.menuHelpInternetAllberg.Index = 1;
            this.menuHelpInternetAllberg.Text = "Allberg Konsult AB";
            this.menuHelpInternetAllberg.Click += new System.EventHandler(this.menuHelpInternetAllberg_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "-";
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Index = 3;
            this.menuHelpAbout.Text = "Om WinShooter";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.RestoreDirectory = true;
            // 
            // SafeLabel1
            // 
            this.SafeLabel1.Location = new System.Drawing.Point(16, 16);
            this.SafeLabel1.Name = "SafeLabel1";
            this.SafeLabel1.Size = new System.Drawing.Size(100, 23);
            this.SafeLabel1.TabIndex = 0;
            this.SafeLabel1.Text = "Tävlande:";
            // 
            // lblClubCount
            // 
            this.lblClubCount.Location = new System.Drawing.Point(96, 40);
            this.lblClubCount.Name = "lblClubCount";
            this.lblClubCount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblClubCount.Size = new System.Drawing.Size(56, 23);
            this.lblClubCount.TabIndex = 7;
            this.lblClubCount.Text = "0";
            this.toolTip1.SetToolTip(this.lblClubCount, "Här visas hur många klubbar som anmälningarna har ankommit");
            // 
            // SafeLabel4
            // 
            this.SafeLabel4.Location = new System.Drawing.Point(16, 40);
            this.SafeLabel4.Name = "SafeLabel4";
            this.SafeLabel4.Size = new System.Drawing.Size(100, 23);
            this.SafeLabel4.TabIndex = 6;
            this.SafeLabel4.Text = "Klubbar";
            // 
            // lblPatrolCount
            // 
            this.lblPatrolCount.Location = new System.Drawing.Point(96, 88);
            this.lblPatrolCount.Name = "lblPatrolCount";
            this.lblPatrolCount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPatrolCount.Size = new System.Drawing.Size(56, 23);
            this.lblPatrolCount.TabIndex = 5;
            this.lblPatrolCount.Text = "0";
            this.toolTip1.SetToolTip(this.lblPatrolCount, "Här visas hur många patruller som är uppsatta");
            // 
            // lblCompetitorCount
            // 
            this.lblCompetitorCount.Location = new System.Drawing.Point(96, 64);
            this.lblCompetitorCount.Name = "lblCompetitorCount";
            this.lblCompetitorCount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblCompetitorCount.Size = new System.Drawing.Size(56, 23);
            this.lblCompetitorCount.TabIndex = 4;
            this.lblCompetitorCount.Text = "0";
            this.toolTip1.SetToolTip(this.lblCompetitorCount, "Här visas hur många starter som görs");
            // 
            // lblShooterCount
            // 
            this.lblShooterCount.Location = new System.Drawing.Point(96, 16);
            this.lblShooterCount.Name = "lblShooterCount";
            this.lblShooterCount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblShooterCount.Size = new System.Drawing.Size(56, 23);
            this.lblShooterCount.TabIndex = 3;
            this.lblShooterCount.Text = "0";
            this.toolTip1.SetToolTip(this.lblShooterCount, "Här visas hur många fysiska personer som är inskrivna");
            // 
            // lblPatrolCountHeader
            // 
            this.lblPatrolCountHeader.Location = new System.Drawing.Point(16, 88);
            this.lblPatrolCountHeader.Name = "lblPatrolCountHeader";
            this.lblPatrolCountHeader.Size = new System.Drawing.Size(100, 23);
            this.lblPatrolCountHeader.TabIndex = 2;
            this.lblPatrolCountHeader.Text = "Antal patruller";
            // 
            // SafeLabel2
            // 
            this.SafeLabel2.Location = new System.Drawing.Point(16, 64);
            this.SafeLabel2.Name = "SafeLabel2";
            this.SafeLabel2.Size = new System.Drawing.Size(100, 23);
            this.SafeLabel2.TabIndex = 1;
            this.SafeLabel2.Text = "Antal starter:";
            // 
            // btnCompetition
            // 
            this.btnCompetition.Enabled = false;
            this.btnCompetition.Location = new System.Drawing.Point(8, 8);
            this.btnCompetition.Name = "btnCompetition";
            this.btnCompetition.Size = new System.Drawing.Size(75, 23);
            this.btnCompetition.TabIndex = 2;
            this.btnCompetition.Text = "Tävlingsinfo";
            this.toolTip1.SetToolTip(this.btnCompetition, "Här skriver du in information om tävling");
            this.btnCompetition.Click += new System.EventHandler(this.btnCompetition_Click);
            // 
            // btnStations
            // 
            this.btnStations.Enabled = false;
            this.btnStations.Location = new System.Drawing.Point(88, 8);
            this.btnStations.Name = "btnStations";
            this.btnStations.Size = new System.Drawing.Size(75, 23);
            this.btnStations.TabIndex = 3;
            this.btnStations.Text = "Stationer";
            this.toolTip1.SetToolTip(this.btnStations, "Här talar du om hur många stationer som tävlingen omfattar och dess utseende");
            this.btnStations.Click += new System.EventHandler(this.btnStations_Click);
            // 
            // btnClubs
            // 
            this.btnClubs.Enabled = false;
            this.btnClubs.Location = new System.Drawing.Point(8, 32);
            this.btnClubs.Name = "btnClubs";
            this.btnClubs.Size = new System.Drawing.Size(75, 23);
            this.btnClubs.TabIndex = 4;
            this.btnClubs.Text = "Klubbar";
            this.toolTip1.SetToolTip(this.btnClubs, "Här lägger du till och drar ifrån klubbar som skyttarna sedan kan tillhöra");
            this.btnClubs.Click += new System.EventHandler(this.btnClubs_Click);
            // 
            // btnCompetitors
            // 
            this.btnCompetitors.Enabled = false;
            this.btnCompetitors.Location = new System.Drawing.Point(8, 56);
            this.btnCompetitors.Name = "btnCompetitors";
            this.btnCompetitors.Size = new System.Drawing.Size(75, 23);
            this.btnCompetitors.TabIndex = 6;
            this.btnCompetitors.Text = "Deltagare";
            this.toolTip1.SetToolTip(this.btnCompetitors, "Här fyller du i vilka skyttar som ska vara med i tävlingen");
            this.btnCompetitors.Click += new System.EventHandler(this.btnCompetitors_Click);
            // 
            // btnPatrols
            // 
            this.btnPatrols.Enabled = false;
            this.btnPatrols.Location = new System.Drawing.Point(88, 56);
            this.btnPatrols.Name = "btnPatrols";
            this.btnPatrols.Size = new System.Drawing.Size(75, 23);
            this.btnPatrols.TabIndex = 7;
            this.btnPatrols.Text = "Patruller";
            this.toolTip1.SetToolTip(this.btnPatrols, "Här hanterar du patrulltilldelningen");
            this.btnPatrols.Click += new System.EventHandler(this.btnPatrols_Click);
            // 
            // btnWeapons
            // 
            this.btnWeapons.Enabled = false;
            this.btnWeapons.Location = new System.Drawing.Point(88, 32);
            this.btnWeapons.Name = "btnWeapons";
            this.btnWeapons.Size = new System.Drawing.Size(75, 23);
            this.btnWeapons.TabIndex = 5;
            this.btnWeapons.Text = "Vapen";
            this.toolTip1.SetToolTip(this.btnWeapons, "Här lägger du till och drar ifrån vapen som skyttarna sedan kan använda");
            this.btnWeapons.Click += new System.EventHandler(this.btnWeapons_Click);
            // 
            // btnResults
            // 
            this.btnResults.Enabled = false;
            this.btnResults.Location = new System.Drawing.Point(8, 80);
            this.btnResults.Name = "btnResults";
            this.btnResults.Size = new System.Drawing.Size(155, 23);
            this.btnResults.TabIndex = 8;
            this.btnResults.Text = "Resultatinmatning";
            this.toolTip1.SetToolTip(this.btnResults, "Här matar du in resultaten");
            this.btnResults.Click += new System.EventHandler(this.btnResults_Click);
            // 
            // btnViewResults
            // 
            this.btnViewResults.Enabled = false;
            this.btnViewResults.Location = new System.Drawing.Point(8, 104);
            this.btnViewResults.Name = "btnViewResults";
            this.btnViewResults.Size = new System.Drawing.Size(75, 23);
            this.btnViewResults.TabIndex = 9;
            this.btnViewResults.Text = "Resultat";
            this.toolTip1.SetToolTip(this.btnViewResults, "Här visar du resultaten");
            this.btnViewResults.Click += new System.EventHandler(this.btnViewResults_Click);
            // 
            // btnTeamManagement
            // 
            this.btnTeamManagement.Enabled = false;
            this.btnTeamManagement.Location = new System.Drawing.Point(88, 104);
            this.btnTeamManagement.Name = "btnTeamManagement";
            this.btnTeamManagement.Size = new System.Drawing.Size(75, 23);
            this.btnTeamManagement.TabIndex = 11;
            this.btnTeamManagement.Text = "Lag";
            this.toolTip1.SetToolTip(this.btnTeamManagement, "Här hanterar du lagtävlingen");
            this.btnTeamManagement.Click += new System.EventHandler(this.btnTeamManagement_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SafeLabel2);
            this.groupBox1.Controls.Add(this.SafeLabel1);
            this.groupBox1.Controls.Add(this.lblClubCount);
            this.groupBox1.Controls.Add(this.SafeLabel4);
            this.groupBox1.Controls.Add(this.lblPatrolCount);
            this.groupBox1.Controls.Add(this.lblCompetitorCount);
            this.groupBox1.Controls.Add(this.lblShooterCount);
            this.groupBox1.Controls.Add(this.lblPatrolCountHeader);
            this.groupBox1.Location = new System.Drawing.Point(168, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 120);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tävlingsinfo";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 130);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(328, 23);
            this.progressBar1.TabIndex = 12;
            this.progressBar1.Visible = false;
            // 
            // timerEasterEgg
            // 
            this.timerEasterEgg.Interval = 10000;
            this.timerEasterEgg.Tick += new System.EventHandler(this.timerEasterEgg_Tick);
            // 
            // FMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(358, 157);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnTeamManagement);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnViewResults);
            this.Controls.Add(this.btnResults);
            this.Controls.Add(this.btnWeapons);
            this.Controls.Add(this.btnPatrols);
            this.Controls.Add(this.btnCompetitors);
            this.Controls.Add(this.btnClubs);
            this.Controls.Add(this.btnStations);
            this.Controls.Add(this.btnCompetition);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.Name = "FMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "FMain";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        #region Formhandling
        /*
        private void EnableMainFromOtherThread()
        {
            BeginInvoke(EnableMain);
        }*/
        private event MethodInvoker EnableMain;
        private void enableMain()
        {
            try
            {
                Trace.WriteLine("FMain: Entering enableMain from thread \"" + 
                    Thread.CurrentThread.Name + "\" " + 
                    " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                    DateTime.Now.ToLongTimeString());

                Enabled = true;
                //TopMost = true;
                Focus();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }
        }
        private void setHeader()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(SetHeaderHandler);
                return;
            }

            try
            {
                Trace.WriteLine("FMain: Entering setHeaderDefault()");
                string text = "WinShooter v" + AssemblyVersion + " ";
                try
                {
                    if (CommonCode.GetCompetitions().Length>0)
                        text += "- " + CommonCode.CompetitionCurrent.Name;
                }
                catch(Exception)
                {
                }

                Text = text;
            }
            catch(System.NullReferenceException)
            {
            }
        }
        private void prepareMenus(bool gotDatabase)
        {
            Trace.WriteLine("FMain: Entering prepareMenus()");

            if (gotDatabase)
            {
                menuFileNew.Enabled = false;
                menuFileOpen.Enabled = false;
                menuFileConnect.Enabled = false;
                btnCompetition.Enabled = true;
                //btnStations.Enabled = true;
                btnClubs.Enabled = true;
                btnWeapons.Enabled = true;
                btnTeamManagement.Enabled = true;
                btnStations.Enabled = true;
                CommonCode_UpdatedCompetition();
            }
            else
            {
                menuFileConnect.Enabled = true;
                btnCompetition.Enabled = false;
                btnStations.Enabled = false;
                btnClubs.Enabled = false;
                btnWeapons.Enabled = false;
                btnPatrols.Enabled = false;
                btnTeamManagement.Enabled = false;
                setHeader();
            }
        }

        #endregion


        #region Menus
        #region Menu File
        private string currentFile = "";
        private void menuFileExit_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuFileExit_Click()");
            this.Dispose();
        }

        private void menuFileUpdate_Click(object sender, System.EventArgs e)
        {
            string urlstring = "";
            string versionstring = "";
            string autoupdateurl = "http://www.winshooter.se/Update/updatefiles.txt";
            if (checkForNewerVersionInternet(ref urlstring, ref versionstring, ref autoupdateurl))
            {
                DialogResult res = 
                    MessageBox.Show("Det finns en uppdaterad version till WinShooter, version " + 
                    versionstring + ". Vill du hämta den nu?", 
                    "Uppdatering", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    updateWinShooterNow(autoupdateurl, urlstring);
                }
            }
            else
                MessageBox.Show("Det finns inte någon uppdaterad version till WinShooter.", 
                    "Uppdatering", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
        }
        private void menuFileUpdateBeta_Click(object sender, System.EventArgs e)
        {
            string autoupdateurl = "http://www.winshooter.se/UpdateBeta/updatefiles.txt";
            updateWinShooterNow(autoupdateurl, "http://www.winshooter.se");
        }
        private void menuFileUpdateShooters_Click(object sender, EventArgs e)
        {
            string fileContent = CommonCode.GetCacheFileContent();

            try
            {
                UpdateCache.UpdateCache updateCache = new UpdateCache.UpdateCache();

                updateCache.FromLocalFile(fileContent);

                MessageBox.Show("Tack för att du skickar uppdateringar på skyttar " +
                    "så att de kan uppdateras i nästa version av WinShooter.",
                    "Tack för hjälpen!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception exc)
            {
                Trace.WriteLine("Exception occured in menuFileUpdateShooters_Click(): " +
                    exc.ToString());
                MessageBox.Show("Kunde inte köra uppdatering av skyttar. Försök igen senare.",
                    "Ett fel har uppstått",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void menuFileNew_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuFileNew_Click()");

            try
            {
                saveFileDialog1.Title = "Välj nytt filnamn";
                saveFileDialog1.Filter = "*.wsk (WinShooter)|*.wsk";
                saveFileDialog1.InitialDirectory = 
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                DialogResult res = saveFileDialog1.ShowDialog();
                if (res == DialogResult.Abort | res == DialogResult.Cancel |
                    saveFileDialog1.FileName == "")
                    return;

                try
                {
                    Cursor = Cursors.WaitCursor;
                    currentFile = saveFileDialog1.FileName;
                    CommonCode.CreateAccessDatabase(saveFileDialog1.FileName);
                    CommonCode.CreateDefaultDatabaseContent();
                }
                catch(Exception exc)
                {
                    MessageBox.Show("Ett fel uppstod:\n" + exc.ToString(), 
                        "Felmeddelande",
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Stop);

                    Trace.WriteLine("Fmain: " + exc.ToString());
                }

                // We got a file
                prepareMenus(true);
                Cursor = Cursors.Default;

                // Update all
                CommonCode_All();

                // Start wizard to create competition
                FCompetitionWizard wizard = new FCompetitionWizard(ref CommonCode);
                wizard.EnableMain += new MethodInvoker(enableMain);
                wizard.CancelMain += new MethodInvoker(cancelWizard);
                wizard.Visible = true;
                wizard.Focus();

                Enabled = false;
            }
            catch(Exception exc)
            {
                Trace.WriteLine("An exception occured: " + exc.ToString());
#if DEBUG
                MessageBox.Show("Ett allvarligt fel har uppstått: \"" + exc.ToString() + "\"",
                    "Major Failure", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Stop);
#else
                MessageBox.Show("Ett allvarligt fel har uppstått: \"" + exc.Message + "\"",
                    "Major Failure", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Stop);
#endif
                Dispose(true);
            }
        }
        private void cancelWizard()
        {
            Enabled = true;
            prepareMenus(false);
            Dispose();
        }
        private void menuFileOpen_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuFileOpen_Click()");

            string startDir = Allberg.Shooter.Common.LocalSettings.DataPath;

            if (startDir.Trim().Length == 0)
                startDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            openFileDialog1.Title = "Öppna befintlig fil";
            openFileDialog1.InitialDirectory = startDir;
#if DEBUG
            openFileDialog1.Filter = "*.wsk (Allberg WinShooter)|*.wsk|*.mdb (Microsoft Access|*.mdb";
#else
            openFileDialog1.Filter = "*.wsk (Allberg WinShooter)|*.wsk";
#endif
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Abort | res == DialogResult.Cancel |
                openFileDialog1.FileName == "")
                return;

            Allberg.Shooter.Common.LocalSettings.DataPath = 
                Path.GetDirectoryName(openFileDialog1.FileName);

            FileOpen(openFileDialog1.FileName);
        }

        private void FileOpen(string filename)
        {
            currentFile = filename;
            Cursor = Cursors.WaitCursor;
            try
            {
                CommonCode.OpenAccessDatabase(filename);
                CommonCode.OpenDatabase();
            }
                catch(System.Data.OleDb.OleDbException exc)
            {
                Trace.WriteLine("FMain: " + exc.ToString());
                MessageBox.Show("Ett fel uppstod vid öppnande av filen, " +
                    "troligen beroende på att filen redan är öppen i t.ex. " +
                    "WinShooter eller Microsoft Access.",
                    "Kan inte öppna filen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FMain: " + exc.ToString());
                MessageBox.Show("Ett fel uppstod:\n" + exc.ToString(), 
                    "Felmeddelande",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Stop);
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            // We got a file
            prepareMenus(true);

            if (CommonCode.GetCompetitions().Length == 0)
            {
                Trace.WriteLine("Not even one competition exists in newly opened file. Exiting.");

                MessageBox.Show("I denna fil finns inte ens en enda tävling " + 
                    "definierad. Den kan därmed inte öppnas.",
                    "Felmeddelande",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                Application.Exit();
            }

            // Update all
            //CommonCode_All();
        }

        private void menuFileConnect_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuFileConnect_Click()");
            myServerConnectWindow.EnableMe();
        }
        System.Timers.Timer timerSyncThread;
        private void ConnectToServer(string hostname)
        {			
            Trace.WriteLine("FMain: Entering ConnectToServer(\"" + hostname + "\")");
            Cursor = Cursors.WaitCursor;
            try
            {
                CommonCode.ServerConnect(hostname);
                Trace.WriteLine("FMain: Entering ServerConnect:Opendatabase()");

                // We got a server
                CommonCode_All();
                prepareMenus(true);
                menuImportText.Enabled = false;

                timerSyncThread = new System.Timers.Timer(30 * 1000);
                timerSyncThread.Enabled = true;
                timerSyncThread.Elapsed += new System.Timers.ElapsedEventHandler(timerSync_Tick);
                timerSyncThread.Start();
            }
            catch (System.Net.WebException exc)
            {
                MessageBox.Show("Kunde inte ansluta till servern.\r\n" +
                    "Kontrollera att du angivit rätt servernamn eller " +
                    "serveradress och att WinShooterservern är " +
                    "startad.\r\n" +
                    "\r\nOm du har Windows XP med ServicePack 2 " +
                    "installerat på servern, läs i " +
                    "\"läs mig\" som finns under startmenyn.",
                    "Felmeddelande",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                Trace.WriteLine("FMain while connecting to server: " + exc.ToString());
            }
            catch (System.Reflection.TargetInvocationException exc)
            {
                MessageBox.Show("Ett fel uppstod vid anslutning till servern. " +
                    "Verifiera att det är rätt version på servern och klienten.",
                    "Felmeddelande",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                Trace.WriteLine("FMain while connecting to server: " + exc.ToString());
            }
            catch (ApplicationException exc)
            {
                MessageBox.Show("Ett fel uppstod vid anslutning till servern:\r\n" + exc.Message,
                    "Felmeddelande",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                Trace.WriteLine("FMain while connecting to server: " + exc.ToString());
            }
            catch (System.Runtime.Serialization.SerializationException exc)
            {
                if (exc.Message.IndexOf("Cannot find the assembly") > -1)
                {
                    MessageBox.Show("Versionen på server och på klient matchar inte.\r\n" +
                        "Kontroller att det är samma version.",
                        "Felmeddelande",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Ett fel uppstod vid anslutning till servern:\r\n" + exc.Message,
                        "Felmeddelande",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    Trace.WriteLine("FMain: " + exc.ToString());
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ett fel uppstod (ConnectToServer):\n" + exc.ToString(),
                    "Felmeddelande",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                Trace.WriteLine("FMain: " + exc.ToString());
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
                Trace.WriteLine("FMain: Exiting ServerConnect()");
            }
        }

        #endregion


#if VisualStudio
        #region Menu Print
        #region Menu Print->Patrol
        private void menuPrintPatrollistByPatrol_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuPrintPatrollistByPatrol_Click()");

            FPrintSelection printSelection = new FPrintSelection();
            printSelection.chkPrintAll.Checked = false;
            switch(CommonCode.GetCompetitions()[0].Type)
            {
                case Structs.CompetitionTypeEnum.Field:
                    printSelection.chkPrintAll.Text = "Skriv ut samtliga patruller";
                    printSelection.lblSelection.Text = "Patrull";
                    break;
                case Structs.CompetitionTypeEnum.Precision:
                    printSelection.chkPrintAll.Text = "Skriv ut samtliga lag";
                    printSelection.lblSelection.Text = "Lag";
                    break;
            }

            // Add rows to dropdown
            Structs.Patrol[] patrols = 
                CommonCode.GetPatrols();

            foreach(Structs.Patrol patrol in patrols)
            {
                System.Data.DataRow row =
                    printSelection.TablePrintSelection.NewRow();
                row["Id"] = patrol.PatrolId;
                row["Name"] = patrol.PatrolId.ToString() + " - " + 
                    patrol.StartDateTimeDisplay.ToShortTimeString() +
                    " - " + patrol.PClass.ToString();
                printSelection.TablePrintSelection.Rows.Add(row);
            }

            // make sure there is a call back
            printSelection.EnablePrint += 
                new FPrintSelection.EnablePrintHandler(menuPrintPatrollistByPatrol_Print);
            printSelection.EnableMain += 
                new FPrintSelection.EnableMainHandler(enableMain);
            printSelection.Visible = true;
            printSelection.Focus();
        }

        private void menuPrintPatrollistByPatrol_Print(string PatrolId, FPrintSelection printSelection)
        {
            Trace.WriteLine("FMain: Entering menuPrintPatrollistByPatrol_Print()");

            try 
            {
                CPrintPatrollistByPatrol pd = new CPrintPatrollistByPatrol(ref CommonCode);
                pd.MaxLanes = CommonCode.GetCompetitions()[0].PatrolSize;
                pd.PatrolIdToPrint = int.Parse(PatrolId);

                PrintDialog dlg = new PrintDialog() ;
                dlg.Document = pd;
                DialogResult result = dlg.ShowDialog();

                if (result == DialogResult.OK) 
                {
                    pd.Print();
                }

            } 
            catch(Exception exc)
            {
                MessageBox.Show("Fel uppstod vid utskrift:\n" + exc.ToString());
                Trace.WriteLine("FMain: " + exc.ToString());
            }
        }
        private void menuPrintPatrollistByClub_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuPrintPatrollistByClub_Click()");

            FPrintSelection printSelection = new FPrintSelection();
            printSelection.chkPrintAll.Text = "Skriv ut för samtliga klubbar";
            printSelection.chkPrintAll.Visible = false;
            printSelection.chkPrintAll.Checked = false;
            printSelection.DdSelection.Enabled = true;

            // Add rows to dropdown
            Structs.Club[] clubs = 
                CommonCode.GetClubs();

            foreach(Structs.Club club in clubs)
            {
                if (CommonCode.GetShooters(club).Length>0)
                {
                    System.Data.DataRow row =
                        printSelection.TablePrintSelection.NewRow();
                    row["Id"] = club.ClubId;
                    row["Name"] = club.Name;
                    printSelection.TablePrintSelection.Rows.Add(row);
                }
            }

            // make sure there is a call back
            printSelection.EnablePrint += 
                new FPrintSelection.EnablePrintHandler(menuPrintPatrollistByClub_Print);
            printSelection.EnableMain += 
                new FPrintSelection.EnableMainHandler(enableMain);
            printSelection.Visible = true;
            printSelection.Focus();
        }

        private void menuPrintPatrollistByClub_Print(string ClubId, FPrintSelection printSelection)
        {
            Trace.WriteLine("FMain: Entering menuPrintPatrollistByClub_Print()");

            try 
            {
                CPrintPatrollistByClub pd = new CPrintPatrollistByClub(ref CommonCode);
                pd.ClubIdToPrint = ClubId;

                PrintDialog dlg = new PrintDialog() ;
                dlg.Document = pd;
                dlg.Document.DefaultPageSettings.Landscape = false;
                DialogResult result = dlg.ShowDialog();

                if (result == DialogResult.OK) 
                {
                    pd.Print();
                }

            } 
            catch(Exception exc)
            {
                MessageBox.Show("Fel uppstod vid utskrift:\n" + exc.ToString());
                Trace.WriteLine("FMain: " + exc.ToString());
            }
        }

        private void menuPrintPatrollistByShooter_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuPrintPatrollistByShooter_Click()");

            FPrintSelection printSelection = new FPrintSelection();
            printSelection.chkPrintAll.Text = "Skriv ut samtliga skyttar";

            // Add rows to dropdown
            Structs.Shooter[] shooters =
                CommonCode.GetShooters();

            foreach(Structs.Shooter shooter in shooters)
            {
                System.Data.DataRow row =
                    printSelection.TablePrintSelection.NewRow();
                row["Id"] = shooter.ShooterId;
                row["Name"] = shooter.Givenname + ", " + shooter.Surname;
                printSelection.TablePrintSelection.Rows.Add(row);
            }

            // make sure there is a call back
            printSelection.EnablePrint += 
                new FPrintSelection.EnablePrintHandler(menuPrintPatrollistByShooter_Print);
            printSelection.EnableMain += 
                new FPrintSelection.EnableMainHandler(enableMain);
            printSelection.Visible = true;
            printSelection.Focus();
        }

        private void menuPrintPatrollistByShooter_Print(string ShooterId, FPrintSelection printSelection)
        {
            Trace.WriteLine("FMain: Entering menuPrintPatrollistByShooter_Print()");

            try 
            {
                CPrintPatrollistByUser pd = new CPrintPatrollistByUser(ref CommonCode);
                pd.ShooterIdToPrint = int.Parse(ShooterId);

                PrintDialog dlg = new PrintDialog() ;
                dlg.Document = pd;
                DialogResult result = dlg.ShowDialog();

                if (result == DialogResult.OK) 
                {
                    pd.Print();
                }

            } 
            catch(Exception exc)
            {
                MessageBox.Show("Fel uppstod vid utskrift:\n" + exc.ToString());
                Trace.WriteLine("FMain: " + exc.ToString());
            }
        }

#endregion

        private bool resultsExist(Structs.ResultWeaponsClass wclass)
        {
            for(int i=1;i<=Structs.ShootersClassMax;i++)
            {
                Structs.ShootersClass uclass =
                    (Structs.ShootersClass)i;

                try
                {
                    int.Parse(uclass.ToString());
                }
                catch(System.FormatException)
                {
                    // Ok, found a userclass
                    if (CommonCode.GetCompetitorResultsCount(wclass, uclass)>0)
                        return true;
                }
            }
            return false;
        }
        private bool resultsExist(Structs.ResultWeaponsClass wclass, string clubId)
        {
            for(int i=1;i<=Structs.ShootersClassMax;i++)
            {
                Structs.ShootersClass uclass =
                    (Structs.ShootersClass)i;

                try
                {
                    int.Parse(uclass.ToString());
                }
                catch(System.FormatException)
                {
                    // Ok, found a userclass
                    if (CommonCode.GetCompetitorResultsCount(wclass, uclass, clubId)>0)
                        return true;
                }
            }
            return false;
        }
        private void menuPrintResultAll_Click(object sender, System.EventArgs e)
        {
            menuPrintResultAll_Click(false);
        }
        private void menuPrintPrelResultAll_Click(object sender, System.EventArgs e)
        {
            menuPrintResultAll_Click(true);
        }
        private void menuPrintResultAll_Click(bool prelResults)
        {
            Trace.WriteLine("FMain: Entering menuPrintResultAll_Click()");

            List<CPrintResultlist> list = new List<CPrintResultlist>();
            DateTime start = DateTime.Now;
            Trace.WriteLine("FMain.menuPrintResultAll_Click Starting checking what classes have results");
            Structs.ResultWeaponsClass[] weaponClasses = CommonCode.GetResultWeaponClassesWithResults();

            foreach (Structs.ResultWeaponsClass wclass in weaponClasses)
            {
                CPrintResultlist pd = new CPrintResultlist(ref CommonCode,
                    wclass,
                    Structs.ShootersClass.Okänd,
                    prelResults,
                    null);
                pd.PrinterSettings.DefaultPageSettings.Landscape =
                    CommonCode.Settings.PrinterSettings.PaperResultDocument.Landscape;
                list.Add(pd);
            }
            
            Trace.WriteLine("FMain.menuPrintResultAll_Click: Checking results took " +
                Common.Formatter.FormatTimeSpan(DateTime.Now - start) + "sec.");

            if (list.Count == 0)
                MessageBox.Show("Det finns inte några resultat inmatade.");

            PrintDialog dlg = new PrintDialog() ;
            dlg.Document = (CPrintResultlist)list[0];
            DialogResult result = dlg.ShowDialog();

            if (result != DialogResult.OK) 
            {
                return;
            }

            Trace.WriteLine("FMain.menuPrintResultAll_Click: Printing documents.");
            System.Threading.Thread printingThread = new Thread(
                new ParameterizedThreadStart(this.printDocuments));
            printingThread.IsBackground = true;
            printingThread.Start((object[])list.ToArray());

            Trace.WriteLine("FMain.menuPrintResultAll_Click: Printing documents took " +
                Common.Formatter.FormatTimeSpan(DateTime.Now - start) + "sec.");
        }
        private void printDocuments(object obj)
        {
            System.Drawing.Printing.PrintDocument[] printObjs = 
                (System.Drawing.Printing.PrintDocument[])obj;

            foreach (System.Drawing.Printing.PrintDocument printObj in
                printObjs)
            {
                printObj.PrinterSettings = printObjs[0].PrinterSettings;
                printObj.Print();
            }
        }
        private void menuPrintPrelResultAllPreview_Click(object sender, System.EventArgs e)
        {
            menuPrintPrelResultAllPreview_Click(true);
        }
        private void menuPrintResultAllPreview_Click(object sender, System.EventArgs e)
        {
            menuPrintPrelResultAllPreview_Click(false);
        }
        private void menuPrintPrelResultAllPreview_Click(bool prelResults)
        {
            Trace.WriteLine("FMain: Entering menuPrintPrelResultAllPreview_Click()");

            Structs.ResultWeaponsClass[] rwclasses =
                CommonCode.GetResultWeaponClassesWithResults();

            bool printed = false;
            foreach (Structs.ResultWeaponsClass rws in rwclasses)
            {
                printPreviewResults(rws, prelResults);
                printed = true;
            }

            if (printed == false)
                MessageBox.Show("Det finns inte några resultat inmatade.");
        }

        private void menuPrintResultA_Click(object sender, System.EventArgs e)
        {
            menuPrintPrelResultA_Click(false);
        }
        private void menuPrintPrelResultA_Click(object sender, System.EventArgs e)
        {
            menuPrintPrelResultA_Click(true);
        }
        private void menuPrintPrelResultA_Click(bool prelResults)
        {
            Trace.WriteLine("FMain: Entering menuPrintResultA_Click()");
            if (!resultsExist(Structs.ResultWeaponsClass.A))
                MessageBox.Show("Det finns inte några resultat inmatade för klass A");
            else
                printResults(Structs.ResultWeaponsClass.A, prelResults);
        }
        private void menuPrintResultB_Click(object sender, System.EventArgs e)
        {
            menuPrintResultB_Click(false);
        }
        private void menuPrintPrelResultB_Click(object sender, System.EventArgs e)
        {
            menuPrintResultB_Click(true);		
        }
        private void menuPrintResultB_Click(bool prelResult)
        {
            Trace.WriteLine("FMain: Entering menuPrintResultB_Click()");

            if (!resultsExist(Structs.ResultWeaponsClass.B))
                MessageBox.Show("Det finns inte några resultat inmatade för klass B");
            else
                printResults(Structs.ResultWeaponsClass.B, prelResult);
        }
        private void menuPrintResultC_Click(object sender, System.EventArgs e)
        {
            menuPrintResultC_Click(false);
        }
        private void menuPrintPrelResultC_Click(object sender, System.EventArgs e)
        {
            menuPrintResultC_Click(true);
        }
        private void menuPrintResultC_Click(bool prelResult)
        {
            Trace.WriteLine("FMain: Entering menuPrintResultC_Click()");

            if (!resultsExist(Structs.ResultWeaponsClass.C))
                MessageBox.Show("Det finns inte några resultat inmatade för klass C");
            else
                printResults(Structs.ResultWeaponsClass.C, prelResult);
        }
        private void menuPrintResultR_Click(object sender, System.EventArgs e)
        {
            menuPrintResultR_Click(false);
        }
        private void menuPrintPrelResultR_Click(object sender, System.EventArgs e)
        {
            menuPrintResultR_Click(true);
        }
        private void menuPrintResultR_Click(bool prelResult)
        {
            Trace.WriteLine("FMain: Entering menuPrintResultR_Click()");

            if (!resultsExist(Structs.ResultWeaponsClass.R))
                MessageBox.Show("Det finns inte några resultat inmatade för klass R");
            else
                printResults(Structs.ResultWeaponsClass.R, prelResult);
        }
        private void menuPrintResultM_Click(object sender, System.EventArgs e)
        {
            menuPrintResultM_Click(false);
        }
        private void menuPrintPrelResultM_Click(object sender, System.EventArgs e)
        {
            menuPrintResultM_Click(true);
        }
        private void menuPrintResultM_Click(bool prelResult)
        {
            Trace.WriteLine("FMain: Entering menuPrintResultM_Click()");

            if (!resultsExist(Structs.ResultWeaponsClass.M))
                MessageBox.Show("Det finns inte några resultat inmatade för klass M");
            else
                printResults(Structs.ResultWeaponsClass.M, prelResult);
        }
        private void printResults(Structs.ResultWeaponsClass wclass, bool prelResults)
        {
            printResults(wclass, prelResults, null);
        }
        private void printResults(Structs.ResultWeaponsClass wclass, bool prelResults, string clubId)
        {
            CPrintResultlist pd = new CPrintResultlist(ref CommonCode, 
                wclass, 
                Structs.ShootersClass.Okänd,
                prelResults,
                clubId);

            PrintDialog dlg = new PrintDialog() ;
            dlg.Document = pd;
            DialogResult result = dlg.ShowDialog();

            pd.DefaultPageSettings.Landscape =
                CommonCode.Settings.PrinterSettings.PaperResultDocument.Landscape;
            dlg.PrinterSettings.DefaultPageSettings.Landscape =
                CommonCode.Settings.PrinterSettings.PaperResultDocument.Landscape;
            
            if (result == DialogResult.OK) 
            {
                List<CPrintResultlist> list = new List<CPrintResultlist>();
                list.Add(pd);
                Trace.WriteLine("FMain.printResults: Printing document.");
                System.Threading.Thread printingThread = new Thread(
                    new ParameterizedThreadStart(this.printDocuments));
                printingThread.IsBackground = true;
                printingThread.Start((object[])list.ToArray());
            }
        }
        private void printResults()
        {
            CPrintResultlist pd = new CPrintResultlist(ref CommonCode, 
                Structs.ResultWeaponsClass.Unknown, 
                Structs.ShootersClass.Okänd,
                false,
                null);

            PrintDialog dlg = new PrintDialog() ;
            dlg.Document = pd;
            dlg.Document.DefaultPageSettings.Landscape = false;
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK) 
            {
                pd.Print();
            }
        }
        private void printPreviewResults(Structs.ResultWeaponsClass wclass, bool prelResults)
        {
            CPrintResultlist pd = new CPrintResultlist(ref CommonCode, 
                wclass, 
                Structs.ShootersClass.Okänd,
                prelResults,
                null);

            PrintPreviewDialog dlg = new PrintPreviewDialog() ;
            dlg.Document = pd;
            dlg.Document.DefaultPageSettings.Landscape = false;
            DialogResult result = dlg.ShowDialog();
        }

        
        private void menuPrintResultClub_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuPrintResultClub_Click()");

            FPrintSelection printSelection = new FPrintSelection();
            printSelection.chkPrintAll.Text = "Skriv ut samtliga patruller";
            printSelection.chkPrintAll.Checked = false;
            printSelection.lblSelection.Text = "Patrull";

            // Add rows to dropdown
            Structs.Club[] clubs = 
                CommonCode.GetClubs();

            foreach(Structs.Club club in clubs)
            {
                if (CommonCode.GetShooters(club).Length>0)
                {
                    System.Data.DataRow row =
                        printSelection.TablePrintSelection.NewRow();
                    row["Id"] = club.ClubId;
                    row["Name"] = club.Name;
                    printSelection.TablePrintSelection.Rows.Add(row);
                }
            }

            // make sure there is a call back
            printSelection.EnablePrint += 
                new FPrintSelection.EnablePrintHandler(menuPrintResultClub_Print);
            printSelection.EnableMain += 
                new FPrintSelection.EnableMainHandler(enableMain);
            printSelection.lblSelection.Visible = false;
            printSelection.chkPrintAll.Checked = false;
            printSelection.chkPrintAll.Visible = false;
            printSelection.Visible = true;
            printSelection.Focus();

            Trace.WriteLine("FMain.menuPrintResultClub_Click ending");
        }

        private void menuPrintResultClub_Print(string clubId, FPrintSelection printSelection)
        {
            Trace.WriteLine("FMain: Entering menuPrintResultClub_Print()");

            try 
            {
                bool printed = false;
                for(int i=0;i<=Structs.WeaponClassMax;i++)
                {
                    Structs.ResultWeaponsClass wclass = (Structs.ResultWeaponsClass)i;
                    try
                    {
                        int.Parse(wclass.ToString());
                    }
                    catch(System.FormatException)
                    {
                        if (resultsExist(wclass, clubId))
                        {
                            printResults(wclass, false, clubId);
                            printed = true;
                        }
                    }
                }
                if (printed == false)
                    MessageBox.Show("Det finns inte några resultat inmatade.");
            } 
            catch(Exception exc)
            {
                MessageBox.Show("Fel uppstod vid utskrift:\n" + exc.ToString());
                Trace.WriteLine("FMain: " + exc.ToString());
            }
        }

        private void menuPrintPrelResultPatrol_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuPrintPrelResultPatrol_Click()");

            FPrintSelection printSelection = new FPrintSelection();
            printSelection.chkPrintAll.Text = "";
            printSelection.chkPrintAll.Checked = false;
            printSelection.chkPrintAll.Visible = false;
            printSelection.lblSelection.Text = "Patrull";

            // Add rows to dropdown
            Structs.Patrol[] patrols = 
                CommonCode.GetPatrols();

            foreach(Structs.Patrol patrol in patrols)
            {
                System.Data.DataRow row =
                    printSelection.TablePrintSelection.NewRow();
                row["Id"] = patrol.PatrolId.ToString();
                row["Name"] = patrol.PatrolId.ToString() + " - " + patrol.StartDateTimeDisplay.ToShortTimeString();
                printSelection.TablePrintSelection.Rows.Add(row);
            }

            // make sure there is a call back
            printSelection.EnablePrint += 
                new FPrintSelection.EnablePrintHandler(menuPrintPrelResultPatrol_Print);
            printSelection.EnableMain += 
                new FPrintSelection.EnableMainHandler(enableMain);
            printSelection.lblSelection.Visible = true;
            printSelection.chkPrintAll.Checked = false;
            printSelection.chkPrintAll.Visible = false;
            printSelection.Visible = true;
            printSelection.Focus();

            Trace.WriteLine("FMain.menuPrintPrelResultPatrol_Click ending");
        }

        private void menuPrintPrelResultPatrol_Print(string patrolidstring, 
            FPrintSelection printSelection)
        {
            Structs.Patrol patrol = CommonCode.GetPatrol(int.Parse(patrolidstring));
            CPrintResultlistByPatrol print = new CPrintResultlistByPatrol(ref CommonCode, patrol);

            PrintDialog dlg = new PrintDialog() ;
            dlg.Document = print;
            dlg.Document.DefaultPageSettings.Landscape = true;
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK) 
            {
                print.Color = dlg.PrinterSettings.SupportsColor;
                print.Print();
            }
        }

        private void menuPrintFollowupGroupByClub_Click(object sender, EventArgs e)
        {
            CPrintFollowup print = new CPrintFollowup(ref CommonCode);

            PrintDialog dlg = new PrintDialog();
            dlg.Document = print;
            dlg.Document.DefaultPageSettings.Landscape = false;
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                print.Color = dlg.PrinterSettings.SupportsColor;
                print.Print();
            }
        }

        private void menuPrintPrinterTest_Click(object sender, EventArgs e)
        {
            CPrintPrinterTest print = new CPrintPrinterTest(ref CommonCode);

            PrintDialog dlg = new PrintDialog();
            dlg.Document = print;
            dlg.Document.DefaultPageSettings.Landscape = false;
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                print.Print();
            }
        }

        #endregion

        #region Menu Labels
        private void menuPrintLabelsMirrors_Click(object sender, System.EventArgs e)
        {
            CPrintMirrorLabels print = new CPrintMirrorLabels (ref CommonCode);

            PrintDialog dlg = new PrintDialog() ;
            dlg.Document = print;
            dlg.Document.DefaultPageSettings.Landscape = false;
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK) 
            {
                print.Print();
            }
        }
        private void menuPrintLabelsResultPatrol_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuPrintLabelsResultPatrol_Click()");

            FPrintSelection printSelection = new FPrintSelection();
            printSelection.chkPrintAll.Text = "";
            printSelection.chkPrintAll.Checked = false;
            printSelection.chkPrintAll.Visible = false;
            printSelection.lblSelection.Text = "Patrull";

            // Add rows to dropdown
            Structs.Patrol[] patrols =
                CommonCode.GetPatrols();

            foreach (Structs.Patrol patrol in patrols)
            {
                System.Data.DataRow row =
                    printSelection.TablePrintSelection.NewRow();
                row["Id"] = patrol.PatrolId.ToString();
                row["Name"] = patrol.PatrolId.ToString() + " - " + patrol.StartDateTimeDisplay.ToShortTimeString();
                printSelection.TablePrintSelection.Rows.Add(row);
            }

            // make sure there is a call back
            printSelection.EnablePrint +=
                new FPrintSelection.EnablePrintHandler(menuPrintLabelsResultPatrol_Print);
            printSelection.EnableMain +=
                new FPrintSelection.EnableMainHandler(enableMain);
            printSelection.lblSelection.Visible = true;
            printSelection.chkPrintAll.Checked = false;
            printSelection.chkPrintAll.Visible = false;
            printSelection.numericUpDown1.Visible = true;
            printSelection.lblNumericUpDown1.Visible = true;
            printSelection.lblNumericUpDown1.Text = "Hoppa över etiketter";
            printSelection.Visible = true;
            printSelection.Focus();

            Trace.WriteLine("FMain.menuPrintPrelResultPatrol_Click ending");
        }

        private void menuPrintLabelsResultPatrol_Print(string patrolidstring, 
            FPrintSelection printSelection)
        {
            Structs.Patrol patrol = CommonCode.GetPatrol(int.Parse(patrolidstring));
            CPrintResultLabels print = new CPrintResultLabels(
                ref CommonCode, 
                patrol, 
                (int)printSelection.numericUpDown1.Value);

            PrintDialog dlg = new PrintDialog();
            dlg.Document = print;
            dlg.Document.DefaultPageSettings.Landscape = false;
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                print.Print();
            }
        }
        #endregion
#endif
        #region Import
        private void menuImportText_Click(object sender, System.EventArgs e)
        {
            myImportWindow.enableMe();
        }
        #endregion

        #region Export
        #region Menu Internet
        private void menuInternetPatrollist_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuInternetPatrollist_Click()");
            exportPatrolsToInternet(false);
            Trace.WriteLine("FMain: menuInternetPatrollist_Click() ended.");
        }

        private void exportPatrolsToInternet(bool byClub)
        {
            Trace.WriteLine("FMain: Entering exportPatrolsToInternet()");

            saveFileDialogHtml.AddExtension = true;
            saveFileDialogHtml.DefaultExt = "html";
            saveFileDialogHtml.Filter = "Internet files *.html | *.html";
            saveFileDialogHtml.OverwritePrompt = true;
            saveFileDialogHtml.RestoreDirectory = true;
            saveFileDialogHtml.Title = "Exportera patruller till Internetformat";

            if (saveFileDialogHtml.ShowDialog() == DialogResult.Cancel)
                return;
            string html;
            if (!byClub)
                html = CommonCode.InternetHtmlExportPatrols();
            else
                html = CommonCode.InternetHtmlExportPatrolsByClub();

            printToFile(html, saveFileDialogHtml.FileName);
            try
            {
                System.Diagnostics.Process.Start(saveFileDialogHtml.FileName);
            }
            catch(Exception exc)
            {
                Trace.WriteLine("Exception occured in exportPatrolsToInternet(): " +
                    exc.ToString());
                MessageBox.Show("Kunde inte starta Internet Explorer till " +
                    "\"" + saveFileDialogHtml.FileName + "\".", 
                    "Ett fel har uppstått", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void menuInternetPatrollistByClub_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuInternetPatrollist_Click()");
            exportPatrolsToInternet(true);
            Trace.WriteLine("FMain: menuInternetPatrollist_Click() ended.");
        }


        private void menuInternetResults_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuInternetResults_Click()");
            exportResultsToInternet();
        }

        private void exportResultsToInternet()
        {
            Trace.WriteLine("FMain: Entering exportResultsToInternet()");

            saveFileDialogHtml.AddExtension = true;
            saveFileDialogHtml.DefaultExt = "html";
            saveFileDialogHtml.Filter = "Internet files *.html | *.html";
            saveFileDialogHtml.OverwritePrompt = true;
            saveFileDialogHtml.RestoreDirectory = true;
            saveFileDialogHtml.Title = "Exportera resultat till Internetformat";

            if (saveFileDialogHtml.ShowDialog() == DialogResult.Cancel)
                return;

            bool finalResults = false;
            if (CommonCode.GetCompetitions()[0].UsePriceMoney)
            {
                DialogResult res = MessageBox.Show("Är tävlingen avslutad?", 
                    "Slutresultat", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question, 
                    MessageBoxDefaultButton.Button2);
                if (res == DialogResult.Yes)
                    finalResults = true;
            }

            string html = CommonCode.InternetHtmlExportResults(finalResults);
            printToFile(html, saveFileDialogHtml.FileName);
            try
            {
                System.Diagnostics.Process.Start(saveFileDialogHtml.FileName);
            }
            catch(Exception exc)
            {
                Trace.WriteLine("Exception occured in exportResultsToInternet(): " +
                    exc.ToString());
                MessageBox.Show("Kunde inte starta Internet Explorer till " +
                    "\"http://www.allberg.se/WinShooter\".", 
                    "Ett fel har uppstått", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void printToFile(string html, string fileName)
        {
            try
            {
                if (System.IO.File.Exists(fileName))
                    System.IO.File.Delete(fileName);

                // make a new FileStream object, ready for read and write steps. 
                System.IO.FileStream fs = new 
                    System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate, 
                    System.IO.FileAccess.ReadWrite);

                System.IO.StreamWriter w = new System.IO.StreamWriter(fs, 
                    System.Text.Encoding.Default); // create a stream writer 
                // set the file pointer to the end of file
                w.BaseStream.Seek(0, System.IO.SeekOrigin.End); 

                w.Write(html.Replace(">", ">\r\n")); 
                w.Flush();
                w.Close();
                fs.Close();
            }
            catch(Exception)
            {
                MessageBox.Show("Ops");
                printToFile(html, fileName);
            }
        }

        #endregion

        #region Menu PDF
        private void menuPdfPatrollist_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuPdfPatrollist_Click()");
            exportPatrolsToPdf(false);
            Trace.WriteLine("FMain: menuPdfPatrollist_Click() ended.");
        }

        private void exportPatrolsToPdf(bool byClub)
        {
            Trace.WriteLine("FMain: Entering exportPatrolsToPdf()");

            saveFileDialogHtml.InitialDirectory = 
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialogHtml.AddExtension = true;
            saveFileDialogHtml.DefaultExt = "pdf";
            saveFileDialogHtml.Filter = "Adobe Acrobat Document *.pdf | *.pdf";
            saveFileDialogHtml.OverwritePrompt = true;
            saveFileDialogHtml.RestoreDirectory = true;
            saveFileDialogHtml.Title = "Exportera patruller till PDF";

            if (saveFileDialogHtml.ShowDialog() == DialogResult.Cancel)
                return;

            Thread exportThread = new Thread(
                new ParameterizedThreadStart(exportPatrolsToPdfThread));
            exportThread.Name = "PdfExport";
            exportThread.IsBackground = true;
            exportThread.Start(byClub);
        }
        private void exportPatrolsToPdfThread(object byClub)
        {
            exportPatrolsToPdfThread((bool)byClub);
        }
        private void exportPatrolsToPdfThread(bool byClub)
        {
            byte[] pdf;
            if (!byClub)
                pdf = CommonCode.InternetPdfExportPatrols();
            else
                pdf = CommonCode.InternetPdfExportPatrolsByClub();

            printToFile(pdf, saveFileDialogHtml.FileName);
            try
            {
                if (!CheckPDFInstalled())
                {
                    DialogResult res = 
                        MessageBox.Show("Adobe Acrobat Reader är inte " + 
                        "installerat. " +
                        "Det gör att du inte kan läsa manualen och inte " +
                        "kan titta på exporterade data.\r\n\r\n" + 
                        "Vill du öppna http://www.adobe.se för att ladda " +
                        "ner och installera Adobe Acrobat Reader?", 
                        "Adobe Acrobat Reader", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("IExplore.exe", 
                            "http://www.adobe.se");
                    }
                }
                else
                    System.Diagnostics.Process.Start(saveFileDialogHtml.FileName);
            }
            catch(Exception exc)
            {
                Trace.WriteLine("Exception occured in exportPatrolsToInternet(): " +
                    exc.ToString());
                MessageBox.Show("Kunde inte starta Internet Explorer till " +
                    "\"" + saveFileDialogHtml.FileName + "\".", 
                    "Ett fel har uppstått", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
        private void menuPdfPatrollistByClub_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuInternetPatrollist_Click()");
            exportPatrolsToPdf(true);
            Trace.WriteLine("FMain: menuInternetPatrollist_Click() ended.");
        }
        private void menuPdfResults_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuInternetResults_Click()");
            exportResultsToPdf();
            Trace.WriteLine("FMain: menuInternetResults_Click ended.");
        }

        private void exportResultsToPdf()
        {
            Trace.WriteLine("FMain: Entering exportResultsToPdf()");

            if (CommonCode.GetCompetitorResultsCount() == 0)
            {
                Trace.WriteLine("FMain: exportResultsToPdf() failed since there is no results.");
                MessageBox.Show("Det måste finnas inmatade resultat för att kunna exportera till PDF.");
                return;
            }

            saveFileDialogHtml.InitialDirectory = 
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialogHtml.AddExtension = true;
            saveFileDialogHtml.DefaultExt = "pdf";
            saveFileDialogHtml.Filter = "Adobe Acrobat Document *.pdf | *.pdf";
            saveFileDialogHtml.OverwritePrompt = true;
            saveFileDialogHtml.RestoreDirectory = true;
            saveFileDialogHtml.Title = "Exportera resultat till PDF";

            if (saveFileDialogHtml.ShowDialog() == DialogResult.Cancel)
                return;

            Thread exportThread = new Thread(
                new ThreadStart(exportResultsToPdfThread));
            exportThread.Name = "PdfExport";
            exportThread.IsBackground = true;
            exportThread.Start();
        }
        private void exportResultsToPdfThread()
        {
            bool finalResults = true;

            byte[] pdf = CommonCode.InternetPdfExportResults(finalResults, true);
            printToFile(pdf, saveFileDialogHtml.FileName);
            try
            {
                if (!CheckPDFInstalled())
                {
                    DialogResult res = 
                        MessageBox.Show("Adobe Acrobat Reader är inte " + 
                        "installerat. " +
                        "Det gör att du inte kan läsa manualen och inte " +
                        "kan titta på exporterade data.\r\n\r\n" + 
                        "Vill du öppna http://www.adobe.se för att ladda " +
                        "ner och installera Adobe Acrobat Reader?", 
                        "Adobe Acrobat Reader", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("IExplore.exe", 
                            "http://www.adobe.se");
                    }
                }
                else
                    System.Diagnostics.Process.Start(saveFileDialogHtml.FileName);
            }
            catch(Exception exc)
            {
                Trace.WriteLine("Exception occured in exportResultsToPdf(): " +
                    exc.ToString());
                MessageBox.Show("Kunde inte starta Internet Explorer till " +
                    "\"" + saveFileDialogHtml.FileName + "\".", 
                    "Ett fel har uppstått", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
        private void printToFile(byte[] content, string fileName)
        {
            try
            {
                if (System.IO.File.Exists(fileName))
                    System.IO.File.Delete(fileName);

                // make a new FileStream object, ready for read and write steps. 
                System.IO.FileStream fs = new 
                    System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate, 
                    System.IO.FileAccess.ReadWrite);

                // create a stream writer 
                System.IO.BinaryWriter w = new System.IO.BinaryWriter(fs);

                // set the file pointer to the end of file
                w.BaseStream.Seek(0, System.IO.SeekOrigin.End); 

                w.Write(content);
                w.Flush();
                w.Close();
                fs.Close();
            }
            catch(System.IO.IOException exc)
            {
                if (exc.ToString().IndexOf("The process cannot access the file") > -1 &
                    exc.ToString().IndexOf("because it is being used by another process") > -1)
                {
                    DialogResult res =
                        MessageBox.Show("Filnamnet som du angivit finns redan och är öppen av något annat program. Troligen är det Acrobat Reader. Stänger ner Acrobat Reader och klicka på OK.", "Filen används", MessageBoxButtons.OKCancel);
                    if (res == DialogResult.OK)
                        printToFile(content, fileName);
                }
                else
                    throw;
            }
        }
        #endregion

        #region Menu Excel
        private void menuExcelResults_Click(object sender, EventArgs e)
        {
            excelExportResults();
        }
        private void excelExportResults()
        {
            Trace.WriteLine("FMain: Entering excelExportResults()");

            if (CommonCode.GetCompetitorResultsCount() == 0)
            {
                Trace.WriteLine("FMain: excelExportResults() failed since there is no results.");
                MessageBox.Show("Det måste finnas inmatade resultat för att kunna exportera till Excel.");
                return;
            }

            saveFileDialogHtml.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialogHtml.AddExtension = true;
            saveFileDialogHtml.DefaultExt = "xls";
            saveFileDialogHtml.Filter = "Microsoft Excel Document *.xls | *.xls";
            saveFileDialogHtml.OverwritePrompt = true;
            saveFileDialogHtml.RestoreDirectory = true;
            saveFileDialogHtml.Title = "Exportera resultat till Excel";

            if (saveFileDialogHtml.ShowDialog() == DialogResult.Cancel)
                return;

            bool finalResults = true;

            byte[] pdf = CommonCode.InternetExcelExportResults(finalResults);
            printToFile(pdf, saveFileDialogHtml.FileName);
            try
            {
                if (!CheckExcelInstalled())
                {
                    DialogResult res =
                        MessageBox.Show("Microsoft Excel är inte " +
                        "installerat. ",
                        "Microsoft Excel kan inte hittas",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    System.Diagnostics.Process.Start(saveFileDialogHtml.FileName);
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLine("Exception occured in exportResultsToPdf(): " +
                    exc.ToString());
                MessageBox.Show("Kunde inte starta Internet Explorer till " +
                    "\"" + saveFileDialogHtml.FileName + "\".",
                    "Ett fel har uppstått",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Xml
        private void menuExportXml_Click(object sender, EventArgs e)
        {
            saveFileDialogHtml.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialogHtml.AddExtension = true;
            saveFileDialogHtml.DefaultExt = "xml";
            saveFileDialogHtml.Filter = "Xml Dokument *.xml | *.xml";
            saveFileDialogHtml.OverwritePrompt = true;
            saveFileDialogHtml.RestoreDirectory = true;
            saveFileDialogHtml.Title = "Exportera databas till XML";

            if (saveFileDialogHtml.ShowDialog() == DialogResult.Cancel)
                return;

            byte[] content =
                CommonCode.InternetXmlExport();

            printToFile(content, saveFileDialogHtml.FileName);
            try
            {
                System.Diagnostics.Process.Start(saveFileDialogHtml.FileName);
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #region Text
        private void menuExportText_Click(object sender, EventArgs e)
        {
            saveFileDialogHtml.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialogHtml.AddExtension = true;
            saveFileDialogHtml.DefaultExt = "txt";
            saveFileDialogHtml.Filter = "Text-dokument *.txt | *.txt";
            saveFileDialogHtml.OverwritePrompt = true;
            saveFileDialogHtml.RestoreDirectory = true;
            saveFileDialogHtml.Title = "Exportera skyttar till text-fil";

            if (saveFileDialogHtml.ShowDialog() == DialogResult.Cancel)
                return;

            byte[] content =
                CommonCode.InternetTextExport();

            printToFile(content, saveFileDialogHtml.FileName);
            try
            {
                System.Diagnostics.Process.Start(saveFileDialogHtml.FileName);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Weapons
        private void menuExportWeapons_Click(object sender, EventArgs e)
        {
            saveFileDialogHtml.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialogHtml.AddExtension = true;
            saveFileDialogHtml.DefaultExt = "txt";
            saveFileDialogHtml.Filter = "Text-dokument *.txt | *.txt";
            saveFileDialogHtml.OverwritePrompt = true;
            saveFileDialogHtml.RestoreDirectory = true;
            saveFileDialogHtml.Title = "Exportera vapen till text-fil";

            if (saveFileDialogHtml.ShowDialog() == DialogResult.Cancel)
                return;

            byte[] content =
                CommonCode.InternetWeaponsExport();

            printToFile(content, saveFileDialogHtml.FileName);
            try
            {
                System.Diagnostics.Process.Start(saveFileDialogHtml.FileName);
            }
            catch (Exception)
            {
            }
        }
        private void menuExportClubs_Click(object sender, EventArgs e)
        {
            saveFileDialogHtml.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialogHtml.AddExtension = true;
            saveFileDialogHtml.DefaultExt = "txt";
            saveFileDialogHtml.Filter = "Text-dokument *.txt | *.txt";
            saveFileDialogHtml.OverwritePrompt = true;
            saveFileDialogHtml.RestoreDirectory = true;
            saveFileDialogHtml.Title = "Exportera klubbar till text-fil";

            if (saveFileDialogHtml.ShowDialog() == DialogResult.Cancel)
                return;

            byte[] content =
                CommonCode.InternetClubsExport();

            printToFile(content, saveFileDialogHtml.FileName);
            try
            {
                System.Diagnostics.Process.Start(saveFileDialogHtml.FileName);
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #endregion

        #region Menu Help
        private void menuHelpManual_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuHelpManual_Click()");
            try
            {
                if (!CheckPDFInstalled())
                {
                    DialogResult res = 
                        MessageBox.Show("Adobe Acrobat Reader är inte " + 
                        "installerat. " +
                        "Det gör att du inte kan läsa manualen och inte " +
                        "kan titta på exporterade data.\r\n\r\n" + 
                        "Vill du öppna http://www.adobe.se för att ladda " +
                        "ner och installera Adobe Acrobat Reader?", 
                        "Adobe Acrobat Reader", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("IExplore.exe", 
                            "http://www.adobe.se");
                    }
                }
                else
                    System.Diagnostics.Process.Start("Manual.pdf");
            }
            catch(Exception exc)
            {
                Trace.WriteLine("Exception occured in menuHelpManual_Click(): " +
                    exc.ToString());
                MessageBox.Show("Kunde inte starta Manual.pdf.", 
                    "Ett fel har uppstått", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void menuHelpInternetAllberg_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuHelpInternetAllberg_Click()");

            try
            {
                System.Diagnostics.Process.Start("IExplore.exe", "http://www.allberg.se");
            }
            catch(Exception exc)
            {
                Trace.WriteLine("Exception occured in menuHelpSupport_Click(): " +
                    exc.ToString());
                MessageBox.Show("Kunde inte starta Internet Explorer till " +
                    "\"http://www.allberg.se\".", 
                    "Ett fel har uppstått", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
        private void menuHelpSupport_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuHelpSupport_Click()");

            try
            {
                System.Diagnostics.Process.Start("IExplore.exe", "http://www.winshooter.se");
            }
            catch(Exception exc)
            {
                Trace.WriteLine("Exception occured in menuHelpSupport_Click(): " +
                    exc.ToString());
                MessageBox.Show("Kunde inte starta Internet Explorer till " +
                    "\"http://www.winshooter.se\".", 
                    "Ett fel har uppstått", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void menuHelpAbout_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering menuHelpAbout_Click()");

            Enabled = false;
            myAboutWindow.Visible = true;
        }
        #endregion


        #region Menu Settings
        private void menuSettings_Click(object sender, System.EventArgs e)
        {
            mySettingsWindow.EnableMe();
        }
        #endregion

        #endregion


        #region Handlers

        private delegate void MenuItemEnabledHandler(MenuItem item, bool itemEnabled);
        private MenuItemEnabledHandler MenuItemEnabledInvoker;

        private void MenuItemEnabled(MenuItem item, bool itemEnabled)
        {
            item.Enabled = itemEnabled;
        }
        private delegate void MenuItemVisibleHandler(MenuItem item, bool itemVisible);
        private MenuItemVisibleHandler MenuItemVisibleInvoker;

        private void MenuItemVisible(MenuItem item, bool itemVisible)
        {
            item.Visible = itemVisible;
        }
        readonly object CommonCode_AllLock = new object();
        private void CommonCode_All()
        {
            Trace.WriteLine("FMain: CommonCode_All() " + 
                " locking \"CommonCode_AllLock\" on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " )");

            lock(CommonCode_AllLock)
            {
                Trace.WriteLine("FMain: CommonCode_All() " + 
                    " locked \"CommonCode_AllLock\" on thread \"" +
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString() + " )");

                if (CommonCode_AllThread == null)
                    CommonCode_AllThread = 
                        new Thread(
                        new ThreadStart(CommonCode_AllWithThread));
                CommonCode_AllThread.Name = "FMain:CommonCode_AllThread";
                CommonCode_AllThread.IsBackground = true;
                CommonCode_AllThread.Start();

                Trace.WriteLine("FMain: CommonCode_All() " + 
                    " unlocking \"CommonCode_AllLock\" on thread \"" +
                    Thread.CurrentThread.Name + "\" ( " +
                    Thread.CurrentThread.ManagedThreadId.ToString() + " )");
            }
        }
        Thread CommonCode_AllThread;
        private void CommonCode_AllWithThread()
        {
            CommonCode_UpdatedClub();
            CommonCode_UpdatedWeapon();
            CommonCode_UpdatedCompetition();
            CommonCode_UpdatedStation();
            CommonCode_UpdatedShooter();
            CommonCode_UpdatedCompetitor();
            CommonCode_UpdatedPatrol();
            CommonCode_UpdatedCompetitorResult();
        }
        private void CommonCode_UpdatedCompetition()
        {
            Trace.WriteLine("FMain: Entering CommonCode_UpdatedCompetition()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            try
            {
                myPatrolsWindow.UpdatedCompetition();
                myResultsViewWindow.UpdatedCompetition();
                setHeader();
                Structs.Competition[] comps =
                    CommonCode.GetCompetitions();
                if (comps.Length>0)
                {
                    Structs.Competition comp = comps[0];

                    btnPatrols.Enabled = true;
                    btnStations.Enabled = true;

                    if (comp.Type == Structs.CompetitionTypeEnum.Precision)
                    {
                        if (btnStations.Text != "Serier")
                        {
                            UpdateDisplayStringsEvent();
                        } 
                    }
                }
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FMain: Exception" + exc.ToString());
            }
            checkForEnableCompetitors();
            Trace.WriteLine("FMain: Exiting CommonCode_UpdatedCompetition()");
        }
        private void updateDisplayStrings()
        {
            if (!btnStations.InvokeRequired)
            {
                btnStations.Text = "Serier";
                btnPatrols.Text = "Skjutlag";
                menuPrintPrelResultPatrol.Text = "Enskilt skjutlag";
                menuPdfPatrollist.Text = "Laglista";
                menuPdfPatrollistByClub.Text = "Laglista efter klubb";
                menuPrintPatrollist.Text = "Lag";
                menuPrintPatrollistByPatrol.Text = "Efter Lag (Lagpärm)";
                lblPatrolCountHeader.Text = "Antal skjutlag";
            }
            else
            {
                this.Invoke(UpdateDisplayStringsEvent);
            }
        }
        private void CommonCode_UpdatedStation()
        {
            Trace.WriteLine("FMain: Entering CommonCode_UpdatedStation()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            try
            {
                Structs.Competition[] comps = CommonCode.GetCompetitions();
                if (comps.Length == 0)
                    return;

                switch(comps[0].Type)
                {
                    case Structs.CompetitionTypeEnum.Field:
                        myStationsFieldWindow.UpdatedStations();
                        break;
                    case Structs.CompetitionTypeEnum.MagnumField:
                        myStationsFieldWindow.UpdatedStations();
                        break;
                    case Structs.CompetitionTypeEnum.Precision:
                        myStationsPrecisionWindow.UpdatedStations();
                        break;
                    default:
                        throw new ApplicationException("Unknown comp type: " + comps[0].Type.ToString());
                }
                checkForEnableResultsButton();
            }
            catch(Exception exc)
            {
                Trace.WriteLine("Exception occured in CommonCode_UpdatedStation: " + exc.ToString());
            }

            Trace.WriteLine("FMain: Exiting CommonCode_UpdatedStation()");
        }
        private void CommonCode_UpdatedClub()
        {
            Trace.WriteLine("FMain: Entering CommonCode_UpdatedClub()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            try
            {
                myClubsWindow.UpdatedClubs();
                myTeamsWindow.UpdatedClubs();

                Trace.WriteLine("FMain: Entering CommonCode_UpdatedClub:myCompetitorsWindow.updatedClubs");
                myCompetitorsWindow.updatedClubs();

                Trace.WriteLine("FMain: Entering CommonCode_UpdatedClub:GetClubsCountWithUsers()");
                setlblClubCount( CommonCode.GetClubsCountWithShooters().ToString() );

                Trace.WriteLine("FMain: Entering CommonCode_UpdatedClub:checkForEnableCompetitors()");
                checkForEnableCompetitors();
            }
            catch(Exception exc)
            {
                Trace.WriteLine("Exception occured in CommonCode_UpdatedClub: " + exc.ToString());
            }
        
            Trace.WriteLine("FMain: Exiting CommonCode_UpdatedClub()");
        }
        private void CommonCode_UpdatedWeapon()
        {
            Trace.WriteLine("FMain: Entering CommonCode_UpdatedWeapon()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            myWeaponsWindow.updatedWeapons();
            myCompetitorsWindow.updatedWeapons();
            checkForEnableCompetitors();
        
            Trace.WriteLine("FMain: Entering CommonCode_UpdatedWeapon()");
        }
        private void CommonCode_UpdatedShooter()
        {
            Trace.WriteLine("FMain: Entering CommonCode_UpdatedShooter()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            myCompetitorsWindow.updatedShooters();
            setlblShooterCount(CommonCode.GetShootersCount()
                .ToString());
            setlblClubCount(CommonCode.GetClubsCountWithShooters()
                .ToString());
        
            Trace.WriteLine("FMain: Exiting CommonCode_UpdatedShooter()");
        }
        private void CommonCode_UpdatedCompetitor()
        {
            Trace.WriteLine("FMain: Entering CommonCode_UpdatedCompetitor()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");
            // Get count to Main GUI
            setlblCompetitorCount(
                CommonCode.GetCompetitorsCount().ToString()); 
            myPatrolsWindow.UpdatedPatrols();
            myResultsViewWindow.UpdatedCompetitors();
            checkForEnableResultsButton();
        }
        private void CommonCode_UpdatedCompetitorResult()
        {
            Trace.WriteLine("FMain: Entering CommonCode_UpdatedCompetitorResult()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");
            myResultsViewWindow.UpdatedCompetitorResults();
        }
        private void CommonCode_UpdatedPatrol()
        {
            Trace.WriteLine("FMain: Entering CommonCode_UpdatedPatrol()");

            setlblPatrolCount(CommonCode.GetPatrolsCount().ToString());
            myPatrolsWindow.UpdatedPatrols();
            myCompetitorsWindow.updatedPatrols();
            checkForEnableResultsButton();
            checkForEnablePrintPatrol();
        }
        private void CommonCode_UpdatedTeam()
        {
            Trace.WriteLine("FMain: Entering CommonCode_UpdatedTeam()");
            myTeamsWindow.UpdatedTeams();
            Trace.WriteLine("FMain: Ended CommonCode_UpdatedTeam()");
        }
        private void checkForEnablePrintPatrol()
        {
            Trace.WriteLine("FMain: Entering checkForEnablePrintPatrol()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            if (CommonCode.GetPatrolsCount()>0)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(MenuItemEnabledInvoker, new object[] { menuPrint, true });
                    this.Invoke(MenuItemEnabledInvoker, new object[] { menuPrintPatrollist, true });
                    this.Invoke(MenuItemEnabledInvoker, new object[] { menuInternet, true });
                    this.Invoke(MenuItemEnabledInvoker, new object[] { menuPdf, true });
                    this.Invoke(MenuItemEnabledInvoker, new object[] { menuExcel, true });
                    this.Invoke(MenuItemEnabledInvoker, new object[] { menuInternetPatrollist, true });
                    this.Invoke(MenuItemEnabledInvoker, new object[] { menuPdfPatrollist, true });
                    this.Invoke(MenuItemEnabledInvoker, new object[] { menuInternetPatrollistByClub, true });
                    this.Invoke(MenuItemEnabledInvoker, new object[] { menuPdfPatrollistByClub, true });
                }
                else
                {
                    menuPrint.Enabled = true;
                    menuPrintPatrollist.Enabled = true;
                    menuInternet.Enabled = true;
                    menuPdf.Enabled = true;
                    menuExcel.Enabled = true;
                    menuInternetPatrollist.Enabled = true;
                    menuPdfPatrollist.Enabled = true;
                    menuInternetPatrollistByClub.Enabled = true;
                    menuPdfPatrollistByClub.Enabled = true;
                }
            }
        }
        private void checkForEnableCompetitors()
        {
            Trace.WriteLine("FMain: Entering checkForEnableCompetitors()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            if (CommonCode.GetClubsCount()>0 &
                CommonCode.GetWeaponsCount()>0 &
                CommonCode.GetCompetitionsCount()>0)
            {
                btnCompetitors.Enabled = true;
                if (this.InvokeRequired)
                {
                    Invoke(MenuItemEnabledInvoker, new object[] { menuImport, true });
                    Invoke(MenuItemEnabledInvoker, new object[] { menuExport, true });
                }
                else
                {
                    menuImport.Enabled = true;
                    menuExport.Enabled = true;
                }
            }
            else
            {
                btnCompetitors.Enabled = false;
                if (InvokeRequired)
                {
                    Invoke(MenuItemEnabledInvoker, new object[] { menuImport, false });
                    Invoke(MenuItemEnabledInvoker, new object[] { menuExport, false });
                }
                else
                {
                    menuImport.Enabled = false;
                    menuExport.Enabled = false;
                }
            }
            Trace.WriteLine("FMain: Ending checkForEnableCompetitors()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");
        }
        private void checkForEnableResultsButton()
        {
            Trace.WriteLine("FMain: Entering checkForEnableResultsButton()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            if (CommonCode.GetPatrolsCount() > 0 &
                CommonCode.GetShootersCount() >0 &
                CommonCode.GetCompetitorsCount() > 0 &
                CommonCode.GetStationsCount() > 0)
            {
                btnResults.Enabled = true;
                btnViewResults.Enabled = true;
                btnTeamManagement.Enabled = true;
                if (InvokeRequired)
                {
                    Invoke(MenuItemEnabledInvoker, new object[] { menuPrintResults, true });
                    Invoke(MenuItemEnabledInvoker, new object[] { menuPrintLabelsResultPatrol, true });
                    Invoke(MenuItemEnabledInvoker, new object[] { menuPrintPrelResults, true });
                    Invoke(MenuItemEnabledInvoker, new object[] { menuInternetResults, true });
                    Invoke(MenuItemEnabledInvoker, new object[] { menuPdfResults, true });
                    Invoke(MenuItemEnabledInvoker, new object[] { menuExcelResults, true });

                    if (CommonCode.CompetitionCurrent.Type ==
                        Structs.CompetitionTypeEnum.Precision)
                    {
                        Invoke(MenuItemEnabledInvoker, new object[] { menuPrintLabels, true });
                        Invoke(MenuItemVisibleInvoker, new object[] { menuPrintLabels, true });
                        Invoke(MenuItemEnabledInvoker, new object[] { menuPrintLabelsMirrors, true });
                        Invoke(MenuItemVisibleInvoker, new object[] { menuPrintLabelsMirrors, true });
                    }
                }
                else
                {
                    menuPrintResults.Enabled = true;
                    menuPrintLabelsResultPatrol.Enabled = true;
                    menuPrintPrelResults.Enabled = true;
                    menuInternetResults.Enabled = true;
                    menuPdfResults.Enabled = true;
                    menuExcelResults.Enabled = true;
                    
                    if (CommonCode.CompetitionCurrent.Type ==
                        Structs.CompetitionTypeEnum.Precision)
                    {
                        menuPrintLabels.Visible = true;
                        menuPrintLabelsMirrors.Visible = true;
                        menuPrintLabels.Enabled = true;
                        menuPrintLabelsMirrors.Enabled = true;
                    }
                }
            }
            else
            {
                btnResults.Enabled = false;
                btnViewResults.Enabled = false;
                btnTeamManagement.Enabled = false;
                if (InvokeRequired)
                {
                    Invoke(MenuItemEnabledInvoker, new object[] { menuExcelResults, false });
                }
                else
                {
                    menuExcelResults.Enabled = false;
                }
            }
        }
        private void setlblPatrolCount(string toSet)
        {
            Trace.WriteLine("FMain: Entering setlblPatrolCount()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            if (lblPatrolCount.InvokeRequired)
            {
                Trace.WriteLine("FMain: setlblPatrolCount: Invokation Required. Calling Invoker.");
                Invoke(SetlblPatrolCount, new object[] {toSet});
            }
            else
            {
                lblPatrolCount.Text = toSet;
            }
            Trace.WriteLine("FMain: Ending setlblPatrolCount()");
        }
        private void setlblShooterCount(string toSet)
        {
            Trace.WriteLine("FMain: Entering setlblShooterCount()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            if (lblShooterCount.InvokeRequired)
            {
                Trace.WriteLine("FMain: setlblPatrolCount: Invokation Required. Calling Invoker.");
                Invoke(SetlblShooterCount, new object[] {toSet});
            }
            else
            {
                lblShooterCount.Text = toSet;
            }
            Trace.WriteLine("FMain: Ending setlblShooterCount()");
        }
        private void setlblCompetitorCount(string toSet)
        {
            Trace.WriteLine("FMain: Entering setlblCompetitorCount()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");
            if (lblCompetitorCount.InvokeRequired)
            {
                Trace.WriteLine("FMain: setlblPatrolCount: Invokation Required. Calling Invoker.");
                Invoke(SetlblCompetitorCount, new object[] {toSet});
            }
            else
            {
                lblCompetitorCount.Text = toSet;
            }
            Trace.WriteLine("FMain: Ending setlblCompetitorCount()");
        }
        private void setlblClubCount(string toSet)
        {
            Trace.WriteLine("FMain: Entering setlblClubCount()" +
                " on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
                Thread.CurrentThread.ManagedThreadId.ToString() + " ) ");

            if (lblClubCount.InvokeRequired)
            {
                Trace.WriteLine("FMain: setlblPatrolCount: Invokation Required. Calling Invoker.");
                BeginInvoke(SetlblClubCount, new object[] {toSet});
            }
            else
            {
                lblClubCount.Text = toSet;
            }

            Trace.WriteLine("FMain: Ending setlblClubCount()");
        }
        #endregion


        #region Buttons
        private void btnCompetition_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering btnCompetition_Click()");

            Structs.Competition comp = CommonCode.GetCompetitions()[0];
            switch(comp.Type)
            {
                case Structs.CompetitionTypeEnum.Field:
                    myCompetitionFieldWindow.enableMe();
                    break;
                case Structs.CompetitionTypeEnum.MagnumField:
                    myCompetitionFieldWindow.enableMe();
                    break;
                case Structs.CompetitionTypeEnum.Precision:
                    myCompetitionPrecisionWindow.enableMe();
                    break;
                default:
                    throw new ApplicationException("Unknown CompetitionType: " + comp.Type.ToString());
            }
        }
        private void btnStations_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering btnStations_Click()");

            Structs.Competition comp = CommonCode.GetCompetitions()[0];
            switch(comp.Type)
            {
                case Structs.CompetitionTypeEnum.Field:
                    myStationsFieldWindow.enableMe();
                    break;
                case Structs.CompetitionTypeEnum.MagnumField:
                    myStationsFieldWindow.enableMe();
                    break;
                case Structs.CompetitionTypeEnum.Precision:
                    myStationsPrecisionWindow.enableMe();
                    break;
                default:
                    throw new ApplicationException("Unknown competition type: " + comp.Type.ToString());
            }
        }

        private void btnClubs_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering btnClubs_Click()");

            myClubsWindow.EnableMe();
        }

        private void btnCompetitors_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering btnCompetitors_Click()");

            myCompetitorsWindow.EnableMe();
        }

        private void btnWeapons_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering btnWeapons_Click()");

            myWeaponsWindow.enableMe();
        }
        private void btnPatrols_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering btnPatrols_Click()");

            myPatrolsWindow.enableMe();
        }
        private void btnResults_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering btnResults_Click()");

            myResultsWindow.enableMe();		
        }
        private void btnViewResults_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering btnViewResults_Click()");

            myResultsViewWindow.enableMe();
        }
        private void btnTeamManagement_Click(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: Entering btnTeamManagement_Click()");

            myTeamsWindow.EnableMe();
        }

        #endregion


        internal string AssemblyVersion = "";
        private void getAssemblyVersion()
        {
            AssemblyVersion = getAssemblyVersion(4);
        }
        private string getAssemblyVersion(int nrOfNrs)
        {
            // Get running assembly version
            System.Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            string assemblyVersion = ver.Major.ToString() + "." + ver.Minor.ToString();
            if (!(ver.Build == 0 &
                ver.Revision == 0))
            {
                assemblyVersion += "." +
                    ver.Build.ToString();
                if (ver.Revision != 0)
                {
                    assemblyVersion += "." +
                        ver.Revision.ToString();
                }
            }
            return assemblyVersion;
        }

        private void checkForNewerVersion()
        {
            checkForNewerVersion(true);
        }
        private void checkForNewerVersion(bool wait)
        {
            Trace.WriteLine("FMain: checkForNewerVersion started on thread \"" +
                Thread.CurrentThread.Name + "\" ( " +
            Thread.CurrentThread.ManagedThreadId.ToString() + " ) Sleeping...");
            if (wait)
                Thread.Sleep(10000);
            Trace.WriteLine("FMain: checkForNewerVersion wakeup.");
            try
            {
                bool timeToCheck = false;
                // Read last check time from registry.
                RegistryKey rk = Registry.CurrentUser;
                RegistryKey winshooterRead = rk.OpenSubKey("Software\\Allberg\\WinShooter", false);
                if (winshooterRead != null)
                {
                    // Registry hive did exist.
                    string val = (string)winshooterRead.GetValue("LastNewVersionCheck");
                    if (val != null)
                    {
                        DateTime last = DateTime.Parse(val);
                        // If more that 1 week, ask user if to check
                        if (last.AddDays(7) < DateTime.Now)
                        {
                            timeToCheck = true;
                        }
                    }
                    else
                    {
                        timeToCheck = true;
                    }
                }
                else
                {
                    timeToCheck = true;
                }

                if (timeToCheck)
                {
                    Trace.WriteLine("FMain: checkForNewerVersion:Time to check for newer version");

                    // Write todays date & time to registry
                    Trace.WriteLine("FMain: checkForNewerVersion:Setting new value to Registry");
                    RegistryKey winshooter = rk.OpenSubKey("Software\\Allberg\\WinShooter", true);
                    if (winshooter == null)
                    {
                        Trace.WriteLine("FMain: checkForNewerVersion:Hive didn't exist, creating hive");

                        // Registry hive did not exist.
                        rk.CreateSubKey("Software\\Allberg\\WinShooter");
                        winshooter = rk.OpenSubKey("Software\\Allberg\\WinShooter", true);
                    }
                    winshooter.SetValue("LastNewVersionCheck", DateTime.Now.ToString());

                    Trace.WriteLine("FMain: checkForNewerVersion:New value is set to Registry");
#if !DEBUG
                    if (DialogResult.Yes == 
                        MessageBox.Show("WinShooter vill kontrollera om det " + 
                        "finns en nyare version tillgänglig. " + 
                        "Vill du tillåta det nu?", 
                        "Ny version", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question))
                    {
                        string urlstring = "";
                        string versionstring = "";
                        string autoupdateurl = "";
                        if (checkForNewerVersionInternet(ref urlstring, ref versionstring, ref autoupdateurl))
                        {
                            DialogResult res = MessageBox.Show(
                                "En nyare version av WinShooter finns " + 
                                "tillgänglig (" + versionstring + "). Vill du hämta den nu?", 
                                "Ny version", 
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Information);
                            if (res == DialogResult.Yes)
                            {
                                updateWinShooterNow(autoupdateurl, urlstring);
                            }
                            return;
                        }
                    }
#else
                    Trace.WriteLine("FMain: THIS IS BETA. Connecting to update files.");
                    string urlstring = "";
                    string versionstring = "";
                    string autoupdateurl = "";
                    if (checkForNewerVersionInternet(ref urlstring, ref versionstring, ref autoupdateurl,
                        "http://www.winshooter.se/currentbetaversion.txt"))
                    {
                        updateWinShooterNow(autoupdateurl, urlstring);
                    }
#endif
                }
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }
            finally
            {
                Trace.WriteLine("FMain: checkForNewerVersion ended.");
            }
        }

        private bool checkForNewerVersionInternet(
            ref string urlstring, ref string versionstring, ref string autoupdateurl)
        {
            return checkForNewerVersionInternet(ref urlstring, ref versionstring, ref autoupdateurl,
                "http://www.winshooter.se/currentversion.txt");
        }
        private bool checkForNewerVersionInternet(
            ref string urlstring, ref string versionstring, ref string autoupdateurl,
            string urlToVersionFile)
        {
            // Check version at server
            System.Net.WebClient webclient = new System.Net.WebClient();
            System.IO.Stream stream = 
                webclient.OpenRead(urlToVersionFile);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string file = reader.ReadLine();
            versionstring = file.Split(';')[0];
            urlstring = file.Split(';')[1];
            autoupdateurl = file.Split(';')[2];
            Trace.WriteLine(file);
            reader.Close();
            stream.Close();
            webclient.Dispose();

            Version version = new Version(versionstring);
            if (version > Assembly.GetExecutingAssembly().GetName().Version)
                return true;
            else
                return false;
        }
        private void timerSync_Tick(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: timerSync_Tick started successfully from thread \"" +
                Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            try
            {
                CommonCode.Sync();
            }
            catch(System.Net.WebException exc)
            {
                timerSyncThread.Stop();
                Trace.WriteLine("Lost contact with server?:\r\n" + exc.ToString());
                MessageBox.Show("Tappade kontakten med servern.\r\n" + 
                    "Starta om programmet och anslut till servern igen", 
                    "Tappade server-kontakt", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Stop);
                Dispose(true);
            }
            catch(Exception exc)
            {
                timerSyncThread.Stop();
                Trace.WriteLine("Lost contact with server?:\r\n" + exc.ToString());
                MessageBox.Show("Tappade kontakten med servern.\r\n" + 
                    "Starta om programmet och anslut till servern igen", 
                    "Tappade server-kontakt", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Stop);
                
                Dispose(true);
            }
        }


        private bool CheckPDFInstalled()
        {
            string RegistryPlace = @".pdf";
            RegistryKey rk = Registry.ClassesRoot;
            RegistryKey pdfKey = rk.OpenSubKey(RegistryPlace);
            if (pdfKey == null)
                return false;

            object defaultValue = pdfKey.GetValue("");
            if (defaultValue == null ||
                defaultValue.ToString() == "")
                return false;

            return true;
        }


        private bool CheckExcelInstalled()
        {
            string RegistryPlace = @".xls";
            RegistryKey rk = Registry.ClassesRoot;
            RegistryKey pdfKey = rk.OpenSubKey(RegistryPlace);
            if (pdfKey == null)
                return false;

            object defaultValue = pdfKey.GetValue("");
            if (defaultValue == null ||
                defaultValue.ToString() == "")
                return false;

            return true;
        }

        #region Autoupdate
        delegate void UpdateWinShooterNowDelegate(string urlstring, string manualurlstring);
        event UpdateWinShooterNowDelegate UpdateWinShooterNowEvent;


        private void updateWinShooterNow(string urlstring, string manualUrlString)
        {
            Trace.WriteLine("FMain: updateWinShooterNow started from thread \"" +
                Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            if (this.InvokeRequired)
            {
                Trace.WriteLine("FMain: updateWinShooterNow needs to be invoked. Invoking now...");
                Invoke(UpdateWinShooterNowEvent, new object[] { urlstring });
                return;
            }

            if (AutoUpdater.UpdaterLib.HandleVista.IsRunningVistaProtectedMode())
            {
                MessageBox.Show("Du kör Vista och Winshooter kör inte som en administratör. " +
                    "Det gör att Winshooter inte kommer att kunna uppgraderas. Du rekommenderas " +
                    "att endera köra Winshooter som administratör eller att ladda ner en helt " +
                    "ny installation.",
                    "Vista Protected Mode",
                    MessageBoxButtons.OK);

                System.Diagnostics.Process.Start(manualUrlString);

                return;
            }

            try
            {
                Assembly ass = Assembly.GetExecutingAssembly();
                string currentdir = ass.Location;
                currentdir = currentdir.Substring(0, currentdir.LastIndexOf("\\"));
                string targetdir = currentdir + "\\Updater";

                Allberg.AutoUpdater.UpdaterLib.IDownloader downloader = 
                    new Allberg.AutoUpdater.UpdaterLib.DownloaderHttpClient();
                //downloader.FileDone += new Allberg.AutoUpdater.UpdaterLib.FileDoneHandler(downloader_FileDone);
                downloader.BytesDone += new Allberg.AutoUpdater.UpdaterLib.BytesDoneHandler(downloader_BytesDone);

                downloader.AllFilesDone += 
                    new Allberg.AutoUpdater.UpdaterLib.AllFilesDoneHandler(
                    downloader_AllFilesDone);
                downloader.UpdateFailed +=
                    new Allberg.AutoUpdater.UpdaterLib.UpdateFailedHandler(downloader_UpdateFailed);

                progressBar1.Maximum = downloader.PrepareFiles(
                    urlstring,
                    targetdir, 
                    currentdir);
                progressBar1.Visible = true;

                System.Threading.Thread thread = new System.Threading.Thread(
                    new System.Threading.ThreadStart(downloader.GetFiles));
                thread.IsBackground = true;
                thread.Start();
            }
            catch(Exception exc)
            {
                Trace.WriteLine("Exception occured in exportResultsToInternet(): " +
                    exc.ToString());
                MessageBox.Show("Kunde köra uppdatering av WinShooter. Försök igen senare.", 
                    "Ett fel har uppstått", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void downloader_AllFilesDone()
        {
            MessageBox.Show("Nerladdningen av ny version av WinShooter är färdig. " +
                "Nuvarande version måste stängas för att uppgraderingen ska kunna ske.", 
                "Uppgradering", 
                MessageBoxButtons.OK);

            Assembly ass = Assembly.GetExecutingAssembly();
            string targetdir = ass.Location.Substring(
                0, ass.Location.LastIndexOf("\\"));
            string sourcedir = targetdir + "\\Updater";

            string process = "winshooter";

            string fileToRun = "UpdaterGui.exe";
            if (System.IO.File.Exists(sourcedir + "\\" + fileToRun))
                fileToRun = sourcedir + "\\" + fileToRun;


            string arguments = process + " \"" + sourcedir + "\" \"" + targetdir + 
                    "\" \"" + ass.Location + "\"";
            if ( currentFile != "")
            {
                arguments += " \"" + currentFile + "\" ";
            }

            if (Trace.Listeners.Count > 1)
                arguments += " debug ";

            arguments = arguments.Trim();
            Trace.WriteLine("Starting updategui: \"" + fileToRun + "\" with arguments \r\n\"" +
                arguments + "\"");

            System.Diagnostics.Process.Start(fileToRun, 
                arguments);

            Application.Exit();
        }
        void downloader_BytesDone(int bytesReceived)
        {
            //Trace.WriteLine("Bytes done: " + bytesReceived.ToString("### ### ### ###") +
            //    " / " + progressBar1.Maximum.ToString("### ### ### ###"));
            if (bytesReceived < this.progressBar1.Maximum)
                progressBar1.Value = bytesReceived;
        }

        private void downloader_UpdateFailed(Exception exc)
        {
            Trace.WriteLine("FMain: UpdateFailed: " + exc.ToString());
            MessageBox.Show("Ett fel uppstod vid uppdatering: " + exc.Message);
        }

        #endregion

        private void menuTestErrorHandling_Click(object sender, EventArgs e)
        {
            throw new ApplicationException("This is a Test");
        }

        private void timerEasterEgg_Tick(object sender, EventArgs e)
        {
            timerEasterEgg.Enabled = false;
            timerEasterEgg.Stop();

            ScreenEffect effect = new ScreenEffect();
            effect.RunEasterEgg();
        }
    }
}
