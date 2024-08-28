using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class ModificaNomeColonna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_roomImages_Rooms_RoomID",
                table: "roomImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roomImages",
                table: "roomImages");

            migrationBuilder.RenameTable(
                name: "roomImages",
                newName: "RoomImages");

            migrationBuilder.RenameIndex(
                name: "IX_roomImages_RoomID",
                table: "RoomImages",
                newName: "IX_RoomImages_RoomID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomImages",
                table: "RoomImages",
                column: "ImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomImages_Rooms_RoomID",
                table: "RoomImages",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomImages_Rooms_RoomID",
                table: "RoomImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomImages",
                table: "RoomImages");

            migrationBuilder.RenameTable(
                name: "RoomImages",
                newName: "roomImages");

            migrationBuilder.RenameIndex(
                name: "IX_RoomImages_RoomID",
                table: "roomImages",
                newName: "IX_roomImages_RoomID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomImages",
                table: "roomImages",
                column: "ImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_roomImages_Rooms_RoomID",
                table: "roomImages",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
