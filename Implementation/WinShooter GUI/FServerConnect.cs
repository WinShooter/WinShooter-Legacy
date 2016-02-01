// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FServerConnect.cs" company="John Allberg">
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
//   Summary description for FServerConnect.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.Windows
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using Allberg.Shooter.Windows.Forms;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for FServerConnect.
    /// </summary>
    public class FServerConnect : System.Windows.Forms.Form
    {
        private Forms.SafeLabel _safeLabel1;
        private Forms.SafeButton _btnConnect;
        private Forms.SafeButton _btnCancel;
        private Forms.SafeComboBox _ddServerAddress;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer _timerSearchServer;
        private System.Windows.Forms.Label _label1;
        private System.Windows.Forms.Label _lblCompetition;

        public delegate void ConnectToServerHandler(string hostname);
        public event ConnectToServerHandler ConnectToServer;
        public delegate void HandleServerUdpHandler(string servermessage);
        public event HandleServerUdpHandler HandleServerUdp;
        public delegate void EnableMainHandler();
        public event EnableMainHandler EnableMain;
        internal bool DisposeNow;

        public FServerConnect()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            Trace.WriteLine("FServerConnect: Creating.");
            Trace.WriteLine("FServerConnect: Created.");
            HandleServerUdp += handleServerUdp;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FServerConnect: Dispose(" + disposing + ")" +
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
                Trace.WriteLine("FServerConnect: exception while disposing:" + exc);
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

            Trace.WriteLine("FServerConnect: Dispose(" + disposing + ")" +
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FServerConnect));
            this._safeLabel1 = new SafeLabel();
            this._btnConnect = new SafeButton();
            this._btnCancel = new SafeButton();
            this._ddServerAddress = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this._timerSearchServer = new System.Windows.Forms.Timer(this.components);
            this._label1 = new System.Windows.Forms.Label();
            this._lblCompetition = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SafeLabel1
            // 
            this._safeLabel1.Location = new System.Drawing.Point(8, 8);
            this._safeLabel1.Name = "_safeLabel1";
            this._safeLabel1.Size = new System.Drawing.Size(64, 23);
            this._safeLabel1.TabIndex = 0;
            this._safeLabel1.Text = "Server";
            // 
            // btnConnect
            // 
            this._btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnConnect.Location = new System.Drawing.Point(128, 64);
            this._btnConnect.Name = "_btnConnect";
            this._btnConnect.TabIndex = 1;
            this._btnConnect.Text = "Anslut";
            this._btnConnect.Click += new System.EventHandler(this.BtnConnectClick);
            // 
            // btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(208, 64);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Avbryt";
            this._btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // ddServerAddress
            // 
            this._ddServerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this._ddServerAddress.Location = new System.Drawing.Point(72, 8);
            this._ddServerAddress.Name = "_ddServerAddress";
            this._ddServerAddress.Size = new System.Drawing.Size(208, 21);
            this._ddServerAddress.TabIndex = 3;
            this._ddServerAddress.SelectedIndexChanged += new System.EventHandler(this.DdServerAddressSelectedIndexChanged);
            // 
            // timerSearchServer
            // 
            this._timerSearchServer.Interval = 500;
            this._timerSearchServer.Tick += new System.EventHandler(this.TimerSearchServerTick);
            // 
            // label1
            // 
            this._label1.Location = new System.Drawing.Point(8, 32);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(64, 23);
            this._label1.TabIndex = 4;
            this._label1.Text = "Tävling";
            // 
            // lblCompetition
            // 
            this._lblCompetition.Location = new System.Drawing.Point(72, 32);
            this._lblCompetition.Name = "_lblCompetition";
            this._lblCompetition.Size = new System.Drawing.Size(208, 23);
            this._lblCompetition.TabIndex = 5;
            // 
            // FServerConnect
            // 
            this.AcceptButton = this._btnConnect;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(288, 94);
            this.Controls.Add(this._lblCompetition);
            this.Controls.Add(this._label1);
            this.Controls.Add(this._ddServerAddress);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnConnect);
            this.Controls.Add(this._safeLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FServerConnect";
            this.Text = "Anslut till server";
            this.ResumeLayout(false);

        }
        #endregion

        internal void EnableMe()
        {
            Visible = true;
            Focus();
            _ddServerAddress.Items.Clear();
            _ddServerAddress.Focus();
            _servers = new Hashtable();

            var udpClientThread = 
                new Thread(CollectServersViaUdp) {IsBackground = true};
            udpClientThread.Start();

            Thread.Sleep(500);
            _timerSearchServer.Enabled = true;
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Visible = false;
            _timerSearchServer.Enabled = false;
            EnableMain();
        }

        private void BtnConnectClick(object sender, EventArgs e)
        {
            _timerSearchServer.Enabled = false;
            var hostname = _ddServerAddress.Text;
            if (hostname == "")
            {
                hostname = "localhost";
            }
            Visible = false;
            try
            {
                ConnectToServer(hostname);
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }
        }

        
        private static void SendUdpQuery()
        {
            try
            {
                //localHostEntry = new IPHostEntry();
                //localHostEntry = Dns.GetHostEntry(Dns.GetHostName());

                var clientSock = new Socket(AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Udp);

                clientSock.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.Broadcast,
                    1);
                var ipaddress = IPAddress.Broadcast;
                var iep = new IPEndPoint(ipaddress, NetworkSettings.GroupPortServer);
                const string strToSend = "Message=WinShooter Server Requested;";

                var data = Encoding.UTF8.GetBytes(strToSend); // put your data here
                Trace.WriteLine("Sending Data: \"" + strToSend + "\"");
                try
                {
                    clientSock.SendTo(data, iep);
                }
                catch (Exception exc)
                {
                    Trace.WriteLine("Exception while trying to broadcast for server: " + exc);
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLine("Exception while preparing to broadcast for server: " + exc);
            }
        }

        private void CollectServersViaUdp()
        {
            try
            {
                Trace.WriteLine("Client: Preparing UDP Broadcast Server Collector");
                //localHostEntry = new IPHostEntry();
                var localHostEntry = Dns.GetHostEntry(Dns.GetHostName());

                //Create a UDP socket.
                var soUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                soUdp.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.Broadcast,
                    1);
            
                var localIpEndPoint = 
                    new IPEndPoint(new IPAddress( new byte[] {0,0,0,0}),
                    NetworkSettings.GroupPortServer);
                
                soUdp.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.ReuseAddress, 1);
                soUdp.Bind(localIpEndPoint);

                Trace.WriteLine("FServerConnect: Starting UDP Broadcast Server Collector" + localHostEntry.AddressList[0]);
                while (true)
                {
                    var received = new Byte[512];
                    var tmpIpEndPoint = new IPEndPoint(
                        localHostEntry.AddressList[0], 
                        NetworkSettings.GroupPortServer);

                    EndPoint remoteEp = (tmpIpEndPoint);

                    Trace.WriteLine("ClientUDP: Waiting to receive UDP message.");
                    int bytesReceived = soUdp.ReceiveFrom(received, ref remoteEp);

                    string receivedString = Encoding.UTF8.GetString(received, 0, bytesReceived);

                    Trace.WriteLine("ClientUDP: " + bytesReceived + " bytes received: \"" +
                        receivedString + "\"");

                    if (!(receivedString.IndexOf("WinShooter Server") > -1 &
                          receivedString.IndexOf("Requested") == -1)) 
                        continue;

                    // Process udp on main thread
                    if (InvokeRequired)
                        Invoke(HandleServerUdp, new object[] { receivedString });
                    else
                        HandleServerUdp(receivedString);
                }
            }
            catch (SocketException se)
            {
                Trace.WriteLine("A Socket Exception has occurred!" + se);
                //throw;
            }
        }

        Hashtable _servers = new Hashtable();
        private void handleServerUdp(string servermessage)
        {
            try
            {
                var ip = GetMessagePart("IP", servermessage) + ":" +
                    GetMessagePart("Port", servermessage);
                var competition = GetMessagePart("Competition", servermessage);
                if (!_servers.ContainsKey(ip))
                {
                    _servers.Add(ip, competition);
                    _ddServerAddress.Items.Add(ip);
                    if (_ddServerAddress.SelectedIndex == -1)
                    {
                        _ddServerAddress.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLine("FServerConnect: Exception while handling ServerUDP: " +
                    exc);
            }
        }

        private static string GetMessagePart(string part, string message)
        {
            int start = message.IndexOf(part);
            if (start == -1)
                return "";

            start += part.Length +1;
            int last = message.IndexOf(";", start);
            if (last == -1)
                last = message.Length;
            string thisPart = message.Substring(start, last-start);
            return thisPart;
        }

        private void TimerSearchServerTick(object sender, EventArgs e)
        {
            SendUdpQuery();
        }

        private void DdServerAddressSelectedIndexChanged(object sender, EventArgs e)
        {
            var ip = (string)_ddServerAddress.SelectedItem;
            var competition = (string)_servers[ip];
            _lblCompetition.Text = competition ?? "";
        }
    }
}
