using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoodHamburguerApp.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InicializacaoBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidoItens",
                columns: table => new
                {
                    ItensId = table.Column<int>(type: "int", nullable: false),
                    PedidosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItens", x => new { x.ItensId, x.PedidosId });
                    table.ForeignKey(
                        name: "FK_PedidoItens_Itens_ItensId",
                        column: x => x.ItensId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItens_Pedidos_PedidosId",
                        column: x => x.PedidosId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Itens",
                columns: new[] { "Id", "Categoria", "CreatedAt", "Descricao", "Nome", "Preco", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Sanduiche", new DateTime(2026, 4, 24, 0, 4, 17, 981, DateTimeKind.Utc).AddTicks(2206), "Pão, carne bovina 150g e queijo cheddar.", "X Burger", 5.00m, null },
                    { 2, "Sanduiche", new DateTime(2026, 4, 24, 0, 4, 17, 981, DateTimeKind.Utc).AddTicks(2892), "Pão, carne bovina 150g, ovo frito e queijo cheddar.", "X Egg", 4.50m, null },
                    { 3, "Sanduiche", new DateTime(2026, 4, 24, 0, 4, 17, 981, DateTimeKind.Utc).AddTicks(2893), "Pão, carne bovina 150g, bacon e queijo cheddar.", "X Bacon", 7.00m, null },
                    { 4, "Batata", new DateTime(2026, 4, 24, 0, 4, 17, 981, DateTimeKind.Utc).AddTicks(2894), "Batata frita crocante.", "Batata frita", 2.00m, null },
                    { 5, "Refrigerante", new DateTime(2026, 4, 24, 0, 4, 17, 981, DateTimeKind.Utc).AddTicks(2895), "Refrigerante gelado.", "Refrigerante", 2.50m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_PedidosId",
                table: "PedidoItens",
                column: "PedidosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoItens");

            migrationBuilder.DropTable(
                name: "Itens");

            migrationBuilder.DropTable(
                name: "Pedidos");
        }
    }
}
