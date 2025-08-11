using ClinicaAPI.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DAL
{
    public class ApplicationDbContext : IdentityDbContext<Utilizador>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>
        options) : base(options) { }
        // DbSets para suas entidades
        public DbSet<AtoClinico> AtosClinicos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoAtoClinico> PedidoAtoClinicos { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<SubsistemaSaude> SubsistemasSaude { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurações para a entidade Utilizador (se houver)
            modelBuilder.Entity<Utilizador>()
            .HasMany(u => u.Pedidos)
            .WithOne(p => p.Utilizador)
            .HasForeignKey(p => p.UtilizadorId)
            .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata de utilizador com pedidos
            // Configurações para PedidoAtoClinico (chaves compostas e relacionamentos)
            modelBuilder.Entity<PedidoAtoClinico>()
        .HasKey(pac => pac.PedidoAtoClinicoId); // Chave primária simples
        modelBuilder.Entity<PedidoAtoClinico>()
        .HasOne(pac => pac.Pedido)
        .WithMany(p => p.PedidoAtoClinicos)
        .HasForeignKey(pac => pac.PedidoId)
        .OnDelete(DeleteBehavior.Cascade); // Se o pedido for excluído, os atos clínicos associados também são
        modelBuilder.Entity<PedidoAtoClinico>()
        .HasOne(pac => pac.AtoClinico)
        .WithMany(ac => ac.PedidoAtoClinicos)
        .HasForeignKey(pac => pac.AtoClinicoId)
        .OnDelete(DeleteBehavior.Restrict); // Não exclui AtoClinico se houver PedidoAtoClinico
        modelBuilder.Entity<PedidoAtoClinico>()
        .HasOne(pac => pac.SubsistemaSaude)
        .WithMany(ss => ss.PedidoAtoClinicos)
        .HasForeignKey(pac => pac.SubsistemaSaudeId)
        .IsRequired(false) // Pode ser nulo
        .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PedidoAtoClinico>()
            .HasOne(pac => pac.Profissional)
            .WithMany(p => p.PedidoAtoClinicos)
            .HasForeignKey(pac => pac.ProfissionalId)
            .IsRequired(false) // Pode ser nulo
            .OnDelete(DeleteBehavior.Restrict);
            // Renomear tabelas do Identity para algo mais limpo (opcional)
            modelBuilder.Entity<Utilizador>().ToTable("Utilizadores");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>
            ().ToTable("UtilizadorRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>
            ().ToTable("UtilizadorClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>
            ().ToTable("UtilizadorLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>
            ().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>
            ().ToTable("UtilizadorTokens");
        }
    }
}
