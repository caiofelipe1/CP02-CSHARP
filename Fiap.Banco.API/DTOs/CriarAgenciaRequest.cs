namespace Fiap.Banco.API.DTOs;

public class CriarAgenciaRequest
{
    public string Numero { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string Endereco { get; set; } = string.Empty;
}