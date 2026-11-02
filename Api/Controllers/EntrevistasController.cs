using Api.Controllers.Dtos;
using Api.Servicos;
using Aplicacao.Interfaces;
using Aplicacao.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/entrevistas")]
public class EntrevistasController : ControllerBase
{
    private readonly IEntrevistaAplicacao _entrevistaAplicacao;
    private readonly IAvaliacaoAplicacao _avaliacaoAplicacao;

    public EntrevistasController(IEntrevistaAplicacao entrevistaAplicacao, IAvaliacaoAplicacao avaliacaoAplicacao)
    {
        _entrevistaAplicacao = entrevistaAplicacao;
        _avaliacaoAplicacao = avaliacaoAplicacao;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntrevistaResponse>>> ObterTodas([FromQuery] int? idVaga)
    {
        var entrevistas = await _entrevistaAplicacao.ObterTodasAsync(idVaga);
        return Ok(entrevistas.Select(e => e.ParaResponse()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EntrevistaResponse>> ObterPorId(int id)
    {
        var entrevista = await _entrevistaAplicacao.ObterPorIdAsync(id);
        if (entrevista is null)
        {
            return NotFound();
        }

        return Ok(entrevista.ParaResponse());
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<EntrevistaResponse>> Enviar([FromForm] EnviarEntrevistaRequest request)
    {
        var transcricao = request.Transcricao;

        if (request.Arquivo is not null)
        {
            transcricao = await LeitorTranscricao.LerAsync(request.Arquivo);
            if (transcricao is null)
            {
                ModelState.AddModelError(nameof(request.Arquivo), "Não foi possível ler o arquivo .docx. Verifique se ele não está corrompido.");
                return ValidationProblem(ModelState);
            }
        }

        var entrevista = await _entrevistaAplicacao.EnviarAsync(request.IdVaga, request.IdCandidato, transcricao!);

        return CreatedAtAction(nameof(ObterPorId), new { id = entrevista.Id }, entrevista.ParaResponse());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var excluida = await _entrevistaAplicacao.ExcluirAsync(id);
        if (!excluida)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("{id:int}/avaliacao")]
    public async Task<ActionResult<AvaliacaoResponse>> ObterAvaliacao(int id)
    {
        var relatorio = await _avaliacaoAplicacao.ObterRelatorioAsync(id);
        if (relatorio is null)
        {
            return NotFound();
        }

        return Ok(relatorio.ParaResponse());
    }

    [HttpPut("{id:int}/avaliacao")]
    public async Task<ActionResult<AvaliacaoResponse>> SalvarAvaliacao(int id, SalvarAvaliacaoRequest request)
    {
        var notas = request.Notas
            .Select(n => new NotaCriterioEntrada(n.IdCriterio, n.Nota, n.Justificativa))
            .ToList();

        var relatorio = await _avaliacaoAplicacao.SalvarAsync(id, request.Parecer, notas);
        if (relatorio is null)
        {
            return NotFound();
        }

        return Ok(relatorio.ParaResponse());
    }
}
