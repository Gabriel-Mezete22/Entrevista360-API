namespace Dominio.Entidades;

public class Criterio
{
    public int Id { get; set; }

    public int IdVaga { get; set; }
    public Vaga? Vaga { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public int Peso { get; set; } = 3;
}
