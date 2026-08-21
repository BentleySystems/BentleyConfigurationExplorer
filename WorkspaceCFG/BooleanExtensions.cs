// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;

namespace WorkspaceCFG
{

    public static class BooleanExtensions
    {

        public static string ToYesNoString(this bool value)
        {
            return value ? "Yes" : "No";
        }

    }
}