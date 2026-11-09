using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedAboutAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Abouts",
                newName: "Title2");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Abouts",
                newName: "ImageUrl2");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Abouts",
                newName: "Title1");

            migrationBuilder.AddColumn<string>(
                name: "BottomDescription",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BottomImageUrl",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BottomTitle",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description1",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description2",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FeatureDescription1",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FeatureDescription2",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FeatureDescription3",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FeatureTitle1",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FeatureTitle2",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FeatureTitle3",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl1",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BottomDescription",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "BottomImageUrl",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "BottomTitle",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "Description1",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "Description2",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "FeatureDescription1",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "FeatureDescription2",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "FeatureDescription3",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "FeatureTitle1",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "FeatureTitle2",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "FeatureTitle3",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "ImageUrl1",
                table: "Abouts");

            migrationBuilder.RenameColumn(
                name: "Title2",
                table: "Abouts",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Title1",
                table: "Abouts",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "ImageUrl2",
                table: "Abouts",
                newName: "ImageUrl");
        }
    }
}
