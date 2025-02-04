using Spire.Xls;
using System.Data;
using System.IO;

namespace PensionHackathonBackend.Modules.Functionality
{
    public class ReadExcelToTextAndDataTables
    {
        /* Чтение данных из Excel документа в строковый файл */
        public static void ReadExcelToText(string filePath)
        {
            Workbook workbook = new();
            workbook.LoadFromFile(filePath);

            Worksheet worksheet = workbook.Worksheets[0];

            string outputFile = "Output.txt";
            StreamWriter writer = new(outputFile);

            for (int row = 1; row <= worksheet.LastRow; ++row)
            {
                for (int column = 1; column <= worksheet.LastColumn; ++column)
                {
                    CellRange range = worksheet.Range[row, column];
                    string cellValue = range.Text == null ? string.Empty :
                        range.Text.ToString();

                    writer.Write(cellValue + '\t');
                }

                writer.WriteLine();
            }

            writer.Close();

            workbook.Dispose();
        }

        /* Чтение данных из Excel документа и возврат в табличной форме */
        public static DataTable ReadToDataTable(string filePath)
        {
            Workbook workbook = new();

            workbook.LoadFromFile(filePath);

            Worksheet worksheet = workbook.Worksheets[0];

            workbook.Dispose();

            return worksheet.ExportDataTable();
        }
    }
}
