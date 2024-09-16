using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballLeague.Migrations
{
    public partial class ImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLogo",
                table: "Clubs");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Clubs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Clubs");

            migrationBuilder.AddColumn<string>(
                name: "ImageLogo",
                table: "Clubs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
