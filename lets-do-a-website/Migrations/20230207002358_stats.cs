using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lets_do_a_website.Migrations
{
    public partial class stats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Submitted",
                table: "RunStats",
                newName: "StartTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstUsed",
                table: "Trackers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DeathCount",
                table: "RunStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "RunStats",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstUsed",
                table: "Trackers");

            migrationBuilder.DropColumn(
                name: "DeathCount",
                table: "RunStats");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "RunStats");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "RunStats",
                newName: "Submitted");
        }
    }
}
