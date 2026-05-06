namespace Fiap.Banco.API.DTOs;

public class CriarContratacaoRequest
{
    public int ClienteId { get; set; }

    public int ProdutoId { get; set; }

    public string? CnpjEmpresaEmpregadora { get; set; }
}