using AppMvcBasica.Models;
using System;
using System.Threading.Tasks;

namespace DevPaines.Business.Interfaces
{
    public interface IProdutoService : IDisposable
    {
        Task Adicionar(Produto produto);
        Task Atualizar(Produto produto);
        Task Remover(Guid guid);
    }
}