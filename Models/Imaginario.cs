using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace tccReladorioDeCriacao.Models;

public class Imaginario
{
    public int Id  { get; set; }


    public int numero { get; set; }
    public string Nome { get; set; }
    public double Altura { get; set; }
    public int Idade { get; set; }
    public double Ano { get; set; }
    public string Local { get; set; }
    public string DescricaoLocal { get; set; }


    public string Descricao { get; set; }
    public string ContextoHistorico { get; set; } 
    public string DescricaContexto { get; set; }

    public string PontoFraco { get; set; }
    public string DescricaoPontoFraco { get; set; }


    public string Morte { get; set; }
    public string DescricaoMorte { get; set; }


    public double Inteligencia { get; set; }
    public double Força { get; set; }
    public double Velocidade { get; set; }
    public double Defesa { get; set; }
    public double Poder { get; set; }
    public string DescricaodosPontosFisicos { get; set; }


    public double PoderdeAtaque { get; set; }
    public double PoderdeDefesa { get; set;}
    public double PoderdeVelocidade { get; set; }
    public string TipodePoderes { get; set; }
    public string LimitidosPoderes { get; set; }
    public double PoderTotal { get; set; }
    public string DescricaodosPoderes { get; set; }


    public string NomedoCriador { get; set; }
    public int DatadeCriacao { get; set; }
    public string MundoemQuesePassar { get; set; }


    public string? Imagem { get; set; }

    public List<ImagemImaginario> Imagens { get; set;  } = new List<ImagemImaginario>();
}