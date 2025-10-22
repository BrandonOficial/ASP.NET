/*
 * Data/ApplicationDbContext.cs
 * Contexto do EF Core que gerencia a sessão com o banco de dados.
 */
using CadastroFornecedores.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroFornecedores.Data
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