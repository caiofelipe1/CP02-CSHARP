using Fiap.Banco.API.Data;
using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Controllers;

[ApiController]
[Route("api/agencias")]
public class AgenciasController : ControllerBase
{
    private readonly AppDbContext _context;

    public AgenciasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CriarAgenciaRequest request)
    {
        var agenciaExistente = await _context.Agencias
    .FirstOrDefaultAsync(a => a.Numero == request.Numero);

        if (agenciaExistente is not null)
            return BadRequest(new { mensagem = "Já existe uma agência com este número." });

        var agencia = new Agencia
        {
            Numero = request.Numero,
            Nome = request.Nome,
            Endereco = request.Endereco
        };

        _context.Agencias.Add(agencia);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = agencia.Id }, new
        {
            agencia.Id,
            agencia.Numero,
            agencia.Nome,
            agencia.Endereco
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var agencia = await _context.Agencias
            .Include(a => a.Clientes)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (agencia is null)
            return NotFound(new { mensagem = "Agência não encontrada." });

        return Ok(new
        {
            agencia.Id,
            agencia.Numero,
            agencia.Nome,
            agencia.Endereco,
            Clientes = agencia.Clientes.Select(c => new
            {
                c.Id,
                c.Nome,
                TipoCliente = c is PessoaFisica ? "PF" : "PJ"
            })
        });
    }
}