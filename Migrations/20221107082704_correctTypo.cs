using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchSystem.Migrations
{
    /// <inheritdoc />
    public partial class correctTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CollectionedAmount",
                table: "Tithe",
                newName: "CollectedAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CollectedAmount",
                table: "Tithe",
                newName: "CollectionedAmount");
        }
    }
}
