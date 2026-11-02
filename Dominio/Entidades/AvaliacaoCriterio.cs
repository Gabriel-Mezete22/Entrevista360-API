namespace Dominio.Entidades;

public class AvaliacaoCriterio
{
    public int Id { get; set; }

    public int IdAvaliacao { get; set; }
    public Avaliacao? Avaliacao { get; set; }

    public int IdCriterio { get; set; }
    public Criterio? Criterio { get; set; }

    public decimal Nota { get; set; }

    public string? Justificativa { get; set; }
}
