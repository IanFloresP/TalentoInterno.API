// --- LIBRERÍAS DE EXCEL ---
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
using System.Reflection;
using System.Threading.Tasks;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class ExportacionService : IExportacionService
{
    // --- La licencia debe ser establecida estáticamente una vez ---
    static ExportacionService()
    {
        // Esto soluciona los errores de inicialización y asegura el uso no comercial
        ExcelPackage.License.SetNonCommercialPersonal("TalentoInterno User");
    }

    public async Task<byte[]> GenerarRankingExcel(IEnumerable<MatchResultDTO> rankingData)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Ranking de Candidatos");
            var colorAzul = System.Drawing.Color.FromArgb(31, 78, 121);

            worksheet.Cells["A1"].Value = "ID Colaborador";
            worksheet.Cells["B1"].Value = "Porcentaje Match";
            worksheet.Cells["C1"].Value = "Skills Cumplidas";
            worksheet.Cells["D1"].Value = "Skills Faltantes";

            using (var range = worksheet.Cells["A1:D1"])
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
                worksheet.Cells[row, 1].Value = candidato.ColaboradorId;
                worksheet.Cells[row, 2].Value = candidato.PorcentajeMatch / 100.0;
                worksheet.Cells[row, 2].Style.Numberformat.Format = "0.00%";
                worksheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells[row, 3].Value = string.Join(", ", candidato.SkillsQueCumple?.Select(s => s.Nombre) ?? Enumerable.Empty<string>());
                worksheet.Cells[row, 4].Value = string.Join(", ", candidato.SkillsFaltantes?.Select(s => s.Nombre) ?? Enumerable.Empty<string>());

                row++;
            }

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
    public async Task<byte[]> GenerarBrechasExcel(IEnumerable<BrechaSkillDto> brechasData, string tituloVacante)
    {
        ExcelPackage.License.SetNonCommercialPersonal("TalentoInterno");

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Brechas");
            var colorAzul = System.Drawing.Color.FromArgb(31, 78, 121);
            var colorRojo = System.Drawing.Color.Red;
            var colorVerde = System.Drawing.Color.FromArgb(0, 176, 80); // Verde excel

            // 1. Título
            worksheet.Cells["A1"].Value = "ANÁLISIS DE BRECHAS DE COMPETENCIAS";
            worksheet.Cells["A1:E1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.Font.Color.SetColor(colorAzul);
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worksheet.Cells["A2"].Value = $"Vacante: {tituloVacante}";
            worksheet.Cells["A2:E2"].Merge = true;
            worksheet.Cells["A2"].Style.Font.Italic = true;
            worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // 2. Encabezados
            int headerRow = 4;
            worksheet.Cells[headerRow, 1].Value = "Candidato";
            worksheet.Cells[headerRow, 2].Value = "Habilidad Requerida";
            worksheet.Cells[headerRow, 3].Value = "Nivel Requerido";
            worksheet.Cells[headerRow, 4].Value = "Nivel Actual";
            worksheet.Cells[headerRow, 5].Value = "Brecha (%)";

            using (var range = worksheet.Cells[headerRow, 1, headerRow, 5])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            }

            // 3. Llenar Datos
            int row = 5;
            foreach (var item in brechasData)
            {
                worksheet.Cells[row, 1].Value = item.NombreColaborador;
                worksheet.Cells[row, 2].Value = item.SkillNombre;
                worksheet.Cells[row, 3].Value = item.NivelRequerido;
                worksheet.Cells[row, 4].Value = item.NivelActual;

                var cellBrecha = worksheet.Cells[row, 5];

                if (item.BrechaPorcentaje <= 0)
                {
                    // Caso: Cumple
                    cellBrecha.Value = "Cumple";
                    cellBrecha.Style.Font.Color.SetColor(colorVerde);
                    cellBrecha.Style.Font.Bold = true;
                }
                else
                {
                    // Caso: Brecha
                    cellBrecha.Value = item.BrechaPorcentaje;
                    cellBrecha.Style.Numberformat.Format = "0%";
                    cellBrecha.Style.Font.Color.SetColor(colorRojo);
                    cellBrecha.Style.Font.Bold = true;
                }

                row++;
            }

            worksheet.Cells.AutoFitColumns();
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


    public Task<byte[]> GenerarMatchPdf(MatchResultDTO matchData, ColaboradorDTO? colaboradorData)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = new MatchPdfDocument(matchData, colaboradorData);
        byte[] fileBytes = document.GeneratePdf();
        return Task.FromResult(fileBytes);
    }
}


