using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    DocumentStatus = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ItemPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => new { x.DocumentId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_OrderDetail_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Document",
                columns: new[] { "Id", "Content", "CreatedBy", "CreationDate", "DeletedBy", "DeletionDate", "Description", "DocumentStatus", "Images", "ModificatedBy", "ModificationDate", "Price", "Priority", "Quantity", "Title" },
                values: new object[,]
                {
                    { new Guid("00000003-0000-0000-0000-000000000000"), "21 tuổi,đẹp trai", "ToanNM", new DateTime(2024, 5, 17, 15, 1, 8, 962, DateTimeKind.Utc).AddTicks(5883), null, null, "Đây là nguyenmanhtoan", 0, null, null, null, 100m, 1, 100, "Giới thiệu" },
                    { new Guid("00000004-0000-0000-0000-000000000000"), "22 tuổi,đẹp trai", "ToanNM", new DateTime(2024, 5, 17, 15, 1, 8, 962, DateTimeKind.Utc).AddTicks(5889), null, null, "Những cái mô tả", 0, null, null, null, 1000m, 2, 100, "Giới thiệu căn bản" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Address", "CreatedBy", "CreationDate", "DeletedBy", "DeletionDate", "Email", "FullName", "Images", "ModificatedBy", "ModificationDate", "Password", "Phone", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("00000001-0000-0000-0000-000000000000"), "Australia", null, new DateTime(2024, 5, 17, 15, 1, 8, 965, DateTimeKind.Utc).AddTicks(7341), null, null, "Admin@gmail.com", "Admin", null, null, null, "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522", "08692743xx", 0, "Admin" },
                    { new Guid("ec5e0e26-38b3-4c11-8030-cb1211cb1d53"), "Australia", null, new DateTime(2024, 5, 17, 15, 1, 8, 965, DateTimeKind.Utc).AddTicks(7344), null, null, "Toanmnh2002@gmail.com", "ToanNguyen", null, null, null, "$2a$12$v2Agh6VLGNyWXLZeb4aH/eBjYYF7WtMxqsmJLxSvKPkB4v.9pNzse", "086927346x", 1, "ToanNGuyen" }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeletedBy", "DeletionDate", "ModificatedBy", "ModificationDate", "OrderStatus", "PaymentMethod", "PaymentStatus", "PaymentUrl", "TotalPrice", "UserId" },
                values: new object[] { new Guid("f9f7cfa8-0bc0-4ec5-9fe1-b7f8e658b27a"), "ToanNM", new DateTime(2024, 5, 17, 15, 1, 8, 962, DateTimeKind.Utc).AddTicks(9824), null, null, null, null, 2, null, 0, null, 55000m, new Guid("ec5e0e26-38b3-4c11-8030-cb1211cb1d53") });

            migrationBuilder.InsertData(
                table: "OrderDetail",
                columns: new[] { "DocumentId", "OrderId", "CreatedBy", "CreationDate", "DeletedBy", "DeletionDate", "ItemPrice", "ModificatedBy", "ModificationDate", "Quantity" },
                values: new object[,]
                {
                    { new Guid("00000003-0000-0000-0000-000000000000"), new Guid("f9f7cfa8-0bc0-4ec5-9fe1-b7f8e658b27a"), "ToanNM", new DateTime(2024, 5, 17, 15, 1, 8, 965, DateTimeKind.Utc).AddTicks(3307), null, null, 100m, null, null, 5 },
                    { new Guid("00000004-0000-0000-0000-000000000000"), new Guid("f9f7cfa8-0bc0-4ec5-9fe1-b7f8e658b27a"), "ToanNM", new DateTime(2024, 5, 17, 15, 1, 8, 965, DateTimeKind.Utc).AddTicks(3321), null, null, 10000m, null, null, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Phone",
                table: "User",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
