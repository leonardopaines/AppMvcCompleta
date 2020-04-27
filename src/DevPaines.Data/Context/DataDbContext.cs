using AppMvcBasica.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DevPaines.Data.Context
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Se for necessário será criada com varchar 100
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                                                        .SelectMany(e => e.GetProperties()
                                                                        .Where(p => p.ClrType == typeof(string))
                                                                    ))
            {
                property.Relational().ColumnType = "varchar(100)";
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataDbContext).Assembly);

            //Remover deleção em cascata
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                                            .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder);
        }

    }
}