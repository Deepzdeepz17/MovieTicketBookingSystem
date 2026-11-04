
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingSystem.Migrations
{
    public partial class AddUserIdToBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add UserId as nullable temporarily
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Bookings",
                type: "int",
                nullable: true);

            // Step 2: Assign existing bookings to registered users
            migrationBuilder.Sql(@"
                UPDATE b
                SET b.UserId = u.UserId
                FROM Bookings b
                INNER JOIN Users u
                    ON b.CustomerEmail = u.Email;
            ");

            // Step 3: Stop migration if any booking has no matching user
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1
                    FROM Bookings
                    WHERE UserId IS NULL
                )
                BEGIN
                    THROW 50001,
                    'Some bookings could not be associated with users.',
                    1;
                END
            ");

            // Step 4: Make UserId mandatory
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Step 5: Create index
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            // Step 6: Create foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bookings");
        }
    }
}
