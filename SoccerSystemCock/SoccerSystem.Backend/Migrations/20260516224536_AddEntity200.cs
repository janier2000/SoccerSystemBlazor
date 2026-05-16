using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerSystem.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddEntity200 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DoublePoints",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoublePoints",
                table: "Matches");
        }
    }
}
