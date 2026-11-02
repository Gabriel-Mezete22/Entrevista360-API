using Aplicacao.Interfaces;
using Dominio.Entidades;
using Dominio.Enums;
using Repositorio.Interfaces;

namespace Aplicacao;

public class EntrevistaAplicacao : IEntrevistaAplicacao
{
    public const int TamanhoMaximoTranscricao = 200_000;

    private readonly IEntrevistaRepositorio _entrevistaRepositorio;
    private readonly IVagaRepositorio _vagaRepositorio;
    private readonly ICandidatoRepositorio _candidatoRepositorio;

    public EntrevistaAplicacao(
        IEntrevistaRepositorio entrevistaRepositorio,
        IVagaRepositorio vagaRepositorio,
        ICandidatoRepositorio candidatoRepositorio)
    {
        _entrevistaRepositorio = entrevistaRepositorio;
        _vagaRepositorio = vagaRepositorio;
        _candidatoRepositorio = candidatoRepositorio;
    }

    public async Task<IEnumerable<Entrevista>> ObterTodasAsync(int? idVaga)
    {
        return await _entrevistaRepositorio.ObterTodasAsync(idVaga);
    }

    public async Task<Entrevista?> ObterPorIdAsync(int id)
    {
        return await _entrevistaRepositorio.ObterPorIdAsync(id);
    }

    public async Task<Entrevista> EnviarAsync(int idVaga, int idCandidato, string transcricao)
    {
        var vaga = await _vagaRepositorio.ObterPorIdAsync(idVaga);
        if (vaga is null)
        {
            throw new InvalidOperationException("Vaga não encontrada.");
        }

        var candidato = await _candidatoRepositorio.ObterPorIdAsync(idCandidato);
        if (candidato is null)
        {
            throw new InvalidOperationException("Candidato não encontrado.");
        }

        if (vaga.Status != StatusVaga.Aberta)
        {
            throw new InvalidOperationException("Esta vaga está encerrada e não recebe novas entrevistas.");
        }

        if (vaga.Criterios.Count == 0)
        {
            throw new InvalidOperationException("Cadastre ao menos um critério na vaga antes de enviar entrevistas.");
        }

        transcricao = transcricao.Trim();

        if (transcricao.Length == 0)
        {
            throw new InvalidOperationException("A transcrição enviada está vazia.");
        }

        if (transcricao.Length > TamanhoMaximoTranscricao)
        {
            throw new InvalidOperationException($"A transcrição excede o limite de {TamanhoMaximoTranscricao} caracteres.");
        }

        var entrevista = new Entrevista
        {
            IdVaga = idVaga,
            IdCandidato = idCandidato,
            Transcricao = transcricao,
            Status = StatusEntrevista.Enviada,
            DataEnvio = DateTime.UtcNow,
        };

        entrevista = await _entrevistaRepositorio.CriarAsync(entrevista);

        return (await _entrevistaRepositorio.ObterPorIdAsync(entrevista.Id))!;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var entrevista = await _entrevistaRepositorio.ObterPorIdAsync(id);
        if (entrevista is null)
        {
            return false;
        }

        return await _entrevistaRepositorio.ExcluirAsync(id);
    }
}
