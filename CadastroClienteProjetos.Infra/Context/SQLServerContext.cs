using CadastroClienteProjetos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CadastroClienteProjetos.Infra.Context
{
    public class SQLServerContext : DbContext
    {
        public SQLServerContext(DbContextOptions<SQLServerContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Projeto> Projeto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.EnableSensitiveDataLogging();
            base.OnConfiguring(builder);
        }
    }
}