using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballLeague.Migrations
{
    public partial class AddPlayerIdToIncidents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Players_PlayerId",
                table: "Incidents");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Incidents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Players_PlayerId",
                table: "Incidents",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Players_PlayerId",
                table: "Incidents");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Incidents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Players_PlayerId",
                table: "Incidents",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
