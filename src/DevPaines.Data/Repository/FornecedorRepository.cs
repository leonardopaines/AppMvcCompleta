using AppMvcBasica.Models;
using DevPaines.Business.Interfaces;
using DevPaines.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevPaines.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(DataDbContext dataDbContext) : base(dataDbContext)
        {
        }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await DataDbContext.Fornecedores
                .AsNoTracking()
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await DataDbContext.Fornecedores
                .AsNoTracking()
                .Include(c => c.Endereco)
                .Include(p => p.Produtos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
