// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FServerConnect.cs" company="John Allberg">
//   Copyright ©2001-2016 John Allberg
//   //   This program is free software; you can redistribute it and/or
//   //   modify it under the terms of the GNU General Public License
//   //   as published by the Free Software Foundation; either version 2
//   //   of the License, or (at your option) any later version.
//   //   This program is distributed in the hope that it will be useful,
//   //   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   //   MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
//   //   GNU General Public License for more details.
//   //   You should have received a copy of the GNU General Public License
//   //   along with this program; if not, write to the Free Software
//   //   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allberg.Shooter.Windows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Resources;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    using Allberg.Shooter.Windows.Forms;
    using Allberg.Shooter.WinShooterServerRemoting;

    using Timer = System.Windows.Forms.Timer;

    /// <summary>
    /// Summary description for FServerConnect.
    /// </summary>
    public class FServerConnect : Form
    {
        /// <summary>
        /// The _safe label 1.
        /// </summary>
        private SafeLabel _safeLabel1;

        /// <summary>
        /// The _btn connect.
        /// </summary>
        private SafeButton _btnConnect;

        /// <summary>
        /// The _btn cancel.
        /// </summary>
        private SafeButton _btnCancel;

        /// <summary>
        /// The _dd server address.
        /// </summary>
        private SafeComboBox _ddServerAddress;

        /// <summary>
        /// The components.
        /// </summary>
        private IContainer components;

        /// <summary>
        /// The _timer search server.
        /// </summary>
        private Timer _timerSearchServer;

        /// <summary>
        /// The _label 1.
        /// </summary>
        private Label _label1;

        /// <summary>
        /// The _lbl competition.
        /// </summary>
        private Label _lblCompetition;

        /// <summary>
        /// The connect to server handler.
        /// </summary>
        /// <param name="hostname">
        /// The hostname.
        /// </param>
        public delegate void ConnectToServerHandler(string hostname);

        /// <summary>
        /// The connect to server.
        /// </summary>
        public event ConnectToServerHandler ConnectToServer;

        /// <summary>
        /// The handle server udp handler.
        /// </summary>
        /// <param name="servermessage">
        /// The servermessage.
        /// </param>
        public delegate void HandleServerUdpHandler(string servermessage);

        /// <summary>
        /// The handle server udp.
        /// </summary>
        public event HandleServerUdpHandler HandleServerUdp;

        /// <summary>
        /// The enable main handler.
        /// </summary>
        public delegate void EnableMainHandler();

        /// <summary>
        /// The enable main.
        /// </summary>
        public event EnableMainHandler EnableMain;

        /// <summary>
        /// Initializes a new instance of the <see cref="FServerConnect"/> class.
        /// </summary>
        public FServerConnect()
        {
            this.InitializeComponent();
            Trace.WriteLine("FServerConnect: Creating.");
            Trace.WriteLine("FServerConnect: Created.");
            this.HandleServerUdp += this.handleServerUdp;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose( bool disposing )
        {
            Trace.WriteLine("FServerConnect: Dispose(" + disposing + ")" +
                "from thread \"" + Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                DateTime.Now.ToLongTimeString());

            Visible = false;
            try
            {
                this.EnableMain();
            }
            catch(Exception exc)
            {
                Trace.WriteLine("FServerConnect: exception while disposing:" + exc);
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ResourceManager resources = new System.Resources.ResourceManager(typeof(FServerConnect));
            this._safeLabel1 = new SafeLabel();
            this._btnConnect = new SafeButton();
            this._btnCancel = new SafeButton();
            this._ddServerAddress = new Allberg.Shooter.Windows.Forms.SafeComboBox();
            this._timerSearchServer = new System.Windows.Forms.Timer(this.components);
            this._label1 = new System.Windows.Forms.Label();
            this._lblCompetition = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // SafeLabel1
            this._safeLabel1.Location = new System.Drawing.Point(8, 8);
            this._safeLabel1.Name = "_safeLabel1";
            this._safeLabel1.Size = new System.Drawing.Size(64, 23);
            this._safeLabel1.TabIndex = 0;
            this._safeLabel1.Text = "Server";

            // btnConnect
            this._btnConnect.Anchor =
                (System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this._btnConnect.Location = new System.Drawing.Point(128, 64);
            this._btnConnect.Name = "_btnConnect";
            this._btnConnect.TabIndex = 1;
            this._btnConnect.Text = "Anslut";
            this._btnConnect.Click += new System.EventHandler(this.BtnConnectClick);

            // btnCancel
            this._btnCancel.Anchor =
                (System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(208, 64);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Avbryt";
            this._btnCancel.Click += new System.EventHandler(this.BtnCancelClick);

            // ddServerAddress
            this._ddServerAddress.Anchor =
                (System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right));
            this._ddServerAddress.Location = new System.Drawing.Point(72, 8);
            this._ddServerAddress.Name = "_ddServerAddress";
            this._ddServerAddress.Size = new System.Drawing.Size(208, 21);
            this._ddServerAddress.TabIndex = 3;
            this._ddServerAddress.SelectedIndexChanged +=
                new System.EventHandler(this.DdServerAddressSelectedIndexChanged);

            // timerSearchServer
            this._timerSearchServer.Interval = 500;
            this._timerSearchServer.Tick += new System.EventHandler(this.TimerSearchServerTick);

            // label1
            this._label1.Location = new System.Drawing.Point(8, 32);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(64, 23);
            this._label1.TabIndex = 4;
            this._label1.Text = "Tävling";

            // lblCompetition
            this._lblCompetition.Location = new System.Drawing.Point(72, 32);
            this._lblCompetition.Name = "_lblCompetition";
            this._lblCompetition.Size = new System.Drawing.Size(208, 23);
            this._lblCompetition.TabIndex = 5;

            // FServerConnect
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
            this.Icon = (System.Drawing.Icon)(resources.GetObject("$this.Icon"));
            this.Name = "FServerConnect";
            this.Text = "Anslut till server";
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// The enable me.
        /// </summary>
        internal void EnableMe()
        {
            this.Visible = true;
            this.Focus();
            this._ddServerAddress.Items.Clear();
            this._ddServerAddress.Focus();
            this.servers = new Dictionary<string, string>();

            var udpClientThread = 
                new Thread(this.CollectServersViaUdp) {IsBackground = true};
            udpClientThread.Start();

            Thread.Sleep(500);
            this._timerSearchServer.Enabled = true;
        }

        /// <summary>
        /// The btn cancel click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnCancelClick(object sender, EventArgs e)
        {
            this.Visible = false;
            this._timerSearchServer.Enabled = false;
            this.EnableMain();
        }

        /// <summary>
        /// The btn connect click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnConnectClick(object sender, EventArgs e)
        {
            _timerSearchServer.Enabled = false;
            var hostname = _ddServerAddress.Text;
            if (hostname == string.Empty)
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

        /// <summary>
        /// The send udp query.
        /// </summary>
        private static void SendUdpQuery()
        {
            try
            {
                // localHostEntry = new IPHostEntry();
                // localHostEntry = Dns.GetHostEntry(Dns.GetHostName());
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

        /// <summary>
        /// The collect servers via udp.
        /// </summary>
        private void CollectServersViaUdp()
        {
            try
            {
                Trace.WriteLine("Client: Preparing UDP Broadcast Server Collector");

                // localHostEntry = new IPHostEntry();
                var localHostEntry = Dns.GetHostEntry(Dns.GetHostName());

                // Create a UDP socket.
                var soUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                soUdp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                var localIpEndPoint = new IPEndPoint(
                    new IPAddress(new byte[] { 0, 0, 0, 0 }), 
                    NetworkSettings.GroupPortServer);

                soUdp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                soUdp.Bind(localIpEndPoint);

                Trace.WriteLine(
                    "FServerConnect: Starting UDP Broadcast Server Collector" + localHostEntry.AddressList[0]);
                while (true)
                {
                    var received = new byte[512];
                    var tmpIpEndPoint = new IPEndPoint(localHostEntry.AddressList[0], NetworkSettings.GroupPortServer);

                    EndPoint remoteEp = tmpIpEndPoint;

                    Trace.WriteLine("ClientUDP: Waiting to receive UDP message.");
                    int bytesReceived = soUdp.ReceiveFrom(received, ref remoteEp);

                    string receivedString = Encoding.UTF8.GetString(received, 0, bytesReceived);

                    Trace.WriteLine("ClientUDP: " + bytesReceived + " bytes received: \"" + receivedString + "\"");

                    if (!(receivedString.IndexOf("WinShooter Server") > -1 & receivedString.IndexOf("Requested") == -1))
                    {
                        continue;
                    }

                    // Process udp on main thread
                    if (InvokeRequired)
                    {
                        Invoke(HandleServerUdp, new object[] { receivedString });
                    }
                    else
                    {
                        HandleServerUdp(receivedString);
                    }
                }
            }
            catch (SocketException se)
            {
                Trace.WriteLine("A Socket Exception has occurred!" + se);

                // throw;
            }
        }

        /// <summary>
        /// The servers.
        /// </summary>
        private Dictionary<string, string> servers = new Dictionary<string, string>();

        /// <summary>
        /// The handle server udp.
        /// </summary>
        /// <param name="servermessage">
        /// The servermessage.
        /// </param>
        private void handleServerUdp(string servermessage)
        {
            try
            {
                var ip = GetMessagePart("IP", servermessage) + ":" +
                    GetMessagePart("Port", servermessage);
                var competition = GetMessagePart("Competition", servermessage);
                if (this.servers.ContainsKey(ip))
                {
                    return;
                }

                this.servers.Add(ip, competition);
                this._ddServerAddress.Items.Add(ip);
                if (this._ddServerAddress.SelectedIndex == -1)
                {
                    this._ddServerAddress.SelectedIndex = 0;
                }
            }
            catch (Exception exc)
            {
                Trace.WriteLine("FServerConnect: Exception while handling ServerUDP: " +
                    exc);
            }
        }

        /// <summary>
        /// The get message part.
        /// </summary>
        /// <param name="part">
        /// The part.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetMessagePart(string part, string message)
        {
            int start = message.IndexOf(part);
            if (start == -1)
                return string.Empty;

            start += part.Length +1;
            int last = message.IndexOf(";", start);
            if (last == -1)
                last = message.Length;
            string thisPart = message.Substring(start, last-start);
            return thisPart;
        }

        /// <summary>
        /// The timer search server tick.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TimerSearchServerTick(object sender, EventArgs e)
        {
            SendUdpQuery();
        }

        /// <summary>
        /// The dd server address selected index changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DdServerAddressSelectedIndexChanged(object sender, EventArgs e)
        {
            var ip = (string)_ddServerAddress.SelectedItem;
            var competition = (string)this.servers[ip];
            _lblCompetition.Text = competition ?? string.Empty;
        }
    }
}
