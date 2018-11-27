using Microsoft.EntityFrameworkCore.Migrations;

namespace RegTempus.Migrations
{
    public partial class AddedYearProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "TimeMeasurements",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "TimeMeasurements");
        }
    }
}
