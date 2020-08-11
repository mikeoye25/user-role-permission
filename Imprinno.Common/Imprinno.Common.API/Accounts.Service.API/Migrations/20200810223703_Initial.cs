using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Accounts.Service.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mess");

            migrationBuilder.EnsureSchema(
                name: "acc");

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "acc",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(maxLength: 30, nullable: false),
                    description = table.Column<string>(nullable: true),
                    permissions = table.Column<string>(nullable: true),
                    createdon = table.Column<DateTime>(nullable: false),
                    updatedon = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                schema: "mess",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdon = table.Column<DateTime>(nullable: false),
                    updatedon = table.Column<DateTime>(nullable: false),
                    to = table.Column<string>(maxLength: 20, nullable: false),
                    from = table.Column<string>(maxLength: 20, nullable: true),
                    sendername = table.Column<string>(maxLength: 100, nullable: true),
                    subject = table.Column<string>(maxLength: 255, nullable: true),
                    body = table.Column<string>(maxLength: 8192, nullable: false),
                    messagetype = table.Column<int>(nullable: false),
                    status = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "acc",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    username = table.Column<string>(maxLength: 30, nullable: false),
                    passwordhash = table.Column<string>(nullable: false),
                    passwordsalt = table.Column<string>(nullable: true),
                    firstname = table.Column<string>(maxLength: 512, nullable: false),
                    lastname = table.Column<string>(maxLength: 512, nullable: false),
                    address = table.Column<string>(nullable: true),
                    email = table.Column<string>(maxLength: 255, nullable: false),
                    emailverified = table.Column<bool>(nullable: true),
                    phone = table.Column<string>(maxLength: 50, nullable: true),
                    phoneverified = table.Column<bool>(nullable: true),
                    dob = table.Column<DateTime>(type: "date", nullable: true),
                    sex = table.Column<string>(maxLength: 20, nullable: true),
                    aboutme = table.Column<string>(nullable: true),
                    profileimageurl = table.Column<string>(maxLength: 255, nullable: true),
                    registrationip = table.Column<string>(maxLength: 50, nullable: true),
                    lastseen = table.Column<DateTimeOffset>(nullable: true),
                    isdisabled = table.Column<bool>(nullable: true),
                    isdeleted = table.Column<bool>(nullable: true),
                    language = table.Column<string>(maxLength: 30, nullable: true),
                    roleid = table.Column<Guid>(nullable: true),
                    createdon = table.Column<DateTime>(nullable: false),
                    updatedon = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_roles_roleid",
                        column: x => x.roleid,
                        principalSchema: "acc",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_roleid",
                schema: "acc",
                table: "users",
                column: "roleid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users",
                schema: "acc");

            migrationBuilder.DropTable(
                name: "messages",
                schema: "mess");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "acc");
        }
    }
}
