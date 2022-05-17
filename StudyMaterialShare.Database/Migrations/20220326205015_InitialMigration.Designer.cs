﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudyMaterialShare.Database;

#nullable disable

namespace StudyMaterialShare.Database.Migrations
{
    [DbContext(typeof(StudyMaterialShareDbContext))]
    [Migration("20220326205015_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("StudyMaterialShare.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("RateValue")
                        .HasColumnType("int");

                    b.Property<int>("StudyMaterialId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudyMaterialId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("StudyMaterialShare.Models.StudyMaterial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Downloads")
                        .HasColumnType("int");

                    b.Property<byte[]>("File")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("StudyMaterials");
                });

            modelBuilder.Entity("StudyMaterialShare.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("StudyMaterialShare.Models.Rating", b =>
                {
                    b.HasOne("StudyMaterialShare.Models.StudyMaterial", "StudyMaterial")
                        .WithMany("Ratings")
                        .HasForeignKey("StudyMaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StudyMaterial");
                });

            modelBuilder.Entity("StudyMaterialShare.Models.StudyMaterial", b =>
                {
                    b.HasOne("StudyMaterialShare.Models.Subject", "Subject")
                        .WithMany("StudyMaterials")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("StudyMaterialShare.Models.StudyMaterial", b =>
                {
                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("StudyMaterialShare.Models.Subject", b =>
                {
                    b.Navigation("StudyMaterials");
                });
#pragma warning restore 612, 618
        }
    }
}