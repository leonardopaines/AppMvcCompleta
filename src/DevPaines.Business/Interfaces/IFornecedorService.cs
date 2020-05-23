using AppMvcBasica.Models;
using System;
using System.Threading.Tasks;

namespace DevPaines.Business.Interfaces
{
    public interface IFornecedorService : IDisposable
    {
        Task Adicionar(Fornecedor fornecedor);
        Task Atualizar(Fornecedor fornecedor);
        Task Remover(Guid guid);
        Task AtualizarEndereco(Endereco endereco);
    }
}