using AppMvcBasica.Models;
using DevPaines.Business.Interfaces;
using DevPaines.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace DevPaines.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository,
                              INotificador notificador) : base(notificador)
            => this._produtoRepository = produtoRepository;

        public async Task Adicionar(Produto produto)
        {
            if (!base.ExecutarValidacao(new ProdutoValidation(), produto))
                return;

            await _produtoRepository.Adicionar(produto);
        }
        public async Task Atualizar(Produto produto)
        {
            if (!base.ExecutarValidacao(new ProdutoValidation(), produto))
                return;

            await _produtoRepository.Atualizar(produto);
        }

        public async Task Remover(Guid guid)
        {
            await _produtoRepository.Remover(guid);
        }

        public void Dispose() => _produtoRepository?.Dispose();
    }
}
