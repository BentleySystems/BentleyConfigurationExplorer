// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{
    // ---------------------------------------------------------------------------------------
    // 
    // Creates a collection of System.IO.FileSystemWatcher objects that raise events and warns the user when a cfg file is changed.
    // 
    // 
    // 
    // ---------------------------------------------------------------------------------------
    public class FileWatcher
    {

        private static DateTime _lastEventTime;
        private static bool _enableWatch;
        private static List<FileSystemWatcher> _watcherList = new List<FileSystemWatcher>(); // FIX: Use List<FileSystemWatcher>

        private static DateTime LastEventTime            // Stores the last date/time an event was raised
        {
            get
            {
                return _lastEventTime;
            }

            set
            {
                _lastEventTime = value;
            }
        }

        public static bool EnableWatch
        {
            get
            {
                return _enableWatch;
            }

            set
            {
                _enableWatch = value;
            }
        }

        public static void SetEnable(bool value)
        {
            EnableWatch = value;
            foreach (FileSystemWatcher watcher in _watcherList)
                watcher.EnableRaisingEvents = value;
        }

        public static void AddWatch(ref string filePath)
        {
            string path;
            string folderName;
            path = filePath;

            if (!path.Contains("\\"))
            {
                path = Utilities.GetExeFolder() + path;
            }
            folderName = UtilitiesPath.GetDirectoryName(ref path);

            // If this a unc path, remove one \ from the start as FileSystemWatcher can't handle triple slashes
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(@"\\\", folderName.Length >= 3 ? folderName.Substring(0, 3) : "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                folderName = folderName.Substring(1);

            var watcher = new FileSystemWatcher(folderName, UtilitiesPath.GetFileName(path));
            watcher.Path = folderName;
            watcher.Changed += NotifyChange;
            watcher.Deleted += NotifyChange;
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = false;

            _watcherList.Add(watcher);
        }

        public static void ClearWatches()
        {
            foreach (FileSystemWatcher watcher in _watcherList)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }

            _watcherList = null;
            _watcherList = new List<FileSystemWatcher>(); // FIX: Use List<FileSystemWatcher>
        }

        public static void NotifyChange(object source, FileSystemEventArgs e)
        {
            if (DateTime.Now.Subtract(LastEventTime).Seconds <= 1)
            {
                LastEventTime = DateTime.Now;
                return;
            }

            LastEventTime = DateTime.Now;
            if (!EnableWatch)
                return;

            System.Windows.Forms.DialogResult result;

            if (EnableWatch)
            {
                if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    result = System.Windows.Forms.MessageBox.Show(string.Format(CEResource.TXT_ErrChangesHaveBeenMadeTo, e.FullPath), CEResource.TXT_FileChanged, System.Windows.Forms.MessageBoxButtons.OK);
                }

                if (e.ChangeType == WatcherChangeTypes.Deleted)
                {
                    result = System.Windows.Forms.MessageBox.Show(string.Format(CEResource.TXT_ErrFileHasBeenChanged, e.FullPath), CEResource.TXT_FileDeleted, System.Windows.Forms.MessageBoxButtons.OK);
                }
            }

            LastEventTime = DateTime.Now;
        }

    }
}