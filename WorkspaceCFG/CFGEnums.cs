// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Globalization;

namespace WorkspaceCFG
{
    // ---------------------------------------------------------------------------------------
    // 
    // Global Enumeration Values
    // 
    // ---------------------------------------------------------------------------------------

    public static class CFGEnums
    {

        // TT = Token Type
        public enum TT : int
        {

            //LineMarker
            ttEOL=00101,

            // Preprocessor Directives
            ttIF = 10001,
            ttELIF = 10002,
            ttENDIF = 10003,
            ttELSE = 10004,
            ttLOCK = 10005,
            ttUNDEF = 10006,
            ttERROR = 10007,
            ttINCLUDE = 10008,
            ttLEVEL = 10009,
            ttIFDEF = 10010,
            ttIFNDEF = 10011,
            ttIFFEATURE = 10012,

            // String Functions
            ttFUNCOPEN = 12601,
            ttFUNCOPENabs = 12602,
            ttFUNCCLOSE = 12603,
            ttFUNCCLOSEabs = 12604,
            ttCONCAT = 11002,
            ttDEV = 11003,
            ttDIR = 11004,
            ttDEVDIR = 11005,
            ttPARENTDIR = 11006,
            ttPARENTDEVDIR = 11007,
            ttBASENAME = 11008,
            ttFILENAME = 11009,
            ttEXT = 11010,
            ttNOEXT = 11011,
            ttFIRST = 11012,
            ttBUILD = 11013,
            ttREGISTRYREAD = 11014,
            ttFIRSTDIRPIECE = 11015,
            ttLASTDIRPIECE = 11016,
            ttGETVAR = 11017,
            ttDOLLAR = 11018,
            ttBACKTRACK = 11019,

            ttMULTIPLY = 13801,     // (math)
            ttDIVIDE = 13802,       // (math)
            ttMODULUS = 13803,      // (math)
            ttADD = 13701,          // (math)
            ttSUBTRACT = 13702,     // (math)
            ttSHIFTLEFT = 13601,    // (math)
            ttSHIFTRIGHT = 13602,   // (math)

            ttDEFINED = 13501,
            ttEXISTS = 13502,
            ttGREATEROREQUAL = 13503,       // (math) Greater than equal to
            ttLESSTOREQUAL = 13504,       // (math) Less than equal to
            ttGT = 13505,        // (math) Greater than
            ttLT = 13506,        // (math) Less than
            ttFEATURE = 13507,

            ttEQUALS = 13401,
            ttNOTEQUALS = 13402,

            ttBITAND = 13001,    // (math) bitwise and
            ttBITXOR = 12901,    // (math) bitwise xor
            ttBITOR = 12801,     // (math) bitwise or

            // ttNOT = 12101       'boolean not
            ttOR = 12201,        // boolean or
            ttAND = 12301,       // boolean and
            ttNOT = 12401,       // boolean not

            // Assignment Operators
            ttCOLON = 10101,
            ttEQUATE = 10102,
            ttGREATERTHAN = 10103,
            ttLESSTHAN = 10104,

            // Special operator
            ttImpliedAppend = 10501,

            // Grouping characters
            ttOPENPARENTHESIS = 30001,
            ttCLOSEPARENTHESIS = 30002,
            ttOPENBRACKET = 30003,
            ttCLOSEBRACKET = 30004,
            ttQUOTE = 30005,
            ttCOMMA = 30006,

            // Operands
            ttVarName = 20001,
            ttVarValue = 20002,
            ttVarValueCurrent = 20003,
            ttString = 20004,
            ttStringWithParenthesis = 20005,
            ttStringWithQuotes = 20006,
            ttStringWithBrackets = 20007,
            ttBoolean = 20008,
            ttArgCount = 20009,
            ttLevelNo = 20010,
            ttNUMBER = 20011,    // (math)
            ttECHO = 20012
        }

        public enum TokenGroup : byte
        {
            tgOperand = 1,
            tgGroupingChar = 2,
            tgAssignment = 3,
            tgBooleanFunc = 4,
            tgFunction = 5,
            tgPreProc = 6,
            tgImpliedAppend = 7,
            tgLineMarker = 8
        }

