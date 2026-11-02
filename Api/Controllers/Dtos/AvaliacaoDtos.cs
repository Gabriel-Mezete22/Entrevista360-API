using System.ComponentModel.DataAnnotations;
using Dominio.Enums;

namespace Api.Controllers.Dtos;

public record NotaCriterioRequest(
    int IdCriterio,
    [Range(0.0, 10.0, ErrorMessage = "A nota deve estar entre 0 e 10.")]
    decimal? Nota,
    [StringLength(1000, ErrorMessage = "A justificativa pode ter no máximo 1000 caracteres.")]
    string? Justificativa);

public record SalvarAvaliacaoRequest(
    [StringLength(2000, ErrorMessage = "O parecer pode ter no máximo 2000 caracteres.")]
    string? Parecer,
    [Required(ErrorMessage = "Envie a lista de notas.")]
    List<NotaCriterioRequest> Notas);

public record LinhaAvaliacaoResponse(
    int IdCriterio,
    string NomeCriterio,
    string? DescricaoCriterio,
    int Peso,
    decimal? Nota,
    string? Justificativa);

public record AvaliacaoResponse(
    EntrevistaResponse Entrevista,
    string? Parecer,
    decimal? NotaGeral,
    int CriteriosAvaliados,
    int TotalCriterios,
    DateTime? AtualizadaEm,
    List<LinhaAvaliacaoResponse> Criterios);

public record ComparativoCandidatoResponse(
    int IdEntrevista,
    int IdCandidato,
    string NomeCandidato,
    StatusEntrevista Status,
    decimal? NotaGeral,
    List<decimal?> Notas);

public record ComparativoResponse(
    int IdVaga,
    string TituloVaga,
    List<CriterioResponse> Criterios,
    List<ComparativoCandidatoResponse> Candidatos);
