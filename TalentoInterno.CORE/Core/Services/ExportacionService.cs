// --- LIBRERÍAS DE EXCEL ---
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
// --- LIBRERÍAS DE PDF ---
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing; // Para System.Drawing.Color
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data.Pdf;

namespace TalentoInterno.CORE.Core.Services;

public class ExportacionService : IExportacionService
{
    private readonly IConfiguration _config;
    public ExportacionService(IConfiguration config)
    {
        _config = config;
        // Esto soluciona los errores de inicialización y asegura el uso no comercial
        ExcelPackage.License.SetNonCommercialPersonal("TalentoInterno User");
    }

    public async Task<byte[]> GenerarRankingExcel(IEnumerable<MatchResultDTO> rankingData)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Ranking de Candidatos");
            var colorAzul = System.Drawing.Color.FromArgb(31, 78, 121);

            // ✅ NUEVOS HEADERS
            worksheet.Cells["A1"].Value = "Colaborador";
            worksheet.Cells["B1"].Value = "ID Colaborador";
            worksheet.Cells["C1"].Value = "Porcentaje Match";
            worksheet.Cells["D1"].Value = "Skills Cumplidas";
            worksheet.Cells["E1"].Value = "Skills Faltantes";

            // ✅ ESTILO HEADER
            using (var range = worksheet.Cells["A1:E1"])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.Font.Color.SetColor(colorAzul);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            int row = 2;
            foreach (var candidato in rankingData ?? Enumerable.Empty<MatchResultDTO>())
            {
                // ✅ USAR NOMBRE YA EXISTENTE EN DTO
                worksheet.Cells[row, 1].Value = candidato.Nombre ?? "—";
                worksheet.Cells[row, 2].Value = candidato.ColaboradorId;

                worksheet.Cells[row, 3].Value = candidato.PorcentajeMatch / 100.0;
                worksheet.Cells[row, 3].Style.Numberformat.Format = "0.00%";
                worksheet.Cells[row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells[row, 4].Value =
                    string.Join(", ", candidato.SkillsQueCumple?.Select(s => s.Nombre) ?? Enumerable.Empty<string>());

                worksheet.Cells[row, 5].Value =
                    string.Join(", ", candidato.SkillsFaltantes?.Select(s => s.Nombre) ?? Enumerable.Empty<string>());

                row++;
            }

            // ✅ AUTOFIT + BORDES
            if (worksheet.Dimension != null)
            {
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                using (var full = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column])
                {
                    full.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    full.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    full.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    full.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }
            }

