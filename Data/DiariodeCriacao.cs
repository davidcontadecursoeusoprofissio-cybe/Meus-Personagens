using tccReladorioDeCriacao.Models;
using tccReladorioDeCriacao.Data;
namespace tccReladorioDeCriacao.Data
{
    public class DiariodeCriacao
    {
        private readonly AppDbContext _context;

        public DiariodeCriacao(AppDbContext context)
        {
            _context = context;
        }

        public List<Imaginario> PuxarElementosdoImaginario()
        {
            return _context.Imaginarios.ToList();
        }

        public void Adicionar(Imaginario imaginario)
        {
            _context.Imaginarios.Add(imaginario);
            _context.SaveChanges();
        }
        public void Deletar(int id)
        {
            var imaginario = _context.Imaginarios.Find(id);
            if ( imaginario != null)
            {
                _context.Imaginarios.Remove(imaginario);
                _context.SaveChanges();
            }
        }
        
    }
}