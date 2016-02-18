// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloaderHttpClient.cs" company="John Allberg">
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
//   Summary description for DownloaderHttpClient.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.AutoUpdater.UpdaterLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Summary description for DownloaderHttpClient.
    /// </summary>
    public class DownloaderHttpClient : IDownloader
    {
        public DownloaderHttpClient()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region IDownloader Members
        public event UpdateFailedHandler UpdateFailed;
        public event AllFilesDoneHandler AllFilesDone;
        public event FileDoneHandler FileDone;
        public event CurrentFileProgressHandler CurrentFileProgress;
        public event BytesDoneHandler BytesDone;
        //public event SetNumberOfFilesHandler SetNumberOfFiles;

        public int PrepareFiles(string Url, string TargetDir, string CurrentDir)
        {
            if (TargetDir.EndsWith("\\"))
                TargetDir = TargetDir.Substring(0, TargetDir.Length-1);
            if (CurrentDir.EndsWith("\\"))
                CurrentDir = CurrentDir.Substring(0, CurrentDir.Length-1);

            if (!Directory.Exists(TargetDir))
                Directory.CreateDirectory(TargetDir);
            targetDir = TargetDir + "\\";

            if (!Directory.Exists(CurrentDir))
                throw new ApplicationException(CurrentDir + " doesn't exist");
            currentDir = CurrentDir + "\\";

            byte[] bytes = getFile(Url, -1);
            string content = System.Text.UTF8Encoding.UTF8.GetString(bytes).Replace("\n", string.Empty);;
            string[] files = content.Split('\r');

            List<string> urlsList = new List<string>();
            int sizeToDownLoad = 0;
            Trace.WriteLine("DownloaderHttpClient.PrepareFiles: Checking what files to actually download.");
            foreach (string fileLine in files)
            {
                if (fileLine.Trim() != string.Empty)
                {
                    string[] parts = fileLine.Split(';');
                    string fileUrl = parts[0].Trim();
                    string fileHash = parts[1].Trim();
                    int fileSize = int.Parse(parts[2].Trim());

                    if (checkFileShouldBeFetched(fileUrl, fileHash))
                    {
                        Trace.WriteLine("File to be downloaded: " + fileUrl);
                        fileHashes.Add(fileUrl, fileHash);
                        fileSizes.Add(fileUrl, fileSize);
                        urlsList.Add(fileUrl);

                        sizeToDownLoad += fileSize;
                    }
                }
            }
            urls = urlsList.ToArray();

            return sizeToDownLoad;
        }

        private string[] urls;
        private string targetDir;
        private string currentDir;
        private Hashtable fileHashes = new Hashtable();
        private Hashtable fileSizes = new Hashtable();
        public void PrepareFiles(string[] Urls, string TargetDir, string CurrentDir)
        {
            urls = Urls;

            if (TargetDir.EndsWith("\\"))
                TargetDir = TargetDir.Substring(0, TargetDir.Length-1);
            if (CurrentDir.EndsWith("\\"))
                CurrentDir = CurrentDir.Substring(0, CurrentDir.Length-1);

            if (!Directory.Exists(TargetDir))
                Directory.CreateDirectory(TargetDir);
            if (!Directory.Exists(CurrentDir))
                throw new ApplicationException(CurrentDir + " doesn't exist");
            currentDir = currentDir + "\\";

            targetDir = TargetDir + "\\";
        }
        public void GetFiles()
        {
            int bytesReceived = 0;
            foreach(string url in urls)
            {
                bool fileIsOk = false;
                int retries = 0;
                Exception lastException = null;
                while (!fileIsOk & retries < 5)
                {
                    try
                    {
                        fileIsOk = GetFile(url, bytesReceived);
                        bytesReceived += (int)fileSizes[url];
                    }
                    catch(Exception exc)
                    {
                        retries++;
                        Trace.WriteLine(exc.ToString());
                        lastException = exc;
                    }
                }
                if (retries >= 5)
                {
                    try
                    {
                        UpdateFailed(lastException);
                    }
                    catch(System.NullReferenceException)
                    {
                    }
                    return;
                }
            }

            try
            {
                AllFilesDone();
            }
            catch(System.NullReferenceException)
            {
            }
        }
        #endregion

        private bool checkFileShouldBeFetched(string url, string fileHash)
        {
            if (url.ToLower().EndsWith("updatergui.exe"))
                return true;

            string thisFileName = url.Substring(1 + url.LastIndexOf("/"));
            string target = targetDir + thisFileName;
            string currentTarget = currentDir + thisFileName;

            if (File.Exists(currentTarget))
            {
                // File already exist in software directory
                byte[] fileBytes = Common.OpenFile(currentTarget);
                if ( Common.CheckHashMatch(url, fileBytes, fileHash))
                {
                    // Hash matches current file
                    return false;
                }
            }

            if (File.Exists(target))
            {
                // File already exist in download directory
                byte[] fileBytes = Common.OpenFile(target);
                if (Common.CheckHashMatch(url, fileBytes, fileHash))
                {
                    // Hash matches file in download directory
                    return false;
                }
            }
            return true;
        }
        private bool GetFile(string url, int bytesAlreadyReceived)
        {
            string thisFileName = url.Substring(1 + url.LastIndexOf("/")); ;
            string target = targetDir + thisFileName;

            byte[] bytes = getFile(url, bytesAlreadyReceived);
            Common.CheckHashMatch(url, bytes, (string)fileHashes[url]);
            Common.WriteFile(bytes, target);
            try
            {
                if (FileDone != null)
                    FileDone(thisFileName);
            }
            catch(System.NullReferenceException)
            {
            }
            return true;
        }

        private byte[] getFile(string url, int bytesAlreadyReceived)
        {
            WebResponse result = null;

            try 
            {
                WebRequest req = WebRequest.Create(url);
                result = req.GetResponse();
                long totLen = result.ContentLength;
                Stream ReceiveStream = result.GetResponseStream();
                MemoryStream memStream = new MemoryStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                BinaryReader sr = new BinaryReader( ReceiveStream);

                Trace.WriteLine("\r\nResponse stream received");

                long nrOfBytes = 0;
                byte[] read = new byte[256];
                int count = sr.Read(read, 0, 256);
                int totalCount = count;

                while (count > 0) 
                {
                    nrOfBytes += count;
                    memStream.Write(read,0,count);
                    updateCurrentFileProgress(nrOfBytes, totLen);

                    count = sr.Read(read, 0, 256);
                    totalCount += count;
                    try
                    {
                        if (bytesAlreadyReceived > -1)
                            BytesDone(bytesAlreadyReceived + totalCount);
                    }
                    catch (System.NullReferenceException)
                    {
                    }
                }
                return memStream.ToArray();
            } 
            catch(Exception exc) 
            {
                Trace.WriteLine(exc.ToString());
                throw;
            } 
            finally 
            {
                if ( result != null ) 
                {
                    result.Close();
                }
            }
        }

        private long lastProgress = -1;
        private void updateCurrentFileProgress(long current, long tot)
        {
            current = current * 100;
            long currentProgress = current / tot;

            if (lastProgress != currentProgress)
            {
                Trace.WriteLine(currentProgress.ToString() + "%");
                try
                {
                    CurrentFileProgress((int)currentProgress);
                }
                catch(System.NullReferenceException)
                {
                }
                lastProgress = currentProgress;
            }

        }

    }
}
