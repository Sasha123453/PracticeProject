using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticeProject.Migrations
{
    /// <inheritdoc />
    public partial class again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Resources",
                newName: "ShortDescription");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Resources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ResourceId",
                table: "Comments",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Resources_ResourceId",
                table: "Comments",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Resources_ResourceId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ResourceId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "Resources",
                newName: "Description");
        }
    }
}
