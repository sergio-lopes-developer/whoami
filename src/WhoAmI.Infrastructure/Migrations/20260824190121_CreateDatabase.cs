using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhoAmI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    first_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 254, nullable: false),
                    linkedin_url = table.Column<string>(type: "TEXT", nullable: false),
                    github_url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "UX_Profiles_Email",
                table: "Profiles",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Profiles_Guid",
                table: "Profiles",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
