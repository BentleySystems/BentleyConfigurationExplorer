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
    // Class CFGToken represents a token value in a parsed CFGLine
    // 
    // ---------------------------------------------------------------------------------------

    [Serializable()]
    [JsonObject(MemberSerialization.Fields)]
    public class CFGToken : IComparable <CFGToken>
    {

        // ---------------------------------------------------------------------------------------
        // Public Members
        // ---------------+---------------+---------------+---------------+---------------+-------
        public string Value;                  // Token's Value
        public string ValueOrg;                  // Token's Value
        public CFGEnums.TT TokenType;                  // Token's Type
        public CFGEnums.TokenGroup TokenGroup;         // Token's Group
        public int Position;              // Position in the string where the token was found (used to sort tokens)
        public int PositionRelative;              // Position in the string where the token was found (used to sort tokens)
        public int Precedence;            // Tokens precedence value for processing
        public CFGEnums.TT OriginalType;               // Stores the original token type in case it changes during processing
        public string ValueWorking;
        public CFGEnums.TT TokenTypeWorking;
        public CFGEnums.TokenGroup TokenGroupWorking;
        public int TokenIndex = 0;
        public int EncapluationMatch = 0;
        public int ExpansionLevel = 0;
        public bool InFunction = false;
        public int IndexFunctionClose = 0;
        // ---------------------------------------------------------------------------------------
        // @description: Allows tokens to be sorted based on the pos value
        // ---------------+---------------+---------------+---------------+---------------+-------


        public int CompareTo(CFGToken other)
        {
            if (other == null) return 0;

            int postionComparison = this.Position.CompareTo(other.Position);
            if (postionComparison != 0) return postionComparison;

            return  this.PositionRelative.CompareTo(other.PositionRelative);

        }

        // ---------------------------------------------------------------------------------------
        // @description: Gets the value of the token; includes extra characters if needed
        // ---------------+---------------+---------------+---------------+---------------+-------
        public string GetValue(bool IncludeExtras = false)
        {
            if (IncludeExtras && TokenType == CFGEnums.TT.ttStringWithQuotes)
                return '"' + Value + '"';  // surround with " "
            if (IncludeExtras && TokenType == CFGEnums.TT.ttStringWithParenthesis)
                return "(" + Value + ")";     // surround with ( )
            if (IncludeExtras && TokenType == CFGEnums.TT.ttStringWithBrackets)
                return "{" + Value + "}";       // surround with { }

            return Value;
        }

        // ---------------------------------------------------------------------------------------
        // @description: Sets the precedence value of the token
        // ---------------+---------------+---------------+---------------+---------------+-------
        public void SetPrecedence()
        {
            Precedence = (int)Math.Floor((double)TokenType / 100);
        }

    }
}