﻿// <auto-generated />
using System;
using ELMS.WEB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ELMS.WEB.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20201213150515_Initial-Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("ELMS.WEB.Entities.Equipment.EquipmentEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("AmendedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("PurchasePrice")
                        .HasColumnType("float");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("WarrantyExpirationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Equipment");
                });
#pragma warning restore 612, 618
        }
    }
}
