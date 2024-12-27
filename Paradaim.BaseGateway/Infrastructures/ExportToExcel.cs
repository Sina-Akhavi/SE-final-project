using System.Data;
using ClosedXML.Excel;
using Paradaim.BaseGateway.Infrastructures.Extensions;

namespace Paradaim.BaseGateway.Infrastructures
{
    public class ExportToExcel
    {
        public static void Export(DataSet dataSet, string path)
        {
            var workbook = new XLWorkbook();
            foreach (DataTable table in dataSet.Tables)
            {
                var worksheet = workbook.Worksheets.Add(table.TableName);

                // Add the column headers to the worksheet
                for (int colIndex = 0; colIndex < table.Columns.Count; colIndex++)
                {
                    worksheet.Cell(1, colIndex + 1).Value = table.Columns[colIndex].ColumnName;
                }

                // Add the rows to the worksheet
                for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < table.Columns.Count; colIndex++)
                    {
                        var cellValue = table.Rows[rowIndex][colIndex];
                        worksheet.Cell(rowIndex + 2, colIndex + 1).Value = cellValue?.ToString() ?? string.Empty;
                    }
                }
            }
            workbook.SaveAs(path);
        }


        public static void Export<T>(List<T> data, string path)
        {
            DataTable table = data.ToDataTable();
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            Export(dataSet, path);
        }
        public static void Export(DataTable table, string path)
        {
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            Export(dataSet, path);
        }
    }
}
