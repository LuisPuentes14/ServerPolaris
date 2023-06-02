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
                entity.HasKey(e => e.ClienteId).HasName("PK__cliente__47E34D64B7032584");

                entity.ToTable("cliente");

                entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
                entity.Property(e => e.ClienteName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cliente_name");
            });

            modelBuilder.Entity<DataBase>(entity =>
            {
                entity.HasKey(e => e.DataBaseId).HasName("PK__data_bas__92AA52EC9424694C");

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

                entity.HasOne(d => d.Cliente).WithMany(p => p.Databases)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__data_base__clien__2B3F6F97");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.LogId).HasName("PK__log__9E2397E01866E5BF");

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

                entity.HasOne(d => d.Cliente).WithMany(p => p.Logs)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__log__cliente_id__2C3393D0");

                entity.HasOne(d => d.LogIdTipoLogNavigation).WithMany(p => p.Logs)
                    .HasForeignKey(d => d.LogIdTipoLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__log__log_id_tipo__2A4B4B5E");
            });

            modelBuilder.Entity<ModulosWeb>(entity =>
            {
                entity.HasKey(e => e.ModId).HasName("PK__modulos___65659BEEF1E6DD71");

                entity.ToTable("modulos_web");

                entity.Property(e => e.ModId).HasColumnName("mod_id");
                entity.Property(e => e.IdTipoModulo).HasColumnName("id_tipo_modulo");
                entity.Property(e => e.ModIdHijo).HasColumnName("mod_id_hijo");
                entity.Property(e => e.ModIdPadre).HasColumnName("mod_id_padre");
                entity.Property(e => e.ModNombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mod_Nombre");
                entity.Property(e => e.ModUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mod_Url");

                entity.HasOne(d => d.IdTipoModuloNavigation).WithMany(p => p.ModulosWebs)
                    .HasForeignKey(d => d.IdTipoModulo)
                    .HasConstraintName("FK__modulos_w__id_ti__44FF419A");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.PerfilId).HasName("PK__perfil__638DD32CC62EFABA");

                entity.ToTable("perfil");

                entity.Property(e => e.PerfilId).HasColumnName("perfil_id");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<PerfilUsuario>(entity =>
            {
                entity.HasKey(e => e.PerfilUsuarioId).HasName("PK__perfil_u__75D74D38B7C136BC");

                entity.ToTable("perfil_usuario");

                entity.Property(e => e.PerfilUsuarioId).HasColumnName("perfil_usuario_id");
                entity.Property(e => e.PerfilId).HasColumnName("perfil_id");
                entity.Property(e => e.UsuId).HasColumnName("usu_id");

                entity.HasOne(d => d.Perfil).WithMany(p => p.PerfilUsuarios)
                    .HasForeignKey(d => d.PerfilId)
                    .HasConstraintName("FK__perfil_us__perfi__4316F928");

                entity.HasOne(d => d.Usu).WithMany(p => p.PerfilUsuarios)
                    .HasForeignKey(d => d.UsuId)
                    .HasConstraintName("FK__perfil_us__usu_i__4222D4EF");
            });

            modelBuilder.Entity<PermisosPerfilModulo>(entity =>
            {
                entity.HasKey(e => e.PerId).HasName("PK__permisos__32A15E67D3CB2F57");

                entity.ToTable("permisos_perfil_modulos");

                entity.Property(e => e.PerId).HasColumnName("per_id");
                entity.Property(e => e.ModId).HasColumnName("mod_id");
                entity.Property(e => e.PerAcceder).HasColumnName("per_acceder");
                entity.Property(e => e.PerActualizar).HasColumnName("per_actualizar");
                entity.Property(e => e.PerEliminar).HasColumnName("per_eliminar");
                entity.Property(e => e.PerInsertar).HasColumnName("per_Insertar");
                entity.Property(e => e.PerfilId).HasColumnName("perfil_id");

                entity.HasOne(d => d.Mod).WithMany(p => p.PermisosPerfilModulos)
                    .HasForeignKey(d => d.ModId)
                    .HasConstraintName("FK__permisos___mod_i__46E78A0C");

                entity.HasOne(d => d.Perfil).WithMany(p => p.PermisosPerfilModulos)
                    .HasForeignKey(d => d.PerfilId)
                    .HasConstraintName("FK__permisos___perfi__45F365D3");
            });

            modelBuilder.Entity<TipoEstadoUsuario>(entity =>
            {
                entity.HasKey(e => e.EstadoId).HasName("PK__tipo_est__053774EF8EDD3081");

                entity.ToTable("tipo_estado_usuario");

                entity.Property(e => e.EstadoId).HasColumnName("estado_id");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<TipoLog>(entity =>
            {
                entity.HasKey(e => e.TipoLogId).HasName("PK__tipo_log__751B3FD3F0565CE7");

                entity.ToTable("tipo_log");

                entity.Property(e => e.TipoLogId).HasColumnName("tipo_log_id");
                entity.Property(e => e.TipoLogDescripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("tipo_log_descripcion");
            });

            modelBuilder.Entity<TipoModulo>(entity =>
            {
                entity.HasKey(e => e.IdTipoModulo).HasName("PK__tipo_mod__5DC8AF46FB447BC8");

                entity.ToTable("tipo_modulo");

                entity.Property(e => e.IdTipoModulo).HasColumnName("id_tipo_modulo");
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.UsuId).HasName("PK__usuario__430A673CA9E14534");

                entity.ToTable("usuario");

                entity.Property(e => e.UsuId).HasColumnName("usu_id");
                entity.Property(e => e.EstadoId).HasColumnName("estado_id");
                entity.Property(e => e.UsuEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("usu_email");
                entity.Property(e => e.UsuFechaActualizacion)
                    .HasColumnType("datetime")
                    .HasColumnName("usu_fecha_actualizacion");
                entity.Property(e => e.UsuFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("usu_fecha_creacion");
                entity.Property(e => e.UsuFechaExpPassword)
                    .HasColumnType("datetime")
                    .HasColumnName("usu_fecha_exp_password");
                entity.Property(e => e.UsuLogin)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("usu_login");
                entity.Property(e => e.UsuNombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("usu_nombre");
                entity.Property(e => e.UsuPassword)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("usu_password");

                entity.HasOne(d => d.Estado).WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.EstadoId)
                    .HasConstraintName("FK__usuario__estado___440B1D61");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
