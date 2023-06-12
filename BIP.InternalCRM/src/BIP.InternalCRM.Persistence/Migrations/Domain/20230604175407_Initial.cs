using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIP.InternalCRM.Persistence.Migrations.Domain
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "domain");

            migrationBuilder.EnsureSchema(
                name: "events");

            migrationBuilder.EnsureSchema(
                name: "shopify");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContactInfo_Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactInfo_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactInfo_Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DomainEvents",
                schema: "events",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccurredOn = table.Column<long>(type: "bigint", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEvents", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Icon_Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project_OriginalFilename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project_Filename = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsRelations",
                schema: "shopify",
                columns: table => new
                {
                    ShopifyProductId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    DomainProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsRelations", x => x.ShopifyProductId);
                });

            migrationBuilder.CreateTable(
                name: "CustomersRelations",
                schema: "domain",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersRelations", x => new { x.CustomerId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CustomersRelations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "domain",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomersRelations_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "domain",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leads",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContactInfo_Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactInfo_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactInfo_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cost = table.Column<decimal>(type: "money", precision: 38, scale: 8, nullable: false),
                    StartDate = table.Column<long>(type: "bigint", nullable: false),
                    EndDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leads_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "domain",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "money", precision: 38, scale: 8, nullable: false),
                    ReceivedDate = table.Column<long>(type: "bigint", nullable: false),
                    DueDate = table.Column<long>(type: "bigint", nullable: false),
                    PaidDate = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "domain",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "domain",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "money", precision: 38, scale: 8, nullable: false),
                    ReceivedDate = table.Column<long>(type: "bigint", nullable: false),
                    DueDate = table.Column<long>(type: "bigint", nullable: false),
                    PaidDate = table.Column<long>(type: "bigint", nullable: true),
                    IsOverdue = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "domain",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "domain",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "domain",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "money", precision: 38, scale: 8, nullable: false),
                    ReceivedDate = table.Column<long>(type: "bigint", nullable: false),
                    IsOverdue = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "domain",
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubLegalEntity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "money", precision: 38, scale: 8, nullable: false),
                    PaidDate = table.Column<long>(type: "bigint", nullable: true),
                    ValidFrom = table.Column<long>(type: "bigint", nullable: false),
                    ValidUntil = table.Column<long>(type: "bigint", nullable: false),
                    License_Key = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    License_Filename = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "domain",
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name",
                schema: "domain",
                table: "Customers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomersRelations_ProductId",
                schema: "domain",
                table: "CustomersRelations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                schema: "domain",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Number",
                schema: "domain",
                table: "Invoices",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ProductId",
                schema: "domain",
                table: "Invoices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PurchaseOrderId",
                schema: "domain",
                table: "Invoices",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_Name",
                schema: "domain",
                table: "Leads",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ProductId",
                schema: "domain",
                table: "Leads",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_InvoiceId",
                schema: "domain",
                table: "Payments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Number",
                schema: "domain",
                table: "Payments",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                schema: "domain",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsRelations_DomainProductId",
                schema: "shopify",
                table: "ProductsRelations",
                column: "DomainProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CustomerId",
                schema: "domain",
                table: "PurchaseOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Number",
                schema: "domain",
                table: "PurchaseOrders",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ProductId",
                schema: "domain",
                table: "PurchaseOrders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_InvoiceId",
                schema: "domain",
                table: "Subscriptions",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Number",
                schema: "domain",
                table: "Subscriptions",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomersRelations",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "DomainEvents",
                schema: "events");

            migrationBuilder.DropTable(
                name: "Leads",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "ProductsRelations",
                schema: "shopify");

            migrationBuilder.DropTable(
                name: "Subscriptions",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "domain");
        }
    }
}
