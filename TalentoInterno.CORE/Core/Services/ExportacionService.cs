using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing; // Para System.Drawing.Color

// --- LIBRERÍAS DE EXCEL ---
using OfficeOpenXml;
using OfficeOpenXml.Style;

// --- LIBRERÍAS DE PDF ---
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TalentoInterno.CORE.Core.Services;

public class ExportacionService : IExportacionService
{
    public async Task<byte[]> GenerarRankingExcel(IEnumerable<MatchResultDTO> rankingData)
    {
        // --- CORRECCIÓN DE LICENCIA ---
        ExcelPackage.License.SetNonCommercialPersonal("TalentoInterno.API");

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Ranking de Candidatos");

            // --- 1. Poner Cabeceras (Headers) ---
            worksheet.Cells["A1"].Value = "ID Colaborador";
            worksheet.Cells["B1"].Value = "Porcentaje Match";
            worksheet.Cells["C1"].Value = "Skills Cumplidas";
            worksheet.Cells["D1"].Value = "Skills Faltantes";

            using (var range = worksheet.Cells["A1:D1"])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            // --- 2. Llenar los Datos ---
            int row = 2;
            foreach (var candidato in rankingData)
            {
                worksheet.Cells[row, 1].Value = candidato.ColaboradorId;
                worksheet.Cells[row, 2].Value = candidato.PorcentajeMatch / 100;
                worksheet.Cells[row, 2].Style.Numberformat.Format = "0.00%";

                worksheet.Cells[row, 3].Value = string.Join(", ", candidato.SkillsQueCumple.Select(s => s.Nombre));
                worksheet.Cells[row, 4].Value = string.Join(", ", candidato.SkillsFaltantes.Select(s => s.Nombre));

                row++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return await package.GetAsByteArrayAsync();
        }
    }

    public async Task<byte[]> GenerarBrechasExcel(IEnumerable<object> brechasData)
    {
        // --- CORRECCIÓN DE LICENCIA ---
        ExcelPackage.License.SetNonCommercialPersonal("TalentoInterno.API");

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Brechas");
            worksheet.Cells["A1"].Value = "WIP - Brechas de Habilidad";
            // (Aquí iría la lógica para llenar las brechas)

            return await package.GetAsByteArrayAsync();
        }
    }

    // --- IMPLEMENTACIÓN DE PDF (MODIFICADA) ---
    public Task<byte[]> GenerarMatchPdf(MatchResultDTO matchData, ColaboradorDTO? colaboradorData)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        // Pasamos ambos DTOs al constructor del documento
        var document = new MatchPdfDocument(matchData, colaboradorData);

        byte[] fileBytes = document.GeneratePdf();
        return Task.FromResult(fileBytes);
    }
}

// --- CLASE AUXILIAR PARA EL PDF (MODIFICADA) ---
internal class MatchPdfDocument : IDocument
{
    private readonly MatchResultDTO _data;
    private readonly ColaboradorDTO? _colaborador; // ¡NUEVO!
    private const int MaxNivel = 5; // Asumimos 5 como el nivel máximo posible

