using Microsoft.EntityFrameworkCore;
using tccReladorioDeCriacao.Models;

namespace tccReladorioDeCriacao
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
        public Dbset<Imaginario> Imaginarios { get; set; }
        
    }
}