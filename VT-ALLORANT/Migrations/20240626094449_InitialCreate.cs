using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VT_ALLORANT.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordUser",
                columns: table => new
                {
                    DiscordUserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    DiscordId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordUser", x => x.DiscordUserId);
                });

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.TournamentId);
                });

            migrationBuilder.CreateTable(
                name: "ValorantUser",
                columns: table => new
                {
                    ValorantUserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PUUID = table.Column<string>(type: "TEXT", nullable: false),
                    NAME = table.Column<string>(type: "TEXT", nullable: false),
                    TAG = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValorantUser", x => x.ValorantUserId);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DiscordUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorantUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Player_DiscordUser_DiscordUserId",
                        column: x => x.DiscordUserId,
                        principalTable: "DiscordUser",
                        principalColumn: "DiscordUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Player_ValorantUser_ValorantUserId",
                        column: x => x.ValorantUserId,
                        principalTable: "ValorantUser",
                        principalColumn: "ValorantUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LeaderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_Player_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentModerator",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModeratorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentModerator", x => new { x.ModeratorId, x.TournamentId });
                    table.ForeignKey(
                        name: "FK_TournamentModerator_Player_ModeratorId",
                        column: x => x.ModeratorId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentModerator_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentObserver",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ObserverId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentObserver", x => new { x.ObserverId, x.TournamentId });
                    table.ForeignKey(
                        name: "FK_TournamentObserver_Player_ObserverId",
                        column: x => x.ObserverId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentObserver_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Team1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Team2Id = table.Column<int>(type: "INTEGER", nullable: false),
                    WinnerId = table.Column<int>(type: "INTEGER", nullable: true),
                    ModeratorId = table.Column<int>(type: "INTEGER", nullable: false),
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_Player_ModeratorId",
                        column: x => x.ModeratorId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Team_Team1Id",
                        column: x => x.Team1Id,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Team_Team2Id",
                        column: x => x.Team2Id,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Team_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamPlayer",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPlayer", x => new { x.PlayerId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_TeamPlayer_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamPlayer_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTeam",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTeam", x => new { x.TeamId, x.TournamentId });
                    table.ForeignKey(
                        name: "FK_TournamentTeam_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentTeam_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_ModeratorId",
                table: "Games",
                column: "ModeratorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_Team1Id",
                table: "Games",
                column: "Team1Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_Team2Id",
                table: "Games",
                column: "Team2Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_TournamentId",
                table: "Games",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_WinnerId",
                table: "Games",
                column: "WinnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_DiscordUserId",
                table: "Player",
                column: "DiscordUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_ValorantUserId",
                table: "Player",
                column: "ValorantUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_LeaderId",
                table: "Team",
                column: "LeaderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayer_TeamId",
                table: "TeamPlayer",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentModerator_TournamentId",
                table: "TournamentModerator",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentObserver_TournamentId",
                table: "TournamentObserver",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeam_TournamentId",
                table: "TournamentTeam",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "TeamPlayer");

            migrationBuilder.DropTable(
                name: "TournamentModerator");

            migrationBuilder.DropTable(
                name: "TournamentObserver");

            migrationBuilder.DropTable(
                name: "TournamentTeam");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Tournament");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "DiscordUser");

            migrationBuilder.DropTable(
                name: "ValorantUser");
        }
    }
}
