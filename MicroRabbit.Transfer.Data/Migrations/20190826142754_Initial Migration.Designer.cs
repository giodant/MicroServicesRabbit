﻿// <auto-generated />
using MicroRabbit.Transfer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MicroRabbit.Transfer.Data.Migrations
{
    [DbContext(typeof(TransferDbContext))]
    [Migration("20190826142754_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("MicroRabbit.Transfer.Domain.Models.TransferLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FromAccount");

                    b.Property<int>("ToAccount");

                    b.Property<decimal>("TransferAmount");

                    b.HasKey("Id");

                    b.ToTable("TransferLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
