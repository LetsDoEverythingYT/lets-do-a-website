using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lets_do_a_website.Migrations
{
    public partial class newstuff2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OverlayWrap",
                table: "UserSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverlayWrap",
                table: "UserSettings");
        }
    }
}
