using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VT_ALLORANT.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Tournaments_TournamentId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentModerator_Tournaments_TournamentId",
                table: "TournamentModerator");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentObserver_Tournaments_TournamentId",
                table: "TournamentObserver");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_Tournaments_TournamentId",
                table: "TournamentTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments");

            migrationBuilder.RenameTable(
                name: "Tournaments",
                newName: "Tournament");

            migrationBuilder.AddColumn<int>(
                name: "RankedScore",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Tournament_TournamentId",
                table: "Games",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentModerator_Tournament_TournamentId",
                table: "TournamentModerator",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentObserver_Tournament_TournamentId",
                table: "TournamentObserver",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeam_Tournament_TournamentId",
                table: "TournamentTeam",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Tournament_TournamentId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentModerator_Tournament_TournamentId",
                table: "TournamentModerator");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentObserver_Tournament_TournamentId",
                table: "TournamentObserver");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_Tournament_TournamentId",
                table: "TournamentTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament");

            migrationBuilder.DropColumn(
                name: "RankedScore",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "Tournament",
                newName: "Tournaments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Tournaments_TournamentId",
                table: "Games",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentModerator_Tournaments_TournamentId",
                table: "TournamentModerator",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentObserver_Tournaments_TournamentId",
                table: "TournamentObserver",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeam_Tournaments_TournamentId",
                table: "TournamentTeam",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
