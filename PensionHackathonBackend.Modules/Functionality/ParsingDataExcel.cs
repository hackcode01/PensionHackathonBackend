using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace PensionHackathonBackend.Modules.Functionality
{
    /* Класс для парсинга данных из Excel файлов */
    public class ParsingDataExcel
    {
        /* Метод получения заголовков столбцов Excel документа */
        private static List<string> GetColumnHeaders(ExcelWorksheet worksheet)
        {
            List<string> columnHeaders = [];
            for (int column = 1; column <= worksheet.Dimension.Columns; ++column)
            {
                string header = worksheet.Cells[1, column].Value?.ToString() ??
                    string.Empty;
                columnHeaders.Add(header);
            }

            return columnHeaders;
        }

        /* Парсинг Excel документа в строковый словарь */
        public static List<Dictionary<string, string>> ParseExcelFile(string filePath)
        {
            List<Dictionary<string, string>> parseData = [];

            using ExcelPackage package = new(new FileInfo(filePath));
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            List<string> columnHeaders = GetColumnHeaders(worksheet);

            for (int row = 2; row <= worksheet.Dimension.Rows; ++row)
            {
                Dictionary<string, string> rowData = [];
                for (int column = 1; column <= columnHeaders.Count; ++column)
                {
                    string cellValue = worksheet.Cells[row, column].Value?.ToString() ?? string.Empty;

                    rowData.Add(columnHeaders[column - 1], cellValue);
                }

                parseData.Add(rowData);
            }

            return parseData;
        }

        /* Вывод данных из Excel документа в консоль */
        public static void PrintData(string excelFilePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            List<Dictionary<string, string>> data = ParseExcelFile(excelFilePath);

            foreach (Dictionary<string, string> rowData in data)
            {
                Console.WriteLine("------------------");
                foreach (var kvp in rowData)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
        }
    }
}
