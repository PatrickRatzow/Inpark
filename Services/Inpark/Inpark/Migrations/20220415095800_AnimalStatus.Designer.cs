﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Zeta.Inpark;
using Zoo.Inpark;

#nullable disable

namespace Zoo.Inpark.Migrations
{
    [DbContext(typeof(InparkDbContext))]
    [Migration("20220415095800_AnimalStatus")]
    partial class AnimalStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Zoo.Inpark.Entities.Animal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("Zoo.Inpark.Entities.OpeningHour", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("Open")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("OpeningHours");
                });

            modelBuilder.Entity("Zoo.Inpark.Entities.Animal", b =>
                {
                    b.OwnsOne("Zoo.Inpark.ValueObjects.AnimalName", "Name", b1 =>
                        {
                            b1.Property<Guid>("AnimalId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("LatinName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("AnimalId");

                            b1.ToTable("Animals");

                            b1.WithOwner()
                                .HasForeignKey("AnimalId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Zoo.Inpark.Entities.OpeningHour", b =>
                {
                    b.OwnsOne("Zoo.Inpark.ValueObjects.TimeRange", "Range", b1 =>
                        {
                            b1.Property<Guid>("OpeningHourId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTimeOffset>("End")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("TimeRange_End");

                            b1.Property<DateTimeOffset>("Start")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("TimeRange_Start");

                            b1.HasKey("OpeningHourId");

                            b1.ToTable("OpeningHours");

                            b1.WithOwner()
                                .HasForeignKey("OpeningHourId");
                        });

                    b.Navigation("Range")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
