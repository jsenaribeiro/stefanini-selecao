using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CONCLUSAO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCriacao",
                table: "Pessoas",
                newName: "Log_DataCriacao");

            migrationBuilder.RenameColumn(
                name: "DataAtualizacao",
                table: "Pessoas",
                newName: "Log_DataAtualizacao");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Pessoas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_CPF",
                table: "Pessoas",
                column: "CPF",
                unique: true,
                filter: "[CPF] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pessoas_CPF",
                table: "Pessoas");

            migrationBuilder.RenameColumn(
                name: "Log_DataCriacao",
                table: "Pessoas",
                newName: "DataCriacao");

            migrationBuilder.RenameColumn(
                name: "Log_DataAtualizacao",
                table: "Pessoas",
                newName: "DataAtualizacao");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Pessoas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
