using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalBlog.Data.Migrations
{
    public partial class MoreProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BlogPosts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlogPosts");
        }
    }
}
