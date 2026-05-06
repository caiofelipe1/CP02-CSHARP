using Fiap.Banco.API.Enums;

namespace Fiap.Banco.API.Models;

public class Contratacao
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public Cliente Cliente { get; set; } = null!;

    public int ProdutoId { get; set; }

    public Produto Produto { get; set; } = null!;

    public StatusContratacao Status { get; set; } = StatusContratacao.Pendente;

    public DateTime DataSolicitacao { get; set; } = DateTime.Now;

    public string? CnpjEmpresaEmpregadora { get; set; }

    public string? MotivoReprovacao { get; set; }
}