// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpProcessor.cs" company="John Allberg">
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
//   Summary description for HttpProcessor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServer
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Summary description for HttpProcessor.
    /// </summary>
    public class HttpProcessor
    {
        private static int _threads;
        private readonly Socket _socket;
        private NetworkStream _networkStream;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;
        private string _method;
        private string _url;
        private string _protocol;
        private readonly Hashtable _headers;
        private string _request;
        private const bool KeepAlive = false;
        private int _numRequests;
        private readonly bool _verbose = HttpServer.verbose;
        private readonly TimeSpan _cacheTime;

        /**
         * Each HTTP processor object handles one client.  If Keep-Alive is enabled then this
         * object will be reused for subsequent requests until the client breaks keep-alive.
         * This usually happens when it times out.  Because this could easily lead to a DoS
         * attack, we keep track of the number of open processors and only allow 100 to be
         * persistent active at any one time.  Additionally, we do not allow more than 500
         * outstanding requests.
         */
        
        internal HttpProcessor(Socket socket, ref ClientInterface commonCode, TimeSpan cacheTime) 
        {
            _socket = socket;
            _headers = new Hashtable();
            _commonCode = commonCode;
            _cacheTime = cacheTime;
        }

        readonly ClientInterface _commonCode;


        /**
         * This is the main method of each thread of HTTP processing.  We pass this method
         * to the thread constructor when starting a new connection.
         */
        public void Process() 
        {
            try 
            {
                Trace.WriteLine("HttpProcessor: process started from thread \"" + 
                    Thread.CurrentThread.Name + "\" " +
                    " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                    DateTime.Now.ToLongTimeString());

                // Increment the number of current connections
                Interlocked.Increment(ref _threads);
                Trace.WriteLine("HttpProcessor: Current Number Of Threads: " +
                    _threads);
                // Bundle up our sockets nice and tight in various streams
                _networkStream = new NetworkStream(_socket, FileAccess.ReadWrite);
                // It looks like these streams buffer
                _streamReader = new StreamReader(_networkStream);
                _streamWriter = new StreamWriter(_networkStream);
                // Parse the request, if that succeeds, read the headers, if that
                // succeeds, then write the given URL to the stream, if possible.
                while (ParseRequest())
                {
                    if (ReadHeaders()) 
                    {
                        // This makes sure we don't have too many persistent connections and also
                        // checks to see if the client can maintain keep-alive, if so then we will
                        // keep this http processor around to process again.
                        if (_threads <= 100 && "Keep-Alive".Equals(_headers["Connection"]))
                        {
                            //keepAlive = true;
                        }
                        // Copy the file to the socket
                        WriteUrl();
                        // If keep alive is not active then we want to close down the streams
                        // and shutdown the socket
                        if (!KeepAlive)
                        {
                            Thread.Sleep(50);
                            Trace.WriteLine("HttpProcessor: Process: " +
                                "Flushing and closing network connection on thread \"" + 
                                Thread.CurrentThread.Name + "\" " +
                                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                                DateTime.Now.ToLongTimeString());

                            _networkStream.Flush();
                            _networkStream.Close();
                            _socket.Shutdown(SocketShutdown.Both);
                            break;
                        }
                    }
                }
            } 
            finally 
            {
                // Always decrement the number of connections
                Interlocked.Decrement(ref _threads);	
                Trace.WriteLine("HttpProcessor: Exiting Process(). " +
                    "Number Of Threads is now " + _threads +
                    ". Current Thread is \"" + 
                    Thread.CurrentThread.Name + "\" " +
                    " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                    DateTime.Now.ToLongTimeString());

            }
        }

        public bool ParseRequest() 
        {
            Trace.WriteLine("HttpProcessor: parseRequest started from thread \"" + 
                Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                DateTime.Now.ToLongTimeString());

            // The number of requests handled by this persistent connection
            _numRequests++;
            // Here is where we ensure that we are not overloaded
            if (_threads > 500) 
            {
                WriteError(502, "Server temporarily overloaded");
                return false;
            }
            // FIXME: This could conceivably used to DoS us if we never finish reading the
            // line and they never hang up.  We could set the socket options to limit
            // the amount of time before reading a request.
            try 
            {
                _request = null;
                _request = _streamReader.ReadLine();
            } 
            catch (IOException) 
            {
            }
            // If the request line is null, then the other end has hung up on us.  A well
            // behaved client will do this after 15-60 seconds of inactivity.
            if (_request == null) 
            {
                if (_verbose) 
                {
                    Console.WriteLine("Keep-alive broken after " + _numRequests + " requests");
                }
                return false;
            }
            // HTTP request lines are of the form:
            // [METHOD] [Encoded URL] HTTP/1.?
            var tokens = _request.Split(new[]{' '});
            if (tokens.Length != 3) 
            {
                WriteError(400, "Bad request");
                return false;
            }
            // We currently only handle GET requests
            _method = tokens[0];
            if(!_method.Equals("GET")) 
            {
                WriteError(501, _method + " not implemented");
                return false;
            }
            _url = tokens[1];
            // Only accept valid urls
            if (!_url.StartsWith("/")) 
            {
                WriteError(400, "Bad URL");
                return false;
            }
            // Decode all encoded parts of the URL using the built in URI processing class
            int i = 0;
            while((i = _url.IndexOf("%", i)) != -1)
            {
                _url = _url.Substring(0, i) + Uri.HexUnescape(_url, ref i) + _url.Substring(i);
            }
            // Lets just make sure we are using HTTP, thats about all I care about
            _protocol = tokens[2];
            if (!_protocol.StartsWith("HTTP/")) 
            {
                WriteError(400, "Bad protocol: " + _protocol);
            }

            Trace.WriteLine("HttpProcessor: parseRequest ended on thread \"" + 
                Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                DateTime.Now.ToLongTimeString());

            return true;
        }

        public bool ReadHeaders() 
        {
            Trace.WriteLine("HttpProcessor: readHeaders started from thread \"" + 
                Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                DateTime.Now.ToLongTimeString());

            string line;
            string name = null;
            // The headers end with either a socket close (!) or an empty line
            while((line = _streamReader.ReadLine()) != null && line != "") 
            {
                // If the value begins with a space or a hard tab then this
                // is an extension of the value of the previous header and
                // should be appended
                if (name != null && Char.IsWhiteSpace(line[0])) 
                {
                    _headers[name] += line;
                    continue;
                }
                // Headers consist of [NAME]: [VALUE] + possible extension lines
                int firstColon = line.IndexOf(":");
                if (firstColon != -1) 
                {
                    name = line.Substring(0, firstColon);
                    String value = line.Substring(firstColon + 1).Trim();
                    if (_verbose) Console.WriteLine(name + ": " + value);
                    _headers[name] = value;
                } 
                else 
                {
                    WriteError(400, "Bad header: " + line);
                    return false;
                }
            }

            Trace.WriteLine("HttpProcessor: readHeaders ended on thread \"" + 
                Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                DateTime.Now.ToLongTimeString());

            return line != null;
        }

        /**
         * We need to make sure that the url that we are trying to treat as a file
         * lies below the document root of the http server so that people can't grab
         * random files off your computer while this is running.
         */
        public void WriteUrl() 
        {
            Trace.WriteLine("HttpProcessor: writeURL started from thread \"" + 
                Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                DateTime.Now.ToLongTimeString());

            try 
            {
                string file = "<html>\r\n" + 
                    "<body>\r\n" +
                    "<h1>WinShooter</h1>\r\n" +
                    "<li><a href=\"/patrullista\">Patrullista</a>\r\n" +
                    "<li><a href=\"/resultat\">Resultatlista</a>\r\n" +
                    "</body>\r\n" +
                    "</html>\r\n";
                byte[] bytes = null;
                string contentType = "";

                if (_commonCode.GetCompetitionsCount() == 0)
                {
                    // No competition exist
                    file = "<html>\r\n" + 
                        "<body>\r\n" +
                        "<h1>WinShooter</h1>\r\n" +
                        "Ingen t&auml;vling finns &auml;nnu. Skapa en t&auml;vling fr&aring;n klienten.\r\n" +
                        "</body>\r\n" +
                        "</html>\r\n";
                }

                var cache = HttpCache.GetInstance();
                if (!cache.IsInCache(_url.ToLower(), _cacheTime))
                {
                    switch(_url.ToLower())
                    {
                            #region Patrullista
                        case "/patrullista":
                        {
                            file = _commonCode.InternetHtmlExportPatrols();
                            break;
                        }
                            #endregion
                            #region Resultat
                        case "/resultat":
                        {
                            const bool finalResults = false;
                            file = _commonCode.InternetHtmlExportResults(finalResults);
                            file = file.Replace("<head>", "<head>\r\n" +
                                    "<meta http-equiv=\"refresh\" content=\"120\">\r\n");
                            break;
                        }
                            #endregion
                            #region Images
                        case "/img/logga.jpg":
                        {
                            contentType = "image/jpeg";
                            bytes = Common.CEmbeddedResources.GetEmbeddedResourceBytes(
                                "Allberg.Shooter.Common.WinShooterLogga.jpg");
                            break;
                        }
                            #endregion
                    }
                    // Make sure it is marked as UTF8
                    if (bytes == null)
                    {
                        file = file.Replace("<head>", "<head>\r\n<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n");
                        file = file.Replace("src=\"http://www.allberg.se/WinShooter/", "src=\"/");
                        bytes = Encoding.UTF8.GetBytes(file);
                    }

                    // Add to cache
                    cache.AddToCache(_url.ToLower(), bytes, contentType);
                }
                else
                {
                    // Page is in cache
                    bytes = cache.ReturnFromCache(_url.ToLower());
                    contentType = cache.ReturnContentTypeFromCache(_url.ToLower());
                }

                long left = bytes.Length;
                if (contentType == "")
                    WriteSuccess(left);
                else
                    WriteSuccess(left, contentType);
                // Copy the contents of the file to the stream, ensure that we never write
                // more than the content length we specified.  Just in case the file somehow
                // changes out from under us, although I don't know if that is possible.
                _networkStream.Write(bytes,
                    0,
                    bytes.Length);
                _networkStream.Flush();
                Thread.Sleep(150);
            }
            catch(Exception exc) 
            {
                Trace.WriteLine("HttpProcessor: WriteURL Exception: " +
                    exc + " on thread \"" + 
                    Thread.CurrentThread.Name + "\" " +
                    " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                    DateTime.Now.ToLongTimeString());

                WriteFailure(exc.Message);
            }
            
            Trace.WriteLine("HttpProcessor: WriteUrl ended on thread \"" + 
                Thread.CurrentThread.Name + "\" " +
                " ( " + Thread.CurrentThread.ManagedThreadId + " ) " +
                DateTime.Now.ToLongTimeString());
        }

        /**
         * These write out the various HTTP responses that are possible with this
         * very simple web server.
         */

        public void WriteSuccess(long length) 
        {
            WriteResult(200, "OK", length);
        }

        public void WriteSuccess(long length, string content) 
        {
            WriteResult(200, "OK", length, content);
        }

        public void WriteFailure() 
        {
            WriteFailure("File not found");
        }
        public void WriteFailure(string message) 
        {
            WriteError(500, message);
        }
    
        public void WriteForbidden() 
        {
            WriteError(403, "Forbidden");
        }

        public void WriteError(int status, string message) 
        {
            string output = "<h1>HTTP/1.0 " + status + " " + message + "</h1>";
            WriteResult(status, message, output.Length);
            _streamWriter.Write(output);
            _streamWriter.Flush();
        }

        public void WriteResult(int status, string message, long length) 
        {
            WriteResult(status, message, length, "text/html");
        }

        public void WriteResult(int status, string message, long length, string content) 
        {
            if (_verbose) Console.WriteLine(_request + " " + status + " " + _numRequests);
            _streamWriter.Write("HTTP/1.0 " + status + " " + message + "\r\n");
            _streamWriter.Write("Content-Length: " + length + "\r\n");
            _streamWriter.Write("Content-Type: " + content + "\r\n");
            if (KeepAlive)
            {
                _streamWriter.Write("Connection: Keep-Alive\r\n");
            }
            else
            {
                _streamWriter.Write("Connection: close\r\n");
            }
            _streamWriter.Write("\r\n");
            _streamWriter.Flush();
        }

    }
}
