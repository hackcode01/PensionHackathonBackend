using System.Collections.Generic;
using System.IO;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace PensionHackathonBackend.Endpoints;

public static class PdfServiceEndpoint
{
    /* Добавление методов для отображения запросов по работе с Pdf файлами */
    public static IEndpointRouteBuilder AddPdfServiceEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("GeneratePdf", GeneratePdf);

        return app;
    }

    /* Генерация Pdf файла из табличных данных */
    private static IResult GeneratePdf([FromBody] List<KeyValuePair<int, string>> data)
    {
        using var memoryStream = new MemoryStream();
        var pdfWriter = new PdfWriter(memoryStream);
        var pdfDocument = new PdfDocument(pdfWriter);
        var document = new Document(pdfDocument);

        document.Add(new Paragraph("Таблица Идентификатор - Значение")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(18));

        /* Создание таблицы с двумя столбцами */
        var table = new Table(UnitValue.CreatePercentArray([1, 2]))
            .UseAllAvailableWidth();

        /* Добавление заголовков столбцов */
        table.AddHeaderCell("Идентификатор");
        table.AddHeaderCell("Значение");

        /* Добавление строк данных */
        foreach (var pair in data)
        {
            table.AddCell(pair.Key.ToString());
            table.AddCell(pair.Value);
        }

        /* Добавление таблицы в документ */
        document.Add(table);
        document.Close();

        var fileName = "Отчет.pdf";

        return Results.File(memoryStream.ToArray(), "application/pdf", fileName);
    }
}