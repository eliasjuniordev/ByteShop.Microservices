using ByteShop.Cart.Application.Dtos;
using ByteShop.Cart.Application.Interfaces;
using ByteShop.Cart.Domain.Entities;
using ByteShop.Cart.Domain.Interfaces;

namespace ByteShop.Cart.Application.Services
{
    public class CarrinhoAppService : ICarrinhoAppService
    {
        private readonly ICarrinhoRepository _carrinhoRepository;

        public CarrinhoAppService(ICarrinhoRepository carrinhoRepository)
        {
            _carrinhoRepository = carrinhoRepository;
        }

        public async Task<CarrinhoCompraDto?> ObterPorClienteIdAsync(string clienteId)
        {
            var carrinho = await _carrinhoRepository.ObterPorClienteIdAsync(clienteId);
            return carrinho == null ? null : MapearParaDto(carrinho);
        }

        public async Task<CarrinhoCompraDto> AdicionarItemAsync(string clienteId, AdicionarItemCarrinhoDto itemDto)
        {
            
            var carrinho = await _carrinhoRepository.ObterPorClienteIdAsync(clienteId)
                           ?? new CarrinhoCompra(clienteId);

           
            var itemExistente = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == itemDto.ProdutoId);

            int novaQuantidade = itemDto.Quantidade;
            if (itemExistente != null)
            {
                novaQuantidade += itemExistente.Quantidade;
                carrinho.Itens.Remove(itemExistente);
            }

         
            var novoItem = new CarrinhoCompraItem(
                itemDto.ProdutoId,
                itemDto.ProdutoNome,
                itemDto.PrecoUnitario,
                novaQuantidade,
                itemDto.ImagemUrl
            );

            carrinho.Itens.Add(novoItem);

       
            var carrinhoAtualizado = await _carrinhoRepository.AtualizarAsync(carrinho);

            return MapearParaDto(carrinhoAtualizado);
        }

        public async Task<CarrinhoCompraDto?> RemoverItemAsync(string clienteId, Guid produtoId)
        {
            var carrinho = await _carrinhoRepository.ObterPorClienteIdAsync(clienteId);
            if (carrinho == null) return null;

            var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item != null)
            {
                carrinho.Itens.Remove(item);
                await _carrinhoRepository.AtualizarAsync(carrinho);
            }

            return MapearParaDto(carrinho);
        }

        public async Task LimparCarrinhoAsync(string clienteId)
        {
            await _carrinhoRepository.LimparCarrinhoAsync(clienteId);
        }


        private static CarrinhoCompraDto MapearParaDto(CarrinhoCompra carrinho)
        {
            return new CarrinhoCompraDto
            {
                ClienteId = carrinho.ClienteId,
                PrecoTotal = carrinho.PrecoTotal,
                Itens = carrinho.Itens.Select(i => new CarrinhoCompraItemDto
                {
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.ProdutoNome,
                    PrecoUnitario = i.PrecoUnitario,
                    Quantidade = i.Quantidade,
                    ImagemUrl = i.ImagemUrl
                }).ToList()
            };
        }
    }
}