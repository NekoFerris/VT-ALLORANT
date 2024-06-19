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
                name: "PlayersInTeams",
                columns: table => new
                {
                    PlayersPlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamsTeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersInTeams", x => new { x.PlayersPlayerId, x.TeamsTeamId });
                    table.ForeignKey(
                        name: "FK_PlayersInTeams_Player_PlayersPlayerId",
                        column: x => x.PlayersPlayerId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayersInTeams_Team_TeamsTeamId",
                        column: x => x.TeamsTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_DiscordUserId",
                table: "Player",
                column: "DiscordUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_ValorantUserId",
                table: "Player",
                column: "ValorantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersInTeams_TeamsTeamId",
                table: "PlayersInTeams",
                column: "TeamsTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_LeaderId",
                table: "Team",
                column: "LeaderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayersInTeams");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "DiscordUser");

            migrationBuilder.DropTable(
                name: "ValorantUser");
        }
    }
}
