using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mvc.DataAccess.Migrations
{
    public partial class SeedDataProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "CoverTypeId", "CreatedAt", "Description", "ISBN", "ImageUrl", "Name", "Price", "Price100", "Price50" },
                values: new object[,]
                {
                    { 1, "Neque porro", 1, 1, new DateTime(2023, 10, 17, 7, 40, 50, 0, DateTimeKind.Unspecified), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam at erat vitae odio porttitor egestas. Duis tempor congue ex, luctus gravida felis ullamcorper nec. Donec mollis urna at justo pellentesque, ut sodales purus lobortis. Proin lacus tellus, lacinia eu leo ut, congue sollicitudin urna. In at varius lorem. Aliquam ornare eleifend dui, quis convallis orci luctus nec. Donec pellentesque molestie mi pretium accumsan. Phasellus rutrum, magna in finibus feugiat, sapien ligula varius odio, vel viverra velit diam id augue. Nunc felis mi, dignissim sit amet sollicitudin eu, tincidunt ac mi. Praesent eu pulvinar mi. Vestibulum leo ante, vestibulum lobortis justo volutpat, vulputate vestibulum orci.", "1234567890123", "default.jpg", "Secrets of Divine Love: A Spiritual Journey into the Heart of Islam", 50000.0, 30000.0, 40000.0 },
                    { 2, "Sloane Crosley (Goodreads Author)", 2, 2, new DateTime(2023, 10, 18, 7, 40, 50, 0, DateTimeKind.Unspecified), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam at erat vitae odio porttitor egestas. Duis tempor congue ex, luctus gravida felis ullamcorper nec. Donec mollis urna at justo pellentesque, ut sodales purus lobortis. Proin lacus tellus, lacinia eu leo ut, congue sollicitudin urna. In at varius lorem. Aliquam ornare eleifend dui, quis convallis orci luctus nec. Donec pellentesque molestie mi pretium accumsan. Phasellus rutrum, magna in finibus feugiat, sapien ligula varius odio, vel viverra velit diam id augue. Nunc felis mi, dignissim sit amet sollicitudin eu, tincidunt ac mi. Praesent eu pulvinar mi. Vestibulum leo ante, vestibulum lobortis justo volutpat, vulputate vestibulum orci.", "1234567890124", "default.jpg", "I Was Told There'd Be Cake: Essays", 25000.0, 10000.0, 15000.0 },
                    { 3, "Anthony Burgess", 3, 3, new DateTime(2023, 10, 19, 8, 40, 50, 0, DateTimeKind.Unspecified), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam at erat vitae odio porttitor egestas. Duis tempor congue ex, luctus gravida felis ullamcorper nec. Donec mollis urna at justo pellentesque, ut sodales purus lobortis. Proin lacus tellus, lacinia eu leo ut, congue sollicitudin urna. In at varius lorem. Aliquam ornare eleifend dui, quis convallis orci luctus nec. Donec pellentesque molestie mi pretium accumsan. Phasellus rutrum, magna in finibus feugiat, sapien ligula varius odio, vel viverra velit diam id augue. Nunc felis mi, dignissim sit amet sollicitudin eu, tincidunt ac mi. Praesent eu pulvinar mi. Vestibulum leo ante, vestibulum lobortis justo volutpat, vulputate vestibulum orci.", "1234567890125", "default.jpg", "A Clockwork Orange", 40000.0, 20000.0, 30000.0 },
                    { 4, "Philip K. Dick", 1, 2, new DateTime(2023, 10, 20, 8, 40, 50, 0, DateTimeKind.Unspecified), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam at erat vitae odio porttitor egestas. Duis tempor congue ex, luctus gravida felis ullamcorper nec. Donec mollis urna at justo pellentesque, ut sodales purus lobortis. Proin lacus tellus, lacinia eu leo ut, congue sollicitudin urna. In at varius lorem. Aliquam ornare eleifend dui, quis convallis orci luctus nec. Donec pellentesque molestie mi pretium accumsan. Phasellus rutrum, magna in finibus feugiat, sapien ligula varius odio, vel viverra velit diam id augue. Nunc felis mi, dignissim sit amet sollicitudin eu, tincidunt ac mi. Praesent eu pulvinar mi. Vestibulum leo ante, vestibulum lobortis justo volutpat, vulputate vestibulum orci.", "1234567890126", "default.jpg", "Do Androids Dream of Electric Sheep?", 60000.0, 20000.0, 40000.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
