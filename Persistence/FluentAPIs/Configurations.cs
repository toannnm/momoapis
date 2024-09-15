using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Newtonsoft.Json;

namespace Persistence.FluentAPIs
{
    public class Configurations
    {
        public class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
        {
            public virtual void Configure(EntityTypeBuilder<TEntity> builder)
            {
                builder.HasKey(x => x.Id);
                builder.HasQueryFilter(x => !x.IsDeleted);
                builder.Property(x => x.Id).HasValueGenerator<SequentialGuidValueGenerator>().ValueGeneratedOnAdd();
                builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            }
        }

        public class UserConfiguration : BaseConfiguration<User>
        {
            public override void Configure(EntityTypeBuilder<User> builder)
            {
                base.Configure(builder);

                builder.Property(x => x.FullName)
                       .IsRequired();

                builder.HasIndex(x => x.Email).IsUnique();

                builder.HasIndex(x => x.Phone).IsUnique();

                builder.HasIndex(x => x.Username).IsUnique();

                builder.Property(x => x.Password)
                       .IsRequired();

                builder.HasData(new User
                {
                    Id = new Guid("00000001-0000-0000-0000-000000000000"),
                    Email = "Admin@gmail.com",
                    Password = "$2a$11$l9kCBg7x7MIaQkIv0gR7Ve.Q89G1EaLZUqW3WXsX7qKRJklzGi522",
                    Address = "Australia",
                    Username = "Admin",
                    Phone = "08692743xx",
                    FullName = "Admin",
                    Role = RoleEnum.Admin,
                    CreationDate = DateTime.UtcNow
                },
                new User
                {
                    Id = new Guid("ec5e0e26-38b3-4c11-8030-cb1211cb1d53"),
                    Email = "Toanmnh2002@gmail.com",
                    CreationDate = DateTime.UtcNow,
                    Password = "$2a$12$v2Agh6VLGNyWXLZeb4aH/eBjYYF7WtMxqsmJLxSvKPkB4v.9pNzse",
                    Phone = "086927346x",
                    Username = "ToanNGuyen",
                    Address = "Australia",
                    Role = RoleEnum.Customer,
                    FullName = "ToanNguyen"
                }

                );

                builder.Property(x => x.Images)
                       .HasConversion(
                           jsonToString => JsonConvert.SerializeObject(jsonToString),
                           stringToJson => JsonConvert.DeserializeObject<List<string>>(stringToJson));
            }
        }

        public class DocumentConfiguration : BaseConfiguration<Document>
        {
            public override void Configure(EntityTypeBuilder<Document> builder)
            {
                base.Configure(builder);

                builder.HasKey(x => x.Id);

                builder.HasData(new Document
                {
                    Id = new Guid("00000003-0000-0000-0000-000000000000"),
                    Description = "Đây là nguyenmanhtoan",
                    Title = "Giới thiệu",
                    Content = "21 tuổi,đẹp trai",
                    Quantity = 100,
                    Price = 100,
                    Priority = PriorityEnum.Medium,
                    DocumentStatus = DocumentStatusEnum.Active,
                    CreationDate = DateTime.UtcNow,
                    CreatedBy = "ToanNM"
                },
                new Document
                {
                    Id = new Guid("00000004-0000-0000-0000-000000000000"),
                    Description = "Những cái mô tả",
                    Title = "Giới thiệu căn bản",
                    Content = "22 tuổi,đẹp trai",
                    Quantity = 100,
                    Price = 1000,
                    Priority = PriorityEnum.High,
                    DocumentStatus = DocumentStatusEnum.Active,
                    CreationDate = DateTime.UtcNow,
                    CreatedBy = "ToanNM"
                });

                builder.Property(x => x.Images)
                       .HasConversion(
                           jsonToString => JsonConvert.SerializeObject(jsonToString),
                           stringToJson => JsonConvert.DeserializeObject<List<string>>(stringToJson));
            }
        }
        public class OrderDetailTagConfiguration : BaseConfiguration<OrderDetail>
        {
            public override void Configure(EntityTypeBuilder<OrderDetail> builder)
            {
                base.Configure(builder);
                builder.HasKey(x => new { x.DocumentId, x.OrderId });
                builder.Ignore(x => x.Id);
                builder.HasOne(x => x.Order)
                       .WithMany(x => x.OrderDetails)
                       .HasForeignKey(x => x.OrderId);

                builder.HasOne(x => x.Document)
                       .WithMany(x => x.OrderDetails)
                       .HasForeignKey(x => x.DocumentId);

                builder.HasData(new OrderDetail
                {
                    CreationDate = DateTime.UtcNow,
                    DocumentId = new Guid("00000003-0000-0000-0000-000000000000"),
                    OrderId = new Guid("f9f7cfa8-0bc0-4ec5-9fe1-b7f8e658b27a"),
                    Quantity = 5,
                    ItemPrice = 100,
                    CreatedBy = "ToanNM"
                },
                new OrderDetail
                {
                    CreationDate = DateTime.UtcNow,
                    DocumentId = new Guid("00000004-0000-0000-0000-000000000000"),
                    OrderId = new Guid("f9f7cfa8-0bc0-4ec5-9fe1-b7f8e658b27a"),
                    ItemPrice = 10000,
                    Quantity = 5,
                    CreatedBy = "ToanNM"
                });
            }
        }

        public class OrderConfiguration : BaseConfiguration<Order>
        {
            public override void Configure(EntityTypeBuilder<Order> builder)
            {
                base.Configure(builder);
                builder.HasOne(x => x.User)
                       .WithMany(x => x.Orders)
                       .HasForeignKey(x => x.UserId);
                builder.HasData(new Order
                {
                    Id = new Guid("f9f7cfa8-0bc0-4ec5-9fe1-b7f8e658b27a"),
                    UserId = new Guid("ec5e0e26-38b3-4c11-8030-cb1211cb1d53"),
                    CreationDate = DateTime.UtcNow,
                    PaymentMethod = null,
                    PaymentUrl = null,
                    TotalPrice = 55000,
                    PaymentStatus = PaymentStatusEnum.Inprocessing,
                    OrderStatus = OrderStatusEnum.Completed,
                    CreatedBy = "ToanNM",
                });
            }
        }

    }
}
