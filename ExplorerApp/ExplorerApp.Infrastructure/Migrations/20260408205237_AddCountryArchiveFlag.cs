using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExplorerApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryArchiveFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Countries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Countries");
        }
    }
}