        public enum CFGEventType : byte
        {
            cfgMessage = 1,
            cfgWarning = 2,
            cfgError = 3,
            cfgCritical = 4,
            cfgVardef = 5,
            cfgVarlocked = 6,
            cfgVarundef = 7,
            cfgInclude = 8,
            cfgFinishInclude = 9,
            cfgLevelChange = 10,
            cfgVarCreated = 11,
            cfgAbort = 12,
            cfgBeingIncluded = 13
        }

        public struct MSVersion
        {
            public string version;
            public string rscVersion;
            public int developmentNumber;
            public int platform;
            public int productIDas;
            public string productName;
            public string platformName;
            public string buildName;
            public int betaBuild;
            public string productConfigVarName;
            public string productDirectoryName;
            public string productFileBaseName;
            public string productFileTypeName;
            public string productFullMarketingName;
            public string productGroupName;
            public string productMarketingSuffixName;
            public string productMarketingSuffixShortName;
            public string productProductName;
            public string productProductShortName;
            public string productShortMarketingName;
            public string productSubGroupName;

            public string productTitlebarName;
            public string productCopyrightYears;
            public string productBaseName;
            public string productShortName;

            // Public productMarketingSuffixName As String

        }

        [Serializable()]
        public struct VariableData
        {
            public string Name;
            public string Type;
            public string ShortDescription;
            public string LongDescription;
            public string Restart;
            public string Project;
            public string Version;
            public string Category;
            public string Application;
            public string Comment;
            public string Status;
        }

        [Serializable()]
        public struct VarInfo
        {
            public string Name;
            public string Type;
            public string ShortDesc;
            public string LongDesc;
            public string Restart;
            public string Project;
            public string Version;
            public string Category;
            public string App;
            public string Comment;
            public string Status;
        }


        [Serializable()]
        public struct Application
        {
            public string Name;
            public string Path;
            public string StartupCfgPath;
            public string CommandOptions;
            public string Iconfile;
            public int IconNumber;
            public string PredefinedPath;
            public string EXEPath;
            public string Workspace;
            public string Workset;
            public string Role;
            public string Rules;
            public string Version;
            public string CESettingsDir;
            public string LastSelectedWorkspace;
            public string LastSelectedWorkset;
            public string LastSelectedRole;
            public string VariableDBNames;
            public string PredefinedCfgNames;
            public string ConfigurationRoot;


            public static bool operator ==(Application obj1, Application obj2)
            {
                return AreEqual(obj1, obj2);
            }

            public static bool operator !=(Application obj1, Application obj2)
            {
                return !AreEqual(obj1, obj2);
            }

            public override bool Equals(object obj)
            {
                return obj is Application application && AreEqual(this, application);
            }

            public override int GetHashCode()
            {
                var compareInfo = CultureInfo.CurrentCulture.CompareInfo;
                const CompareOptions options = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
                var hash = new HashCode();

                hash.Add(compareInfo.GetHashCode(Name ?? "", options));
                hash.Add(compareInfo.GetHashCode(StartupCfgPath ?? "", options));
                hash.Add(compareInfo.GetHashCode(ConfigurationRoot ?? "", options));
                hash.Add(compareInfo.GetHashCode(CommandOptions ?? "", options));
                hash.Add(compareInfo.GetHashCode(PredefinedCfgNames ?? "", options));
                hash.Add(compareInfo.GetHashCode(EXEPath ?? "", options));
                hash.Add(compareInfo.GetHashCode(Workspace ?? "", options));
                hash.Add(compareInfo.GetHashCode(Workset ?? "", options));
                hash.Add(compareInfo.GetHashCode(Rules ?? "", options));
                hash.Add(compareInfo.GetHashCode(Version ?? "", options));

                return hash.ToHashCode();
            }

