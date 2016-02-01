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

namespace Allberg.Shooter.WinShooterServer
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Http;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Summary description for FMain.
    /// </summary>
    public class FMain : System.Windows.Forms.Form
    {
        private Allberg.Shooter.Windows.Forms.SafeLabel label1;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtFilename;
        private System.Windows.Forms.DataGrid dataGrid1;
        private DatasetClients datasetClients1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.IContainer components;



        private System.Windows.Forms.Button btnDispose;
        private System.Windows.Forms.Timer timerClientsUpdate;
        private Allberg.Shooter.Windows.Forms.SafeLabel label2;
        private Allberg.Shooter.Windows.Forms.SafeTextBox txtServerAddress;
        private System.Windows.Forms.CheckBox chkWebServer;
        private System.Windows.Forms.Timer timerKeepConnection;
        private Allberg.Shooter.Windows.Forms.SafeLabel label3;
        private System.Windows.Forms.NumericUpDown numCasheSeconds;
        private Allberg.Shooter.Windows.Forms.SafeLabel label4;
        private System.Windows.Forms.Timer timerThreadCount;
        private NumericUpDown numHttpPort;
        private Button btnBackup;
        private System.Windows.Forms.ProgressBar progressBarThreads;
        private DataGridTableStyle dataGridTableStyle1;
        private DataGridTextBoxColumn dataGridTextBoxIPAddress;
        private DataGridTextBoxColumn dataGridTextBoxLastUpdate;

        private delegate void FMainSetServerAddressDelegate(string addresses);
        private event FMainSetServerAddressDelegate FMainSetServerAddressInvoker;
        private event MethodInvoker StartupTimersInvoker;

        private delegate void SetCursorDelegate(System.Windows.Forms.Cursor cursor);
        private event SetCursorDelegate SetCursorInvoker;

        FBackup backupWindow;

        public FMain()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            FMainStartupInvoker += 
                new MethodInvoker(FMainStartup);
            FMainSetServerAddressInvoker += 
                new FMainSetServerAddressDelegate(FMainSetServerAddress);
            StartupTimersInvoker += 
                new MethodInvoker(StartUpTimers);
            SetCursorInvoker += new SetCursorDelegate(setCursor);
            this.FormClosing += new FormClosingEventHandler(FMain_FormClosing);

            System.Threading.Thread.CurrentThread.Name = "Server Main Thread";
            Thread dummyThread = 
                new Thread(
                new ThreadStart(this.FMainStartupDummy));
            dummyThread.Name = "dummyThread";
            dummyThread.Start();
        }



        private void FMainStartupDummy()
        {
            Trace.WriteLine("FMain: FMainStartupDummy started from thread \"" + 
                System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());
            while(!this.IsHandleCreated)
                System.Threading.Thread.Sleep(50);
            this.BeginInvoke(FMainStartupInvoker);
        }
        private event MethodInvoker FMainStartupInvoker;
        private void FMainStartup()
        {
            Trace.WriteLine("FMain: FMainStartup started from thread \"" + 
                System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());
            getAssemblyVersion();

            // Open database
            string startDir = Allberg.Shooter.Common.LocalSettings.DataPath;

            if (startDir.Trim().Length == 0)
                startDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            this.openFileDialog1.CheckFileExists = true;
            this.openFileDialog1.Filter = "Allberg Skytte *.wsk|*.wsk";
            this.openFileDialog1.InitialDirectory = startDir;
            DialogResult res = this.openFileDialog1.ShowDialog();
            if (res != DialogResult.OK)
            {
                Trace.WriteLine("FMain: Database not choosen. " +
                    "Setting DoImmmidiateDispose= true");

                MessageBox.Show("Du måste välja en befintlig databas vid start av servern." + 
                    "\r\nStarta servern igen och välj en befintlig databas.", 
                    "Fel", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            Allberg.Shooter.Common.LocalSettings.DataPath =
                Path.GetDirectoryName(openFileDialog1.FileName);

            ServerStartupMethodInvoker += new MethodInvoker(this.ServerStartup);

            System.Threading.Thread serverStartThread =
                new Thread(
                new ThreadStart(ServerStartup));
            serverStartThread.Name = "serverStartThread";
            serverStartThread.Start();
        }

        private MethodInvoker ServerStartupMethodInvoker;
        private void ServerStartupDummy()
        {
            try
            {
                Trace.WriteLine("FMain: FMainStartupDummy started from thread \"" + 
                    System.Threading.Thread.CurrentThread.Name + "\" " +
                    " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                    DateTime.Now.ToLongTimeString());
                while(!this.IsHandleCreated)
                    System.Threading.Thread.Sleep(50);
                this.BeginInvoke(ServerStartupMethodInvoker);
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FMain: FMainStartupDummy Exception: " + exc.ToString());
            }
        }

        private void setCursor(System.Windows.Forms.Cursor cursor)
        {
            
            if (this.InvokeRequired)
                Invoke(SetCursorInvoker, new object[] { cursor });
            else
                this.Cursor = cursor;
        }
        private int ServerPort = 90;
        private void ServerStartup()
        {
            setCursor(System.Windows.Forms.Cursors.WaitCursor);

            try
            {
                // Start server
                Trace.WriteLine("FMain: starting server from thread \"" + 
                    System.Threading.Thread.CurrentThread.Name + "\" " +
                    " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                    DateTime.Now.ToLongTimeString());

                bool keepRunning = true;
                while (keepRunning)
                {
                    try
                    {
                        ServerStartupRemoting();
                        keepRunning = false;
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                        ServerPort++;
                    }
                }
                ServerStartupOpenFile();
                if (myInterface.GetCompetitions().Length == 0)
                {
                    Trace.WriteLine("Not even one competition exists in newly opened file. Exiting.");

                    MessageBox.Show("I denna fil finns inte ens en enda tävling " +
                        "definierad. Den kan därmed inte öppnas.",
                        "Felmeddelande",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    Application.Exit();
                }
                ServerStartupUdpListener();
                backupWindow = new FBackup(myInterface);
                backupWindow.EnableMain += 
                    new FBackup.EnableMainHandler(EnableMain);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception: " + exc.ToString());
                Trace.WriteLine("FMain: ServerStartup: Exception: " + exc.ToString());
            }
            finally
            {
                setCursor(System.Windows.Forms.Cursors.Default);

                Trace.WriteLine("FMain: ServerStartup: Done.");
            }
        }

        void EnableMain()
        {
            this.Focus();
        }

        

        private void ServerStartupRemoting()
        {
            SoapClientFormatterSinkProvider clientSinkProvider = new SoapClientFormatterSinkProvider();
            IDictionary props = new Hashtable();
            props["typeFilterLevel"] = "Full";

            SoapServerFormatterSinkProvider serverSinkProvider =
                new SoapServerFormatterSinkProvider(props, null);
            ClientIpServerSinkProvider clientIpSinkProvider =
                new ClientIpServerSinkProvider(props, null);
            serverSinkProvider.Next = clientIpSinkProvider;


            props["port"] = ServerPort;
            props["name"] = "ServerChannel";
            HttpChannel chan = new HttpChannel(props, clientSinkProvider, serverSinkProvider);
            string name = chan.ChannelName;
            ChannelServices.RegisterChannel(chan, false);


            if (System.IO.File.Exists(configFile))
                RemotingConfiguration.Configure(configFile, false);
            else
                MessageBox.Show("Missing config file:\r\n" + configFile);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Allberg.Shooter.WinShooterServer.ClientInterface),
                "WinShooterServer",
                WellKnownObjectMode.Singleton);
        }

        private void ServerStartupOpenFile()
        {
            // Connect to server and open file
            Trace.WriteLine("FMain: Starting local client.");

            HttpChannel clientchan = new HttpChannel(0);
            ChannelServices.RegisterChannel(clientchan, false);
            myInterface = (Allberg.Shooter.WinShooterServer.ClientInterface)
                Activator.GetObject(
                typeof(Allberg.Shooter.WinShooterServer.ClientInterface),
                "http://localhost:" + ServerPort.ToString() + "/WinShooterServer");

            // Open database
            Trace.WriteLine("FMain: using local client to open file: \"" +
                this.openFileDialog1.FileName + "\"");

            myInterface.OpenAccessDatabase(this.openFileDialog1.FileName);
            myInterface.OpenDatabase();
            this.txtFilename.Text = this.openFileDialog1.FileName;

            Trace.WriteLine("FMain: displaying server address.");

            // Display server address
            IPHostEntry serverHost =
                Dns.GetHostEntry(Dns.GetHostName());
            string addresses = "";
            foreach (IPAddress address in serverHost.AddressList)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    if (addresses.Length > 0)
                        addresses += ", ";

                    addresses += address.ToString();
                }
            }
            Trace.WriteLine("FMain: Serveraddresses: " + addresses);
            this.BeginInvoke(FMainSetServerAddressInvoker, new object[] { addresses });

            // fixup timers
            Trace.WriteLine("FMain: ServerStartup: Disabling/Enabling timers from thread \"" +
                System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            this.BeginInvoke(StartupTimersInvoker);
        }

        private void ServerStartupUdpListener()
        {
            // Start up UDP responder
            udpProcessor = new UdpProcessor(myInterface.GetCompetitions()[0].Name, ServerPort);
            UdpThread = new Thread(new ThreadStart(udpProcessor.Start));
            UdpThread.IsBackground = true;
            UdpThread.Start();
        }

        UdpProcessor udpProcessor;
        Thread UdpThread;

        private void StartUpTimers()
        {
            Trace.WriteLine("FMain: StartUpTimers started from thread \"" + 
                System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            timerClientsUpdate.Enabled = true;
            timerKeepConnection.Enabled = true;
            timerClientsUpdate.Enabled = true;

            Trace.WriteLine("FMain: StartUpTimers ended.");
        }

        private void FMainSetServerAddress(string addresses)
        {
            Trace.WriteLine("FMain: FMainSetServerAddress started from thread \"" + 
                System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());
            this.txtServerAddress.Text = addresses;
            Trace.WriteLine("FMain: FMainSetServerAddress ended");
        }


        Allberg.Shooter.WinShooterServer.ClientInterface myInterface;

        private string configFile
        {
            get
            {
                string filename = "";
                System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
                filename = System.IO.Path.GetDirectoryName(ass.Location) + "\\Server.config";
                
                return filename;
            }
        }

        internal bool UnhandledExceptionOccurred = false;
        void FMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UnhandledExceptionOccurred)
            {
                Visible = false;
                return;
            }

            DialogResult res = 
                MessageBox.Show("Är du säker på att du vill stänga av servern?", 
                "Stänga av", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question, 
                MessageBoxDefaultButton.Button2);

            if (res == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                try
                {
                    backupWindow.DisposeNow = true;
                }
                catch (Exception)
                {
                }
                try
                {
                    UdpThread.Abort();
                }
                catch(Exception exc)
                {
                    Trace.WriteLine("Exception while stopping UdpThread:" + exc.ToString());
                }
                if (components != null) 
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.datasetClients1 = new Allberg.Shooter.WinShooterServer.DatasetClients();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxIPAddress = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxLastUpdate = new System.Windows.Forms.DataGridTextBoxColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnDispose = new System.Windows.Forms.Button();
            this.timerClientsUpdate = new System.Windows.Forms.Timer(this.components);
            this.chkWebServer = new System.Windows.Forms.CheckBox();
            this.timerKeepConnection = new System.Windows.Forms.Timer(this.components);
            this.numCasheSeconds = new System.Windows.Forms.NumericUpDown();
            this.progressBarThreads = new System.Windows.Forms.ProgressBar();
            this.timerThreadCount = new System.Windows.Forms.Timer(this.components);
            this.numHttpPort = new System.Windows.Forms.NumericUpDown();
            this.btnBackup = new System.Windows.Forms.Button();
            this.label4 = new Allberg.Shooter.Windows.Forms.SafeLabel();
            this.label3 = new Allberg.Shooter.Windows.Forms.SafeLabel();
            this.txtServerAddress = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.txtFilename = new Allberg.Shooter.Windows.Forms.SafeTextBox();
            this.label2 = new Allberg.Shooter.Windows.Forms.SafeLabel();
            this.label1 = new Allberg.Shooter.Windows.Forms.SafeLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datasetClients1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCasheSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHttpPort)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrid1
            // 
            this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid1.CaptionText = "Anslutna klienter";
            this.dataGrid1.DataMember = "Clients";
            this.dataGrid1.DataSource = this.datasetClients1;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(12, 88);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.RowHeadersVisible = false;
            this.dataGrid1.Size = new System.Drawing.Size(576, 191);
            this.dataGrid1.TabIndex = 2;
            this.dataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            // 
            // datasetClients1
            // 
            this.datasetClients1.DataSetName = "DatasetClients";
            this.datasetClients1.Locale = new System.Globalization.CultureInfo("en-US");
            this.datasetClients1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = this.dataGrid1;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dataGridTextBoxIPAddress,
            this.dataGridTextBoxLastUpdate});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "Clients";
            this.dataGridTableStyle1.PreferredColumnWidth = 125;
            this.dataGridTableStyle1.ReadOnly = true;
            this.dataGridTableStyle1.RowHeadersVisible = false;
            // 
            // dataGridTextBoxIPAddress
            // 
            this.dataGridTextBoxIPAddress.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.dataGridTextBoxIPAddress.Format = "";
            this.dataGridTextBoxIPAddress.FormatInfo = null;
            this.dataGridTextBoxIPAddress.HeaderText = "Klient";
            this.dataGridTextBoxIPAddress.MappingName = "IPAddress";
            this.dataGridTextBoxIPAddress.NullText = "";
            this.dataGridTextBoxIPAddress.ReadOnly = true;
            this.dataGridTextBoxIPAddress.Width = 125;
            // 
            // dataGridTextBoxLastUpdate
            // 
            this.dataGridTextBoxLastUpdate.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.dataGridTextBoxLastUpdate.Format = "";
            this.dataGridTextBoxLastUpdate.FormatInfo = null;
            this.dataGridTextBoxLastUpdate.HeaderText = "Senast anslutning";
            this.dataGridTextBoxLastUpdate.MappingName = "LastUpdate";
            this.dataGridTextBoxLastUpdate.NullText = "";
            this.dataGridTextBoxLastUpdate.ReadOnly = true;
            this.dataGridTextBoxLastUpdate.Width = 150;
            // 
            // btnDispose
            // 
            this.btnDispose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDispose.Location = new System.Drawing.Point(496, 287);
            this.btnDispose.Name = "btnDispose";
            this.btnDispose.Size = new System.Drawing.Size(88, 23);
            this.btnDispose.TabIndex = 3;
            this.btnDispose.Text = "Avsluta server";
            this.btnDispose.Click += new System.EventHandler(this.btnDispose_Click);
            // 
            // timerClientsUpdate
            // 
            this.timerClientsUpdate.Interval = 5000;
            this.timerClientsUpdate.Tick += new System.EventHandler(this.timerClientsUpdate_Tick);
            // 
            // chkWebServer
            // 
            this.chkWebServer.Location = new System.Drawing.Point(96, 58);
            this.chkWebServer.Name = "chkWebServer";
            this.chkWebServer.Size = new System.Drawing.Size(152, 24);
            this.chkWebServer.TabIndex = 6;
            this.chkWebServer.Text = "Kör som webbserver med";
            this.chkWebServer.CheckedChanged += new System.EventHandler(this.chkWebServer_CheckedChanged);
            // 
            // timerKeepConnection
            // 
            this.timerKeepConnection.Interval = 60000;
            this.timerKeepConnection.Tick += new System.EventHandler(this.timerKeepConnection_Tick);
            // 
            // numCasheSeconds
            // 
            this.numCasheSeconds.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCasheSeconds.Location = new System.Drawing.Point(248, 58);
            this.numCasheSeconds.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numCasheSeconds.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCasheSeconds.Name = "numCasheSeconds";
            this.numCasheSeconds.Size = new System.Drawing.Size(56, 20);
            this.numCasheSeconds.TabIndex = 7;
            this.numCasheSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numCasheSeconds.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // progressBarThreads
            // 
            this.progressBarThreads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarThreads.Location = new System.Drawing.Point(120, 287);
            this.progressBarThreads.Name = "progressBarThreads";
            this.progressBarThreads.Size = new System.Drawing.Size(368, 23);
            this.progressBarThreads.TabIndex = 9;
            this.progressBarThreads.Click += new System.EventHandler(this.progressBarThreads_Click);
            // 
            // timerThreadCount
            // 
            this.timerThreadCount.Enabled = true;
            this.timerThreadCount.Tick += new System.EventHandler(this.timerThreadCount_Tick);
            // 
            // numHttpPort
            // 
            this.numHttpPort.Location = new System.Drawing.Point(441, 58);
            this.numHttpPort.Maximum = new decimal(new int[] {
            89,
            0,
            0,
            0});
            this.numHttpPort.Minimum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numHttpPort.Name = "numHttpPort";
            this.numHttpPort.Size = new System.Drawing.Size(56, 20);
            this.numHttpPort.TabIndex = 11;
            this.numHttpPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numHttpPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(8, 58);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(75, 23);
            this.btnBackup.TabIndex = 12;
            this.btnBackup.Text = "Backup";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(8, 287);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 10;
            this.label4.Text = "Antal trådar";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(310, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "sekunders cache på port";
            // 
            // txtServerAddress
            // 
            this.txtServerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerAddress.Location = new System.Drawing.Point(96, 32);
            this.txtServerAddress.Name = "txtServerAddress";
            this.txtServerAddress.ReadOnly = true;
            this.txtServerAddress.Size = new System.Drawing.Size(488, 20);
            this.txtServerAddress.TabIndex = 5;
            // 
            // txtFilename
            // 
            this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename.Location = new System.Drawing.Point(96, 8);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(488, 20);
            this.txtFilename.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Server adress";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Databas";
            // 
            // FMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(592, 317);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.numHttpPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.progressBarThreads);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numCasheSeconds);
            this.Controls.Add(this.chkWebServer);
            this.Controls.Add(this.txtServerAddress);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDispose);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FMain";
            this.Text = "WinShooter Server";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datasetClients1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCasheSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHttpPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private void btnDispose_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }


        private readonly object updatingClients = new object();
        private void timerClientsUpdate_Tick(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: timerClientsUpdate_Tick started from thread \"" + 
                System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            try
            {
                lock(updatingClients)
                {
                    Hashtable clients = myInterface.SyncClients();
                    datasetClients1.Clients.Clear();
                    foreach(object obj in clients.Keys)
                    {
                        DatasetClients.ClientsRow row = datasetClients1.Clients.NewClientsRow();
                        row.IPAddress = obj.ToString();
                        row.LastUpdate = ((DateTime)clients[obj]).ToLongTimeString();
                        TimeSpan diff = DateTime.Now - (DateTime)clients[obj];
                        if (diff.TotalMinutes < 10)
                            datasetClients1.Clients.AddClientsRow(row);
                    }
                    this.dataGrid1.DataSource = datasetClients1;
                    this.dataGrid1.DataMember = "Clients";
                }
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FMain: timerClientsUpdate_Tick Exception:" + exc.ToString());
            }
        }

        private void timerKeepConnection_Tick(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: timerKeepConnection_Tick started from thread \"" + 
                System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());
            try
            {
                myInterface.Sync(true);
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FMain: timerKeepConnection_Tick Exception:" + exc.ToString());
            }
        }

        #region EvaluationCode
        internal string AssemblyVersion = "";
        private void getAssemblyVersion()
        {
            AssemblyVersion = getAssemblyVersion(4);
        }
        private string getAssemblyVersion(int nrOfNrs)
        {
            // Get running assembly version
            System.Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            string assemblyVersion = ver.Major.ToString();
            if (!(ver.Minor == 0 &
                ver.Build == 0 &
                ver.Revision == 0))
            {
                assemblyVersion += "." +
                    ver.Minor.ToString();
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
            }
            return assemblyVersion;
        }

        #endregion

        #region WebServer
        HttpServer httpServer;
        Thread httpThread;
        private void chkWebServer_CheckedChanged(object sender, System.EventArgs e)
        {
            Trace.WriteLine("FMain: chkWebServer_CheckedChanged started from thread \"" + 
                System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            if (this.chkWebServer.Checked)
            {
                Trace.WriteLine("FMain: WebServer is to be started.");


                httpServer = new HttpServer(
                    ref myInterface, 
                    new TimeSpan(0,0,0,(int)numCasheSeconds.Value),
                    (int)numHttpPort.Value);
                httpThread = new Thread(
                    new System.Threading.ThreadStart(httpServer.listen));
                httpThread.Name = "HttpServer";
                httpThread.Start();
            }
            else
            {
                Trace.WriteLine("FMain: WebServer is to be shut down.");

                if (httpThread != null)
                {
                    httpThread.Abort();
                }
                httpServer.Shutdown();
            }
        }
        #endregion

        DateTime lastThreadWarning = new DateTime(2001,1,1,0,0,0);
        private void timerThreadCount_Tick(object sender, System.EventArgs e)
        {
            int workerThreadsMax = 0;
            int portThreadsMax = 0;
            ThreadPool.GetMaxThreads(out workerThreadsMax, out portThreadsMax);
            progressBarThreads.Maximum = workerThreadsMax;

            int workerThreadsFree = 0;
            int portThreadsFree = 0;
            ThreadPool.GetAvailableThreads(out workerThreadsFree, out portThreadsFree);
            progressBarThreads.Value = workerThreadsMax-workerThreadsFree;

            if (workerThreadsFree < 5)
            {
                if ((DateTime.Now - lastThreadWarning).TotalSeconds > 5)
                {
                    Trace.WriteLine("Warning! The number of free threads in the ThreadPool is now " +
                        workerThreadsFree.ToString());
                    lastThreadWarning = DateTime.Now;
                }
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            backupWindow.EnableMe();
        }

        internal string CurrentFilename
        {
            get
            {
                return openFileDialog1.FileName;
            }
        }

        private void progressBarThreads_Click(object sender, EventArgs e)
        {
#if DEBUG
            throw new ApplicationException("This is a test");
#endif
        }

    }
}
