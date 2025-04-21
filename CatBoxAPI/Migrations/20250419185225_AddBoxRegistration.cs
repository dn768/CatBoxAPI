using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatBoxAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBoxRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "CatProfile",
                type: "Decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "Decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CatProfile",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateTable(
                name: "BoxRegistration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoxSize = table.Column<int>(type: "int", nullable: false),
                    SpecialFeatures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoxRegistration_CatProfile_CatId",
                        column: x => x.CatId,
                        principalTable: "CatProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoxRegistration_CatId",
                table: "BoxRegistration",
                column: "CatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoxRegistration");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CatProfile");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "CatProfile",
                type: "Decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "Decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
