using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebReservation.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuestName = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    ReservationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Hours = table.Column<int>(type: "integer", nullable: false),
                    NumTable = table.Column<int>(type: "integer", nullable: false),
                    hall = table.Column<int>(type: "integer", nullable: false),
                    GuestComment = table.Column<string>(type: "text", nullable: true),
                    EndTimeDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    GuestNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
