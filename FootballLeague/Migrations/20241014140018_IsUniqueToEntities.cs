using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballLeague.Migrations
{
    public partial class IsUniqueToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NamePosition",
                table: "Functions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_Name",
                table: "Rounds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Name",
                table: "Positions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Functions_NamePosition",
                table: "Functions",
                column: "NamePosition",
                unique: true,
                filter: "[NamePosition] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_Name",
                table: "Clubs",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rounds_Name",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Positions_Name",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Functions_NamePosition",
                table: "Functions");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_Name",
                table: "Clubs");

            migrationBuilder.AlterColumn<string>(
                name: "NamePosition",
                table: "Functions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
