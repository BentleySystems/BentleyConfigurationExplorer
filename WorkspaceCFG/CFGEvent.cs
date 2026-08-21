// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using Newtonsoft.Json;

namespace WorkspaceCFG
{
    // ---------------------------------------------------------------------------------------
    // 
    // Class CFGEvent - Represents a historical event during workspace processing
    // 
    // ---------------------------------------------------------------------------------------

    [Serializable()]
    [JsonObject(MemberSerialization.Fields)]
    public class CFGEvent
    {

        // ---------------------------------------------------------------------------------------
        // Public Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public static int NextUID = 0;
        public int UID;                       // Unique event ID
        public string Description;
        public CFGEnums.CFGEventType EventType;
        public CFGLine ParentLine;
        public CFGFile ParentFile;
        public CFGVariable Variable;
        public string VarValue;
        public string VarExpansion;
        public string VarName;
        public int Level;

        public CFGEvent()
        {
            UID = NextUID;
            NextUID += 1;
            Level = -999;
        }

    }
}