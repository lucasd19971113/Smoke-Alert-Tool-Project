using Microsoft.EntityFrameworkCore;
using Projeto_IOT.Models;

namespace Projeto_IOT.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
        public static string ConnectionString { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
    }
}