using Dominio.Entidades;

namespace Repositorio.Interfaces;

public interface ICandidatoRepositorio
{
    Task<IEnumerable<Candidato>> ObterTodosAsync();
    Task<Candidato?> ObterPorIdAsync(int id);
    Task<Candidato> CriarAsync(Candidato candidato);
    Task<bool> AtualizarAsync(Candidato candidato);
    Task<bool> ExcluirAsync(int id);
    Task<bool> PossuiEntrevistasAsync(int id);
}
