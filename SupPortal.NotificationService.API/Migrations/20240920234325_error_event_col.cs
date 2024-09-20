using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupPortal.NotificationService.API.Migrations
{
    /// <inheritdoc />
    public partial class error_event_col : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventError",
                table: "MailInboxs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventError",
                table: "MailInboxs");
        }
    }
}
