using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class RemoverIdUsuarioDeVagaECandidato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatos_Usuarios_IdUsuario",
                table: "Candidatos");

            migrationBuilder.DropForeignKey(
                name: "FK_Vagas_Usuarios_IdUsuario",
                table: "Vagas");

            migrationBuilder.DropIndex(
                name: "IX_Vagas_IdUsuario",
                table: "Vagas");

            migrationBuilder.DropIndex(
                name: "IX_Candidatos_IdUsuario",
                table: "Candidatos");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Vagas");

            migrationBuilder.DropColumn(
                name: "CaminhoArquivo",
                table: "Entrevistas");

            migrationBuilder.DropColumn(
                name: "NomeArquivoOriginal",
                table: "Entrevistas");

            migrationBuilder.DropColumn(
                name: "TamanhoBytes",
                table: "Entrevistas");

            migrationBuilder.DropColumn(
                name: "TipoConteudo",
                table: "Entrevistas");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Candidatos");

            migrationBuilder.AddColumn<string>(
                name: "Transcricao",
                table: "Entrevistas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Transcricao",
                table: "Entrevistas");

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Vagas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CaminhoArquivo",
                table: "Entrevistas",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeArquivoOriginal",
                table: "Entrevistas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "TamanhoBytes",
                table: "Entrevistas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "TipoConteudo",
                table: "Entrevistas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Candidatos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_IdUsuario",
                table: "Vagas",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_IdUsuario",
                table: "Candidatos",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatos_Usuarios_IdUsuario",
                table: "Candidatos",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vagas_Usuarios_IdUsuario",
                table: "Vagas",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
