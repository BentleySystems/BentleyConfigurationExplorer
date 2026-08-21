// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WorkspaceCFG
{
    // Sorts file name string array using StrCmpLogicalW to match the Windows sort by name order
    public class FileComparer : IComparer<string>
    {

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string s1, string s2);

        public int Compare(string x, string y)
        {

            return FileComparer.StrCmpLogicalW(x,y);
        }

    }
}