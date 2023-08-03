using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logistic.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FullAddress = table.Column<string>(type: "text", nullable: false),
                    inactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Addressid = table.Column<long>(type: "bigint", nullable: true),
                    inactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Customers_addresses_Addressid",
                        column: x => x.Addressid,
                        principalTable: "addresses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "delivery_points",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Addressid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    inactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_points", x => x.id);
                    table.ForeignKey(
                        name: "FK_delivery_points_addresses_Addressid",
                        column: x => x.Addressid,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Addressid",
                table: "Customers",
                column: "Addressid");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_points_Addressid",
                table: "delivery_points",
                column: "Addressid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "delivery_points");

            migrationBuilder.DropTable(
                name: "addresses");
        }
    }
}
