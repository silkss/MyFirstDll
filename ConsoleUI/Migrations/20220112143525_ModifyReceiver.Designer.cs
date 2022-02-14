﻿// <auto-generated />
using System;
using ConsoleUI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConsoleUI.Migrations
{
    [DbContext(typeof(ReceiversContext))]
    [Migration("20220112143525_ModifyReceiver")]
    partial class ModifyReceiver
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("ConsoleUI.Models.Receiver", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Added")
                        .HasColumnType("TEXT");

                    b.Property<long>("ChatId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Receivers");
                });
#pragma warning restore 612, 618
        }
    }
}