using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TODO.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePicUrl_To_Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profile_pic_url",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile_pic_url",
                table: "user");
        }
    }
}
