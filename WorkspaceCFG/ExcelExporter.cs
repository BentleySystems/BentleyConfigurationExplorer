// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System.Drawing;
using System.IO;

namespace WorkspaceCFG
{
    // ---------------------------------------------------------------------------------------
    // 
    // Excel Exporter - Saves a Datagridview to an excel file
    // 
    // ---------------------------------------------------------------------------------------

    public static class ExcelExporter
    {

        private static string CellStyletoExcel(ref System.Windows.Forms.DataGridViewRow inrow)
        {
            if (inrow.DefaultCellStyle.BackColor == Color.DarkRed)
                return "3";
            if (inrow.DefaultCellStyle.BackColor == Color.DarkOrange)
                return "4";
            if (inrow.DefaultCellStyle.ForeColor == Color.Blue)
                return "5";
            if (inrow.DefaultCellStyle.ForeColor == Color.LightGray)
                return "6";
            if (inrow.DefaultCellStyle.ForeColor == Color.Red)
                return "7";

            return "2";
        }

        public static void DataGridExport(System.Windows.Forms.DataGridView grdView, ref string SheetName, string filePath)
        {
            // Open the file
            using (var streamWriter = new StreamWriter(filePath, false))
            {
                // Create Header
                var argstreamWriter = streamWriter;
                WriteHeader(ref argstreamWriter, ref SheetName);

                var argstreamWriter1 = streamWriter;
                WriteColumns(ref argstreamWriter1, grdView);

                // Write contents for each cell
                var argstreamWriter2 = streamWriter;
                WriteCells(ref argstreamWriter2, grdView);

                // Close  the document
                streamWriter.WriteLine("  </ss:Table>");
                streamWriter.WriteLine("</ss:Worksheet>");
                streamWriter.WriteLine("</ss:Workbook>");
            }
        }

        public static void DataGridExport(System.Windows.Forms.DataGridView DG1, System.Windows.Forms.DataGridView DG2, ref string SheetName, string filePath)
        {
            // Open the file
            using (var streamWriter = new StreamWriter(filePath, false))
            {
                // Create Header
                var argstreamWriter = streamWriter;
                WriteHeader(ref argstreamWriter, ref SheetName);

                var argstreamWriter1 = streamWriter;
                WriteColumns(ref argstreamWriter1, DG1, DG2);

                // Write contents for each cell
                var argstreamWriter2 = streamWriter;
                WriteCells(ref argstreamWriter2, DG1, DG2);

                // Close  the document
                streamWriter.WriteLine("  </ss:Table>");
                streamWriter.WriteLine("</ss:Worksheet>");
                streamWriter.WriteLine("</ss:Workbook>");
            }
        }

        private static string LimitLength(ref string str)
        {
            // Remove ", >, < from the string
            return str.Replace("\"", "").Replace(">", "").Replace("<", "");
        }

        private static void WriteCells(ref StreamWriter streamWriter, System.Windows.Forms.DataGridView grdView)
        {
            int subtractBy;
            string cellText;
            string cellstyle;

            // Check for an empty row at the end due to Adding allowed on the DataGridView
            if (grdView.AllowUserToAddRows == true)
                subtractBy = 2;
            else
                subtractBy = 1;

            // Write each cell
            for (int i = 0, loopTo = grdView.RowCount - subtractBy; i <= loopTo; i++)
            {
                streamWriter.WriteLine(string.Format("    <ss:Row ss:Height=\"{0}\">", grdView.Rows[i].Height));
                var tmp = grdView.Rows;
                var arginrow = tmp[i];
                cellstyle = CellStyletoExcel(ref arginrow);
                for (int intCol = 0, loopTo1 = grdView.Columns.Count - 1; intCol <= loopTo1; intCol++)
                {
                    if (grdView[intCol, i].Value is not null)
                    {
                        cellText = grdView[intCol, i].Value.ToString();
                        // Check for null cell and change it to empty to avoid error
                        if (string.IsNullOrEmpty(cellText))
                            cellText = "";
                        string localLimitLength() { string argstr = cellText.ToString(); var ret = LimitLength(ref argstr); return ret; }

                        streamWriter.WriteLine(string.Format("      <ss:Cell ss:StyleID=\"" + cellstyle + "\">" + "<ss:Data ss:Type=\"String\">{0}</ss:Data></ss:Cell>", localLimitLength()));
                    }
                }
                streamWriter.WriteLine("    </ss:Row>");
            }
        }

