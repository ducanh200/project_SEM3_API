using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SEM3_API.Entities;

public partial class Sem3ApiContext : DbContext
{
    public Sem3ApiContext()
    {
    }

    public Sem3ApiContext(DbContextOptions<Sem3ApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Donate> Donates { get; set; }

    public virtual DbSet<DonateDetail> DonateDetails { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=SEM3_API;Integrated Security=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__countrie__3213E83FE1E1E245");

            entity.ToTable("countries");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Donate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__donates__3213E83FDD6F68D8");

            entity.ToTable("donates");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(14, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Project).WithMany(p => p.Donates)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__donates__project__3864608B");

            entity.HasOne(d => d.User).WithMany(p => p.Donates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__donates__user_id__395884C4");
        });

        modelBuilder.Entity<DonateDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("donate_details");

            entity.Property(e => e.Amount)
                .HasColumnType("decimal(14, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.DonateId).HasColumnName("donate_id");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");

            entity.HasOne(d => d.Donate).WithMany()
                .HasForeignKey(d => d.DonateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__donate_de__donat__367C1819");

            entity.HasOne(d => d.Project).WithMany()
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__donate_de__proje__37703C52");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__feedback__3213E83FA98279A1");

            entity.ToTable("feedback");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Project).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__feedback__projec__339FAB6E");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__feedback__user_i__32AB8735");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__news__3213E83F81FC7381");

            entity.ToTable("news");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("thumbnail");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");

            entity.HasOne(d => d.Country).WithMany(p => p.News)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__news__country_id__3493CFA7");

            entity.HasOne(d => d.Topic).WithMany(p => p.News)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__news__topic_id__3587F3E0");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__projects__3213E83F8F2C1CFA");

            entity.ToTable("projects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Begin)
                .HasColumnType("datetime")
                .HasColumnName("begin");
            entity.Property(e => e.City)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Finish)
                .HasColumnType("datetime")
                .HasColumnName("finish");
            entity.Property(e => e.Fund)
                .HasColumnType("decimal(14, 2)")
                .HasColumnName("fund");
            entity.Property(e => e.Name)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Thumbnail1)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("thumbnail_1");
            entity.Property(e => e.Thumbnail2)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("thumbnail_2");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");

            entity.HasOne(d => d.Country).WithMany(p => p.Projects)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__projects__countr__30C33EC3");

            entity.HasOne(d => d.Topic).WithMany(p => p.Projects)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__projects__topic___31B762FC");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__topics__3213E83FE3AEF140");

            entity.ToTable("topics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F9A63EB9A");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
