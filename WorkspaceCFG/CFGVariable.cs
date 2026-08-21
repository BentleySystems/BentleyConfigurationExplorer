// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using Newtonsoft.Json;

namespace WorkspaceCFG;

[Serializable]
[JsonObject(MemberSerialization.Fields)]
public class CFGVariable
{
    private List<string> _parents; // List of Parent Variables

    [JsonProperty(IsReference = false)] // Arrays can't be tracked by PreserveReferencesHandling
    public string[] Args = new string[7];
    public string FinalExpansion; // Final Expansion
    public bool IsDefined; // Defined status
    public bool IsLocked; // Lock status
    public int Level; // Final Level / Current Level

    public string Name; // Variable Name
    public bool NeedsSpecialExpansion;
    public CFGConfiguration ParentConfiguration; // Reference to the owning workspace
    public string Value; // Final Value / Current Value

    [JsonProperty(IsReference = false)] // Arrays can't be tracked by PreserveReferencesHandling
    public string[] Values = new string[7]; // Value for each level
    private bool setFinal = false;

    public CFGVariable(CFGConfiguration wrk)
    {
        //Debug.WriteLine($"CFGVariable: public CFGVariable(CFGConfiguration wrk) ");
        Level = -1;
        IsLocked = false;
        IsDefined = true;
        ParentConfiguration = wrk;
        NeedsSpecialExpansion = false;
    }

    ~CFGVariable()
    {
        ParentConfiguration = null;
        Value = null;
        FinalExpansion = null;
    }

    public void SetValue(string invalue, int inlevel = 0)
    {
        //if (IsLocked || !IsDefined)
        if (IsLocked)
            return;

        if (inlevel < 0)
            inlevel = 0;

        Values[inlevel] = Utilities.CleanupMultiVariableValue(invalue);
        Value = Values[inlevel];
    }

    public void SetFinalExpansion()
    {
        bool RoleFound = false;
        string OldValue = Value;
        setFinal = true;
        

            if (!string.IsNullOrEmpty(Value) && Value.IndexOf("role", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            RoleFound = true;
            //Debugger.Break();
        }

        // Ensure FinalExpansion is never null
        FinalExpansion = Expand() ?? string.Empty;

        if (RoleFound)
        {
            if (!string.IsNullOrEmpty(Value) && Value.IndexOf("role", StringComparison.OrdinalIgnoreCase) == -1)
            {
                Debugger.Break();
            }
        }
        RoleFound = false;

        // Find the variable's level
        for (int t = 6; t >= 0; t--)
        {
            if (!string.IsNullOrEmpty(Values[t]))
            {
                Level = t;
                break;
            }
        }

        // Check for duplicate values or expansions
        string[] exp = Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        for (int t = 0; t < exp.Length; t++)
        {
            for (int y = t + 1; y < exp.Length; y++)
            {
                if (string.Equals(exp[t].Trim(), exp[y].Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    var argetype = CFGEnums.CFGEventType.cfgWarning;
                    var argdescription = "Duplicate Value - " + exp[y];
                    CFGLine argline = null;
                    CFGFile argfile = null;
                    var argvariable = this;
                    ParentConfiguration.AddEvent(ref argetype, ref argdescription, argline, argfile, argvariable, Name);
                    return;
                }
            }
        }

        exp = FinalExpansion.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        for (int t = 0; t < exp.Length; t++)
        {
            for (int y = t + 1; y < exp.Length; y++)
            {
                if (string.Equals(exp[t].Trim(), exp[y].Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    var argetype1 = CFGEnums.CFGEventType.cfgWarning;
                    var argdescription1 = "Duplicate Expansion - " + exp[y];
                    CFGLine argline1 = null;
                    CFGFile argfile1 = null;
                    var argvariable1 = this;
                    ParentConfiguration.AddEvent(ref argetype1, ref argdescription1, argline1, argfile1, argvariable1, Name);
                    return;
                }
            }
        }

        FinalExpansion = Utilities.CleanupMultiVariableValue(FinalExpansion);
    }

    public void Undefine()
    {
        if (IsLocked)
            return;

        IsDefined = false;
        if (0 > Level || 6 < Level) // Level not within range, set it 0
            Level = 0;

        Values[Level] = string.Empty;

        Level = -3; // Level -3 = undefined
        Value = string.Empty;
        FinalExpansion = string.Empty;
        _parents = null;
    }

    public string GetValue(ref int clevel)
    {
        if (clevel < 0)
            clevel = 0;

        for (int t = clevel; t >= 0; t--)
        {
            if (!string.IsNullOrEmpty(Values[t]))
                return Values[t];
        }

        return string.Empty;
    }

    public string Expand(int currentlevel = 6)
    {
       
        //Debug.WriteLine($"CFGVariable: Expand(int currentlevel = 6)");
        Value = GetValue(ref currentlevel);

        // Add check for "role" and provide a breakpoint

        var expandedValue = MacroParser.ParseLine(ParentConfiguration, currentlevel, Value, true);

        if (expandedValue is not null && expandedValue.Contains("/"))
            expandedValue = expandedValue.Replace(@"\", "/");

        //Debug.WriteLine($"CFGVariable: Expand(int currentlevel = 6) return: " +expandedValue);
        return expandedValue;
    }

    public int NumberOfValues()
    {
        string[] ms = FinalExpansion.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        return ms.Length;
    }

    public void AppendParents(ref List<string> inParents, ref bool ClearParents)
    {
        if (_parents is null)
            _parents = new List<string>();

        if (ClearParents)
            _parents.Clear();

        foreach (string parent in inParents)
        {
            if (!_parents.Contains(parent))
                _parents.Add(parent);
        }
    }

    public List<SubVariable> GetParents()
    {
        if (_parents is null || _parents.Count == 0)
            return null;

        var parentlist = new List<SubVariable>();
        int generation = 1;

        foreach (string parent in _parents)
        {
            var var = ParentConfiguration.GetVariable(parent);
            if (var is not null)
            {
                var argpath = string.Empty;
                AddSubVar(ref parentlist, ref var, generation, ref argpath);
            }
        }

        return parentlist;
    }

    public List<SubVariable> GetChildren()
    {
        var childrenlist = new List<SubVariable>();
        int generation = 1;

        foreach (var currentVar in ParentConfiguration.Variables.Values)
        {
            if (currentVar._parents is not null && currentVar._parents.Contains(Name))
            {
                var argpath = Name + " -> " + currentVar.Name;
                var tempVar = currentVar; // Create a temporary variable to pass by reference
                AddSubVar(ref childrenlist, ref tempVar, generation, ref argpath);
            }
        }

        return childrenlist;
    }

    private void AddSubVar(ref List<SubVariable> slist, ref CFGVariable var, int generation, ref string path)
    {
        var tempVar = var; // Create a temporary variable to avoid using ref in the lambda
        if (slist.Exists(sv => sv.Variable.Name == tempVar.Name))
            return;

        var sv = new SubVariable
        {
            Variable = var,
            Generation = generation,
            Path = path
        };
        slist.Add(sv);
    }
}