            return await package.GetAsByteArrayAsync();
        }
    }


    // --- IMPLEMENTACIÓN DE BRECHAS (MEJORADA) ---
    public async Task<byte[]> GenerarReporteConsolidadoAsync(IEnumerable<MatchResultDTO> candidatosRanking, string tituloVacante)
    {
        ExcelPackage.License.SetNonCommercialPersonal("TalentoInterno");

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Ranking Brechas");

            // Colores
            var colorAzul = System.Drawing.Color.FromArgb(31, 78, 121);
            var colorVerde = System.Drawing.Color.FromArgb(0, 176, 80);
            var colorRojo = System.Drawing.Color.Red;
            var colorGris = System.Drawing.Color.LightGray;

            // 1. TÍTULO
            worksheet.Cells["A1"].Value = "REPORTE DE BRECHAS (FALTANTES)"; // <--- Cambio de Título
            worksheet.Cells["A1:H1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.Font.Color.SetColor(colorAzul);
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worksheet.Cells["A2"].Value = $"Vacante: {tituloVacante} | Generado: {DateTime.Now:dd/MM/yyyy}";
            worksheet.Cells["A2:H2"].Merge = true;
            worksheet.Cells["A2"].Style.Font.Italic = true;
            worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // 2. ENCABEZADOS
            int headerRow = 4;
            string[] headers = {
            "ID", "Candidato", "Área", "Departamento",
            "Rol Actual", "% Brecha (Faltante)", "Estado", "Detalle de Brechas"
        }; // <--- Cambié el encabezado de la columna 6

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cells[headerRow, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(colorGris);
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            }

            // 3. LLENADO DE DATOS
            int row = 5;
            foreach (var item in candidatosRanking)
            {
                worksheet.Cells[row, 1].Value = item.ColaboradorId;
                worksheet.Cells[row, 2].Value = item.Nombre;
                worksheet.Cells[row, 3].Value = item.AreaNombre ?? "General";
                worksheet.Cells[row, 4].Value = item.DepartamentoNombre ?? "General";
                worksheet.Cells[row, 5].Value = item.RolNombre ?? "Colaborador";

                // --- AQUÍ ESTÁ EL CAMBIO MATEMÁTICO ---
                double brecha = 100 - item.PorcentajeMatch; // Calculamos lo que falta
                if (brecha < 0) brecha = 0; // Por seguridad

                var cellBrecha = worksheet.Cells[row, 6];
                cellBrecha.Value = brecha / 100d; // Excel usa decimales (0.20 = 20%)
                cellBrecha.Style.Numberformat.Format = "0%";

                // --- LÓGICA DE COLORES INVERTIDA ---
                // Brecha pequeña (<20%) es BUENO -> Verde
                // Brecha grande (>50%) es MALO -> Rojo
                if (brecha <= 20)
                {
                    cellBrecha.Style.Font.Color.SetColor(colorVerde);
                }
                else if (brecha > 50)
                {
                    cellBrecha.Style.Font.Color.SetColor(colorRojo);
                }
                else
                {
                    cellBrecha.Style.Font.Color.SetColor(System.Drawing.Color.Orange);
                }

                // Estado (Texto para filtrar rápido)
                // Si la brecha es menor al 30%, es APTO.
                worksheet.Cells[row, 7].Value = brecha <= 30 ? "APTO" : "NO APTO";

                // Detalle de Brechas (Texto)
                var listaBrechas = item.SkillsFaltantes.Select(s => $"{s.Nombre} ({s.NivelColaboradorNombre ?? "Nulo"})");
                string textoBrechas = string.Join(", ", listaBrechas);
                worksheet.Cells[row, 8].Value = string.IsNullOrEmpty(textoBrechas) ? "Ninguna" : textoBrechas;

                row++;
            }

            worksheet.Cells[headerRow, 1, row - 1, 8].AutoFilter = true;
            worksheet.Cells.AutoFitColumns();
            worksheet.View.FreezePanes(5, 1);

            return await package.GetAsByteArrayAsync();
        }
    }

    // --- IMPLEMENTACIÓN DE KPIs (Corregido con Rango) ---
    public async Task<byte[]> GenerarKpisExcel(KpiReportDto kpiData, DateTime? desde, DateTime? hasta)
    {
        try
        {


            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("KPIs");

                string rango = $"{(desde?.ToString("yyyy-MM-dd") ?? "Inicio")} - {(hasta?.ToString("yyyy-MM-dd") ?? "Actual")}";

                worksheet.Cells["A1:C1"].Merge = true;
                worksheet.Cells["A1"].Value = "Reporte de KPIs Generales";
                worksheet.Cells["A1"].Style.Font.Size = 16;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Row(1).Height = 26;

                if (desde.HasValue || hasta.HasValue)
                {
                    worksheet.Cells["A2:C2"].Merge = true;
                    worksheet.Cells["A2"].Value = $"Rango: {rango}";
                    worksheet.Cells["A2"].Style.Font.Italic = true;
                }

                int tileRow = 4;
                worksheet.Cells[tileRow, 1].Value = "Total Colaboradores";
                worksheet.Cells[tileRow, 2].Value = kpiData?.TotalColaboradoresActivos ?? 0;

                worksheet.Cells[tileRow + 1, 1].Value = "Vacantes Abiertas";
                worksheet.Cells[tileRow + 1, 2].Value = kpiData?.TotalVacantesAbiertas ?? 0;

                if (worksheet.Dimension != null)
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                else
                    worksheet.Cells["A1:B6"].AutoFitColumns();

                return await package.GetAsByteArrayAsync();
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error generando Excel de KPIs: " + ex.Message, ex);
        }
    }


    public Task<byte[]> GenerarPdfConsolidadoAsync(IEnumerable<MatchResultDTO> candidatosRanking, string tituloVacante)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        // Instanciamos el documento pasando TODA la lista
        var document = new BookCandidatosDocument(candidatosRanking, tituloVacante);

        return Task.FromResult(document.GeneratePdf());
    }



    public async Task EnviarCorreoPersonalizadoAsync(EmailComposeDto correoDto)
    {
        // 1. Preparamos los datos del servidor (Usando _config correctamente)
        var emailEmisor = _config["EmailSettings:Email"]!;
        var passwordEmisor = _config["EmailSettings:Password"]!;
        var host = _config["EmailSettings:Host"]!;
        var port = int.Parse(_config["EmailSettings:Port"]!);

        // 2. DEFINIMOS LA PLANTILLA HTML (El diseño bonito)
        string plantillaHtml = @"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body { font-family: Arial, sans-serif; background-color: #f4f4f4; }
            .container { max-width: 600px; margin: 20px auto; background-color: #ffffff; padding: 20px; }
            .header { background-color: #0056b3; color: white; padding: 10px; text-align: center; }
            .content { padding: 20px; color: #333; }
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h2>Talento Interno</h2>
            </div>
            <div class='content'>
                [CUERPO_DEL_MENSAJE]
            </div>
            <div style='text-align:center; font-size:12px; color:#777;'>
                <p>Mensaje automático del sistema
                    TATA CONSULTANCY SERVICES</p>
            </div>
        </div>
    </body>
    </html>";

        // PROCESAMIENTO SEGURO
        string textoUsuario = correoDto.Cuerpo ?? "";
        string textoHtml = textoUsuario.Replace("\n", "<br>"); // Saltos de línea

        // AQUÍ ESTÁ EL ARREGLO: Usamos Replace en lugar de string.Format
        // Esto no se confunde con las llaves { } del CSS
        string cuerpoFinal = plantillaHtml.Replace("[CUERPO_DEL_MENSAJE]", textoHtml);

        using (var client = new SmtpClient(host, port))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(emailEmisor, passwordEmisor);

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(emailEmisor, "Talento Interno");
                mailMessage.To.Add(correoDto.Destinatario);
                mailMessage.Subject = correoDto.Asunto;
                mailMessage.Body = cuerpoFinal;
                mailMessage.IsBodyHtml = true;

                // Adjuntos (Igual que antes)
                if (correoDto.Adjuntos != null && correoDto.Adjuntos.Count > 0)
                {
                    foreach (var archivo in correoDto.Adjuntos)
                    {
                        if (archivo.Length > 0)
                        {
                            mailMessage.Attachments.Add(new Attachment(archivo.OpenReadStream(), archivo.FileName));
                        }
                    }
                }

                await client.SendMailAsync(mailMessage);
            }
        }
    }

}

