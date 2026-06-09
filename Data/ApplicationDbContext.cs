using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Models;

namespace WorldCupStickers.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pais> Paises => Set<Pais>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Jugador> Jugadores => Set<Jugador>();
    public DbSet<Cromo> Cromos => Set<Cromo>();
    public DbSet<Album> Albumes => Set<Album>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<UsuarioCromo> UsuarioCromos => Set<UsuarioCromo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Clave compuesta para la relación N:M Usuario-Cromo
        modelBuilder.Entity<UsuarioCromo>()
            .HasKey(uc => new { uc.UsuarioId, uc.CromoId });

        // Número de cromo único
        modelBuilder.Entity<Cromo>()
            .HasIndex(c => c.NumeroCromo)
            .IsUnique();

        // Código FIFA único
        modelBuilder.Entity<Pais>()
            .HasIndex(p => p.CodigoFifa)
            .IsUnique();

        // Email de usuario único
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Restricciones de borrado para evitar cascadas múltiples
        modelBuilder.Entity<Equipo>()
            .HasOne(e => e.Pais)
            .WithMany(p => p.Equipos)
            .HasForeignKey(e => e.PaisId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Jugador>()
            .HasOne(j => j.Equipo)
            .WithMany(e => e.Jugadores)
            .HasForeignKey(j => j.EquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cromo>()
            .HasOne(c => c.Jugador)
            .WithMany(j => j.Cromos)
            .HasForeignKey(c => c.JugadorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cromo>()
            .HasOne(c => c.Equipo)
            .WithMany(e => e.Cromos)
            .HasForeignKey(c => c.EquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cromo>()
            .HasOne(c => c.Album)
            .WithMany(a => a.Cromos)
            .HasForeignKey(c => c.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UsuarioCromo>()
            .HasOne(uc => uc.Cromo)
            .WithMany(c => c.UsuarioCromos)
            .HasForeignKey(uc => uc.CromoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UsuarioCromo>()
            .HasOne(uc => uc.Usuario)
            .WithMany(u => u.UsuarioCromos)
            .HasForeignKey(uc => uc.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Guardar el enum como texto para legibilidad en la DB (útil en Navicat)
        modelBuilder.Entity<UsuarioCromo>()
            .Property(uc => uc.Estado)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}
