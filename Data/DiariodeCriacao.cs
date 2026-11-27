
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
        // Recebe um objeto Imaginario com as informações
        // preenchidas no formulário de cadastro.
        // Add adiciona o personagem ao banco.
        // SaveChanges salva efetivamente as informações.
        public void Adicionar(Imaginario imaginario)
        {
            _context.Imaginarios.Add(imaginario);
            _context.SaveChanges();
        }

        // BUSCAR POR ID:
        // Procura um personagem específico através do seu ID.
        // É utilizado para abrir a ficha de um personagem
        // ou carregar suas informações na tela de edição.
        // Se não encontrar o personagem, retorna null.
        public Imaginario? BuscarPorId(int id)
        {
            return _context.Imaginarios
                .Include(i => i.Imagens)
                .FirstOrDefault(i => i.Id == id);
        }

        // EDITAR PERSONAGEM:
        // Permite alterar TODAS as propriedades do Model Imaginario,
        // como nome, idade, altura, história, poderes, características
        // físicas, informações do criador e demais campos.
        //
        // Primeiro procura o personagem pelo ID.
        // Depois verifica se ele existe.
        // SetValues copia os novos valores recebidos para o registro
        // que já está cadastrado no banco.
        // SaveChanges salva as alterações.
        //
        // ATENÇÃO: as imagens relacionadas são administradas
        // separadamente pelos métodos abaixo.
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
        // Permite adicionar várias imagens ao mesmo personagem.
        // Recebe o ID do personagem e o caminho do arquivo de imagem.
        //
        // O Controller deve receber o arquivo enviado pelo usuário,
        // salvar esse arquivo na pasta wwwroot e passar seu caminho
        // para este método.
        //
        // Cada imagem será registrada separadamente no banco,
        // mas todas ficarão vinculadas ao mesmo personagem.
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
        // Procura todas as imagens que pertencem a um personagem.
        // Where filtra as imagens utilizando o ID do personagem.
        // ToList retorna todas as imagens encontradas.
        // Pode ser utilizado para montar a galeria de imagens.
        public List<ImagemImaginario> PuxarImagens(int id)
        {
            return _context.ImagensImaginarios
                .Where(i => i.ImaginarioId == id)
                .ToList();
        }

        // EXCLUIR IMAGEM:
        // Procura uma imagem específica pelo seu próprio ID.
        // Se encontrar, remove o registro do banco de dados.
        //
        // ATENÇÃO: este método não apaga o arquivo físico
        // da pasta wwwroot. Essa tarefa deve ser feita no Controller.
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
        // Procura o personagem pelo ID e carrega suas imagens.
        // Se encontrar, remove o personagem do banco.
        // SaveChanges confirma a exclusão.
        //
        // O relacionamento com as imagens precisa estar configurado
        // para exclusão em cascata no AppDbContext.
        // Os arquivos físicos das imagens também precisam
        // ser removidos pelo Controller.
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
