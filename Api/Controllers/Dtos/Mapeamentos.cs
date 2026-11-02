using Aplicacao.Modelos;
using Dominio.Entidades;

namespace Api.Controllers.Dtos;

public static class Mapeamentos
{
    public static CriterioResponse ParaResponse(this Criterio criterio)
    {
        return new CriterioResponse(criterio.Id, criterio.Nome, criterio.Descricao, criterio.Peso);
    }

    public static VagaResumoResponse ParaResumo(this Vaga vaga)
    {
        return new VagaResumoResponse(
            vaga.Id,
            vaga.Titulo,
            vaga.Status,
            vaga.Criterios.Count,
            vaga.Entrevistas.Count,
            vaga.CriadaEm);
    }

    public static VagaResponse ParaResponse(this Vaga vaga)
    {
        return new VagaResponse(
            vaga.Id,
            vaga.Titulo,
            vaga.Descricao,
            vaga.Status,
            vaga.CriadaEm,
            vaga.Criterios.OrderBy(c => c.Id).Select(c => c.ParaResponse()).ToList());
    }

    public static CandidatoResponse ParaResponse(this Candidato candidato)
    {
        return new CandidatoResponse(candidato.Id, candidato.Nome, candidato.Email, candidato.CriadoEm);
    }

    public static EntrevistaResponse ParaResponse(this Entrevista entrevista)
    {
        return new EntrevistaResponse(
            entrevista.Id,
            entrevista.IdVaga,
            entrevista.Vaga?.Titulo ?? string.Empty,
            entrevista.IdCandidato,
            entrevista.Candidato?.Nome ?? string.Empty,
            entrevista.Status,
            entrevista.Transcricao,
            entrevista.DataEnvio,
            entrevista.Avaliacao?.NotaGeral);
    }

    public static AvaliacaoResponse ParaResponse(this RelatorioAvaliacao relatorio)
    {
        return new AvaliacaoResponse(
            relatorio.Entrevista.ParaResponse(),
            relatorio.Parecer,
            relatorio.NotaGeral,
            relatorio.Linhas.Count(l => l.Nota.HasValue),
            relatorio.Linhas.Count,
            relatorio.AtualizadaEm,
            relatorio.Linhas
                .Select(l => new LinhaAvaliacaoResponse(
                    l.IdCriterio, l.NomeCriterio, l.DescricaoCriterio, l.Peso, l.Nota, l.Justificativa))
                .ToList());
    }

    public static ComparativoResponse ParaResponse(this ComparativoVaga comparativo)
    {
        return new ComparativoResponse(
            comparativo.Vaga.Id,
            comparativo.Vaga.Titulo,
            comparativo.Criterios.Select(c => c.ParaResponse()).ToList(),
            comparativo.Linhas
                .Select(l => new ComparativoCandidatoResponse(
                    l.Entrevista.Id,
                    l.Entrevista.IdCandidato,
                    l.Entrevista.Candidato?.Nome ?? string.Empty,
                    l.Entrevista.Status,
                    l.NotaGeral,
                    l.Notas.ToList()))
                .ToList());
    }
}
