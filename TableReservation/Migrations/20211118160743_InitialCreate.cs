using Microsoft.EntityFrameworkCore.Migrations;

namespace TableReservation.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservedTables");

            migrationBuilder.DropTable(
                name: "Tables");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.TableId);
                });

            migrationBuilder.CreateTable(
                name: "ReservedTables",
                columns: table => new
                {
                    ResTableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationResId = table.Column<int>(type: "int", nullable: false),
                    TablesTableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedTables", x => x.ResTableId);
                    table.ForeignKey(
                        name: "FK_ReservedTables_Reservations_ReservationResId",
                        column: x => x.ReservationResId,
                        principalTable: "Reservations",
                        principalColumn: "ResId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservedTables_Tables_TablesTableId",
                        column: x => x.TablesTableId,
                        principalTable: "Tables",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservedTables_ReservationResId",
                table: "ReservedTables",
                column: "ReservationResId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedTables_TablesTableId",
                table: "ReservedTables",
                column: "TablesTableId");
        }
    }
}
