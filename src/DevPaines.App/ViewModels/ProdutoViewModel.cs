using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace DevPaines.App.ViewModels
{
    public class ProdutoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Fornecedor")]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter {1} caracteres.", MinimumLength = 2)]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter {1} caracteres.", MinimumLength = 2)]
        public string Descricao { get; set; }

        [DisplayName("Imagem do Produto")]
        public IFormFile ImagemUpload { get; set; }

        public string Imagem { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Valor { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }

        [DisplayName("Ativo ?")]
        public bool Ativo { get; set; }

        public FornecedorViewModel Fornecedor { get; set; }

        public IEnumerable<FornecedorViewModel> Fornecedores { get; set; }
    }

    public static class ProdutoViewModelExtension
    {
        public static async Task<bool> UploadImagemAsync(this ProdutoViewModel produtoViewModel)
        {
            if (produtoViewModel.ImagemUpload.Length <= 0)
                return false;

            var imgPrefixo = $"{Guid.NewGuid()}_";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + produtoViewModel.ImagemUpload.FileName);

            if (File.Exists(path))
                return false;

            using (var stream = new FileStream(path, FileMode.Create))
                await produtoViewModel.ImagemUpload.CopyToAsync(stream);

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            return true;
        }

        public static void EditModel(this ProdutoViewModel produtoViewModel, ProdutoViewModel produtoViewModelAtualizacao)
        {
            produtoViewModel.Nome = produtoViewModelAtualizacao.Nome;
            produtoViewModel.Descricao = produtoViewModelAtualizacao.Descricao;
            produtoViewModel.Valor = produtoViewModelAtualizacao.Valor;
            produtoViewModel.Ativo = produtoViewModelAtualizacao.Ativo;

            if (!string.IsNullOrWhiteSpace(produtoViewModelAtualizacao.Imagem))
                produtoViewModel.Imagem = produtoViewModelAtualizacao.Imagem;
        }
    }
}
