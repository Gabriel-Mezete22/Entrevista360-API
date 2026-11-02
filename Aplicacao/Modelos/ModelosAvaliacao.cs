using Dominio.Entidades;

namespace Aplicacao.Modelos;

public record NotaCriterioEntrada(int IdCriterio, decimal? Nota, string? Justificativa);

public record LinhaRelatorio(
    int IdCriterio,
    string NomeCriterio,
    string? DescricaoCriterio,
    int Peso,
    decimal? Nota,
    string? Justificativa);

public record RelatorioAvaliacao(
    Entrevista Entrevista,
    string? Parecer,
    decimal? NotaGeral,
    DateTime? AtualizadaEm,
    IReadOnlyList<LinhaRelatorio> Linhas);

public record LinhaComparativo(
    Entrevista Entrevista,
    decimal? NotaGeral,
    IReadOnlyList<decimal?> Notas);

public record ComparativoVaga(
    Vaga Vaga,
    IReadOnlyList<Criterio> Criterios,
    IReadOnlyList<LinhaComparativo> Linhas);
