﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoapSimulator.Core.Models;

#nullable disable

namespace SoapSimulator.Core.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.12");

            modelBuilder.Entity("SoapSimulator.Core.Models.SoapAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("MethodName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("SystemConfigurationId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MethodName")
                        .IsUnique();

                    b.HasIndex("SystemConfigurationId");

                    b.ToTable("SoapActions", (string)null);
                });

            modelBuilder.Entity("SoapSimulator.Core.Models.SystemConfiguration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCurrent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SystemConfigurations", (string)null);
                });

            modelBuilder.Entity("SoapSimulator.Core.Models.SoapAction", b =>
                {
                    b.HasOne("SoapSimulator.Core.Models.SystemConfiguration", "SystemConfiguration")
                        .WithMany("Actions")
                        .HasForeignKey("SystemConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("SoapSimulator.Core.Models.SoapAction.Request#SoapSimulator.Core.Models.RequestFormat", "Request", b1 =>
                        {
                            b1.Property<Guid>("ActionId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Body")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("DateCreated")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT");

                            b1.Property<string>("XMLPath")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("ActionId");

                            b1.ToTable("RequestFormats", (string)null);

                            b1.WithOwner("Action")
                                .HasForeignKey("ActionId");

                            b1.Navigation("Action");
                        });

                    b.OwnsOne("SoapSimulator.Core.Models.SoapAction.Response#SoapSimulator.Core.Models.ResponseFormat", "Response", b1 =>
                        {
                            b1.Property<Guid>("ActionId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Body")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("DateCreated")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT");

                            b1.Property<string>("XMLPath")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("ActionId");

                            b1.ToTable("ResponseFormats", (string)null);

                            b1.WithOwner("Action")
                                .HasForeignKey("ActionId");

                            b1.Navigation("Action");
                        });

                    b.Navigation("Request")
                        .IsRequired();

                    b.Navigation("Response")
                        .IsRequired();

                    b.Navigation("SystemConfiguration");
                });

            modelBuilder.Entity("SoapSimulator.Core.Models.SystemConfiguration", b =>
                {
                    b.Navigation("Actions");
                });
#pragma warning restore 612, 618
        }
    }
}
