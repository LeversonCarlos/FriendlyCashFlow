﻿// <auto-generated />
using System;
using FriendlyCashFlow.API.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FriendlyCashFlow.Migrations
{
    [DbContext(typeof(dbContext))]
    [Migration("20210817000251_Budget")]
    partial class Budget
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FriendlyCashFlow.API.Accounts.AccountData", b =>
                {
                    b.Property<long>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AccountID")
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnName("Active")
                        .HasColumnType("bit");

                    b.Property<short?>("ClosingDay")
                        .HasColumnName("ClosingDay")
                        .HasColumnType("smallint");

                    b.Property<short?>("DueDay")
                        .HasColumnName("DueDay")
                        .HasColumnType("smallint");

                    b.Property<string>("ResourceID")
                        .IsRequired()
                        .HasColumnName("ResourceID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus")
                        .HasColumnType("smallint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("Text")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<short>("Type")
                        .HasColumnName("Type")
                        .HasColumnType("smallint");

                    b.HasKey("AccountID");

                    b.HasIndex("RowStatus", "ResourceID", "AccountID", "Text")
                        .HasName("v6_dataAccounts_index_Search");

                    b.ToTable("v6_dataAccounts");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Balances.BalanceData", b =>
                {
                    b.Property<string>("ResourceID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<long>("AccountID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PaidValue")
                        .HasColumnType("decimal(15,2)");

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus")
                        .HasColumnType("smallint");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("decimal(15,2)");

                    b.HasKey("ResourceID", "AccountID", "Date");

                    b.HasIndex("RowStatus", "ResourceID", "Date", "AccountID")
                        .HasName("v6_dataBalance_index_Search");

                    b.ToTable("v6_dataBalance");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Budget.BudgetData", b =>
                {
                    b.Property<long>("BudgetID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("PatternID")
                        .HasColumnType("bigint");

                    b.Property<string>("ResourceID")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus")
                        .HasColumnType("smallint");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(15,2)");

                    b.HasKey("BudgetID");

                    b.HasIndex("PatternID");

                    b.HasIndex("RowStatus", "ResourceID", "PatternID")
                        .HasName("v6_dataBudget_index_Search")
                        .HasAnnotation("SqlServer:Include", new[] { "BudgetID", "Value" });

                    b.ToTable("v6_dataBudget");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Categories.CategoryData", b =>
                {
                    b.Property<long>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CategoryID")
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HierarchyText")
                        .IsRequired()
                        .HasColumnName("HierarchyText")
                        .HasColumnType("varchar(4000)")
                        .HasMaxLength(4000);

                    b.Property<long?>("ParentID")
                        .HasColumnName("ParentID")
                        .HasColumnType("bigint");

                    b.Property<string>("ResourceID")
                        .IsRequired()
                        .HasColumnName("ResourceID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus")
                        .HasColumnType("smallint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("Text")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<short>("Type")
                        .HasColumnName("Type")
                        .HasColumnType("smallint");

                    b.HasKey("CategoryID");

                    b.HasIndex("RowStatus", "ResourceID", "Type", "CategoryID", "HierarchyText")
                        .HasName("v6_dataCategories_index_Search");

                    b.HasIndex("RowStatus", "ResourceID", "Type", "ParentID", "CategoryID", "Text")
                        .HasName("v6_dataCategories_index_Parent");

                    b.ToTable("v6_dataCategories");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Entries.EntryData", b =>
                {
                    b.Property<long>("EntryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("AccountID")
                        .HasColumnType("bigint");

                    b.Property<long?>("CategoryID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("EntryValue")
                        .HasColumnType("decimal(15,2)");

                    b.Property<bool>("Paid")
                        .HasColumnType("bit");

                    b.Property<long?>("PatternID")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("PayDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("RecurrencyID")
                        .HasColumnType("bigint");

                    b.Property<short?>("RecurrencyItem")
                        .HasColumnType("smallint");

                    b.Property<short?>("RecurrencyTotal")
                        .HasColumnType("smallint");

                    b.Property<string>("ResourceID")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("SearchDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Sorting")
                        .HasColumnType("decimal(20,10)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("TransferID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<short>("Type")
                        .HasColumnType("smallint");

                    b.HasKey("EntryID");

                    b.HasIndex("RecurrencyID")
                        .HasName("v6_dataEntries_index_SearchRecurrency");

                    b.HasIndex("RowStatus", "ResourceID", "TransferID")
                        .HasName("v6_dataEntries_index_SearchTransfer");

                    b.HasIndex("RowStatus", "ResourceID", "AccountID", "SearchDate")
                        .HasName("v6_dataEntries_index_SearchDate")
                        .HasAnnotation("SqlServer:Include", new[] { "EntryID", "Type", "TransferID", "EntryValue", "Paid" });

                    b.HasIndex("RowStatus", "ResourceID", "AccountID", "Text")
                        .HasName("v6_dataEntries_index_SearchText");

                    b.ToTable("v6_dataEntries");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Patterns.PatternData", b =>
                {
                    b.Property<long>("PatternID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CategoryID")
                        .HasColumnType("bigint");

                    b.Property<short>("Count")
                        .HasColumnType("smallint");

                    b.Property<string>("ResourceID")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus")
                        .HasColumnType("smallint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<short>("Type")
                        .HasColumnType("smallint");

                    b.HasKey("PatternID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("RowStatus", "ResourceID", "Type", "CategoryID", "Text")
                        .HasName("v6_dataPatterns_index_Search");

                    b.ToTable("v6_dataPatterns");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Recurrencies.RecurrencyData", b =>
                {
                    b.Property<long>("RecurrencyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AccountID")
                        .HasColumnType("bigint");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("EntryValue")
                        .HasColumnType("decimal(15,2)");

                    b.Property<DateTime>("InitialDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("PatternID")
                        .HasColumnType("bigint");

                    b.Property<string>("ResourceID")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus")
                        .HasColumnType("smallint");

                    b.Property<short>("Type")
                        .HasColumnType("smallint");

                    b.HasKey("RecurrencyID");

                    b.HasIndex("RowStatus", "ResourceID", "RecurrencyID")
                        .HasName("v6_dataRecurrencies_index_Search");

                    b.ToTable("v6_dataRecurrencies");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Users.UserData", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus")
                        .HasColumnType("smallint");

                    b.Property<string>("SelectedResourceID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("UserID");

                    b.HasIndex("RowStatus", "UserID")
                        .HasName("v6_identityUsers_index_UserID");

                    b.HasIndex("RowStatus", "UserName", "PasswordHash")
                        .HasName("v6_identityUsers_index_Login");

                    b.ToTable("v6_identityUsers");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Users.UserResourceData", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ResourceID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus")
                        .HasColumnType("smallint");

                    b.HasKey("UserID", "ResourceID");

                    b.HasIndex("RowStatus", "UserID", "ResourceID")
                        .HasName("v6_identityUserResources_index_Search");

                    b.ToTable("v6_identityUserResources");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Users.UserRoleData", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("RoleID")
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("ResourceID")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("RowDate")
                        .HasColumnName("RowDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("RowStatus")
                        .HasColumnName("RowStatus")
                        .HasColumnType("smallint");

                    b.HasKey("UserID", "RoleID");

                    b.HasIndex("RowStatus", "UserID", "ResourceID", "RoleID")
                        .HasName("v6_identityUserRoles_index_Search");

                    b.ToTable("v6_identityUserRoles");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Users.UserTokenData", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("RefreshToken")
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("UserID", "RefreshToken");

                    b.HasIndex("RefreshToken")
                        .HasName("v6_identityUserTokens_index_Search");

                    b.ToTable("v6_identityUserTokens");
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Budget.BudgetData", b =>
                {
                    b.HasOne("FriendlyCashFlow.API.Patterns.PatternData", "PatternDetails")
                        .WithMany()
                        .HasForeignKey("PatternID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FriendlyCashFlow.API.Patterns.PatternData", b =>
                {
                    b.HasOne("FriendlyCashFlow.API.Categories.CategoryData", "CategoryDetails")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
