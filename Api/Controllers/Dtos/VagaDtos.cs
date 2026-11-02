using System.ComponentModel.DataAnnotations;
using Dominio.Enums;

namespace Api.Controllers.Dtos;

public record CriterioRequest(
    [Required(ErrorMessage = "Informe o nome do critério.")]
    [StringLength(100, ErrorMessage = "O nome do critério pode ter no máximo 100 caracteres.")]
    string Nome,
    [StringLength(500, ErrorMessage = "A descrição do critério pode ter no máximo 500 caracteres.")]
    string? Descricao,
    [Range(1, 5, ErrorMessage = "O peso deve estar entre 1 e 5.")]
    int Peso);

public record CriarVagaRequest(
    [Required(ErrorMessage = "Informe o título da vaga.")]
    [StringLength(150, ErrorMessage = "O título pode ter no máximo 150 caracteres.")]
    string Titulo,
    [Required(ErrorMessage = "Informe a descrição da vaga.")]
    [StringLength(4000, ErrorMessage = "A descrição pode ter no máximo 4000 caracteres.")]
    string Descricao,
    List<CriterioRequest>? Criterios);

public record AtualizarVagaRequest(
    [Required(ErrorMessage = "Informe o título da vaga.")]
    [StringLength(150, ErrorMessage = "O título pode ter no máximo 150 caracteres.")]
    string Titulo,
    [Required(ErrorMessage = "Informe a descrição da vaga.")]
    [StringLength(4000, ErrorMessage = "A descrição pode ter no máximo 4000 caracteres.")]
    string Descricao,
    StatusVaga Status);

public record CriterioResponse(int Id, string Nome, string? Descricao, int Peso);

public record VagaResumoResponse(
    int Id,
    string Titulo,
    StatusVaga Status,
    int QuantidadeCriterios,
    int QuantidadeEntrevistas,
    DateTime CriadaEm);

public record VagaResponse(
    int Id,
    string Titulo,
    string Descricao,
    StatusVaga Status,
    DateTime CriadaEm,
    List<CriterioResponse> Criterios);
