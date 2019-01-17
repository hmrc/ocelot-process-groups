﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductGrouping.Models;

namespace ProductGrouping.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20181126124522_inital")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    partial class inital
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProductGrouping.Models.ProductGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ProductOwner")
                        .IsRequired()
                        .HasMaxLength(7);

                    b.Property<string>("ProductReference")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("Site")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("ProductGroups");
                });
#pragma warning restore 612, 618
        }
    }
}
