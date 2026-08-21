// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using WorkspaceCFG.My.Resources;

namespace WorkspaceCFG
{

    static class ExcelUtilities
    {

        public static void ExportFileList2(ref CFGConfiguration workspace)
        {
            if (workspace is null)
                return;
            var savefiledialog = new System.Windows.Forms.SaveFileDialog();
            string path;

            // TODO: this will throw an exception if the Excel file is open
            savefiledialog.Title = "Export to Excel";
            savefiledialog.InitialDirectory = CEResource.TXT_CDrive;
            savefiledialog.Filter = CEResource.TXT_FileFilterXlsx;
            savefiledialog.FileName = workspace.ApplicationData.Name;
            savefiledialog.RestoreDirectory = true;
            if (savefiledialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            path = savefiledialog.FileName;
            savefiledialog.Dispose();

            // Workbook
            var spreadsheetDocument = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);
            var workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

            // TODO: Add stylesheet
            // Dim stylePart As WorkbookStylesPart = workbookpart.AddNewPart(Of WorkbookStylesPart)()
            // stylePart.Stylesheet = GenerateStylesheet()
            // stylePart.Stylesheet.Save()

            #region Sheet 1 - Summary

            var worksheetPart1 = workbookpart.AddNewPart<WorksheetPart>();
            var workSheet1 = new Worksheet();
            var sheetData1 = new SheetData();

            // Header
            var headerRow = new Row();
            string[] headerCells = new[] { "Variable Name", "Level", "Variable Value", "Variable Definition and Options", "Variable Location" };

            foreach (string header in headerCells)
            {
                var cell = new Cell()
                {
                    DataType = (EnumValue<CellValues>)CellValues.String,
                    CellValue = new CellValue(header)
                };
                headerRow.Append(cell);
            }

            sheetData1.Append(headerRow);

            // Data
            foreach (CFGVariable variable in workspace.Variables.Values)
            {
                var rowInSheet1 = new Row();
                var variableCell = new Cell()
                {
                    DataType = (EnumValue<CellValues>)CellValues.String,
                    CellValue = new CellValue(variable.Name)
                };
                rowInSheet1.Append(variableCell);

                var levelCell = new Cell()
                {
                    DataType = (EnumValue<CellValues>)CellValues.String,
                    CellValue = new CellValue(Utilities.GetLevelName(variable.Level))
                };
                rowInSheet1.Append(levelCell);

                var expansionCell = new Cell()
                {
                    DataType = (EnumValue<CellValues>)CellValues.String,
                    CellValue = new CellValue(variable.FinalExpansion)
                };
                rowInSheet1.Append(expansionCell);

                var descriptionCell = new Cell()
                {
                    DataType = (EnumValue<CellValues>)CellValues.String,
                    CellValue = new CellValue(Utilities.GetVariableLongDescription(variable.Name)?.Trim())
                };
                rowInSheet1.Append(descriptionCell);

                string cellValue = "";
                var files = new Dictionary<string, string>();

                var filesCell = new Cell()
                {
                    DataType = (EnumValue<CellValues>)CellValues.String,
                    StyleIndex = (UInt32Value)0
                };

                foreach (CFGEvent ev in workspace.CFGEvents)
                {
                    if (ev.Variable is not null && CultureInfo.CurrentCulture.CompareInfo.Compare(ev.Variable.Name ?? "", variable.Name ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    {
                        if (ev.ParentFile is not null)
                        {
                            if (files.ContainsKey(ev.ParentFile.FilePath) == false)
                            {
                                files.Add(ev.ParentFile.FilePath, ev.ParentFile.FilePath);
                            }
                        }
                    }
                }

                foreach (string @file in files.Values)
                    cellValue += @file + '\n';
                filesCell.CellValue = new CellValue(cellValue);
                rowInSheet1.Append(filesCell);
                sheetData1.Append(rowInSheet1);
            }

            workSheet1.AppendChild(sheetData1);
            worksheetPart1.Worksheet = workSheet1;

            var sheet1 = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart1),
                SheetId = (UInt32Value)1,
                Name = "Summary"
            };
            sheets.Append(sheet1);

            #endregion

            #region Sheet 2 - Loading Order
            var worksheetPart2 = workbookpart.AddNewPart<WorksheetPart>();
            var workSheet2 = new Worksheet();
            var sheetData2 = new SheetData();

            // data for sheet 2
            foreach (CFGFile cfile in workspace.CFGFiles)
            {
                var rowInSheet2 = new Row();

                // Cell padding
                for (int i = 0, loopTo = cfile.Depth - 1; i <= loopTo; i++)
                {
                    var passingCell = new Cell()
                    {
                        DataType = (EnumValue<CellValues>)CellValues.String,
                        CellValue = new CellValue("")
                    };
                    rowInSheet2.Append(passingCell);
                }

                // Space padding
                // Dim padding As String = ""
                // For i As Integer = 0 To cfile.Depth
                // padding &= "----"
                // Next

                var cell = new Cell()
                {
                    DataType = (EnumValue<CellValues>)CellValues.String,
                    CellValue = new CellValue(cfile.FilePath)
                };
                rowInSheet2.Append(cell);
                sheetData2.Append(rowInSheet2);
            }

            workSheet2.AppendChild(sheetData2);
            worksheetPart2.Worksheet = workSheet2;

            var sheet2 = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart2),
                SheetId = (UInt32Value)2,
                Name = "Loading Order"
            };
            sheets.Append(sheet2);
            #endregion

