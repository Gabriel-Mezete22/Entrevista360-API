namespace Dominio.Entidades;

public class Avaliacao
{
    public int Id { get; set; }

    public int IdEntrevista { get; set; }
    public Entrevista? Entrevista { get; set; }

    public string? Parecer { get; set; }

    public DateTime CriadaEm { get; set; }
    public DateTime AtualizadaEm { get; set; }

    public List<AvaliacaoCriterio> Criterios { get; set; } = new();

    public decimal? NotaGeral
    {
        get
        {
            var avaliados = Criterios.Where(c => c.Criterio is not null).ToList();

            var somaDosPesos = avaliados.Sum(c => c.Criterio!.Peso);
            if (somaDosPesos == 0)
            {
                return null;
            }

            var somaPonderada = avaliados.Sum(c => c.Nota * c.Criterio!.Peso);
            return Math.Round(somaPonderada / somaDosPesos, 2, MidpointRounding.AwayFromZero);
        }
    }
}
