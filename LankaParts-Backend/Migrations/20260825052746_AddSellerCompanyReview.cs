using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LankaParts_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerCompanyReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewNote",
                table: "SellerCompanies",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedAt",
                table: "SellerCompanies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewedByUserId",
                table: "SellerCompanies",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellerCompanies_ReviewedByUserId",
                table: "SellerCompanies",
                column: "ReviewedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerCompanies_Users_ReviewedByUserId",
                table: "SellerCompanies",
                column: "ReviewedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerCompanies_Users_ReviewedByUserId",
                table: "SellerCompanies");

            migrationBuilder.DropIndex(
                name: "IX_SellerCompanies_ReviewedByUserId",
                table: "SellerCompanies");

            migrationBuilder.DropColumn(
                name: "ReviewNote",
                table: "SellerCompanies");

            migrationBuilder.DropColumn(
                name: "ReviewedAt",
                table: "SellerCompanies");

            migrationBuilder.DropColumn(
                name: "ReviewedByUserId",
                table: "SellerCompanies");
        }
    }
}
