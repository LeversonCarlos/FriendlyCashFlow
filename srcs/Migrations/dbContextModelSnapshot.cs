﻿// <auto-generated />
using System;
using FriendlyCashFlow.API.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FriendlyCashFlow.Migrations
{
    [DbContext(typeof(dbContext))]
    partial class dbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FriendlyCashFlow.API.Accounts.AccountData", b =>
                {
                    b.Property<long>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AccountID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnName("Active");

                    b.Property<short?>("ClosingDay")
                        .HasColumnName("ClosingDay");

                    b.Property<short?>("DueDay")
                        .HasColumnName("DueDay");

                    b.Property<string>("ResourceID")
                        .IsRequired()
                        .HasColumnName("ResourceID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("Text")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<short>("Type")
                        .HasColumnName("Type");

                    b.HasKey("AccountID");

                    b.ToTable("v6_dataAccounts");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Categories.CategoryData", b =>
                {
                    b.Property<long>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CategoryID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HierarchyText")
                        .IsRequired()
                        .HasColumnName("HierarchyText")
                        .HasColumnType("varchar(4000)")
                        .HasMaxLength(4000);

                    b.Property<long?>("ParentID")
                        .HasColumnName("ParentID");

                    b.Property<string>("ResourceID")
                        .IsRequired()
                        .HasColumnName("ResourceID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("Text")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<short>("Type")
                        .HasColumnName("Type");

                    b.HasKey("CategoryID");

                    b.ToTable("v6_dataCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
