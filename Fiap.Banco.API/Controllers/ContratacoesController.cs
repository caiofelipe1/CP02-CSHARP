using Fiap.Banco.API.Data;
using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Enums;
using Fiap.Banco.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Controllers;

[ApiController]
[Route("api/contratacoes")]
public class ContratacoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContratacoesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Solicitar(CriarContratacaoRequest request)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == request.ClienteId);

        if (cliente is null)
            return NotFound(new { mensagem = "Cliente não encontrado." });

        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == request.ProdutoId);

        if (produto is null)
            return NotFound(new { mensagem = "Produto não encontrado." });

        var contratacao = new Contratacao
        {
            ClienteId = request.ClienteId,
            ProdutoId = request.ProdutoId,
            CnpjEmpresaEmpregadora = request.CnpjEmpresaEmpregadora,
            Status = StatusContratacao.Pendente,
            DataSolicitacao = DateTime.Now
        };

        if (produto is ReceberSalario)
        {
            var empresaConveniada = await _context.EmpresasConveniadas
                .FirstOrDefaultAsync(e =>
                    e.Cnpj == request.CnpjEmpresaEmpregadora &&
                    e.Ativa == 1);

            if (empresaConveniada is not null)
            {
                contratacao.Status = StatusContratacao.Aprovada;
            }
            else
            {
                contratacao.Status = StatusContratacao.Reprovada;
                contratacao.MotivoReprovacao = "Empresa empregadora não possui convênio ativo.";
            }
        }

        _context.Contratacoes.Add(contratacao);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = contratacao.Id }, new
        {
            contratacao.Id,
            contratacao.ClienteId,
            contratacao.ProdutoId,
            Status = contratacao.Status.ToString(),
            contratacao.DataSolicitacao,
            contratacao.CnpjEmpresaEmpregadora,
            contratacao.MotivoReprovacao
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var contratacao = await _context.Contratacoes
            .Include(c => c.Cliente)
            .Include(c => c.Produto)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contratacao is null)
            return NotFound(new { mensagem = "Contratação não encontrada." });

        return Ok(new
        {
            contratacao.Id,
            contratacao.DataSolicitacao,
            Status = contratacao.Status.ToString(),
            contratacao.CnpjEmpresaEmpregadora,
            contratacao.MotivoReprovacao,
            Cliente = new
            {
                contratacao.Cliente.Id,
                contratacao.Cliente.Nome,
                TipoCliente = contratacao.Cliente is PessoaFisica ? "PF" : "PJ"
            },
            Produto = new
            {
                contratacao.Produto.Id,
                contratacao.Produto.Nome,
                TipoProduto = contratacao.Produto.GetType().Name
            }
        });
    }
}