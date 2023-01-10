using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lets_do_a_website.Migrations
{
    public partial class newstuff3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteOnDeath",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "OverlayDeleteOnDeath",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "OverlayWrap",
                table: "UserSettings");

            migrationBuilder.AddColumn<int>(
                name: "OverlayOnDeath",
                table: "UserSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrackerOnDeath",
                table: "UserSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverlayOnDeath",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "TrackerOnDeath",
                table: "UserSettings");

            migrationBuilder.AddColumn<bool>(
                name: "DeleteOnDeath",
                table: "UserSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverlayDeleteOnDeath",
                table: "UserSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverlayWrap",
                table: "UserSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
