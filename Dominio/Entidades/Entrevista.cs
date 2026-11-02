using Dominio.Enums;

namespace Dominio.Entidades;

public class Entrevista
{
    public int Id { get; set; }

    public int IdVaga { get; set; }
    public Vaga? Vaga { get; set; }

    public int IdCandidato { get; set; }
    public Candidato? Candidato { get; set; }

    public string Transcricao { get; set; } = string.Empty;

    public StatusEntrevista Status { get; set; } = StatusEntrevista.Enviada;

    public DateTime DataEnvio { get; set; }

    public Avaliacao? Avaliacao { get; set; }
}
