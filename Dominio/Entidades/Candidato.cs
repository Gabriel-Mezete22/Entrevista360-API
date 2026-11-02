namespace Dominio.Entidades;

public class Candidato
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateTime CriadoEm { get; set; }
}
