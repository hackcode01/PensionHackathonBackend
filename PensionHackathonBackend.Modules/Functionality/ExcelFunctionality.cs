using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Linq;
using System.Text;

namespace PensionHackathonBackend.Modules.Functionality
{
    /* Класс по работе с Excel файлами */
    public class ExcelFunctionality
    {
        /* Метод по чтению Excel файлов */
        public static void ReadExcelFile(string filePath)
        {
            try
            {
                using SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false);
                WorkbookPart workbookPart = document.WorkbookPart;
                Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
                StringBuilder excelResult = new();

                foreach (Sheet sheet in sheets.Cast<Sheet>())
                {
                    excelResult.AppendLine("Excel Sheet Name: " + sheet.Name);
                    excelResult.AppendLine("-------------------------------");

                    Worksheet worksheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;

                    SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                    foreach (Row row in sheetData.Cast<Row>())
                    {
                        foreach (Cell cell in row.Cast<Cell>())
                        {
                            string currentCellValue = string.Empty;

                            if (cell.DataType != null)
                            {
                                if (int.TryParse(cell.InnerText, out int id))
                                {
                                    SharedStringItem item = workbookPart
                                        .SharedStringTablePart.SharedStringTable
                                        .Elements<SharedStringItem>().ElementAt(id);

                                    if (item.Text != null)
                                    {
                                        excelResult.Append(item.Text.Text + " ");
                                    }
                                    else if (item.InnerText != null)
                                    {
                                        currentCellValue = item.InnerText;
                                    }
                                    else if (item.InnerXml != null)
                                    {
                                        currentCellValue = item.InnerXml;
                                    }
                                }
                            }
                            else
                            {
                                excelResult.Append(Convert.ToInt16(cell.InnerText));
                            }
                        }
                        excelResult.AppendLine();
                    }
                    excelResult.Append("");
                    Console.WriteLine(excelResult.ToString());
                    Console.ReadLine();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /* Метод по записи данных в Excel файл */
        public static void WriteExcelFile(string filePath)
        {
            using SpreadsheetDocument document = SpreadsheetDocument.Create(filePath,
                SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new();

            SheetData sheetData = new();
            worksheetPart.Worksheet.Append(sheetData);

            Row row_1 = new() { RowIndex = 1 };
            Cell cellA1 = new()
            {
                CellReference = "A1",
                DataType = CellValues.String,
                InnerXml = "Имя"
            };
            Cell cellB1 = new()
            {
                CellReference = "B1",
                DataType = CellValues.String,
                InnerXml = "Возраст"
            };
            row_1.Append(cellA1);
            row_1.Append(cellB1);
            sheetData.Append(row_1);

            Row row_2 = new() { RowIndex = 2 };
            Cell cellA2 = new()
            {
                CellReference = "A2",
                DataType = CellValues.String,
                InnerXml = "Иван"
            };
            Cell cellB2 = new()
            {
                CellReference = "B2",
                DataType = CellValues.Number,
                InnerXml = "25"
            };
            row_2.Append(cellA2);
            row_2.Append(cellB2);
            sheetData.Append(row_2);

            workbookPart.Workbook.Save();
        }

        /* Метод по созданию Excel файла */
        public static void CreateExcelFile(string filePath)
        {
            using SpreadsheetDocument document = SpreadsheetDocument.Create(filePath,
                SpreadsheetDocumentType.Workbook);
            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new();

            SheetData sheetData = new();
            worksheetPart.Worksheet.Append(sheetData);

            workbookPart.Workbook.Save();
        }
    }
}
