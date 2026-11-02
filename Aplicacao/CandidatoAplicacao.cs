using Aplicacao.Interfaces;
using Dominio.Entidades;
using Repositorio.Interfaces;

namespace Aplicacao;

public class CandidatoAplicacao : ICandidatoAplicacao
{
    private readonly ICandidatoRepositorio _candidatoRepositorio;

    public CandidatoAplicacao(ICandidatoRepositorio candidatoRepositorio)
    {
        _candidatoRepositorio = candidatoRepositorio;
    }

    public async Task<IEnumerable<Candidato>> ObterTodosAsync()
    {
        return await _candidatoRepositorio.ObterTodosAsync();
    }

    public async Task<Candidato?> ObterPorIdAsync(int id)
    {
        return await _candidatoRepositorio.ObterPorIdAsync(id);
    }

    public async Task<Candidato> CriarAsync(Candidato candidato)
    {
        candidato.Id = 0;
        candidato.CriadoEm = DateTime.UtcNow;
        Normalizar(candidato);

        return await _candidatoRepositorio.CriarAsync(candidato);
    }

    public async Task<bool> AtualizarAsync(Candidato candidato)
    {
        Normalizar(candidato);

        return await _candidatoRepositorio.AtualizarAsync(candidato);
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var candidato = await _candidatoRepositorio.ObterPorIdAsync(id);
        if (candidato is null)
        {
            return false;
        }

        if (await _candidatoRepositorio.PossuiEntrevistasAsync(id))
        {
            throw new InvalidOperationException("Este candidato já tem entrevistas e não pode ser excluído.");
        }

        return await _candidatoRepositorio.ExcluirAsync(id);
    }

    private static void Normalizar(Candidato candidato)
    {
        candidato.Nome = candidato.Nome.Trim();
        candidato.Email = string.IsNullOrWhiteSpace(candidato.Email) ? null : candidato.Email.Trim().ToLowerInvariant();
    }
}
