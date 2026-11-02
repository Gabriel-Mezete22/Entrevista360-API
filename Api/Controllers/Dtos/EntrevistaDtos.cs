using System.ComponentModel.DataAnnotations;
using Api.Servicos;
using Dominio.Enums;

namespace Api.Controllers.Dtos;

public class EnviarEntrevistaRequest : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecione a vaga.")]
    public int IdVaga { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione o candidato.")]
    public int IdCandidato { get; set; }

    public string? Transcricao { get; set; }

    public IFormFile? Arquivo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var temTexto = !string.IsNullOrWhiteSpace(Transcricao);

        if (temTexto == (Arquivo is not null))
        {
            yield return new ValidationResult(
                "Informe a transcrição em texto ou anexe um arquivo .txt ou .docx (apenas um dos dois).",
                new[] { nameof(Transcricao), nameof(Arquivo) });
        }
        else if (Arquivo is not null && !LeitorTranscricao.ExtensaoPermitida(Arquivo.FileName))
        {
            yield return new ValidationResult("O arquivo da transcrição deve ser .txt ou .docx.", new[] { nameof(Arquivo) });
        }
    }
}

public record EntrevistaResponse(
    int Id,
    int IdVaga,
    string TituloVaga,
    int IdCandidato,
    string NomeCandidato,
    StatusEntrevista Status,
    string Transcricao,
    DateTime DataEnvio,
    decimal? NotaGeral);
