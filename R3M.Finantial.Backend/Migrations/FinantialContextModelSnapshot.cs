﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using R3M.Finantial.Backend.Context;

#nullable disable

namespace R3M.Finantial.Backend.Migrations
{
    [DbContext(typeof(FinantialContext))]
    partial class FinantialContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("R3M.Finantial.Backend.Model.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("InsertedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("inserted_at_utc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("name");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at_utc");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.HasIndex("ParentId", "Name")
                        .IsUnique()
                        .HasDatabaseName("ix_categories_parent_id_name");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("R3M.Finantial.Backend.Model.Institution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric")
                        .HasColumnName("balance");

                    b.Property<decimal>("InitialBalance")
                        .HasColumnType("numeric")
                        .HasColumnName("initial_balance");

                    b.Property<DateOnly>("InitialBalanceDate")
                        .HasColumnType("date")
                        .HasColumnName("initial_balance_date");

                    b.Property<DateTime>("InsertedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("inserted_at_utc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at_utc");

                    b.HasKey("Id")
                        .HasName("pk_institutions");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_institutions_name");

                    b.ToTable("institutions", (string)null);
                });

            modelBuilder.Entity("R3M.Finantial.Backend.Model.Movimentation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("description");

                    b.Property<DateTime>("InsertedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("inserted_at_utc");

                    b.Property<Guid>("InstitutionId")
                        .HasColumnType("uuid")
                        .HasColumnName("institution_id");

                    b.Property<Guid>("PeriodId")
                        .HasColumnType("uuid")
                        .HasColumnName("period_id");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at_utc");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_movimentations");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_movimentations_category_id");

                    b.HasIndex("InstitutionId")
                        .HasDatabaseName("ix_movimentations_institution_id");

                    b.HasIndex("PeriodId")
                        .HasDatabaseName("ix_movimentations_period_id");

                    b.ToTable("movimentations", (string)null);
                });

            modelBuilder.Entity("R3M.Finantial.Backend.Model.Period", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character(6)")
                        .HasColumnName("description")
                        .IsFixedLength();

                    b.Property<DateOnly>("FinalDate")
                        .HasColumnType("date")
                        .HasColumnName("final_date");

                    b.Property<DateOnly>("InitialDate")
                        .HasColumnType("date")
                        .HasColumnName("initial_date");

                    b.Property<DateTime>("InsertedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("inserted_at_utc");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at_utc");

                    b.HasKey("Id")
                        .HasName("pk_periods");

                    b.HasIndex("Description")
                        .IsUnique()
                        .HasDatabaseName("ix_periods_description");

                    b.HasIndex("InitialDate", "FinalDate")
                        .HasDatabaseName("ix_periods_initial_date_final_date");

                    b.ToTable("periods", (string)null);
                });

            modelBuilder.Entity("R3M.Finantial.Backend.Model.Category", b =>
                {
                    b.HasOne("R3M.Finantial.Backend.Model.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_categories_categories_parent_id");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("R3M.Finantial.Backend.Model.Movimentation", b =>
                {
                    b.HasOne("R3M.Finantial.Backend.Model.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_movimentations_categories_category_id");

                    b.HasOne("R3M.Finantial.Backend.Model.Institution", "Institution")
                        .WithMany()
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_movimentations_institutions_institution_id");

                    b.HasOne("R3M.Finantial.Backend.Model.Period", "Period")
                        .WithMany()
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_movimentations_periods_period_id");

                    b.Navigation("Category");

                    b.Navigation("Institution");

                    b.Navigation("Period");
                });

            modelBuilder.Entity("R3M.Finantial.Backend.Model.Category", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}