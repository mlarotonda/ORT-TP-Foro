using Foro_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foro_C.Data
{
    public class ForoContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {

        public ForoContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EntradaMiembro>().HasKey(em => new { em.EntradaId, em.MiembroId });

            modelBuilder.Entity<EntradaMiembro>()
                .HasOne(em => em.Entrada)
                .WithMany(e => e.MiembrosHabilitados)
                .HasForeignKey(em => em.EntradaId);

            modelBuilder.Entity<EntradaMiembro>()
                .HasOne(em => em.Miembro)
                .WithMany(m => m.EntradasHabilitadas)
                .HasForeignKey(em => em.MiembroId);


            modelBuilder.Entity<Reaccion>().HasKey(rt => new { rt.RespuestaId, rt.MiembroId });

            modelBuilder.Entity<Reaccion>()
                .HasOne(r => r.Respuesta)
                .WithMany(rt => rt.Reacciones)
                .HasForeignKey(r => r.RespuestaId);

            modelBuilder.Entity<Reaccion>()
                .HasOne(r => r.Miembro)
                .WithMany(m => m.Reacciones)
                .HasForeignKey(r => r.MiembroId);

            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas"); //Resuelve la utilizacion de ASPNETUSRS como nombre de tabla

            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles"); //Resuelve la utilizacion de ASPNETROLES como nombre de tabla


            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles"); //Resuelve la utilizacion de ASPNETUSRSROLES como nombre de tabla

        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<EntradaMiembro> EntradaMiembros { get; set; }
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Reaccion> Reacciones { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }

        public DbSet<Rol> Roles { get; set; }

    }
}