        private static void WriteCells(ref StreamWriter streamWriter, System.Windows.Forms.DataGridView DG1, System.Windows.Forms.DataGridView DG2)
        {
            int subtractBy;
            string cellText;
            string cellstyle;

            // Check for an empty row at the end due to Adding allowed on the DataGridView
            if (DG1.AllowUserToAddRows == true)
                subtractBy = 2;
            else
                subtractBy = 1;

            // Write each cell
            for (int i = 0, loopTo = DG1.RowCount - subtractBy; i <= loopTo; i++)
            {
                streamWriter.WriteLine(string.Format("    <ss:Row ss:Height=\"{0}\">", DG1.Rows[i].Height));
                var tmp = DG1.Rows;
                var arginrow = tmp[i];
                cellstyle = CellStyletoExcel(ref arginrow);
                for (int intCol = 0, loopTo1 = DG1.Columns.Count - 1; intCol <= loopTo1; intCol++)
                {
                    if (DG1.Columns[intCol].Visible)
                    {
                        cellText = DG1[intCol, i].Value.ToString();
                        // Check for null cell and change it to empty to avoid error
                        if (string.IsNullOrEmpty(cellText))
                            cellText = "";
                        streamWriter.WriteLine(string.Format("      <ss:Cell ss:StyleID=\"" + cellstyle + "\">" + "<ss:Data ss:Type=\"String\">{0}</ss:Data></ss:Cell>", cellText.ToString()));
                    }
                }

                var tmp1 = DG2.Rows;
                var arginrow1 = tmp1[i];
                cellstyle = CellStyletoExcel(ref arginrow1);
                for (int intCol = 0, loopTo2 = DG2.Columns.Count - 1; intCol <= loopTo2; intCol++)
                {
                    if (DG2.Columns[intCol].Visible)
                    {
                        cellText = DG2[intCol, i].Value.ToString();
                        // Check for null cell and change it to empty to avoid error
                        if (string.IsNullOrEmpty(cellText))
                            cellText = "";
                        streamWriter.WriteLine(string.Format("      <ss:Cell ss:StyleID=\"" + cellstyle + "\">" + "<ss:Data ss:Type=\"String\">{0}</ss:Data></ss:Cell>", cellText.ToString()));
                    }
                }

                streamWriter.WriteLine("    </ss:Row>");
            }
        }

        private static void WriteColumns(ref StreamWriter streamWriter, System.Windows.Forms.DataGridView grdView)
        {
            int width;

            // Write the width value for each column (max of 1000 else it raises an error)
            for (int i = 0, loopTo = grdView.Columns.Count - 1; i <= loopTo; i++)
            {
                width = grdView.Columns[i].Width;
                if (width > 1000)
                    width = 1000;
                streamWriter.WriteLine(string.Format("    <ss:Column ss:Width=\"{0}\"/>", width));
            }

            streamWriter.WriteLine("    <ss:Row>");
            for (int i = 0, loopTo1 = grdView.Columns.Count - 1; i <= loopTo1; i++)
                streamWriter.WriteLine(string.Format("      <ss:Cell ss:StyleID=\"1\"><ss:Data ss:Type=\"String\">{0}</ss:Data></ss:Cell>", grdView.Columns[i].HeaderText));
            streamWriter.WriteLine("    </ss:Row>");
        }

        private static void WriteColumns(ref StreamWriter streamWriter, System.Windows.Forms.DataGridView DG1, System.Windows.Forms.DataGridView DG2)
        {
            int width;

            // Write the width value for each column (max of 1000 else it raises an error)
            for (int i = 0, loopTo = DG1.Columns.Count - 1; i <= loopTo; i++)
            {
                if (DG1.Columns[i].Visible)
                {
                    width = DG1.Columns[i].Width;
                    if (width > 1000)
                        width = 1000;
                    streamWriter.WriteLine(string.Format("    <ss:Column ss:Width=\"{0}\"/>", width));
                }
            }

            for (int i = 0, loopTo1 = DG2.Columns.Count - 1; i <= loopTo1; i++)
            {
                if (DG2.Columns[i].Visible)
                {
                    width = DG2.Columns[i].Width;
                    if (width > 1000)
                        width = 1000;
                    streamWriter.WriteLine(string.Format("    <ss:Column ss:Width=\"{0}\"/>", width));
                }
            }

            streamWriter.WriteLine("    <ss:Row>");
            for (int i = 0, loopTo2 = DG1.Columns.Count - 1; i <= loopTo2; i++)
            {
                if (DG1.Columns[i].Visible)
                {
                    streamWriter.WriteLine(string.Format("      <ss:Cell ss:StyleID=\"1\"><ss:Data ss:Type=\"String\">{0}</ss:Data></ss:Cell>", DG1.Columns[i].HeaderText));
                }
            }
            for (int i = 0, loopTo3 = DG1.Columns.Count - 1; i <= loopTo3; i++)
            {
                if (DG2.Columns[i].Visible)
                {
                    streamWriter.WriteLine(string.Format("      <ss:Cell ss:StyleID=\"1\"><ss:Data ss:Type=\"String\">{0}</ss:Data></ss:Cell>", DG2.Columns[i].HeaderText));
                }
            }
            streamWriter.WriteLine("    </ss:Row>");
        }

