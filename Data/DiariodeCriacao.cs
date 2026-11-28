



using tccReladorioDeCriacao.Models;
using tccReladorioDeCriacao.Data;
using Microsoft.EntityFrameworkCore;

namespace tccReladorioDeCriacao.Data
{
    public class DiariodeCriacao
    {
        // Armazena a conexão com o banco de dados.
        // É através dela que conseguimos consultar, adicionar,
        // editar e excluir informações.
        private readonly AppDbContext _context;

        // Construtor: recebe a conexão com o banco de dados
        // e guarda essa conexão na variável _context.
        public DiariodeCriacao(AppDbContext context)
        {
            _context = context;
        }

        // BUSCAR PERSONAGENS:
        // Retorna todos os personagens cadastrados no banco.
        // Include também carrega todas as imagens relacionadas
        // a cada personagem.
        // ToList transforma os resultados em uma lista.
        public List<Imaginario> PuxarElementosdoImaginario()
        {
            return _context.Imaginarios
                .Include(i => i.Imagens)
                .ToList();
        }

        // ADICIONAR PERSONAGEM:
        public void Adicionar(Imaginario imaginario)
        {
            _context.Imaginarios.Add(imaginario);
            _context.SaveChanges();
        }

        // BUSCAR POR ID:
        public Imaginario? BuscarPorId(int id)
        {
            return _context.Imaginarios
                .Include(i => i.Imagens)
                .FirstOrDefault(i => i.Id == id);
        }

        // EDITAR PERSONAGEM:
        public void Editar(Imaginario imaginario)
        {
            var existente = _context.Imaginarios
                .FirstOrDefault(i => i.Id == imaginario.Id);

            if (existente != null)
            {
                _context.Entry(existente)
                    .CurrentValues.SetValues(imaginario);

                _context.SaveChanges();
            }
        }

        // ADICIONAR IMAGEM:
        public void AdicionarImagem(int id, string caminhoImagem)
        {
            var imaginario = _context.Imaginarios.Find(id);

            if (imaginario != null)
            {
                var imagem = new ImagemImaginario
                {
                    ImaginarioId = id,
                    CaminhoImagem = caminhoImagem
                };

                _context.ImagensImaginarios.Add(imagem);
                _context.SaveChanges();
            }
        }

        // BUSCAR IMAGENS:
        public List<ImagemImaginario> PuxarImagens(int id)
        {
            return _context.ImagensImaginarios
                .Where(i => i.ImaginarioId == id)
                .ToList();
        }

        // EDITAR IMAGEM SECUNDÁRIA:
        // Procura uma imagem pelo ID e substitui seu caminho.
        public void EditarImagem(int id, string caminhoImagem)
        {
            var imagem = _context.ImagensImaginarios.Find(id);

            if (imagem != null)
            {
                imagem.CaminhoImagem = caminhoImagem;

                _context.SaveChanges();
            }
        }

        // EXCLUIR IMAGEM:
        public void DeletarImagem(int id)
        {
            var imagem = _context.ImagensImaginarios.Find(id);

            if (imagem != null)
            {
                _context.ImagensImaginarios.Remove(imagem);
                _context.SaveChanges();
            }
        }

        // EXCLUIR PERSONAGEM:
        public void Deletar(int id)
        {
            var imaginario = _context.Imaginarios
                .Include(i => i.Imagens)
                .FirstOrDefault(i => i.Id == id);

            if (imaginario != null)
            {
                _context.Imaginarios.Remove(imaginario);
                _context.SaveChanges();
            }
        }
    }
}




