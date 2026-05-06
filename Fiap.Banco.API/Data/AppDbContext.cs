using Fiap.Banco.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Agencia> Agencias => Set<Agencia>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<PessoaFisica> PessoasFisicas => Set<PessoaFisica>();
    public DbSet<PessoaJuridica> PessoasJuridicas => Set<PessoaJuridica>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<ReceberSalario> ReceberSalarios => Set<ReceberSalario>();
    public DbSet<MaquinaDeCartao> MaquinasDeCartao => Set<MaquinaDeCartao>();
    public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();
    public DbSet<Contratacao> Contratacoes => Set<Contratacao>();
    public DbSet<EmpresaConveniada> EmpresasConveniadas => Set<EmpresaConveniada>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>()
            .HasDiscriminator<string>("TipoCliente")
            .HasValue<PessoaFisica>("PF")
            .HasValue<PessoaJuridica>("PJ");

        modelBuilder.Entity<Produto>()
            .HasDiscriminator<string>("TipoProduto")
            .HasValue<ReceberSalario>("RECEBER_SALARIO")
            .HasValue<MaquinaDeCartao>("MAQUINA_CARTAO")
            .HasValue<Emprestimo>("EMPRESTIMO");

        modelBuilder.Entity<PessoaFisica>()
            .HasIndex(p => p.Cpf)
            .IsUnique();

        modelBuilder.Entity<PessoaJuridica>()
            .HasIndex(p => p.Cnpj)
            .IsUnique();

        modelBuilder.Entity<EmpresaConveniada>()
            .HasIndex(e => e.Cnpj)
            .IsUnique();

        modelBuilder.Entity<Agencia>()
            .HasIndex(a => a.Numero)
            .IsUnique();

        modelBuilder.Entity<Agencia>().HasData(
            new Agencia
            {
                Id = 1,
                Numero = "0001",
                Nome = "Agência Paulista",
                Endereco = "Avenida Paulista, São Paulo"
            }
        );

        modelBuilder.Entity<ReceberSalario>().HasData(
            new ReceberSalario
            {
                Id = 1,
                Nome = "Receber Salário",
                ExigeConvenioEmpresa = 1
            }
        );

        modelBuilder.Entity<EmpresaConveniada>().HasData(
            new EmpresaConveniada
            {
                Id = 1,
                Cnpj = "12345678000199",
                RazaoSocial = "Empresa Conveniada FIAP LTDA",
                Ativa = 1
            }
        );
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();

            if (!string.IsNullOrEmpty(tableName))
            {
                var upperTableName = tableName.ToUpper();

                if (!upperTableName.StartsWith("PB_"))
                {
                    upperTableName = $"PB_{upperTableName}";
                }

                entity.SetTableName(upperTableName);
            }

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.Name.ToUpper());
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName($"PK_{entity.GetTableName()}");
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                foreignKey.SetConstraintName(
                    $"FK_{entity.GetTableName()}_{foreignKey.PrincipalEntityType.GetTableName()}"
                );
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(
                    $"IX_{entity.GetTableName()}_{string.Join("_", index.Properties.Select(p => p.Name.ToUpper()))}"
                );
            }
        }
        base.OnModelCreating(modelBuilder);
    }
}