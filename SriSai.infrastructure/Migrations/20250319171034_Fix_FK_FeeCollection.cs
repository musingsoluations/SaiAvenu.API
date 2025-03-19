using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SriSai.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_FK_FeeCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeCollection_Apartment_Id",
                table: "FeeCollection");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "FeeCollection",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollection_ApartmentId",
                table: "FeeCollection",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeeCollection_Apartment_ApartmentId",
                table: "FeeCollection",
                column: "ApartmentId",
                principalTable: "Apartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeCollection_Apartment_ApartmentId",
                table: "FeeCollection");

            migrationBuilder.DropIndex(
                name: "IX_FeeCollection_ApartmentId",
                table: "FeeCollection");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "FeeCollection",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeCollection_Apartment_Id",
                table: "FeeCollection",
                column: "Id",
                principalTable: "Apartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
