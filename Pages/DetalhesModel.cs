

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

        public Imaginario? Imaginario { get; set; }

        [BindProperty]
        public IFormFile? NovaImagemPrincipal { get; set; }

        [BindProperty]
        public List<IFormFile> NovasImagens { get; set; } = new();

        public IActionResult OnGet()
        {
            Imaginario = _diario.BuscarPorId(Id);

            if (Imaginario == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAtualizarImagensAsync()
        {
            var imaginario = _diario.BuscarPorId(Id);

            if (imaginario == null)
            {
                return NotFound();
            }

            string pasta = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "imagens"
            );

            Directory.CreateDirectory(pasta);

            // Substituir imagem principal.
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

                string nome = Guid.NewGuid().ToString() + extensao;

                string caminho = Path.Combine(pasta, nome);

                using (var stream = new FileStream(caminho, FileMode.Create))
                {
                    await NovaImagemPrincipal.CopyToAsync(stream);
                }

                imaginario.Imagem = "/imagens/" + nome;

                _diario.Editar(imaginario);
            }

            // Adicionar novas imagens à galeria.
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

                    string nome = Guid.NewGuid().ToString() + extensao;

                    string caminho = Path.Combine(pasta, nome);

                    using (var stream = new FileStream(caminho, FileMode.Create))
                    {
                        await arquivo.CopyToAsync(stream);
                    }

                    _diario.AdicionarImagem(
                        Id,
                        "/imagens/" + nome
                    );
                }
            }

            return RedirectToPage("/Detalhes", new { id = Id });
        }

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
