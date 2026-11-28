


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using tccReladorioDeCriacao.Models;
using tccReladorioDeCriacao.Data;

namespace tccReladorioDeCriacao.Pages
{
    public class DetalhesModel : PageModel
    {
        private readonly DiariodeCriacao _diario;
        private readonly IWebHostEnvironment _environment;

        public DetalhesModel(
            DiariodeCriacao diario,
            IWebHostEnvironment environment)
        {
            _diario = diario;
            _environment = environment;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Imaginario? Imaginario { get; set; }

        [BindProperty]
        public IFormFile? NovaImagemPrincipal { get; set; }

        [BindProperty]
        public List<IFormFile> NovasImagens { get; set; } = new();

        [BindProperty]
        public int ImagemId { get; set; }

        [BindProperty]
        public IFormFile? NovaImagemGaleria { get; set; }


        // CARREGAR PERSONAGEM
        public IActionResult OnGet()
        {
            Imaginario = _diario.BuscarPorId(Id);

            if (Imaginario == null)
            {
                return NotFound();
            }

            return Page();
        }


        // EDITAR TODAS AS INFORMAÇÕES
        public IActionResult OnPostEditarInformacoes()
        {
            var dados = Imaginario;

            if (dados == null)
            {
                return NotFound();
            }

            dados.Id = Id;

            _diario.Editar(dados);

            return RedirectToPage("/Detalhes", new { id = Id });
        }


        // SUBSTITUIR IMAGEM PRINCIPAL
        public async Task<IActionResult> OnPostAtualizarImagemPrincipalAsync()
        {
            var imaginario = _diario.BuscarPorId(Id);

            if (imaginario == null)
            {
                return NotFound();
            }

            if (NovaImagemPrincipal != null &&
                NovaImagemPrincipal.Length > 0)
            {
                string extensao = Path.GetExtension(
                    NovaImagemPrincipal.FileName
                ).ToLowerInvariant();

                if (!ExtensaoValida(extensao))
                {
                    return BadRequest("Formato de imagem inválido.");
                }

                string imagemAntiga = imaginario.Imagem ?? "";

                string caminhoImagem = await SalvarImagemAsync(
                    NovaImagemPrincipal,
                    extensao
                );

                imaginario.Imagem = caminhoImagem;

                _diario.Editar(imaginario);

                ExcluirArquivoImagem(imagemAntiga);
            }

            return RedirectToPage("/Detalhes", new { id = Id });
        }


        // SUBSTITUIR IMAGEM SECUNDÁRIA
        public async Task<IActionResult> OnPostEditarImagemAsync()
        {
            if (NovaImagemGaleria == null ||
                NovaImagemGaleria.Length == 0)
            {
                return RedirectToPage("/Detalhes", new { id = Id });
            }

            var imagem = _diario.PuxarImagens(Id)
                .FirstOrDefault(i => i.Id == ImagemId);

            if (imagem == null)
            {
                return NotFound();
            }

            string extensao = Path.GetExtension(
                NovaImagemGaleria.FileName
            ).ToLowerInvariant();

            if (!ExtensaoValida(extensao))
            {
                return BadRequest("Formato de imagem inválido.");
            }

            string imagemAntiga = imagem.CaminhoImagem;

            string caminhoImagem = await SalvarImagemAsync(
                NovaImagemGaleria,
                extensao
            );

            _diario.EditarImagem(
                ImagemId,
                caminhoImagem
            );

            ExcluirArquivoImagem(imagemAntiga);

            return RedirectToPage("/Detalhes", new { id = Id });
        }


        // ADICIONAR IMAGENS À GALERIA
        public async Task<IActionResult> OnPostAdicionarImagensAsync()
        {
            var imaginario = _diario.BuscarPorId(Id);

            if (imaginario == null)
            {
                return NotFound();
            }

            foreach (var arquivo in NovasImagens)
            {
                if (arquivo != null && arquivo.Length > 0)
                {
                    string extensao = Path.GetExtension(
                        arquivo.FileName
                    ).ToLowerInvariant();

                    if (!ExtensaoValida(extensao))
                    {
                        continue;
                    }

                    string caminhoImagem = await SalvarImagemAsync(
                        arquivo,
                        extensao
                    );

                    _diario.AdicionarImagem(
                        Id,
                        caminhoImagem
                    );
                }
            }

            return RedirectToPage("/Detalhes", new { id = Id });
        }


        // EXCLUIR IMAGEM SECUNDÁRIA
        public IActionResult OnPostDeletarImagem()
        {
            var imagem = _diario.PuxarImagens(Id)
                .FirstOrDefault(i => i.Id == ImagemId);

            if (imagem != null)
            {
                string caminhoAntigo = imagem.CaminhoImagem;

                _diario.DeletarImagem(ImagemId);

                ExcluirArquivoImagem(caminhoAntigo);
            }

            return RedirectToPage("/Detalhes", new { id = Id });
        }


        // MÉTODO ORIGINAL: ATUALIZAR IMAGEM PRINCIPAL
        // E ADICIONAR IMAGENS À GALERIA
        public async Task<IActionResult> OnPostAtualizarImagensAsync()
        {
            var imaginario = _diario.BuscarPorId(Id);

            if (imaginario == null)
            {
                return NotFound();
            }

            // Substituir imagem principal
            if (NovaImagemPrincipal != null &&
                NovaImagemPrincipal.Length > 0)
            {
                string extensao = Path.GetExtension(
                    NovaImagemPrincipal.FileName
                ).ToLowerInvariant();

                if (!ExtensaoValida(extensao))
                {
                    ModelState.AddModelError(
                        "NovaImagemPrincipal",
                        "Formato de imagem inválido."
                    );

                    Imaginario = imaginario;
                    return Page();
                }

                string imagemAntiga = imaginario.Imagem ?? "";

                string caminhoImagem = await SalvarImagemAsync(
                    NovaImagemPrincipal,
                    extensao
                );

                imaginario.Imagem = caminhoImagem;

                _diario.Editar(imaginario);

                ExcluirArquivoImagem(imagemAntiga);
            }

            // Adicionar imagens à galeria
            foreach (var arquivo in NovasImagens)
            {
                if (arquivo != null && arquivo.Length > 0)
                {
                    string extensao = Path.GetExtension(
                        arquivo.FileName
                    ).ToLowerInvariant();

                    if (!ExtensaoValida(extensao))
                    {
                        continue;
                    }

                    string caminhoImagem = await SalvarImagemAsync(
                        arquivo,
                        extensao
                    );

                    _diario.AdicionarImagem(
                        Id,
                        caminhoImagem
                    );
                }
            }

            return RedirectToPage("/Detalhes", new { id = Id });
        }


        // SALVAR ARQUIVO NA PASTA WWWROOT
        private async Task<string> SalvarImagemAsync(
            IFormFile arquivo,
            string extensao)
        {
            string pasta = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "imagens"
            );

            Directory.CreateDirectory(pasta);

            string nome = Guid.NewGuid().ToString() + extensao;

            string caminho = Path.Combine(pasta, nome);

            using (var stream = new FileStream(caminho, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return "/imagens/" + nome;
        }


        // EXCLUIR ARQUIVO FÍSICO DA PASTA
        private void ExcluirArquivoImagem(string caminhoImagem)
        {
            if (string.IsNullOrEmpty(caminhoImagem))
            {
                return;
            }

            string nomeArquivo = Path.GetFileName(caminhoImagem);

            string caminho = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "imagens",
                nomeArquivo
            );

            if (System.IO.File.Exists(caminho))
            {
                System.IO.File.Delete(caminho);
            }
        }


        // VALIDAR EXTENSÃO DA IMAGEM
        private bool ExtensaoValida(string extensao)
        {
            string[] permitidas =
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".webp"
            };

            return permitidas.Contains(extensao);
        }
    }
}





