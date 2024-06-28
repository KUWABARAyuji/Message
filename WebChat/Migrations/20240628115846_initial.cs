using Microsoft.EntityFrameworkCore.Migrations;

namespace WebChat.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_ApplicationUserId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Message",
                newName: "SenderUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ApplicationUserId",
                table: "Message",
                newName: "IX_Message_SenderUserId");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverUserId",
                table: "Message",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "IdLong",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ReceiverUserId",
                table: "Message",
                column: "ReceiverUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_ReceiverUserId",
                table: "Message",
                column: "ReceiverUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_SenderUserId",
                table: "Message",
                column: "SenderUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_ReceiverUserId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_SenderUserId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ReceiverUserId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ReceiverUserId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IdLong",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SenderUserId",
                table: "Message",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderUserId",
                table: "Message",
                newName: "IX_Message_ApplicationUserId");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Message",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Message",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_ApplicationUserId",
                table: "Message",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
