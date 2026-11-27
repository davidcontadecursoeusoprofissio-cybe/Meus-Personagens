

using Microsoft.AspNetCore.Mvc.RazorPages;
using tccReladorioDeCriacao.Models;
using tccReladorioDeCriacao.Data;

namespace tccReladorioDeCriacao.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DiariodeCriacao _diario;

        public IndexModel(DiariodeCriacao diario)
        {
            _diario = diario;
        }

        public List<Imaginario> Imaginarios { get; set; } = new();

        public void OnGet()
        {
            Imaginarios = _diario.PuxarElementosdoImaginario();
        }
    }
}


