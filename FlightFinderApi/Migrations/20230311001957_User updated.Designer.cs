﻿// <auto-generated />
using System;
using FlightFinderApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlightFinderApi.Migrations
{
    [DbContext(typeof(FlightFinderDbContext))]
    [Migration("20230311001957_User updated")]
    partial class Userupdated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FlightFinderApi.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ItinerariesFlight_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TotalAdultSeats")
                        .HasColumnType("int");

                    b.Property<int>("TotalChildSeats")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItinerariesFlight_Id");

                    b.HasIndex("UsersId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("FlightFinderApi.Models.Itinerary", b =>
                {
                    b.Property<string>("Flight_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ArrivalAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("AvailableSeats")
                        .HasColumnType("int");

                    b.Property<DateTime>("DepartureAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PricesId")
                        .HasColumnType("int");

                    b.Property<string>("RootRoute_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Flight_Id");

                    b.HasIndex("PricesId");

                    b.HasIndex("RootRoute_Id");

                    b.ToTable("Itineraries");
                });

            modelBuilder.Entity("FlightFinderApi.Models.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Adult")
                        .HasColumnType("float");

                    b.Property<double>("Child")
                        .HasColumnType("float");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("FlightFinderApi.Models.Root", b =>
                {
                    b.Property<string>("Route_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArrivalDestination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DepartureDestination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Route_Id");

                    b.ToTable("Roots");
                });

            modelBuilder.Entity("FlightFinderApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<byte[]>("PassHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PassSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FlightFinderApi.Models.Booking", b =>
                {
                    b.HasOne("FlightFinderApi.Models.Itinerary", "Itineraries")
                        .WithMany()
                        .HasForeignKey("ItinerariesFlight_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlightFinderApi.Models.User", "Users")
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Itineraries");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("FlightFinderApi.Models.Itinerary", b =>
                {
                    b.HasOne("FlightFinderApi.Models.Price", "Prices")
                        .WithMany()
                        .HasForeignKey("PricesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlightFinderApi.Models.Root", "Root")
                        .WithMany("Itineraries")
                        .HasForeignKey("RootRoute_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Prices");

                    b.Navigation("Root");
                });

            modelBuilder.Entity("FlightFinderApi.Models.Root", b =>
                {
                    b.Navigation("Itineraries");
                });
#pragma warning restore 612, 618
        }
    }
}
