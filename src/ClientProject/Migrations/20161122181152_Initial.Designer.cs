using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ClientProject.Data;

namespace ClientProject.Migrations
{
    [DbContext(typeof(ClientContext))]
    [Migration("20161122181152_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ClientProject.Models.Company", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("chkCode");

                    b.HasKey("id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("ClientProject.Models.Employee", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("companyid");

                    b.Property<string>("firstName");

                    b.Property<string>("lastName");

                    b.Property<string>("login");

                    b.Property<string>("mail");

                    b.Property<string>("password");

                    b.Property<decimal>("wallet");

                    b.HasKey("id");

                    b.HasIndex("companyid");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ClientProject.Models.Order", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Employeeid");

                    b.Property<DateTime>("dateOfDelivery");

                    b.Property<decimal>("totalAmount");

                    b.HasKey("id");

                    b.HasIndex("Employeeid");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ClientProject.Models.OrderLine", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("quantity");

                    b.Property<int?>("sandwichid");

                    b.HasKey("id");

                    b.HasIndex("sandwichid");

                    b.ToTable("OrderLines");
                });

            modelBuilder.Entity("ClientProject.Models.OrderLineVegetable", b =>
                {
                    b.Property<int>("orderLineId");

                    b.Property<int>("vegetableId");

                    b.HasKey("orderLineId", "vegetableId");

                    b.HasIndex("orderLineId");

                    b.HasIndex("vegetableId");

                    b.ToTable("OrderLineVegetables");
                });

            modelBuilder.Entity("ClientProject.Models.Sandwich", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("id");

                    b.ToTable("Sandwiches");
                });

            modelBuilder.Entity("ClientProject.Models.Vegetable", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("id");

                    b.ToTable("Vegetables");
                });

            modelBuilder.Entity("ClientProject.Models.Employee", b =>
                {
                    b.HasOne("ClientProject.Models.Company", "company")
                        .WithMany()
                        .HasForeignKey("companyid");
                });

            modelBuilder.Entity("ClientProject.Models.Order", b =>
                {
                    b.HasOne("ClientProject.Models.Employee")
                        .WithMany("orders")
                        .HasForeignKey("Employeeid");
                });

            modelBuilder.Entity("ClientProject.Models.OrderLine", b =>
                {
                    b.HasOne("ClientProject.Models.Sandwich", "sandwich")
                        .WithMany()
                        .HasForeignKey("sandwichid");
                });

            modelBuilder.Entity("ClientProject.Models.OrderLineVegetable", b =>
                {
                    b.HasOne("ClientProject.Models.OrderLine", "orderLine")
                        .WithMany("orderLineVegetables")
                        .HasForeignKey("orderLineId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ClientProject.Models.Vegetable", "vegetable")
                        .WithMany()
                        .HasForeignKey("vegetableId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
