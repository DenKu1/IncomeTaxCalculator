using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IncomeTaxCalculator.Persistence.Migrations;

/// <inheritdoc />
public partial class Initial_Add_TaxBand_01 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "TaxBands",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AnnualSalaryUpperLimit = table.Column<int>(type: "int", nullable: true),
                AnnualSalaryLowerLimit = table.Column<int>(type: "int", nullable: false),
                TaxRate = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TaxBands", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "TaxBands",
            columns: new[] { "Id", "AnnualSalaryLowerLimit", "AnnualSalaryUpperLimit", "TaxRate" },
            values: new object[,]
            {
                { new Guid("bb49c25c-f1e2-4659-8420-c24eab2805cd"), 0, 5000, 0 },
                { new Guid("c65fad47-6d23-425b-a0f8-a62ae584c111"), 20000, null, 40 },
                { new Guid("f0ef7fde-4baf-497b-bae6-faf72e4427a2"), 5000, 20000, 20 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "TaxBands");
    }
}
