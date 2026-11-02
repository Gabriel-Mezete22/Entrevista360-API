using Dominio.Entidades;

namespace Repositorio.Interfaces;

public interface ICriterioRepositorio
{
    Task<Criterio> CriarAsync(Criterio criterio);
    Task<bool> AtualizarAsync(Criterio criterio);
    Task<bool> ExcluirAsync(int id);

    Task<bool> EstaEmUsoAsync(int id);
}
