using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatBoxAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakeApprovalNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "BoxRegistration",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "DecidedOn",
                table: "BoxRegistration",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DecisionReason",
                table: "BoxRegistration",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecidedOn",
                table: "BoxRegistration");

            migrationBuilder.DropColumn(
                name: "DecisionReason",
                table: "BoxRegistration");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "BoxRegistration",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
