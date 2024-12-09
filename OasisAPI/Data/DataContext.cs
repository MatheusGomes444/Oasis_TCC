using Microsoft.EntityFrameworkCore;
using OasisApi.Models;

namespace OasisApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
         public DbSet<User> Users { get; set; }
        public DbSet<Morador> Moradores { get; set; }
        public DbSet<Alojamento> Alojamentos { get; set; }
        public DbSet<FilaDeEspera> FilasDeEspera { get; set; } // Adicionado

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração de Alojamento
            modelBuilder.Entity<Alojamento>().ToTable("Alojamentos");
            modelBuilder.Entity<Alojamento>().HasKey(a => a.Id);
            modelBuilder.Entity<Alojamento>().Property(a => a.Nome).IsRequired(false).HasMaxLength(200);        
            modelBuilder.Entity<Alojamento>().Property(a => a.Equipe).HasMaxLength(100);
            modelBuilder.Entity<Alojamento>().Property(a => a.Telefone);
            modelBuilder.Entity<Alojamento>().Property(a => a.Email).HasMaxLength(100);
            modelBuilder.Entity<Alojamento>().Property(a => a.CapacidadeMaxima);
            modelBuilder.Entity<Alojamento>().Property(a => a.Pet).IsRequired(false);
            modelBuilder.Entity<Alojamento>().Property(a => a.Sexo).IsRequired(false);
            modelBuilder.Entity<Alojamento>().Property(a => a.Pertences).IsRequired(false);
            modelBuilder.Entity<Alojamento>().Property(a => a.Refeicoes).IsRequired(false);

            // Configuração de Morador
            modelBuilder.Entity<Morador>().ToTable("Moradores");
            modelBuilder.Entity<Morador>().HasKey(m => m.Id);
            modelBuilder.Entity<Morador>().Property(m => m.Nome).IsRequired(false).HasMaxLength(200);
            modelBuilder.Entity<Morador>().Property(m => m.CPF).IsRequired(false).HasMaxLength(11);
            modelBuilder.Entity<Morador>().Property(m => m.RG).IsRequired(false).HasMaxLength(9);
            modelBuilder.Entity<Morador>().Property(m => m.Telefone).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Morador>().Property(m => m.Endereco).IsRequired(false).HasMaxLength(200);
            modelBuilder.Entity<Morador>().Property(m => m.Idade).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Morador>().Property(m => m.Observacoes).IsRequired(false);

            // Configurar relacionamento entre Alojamento e Moradores
            modelBuilder.Entity<Morador>()
                .HasOne(m => m.Alojamento)
                .WithMany(a => a.Moradores)
                .HasForeignKey(m => m.AlojamentoId);

            // Configuração de FilaDeEspera
            modelBuilder.Entity<FilaDeEspera>().ToTable("FilasDeEspera");
            modelBuilder.Entity<FilaDeEspera>().HasKey(f => f.Id);

            // Relacionamento com Morador
            modelBuilder.Entity<FilaDeEspera>()
                .HasOne(f => f.Morador)
                .WithMany()
                .HasForeignKey(f => f.MoradorId)
                .OnDelete(DeleteBehavior.NoAction); // Evita exclusão em cascata

            // Relacionamento com Alojamento
            modelBuilder.Entity<FilaDeEspera>()
                .HasOne(f => f.Alojamento)
                .WithMany()
                .HasForeignKey(f => f.AlojamentoId)
                .OnDelete(DeleteBehavior.NoAction); // Evita exclusão em cascata

            // Dados iniciais para Alojamentos
            modelBuilder.Entity<Alojamento>().HasData(
                new Alojamento
                {
                    Id = 1,
                    Nome = "Albergue Vila Maria",
                    Equipe = "Equipe A",
                    Telefone = 123456789,
                    Email = "exemplo1@dominio.com",
                    CapacidadeMaxima = 10,
                    Pet = "Apenas cães",
                    Sexo = "Masculino",
                    Pertences = "Roupas e sapatos",
                    Refeicoes = "Café e janta",
                }
            );

            // Dados iniciais para Moradores
            modelBuilder.Entity<Morador>().HasData(
                new Morador
                {
                    Id = 1,
                    Nome = "Pedro",
                    CPF = "12345678900",
                    RG = "603456789",
                    Telefone = 912345678,
                    Endereco = "Rua Alcântara, 113",
                    Idade = 18,
                    Datanascimento = 0,
                    Nacionalidade = "Brasileiro",
                    Observacoes = "Sem observações",
                    AlojamentoId = 1,
                    Ativo = true
                }
            );
        }
    }
}
