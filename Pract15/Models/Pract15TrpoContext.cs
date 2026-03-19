using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pract15.Models;

public partial class Pract15TrpoContext : DbContext
{
    public Pract15TrpoContext()
    {
    }

    public Pract15TrpoContext(DbContextOptions<Pract15TrpoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Form> Forms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = smh\\POPA; Database = Pract15Trpo; Trusted_Connection = True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Form>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Form__3214EC07B18316D4");

            entity.ToTable("Form");

            entity.Property(e => e.BirthdayYear).HasColumnName("Birthday_Year");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("Last_Name");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
