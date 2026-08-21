// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace WorkspaceCFG
{
    // Sorts FileInfo instances by FullName using StrCmpLogicalW to match the Windows sort by name order
    public class FileInfoComparer : IComparer<FileInfo>
    {

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string s1, string s2);

        public int Compare(FileInfo x, FileInfo y)
        {

            string args1 = x.FullName;
            string args2 = y.FullName;
            return FileInfoComparer.StrCmpLogicalW(args1, args2);
        }

    }
}