// --- CLASE AUXILIAR PARA EL PDF (DISEÑO MEJORADO) ---
internal class MatchPdfDocument : IDocument
    {
        private readonly MatchResultDTO _data;
        private readonly ColaboradorDTO? _colaborador;
        private const float MaxNivel = 3f; // ESCALA AJUSTADA A 3

        public MatchPdfDocument(MatchResultDTO data, ColaboradorDTO? colaborador)
        {
            _data = data;
            _colaborador = colaborador;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.DefaultTextStyle(x => x.FontSize(10));
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
            });
        }

        void ComposeHeader(IContainer container)
        {
            string nombre = _colaborador != null ? $"{_colaborador.Nombres} {_colaborador.Apellidos}" : $"Colaborador ID: {_data.ColaboradorId}";

            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Reporte de Compatibilidad").Bold().FontSize(20).FontColor(Colors.Blue.Medium);
                    col.Item().Text($"Vacante ID: {_data.VacanteId}").FontSize(12);
                    col.Item().Text(nombre).Bold().FontSize(14);
                });

                row.RelativeItem().Column(col =>
                {
                    col.Item().AlignCenter().Text($"{_data.PorcentajeMatch:F0}%").Bold().FontSize(32).FontColor(_data.PorcentajeMatch >= 80 ? Colors.Green.Medium : _data.PorcentajeMatch >= 50 ? Colors.Orange.Medium : Colors.Red.Medium);
                    col.Item().AlignCenter().Text("Match Total");
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(20).Column(col =>
            {
                col.Spacing(20);

                // Tabla Unificada (Más limpio)
                col.Item().Text("Detalle de Habilidades").Bold().FontSize(16).FontColor(Colors.Blue.Darken2);

                // Unimos las listas para mostrar todo junto
                var todasLasSkills = _data.SkillsQueCumple.Concat(_data.SkillsFaltantes).OrderBy(s => s.Nombre);

                ComposeTable(col.Item(), todasLasSkills);
            });
        }

        void ComposeTable(IContainer container, IEnumerable<SkillMatchDetalleDTO> skills)
        {
            container.Table(table =>
            {
                // Definición de Columnas: Agregamos columna para % Brecha
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);   // Habilidad
                    columns.RelativeColumn(1);   // N. Req
                    columns.RelativeColumn(1);   // N. Act
                    columns.RelativeColumn(1);   // Brecha % (Texto)
                    columns.RelativeColumn(3);   // Gráfico Visual
                });

                // Encabezados
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Habilidad").Bold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Req").Bold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Actual").Bold();
                    header.Cell().Element(CellStyle).AlignCenter().Text("Brecha").Bold();
                    header.Cell().Element(CellStyle).Text("Análisis Visual").Bold();
                });

                foreach (var skill in skills)
                {
                    float colab = (float)(skill.NivelColaboradorId ?? 0);
                    float req = (float)skill.NivelRequeridoId;

                    // Cálculo de Brecha para mostrar
                    float brechaRaw = (req - colab) / MaxNivel;
                    string textoBrecha = brechaRaw <= 0 ? "OK" : $"-{brechaRaw:P0}";
                    var colorTexto = brechaRaw <= 0 ? Colors.Green.Medium : Colors.Red.Medium;

                    table.Cell().Element(CellStyle).Text(skill.Nombre);
                    table.Cell().Element(CellStyle).AlignCenter().Text(skill.NivelRequeridoNombre);
                    table.Cell().Element(CellStyle).AlignCenter().Text(skill.NivelColaboradorNombre ?? "-");

                    // Columna de Porcentaje Texto
                    table.Cell().Element(CellStyle).AlignCenter().Text(textoBrecha).FontColor(colorTexto).Bold();

                    // Columna Gráfica
                    table.Cell().Element(CellStyle).Element(c => ComposeSmartBarChart(c, colab, req));
                }
            });
        }

        IContainer CellStyle(IContainer container)
        {
            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingVertical(8).PaddingHorizontal(4);
        }

        // --- NUEVO GRÁFICO INTELIGENTE ---
        void ComposeSmartBarChart(IContainer container, float colab, float req)
        {
            container.Column(col =>
            {
                col.Item().Height(12).Row(row =>
                {
                    // CASO 1: CUMPLE O SUPERA (Verde)
                    if (colab >= req)
                    {
                        // Barra hasta lo requerido (Verde Normal)
                        row.RelativeItem(req).Background(Colors.Green.Medium);

                        // Barra de "Exceso" (Verde Oscuro) si sabe más
                        if (colab > req)
                        {
                            row.RelativeItem(colab - req).Background(Colors.Green.Darken2);
                        }

                        // Espacio vacío restante
                        if (MaxNivel > colab)
                        {
                            row.RelativeItem(MaxNivel - colab).Background(Colors.Grey.Lighten3);
                        }
                    }
                    // CASO 2: FALTA (Rojo)
                    else
                    {
                        // Barra de lo que TIENE (Rojo)
                        if (colab > 0)
                        {
                            row.RelativeItem(colab).Background(Colors.Red.Medium);
                        }

                        // Barra de la BRECHA (Rosado/Rojo Claro) - Lo que le falta para llegar
                        row.RelativeItem(req - colab).Background(Colors.Red.Lighten4).Border(1).BorderColor(Colors.Red.Medium);

                        // Espacio vacío restante hasta el máximo
                        if (MaxNivel > req)
                        {
                            row.RelativeItem(MaxNivel - req).Background(Colors.Grey.Lighten3);
                        }
                    }
                });
            });
        }
    }
