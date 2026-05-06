namespace Fiap.Banco.API.DTOs;

public class CriarPessoaJuridicaRequest
{
    public string Nome { get; set; } = string.Empty;

    public string Cnpj { get; set; } = string.Empty;

    public string RazaoSocial { get; set; } = string.Empty;

    public int AgenciaId { get; set; }
}