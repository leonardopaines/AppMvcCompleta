using AppMvcBasica.Models;
using AutoMapper;
using DevPaines.App.Extensions;
using DevPaines.App.ViewModels;
using DevPaines.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPaines.App.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository,
                                  IProdutoService produtoService,
                                  IFornecedorRepository fornecedorRepository,
                                  INotificador notificador,
                                  IMapper mapper) : base(notificador)
        {
            this._produtoRepository = produtoRepository;
            this._produtoService = produtoService;
            this._fornecedorRepository = fornecedorRepository;
            this._mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            var produtos = this._mapper.Map<IEnumerable<ProdutoViewModel>>(await this._produtoRepository.ObterProdutosFornecedores());
            return this.View(produtos);
        }

        [AllowAnonymous]
        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await this.ObterProduto(id);
            if (produtoViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await this.PopularFornecedores(new ProdutoViewModel());
            return this.View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await this.PopularFornecedores(produtoViewModel);

            if (!this.ModelState.IsValid)
                return this.View(produtoViewModel);

            if (!await produtoViewModel.UploadImagemAsync())
            {
                this.ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return this.View(produtoViewModel);
            }

            await this._produtoService.Adicionar(this._mapper.Map<Produto>(produtoViewModel));

            if (!this.OperacaoValida())
                return this.View(produtoViewModel);

            return this.RedirectToAction("Index");
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await this.ObterProduto(id);
            if (produtoViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
                return this.NotFound();

            var produtoAtualizacao = await this.ObterProduto(id);
            produtoViewModel.Fornecedor = produtoAtualizacao.Fornecedor;
            produtoViewModel.FornecedorId = produtoAtualizacao.FornecedorId;

            if (!this.ModelState.IsValid)
                return this.View(produtoViewModel);

            if (produtoViewModel.ImagemUpload != null
            && !await produtoViewModel.UploadImagemAsync())
            {
                this.ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return this.View(produtoViewModel);
            }

            produtoAtualizacao.EditModel(produtoViewModel);

            await this._produtoService.Atualizar(this._mapper.Map<Produto>(produtoAtualizacao));

            if (!this.OperacaoValida())
                return this.View(produtoViewModel);

            return this.RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await this.ObterProduto(id);
            if (produto == null)
                return this.NotFound();

            return this.View(produto);
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await this.ObterProduto(id);
            if (produto == null)
                return this.NotFound();

            await this._produtoService.Remover(id);

            if (!this.OperacaoValida())
                return this.View(produto);

            TempData["Sucesso"] = "Produto excluido com sucesso!";
            return this.RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = this._mapper.Map<ProdutoViewModel>(await this._produtoRepository.ObterProdutoFornecedor(id));
            produto.Fornecedores = this._mapper.Map<IEnumerable<FornecedorViewModel>>(await this._fornecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = this._mapper.Map<IEnumerable<FornecedorViewModel>>(await this._fornecedorRepository.ObterTodos());
            return produto;
        }
    }
}