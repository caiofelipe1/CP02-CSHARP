namespace Fiap.Banco.API.Models;

public class ReceberSalario : Produto
{
    public int ExigeConvenioEmpresa { get; set; } = 1;
}