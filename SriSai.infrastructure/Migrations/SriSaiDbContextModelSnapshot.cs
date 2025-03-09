﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SriSai.infrastructure.Persistent.DbContext;

#nullable disable

namespace SriSai.infrastructure.Migrations
{
    [DbContext(typeof(SriSaiDbContext))]
    partial class SriSaiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SriSai.Domain.Entity.Users.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasMaxLength(38)
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("CreatedDateTime")
                        .IsRequired()
                        .HasColumnType("timestamp");

                    b.Property<Guid?>("DeletedBy")
                        .HasMaxLength(38)
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("DeletedDateTime")
                        .HasColumnType("timestamp");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUserActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("UpdatedById")
                        .HasMaxLength(38)
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("UpdatedDateTime")
                        .HasColumnType("timestamp");

                    b.HasKey("Id");

                    b.HasIndex("Mobile")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("SriSai.Domain.Entity.Users.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserRoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserEntityId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("SriSai.Domain.Entity.Users.UserRole", b =>
                {
                    b.HasOne("SriSai.Domain.Entity.Users.UserEntity", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SriSai.Domain.Entity.Users.UserEntity", b =>
                {
                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
