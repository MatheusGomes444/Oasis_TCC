using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oasis_API.Migrations
{
    /// <inheritdoc />
    public partial class Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alojamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Equipe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CapacidadeMaxima = table.Column<int>(type: "int", nullable: false),
                    Pet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sexo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pertences = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Refeicoes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alojamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Moradores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CPF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    RG = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    Telefone = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Idade = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    Datanascimento = table.Column<int>(type: "int", nullable: false),
                    Nacionalidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    AlojamentoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moradores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moradores_Alojamentos_AlojamentoId",
                        column: x => x.AlojamentoId,
                        principalTable: "Alojamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilasDeEspera",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoradorId = table.Column<int>(type: "int", nullable: false),
                    AlojamentoId = table.Column<int>(type: "int", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilasDeEspera", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilasDeEspera_Alojamentos_AlojamentoId",
                        column: x => x.AlojamentoId,
                        principalTable: "Alojamentos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FilasDeEspera_Moradores_MoradorId",
                        column: x => x.MoradorId,
                        principalTable: "Moradores",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Alojamentos",
                columns: new[] { "Id", "CapacidadeMaxima", "Email", "Equipe", "Nome", "Pertences", "Pet", "Refeicoes", "Sexo", "Telefone" },
                values: new object[] { 1, 10, "exemplo1@dominio.com", "Equipe A", "Albergue Vila Maria", "Roupas e sapatos", "Apenas cães", "Café e janta", "Masculino", 123456789 });

            migrationBuilder.InsertData(
                table: "Moradores",
                columns: new[] { "Id", "AlojamentoId", "Ativo", "CPF", "Datanascimento", "Endereco", "Idade", "Nacionalidade", "Nome", "Observacoes", "RG", "Telefone" },
                values: new object[] { 1, 1, true, "12345678900", 0, "Rua Alcântara, 113", 18, "Brasileiro", "Pedro", "Sem observações", "603456789", 912345678 });

            migrationBuilder.CreateIndex(
                name: "IX_FilasDeEspera_AlojamentoId",
                table: "FilasDeEspera",
                column: "AlojamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_FilasDeEspera_MoradorId",
                table: "FilasDeEspera",
                column: "MoradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Moradores_AlojamentoId",
                table: "Moradores",
                column: "AlojamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilasDeEspera");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Moradores");

            migrationBuilder.DropTable(
                name: "Alojamentos");
        }
    }
}
