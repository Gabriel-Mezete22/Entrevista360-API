using Api.Controllers.Dtos;
using Aplicacao.Interfaces;
using Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/vagas")]
public class VagasController : ControllerBase
{
    private readonly IVagaAplicacao _vagaAplicacao;
    private readonly IAvaliacaoAplicacao _avaliacaoAplicacao;

    public VagasController(IVagaAplicacao vagaAplicacao, IAvaliacaoAplicacao avaliacaoAplicacao)
    {
        _vagaAplicacao = vagaAplicacao;
        _avaliacaoAplicacao = avaliacaoAplicacao;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VagaResumoResponse>>> ObterTodas()
    {
        var vagas = await _vagaAplicacao.ObterTodasAsync();
        return Ok(vagas.Select(v => v.ParaResumo()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VagaResponse>> ObterPorId(int id)
    {
        var vaga = await _vagaAplicacao.ObterPorIdAsync(id);
        if (vaga is null)
            return NotFound();

        return Ok(vaga.ParaResponse());
    }

    [HttpPost]
    public async Task<ActionResult<VagaResponse>> Criar(CriarVagaRequest request)
    {
        var vaga = new Vaga
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            Criterios = (request.Criterios ?? new List<CriterioRequest>())
                .Select(c => new Criterio { Nome = c.Nome, Descricao = c.Descricao, Peso = c.Peso })
                .ToList(),
        };

        vaga = await _vagaAplicacao.CriarAsync(vaga);

        return CreatedAtAction(nameof(ObterPorId), new { id = vaga.Id }, vaga.ParaResponse());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, AtualizarVagaRequest request)
    {
        var vaga = new Vaga
        {
            Id = id,
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            Status = request.Status,
        };

        var atualizada = await _vagaAplicacao.AtualizarAsync(vaga);
        if (!atualizada)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var excluida = await _vagaAplicacao.ExcluirAsync(id);
        if (!excluida)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("{idVaga:int}/criterios")]
    public async Task<ActionResult<CriterioResponse>> AdicionarCriterio(int idVaga, CriterioRequest request)
    {
        var criterio = new Criterio { Nome = request.Nome, Descricao = request.Descricao, Peso = request.Peso };

        var criado = await _vagaAplicacao.AdicionarCriterioAsync(idVaga, criterio);
        if (criado is null)
        {
            return NotFound();
        }

        return StatusCode(StatusCodes.Status201Created, criado.ParaResponse());
    }

    [HttpPut("{idVaga:int}/criterios/{id:int}")]
    public async Task<IActionResult> AtualizarCriterio(int idVaga, int id, CriterioRequest request)
    {
        var criterio = new Criterio { Id = id, Nome = request.Nome, Descricao = request.Descricao, Peso = request.Peso };

        var atualizado = await _vagaAplicacao.AtualizarCriterioAsync(idVaga, criterio);
        if (!atualizado)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{idVaga:int}/criterios/{id:int}")]
    public async Task<IActionResult> ExcluirCriterio(int idVaga, int id)
    {
        var excluido = await _vagaAplicacao.ExcluirCriterioAsync(idVaga, id);
        if (!excluido)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("{id:int}/comparativo")]
    public async Task<ActionResult<ComparativoResponse>> ObterComparativo(int id)
    {
        var comparativo = await _avaliacaoAplicacao.ObterComparativoAsync(id);
        if (comparativo is null)
        {
            return NotFound();
        }

        return Ok(comparativo.ParaResponse());
    }
}
