// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace WorkspaceCFG
{

    [Serializable]
    public class VariablesSettings
    {
        public List<CFGEnums.VarInfo> DBEntryList = new List<CFGEnums.VarInfo>();
        public void SaveVariableList(string saveToPath)
        {
            XmlSerialization.SerializeToFile(this, saveToPath);
        }

        public static VariablesSettings Load(string fileFullpath)
        {
            return XmlSerialization.DeserializeFromFile<VariablesSettings>(fileFullpath);
        }
    }
}