using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RegTempus.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registrators",
                columns: table => new
                {
                    RegistratorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(maxLength: 36, nullable: true),
                    FirstName = table.Column<string>(maxLength: 30, nullable: true),
                    LastName = table.Column<string>(maxLength: 30, nullable: true),
                    UserHaveStartedTimeMeasure = table.Column<bool>(nullable: false),
                    StartedTimeMeasurement = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrators", x => x.RegistratorId);
                });

            migrationBuilder.CreateTable(
                name: "TimeMeasurements",
                columns: table => new
                {
                    TimeMeasurementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegistratorId = table.Column<int>(nullable: false),
                    TimeStart = table.Column<DateTime>(nullable: false),
                    TimeStop = table.Column<DateTime>(nullable: false),
                    TimeRegistered = table.Column<DateTime>(nullable: false),
                    TimeType = table.Column<string>(nullable: true),
                    DayOfMonth = table.Column<int>(nullable: false),
                    MonthOfYear = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeMeasurements", x => x.TimeMeasurementId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrators");

            migrationBuilder.DropTable(
                name: "TimeMeasurements");
        }
    }
}
