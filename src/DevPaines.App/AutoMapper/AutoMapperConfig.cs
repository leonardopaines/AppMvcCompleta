using AppMvcBasica.Models;
using AutoMapper;
using DevPaines.App.ViewModels;

namespace DevPaines.App.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            this.CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();

            this.CreateMap<Endereco, EnderecoViewModel>().ReverseMap();

            this.CreateMap<Produto, ProdutoViewModel>().ReverseMap();
        }
    }
}
