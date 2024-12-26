using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RobertDev_Chatbot.Migrations
{
    /// <inheritdoc />
    public partial class config : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    BotUsername = table.Column<string>(type: "TEXT", nullable: false),
                    BotAccessToken = table.Column<string>(type: "TEXT", nullable: false),
                    BotRefreshToken = table.Column<string>(type: "TEXT", nullable: false),
                    APIClientId = table.Column<string>(type: "TEXT", nullable: false),
                    APIClientSecret = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelUsername = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.BotUsername);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Config");
        }
    }
}
