using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;
using Repositorio.Configuracoes;

namespace Repositorio.Contexto;

public class EntrevistaDbContext : DbContext
{
    public EntrevistaDbContext(DbContextOptions<EntrevistaDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Vaga> Vagas => Set<Vaga>();
    public DbSet<Criterio> Criterios => Set<Criterio>();
    public DbSet<Candidato> Candidatos => Set<Candidato>();
    public DbSet<Entrevista> Entrevistas => Set<Entrevista>();
    public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
    public DbSet<AvaliacaoCriterio> AvaliacoesCriterio => Set<AvaliacaoCriterio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsuarioConfiguracoes());
        modelBuilder.ApplyConfiguration(new VagaConfiguracoes());
        modelBuilder.ApplyConfiguration(new CriterioConfiguracoes());
        modelBuilder.ApplyConfiguration(new CandidatoConfiguracoes());
        modelBuilder.ApplyConfiguration(new EntrevistaConfiguracoes());
        modelBuilder.ApplyConfiguration(new AvaliacaoConfiguracoes());
        modelBuilder.ApplyConfiguration(new AvaliacaoCriterioConfiguracoes());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<DataUtcConverter>();
    }
}
