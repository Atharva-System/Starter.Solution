using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Starter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Datatype_EstimatedHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedHours",
                table: "Projects");

            migrationBuilder.AddColumn<decimal>(
               name: "EstimatedHours",
               table: "Projects",
               type: "decimal(18,2)",
               nullable: false,
               defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "EstimatedHours",
                table: "Projects",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.DropColumn(
               name: "EstimatedHours",
               table: "Projects");
        }
    }
}
