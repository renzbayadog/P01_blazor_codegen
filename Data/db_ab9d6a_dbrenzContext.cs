using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RenzGrandWeddingblazor.ph.Data.Entities;

public class db_ab9d6a_dbrenzContext : DbContext
{
    public db_ab9d6a_dbrenzContext(DbContextOptions<db_ab9d6a_dbrenzContext> options)
        : base(options)
    {

        Database.SetCommandTimeout(150000);
    }

    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<Pullout> Pullouts { get; set; }
    public DbSet<Sales> Sales { get; set; }
    public DbSet<ProductLine> ProductLines { get; set; }
    public DbSet<Particular> Particulars { get; set; }
    public DbSet<Title> TItles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Delivery>(entity => {
            entity.ToTable("Delivery");
        });

        builder.Entity<Pullout>(entity => {
            entity.ToTable("Pullout");
        });

        builder.Entity<Sales>(entity => {
            entity.ToTable("Sales");
        });
        builder.Entity<ProductLine>(entity => {
            entity.ToTable("ProductLine");
            entity.HasKey(e => new { e.ProductLineId });
            entity.Property(e => e.ProductLineCode).HasColumnType("varchar(50)");
            entity.Property(e => e.ProductLineName).HasColumnType("varchar(50)");
        });

        builder.Entity<Title>(entity => {
            entity.ToTable("TItle");
        });
        builder.Entity<Particular>(entity => {
            entity.ToTable("Particular");
        });

        builder.Seed();
    }
}