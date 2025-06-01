using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RenzGrandWeddingblazor.ph.Migrations
{
    /// <inheritdoc />
    public partial class SeedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductLines",
                columns: new[] { "ProductLineId", "ProductLineCode", "ProductLineName" },
                values: new object[,]
                {
                    { 1, "ET", "EnglishTek" },
                    { 2, "ICT", "ICT" },
                    { 3, "LMS", "LMS" },
                    { 4, "MT", "MathTek" },
                    { 5, "ROBOTEK", "RoboTek" }
                });

           
        }
    }
}