    public MatchPdfDocument(MatchResultDTO data, ColaboradorDTO? colaborador)
    {
        _data = data;
        _colaborador = colaborador; // ¡NUEVO!
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
            });
    }

    void ComposeHeader(IContainer container)
    {
        // Usamos el nombre del colaborador si existe
        string nombreColaborador = _colaborador != null
            ? $"{_colaborador.Nombres} {_colaborador.Apellidos}"
            : $"ID: {_data.ColaboradorId}";

        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text($"Reporte de Match")
                    .Bold().FontSize(20).FontColor(Colors.Blue.Medium);
                col.Item().Text($"Vacante ID: {_data.VacanteId}");
                col.Item().Text(nombreColaborador).Bold(); // ¡MODIFICADO!
            });

            row.RelativeItem().Column(col =>
            {
                col.Item().AlignCenter().Text($"{_data.PorcentajeMatch:F2}%")
                    .Bold().FontSize(28).FontColor(Colors.Blue.Medium);
                col.Item().AlignCenter().Text("Porcentaje de Match");
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(col =>
        {
            col.Spacing(20);

            // Tabla 1: Skills Cumplidas
            col.Item().Text("Habilidades Cumplidas").Bold().FontSize(16).FontColor(Colors.Green.Medium);
            ComposeTable(col.Item(), _data.SkillsQueCumple);

            // Tabla 2: Skills Faltantes
            col.Item().Text("Habilidades Faltantes").Bold().FontSize(16).FontColor(Colors.Red.Medium);
            ComposeTable(col.Item(), _data.SkillsFaltantes);
        });
    }

    // --- MÉTODO REFACTORIZADO PARA CREAR LAS TABLAS ---
    void ComposeTable(IContainer container, IEnumerable<SkillMatchDetalleDTO> skills)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(1.5f); // Skill
                columns.ConstantColumn(80);   // N. Req
                columns.ConstantColumn(80);   // N. Colab
                columns.RelativeColumn(1f);   // Gráfico de Brecha
            });

            table.Header(header =>
            {
                header.Cell().Background(Colors.Grey.Lighten3).Text("Habilidad");
                header.Cell().Background(Colors.Grey.Lighten3).Text("N. Requerido");
                header.Cell().Background(Colors.Grey.Lighten3).Text("N. Colaborador");
                header.Cell().Background(Colors.Grey.Lighten3).Text("Brecha (Gap)");
            });

            foreach (var skill in skills)
            {
                table.Cell().Text(skill.Nombre);
                table.Cell().Text(skill.NivelRequeridoNombre);
                table.Cell().Text(skill.NivelColaboradorNombre ?? "N/A");

                // Celda que dibuja el gráfico de barras
                table.Cell().Element(c =>
                    ComposeBarChart(c, skill.NivelColaboradorId, skill.NivelRequeridoId)
                );
            }
        });
    }

    // --- MÉTODO PARA DIBUJAR EL GRÁFICO DE BARRAS ---
    void ComposeBarChart(IContainer container, byte? nivelColaborador, byte nivelRequerido)
    {
        byte colabNivel = nivelColaborador ?? 0;
        byte reqNivel = nivelRequerido;

        container
            .Background(Colors.Grey.Lighten3) // Fondo de la barra
            .Border(1)
            .Padding(2) // Un pequeño padding
            .Row(row =>
            {
                // --- Barra 1: Lo que tiene el colaborador (Azul) ---
                if (colabNivel > 0)
                {
                    // Si el colaborador supera el requisito, solo pintamos hasta el requisito
                    var nivelPintado = Math.Min(colabNivel, reqNivel);
                    row.RelativeItem((uint)nivelPintado).Height(15)
                       .Background(Colors.Blue.Medium);
                }

                // --- Barra 2: La brecha (Rojo) ---
                if (reqNivel > colabNivel)
                {
                    int brecha = reqNivel - colabNivel;
                    row.RelativeItem((uint)brecha).Height(15)
                       .Background(Colors.Red.Lighten2);
                }

                // --- Barra 3: El "exceso" (Verde) ---
                if (colabNivel > reqNivel)
                {
                    int extra = colabNivel - reqNivel;
                    // Clamp al máximo
                    if (extra + reqNivel > MaxNivel)
                    {
                        extra = MaxNivel - reqNivel;
                    }
                    row.RelativeItem((uint)extra).Height(15)
                       .Background(Colors.Green.Lighten2);
                }

                // --- Barra 4: Lo que sobra (Gris) ---
                var nivelMasAlto = Math.Max(colabNivel, reqNivel);
                if (MaxNivel > nivelMasAlto)
                {
                    int sobrante = MaxNivel - (int)nivelMasAlto;
                    row.RelativeItem((uint)sobrante).Height(15)
                       .Background(Colors.Grey.Lighten2);
                }
            });
    }
}