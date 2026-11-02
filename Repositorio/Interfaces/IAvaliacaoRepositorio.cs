using Dominio.Entidades;
using Dominio.Enums;

namespace Repositorio.Interfaces;

public interface IAvaliacaoRepositorio
{
    Task SalvarAsync(Avaliacao avaliacao, StatusEntrevista statusDaEntrevista);
}
