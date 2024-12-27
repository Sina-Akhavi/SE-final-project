using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class ExcelExporterOpenXml
{
    public void ExportToExcel<T>(IEnumerable<T> dataList, Stream outputStream, string sheetName = "Sheet1")
    {
        using (var spreadsheetDocument = SpreadsheetDocument.Create(outputStream, SpreadsheetDocumentType.Workbook))
        {
            // Create the Workbook
            WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            // Create a worksheet part
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };
            sheets.Append(sheet);

            // Get the SheetData object to append rows
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Get the properties of T using reflection
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Write Header Row
            Row headerRow = new Row();
            foreach (var prop in properties)
            {
                Cell headerCell = new Cell
                {
                    DataType = CellValues.String,
                    CellValue = new CellValue(prop.Name)
                };
                headerRow.Append(headerCell);
            }
            sheetData.Append(headerRow);

            // Write Data Rows
            foreach (var item in dataList)
            {
                Row dataRow = new Row();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item, null)?.ToString() ?? string.Empty;
                    Cell dataCell = new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(value)
                    };
                    dataRow.Append(dataCell);
                }
                sheetData.Append(dataRow);
            }

            // Save and close
            workbookPart.Workbook.Save();
        }
    }
}
