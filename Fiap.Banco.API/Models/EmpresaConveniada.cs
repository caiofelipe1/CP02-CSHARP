namespace Fiap.Banco.API.Models;

public class EmpresaConveniada
{
    public int Id { get; set; }

    public string Cnpj { get; set; } = string.Empty;

    public string RazaoSocial { get; set; } = string.Empty;

    public int Ativa { get; set; } = 1;
}