using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Newtonsoft.Json;

namespace PensionHackathonBackend.Modules.Functionality;

[Serializable]
public class Result
{
    public int Id { get; set; }
    public float Value { get; set; }
}

public static class PdfCreator
{
    public static byte[] CreatePDF(string data)
    {
        var result = JsonConvert.DeserializeObject < List<Result>>(data);
        
        using (var memoryStream = new MemoryStream())
        {
            var pdfWriter = new PdfWriter(memoryStream);
            var pdfDocument = new PdfDocument(pdfWriter);
            var document = new Document(pdfDocument);
            
            document.Add(new Paragraph("Id-Value Table")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18));

            var table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2 }))
                .UseAllAvailableWidth();

            table.AddHeaderCell("ID");
            table.AddHeaderCell("Value");

            // Добавление строк данных
            foreach (var pair in result)
            {
                table.AddCell(pair.Id.ToString());
                table.AddCell(pair.Value.ToString());
            }

            // Добавление таблицы в документ
            document.Add(table);
            document.Close();
            
            return memoryStream.ToArray();
        }
    }
}