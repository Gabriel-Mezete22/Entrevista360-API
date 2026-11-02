using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Dtos;

public record CriarCandidatoRequest(
    [Required(ErrorMessage = "Informe o nome do candidato.")]
    [StringLength(150, ErrorMessage = "O nome pode ter no máximo 150 caracteres.")]
    string Nome,
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(200, ErrorMessage = "O e-mail pode ter no máximo 200 caracteres.")]
    string? Email);

public record AtualizarCandidatoRequest(
    [Required(ErrorMessage = "Informe o nome do candidato.")]
    [StringLength(150, ErrorMessage = "O nome pode ter no máximo 150 caracteres.")]
    string Nome,
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(200, ErrorMessage = "O e-mail pode ter no máximo 200 caracteres.")]
    string? Email);

public record CandidatoResponse(int Id, string Nome, string? Email, DateTime CriadoEm);
