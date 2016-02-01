// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomSoapServerFormatterSinkProvider.cs" company="John Allberg">
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
//   Summary description for CustomSoapServerFormatterSinkProvider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServer
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Messaging;

    /// <summary>
    /// Summary description for CustomSoapServerFormatterSinkProvider.
    /// </summary>
    public class ClientIpServerSinkProvider: IServerChannelSinkProvider 
    {
        private IServerChannelSinkProvider next;
        
        public ClientIpServerSinkProvider(IDictionary properties, ICollection providerData)
        {
        }
        
        public void GetChannelData (IChannelDataStore channelData)
        {
        }
        
        public IServerChannelSink CreateSink (IChannelReceiver channel)
        {
            IServerChannelSink nextSink = null;
            
            if (next != null) 
            {
                nextSink = next.CreateSink(channel);
            }
            
            return new ClientIpServerSink(nextSink);
        }
        
        public IServerChannelSinkProvider Next
        {
            get { return next; }
            set { next = value; }
        }
    }
    
    public class ClientIpServerSink : BaseChannelObjectWithProperties, IServerChannelSink
    {    
        private IServerChannelSink _next;
        
        public ClientIpServerSink (IServerChannelSink next) 
        {
            _next = next;
        }
        
        public void AsyncProcessResponse ( IServerResponseChannelSinkStack sinkStack , Object state , IMessage msg , ITransportHeaders headers , Stream stream ) 
        {
        }
        
        public Stream GetResponseStream ( IServerResponseChannelSinkStack sinkStack , Object state , IMessage msg , ITransportHeaders headers ) 
        {
            return null;
        }
        
        public ServerProcessing ProcessMessage(
            IServerChannelSinkStack sinkStack, 
            IMessage requestMsg, 
            ITransportHeaders requestHeaders, 
            Stream requestStream, 
            out IMessage responseMsg, 
            out ITransportHeaders responseHeaders, 
            out Stream responseStream)
        {
            var ip = requestHeaders[CommonTransportKeys.IPAddress] as IPAddress;
            CallContext.SetData("ClientIPAddress", ip);

            var connectionId = (Int64)requestHeaders[CommonTransportKeys.ConnectionId];
            CallContext.SetData("ClientConnectionId", connectionId); 
            
            if (_next != null) 
            {
                var spres =  _next.ProcessMessage (sinkStack,requestMsg, requestHeaders,requestStream,out responseMsg,out responseHeaders,out responseStream);
                return spres;
            } 
            else 
            {
                responseMsg=null;
                responseHeaders=null;
                responseStream=null;
                return new ServerProcessing();
            }
        }
        
        public IServerChannelSink NextChannelSink 
        {
            get {return _next;}
            set {_next = value;}
        }
    }
}
