// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using Microsoft.Win32;
using System.Drawing;
using System.Security.Permissions;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;

namespace Bentley.AboutApp.Common
{
/*====================================================================================**/
/// <summary></summary>
/*==============+===============+===============+===============+===============+======*/
public class CommonUtil
{
private static bool m_isAboutOpen = false;
private static bool m_isLegalNoticeOpen = false;

/*------------------------------------------------------------------------------------**/
/// <summary>
/// LegalForm  Status.
/// </summary>
/*--------------+---------------+---------------+---------------+---------------+------*/
public static bool IsLegalNoticeOpen
    {
    get
        {
        return m_isLegalNoticeOpen;
        }
    set
        {
        m_isLegalNoticeOpen = value;
        }
    }

/*------------------------------------------------------------------------------------**/
/// <summary>
/// About Status.
/// </summary>
/*--------------+---------------+---------------+---------------+---------------+------*/
public static bool IsAboutOpen
    {
    get
        {
        return m_isAboutOpen;
        }
    set
        {
        m_isAboutOpen = value;
        }
    }
}
}
