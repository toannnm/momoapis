﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DocumentStatus")
                        .HasColumnType("int");

                    b.Property<string>("Images")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("ModificatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Document");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000003-0000-0000-0000-000000000000"),
                            Content = "21 tuổi,đẹp trai",
                            CreatedBy = "ToanNM",
                            CreationDate = new DateTime(2024, 5, 17, 15, 1, 8, 962, DateTimeKind.Utc).AddTicks(5883),
                            Description = "Đây là nguyenmanhtoan",
                            DocumentStatus = 0,
                            IsDeleted = false,
                            Price = 100m,
                            Priority = 1,
                            Quantity = 100,
                            Title = "Giới thiệu"
                        },
                        new
                        {
                            Id = new Guid("00000004-0000-0000-0000-000000000000"),
                            Content = "22 tuổi,đẹp trai",
                            CreatedBy = "ToanNM",
                            CreationDate = new DateTime(2024, 5, 17, 15, 1, 8, 962, DateTimeKind.Utc).AddTicks(5889),
                            Description = "Những cái mô tả",
                            DocumentStatus = 0,
                            IsDeleted = false,
                            Price = 1000m,
                            Priority = 2,
                            Quantity = 100,
                            Title = "Giới thiệu căn bản"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("ModificatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("int");

                    b.Property<string>("PaymentUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Order");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f9f7cfa8-0bc0-4ec5-9fe1-b7f8e658b27a"),
                            CreatedBy = "ToanNM",
                            CreationDate = new DateTime(2024, 5, 17, 15, 1, 8, 962, DateTimeKind.Utc).AddTicks(9824),
                            IsDeleted = false,
                            OrderStatus = 2,
                            PaymentStatus = 0,
                            TotalPrice = 55000m,
                            UserId = new Guid("ec5e0e26-38b3-4c11-8030-cb1211cb1d53")
                        });
                });

            modelBuilder.Entity("Domain.Entities.OrderDetail", b =>
                {
                    b.Property<Guid>("DocumentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<decimal>("ItemPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ModificatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("DocumentId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetail");

                    b.HasData(
                        new
                        {
                            DocumentId = new Guid("00000003-0000-0000-0000-000000000000"),
                            OrderId = new Guid("f9f7cfa8-0bc0-4ec5-9fe1-b7f8e658b27a"),
                            CreatedBy = "ToanNM",
                            CreationDate = new DateTime(2024, 5, 17, 15, 1, 8, 965, DateTimeKind.Utc).AddTicks(3307),
                            IsDeleted = false,
                            ItemPrice = 100m,
                            Quantity = 5
                        },
                        new
                        {
                            DocumentId = new Guid("00000004-0000-0000-0000-000000000000"),
                            OrderId = new Guid("f9f7cfa8-0bc0-4ec5-9fe1-b7f8e658b27a"),
                            CreatedBy = "ToanNM",
                            CreationDate = new DateTime(2024, 5, 17, 15, 1, 8, 965, DateTimeKind.Utc).AddTicks(3321),
                            IsDeleted = false,
                            ItemPrice = 10000m,
                            Quantity = 5
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Images")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("ModificatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000001-0000-0000-0000-000000000000"),
                            Address = "Australia",
                            CreationDate = new DateTime(2024, 5, 17, 15, 1, 8, 965, DateTimeKind.Utc).AddTicks(7341),
                            Email = "Admin@gmail.com",
                            FullName = "Admin",
                            IsDeleted = false,
                            Password = "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522",
                            Phone = "08692743xx",
                            Role = 0,
                            Username = "Admin"
                        },
                        new
                        {
                            Id = new Guid("ec5e0e26-38b3-4c11-8030-cb1211cb1d53"),
                            Address = "Australia",
                            CreationDate = new DateTime(2024, 5, 17, 15, 1, 8, 965, DateTimeKind.Utc).AddTicks(7344),
                            Email = "Toanmnh2002@gmail.com",
                            FullName = "ToanNguyen",
                            IsDeleted = false,
                            Password = "$2a$12$v2Agh6VLGNyWXLZeb4aH/eBjYYF7WtMxqsmJLxSvKPkB4v.9pNzse",
                            Phone = "086927346x",
                            Role = 1,
                            Username = "ToanNGuyen"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Order", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.OrderDetail", b =>
                {
                    b.HasOne("Domain.Entities.Document", "Document")
                        .WithMany("OrderDetails")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Domain.Entities.Document", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("Domain.Entities.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
