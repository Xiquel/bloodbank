using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BloodBank.Authentication.Migrations
{
    public partial class FirstNameAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "asp_net_users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "asp_net_users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "asp_net_users");
        }
    }
}
