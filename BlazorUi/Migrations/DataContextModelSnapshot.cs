﻿// <auto-generated />
using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlazorUi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DataLayer.Models.Instruments.DbFuture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ConId")
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Echange")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InstumentType")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastTradeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LocalSymbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MarketRule")
                        .HasColumnType("int");

                    b.Property<decimal>("MinTick")
                        .HasColumnType("decimal(18,10)");

                    b.Property<int>("Multiplier")
                        .HasColumnType("int");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Futures");
                });

            modelBuilder.Entity("DataLayer.Models.Instruments.DbOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ConId")
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Echange")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FutureId")
                        .HasColumnType("int");

                    b.Property<int>("InstumentType")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastTradeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LocalSymbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MarketRule")
                        .HasColumnType("int");

                    b.Property<decimal>("MinTick")
                        .HasColumnType("decimal(18,10)");

                    b.Property<int>("Multiplier")
                        .HasColumnType("int");

                    b.Property<int>("OptionType")
                        .HasColumnType("int");

                    b.Property<decimal>("Strike")
                        .HasColumnType("decimal(18,10)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TradingClass")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnderlyingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FutureId");

                    b.ToTable("Options");
                });

            modelBuilder.Entity("DataLayer.Models.LongStraddle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ContainerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Strike")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ContainerId");

                    b.ToTable("Straddles");
                });

            modelBuilder.Entity("DataLayer.Models.Strategies.Container", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FutureId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastTradeDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FutureId");

                    b.ToTable("Containers");
                });

            modelBuilder.Entity("DataLayer.Models.Strategies.DbOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Account")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("AvgFilledPrice")
                        .HasColumnType("decimal(18,10)");

                    b.Property<decimal>("Commission")
                        .HasColumnType("decimal(18,10)");

                    b.Property<int>("Direction")
                        .HasColumnType("int");

                    b.Property<int>("FilledQuantity")
                        .HasColumnType("int");

                    b.Property<decimal>("LmtPrice")
                        .HasColumnType("decimal(18,10)");

                    b.Property<int?>("OptionStrategyId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("OrderType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalQuantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OptionStrategyId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DataLayer.Models.Strategies.OptionStrategy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Direction")
                        .HasColumnType("int");

                    b.Property<int?>("LongStraddleId")
                        .HasColumnType("int");

                    b.Property<int?>("OptionId")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int>("StrategyLogic")
                        .HasColumnType("int");

                    b.Property<int>("Volume")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LongStraddleId");

                    b.HasIndex("OptionId");

                    b.ToTable("OptionStrategies");
                });

            modelBuilder.Entity("DataLayer.Models.Instruments.DbOption", b =>
                {
                    b.HasOne("DataLayer.Models.Instruments.DbFuture", "Future")
                        .WithMany("Options")
                        .HasForeignKey("FutureId");

                    b.Navigation("Future");
                });

            modelBuilder.Entity("DataLayer.Models.LongStraddle", b =>
                {
                    b.HasOne("DataLayer.Models.Strategies.Container", "Container")
                        .WithMany("LongStraddles")
                        .HasForeignKey("ContainerId");

                    b.Navigation("Container");
                });

            modelBuilder.Entity("DataLayer.Models.Strategies.Container", b =>
                {
                    b.HasOne("DataLayer.Models.Instruments.DbFuture", "Future")
                        .WithMany("Containers")
                        .HasForeignKey("FutureId");

                    b.Navigation("Future");
                });

            modelBuilder.Entity("DataLayer.Models.Strategies.DbOrder", b =>
                {
                    b.HasOne("DataLayer.Models.Strategies.OptionStrategy", "OptionStrategy")
                        .WithMany("StrategyOrders")
                        .HasForeignKey("OptionStrategyId");

                    b.Navigation("OptionStrategy");
                });

            modelBuilder.Entity("DataLayer.Models.Strategies.OptionStrategy", b =>
                {
                    b.HasOne("DataLayer.Models.LongStraddle", "LongStraddle")
                        .WithMany("OptionStrategies")
                        .HasForeignKey("LongStraddleId");

                    b.HasOne("DataLayer.Models.Instruments.DbOption", "Option")
                        .WithMany()
                        .HasForeignKey("OptionId");

                    b.Navigation("LongStraddle");

                    b.Navigation("Option");
                });

            modelBuilder.Entity("DataLayer.Models.Instruments.DbFuture", b =>
                {
                    b.Navigation("Containers");

                    b.Navigation("Options");
                });

            modelBuilder.Entity("DataLayer.Models.LongStraddle", b =>
                {
                    b.Navigation("OptionStrategies");
                });

            modelBuilder.Entity("DataLayer.Models.Strategies.Container", b =>
                {
                    b.Navigation("LongStraddles");
                });

            modelBuilder.Entity("DataLayer.Models.Strategies.OptionStrategy", b =>
                {
                    b.Navigation("StrategyOrders");
                });
#pragma warning restore 612, 618
        }
    }
}
