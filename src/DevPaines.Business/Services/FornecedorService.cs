using AppMvcBasica.Models;
using DevPaines.Business.Interfaces;
using DevPaines.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevPaines.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository,
                                 IEnderecoRepository enderecoRepository,
                                 INotificador notificador): base(notificador)
        {
            this._fornecedorRepository = fornecedorRepository;
            this._enderecoRepository = enderecoRepository;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            if (!base.ExecutarValidacao(new FornecedorValidation(), fornecedor)
             || !base.ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco))
                return;

            if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado.");
                return;
            }

            await _fornecedorRepository.Adicionar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!base.ExecutarValidacao(new FornecedorValidation(), fornecedor))
                return;

            if (_fornecedorRepository
                .Buscar(f => f.Documento == fornecedor.Documento
                          && f.Id != fornecedor.Id)
                .Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado.");
                return;
            }

            await _fornecedorRepository.Atualizar(fornecedor);
        }
        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!base.ExecutarValidacao(new EnderecoValidation(), endereco))
                return;

            await _enderecoRepository.Atualizar(endereco);
        }

        public async Task Remover(Guid guid)
        {
            if (_fornecedorRepository.ObterFornecedorEndereco(guid).Result.Produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados!");
                return;
            }

            await _fornecedorRepository.Remover(guid);
        }
        public void Dispose()
        {
            _enderecoRepository?.Dispose();
            _fornecedorRepository?.Dispose();
        }
    }
}