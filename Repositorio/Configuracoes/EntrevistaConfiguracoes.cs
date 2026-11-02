using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Entidades;

namespace Repositorio.Configuracoes;

public class EntrevistaConfiguracoes : IEntityTypeConfiguration<Entrevista>
{
    public void Configure(EntityTypeBuilder<Entrevista> builder)
    {
        builder.ToTable("Entrevistas");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Transcricao)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(e => e.IdVaga);
        builder.HasIndex(e => e.IdCandidato);

        builder.HasOne(e => e.Candidato)
            .WithMany()
            .HasForeignKey(e => e.IdCandidato)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
