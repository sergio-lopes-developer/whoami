using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhoAmI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileDeletedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "Profiles",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "Profiles");
        }
    }
}
