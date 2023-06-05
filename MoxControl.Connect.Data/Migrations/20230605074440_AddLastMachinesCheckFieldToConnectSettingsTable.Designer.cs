﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoxControl.Connect.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoxControl.Connect.Data.Migrations
{
    [DbContext(typeof(ConnectDbContext))]
    [Migration("20230605074440_AddLastMachinesCheckFieldToConnectSettingsTable")]
    partial class AddLastMachinesCheckFieldToConnectSettingsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("connect")
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MoxControl.Connect.Models.Entities.ConnectSetting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsMachinesSyncEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsShowSettingsSection")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSystemHasInterface")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastMachinesCheck")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastServersCheck")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("VirtualizationSystem")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ConnectSettings", "connect");
                });

            modelBuilder.Entity("MoxControl.Connect.Models.Entities.ISOImage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("DownloadSuccess")
                        .HasColumnType("boolean");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("StorageMethod")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ISOImages", "connect");
                });

            modelBuilder.Entity("MoxControl.Connect.Models.Entities.Template", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("CPUCores")
                        .HasColumnType("integer");

                    b.Property<int>("CPUSockets")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("HDDSize")
                        .HasColumnType("integer");

                    b.Property<long>("ISOImageId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RAMSize")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ISOImageId");

                    b.ToTable("Templates", "connect");
                });

            modelBuilder.Entity("MoxControl.Connect.Models.Entities.Template", b =>
                {
                    b.HasOne("MoxControl.Connect.Models.Entities.ISOImage", "ISOImage")
                        .WithMany()
                        .HasForeignKey("ISOImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ISOImage");
                });
#pragma warning restore 612, 618
        }
    }
}
