


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using tccReladorioDeCriacao.Models;
using tccReladorioDeCriacao.Data;

public class CadastroModel : PageModel
{
    private readonly DiariodeCriacao _diario;
    private readonly IWebHostEnvironment _environment;

    public CadastroModel(
        DiariodeCriacao diario,
        IWebHostEnvironment environment)
    {
        _diario = diario;
        _environment = environment;
    }

    [BindProperty]
    public Imaginario Imaginario { get; set; } = new();

    [BindProperty]
    public IFormFile? ImagemPrincipal { get; set; }

    [BindProperty]
    public List<IFormFile> Arquivos { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Cria a pasta onde as imagens serão armazenadas.
        string pasta = Path.Combine(
            _environment.ContentRootPath,
            "wwwroot",
            "imagens"
        );

        Directory.CreateDirectory(pasta);

        // Verifica se existe uma imagem principal.
        if (ImagemPrincipal != null && ImagemPrincipal.Length > 0)
        {
            string extensao = Path.GetExtension(
                ImagemPrincipal.FileName
            ).ToLowerInvariant();

            if (!ExtensaoValida(extensao))
            {
                ModelState.AddModelError(
                    "ImagemPrincipal",
                    "Formato de imagem inválido."
                );

                return Page();
            }

            string nomeArquivo = Guid.NewGuid().ToString() + extensao;

            string caminho = Path.Combine(pasta, nomeArquivo);

            using (var stream = new FileStream(caminho, FileMode.Create))
            {
                await ImagemPrincipal.CopyToAsync(stream);
            }

            // Guarda o caminho no Model Imaginario.
            Imaginario.Imagem = "/imagens/" + nomeArquivo;
        }

        // Salva o imaginário no banco.
        // O banco gera o ID automaticamente.
        _diario.Adicionar(Imaginario);

        // Salva todas as imagens da galeria.
        foreach (var arquivo in Arquivos)
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

                string nomeArquivo = Guid.NewGuid().ToString() + extensao;

                string caminho = Path.Combine(pasta, nomeArquivo);

                using (var stream = new FileStream(caminho, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }

                string caminhoBanco = "/imagens/" + nomeArquivo;

                // Vincula a imagem ao imaginário pelo ID.
                _diario.AdicionarImagem(
                    Imaginario.Id,
                    caminhoBanco
                );
            }
        }

        return RedirectToPage("/Index");
    }

    private bool ExtensaoValida(string extensao)
    {
        string[] extensoesPermitidas =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".webp"
        };

        return extensoesPermitidas.Contains(extensao);
    }
}


