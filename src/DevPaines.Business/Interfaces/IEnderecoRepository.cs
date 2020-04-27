using AppMvcBasica.Models;
using System;
using System.Threading.Tasks;

namespace DevPaines.Business.Interfaces
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
    }
}
