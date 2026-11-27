using Microsoft.EntityFrameworkCore;
using tccReladorioDeCriacao.Models;

namespace tccReladorioDeCriacao
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
        public DbSet<Imaginario> Imaginarios { get; set; }
        public DbSet<ImagemImaginario> ImagensImaginarios { get; set; }
        
    }
}