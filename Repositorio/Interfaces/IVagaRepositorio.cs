using Dominio.Entidades;

namespace Repositorio.Interfaces;

public interface IVagaRepositorio
{
    Task<IEnumerable<Vaga>> ObterTodasAsync();

    Task<Vaga?> ObterPorIdAsync(int id);

    Task<Vaga> CriarAsync(Vaga vaga);
    Task<bool> AtualizarAsync(Vaga vaga);
    Task<bool> ExcluirAsync(int id);
    Task<bool> PossuiEntrevistasAsync(int id);
}
