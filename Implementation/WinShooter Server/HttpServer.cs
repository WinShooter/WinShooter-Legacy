// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpServer.cs" company="John Allberg">
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
//   Summary description for HttpServer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServer
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// Summary description for HttpServer.
    /// </summary>
    internal class HttpServer
    {
        // ============================================================
        // Data

        public static bool verbose = false;
        private int port;
        ClientInterface commonCode;
        TimeSpan cacheTime;
 
        // ============================================================
        // Constructor

        internal HttpServer(ref ClientInterface CommonCode, TimeSpan CacheTime, int ServerPort) 
        {
            Trace.WriteLine("HttpServer: Creating in thread \"" + 
                System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            this.port = ServerPort;
            commonCode = CommonCode;
            cacheTime = CacheTime;
        }

        // ============================================================
        // Listener
  
        Socket listener;
        public void listen() 
        {
            Trace.WriteLine("HttpServer: Listen() started from thread \"" + 
                System.Threading.Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                DateTime.Now.ToLongTimeString());

            // Create a new server socket, set up all the endpoints, bind the socket and then listen
            try
            {
                listener = new Socket(0, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipaddress = IPAddress.Parse("0.0.0.0");
                IPEndPoint endpoint = new IPEndPoint(ipaddress, port);
                listener.Bind(endpoint);
                listener.Blocking = true;
                listener.Listen(100);
            }
            catch (System.Net.Sockets.SocketException exc)
            {
                Trace.WriteLine("HttpServer: Exception occured while binding: " + exc.ToString());
                System.Windows.Forms.MessageBox.Show("Det verkar redan finnas en servern på port " + port.ToString() + ". Stäng av webbservern och välj en annan port.");
                return;
            }
            Trace.WriteLine("Http server listening on port " + port);
            try 
            {
                // Accept a new connection from the net, blocking till one comes in
                Trace.WriteLine("HttpServer: Listen().BeginAccept.");

                listener.BeginAccept(new AsyncCallback(startProcess), listener);
            } 
            catch(NullReferenceException exc) 
            {
                Trace.WriteLine("HttpServer: Listen NullReferenceException: " + 
                    exc.ToString());

                // Don't even ask me why they throw this exception when this happens
                Trace.WriteLine("Accept failed.  Another process might be bound to port " + port);
                throw;
            }
            catch(Exception exc)
            {
                Trace.WriteLine("HttpServer: Listen Exception" +
                    exc.ToString());
            }
        }	

        private void startProcess(System.IAsyncResult res)
        {
            try
            {
                Trace.WriteLine("HttpServer: startProcess started from thread \"" + 
                    System.Threading.Thread.CurrentThread.Name + "\" " +
                    " ( " + Thread.CurrentThread.ManagedThreadId.ToString() + " ) " +
                    DateTime.Now.ToLongTimeString());

                Socket listener = (Socket)res.AsyncState;
                Socket s = listener.EndAccept(res);
                listener.BeginAccept(new AsyncCallback(startProcess), listener);

                // Create a new processor for this request
                HttpProcessor processor = new HttpProcessor(s, ref commonCode, cacheTime);
                // Dispatch that processor in its own thread
                Thread thread = new Thread(new ThreadStart(processor.Process));
                thread.Name = "HttpProcessorThread";
                thread.Start();
            }
            catch(System.ObjectDisposedException)
            {
                // Occurs when shutting down the web server
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }
        }

        public void Shutdown()
        {
            try
            {
                listener.Close();
            }
            catch(Exception exc)
            {
                Trace.WriteLine(exc.ToString());
            }
        }

    }
}
