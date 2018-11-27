using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RegTempus.Migrations
{
    public partial class ModifySumOfTimeToTimeSpan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TimeRegistered",
                table: "TimeMeasurements",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeRegistered",
                table: "TimeMeasurements",
                nullable: false,
                oldClrType: typeof(TimeSpan));
        }
    }
}
