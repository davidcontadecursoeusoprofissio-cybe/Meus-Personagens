

namespace tccReladorioDeCriacao.Models;

public class ImagemImaginario
{
    public int Id { get; set; }
    public string CaminhoImagem { get; set; }
    public int ImaginarioId { get; set; }
    public Imaginario Imaginario { get; set; }
        
}