        private static void WriteHeader(ref StreamWriter streamWriter, ref string SheetName)
        {
            streamWriter.WriteLine("<?xml version=\"1.0\"?>");
            streamWriter.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
            streamWriter.Write("<ss:Workbook xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            streamWriter.Write(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            streamWriter.Write(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            streamWriter.Write(" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");
            streamWriter.WriteLine("");
            // Create the styles for the worksheet
            WriteStyles(ref streamWriter);

            // Write the worksheet contents
            streamWriter.WriteLine("<ss:Worksheet ss:Name=\"" + SheetName + "\">");
            streamWriter.WriteLine("  <ss:Table>");
        }

        private static void WriteStyles(ref StreamWriter streamWriter)
        {
            streamWriter.WriteLine("  <ss:Styles>");
            // <Font ss:Size="8"/>
            // Style for the column information
            streamWriter.WriteLine("    <ss:Style ss:ID=\"1\">");
            streamWriter.WriteLine("      <ss:Font ss:Size=\"8\" ss:Bold=\"1\"/>");
            streamWriter.WriteLine("      <ss:Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\" " + "ss:WrapText=\"1\"/>");
            streamWriter.WriteLine("      <ss:Interior ss:Color=\"#C0C0C0\" ss:Pattern=\"Solid\"/>");
            streamWriter.WriteLine("    </ss:Style>");

            // Typical Cell
            streamWriter.WriteLine("    <ss:Style ss:ID=\"2\">");
            streamWriter.WriteLine("      <ss:Alignment ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
            streamWriter.WriteLine("      <ss:Font ss:Size=\"8\"/>");
            streamWriter.WriteLine("    </ss:Style>");

            // Red Fill / White Text
            streamWriter.WriteLine("    <ss:Style ss:ID=\"3\">");
            streamWriter.WriteLine("      <ss:Alignment ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
            streamWriter.WriteLine("      <ss:Font ss:Size=\"8\" ss:Color=\"#FFFFFF\"/>");
            streamWriter.WriteLine("      <ss:Interior ss:Color=\"#800000\" ss:Pattern=\"Solid\"/>");
            streamWriter.WriteLine("    </ss:Style>");

            // Orange Fill
            streamWriter.WriteLine("    <ss:Style ss:ID=\"4\">");
            streamWriter.WriteLine("      <ss:Alignment ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
            streamWriter.WriteLine("      <ss:Font ss:Size=\"8\"/>");
            streamWriter.WriteLine("      <ss:Interior ss:Color=\"#FF6600\" ss:Pattern=\"Solid\"/>");
            streamWriter.WriteLine("    </ss:Style>");

            // Blue Text
            streamWriter.WriteLine("    <ss:Style ss:ID=\"5\">");
            streamWriter.WriteLine("      <ss:Alignment ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
            streamWriter.WriteLine("      <ss:Font ss:Size=\"8\" ss:Color=\"#0000FF\"/>");
            streamWriter.WriteLine("    </ss:Style>");

            // Light Grey Text
            streamWriter.WriteLine("    <ss:Style ss:ID=\"6\">");
            streamWriter.WriteLine("      <ss:Alignment ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
            streamWriter.WriteLine("      <ss:Font ss:Size=\"8\" ss:Color=\"#C0C0C0\"/>");
            streamWriter.WriteLine("    </ss:Style>");

            // RED Text
            streamWriter.WriteLine("    <ss:Style ss:ID=\"7\">");
            streamWriter.WriteLine("      <ss:Alignment ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
            streamWriter.WriteLine("      <ss:Font ss:Size=\"8\" ss:Color=\"#FF0000\"/>");
            streamWriter.WriteLine("    </ss:Style>");
            streamWriter.WriteLine("  </ss:Styles>");
        }

    }
}