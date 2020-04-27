using AppMvcBasica.Models;
using DevPaines.Business.Interfaces;
using DevPaines.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPaines.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(DataDbContext dataDbContext) : base(dataDbContext)
        {
        }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            return await DataDbContext.Produtos
                .AsNoTracking()
                .Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            return await DataDbContext.Produtos
                .AsNoTracking()
                .Include(f => f.Fornecedor)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }
        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await Buscar(p => p.FornecedorId == fornecedorId);
        }
    }
}
