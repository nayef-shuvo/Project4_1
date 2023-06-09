using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project4_1.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthDatabse",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "BLOB", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthDatabse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemDatabse",
                columns: table => new
                {
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SubItem = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDatabse", x => x.Title);
                });

            migrationBuilder.CreateTable(
                name: "TeacherDatabse",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Rank = table.Column<int>(type: "INTEGER", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherDatabse", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthDatabse");

            migrationBuilder.DropTable(
                name: "ItemDatabse");

            migrationBuilder.DropTable(
                name: "TeacherDatabse");
        }
    }
}
