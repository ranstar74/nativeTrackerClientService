using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace nativeTrackerClientService.Entities
{
    public partial class nativeContext : DbContext
    {
        public virtual DbSet<ClientUser> ClientUsers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<Installation> Installations { get; set; }
        public virtual DbSet<InstallationAttachment> InstallationAttachments { get; set; }
        public virtual DbSet<InstallationIssue> InstallationIssues { get; set; }
        public virtual DbSet<IssuePayment> IssuePayments { get; set; }
        public virtual DbSet<IssueStatus> IssueStatuses { get; set; }
        public virtual DbSet<IssueType> IssueTypes { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<StateType> StateTypes { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleState> VehicleStates { get; set; }

        public nativeContext() : base(new DbContextOptions<nativeContext>())
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=\"10.0.7.168, 1414\";Initial Catalog=nativeTrack;Persist Security Info=True;User ID=sa;Password=123;Trust Server Certificate=True;Command Timeout=300");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientUser>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PK_ClientUser_1");

                entity.ToTable("ClientUser", "Client");

                entity.Property(e => e.Login).HasMaxLength(30);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Phone).HasMaxLength(14);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee", "Employee");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Patrynomic).HasMaxLength(30);

                entity.Property(e => e.Photo).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.RoleID)
                    .HasConstraintName("FK_Employee_Role");
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.ToTable("Feature", "Track");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Installation>(entity =>
            {
                entity.HasKey(e => e.IMEI);

                entity.ToTable("Installation", "Track");

                entity.Property(e => e.IMEI).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Installations)
                    .HasForeignKey(d => d.ModelID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Installation_Model");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Installations)
                    .HasForeignKey(d => d.VehicleID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Installation_Vehicle");
            });

            modelBuilder.Entity<InstallationAttachment>(entity =>
            {
                entity.ToTable("InstallationAttachment", "Track");

                entity.Property(e => e.Data).IsRequired();

                entity.HasOne(d => d.IMEINavigation)
                    .WithMany(p => p.InstallationAttachments)
                    .HasForeignKey(d => d.IMEI)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstallationAttachment_Installation");
            });

            modelBuilder.Entity<InstallationIssue>(entity =>
            {
                entity.ToTable("InstallationIssue", "Track");

                entity.Property(e => e.CloseReason).HasMaxLength(1000);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.IssuePayment)
                    .WithMany(p => p.InstallationIssues)
                    .HasForeignKey(d => d.IssuePaymentID)
                    .HasConstraintName("FK_InstallationIssue_IssuePayment");

                entity.HasOne(d => d.IssueType)
                    .WithMany(p => p.InstallationIssues)
                    .HasForeignKey(d => d.IssueTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstallationIssue_IssueType");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.InstallationIssues)
                    .HasForeignKey(d => d.ManagerID)
                    .HasConstraintName("FK_InstallationIssue_Employee");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.InstallationIssues)
                    .HasForeignKey(d => d.ModelID)
                    .HasConstraintName("FK_InstallationIssue_Model");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.InstallationIssues)
                    .HasForeignKey(d => d.StatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstallationIssue_IssueStatus");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.InstallationIssues)
                    .HasForeignKey(d => d.WorkerID)
                    .HasConstraintName("FK_InstallationIssue_Role");
            });

            modelBuilder.Entity<IssuePayment>(entity =>
            {
                entity.ToTable("IssuePayment", "Track");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PayTimestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<IssueStatus>(entity =>
            {
                entity.ToTable("IssueStatus", "Track");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<IssueType>(entity =>
            {
                entity.ToTable("IssueType", "Track");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("Manufacturer", "Track");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.ToTable("Model", "Track");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.ManufacturerID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Model_Manufacturer1");

                entity.HasMany(d => d.Features)
                    .WithMany(p => p.Models)
                    .UsingEntity<Dictionary<string, object>>(
                        "ModelFeature",
                        l => l.HasOne<Feature>().WithMany().HasForeignKey("FeatureID").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ModelFeatures_Feature"),
                        r => r.HasOne<Model>().WithMany().HasForeignKey("ModelID").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ModelFeatures_Model"),
                        j =>
                        {
                            j.HasKey("ModelID", "FeatureID");

                            j.ToTable("ModelFeatures", "Track");
                        });
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.ToTable("Picture", "Track");

                entity.Property(e => e.Data).IsRequired();

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Pictures)
                    .HasForeignKey(d => d.ModelID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Picture_Model");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "Employee");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(16);
            });

            modelBuilder.Entity<StateType>(entity =>
            {
                entity.ToTable("StateType", "Track");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicle", "Vehicle");

                entity.Property(e => e.ClientLogin)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Photo).IsRequired();

                entity.HasOne(d => d.ClientLoginNavigation)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.ClientLogin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehicle_ClientUser1");
            });

            modelBuilder.Entity<VehicleState>(entity =>
            {
                entity.ToTable("VehicleState", "Track");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.IMEINavigation)
                    .WithMany(p => p.VehicleStates)
                    .HasForeignKey(d => d.IMEI)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleState_Installation");

                entity.HasOne(d => d.TrackFeature)
                    .WithMany(p => p.VehicleStates)
                    .HasForeignKey(d => d.TrackFeatureID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleState_Feature");

                entity.HasOne(d => d.TrackFeatureNavigation)
                    .WithMany(p => p.VehicleStates)
                    .HasForeignKey(d => d.TrackFeatureID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleState_StateType");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
