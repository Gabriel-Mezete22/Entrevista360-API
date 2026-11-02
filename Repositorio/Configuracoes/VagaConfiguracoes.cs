using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Entidades;

namespace Repositorio.Configuracoes;

public class VagaConfiguracoes : IEntityTypeConfiguration<Vaga>
{
    public void Configure(EntityTypeBuilder<Vaga> builder)
    {
        builder.ToTable("Vagas");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Titulo)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(v => v.Descricao)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(v => v.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasMany(v => v.Criterios)
            .WithOne(c => c.Vaga)
            .HasForeignKey(c => c.IdVaga)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.Entrevistas)
            .WithOne(e => e.Vaga)
            .HasForeignKey(e => e.IdVaga)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
