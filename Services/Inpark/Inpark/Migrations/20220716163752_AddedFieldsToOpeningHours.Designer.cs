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
    [Migration("20220716163752_AddedFieldsToOpeningHours")]
    partial class AddedFieldsToOpeningHours
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

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<int>("Days")
                        .HasColumnType("int");

                    b.Property<string>("Fields")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

            modelBuilder.Entity("Zoo.Inpark.Entities.ParkEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("ParkEvents");
                });

            modelBuilder.Entity("Zoo.Inpark.Entities.Speak", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Speaks");
                });

            modelBuilder.Entity("Zoo.Inpark.Entities.Animal", b =>
                {
                    b.OwnsOne("Zoo.Inpark.ValueObjects.ImagePair", "Image", b1 =>
                        {
                            b1.Property<Guid>("AnimalId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FullscreenUrl")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PreviewUrl")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("AnimalId");

                            b1.ToTable("Animals");

                            b1.WithOwner()
                                .HasForeignKey("AnimalId");
                        });

                    b.OwnsMany("Zoo.Inpark.ValueObjects.AnimalArea", "Areas", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("AnimalId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Color")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Id");

                            b1.HasIndex("AnimalId");

                            b1.ToTable("AnimalArea");

                            b1.WithOwner()
                                .HasForeignKey("AnimalId");

                            b1.OwnsMany("Zoo.Inpark.ValueObjects.Point", "Points", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<Guid>("AnimalAreaId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<double>("X")
                                        .HasColumnType("float");

                                    b2.Property<double>("Y")
                                        .HasColumnType("float");

                                    b2.HasKey("Id");

                                    b2.HasIndex("AnimalAreaId");

                                    b2.ToTable("Point");

                                    b2.WithOwner()
                                        .HasForeignKey("AnimalAreaId");
                                });

                            b1.Navigation("Points");
                        });

                    b.OwnsOne("Zoo.Inpark.ValueObjects.AnimalName", "Name", b1 =>
                        {
                            b1.Property<Guid>("AnimalId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("LatinName")
                                .IsRequired()
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("AnimalId");

                            b1.HasIndex("LatinName");

                            b1.ToTable("Animals");

                            b1.WithOwner()
                                .HasForeignKey("AnimalId");
                        });

                    b.Navigation("Areas");

                    b.Navigation("Image")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Zoo.Inpark.Entities.OpeningHour", b =>
                {
                    b.OwnsOne("Zoo.Inpark.ValueObjects.TimeRange", "Range", b1 =>
                        {
                            b1.Property<Guid>("OpeningHourId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("End")
                                .HasColumnType("datetime2")
                                .HasColumnName("TimeRange_End");

                            b1.Property<DateTime>("Start")
                                .HasColumnType("datetime2")
                                .HasColumnName("TimeRange_Start");

                            b1.HasKey("OpeningHourId");

                            b1.HasIndex("Start", "End");

                            b1.ToTable("OpeningHours");

                            b1.WithOwner()
                                .HasForeignKey("OpeningHourId");
                        });

                    b.Navigation("Range")
                        .IsRequired();
                });

            modelBuilder.Entity("Zoo.Inpark.Entities.ParkEvent", b =>
                {
                    b.OwnsOne("Zoo.Inpark.ValueObjects.ImagePair", "Image", b1 =>
                        {
                            b1.Property<Guid>("ParkEventId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FullscreenUrl")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PreviewUrl")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ParkEventId");

                            b1.ToTable("ParkEvents");

                            b1.WithOwner()
                                .HasForeignKey("ParkEventId");
                        });

                    b.OwnsOne("Zoo.Inpark.ValueObjects.TimeRange", "Range", b1 =>
                        {
                            b1.Property<Guid>("ParkEventId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("End")
                                .HasColumnType("datetime2")
                                .HasColumnName("TimeRange_End");

                            b1.Property<DateTime>("Start")
                                .HasColumnType("datetime2")
                                .HasColumnName("TimeRange_Start");

                            b1.HasKey("ParkEventId");

                            b1.HasIndex("Start", "End");

                            b1.ToTable("ParkEvents");

                            b1.WithOwner()
                                .HasForeignKey("ParkEventId");
                        });

                    b.Navigation("Image")
                        .IsRequired();

                    b.Navigation("Range")
                        .IsRequired();
                });

            modelBuilder.Entity("Zoo.Inpark.Entities.Speak", b =>
                {
                    b.OwnsOne("Zoo.Inpark.ValueObjects.ImagePair", "Image", b1 =>
                        {
                            b1.Property<Guid>("SpeakId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FullscreenUrl")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PreviewUrl")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("SpeakId");

                            b1.ToTable("Speaks");

                            b1.WithOwner()
                                .HasForeignKey("SpeakId");
                        });

                    b.OwnsMany("Zoo.Inpark.ValueObjects.SpeakTime", "SpeakTimes", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Days")
                                .HasColumnType("int");

                            b1.Property<Guid>("SpeakId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Id");

                            b1.HasIndex("SpeakId");

                            b1.ToTable("SpeakTime");

                            b1.WithOwner()
                                .HasForeignKey("SpeakId");

                            b1.OwnsOne("Zoo.Inpark.ValueObjects.TimeRange", "Range", b2 =>
                                {
                                    b2.Property<Guid>("SpeakTimeId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<DateTime>("End")
                                        .HasColumnType("datetime2")
                                        .HasColumnName("TimeRange_End");

                                    b2.Property<DateTime>("Start")
                                        .HasColumnType("datetime2")
                                        .HasColumnName("TimeRange_Start");

                                    b2.HasKey("SpeakTimeId");

                                    b2.HasIndex("Start", "End");

                                    b2.ToTable("SpeakTime");

                                    b2.WithOwner()
                                        .HasForeignKey("SpeakTimeId");
                                });

                            b1.Navigation("Range")
                                .IsRequired();
                        });

                    b.Navigation("Image")
                        .IsRequired();

                    b.Navigation("SpeakTimes");
                });
#pragma warning restore 612, 618
        }
    }
}
