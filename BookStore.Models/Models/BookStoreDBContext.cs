using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models.Models;

public partial class BookStoreDBContext : DbContext
{
    public BookStoreDBContext()
    {
    }

    public BookStoreDBContext(DbContextOptions<BookStoreDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.BookCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BookPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BookTitle).HasMaxLength(100);
            entity.Property(e => e.InsertedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Books_Categories");

            entity.HasOne(d => d.InsertedByNavigation).WithMany(p => p.BookInsertedByNavigations)
                .HasForeignKey(d => d.InsertedBy)
                .HasConstraintName("FK_Books_Users");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BookUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_Books_Users1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.InsertedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Purchases_Users");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.Property(e => e.BookPurchasedPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.InsertedDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_PurchaseDetails_Books");

            entity.HasOne(d => d.InsertedByNavigation).WithMany(p => p.PurchaseDetailInsertedByNavigations)
                .HasForeignKey(d => d.InsertedBy)
                .HasConstraintName("FK_PurchaseDetails_Users");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("FK_PurchaseDetails_Purchases");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PurchaseDetailUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_PurchaseDetails_Users1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_Role");

            entity.Property(e => e.RoleDescription).HasMaxLength(100);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
