// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDownloader.cs" company="John Allberg">
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
//   Summary description for IDownloader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allberg.AutoUpdater.UpdaterLib
{
    using System;

    /// <summary>
    /// Summary description for IDownloader.
    /// </summary>
    public delegate void AllFilesDoneHandler();
    public delegate void UpdateFailedHandler(Exception exc);
    public delegate void FileDoneHandler(string file);
    public delegate void BytesDoneHandler(int bytesReceived);
    public delegate void SetNumberOfFilesHandler(int count);
    public delegate void CurrentFileProgressHandler(int percent);
    public interface IDownloader
    {
        event AllFilesDoneHandler AllFilesDone;
        event FileDoneHandler FileDone;
        event UpdateFailedHandler UpdateFailed;
        event CurrentFileProgressHandler CurrentFileProgress;
        event BytesDoneHandler BytesDone;
        //event SetNumberOfFilesHandler SetNumberOfFiles;

        int PrepareFiles(string Url, string TargetDir, string CurrentDir);
        void PrepareFiles(string[] Urls, string TargetDir, string CurrentDir);
        void GetFiles();
    }
}
