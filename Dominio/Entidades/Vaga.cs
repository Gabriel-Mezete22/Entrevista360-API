using Dominio.Enums;

namespace Dominio.Entidades;

public class Vaga
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public StatusVaga Status { get; set; } = StatusVaga.Aberta;

    public DateTime CriadaEm { get; set; }

    public List<Criterio> Criterios { get; set; } = new();

    public List<Entrevista> Entrevistas { get; set; } = new();
}