            private static bool AreEqual(Application obj1, Application obj2)
            {
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.Name ?? "", obj2.Name ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.StartupCfgPath ?? "", obj2.StartupCfgPath ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.ConfigurationRoot ?? "", obj2.ConfigurationRoot ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.CommandOptions ?? "", obj2.CommandOptions ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.PredefinedCfgNames ?? "", obj2.PredefinedCfgNames ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.EXEPath ?? "", obj2.EXEPath ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.Workspace ?? "", obj2.Workspace ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.Workset ?? "", obj2.Workset ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.Rules ?? "", obj2.Rules ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.Version ?? "", obj2.Version ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.Workspace ?? "", obj2.Workspace ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0 && CultureInfo.CurrentCulture.CompareInfo.Compare(obj1.Workset ?? "", obj2.Workset ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    return true;
                }

                return false;
            }
        }

        // class used to store data for application selector combos
        // contains applicaton name and application executable path which is uniqueness criteria for an application
        public class ShortApplication
        {
            public string Name { get; private set; }
            public string ExePath { get; private set; }

            public ShortApplication(string name, string exePath)
            {
                Name = name;
                ExePath = exePath;
            }
        }





        public struct ValidationResult : ICloneable
        {

            // Rule parameters
            public string variablename;
            public string opt;
            public string attr;
            public string inval;
            public string varval;

            public bool alert;         // rule raised an alert

            public bool iswarning;
            public bool iserror;
            public bool negation;

            public string status;

            // Record the result's origin
            public string rulename;
            public string linenumber;

            // File/Folder Test
            public bool filefoldertest;
            public bool isFile;
            public bool fileExists;
            public bool isFolder;
            public bool folderExists;

            public object Clone()
            {
                var x = new ValidationResult();

                x.variablename = variablename;
                x.opt = opt;
                x.attr = attr;
                x.inval = inval;
                x.iswarning = iswarning;
                x.iserror = iserror;
                x.negation = negation;
                x.status = status;
                x.rulename = rulename;
                x.linenumber = linenumber;

                return x;
            }
        }

        public enum ConfigurationType : int
        {
            Unknown = 0,
            Main = 1,
            Compare1 = 2,
            Compare2 = 3
        }

        public enum TokenType
        {
            TOKEN_NotStarted = 0,
            TOKEN_EndOfString = -1,
            TOKEN_BadToken = -2,
            TOKEN_LeftParen = 1,
            TOKEN_RightParen = 2,
            TOKEN_LogicalOr = 3,
            TOKEN_LogicalAnd = 4,
            TOKEN_LogicalNegate = 5,
            TOKEN_PreProcDefined = 6,
            TOKEN_PreProcExists = 7,
            TOKEN_Symbol = 8,
            TOKEN_BinaryOr = 9, // InclusiveOrExpression | ExclusiveOrExpression
            TOKEN_BinaryXor = 10, // ExclusiveOrExpression ^ AndExpression
            TOKEN_BinaryAnd = 11, // ConditionalAndExpression && InclusiveOrExpression
            TOKEN_Equals = 12, // EqualityExpression == RelationalExpression
            TOKEN_NotEquals = 13, // EqualityExpression != RelationalExpression
            TOKEN_GreaterThan = 14, // RelationalExpression > ShiftExpression
            TOKEN_LessThan = 15, // RelationalExpression < ShiftExpression
            TOKEN_GreaterOrEqual = 16, // RelationalExpression >= ShiftExpression
            TOKEN_LessOrEqual = 17, // RelationalExpression <= ShiftExpression
            TOKEN_ShiftLeft = 18, // ShiftExpression << AdditiveExpression
            TOKEN_ShiftRight = 19, // ShiftExpression >> AdditiveExpression
            TOKEN_Add = 20, // AdditiveExpression + MultiplicativeExpression
            TOKEN_Subtract = 21, // AdditiveExpression - MultiplicativeExpression
            TOKEN_Multiply = 22, // MultiplicativeExpression * UnaryExpression
            TOKEN_Divide = 23, // MultiplicativeExpression / UnaryExpression
            TOKEN_Modulus = 24, // MultiplicativeExpression % UnaryExpression
            TOKEN_StringConstant = 25,
            TOKEN_Hex = 26,
            TOKEN_Octal = 27,
            TOKEN_Char = 28,
            TOKEN_PreProcessorCommand = 29
        }

    }
}