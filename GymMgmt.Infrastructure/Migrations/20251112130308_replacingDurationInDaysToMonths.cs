using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymMgmt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class replacingDurationInDaysToMonths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationInDays",
                table: "MemberShipPlans",
                newName: "DurationInMonths");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationInMonths",
                table: "MemberShipPlans",
                newName: "DurationInDays");
        }
    }
}
