using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VT_ALLORANT.Migrations
{
    /// <inheritdoc />
    public partial class Update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxTeamRank",
                table: "Tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "OpenForRegistration",
                table: "Tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxTeamRank",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "OpenForRegistration",
                table: "Tournaments");
        }
    }
}
