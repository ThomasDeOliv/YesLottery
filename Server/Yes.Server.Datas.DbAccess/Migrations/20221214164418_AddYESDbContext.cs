using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YES.Server.Datas.DbAccess.Migrations
{
    public partial class AddYESDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "draw",
                columns: table => new
                {
                    private_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    drawed_numbers = table.Column<string>(type: "char(17)", fixedLength: true, maxLength: 17, nullable: true),
                    start_datetime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_draw", x => x.private_id);
                });

            migrationBuilder.CreateTable(
                name: "rank",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descriptor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "statistic",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    people_by_rank = table.Column<int>(type: "int", nullable: false),
                    fk_draw_id = table.Column<int>(type: "int", nullable: false),
                    fk_rank_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statistic", x => x.id);
                    table.ForeignKey(
                        name: "FK_statistic_draw_fk_draw_id",
                        column: x => x.fk_draw_id,
                        principalTable: "draw",
                        principalColumn: "private_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_statistic_rank_fk_rank_id",
                        column: x => x.fk_rank_id,
                        principalTable: "rank",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ticket",
                columns: table => new
                {
                    private_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    access_code = table.Column<string>(type: "char(22)", fixedLength: true, maxLength: 22, nullable: false),
                    played_numbers = table.Column<string>(type: "char(17)", fixedLength: true, maxLength: 17, nullable: false),
                    played_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fk_draw_id = table.Column<int>(type: "int", nullable: false),
                    fk_rank_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket", x => x.private_id);
                    table.ForeignKey(
                        name: "FK_ticket_draw_fk_draw_id",
                        column: x => x.fk_draw_id,
                        principalTable: "draw",
                        principalColumn: "private_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ticket_rank_fk_rank_id",
                        column: x => x.fk_rank_id,
                        principalTable: "rank",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "rank",
                columns: new[] { "id", "descriptor" },
                values: new object[,]
                {
                    { 1, "First Rank, all 6 numbers are valid" },
                    { 2, "Second Rank, 5 of the 6 numbers are valid" },
                    { 3, "Third Rank, 4 of the 6 numbers are valid" },
                    { 4, "Fourth Rank, less than 4 numbers are valid..." },
                    { 5, "Default Rank before until the end of a draw" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_statistic_fk_draw_id",
                table: "statistic",
                column: "fk_draw_id");

            migrationBuilder.CreateIndex(
                name: "IX_statistic_fk_rank_id",
                table: "statistic",
                column: "fk_rank_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_fk_draw_id",
                table: "ticket",
                column: "fk_draw_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_fk_rank_id",
                table: "ticket",
                column: "fk_rank_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "statistic");

            migrationBuilder.DropTable(
                name: "ticket");

            migrationBuilder.DropTable(
                name: "draw");

            migrationBuilder.DropTable(
                name: "rank");
        }
    }
}