            workbookpart.Workbook.Save();
            spreadsheetDocument.Dispose();

            CreateWorkspaceTreeImage(ref workspace);
        }

        public static void CreateWorkspaceTreeImage(ref CFGConfiguration workspace)
        {

            var bmp = new Bitmap(1200, 2400);
            var g = Graphics.FromImage(bmp);
            g.Clear(System.Drawing.Color.White);

            int xStep = 24;
            int yStep = 24;
            int x = xStep;
            int y = yStep;

            var font = new System.Drawing.Font("Arial", 10f);
            var fontBold = new System.Drawing.Font("Arial", 10f, FontStyle.Bold);
            var brush = new SolidBrush(System.Drawing.Color.Black);

            g.DrawString(workspace.ApplicationData.Name, fontBold, brush, x, y);
            y += 2 * yStep;

            foreach (CFGFile cfile in workspace.CFGFiles)
            {
                g.DrawString(cfile.FilePath, font, brush, x + cfile.Depth * xStep, y);

                y += yStep;
            }

            bmp = ResizeBitmapToFitContents(bmp, xStep, new Size(600, 600));
            bmp.Save(@"c:\temp\workspace_tree.bmp");
            bmp.Dispose();
        }

        private static Bitmap ResizeBitmapToFitContents(Bitmap bmp, int margin, Size minimumSize)
        {
            // Find the bounds of the drawn content
            var bounds = FindContentBounds(bmp);

            // Add margin to the bounds
            bounds.Inflate(margin, margin);

            // Ensure the size meets the minimum size requirement
            bounds.Width = Math.Max(bounds.Width, minimumSize.Width);
            bounds.Height = Math.Max(bounds.Height, minimumSize.Height);

            // Create a new bitmap with the resized dimensions
            var resizedBitmap = new Bitmap(bounds.Width, bounds.Height);
            var g = Graphics.FromImage(resizedBitmap);

            // Draw the original bitmap onto the resized bitmap
            g.DrawImage(bmp, new Rectangle(0, 0, bounds.Width, bounds.Height), bounds, GraphicsUnit.Pixel);

            return resizedBitmap;
        }

        private static Rectangle FindContentBounds(Bitmap bmp)
        {
            var nonTransparentColor = bmp.GetPixel(0, 0);

            // Find the bounds of the non-transparent pixels
            int minX = bmp.Width;
            int minY = bmp.Height;
            int maxX = 0;
            int maxY = 0;

            for (int y = 0, loopTo = bmp.Height - 1; y <= loopTo; y++)
            {
                for (int x = 0, loopTo1 = bmp.Width - 1; x <= loopTo1; x++)
                {
                    var pixelColor = bmp.GetPixel(x, y);
                    if (pixelColor != nonTransparentColor)
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }

            // Calculate the bounds
            int width = maxX - minX + 1;
            int height = maxY - minY + 1;

            return new Rectangle(minX, minY, width, height);
        }

    }
}