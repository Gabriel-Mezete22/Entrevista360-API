using Api.Controllers.Dtos;
using Aplicacao.Interfaces;
using Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/candidatos")]
public class CandidatosController : ControllerBase
{
    private readonly ICandidatoAplicacao _candidatoAplicacao;

    public CandidatosController(ICandidatoAplicacao candidatoAplicacao)
    {
        _candidatoAplicacao = candidatoAplicacao;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CandidatoResponse>>> ObterTodos()
    {
        var candidatos = await _candidatoAplicacao.ObterTodosAsync();
        return Ok(candidatos.Select(c => c.ParaResponse()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CandidatoResponse>> ObterPorId(int id)
    {
        var candidato = await _candidatoAplicacao.ObterPorIdAsync(id);
        if (candidato is null) return NotFound();

        return Ok(candidato.ParaResponse());
    }

    [HttpPost]
    public async Task<ActionResult<CandidatoResponse>> Criar(CriarCandidatoRequest request)
    {
        var candidato = new Candidato { Nome = request.Nome, Email = request.Email };

        candidato = await _candidatoAplicacao.CriarAsync(candidato);

        return CreatedAtAction(nameof(ObterPorId), new { id = candidato.Id }, candidato.ParaResponse());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, AtualizarCandidatoRequest request)
    {
        var candidato = new Candidato { Id = id, Nome = request.Nome, Email = request.Email };

        var atualizado = await _candidatoAplicacao.AtualizarAsync(candidato);
        if (!atualizado)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var excluido = await _candidatoAplicacao.ExcluirAsync(id);
        if (!excluido)
        {
            return NotFound();
        }

        return NoContent();
    }
}
