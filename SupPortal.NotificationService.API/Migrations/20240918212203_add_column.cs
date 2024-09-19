using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupPortal.NotificationService.API.Migrations
{
    /// <inheritdoc />
    public partial class add_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "MailInboxs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "MailInboxs");
        }
    }
}
