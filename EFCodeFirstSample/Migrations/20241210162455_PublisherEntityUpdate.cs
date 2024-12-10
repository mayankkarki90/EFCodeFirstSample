using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCodeFirstSample.Migrations
{
    /// <inheritdoc />
    public partial class PublisherEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VideoGames_PublisherId",
                table: "VideoGames",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoGames_Publishers_PublisherId",
                table: "VideoGames",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoGames_Publishers_PublisherId",
                table: "VideoGames");

            migrationBuilder.DropIndex(
                name: "IX_VideoGames_PublisherId",
                table: "VideoGames");
        }
    }
}
