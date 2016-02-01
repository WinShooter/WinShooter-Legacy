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
        private HttpCache()
        {
        }

        static readonly object Locker = new object();
        static HttpCache _theInstance;
        static public HttpCache GetInstance()
        {
            lock(Locker)
            {
                return _theInstance ?? (_theInstance = new HttpCache());
            }
        }

        private readonly Hashtable _pageTimes = Hashtable.Synchronized(new Hashtable());
        private readonly Hashtable _pageContent = Hashtable.Synchronized(new Hashtable());
        private readonly Hashtable _pageContentType = Hashtable.Synchronized(new Hashtable());

        public bool IsInCache(string page, TimeSpan scavaging)
        {
            if (!_pageTimes.ContainsKey(page))
                return false;

            var last = (DateTime)_pageTimes[page];
            return last.Add(scavaging) > DateTime.Now;
        }

        public byte[] ReturnFromCache(string page)
        {
            var bytes = (byte[])_pageContent[page];
            return bytes;
        }

        public string ReturnContentTypeFromCache(string page)
        {
            var type = (string)_pageContentType[page];
            return type;
        }

        public void AddToCache(string page, byte[] content, string type)
        {
            if (_pageTimes.ContainsKey(page))
                _pageTimes[page] = DateTime.Now;
            else
                _pageTimes.Add(page, DateTime.Now);

            if (_pageContent.ContainsKey(page))
                _pageContent[page] = content;
            else
                _pageContent.Add(page, content);

            if (_pageContentType.ContainsKey(page))
                _pageContentType[page] = type;
            else
                _pageContentType.Add(page, type);
        }
    }
}
