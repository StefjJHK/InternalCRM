﻿// <auto-generated />
using System;
using BIP.InternalCRM.Persistence.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BIP.InternalCRM.Persistence.Migrations.Domain
{
    [DbContext(typeof(DomainDbContext))]
    [Migration("20230604175407_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("domain")
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BIP.InternalCRM.Application.Customers.CustomerRelations", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CustomerId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("CustomersRelations", "domain");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Customers.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Customers", "domain");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Invoices.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(38, 8)
                        .HasColumnType("money");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("DueDate")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsOverdue")
                        .HasColumnType("bit");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("PaidDate")
                        .HasColumnType("bigint");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PurchaseOrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("ReceivedDate")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("ProductId");

                    b.HasIndex("PurchaseOrderId");

                    b.ToTable("Invoices", "domain");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Leads.Lead", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Cost")
                        .HasPrecision(38, 8)
                        .HasColumnType("money");

                    b.Property<long>("EndDate")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("StartDate")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ProductId");

                    b.ToTable("Leads", "domain");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Payments.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(38, 8)
                        .HasColumnType("money");

                    b.Property<Guid>("InvoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsOverdue")
                        .HasColumnType("bit");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("ReceivedDate")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("Payments", "domain");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Products", "domain");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.PurchaseOrders.PurchaseOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(38, 8)
                        .HasColumnType("money");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("DueDate")
                        .HasColumnType("bigint");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("PaidDate")
                        .HasColumnType("bigint");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("ReceivedDate")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("ProductId");

                    b.ToTable("PurchaseOrders", "domain");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Subscriptions.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Cost")
                        .HasPrecision(38, 8)
                        .HasColumnType("money");

                    b.Property<Guid>("InvoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("PaidDate")
                        .HasColumnType("bigint");

                    b.Property<string>("SubLegalEntity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ValidFrom")
                        .HasColumnType("bigint");

                    b.Property<long>("ValidUntil")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("Subscriptions", "domain");
                });

            modelBuilder.Entity("BIP.InternalCRM.Persistence.Domain.DomainEventDbEntity", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("OccurredOn")
                        .HasColumnType("bigint");

                    b.HasKey("EventId");

                    b.ToTable("DomainEvents", "events");
                });

            modelBuilder.Entity("BIP.InternalCRM.Shopify.Entities.ShopifyProductsRelations", b =>
                {
                    b.Property<ulong>("ShopifyProductId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<Guid>("DomainProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ShopifyProductId");

                    b.HasIndex("DomainProductId")
                        .IsUnique();

                    b.ToTable("ProductsRelations", "shopify");
                });

            modelBuilder.Entity("BIP.InternalCRM.Application.Customers.CustomerRelations", b =>
                {
                    b.HasOne("BIP.InternalCRM.Domain.Customers.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BIP.InternalCRM.Domain.Products.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Customers.Customer", b =>
                {
                    b.OwnsOne("BIP.InternalCRM.Domain.ValueObjects.ContactInfo", "ContactInfo", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Fullname")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers", "domain");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("ContactInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Invoices.Invoice", b =>
                {
                    b.HasOne("BIP.InternalCRM.Domain.Customers.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BIP.InternalCRM.Domain.Products.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BIP.InternalCRM.Domain.PurchaseOrders.PurchaseOrder", "PurchaseOrder")
                        .WithMany("Invoices")
                        .HasForeignKey("PurchaseOrderId");

                    b.Navigation("PurchaseOrder");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Leads.Lead", b =>
                {
                    b.HasOne("BIP.InternalCRM.Domain.Products.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("BIP.InternalCRM.Domain.ValueObjects.ContactInfo", "ContactInfo", b1 =>
                        {
                            b1.Property<Guid>("LeadId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Fullname")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LeadId");

                            b1.ToTable("Leads", "domain");

                            b1.WithOwner()
                                .HasForeignKey("LeadId");
                        });

                    b.Navigation("ContactInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Payments.Payment", b =>
                {
                    b.HasOne("BIP.InternalCRM.Domain.Invoices.Invoice", null)
                        .WithMany("Payments")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Products.Product", b =>
                {
                    b.OwnsOne("BIP.InternalCRM.Domain.Products.IntelliLockProject", "Project", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Filename")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("OriginalFilename")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products", "domain");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("BIP.InternalCRM.Domain.ValueObjects.Image", "Icon", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Filename")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products", "domain");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("Icon");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.PurchaseOrders.PurchaseOrder", b =>
                {
                    b.HasOne("BIP.InternalCRM.Domain.Customers.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BIP.InternalCRM.Domain.Products.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Subscriptions.Subscription", b =>
                {
                    b.HasOne("BIP.InternalCRM.Domain.Invoices.Invoice", null)
                        .WithMany("Subscriptions")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("BIP.InternalCRM.Domain.Subscriptions.IntelliLockLicense", "License", b1 =>
                        {
                            b1.Property<Guid>("SubscriptionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Filename")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<Guid>("Key")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("SubscriptionId");

                            b1.ToTable("Subscriptions", "domain");

                            b1.WithOwner()
                                .HasForeignKey("SubscriptionId");
                        });

                    b.Navigation("License");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.Invoices.Invoice", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("BIP.InternalCRM.Domain.PurchaseOrders.PurchaseOrder", b =>
                {
                    b.Navigation("Invoices");
                });
#pragma warning restore 612, 618
        }
    }
}
