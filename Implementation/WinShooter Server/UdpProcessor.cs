// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UdpProcessor.cs" company="John Allberg">
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
//   Summary description for UdpProcessor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServer
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using Allberg.Shooter.WinShooterServerRemoting;

    /// <summary>
    /// Summary description for UdpProcessor.
    /// </summary>
    public class UdpProcessor
    {
        readonly IPHostEntry _localHostEntry;

        public UdpProcessor(string competitionName, int serverPort)
        {
            _localHostEntry = new IPHostEntry();

            try
            {
                _localHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            }
            catch(Exception exc)
            {
                throw new ApplicationException("Local Host not found", exc); // fail
            }
            _competitionName = competitionName;
            _winshooterServerPort = serverPort;
        }

        readonly int _winshooterServerPort;
        readonly string _competitionName;
        Socket _socketUdp;
        EndPoint _remoteEp;

        public void Start()
        {

            try
            {
                //Create a UDP socket.
                _socketUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _socketUdp.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.Broadcast,
                    1);
            
                var localIpEndPoint = new IPEndPoint(
                    new IPAddress(new byte[] { 0,0,0,0 }),
                    NetworkSettings.GroupPortServer);

                Debug.WriteLine("Will bind to localIpEndPoint: " + localIpEndPoint);

                _socketUdp.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.ReuseAddress, 1);
                _socketUdp.Bind(localIpEndPoint);
            
                Trace.WriteLine("Server: Starting UDP Broadcast Server");
                while (true)
                {
                    var received = new Byte[512];
                    var tmpIpEndPoint = new IPEndPoint(
                        _localHostEntry.AddressList[0], 
                        NetworkSettings.GroupPortServer);

                    _remoteEp = (tmpIpEndPoint);

                    Trace.WriteLine("ServerUDP: Waiting to receive UDP message.");
                    var bytesReceived = _socketUdp.ReceiveFrom(received, ref _remoteEp);

                    var receivedString = Encoding.UTF8.GetString(received, 0, bytesReceived);

                    Trace.WriteLine("ServerUDP: " + bytesReceived + " bytes received: \"" +
                        receivedString + "\"");

                    if (receivedString.IndexOf("WinShooter Server Requested") > -1)
                    {
                        // We have received a request for a WinShooter Server. Respond
                        SendServerResponse();
                    }
                }
            }
            catch (SocketException se)
            {
                Trace.WriteLine("A Socket Exception has occurred!" + se);
                throw;
            }
        }

        private void SendServerResponse()
        {
            var localHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var address in localHostEntry.AddressList)
            {
                if (address.AddressFamily != AddressFamily.InterNetwork) 
                    continue;

                var clientSock = new Socket(AddressFamily.InterNetwork,
                                               SocketType.Dgram,
                                               ProtocolType.Udp);
                clientSock.SetSocketOption(SocketOptionLevel.Socket,
                                           SocketOptionName.Broadcast,
                                           1);
                var iep = new IPEndPoint(IPAddress.Broadcast, NetworkSettings.GroupPortServer);
                var strToSend = new StringBuilder();
                strToSend.Append("Message=WinShooter Server;");
                strToSend.Append("IP=" + address + ";");
                strToSend.Append("Port=" + _winshooterServerPort + ";");
                strToSend.Append("Competition=" + _competitionName + ";");

                var data = Encoding.UTF8.GetBytes(strToSend.ToString()); // put your data here
                Trace.WriteLine("Sending Data: \"" + strToSend + "\"");

                clientSock.SendTo(data, iep);
            }
        }
    }
}
