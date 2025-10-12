using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CPF_EMAIL_TIPO_ALTERADO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cpf",
                table: "Pessoas",
                newName: "CPF");

            migrationBuilder.RenameColumn(
                name: "Email_email",
                table: "Pessoas",
                newName: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Pessoas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CPF",
                table: "Pessoas",
                newName: "cpf");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Pessoas",
                newName: "Email_email");

            migrationBuilder.AlterColumn<decimal>(
                name: "cpf",
                table: "Pessoas",
                type: "decimal(20,0)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
