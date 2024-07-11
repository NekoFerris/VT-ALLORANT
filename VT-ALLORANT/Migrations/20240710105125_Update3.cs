using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VT_ALLORANT.Migrations
{
    /// <inheritdoc />
    public partial class Update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameTable(
                name: "Tournament",
                newName: "Tournaments");

            migrationBuilder.RenameColumn(
                name: "RankedScore",
                table: "Players",
                newName: "Rank");

            migrationBuilder.AddColumn<int>(
                name: "CurrentStage",
                table: "Tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments",
                column: "TournamentId");

            migrationBuilder.CreateTable(
                name: "GameObserver",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    ObserverId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameObserver", x => new { x.ObserverId, x.GameId });
                    table.ForeignKey(
                        name: "FK_GameObserver_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameObserver_Players_ObserverId",
                        column: x => x.ObserverId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameObserver_GameId",
                table: "GameObserver",
                column: "GameId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "GameObserver");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "CurrentStage",
                table: "Tournaments");

            migrationBuilder.RenameTable(
                name: "Tournaments",
                newName: "Tournament");

            migrationBuilder.RenameColumn(
                name: "Rank",
                table: "Players",
                newName: "RankedScore");

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
    }
}
