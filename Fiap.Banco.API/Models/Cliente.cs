namespace Fiap.Banco.API.Models;

public abstract class Cliente
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public int AgenciaId { get; set; }

    public Agencia Agencia { get; set; } = null!;

    public ICollection<Contratacao> Contratacoes { get; set; } = new List<Contratacao>();
}