using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymMgmt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsRefundAllowedtotableClubsettingsandWillNotRenewtosubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WillNotRenew",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRefundAllowed",
                table: "ClubSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WillNotRenew",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsRefundAllowed",
                table: "ClubSettings");
        }
    }
}
