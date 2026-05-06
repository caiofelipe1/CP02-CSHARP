using Fiap.Banco.API.Data;
using Fiap.Banco.API.DTOs;
using Fiap.Banco.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("pf")]
    public async Task<IActionResult> CriarPessoaFisica(CriarPessoaFisicaRequest request)
    {
        var agencia = await _context.Agencias
            .FirstOrDefaultAsync(a => a.Id == request.AgenciaId);

        if (agencia is null)
            return BadRequest(new { mensagem = "Agência informada não existe." });

        var cpfExistente = await _context.PessoasFisicas
            .FirstOrDefaultAsync(p => p.Cpf == request.Cpf);

        if (cpfExistente is not null)
            return BadRequest(new { mensagem = "Já existe uma pessoa física com este CPF." });

        var cliente = new PessoaFisica
        {
            Nome = request.Nome,
            Cpf = request.Cpf,
            DataNascimento = request.DataNascimento,
            AgenciaId = request.AgenciaId
        };

        _context.PessoasFisicas.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = cliente.Id }, new
        {
            cliente.Id,
            cliente.Nome,
            cliente.Cpf,
            cliente.DataNascimento,
            cliente.AgenciaId
        });
    }

    [HttpPost("pj")]
    public async Task<IActionResult> CriarPessoaJuridica(CriarPessoaJuridicaRequest request)
    {
        var agencia = await _context.Agencias
            .FirstOrDefaultAsync(a => a.Id == request.AgenciaId);

        if (agencia is null)
            return BadRequest(new { mensagem = "Agência informada não existe." });

        var cnpjExistente = await _context.PessoasJuridicas
            .FirstOrDefaultAsync(p => p.Cnpj == request.Cnpj);

        if (cnpjExistente is not null)
            return BadRequest(new { mensagem = "Já existe uma pessoa jurídica com este CNPJ." });

        var cliente = new PessoaJuridica
        {
            Nome = request.Nome,
            Cnpj = request.Cnpj,
            RazaoSocial = request.RazaoSocial,
            AgenciaId = request.AgenciaId
        };

        _context.PessoasJuridicas.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = cliente.Id }, new
        {
            cliente.Id,
            cliente.Nome,
            cliente.Cnpj,
            cliente.RazaoSocial,
            cliente.AgenciaId
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Agencia)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente is null)
            return NotFound(new { mensagem = "Cliente não encontrado." });

        if (cliente is PessoaFisica pf)
        {
            return Ok(new
            {
                pf.Id,
                pf.Nome,
                TipoCliente = "PF",
                pf.Cpf,
                pf.DataNascimento,
                Agencia = new
                {
                    pf.Agencia.Id,
                    pf.Agencia.Numero,
                    pf.Agencia.Nome,
                    pf.Agencia.Endereco
                }
            });
        }

        if (cliente is PessoaJuridica pj)
        {
            return Ok(new
            {
                pj.Id,
                pj.Nome,
                TipoCliente = "PJ",
                pj.Cnpj,
                pj.RazaoSocial,
                Agencia = new
                {
                    pj.Agencia.Id,
                    pj.Agencia.Numero,
                    pj.Agencia.Nome,
                    pj.Agencia.Endereco
                }
            });
        }

        return Ok(cliente);
    }
}