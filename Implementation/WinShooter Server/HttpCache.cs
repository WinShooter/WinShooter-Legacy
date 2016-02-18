// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpCache.cs" company="John Allberg">
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
//   Summary description for HttpCache.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.Shooter.WinShooterServer
{
    using System;
    using System.Collections;

    /// <summary>
    /// Summary description for HttpCache.
    /// </summary>
    public class HttpCache
    {
        /// <summary>
        /// The locker.
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// The the http cache.
        /// </summary>
        private static HttpCache theHttpCache;

        private readonly Hashtable pageTimes = Hashtable.Synchronized(new Hashtable());
        private readonly Hashtable pageContent = Hashtable.Synchronized(new Hashtable());
        private readonly Hashtable pageContentType = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Prevents a default instance of the <see cref="HttpCache"/> class from being created.
        /// </summary>
        private HttpCache()
        {
        }

        /// <summary>
        /// Get the current instance.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpCache"/>.
        /// </returns>
        static public HttpCache GetInstance()
        {
            lock (Locker)
            {
                return theHttpCache ?? (theHttpCache = new HttpCache());
            }
        }

        public bool IsInCache(string page, TimeSpan scavaging)
        {
            if (!this.pageTimes.ContainsKey(page))
            {
                return false;
            }

            var last = (DateTime)this.pageTimes[page];
            return last.Add(scavaging) > DateTime.Now;
        }

        public byte[] ReturnFromCache(string page)
        {
            var bytes = (byte[])this.pageContent[page];
            return bytes;
        }

        public string ReturnContentTypeFromCache(string page)
        {
            var type = (string)this.pageContentType[page];
            return type;
        }

        public void AddToCache(string page, byte[] content, string type)
        {
            if (this.pageTimes.ContainsKey(page))
            {
                this.pageTimes[page] = DateTime.Now;
            }
            else
            {
                this.pageTimes.Add(page, DateTime.Now);
            }

            if (this.pageContent.ContainsKey(page))
            {
                this.pageContent[page] = content;
            }
            else
            {
                this.pageContent.Add(page, content);
            }

            if (this.pageContentType.ContainsKey(page))
            {
                this.pageContentType[page] = type;
            }
            else
            {
                this.pageContentType.Add(page, type);
            }
        }
    }
}
