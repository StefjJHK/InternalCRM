﻿// <auto-generated />
using System;
using BIP.InternalCRM.WebIdentity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BIP.InternalCRM.WebIdentity.Migrations.Auth
{
    [DbContext(typeof(AuthDbContext))]
    partial class AuthDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("auth")
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BIP.InternalCRM.WebIdentity.Permissions.PermissionSet", b =>
                {
                    b.Property<Guid>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserRoleId");

                    b.ToTable("Permissions", "auth");
                });

            modelBuilder.Entity("BIP.InternalCRM.WebIdentity.Roles.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid>("PermissionSetUserRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Priority")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.HasIndex("PermissionSetUserRoleId");

                    b.ToTable("Roles", "auth");
                });

            modelBuilder.Entity("BIP.InternalCRM.WebIdentity.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PermissionSetUserRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PermissionSetUserRoleId");

                    b.ToTable("Users", "auth");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", "auth");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", "auth");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserLogins", "auth");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles", "auth");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserTokens", "auth");
                });

            modelBuilder.Entity("BIP.InternalCRM.WebIdentity.Permissions.PermissionSet", b =>
                {
                    b.OwnsOne("BIP.InternalCRM.WebIdentity.Permissions.AnalyticsPermissionSet", "Analytics", b1 =>
                        {
                            b1.Property<Guid>("PermissionSetUserRoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("CanRead")
                                .HasColumnType("bit");

                            b1.Property<bool?>("CanWrite")
                                .HasColumnType("bit");

                            b1.HasKey("PermissionSetUserRoleId");

                            b1.ToTable("Permissions", "auth");

                            b1.WithOwner()
                                .HasForeignKey("PermissionSetUserRoleId");
                        });

                    b.OwnsOne("BIP.InternalCRM.WebIdentity.Permissions.CustomerPermissionSet", "Customer", b1 =>
                        {
                            b1.Property<Guid>("PermissionSetUserRoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("CanRead")
                                .HasColumnType("bit");

                            b1.Property<bool?>("CanWrite")
                                .HasColumnType("bit");

                            b1.HasKey("PermissionSetUserRoleId");

                            b1.ToTable("Permissions", "auth");

                            b1.WithOwner()
                                .HasForeignKey("PermissionSetUserRoleId");
                        });

                    b.OwnsOne("BIP.InternalCRM.WebIdentity.Permissions.InvoicePermissionSet", "Invoice", b1 =>
                        {
                            b1.Property<Guid>("PermissionSetUserRoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("CanRead")
                                .HasColumnType("bit");

                            b1.Property<bool?>("CanWrite")
                                .HasColumnType("bit");

                            b1.HasKey("PermissionSetUserRoleId");

                            b1.ToTable("Permissions", "auth");

                            b1.WithOwner()
                                .HasForeignKey("PermissionSetUserRoleId");
                        });

                    b.OwnsOne("BIP.InternalCRM.WebIdentity.Permissions.LeadPermissionSet", "Lead", b1 =>
                        {
                            b1.Property<Guid>("PermissionSetUserRoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("CanRead")
                                .HasColumnType("bit");

                            b1.Property<bool?>("CanWrite")
                                .HasColumnType("bit");

                            b1.HasKey("PermissionSetUserRoleId");

                            b1.ToTable("Permissions", "auth");

                            b1.WithOwner()
                                .HasForeignKey("PermissionSetUserRoleId");
                        });

                    b.OwnsOne("BIP.InternalCRM.WebIdentity.Permissions.PaymentPermissionSet", "Payment", b1 =>
                        {
                            b1.Property<Guid>("PermissionSetUserRoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("CanRead")
                                .HasColumnType("bit");

                            b1.Property<bool?>("CanWrite")
                                .HasColumnType("bit");

                            b1.HasKey("PermissionSetUserRoleId");

                            b1.ToTable("Permissions", "auth");

                            b1.WithOwner()
                                .HasForeignKey("PermissionSetUserRoleId");
                        });

                    b.OwnsOne("BIP.InternalCRM.WebIdentity.Permissions.ProductPermissionSet", "Product", b1 =>
                        {
                            b1.Property<Guid>("PermissionSetUserRoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("CanRead")
                                .HasColumnType("bit");

                            b1.Property<bool?>("CanWrite")
                                .HasColumnType("bit");

                            b1.HasKey("PermissionSetUserRoleId");

                            b1.ToTable("Permissions", "auth");

                            b1.WithOwner()
                                .HasForeignKey("PermissionSetUserRoleId");
                        });

                    b.OwnsOne("BIP.InternalCRM.WebIdentity.Permissions.PurchaseOrderPermissionSet", "PurchaseOrder", b1 =>
                        {
                            b1.Property<Guid>("PermissionSetUserRoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("CanRead")
                                .HasColumnType("bit");

                            b1.Property<bool?>("CanWrite")
                                .HasColumnType("bit");

                            b1.HasKey("PermissionSetUserRoleId");

                            b1.ToTable("Permissions", "auth");

                            b1.WithOwner()
                                .HasForeignKey("PermissionSetUserRoleId");
                        });

                    b.OwnsOne("BIP.InternalCRM.WebIdentity.Permissions.SubscriptionPermissionSet", "Subscription", b1 =>
                        {
                            b1.Property<Guid>("PermissionSetUserRoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("CanRead")
                                .HasColumnType("bit");

                            b1.Property<bool?>("CanWrite")
                                .HasColumnType("bit");

                            b1.HasKey("PermissionSetUserRoleId");

                            b1.ToTable("Permissions", "auth");

                            b1.WithOwner()
                                .HasForeignKey("PermissionSetUserRoleId");
                        });

                    b.Navigation("Analytics")
                        .IsRequired();

                    b.Navigation("Customer")
                        .IsRequired();

                    b.Navigation("Invoice")
                        .IsRequired();

                    b.Navigation("Lead")
                        .IsRequired();

                    b.Navigation("Payment")
                        .IsRequired();

                    b.Navigation("Product")
                        .IsRequired();

                    b.Navigation("PurchaseOrder")
                        .IsRequired();

                    b.Navigation("Subscription")
                        .IsRequired();
                });

            modelBuilder.Entity("BIP.InternalCRM.WebIdentity.Roles.Role", b =>
                {
                    b.HasOne("BIP.InternalCRM.WebIdentity.Permissions.PermissionSet", "PermissionSet")
                        .WithMany()
                        .HasForeignKey("PermissionSetUserRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PermissionSet");
                });

            modelBuilder.Entity("BIP.InternalCRM.WebIdentity.Users.User", b =>
                {
                    b.HasOne("BIP.InternalCRM.WebIdentity.Permissions.PermissionSet", "PermissionSet")
                        .WithMany()
                        .HasForeignKey("PermissionSetUserRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PermissionSet");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("BIP.InternalCRM.WebIdentity.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("BIP.InternalCRM.WebIdentity.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("BIP.InternalCRM.WebIdentity.Users.User", null)
                        .WithOne()
                        .HasForeignKey("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("BIP.InternalCRM.WebIdentity.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BIP.InternalCRM.WebIdentity.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("BIP.InternalCRM.WebIdentity.Users.User", null)
                        .WithOne()
                        .HasForeignKey("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
