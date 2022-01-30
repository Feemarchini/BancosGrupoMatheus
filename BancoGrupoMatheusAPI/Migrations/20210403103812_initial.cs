using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BancoGrupoMatheusAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimeiroNome = table.Column<string>(nullable: true),
                    Sobrenome = table.Column<string>(nullable: true),
                    NumeroDeTelefone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NumeroContaGenerated = table.Column<string>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(nullable: false),
                    TipoDeConta = table.Column<string>(nullable: false),
                    Saldo = table.Column<string>(nullable: false),
                    PinStoredHash = table.Column<string>(nullable: true),
                    PinStoredSalt = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenciaTransferencia = table.Column<string>(nullable: true),
                    ValorTransferencia = table.Column<decimal>(nullable: false),
                    StatusTransferencia = table.Column<int>(nullable: false),
                    SaldoAposTransferencia = table.Column<string>(nullable: true),
                    ContaDestinoTransferencia = table.Column<string>(nullable: true),
                    ObservacaoTransferencia = table.Column<string>(nullable: true),
                    TipoTransferencia = table.Column<int>(nullable: false),
                    DataTransferencia = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contas");

            migrationBuilder.DropTable(
                name: "Transacoes");
        }
    }
}
