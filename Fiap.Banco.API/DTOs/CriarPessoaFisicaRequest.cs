namespace Fiap.Banco.API.DTOs;

public class CriarPessoaFisicaRequest
{
    public string Nome { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    public int AgenciaId { get; set; }
}