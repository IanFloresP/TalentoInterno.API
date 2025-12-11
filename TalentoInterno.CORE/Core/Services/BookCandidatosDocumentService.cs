using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Infrastructure.Data.Pdf;

public class BookCandidatosDocument : IDocument
{
    private readonly IEnumerable<MatchResultDTO> _candidatos;
    private readonly string _tituloVacante;
    private const float MaxNivel = 3f; // Escala 1 a 3

    public BookCandidatosDocument(IEnumerable<MatchResultDTO> candidatos, string tituloVacante)
    {
        _candidatos = candidatos;
        _tituloVacante = tituloVacante;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            // Configuración Global
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

            // Encabezado (Se repite en cada página)
            page.Header().Element(ComposeHeader);

            // Contenido (Lista de Candidatos)
            page.Content().Element(ComposeContent);

            // Pie de Página
            page.Footer().AlignCenter().Text(x =>
            {
                x.Span("Página ");
                x.CurrentPageNumber();
                x.Span(" de ");
                x.TotalPages();
            });
        });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("Talento Interno").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                col.Item().Text($"Reporte de Candidatos: {_tituloVacante}").FontSize(12).FontColor(Colors.Grey.Darken2);
            });

            // Logo simulado a la derecha
            row.ConstantItem(50).Height(50).Background(Colors.Grey.Lighten4).AlignCenter().AlignMiddle().Text("TI").Bold();
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(20).Column(col =>
        {
            foreach (var candidato in _candidatos)
            {
                // --- FICHA DEL CANDIDATO ---
                col.Item().Element(c => ComposeCandidateCard(c, candidato));

                // Salto de página después de cada candidato para que quede limpio
                // (excepto si es el último)
                if (candidato != _candidatos.Last())
                    col.Item().PageBreak();
            }
        });
    }

    void ComposeCandidateCard(IContainer container, MatchResultDTO candidato)
    {
        container.Column(col =>
        {
            // 1. Cabecera del Candidato (Nombre y Score)
            col.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingBottom(10).Row(row =>
            {
                // Datos Personales
                row.RelativeItem().Column(info =>
                {
                    info.Item().Text(candidato.Nombre).FontSize(18).Bold().FontColor(Colors.Black);
                    info.Item().Text($"{candidato.RolNombre} | {candidato.AreaNombre}").FontSize(11).FontColor(Colors.Grey.Darken1);
                    info.Item().Text(candidato.Email).FontSize(10).Italic().FontColor(Colors.Blue.Medium);
                });

                // Círculo de Match (Score Grande)
                var colorScore = GetColorForMatch(candidato.PorcentajeMatch);

                row.ConstantItem(100).Column(scoreCol =>
                {
                    scoreCol.Item().AlignCenter().Text($"{candidato.PorcentajeMatch:F0}%").FontSize(28).Bold().FontColor(colorScore);
                    scoreCol.Item().AlignCenter().Text("Coincidencia").FontSize(9).FontColor(Colors.Grey.Medium);
                });
            });

            col.Item().PaddingVertical(15);

            // 2. Tabla de Habilidades con Gráficos
            var todasLasSkills = candidato.SkillsQueCumple.Concat(candidato.SkillsFaltantes).OrderBy(s => s.Nombre);
            col.Item().Element(c => ComposeTable(c, todasLasSkills));
        });
    }

    void ComposeTable(IContainer container, IEnumerable<SkillMatchDetalleDTO> skills)
    {
        container.Table(table =>
        {
            // Definición de Columnas
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(3);   // Habilidad
                columns.RelativeColumn(1);   // Req
                columns.RelativeColumn(1);   // Actual
                columns.RelativeColumn(4);   // GRÁFICO (Barra)
            });

            // Encabezados de Tabla
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Habilidad").SemiBold();
                header.Cell().Element(CellStyle).AlignCenter().Text("Req").SemiBold();
                header.Cell().Element(CellStyle).AlignCenter().Text("Act").SemiBold();
                header.Cell().Element(CellStyle).Text("Análisis de Brecha").SemiBold();
            });

            foreach (var skill in skills)
            {
                float colab = (float)(skill.NivelColaboradorId ?? 0);
                float req = (float)skill.NivelRequeridoId;

                table.Cell().Element(CellStyle).Text(skill.Nombre);
                table.Cell().Element(CellStyle).AlignCenter().Text(skill.NivelRequeridoNombre ?? "-").FontSize(9);
                table.Cell().Element(CellStyle).AlignCenter().Text(skill.NivelColaboradorNombre ?? "-").FontSize(9);

                // LA BARRA GRÁFICA
                table.Cell().Element(CellStyle).Element(c => ComposeSmartBarChart(c, colab, req));
            }
        });
    }

    void ComposeSmartBarChart(IContainer container, float colab, float req)
    {
        container.Column(col =>
        {
            col.Item().Height(14).Row(row =>
            {
                // Lógica Visual:
                // Verde: Lo que tiene.
                // Rojo Claro (con borde): Lo que le falta (La Brecha).
                // Gris: Espacio sobrante hasta el nivel máximo.

                if (colab >= req)
                {
                    // CUMPLE
                    row.RelativeItem(req).Background(Colors.Green.Medium);
                    if (colab > req) row.RelativeItem(colab - req).Background(Colors.Green.Darken2); // Exceso
                    if (MaxNivel > colab) row.RelativeItem(MaxNivel - colab).Background(Colors.Grey.Lighten4);
                }
                else
                {
                    // NO CUMPLE (BRECHA)
                    if (colab > 0) row.RelativeItem(colab).Background(Colors.Green.Lighten2); // Lo que tiene (Verde suave)

                    // La Brecha en Rojo Claro para destacar lo que falta
                    row.RelativeItem(req - colab)
                       .Background(Colors.Red.Lighten4)
                       .Border(1)
                       .BorderColor(Colors.Red.Medium);

                    if (MaxNivel > req) row.RelativeItem(MaxNivel - req).Background(Colors.Grey.Lighten4);
                }
            });

            // Texto pequeño de ayuda visual debajo de la barra
            // col.Item().Text(colab >= req ? "Cumple" : "Brecha detectada").FontSize(7).FontColor(Colors.Grey.Medium).AlignRight();
        });
    }

    static IContainer CellStyle(IContainer container)
    {
        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingVertical(6).PaddingRight(5);
    }

    private string GetColorForMatch(double match)
    {
        if (match >= 80) return Colors.Green.Medium;
        if (match >= 50) return Colors.Orange.Medium;
        return Colors.Red.Medium;
    }
}