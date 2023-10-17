using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mvc.DataAccess.Migrations
{
    public partial class SeedDataCoverType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 2, 40, 50, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 3, 44, 12, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 4, 55, 23, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 5, 22, 34, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "CoverTypes",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 10, 17, 2, 40, 50, 0, DateTimeKind.Unspecified), "Softcover" },
                    { 2, new DateTime(2023, 10, 17, 3, 40, 50, 0, DateTimeKind.Unspecified), "Paperback" },
                    { 3, new DateTime(2023, 10, 17, 4, 40, 50, 0, DateTimeKind.Unspecified), "Hardcover" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CoverTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CoverTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CoverTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 14, 13, 51, 358, DateTimeKind.Local).AddTicks(246));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 14, 13, 51, 358, DateTimeKind.Local).AddTicks(254));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 14, 13, 51, 358, DateTimeKind.Local).AddTicks(255));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 14, 13, 51, 358, DateTimeKind.Local).AddTicks(256));
        }
    }
}
