using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Infrastructure.Data;

public partial class TalentoInternooContext : DbContext
{
    public TalentoInternooContext()
    {
    }

    public TalentoInternooContext(DbContextOptions<TalentoInternooContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Area { get; set; }

    public virtual DbSet<Certificacion> Certificacion { get; set; }

    public virtual DbSet<Colaborador> Colaborador { get; set; }

    public virtual DbSet<ColaboradorCertificacion> ColaboradorCertificacion { get; set; }

    public virtual DbSet<ColaboradorSkill> ColaboradorSkill { get; set; }

    public virtual DbSet<Cuenta> Cuenta { get; set; }

    public virtual DbSet<Departamento> Departamento { get; set; }

    public virtual DbSet<NivelDominio> NivelDominio { get; set; }

    public virtual DbSet<Perfil> Perfil { get; set; }

    public virtual DbSet<Proyecto> Proyecto { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Skill> Skill { get; set; }

    public virtual DbSet<TipoSkill> TipoSkill { get; set; }

    public virtual DbSet<Urgencia> Urgencia { get; set; }

    public virtual DbSet<Vacante> Vacante { get; set; }

    public virtual DbSet<VacanteSkillReq> VacanteSkillReq { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__Area__70B820484A7EC8EA");

            entity.HasIndex(e => e.Nombre, "UQ__Area__75E3EFCF13FA6948").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Certificacion>(entity =>
        {
            entity.HasKey(e => e.CertificacionId).HasName("PK__Certific__398F23C4C266221D");

            entity.HasIndex(e => e.Nombre, "UQ__Certific__75E3EFCF7945A255").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Colaborador>(entity =>
        {
            entity.HasKey(e => e.ColaboradorId).HasName("PK__Colabora__28AA7221DFC2B142");

            entity.HasIndex(e => new { e.AreaId, e.DepartamentoId }, "IX_Colab_AreaDept");

            entity.HasIndex(e => e.RolId, "IX_Colab_Rol");

            entity.HasIndex(e => e.Email, "UQ__Colabora__A9D10534299B2974").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellidos)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.DisponibleMovilidad).HasDefaultValue(false);
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaAlta).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.Colaborador)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Colab_Area");

            entity.HasOne(d => d.Departamento).WithMany(p => p.Colaborador)
                .HasForeignKey(d => d.DepartamentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Colab_Dept");

            entity.HasOne(d => d.Rol).WithMany(p => p.Colaborador)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Colab_Rol");
        });

        modelBuilder.Entity<ColaboradorCertificacion>(entity =>
        {
            entity.HasKey(e => new { e.ColaboradorId, e.CertificacionId }).HasName("PK__Colabora__7B32801D3E4DFA31");

            entity.HasOne(d => d.Certificacion).WithMany(p => p.ColaboradorCertificacion)
                .HasForeignKey(d => d.CertificacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CC_Cert");

            entity.HasOne(d => d.Colaborador).WithMany(p => p.ColaboradorCertificacion)
                .HasForeignKey(d => d.ColaboradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CC_Colab");
        });

        modelBuilder.Entity<ColaboradorSkill>(entity =>
        {
            entity.HasKey(e => new { e.ColaboradorId, e.SkillId }).HasName("PK__Colabora__45507B3924BBCB6A");

            entity.Property(e => e.AniosExp)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(4, 1)");

            entity.HasOne(d => d.Colaborador).WithMany(p => p.ColaboradorSkill)
                .HasForeignKey(d => d.ColaboradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CS_Colab");

            entity.HasOne(d => d.Nivel).WithMany(p => p.ColaboradorSkill)
                .HasForeignKey(d => d.NivelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CS_Nivel");

            entity.HasOne(d => d.Skill).WithMany(p => p.ColaboradorSkill)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CS_Skill");
        });

        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.HasKey(e => e.CuentaId).HasName("PK__Cuenta__40072E8101941ED2");

            entity.HasIndex(e => e.Nombre, "UQ__Cuenta__75E3EFCF34006136").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.DepartamentoId).HasName("PK__Departam__66BB0E3E5A57911E");

            entity.HasIndex(e => e.Nombre, "UQ__Departam__75E3EFCF4457C88F").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NivelDominio>(entity =>
        {
            entity.HasKey(e => e.NivelId).HasName("PK__NivelDom__316FA277D8874FCF");

            entity.HasIndex(e => e.Nombre, "UQ__NivelDom__75E3EFCF7EC00AEE").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.PerfilId).HasName("PK__Perfil__0C005B06D7B378BD");

            entity.HasIndex(e => e.Nombre, "UQ__Perfil__75E3EFCF4A50E1A0").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(e => e.ProyectoId).HasName("PK__Proyecto__CF241D6598F47118");

            entity.HasIndex(e => e.Nombre, "UQ__Proyecto__75E3EFCFC95BB740").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Cuenta).WithMany(p => p.Proyecto)
                .HasForeignKey(d => d.CuentaId)
                .HasConstraintName("FK_Proyecto_Cuenta");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Rol__F92302F16C5CF8ED");

            entity.HasIndex(e => e.Nombre, "UQ__Rol__75E3EFCF9CBBCE51").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skill__DFA09187732FABE9");

            entity.HasIndex(e => e.Nombre, "UQ_Skill").IsUnique();

            entity.Property(e => e.Critico).HasDefaultValue(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.TipoSkill).WithMany(p => p.Skill)
                .HasForeignKey(d => d.TipoSkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Skill_Tipo");
        });

        modelBuilder.Entity<TipoSkill>(entity =>
        {
            entity.HasKey(e => e.TipoSkillId).HasName("PK__TipoSkil__5AF856A5014A6FA7");

            entity.HasIndex(e => e.Nombre, "UQ__TipoSkil__75E3EFCF712C660D").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Urgencia>(entity =>
        {
            entity.HasKey(e => e.UrgenciaId).HasName("PK__Urgencia__FB1E235EAF6AB842");

            entity.HasIndex(e => e.Nombre, "UQ__Urgencia__75E3EFCF9069094A").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Vacante>(entity =>
        {
            entity.HasKey(e => e.VacanteId).HasName("PK__Vacante__4CFFE2696A0AC98C");

            entity.HasIndex(e => new { e.CuentaId, e.ProyectoId }, "IX_Vacante_CuentaProyecto");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Abierta");
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UrgenciaId).HasDefaultValue((byte)2);

            entity.HasOne(d => d.Cuenta).WithMany(p => p.Vacante)
                .HasForeignKey(d => d.CuentaId)
                .HasConstraintName("FK_Vacante_Cuenta");

            entity.HasOne(d => d.Perfil).WithMany(p => p.Vacante)
                .HasForeignKey(d => d.PerfilId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vacante_Perfil");

            entity.HasOne(d => d.Proyecto).WithMany(p => p.Vacante)
                .HasForeignKey(d => d.ProyectoId)
                .HasConstraintName("FK_Vacante_Proyecto");

            entity.HasOne(d => d.Urgencia).WithMany(p => p.Vacante)
                .HasForeignKey(d => d.UrgenciaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vacante_Urg");
        });

        modelBuilder.Entity<VacanteSkillReq>(entity =>
        {
            entity.HasKey(e => new { e.VacanteId, e.SkillId }).HasName("PK__VacanteS__2105EB71D41B6183");

            entity.Property(e => e.Critico).HasDefaultValue(false);
            entity.Property(e => e.Peso)
                .HasDefaultValue(1.0m)
                .HasColumnType("decimal(4, 2)");

            entity.HasOne(d => d.NivelDeseadoNavigation).WithMany(p => p.VacanteSkillReq)
                .HasForeignKey(d => d.NivelDeseado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VSR_Nivel");

            entity.HasOne(d => d.Skill).WithMany(p => p.VacanteSkillReq)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VSR_Skill");

            entity.HasOne(d => d.Vacante).WithMany(p => p.VacanteSkillReq)
                .HasForeignKey(d => d.VacanteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VSR_Vac");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
