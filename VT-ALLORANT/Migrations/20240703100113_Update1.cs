using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VT_ALLORANT.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Player_ModeratorId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Team_Team1Id",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Team_Team2Id",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Team_WinnerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Tournament_TournamentId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_DiscordUser_DiscordUserId",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_ValorantUser_ValorantUserId",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Player_LeaderId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayer_Player_PlayerId",
                table: "TeamPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayer_Team_TeamId",
                table: "TeamPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentModerator_Player_ModeratorId",
                table: "TournamentModerator");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentModerator_Tournament_TournamentId",
                table: "TournamentModerator");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentObserver_Player_ObserverId",
                table: "TournamentObserver");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentObserver_Tournament_TournamentId",
                table: "TournamentObserver");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_Team_TeamId",
                table: "TournamentTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_Tournament_TournamentId",
                table: "TournamentTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TournamentModerator");

            migrationBuilder.RenameTable(
                name: "Tournament",
                newName: "Tournaments");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "Player",
                newName: "Players");

            migrationBuilder.RenameIndex(
                name: "IX_Team_LeaderId",
                table: "Teams",
                newName: "IX_Teams_LeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_ValorantUserId",
                table: "Players",
                newName: "IX_Players_ValorantUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_DiscordUserId",
                table: "Players",
                newName: "IX_Players_DiscordUserId");

            migrationBuilder.AddColumn<string>(
                name: "MatchId",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "MaxPlayers",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments",
                column: "TournamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_ModeratorId",
                table: "Games",
                column: "ModeratorId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_Team1Id",
                table: "Games",
                column: "Team1Id",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_Team2Id",
                table: "Games",
                column: "Team2Id",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_WinnerId",
                table: "Games",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Tournaments_TournamentId",
                table: "Games",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_DiscordUser_DiscordUserId",
                table: "Players",
                column: "DiscordUserId",
                principalTable: "DiscordUser",
                principalColumn: "DiscordUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_ValorantUser_ValorantUserId",
                table: "Players",
                column: "ValorantUserId",
                principalTable: "ValorantUser",
                principalColumn: "ValorantUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayer_Players_PlayerId",
                table: "TeamPlayer",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayer_Teams_TeamId",
                table: "TeamPlayer",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Players_LeaderId",
                table: "Teams",
                column: "LeaderId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentModerator_Players_ModeratorId",
                table: "TournamentModerator",
                column: "ModeratorId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentModerator_Tournaments_TournamentId",
                table: "TournamentModerator",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentObserver_Players_ObserverId",
                table: "TournamentObserver",
                column: "ObserverId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentObserver_Tournaments_TournamentId",
                table: "TournamentObserver",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeam_Teams_TeamId",
                table: "TournamentTeam",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
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
                name: "FK_Games_Players_ModeratorId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_Team1Id",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_Team2Id",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_WinnerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Tournaments_TournamentId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_DiscordUser_DiscordUserId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_ValorantUser_ValorantUserId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayer_Players_PlayerId",
                table: "TeamPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayer_Teams_TeamId",
                table: "TeamPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Players_LeaderId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentModerator_Players_ModeratorId",
                table: "TournamentModerator");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentModerator_Tournaments_TournamentId",
                table: "TournamentModerator");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentObserver_Players_ObserverId",
                table: "TournamentObserver");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentObserver_Tournaments_TournamentId",
                table: "TournamentObserver");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_Teams_TeamId",
                table: "TournamentTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_Tournaments_TournamentId",
                table: "TournamentTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MaxPlayers",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "Tournaments",
                newName: "Tournament");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Player");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_LeaderId",
                table: "Team",
                newName: "IX_Team_LeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_ValorantUserId",
                table: "Player",
                newName: "IX_Player_ValorantUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_DiscordUserId",
                table: "Player",
                newName: "IX_Player_DiscordUserId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TournamentModerator",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament",
                column: "TournamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Team",
                column: "TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                table: "Player",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Player_ModeratorId",
                table: "Games",
                column: "ModeratorId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Team_Team1Id",
                table: "Games",
                column: "Team1Id",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Team_Team2Id",
                table: "Games",
                column: "Team2Id",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Team_WinnerId",
                table: "Games",
                column: "WinnerId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Tournament_TournamentId",
                table: "Games",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_DiscordUser_DiscordUserId",
                table: "Player",
                column: "DiscordUserId",
                principalTable: "DiscordUser",
                principalColumn: "DiscordUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_ValorantUser_ValorantUserId",
                table: "Player",
                column: "ValorantUserId",
                principalTable: "ValorantUser",
                principalColumn: "ValorantUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Player_LeaderId",
                table: "Team",
                column: "LeaderId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayer_Player_PlayerId",
                table: "TeamPlayer",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayer_Team_TeamId",
                table: "TeamPlayer",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentModerator_Player_ModeratorId",
                table: "TournamentModerator",
                column: "ModeratorId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentModerator_Tournament_TournamentId",
                table: "TournamentModerator",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentObserver_Player_ObserverId",
                table: "TournamentObserver",
                column: "ObserverId",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentObserver_Tournament_TournamentId",
                table: "TournamentObserver",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeam_Team_TeamId",
                table: "TournamentTeam",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
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
