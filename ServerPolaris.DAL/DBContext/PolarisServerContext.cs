using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ServerPolaris.Entity;

namespace ServerPolaris.DAL.DBContext
{
    public partial class PolarisServerContext : DbContext
    {
        public PolarisServerContext()
        {
        }

        public PolarisServerContext(DbContextOptions<PolarisServerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<DataBase> Databases { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<TipoLog> TipoLogs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("cliente");

                entity.Property(e => e.ClienteId).HasColumnName("cliente_id");

                entity.Property(e => e.ClienteName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cliente_name");
            });

            modelBuilder.Entity<DataBase>(entity =>
            {
                entity.ToTable("data_base");

                entity.Property(e => e.DataBaseId).HasColumnName("data_base_id");

                entity.Property(e => e.ClienteId).HasColumnName("cliente_id");

                entity.Property(e => e.DataBaseInstance)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("data_base_instance");

                entity.Property(e => e.DataBaseName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("data_base_name");

                entity.Property(e => e.DataBasePassword)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("data_base_password");

                entity.Property(e => e.DataBaseUser)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("data_base_user");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Databases)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__data_base__clien__2B3F6F97");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("log");

                entity.Property(e => e.LogId).HasColumnName("log_id");

                entity.Property(e => e.ClienteId).HasColumnName("cliente_id");

                entity.Property(e => e.LogCreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("log_create_date");

                entity.Property(e => e.LogIdTipoLog).HasColumnName("log_id_tipo_log");

                entity.Property(e => e.LogPathFile)
                    .IsUnicode(false)
                    .HasColumnName("log_path_file");

                entity.Property(e => e.LogUpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("log_update_date");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__log__cliente_id__2C3393D0");

                entity.HasOne(d => d.LogIdTipoLogNavigation)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.LogIdTipoLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__log__log_id_tipo__2A4B4B5E");
            });

            modelBuilder.Entity<TipoLog>(entity =>
            {
                entity.ToTable("tipo_log");

                entity.Property(e => e.TipoLogId).HasColumnName("tipo_log_id");

                entity.Property(e => e.TipoLogDescripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("tipo_log_descripcion");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
