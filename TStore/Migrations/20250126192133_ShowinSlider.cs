using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TStore.Migrations
{
    /// <inheritdoc />
    public partial class ShowinSlider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowinSlider",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowinSlider",
                table: "Categories");
        }
    }
}
