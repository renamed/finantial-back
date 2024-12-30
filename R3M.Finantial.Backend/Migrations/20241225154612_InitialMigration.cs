using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R3M.Finantial.Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    inserted_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_categories_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "institutions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    initial_balance = table.Column<decimal>(type: "numeric", nullable: false),
                    initial_balance_date = table.Column<DateOnly>(type: "date", nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: false),
                    inserted_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_institutions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "periods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    initial_date = table.Column<DateOnly>(type: "date", nullable: false),
                    final_date = table.Column<DateOnly>(type: "date", nullable: false),
                    description = table.Column<string>(type: "character(6)", fixedLength: true, maxLength: 6, nullable: false),
                    inserted_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_periods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "movimentations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    description = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    inserted_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movimentations", x => x.id);
                    table.ForeignKey(
                        name: "fk_movimentations_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_movimentations_institutions_institution_id",
                        column: x => x.institution_id,
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_movimentations_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_categories_parent_id_name",
                table: "categories",
                columns: new[] { "parent_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_institutions_name",
                table: "institutions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_movimentations_category_id",
                table: "movimentations",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_movimentations_institution_id",
                table: "movimentations",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_movimentations_period_id",
                table: "movimentations",
                column: "period_id");

            migrationBuilder.CreateIndex(
                name: "ix_periods_description",
                table: "periods",
                column: "description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_periods_initial_date_final_date",
                table: "periods",
                columns: new[] { "initial_date", "final_date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movimentations");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "institutions");

            migrationBuilder.DropTable(
                name: "periods");
        }
    }
}
