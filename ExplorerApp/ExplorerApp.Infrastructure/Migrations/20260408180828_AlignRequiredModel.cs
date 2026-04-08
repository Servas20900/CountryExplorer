using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExplorerApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlignRequiredModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Countries_CountryId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotes_Favorites_FavoriteId",
                table: "UserNotes");

            migrationBuilder.DropIndex(
                name: "IX_UserNotes_FavoriteId",
                table: "UserNotes");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_CountryId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "FavoriteId",
                table: "UserNotes");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "FlagUrl",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Population",
                table: "Countries");

            migrationBuilder.RenameColumn(
                name: "AddedDate",
                table: "Favorites",
                newName: "AddedAt");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "UserNotes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommonName",
                table: "Favorites",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Favorites",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FlagPngUrl",
                table: "Favorites",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                table: "Countries",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Capital",
                table: "Countries",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "CommonName",
                table: "Countries",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Countries",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FlagPngUrl",
                table: "Countries",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FlagSvgUrl",
                table: "Countries",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Countries",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Countries",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "OfficialName",
                table: "Countries",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subregion",
                table: "Countries",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CountryCode",
                table: "Favorites",
                column: "CountryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryCode",
                table: "Countries",
                column: "CountryCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Favorites_CountryCode",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Countries_CountryCode",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "UserNotes");

            migrationBuilder.DropColumn(
                name: "CommonName",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "FlagPngUrl",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "CommonName",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "FlagPngUrl",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "FlagSvgUrl",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "OfficialName",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Subregion",
                table: "Countries");

            migrationBuilder.RenameColumn(
                name: "AddedAt",
                table: "Favorites",
                newName: "AddedDate");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteId",
                table: "UserNotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Favorites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                table: "Countries",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "Capital",
                table: "Countries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "FlagUrl",
                table: "Countries",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Countries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Population",
                table: "Countries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_UserNotes_FavoriteId",
                table: "UserNotes",
                column: "FavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CountryId",
                table: "Favorites",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Countries_CountryId",
                table: "Favorites",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotes_Favorites_FavoriteId",
                table: "UserNotes",
                column: "FavoriteId",
                principalTable: "Favorites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
