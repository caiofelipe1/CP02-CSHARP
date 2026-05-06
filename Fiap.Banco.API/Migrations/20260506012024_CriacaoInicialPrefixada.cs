using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Banco.API.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicialPrefixada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PB_AGENCIAS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NUMERO = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    NOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ENDERECO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PB_AGENCIAS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PB_EMPRESASCONVENIADAS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CNPJ = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    RAZAOSOCIAL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ATIVA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PB_EMPRESASCONVENIADAS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PB_PRODUTOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TIPOPRODUTO = table.Column<string>(type: "NVARCHAR2(21)", maxLength: 21, nullable: false),
                    TAXAJUROSMENSAL = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    TAXAMDR = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    EXIGECONVENIOEMPRESA = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PB_PRODUTOS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PB_CLIENTES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    AGENCIAID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPOCLIENTE = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    DATANASCIMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CNPJ = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    RAZAOSOCIAL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PB_CLIENTES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PB_CLIENTES_PB_AGENCIAS",
                        column: x => x.AGENCIAID,
                        principalTable: "PB_AGENCIAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PB_CONTRATACOES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CLIENTEID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PRODUTOID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STATUS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATASOLICITACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CNPJEMPRESAEMPREGADORA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOTIVOREPROVACAO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PB_CONTRATACOES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PB_CONTRATACOES_PB_CLIENTES",
                        column: x => x.CLIENTEID,
                        principalTable: "PB_CLIENTES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PB_CONTRATACOES_Produtos",
                        column: x => x.PRODUTOID,
                        principalTable: "PB_PRODUTOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PB_AGENCIAS",
                columns: new[] { "ID", "ENDERECO", "NOME", "NUMERO" },
                values: new object[] { 1, "Avenida Paulista, São Paulo", "Agência Paulista", "0001" });

            migrationBuilder.InsertData(
                table: "PB_EMPRESASCONVENIADAS",
                columns: new[] { "ID", "ATIVA", "CNPJ", "RAZAOSOCIAL" },
                values: new object[] { 1, 1, "12345678000199", "Empresa Conveniada FIAP LTDA" });

            migrationBuilder.InsertData(
                table: "PB_PRODUTOS",
                columns: new[] { "ID", "EXIGECONVENIOEMPRESA", "NOME", "TIPOPRODUTO" },
                values: new object[] { 1, 1, "Receber Salário", "RECEBER_SALARIO" });

            migrationBuilder.CreateIndex(
                name: "IX_PB_AGENCIAS_NUMERO",
                table: "PB_AGENCIAS",
                column: "NUMERO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PB_CLIENTES_AGENCIAID",
                table: "PB_CLIENTES",
                column: "AGENCIAID");

            migrationBuilder.CreateIndex(
                name: "IX_PB_CLIENTES_CNPJ",
                table: "PB_CLIENTES",
                column: "CNPJ",
                unique: true,
                filter: "\"CNPJ\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PB_CLIENTES_CPF",
                table: "PB_CLIENTES",
                column: "CPF",
                unique: true,
                filter: "\"CPF\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PB_CONTRATACOES_CLIENTEID",
                table: "PB_CONTRATACOES",
                column: "CLIENTEID");

            migrationBuilder.CreateIndex(
                name: "IX_PB_CONTRATACOES_PRODUTOID",
                table: "PB_CONTRATACOES",
                column: "PRODUTOID");

            migrationBuilder.CreateIndex(
                name: "IX_PB_EMPRESASCONVENIADAS_CNPJ",
                table: "PB_EMPRESASCONVENIADAS",
                column: "CNPJ",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PB_CONTRATACOES");

            migrationBuilder.DropTable(
                name: "PB_EMPRESASCONVENIADAS");

            migrationBuilder.DropTable(
                name: "PB_CLIENTES");

            migrationBuilder.DropTable(
                name: "PB_PRODUTOS");

            migrationBuilder.DropTable(
                name: "PB_AGENCIAS");
        }
    }
}
