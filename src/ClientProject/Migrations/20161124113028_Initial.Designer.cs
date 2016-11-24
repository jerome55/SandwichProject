using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ClientProject.Data;

namespace ClientProject.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161124113028_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ClientProject.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int>("employeeId");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("employeeId")
                        .IsUnique();

                    b.ToTable("AspNetUsers");
                });

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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ClientProject.Models.ApplicationUser", b =>
                {
                    b.HasOne("ClientProject.Models.Employee", "employee")
                        .WithOne("user")
                        .HasForeignKey("ClientProject.Models.ApplicationUser", "employeeId")
                        .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ClientProject.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ClientProject.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ClientProject.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
