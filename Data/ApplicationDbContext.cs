/*
 * Data/ApplicationDbContext.cs
 * Contexto do EF Core que gerencia a sessão com o banco de dados.
 */
using DESAFIOCRUD.Models; // Precisa achar a pasta Models
using Microsoft.EntityFrameworkCore;

// Garanta que o namespace é o nome do seu projeto.Data
namespace DESAFIOCRUD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Mapeia o modelo Fornecedor para uma tabela "Fornecedores" no banco
        public DbSet<Fornecedor> Fornecedores { get; set; }
    }
}