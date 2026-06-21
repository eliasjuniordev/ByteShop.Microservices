using ByteShop.BuildingBlocks.EventBus.Events; 
using ByteShop.Ordering.Application.Dtos;
using ByteShop.Ordering.Application.Interfaces;
using ByteShop.Ordering.Domain.Entities;
using ByteShop.Ordering.Domain.Interfaces;
using MassTransit; 

namespace ByteShop.Ordering.Application.Services
{
    public class PedidoAppService : IPedidoAppService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPublishEndpoint _publishEndpoint; 

        public PedidoAppService(IPedidoRepository pedidoRepository, IPublishEndpoint publishEndpoint)
        {
            _pedidoRepository = pedidoRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<PedidoDto?> ObterPorIdAsync(Guid id)
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(id);
            return pedido == null ? null : MapearParaDto(pedido);
        }

        public async Task<IEnumerable<PedidoDto>> ObterPorClienteIdAsync(string clienteId)
        {
            var pedidos = await _pedidoRepository.ObterPorClienteIdAsync(clienteId);
            return pedidos.Select(MapearParaDto);
        }

        public async Task<PedidoDto> CriarPedidoAsync(CriarPedidoDto dto)
        {
            var itensDominio = dto.Itens.Select(item =>
                new PedidoItem(item.ProdutoId, item.ProdutoNome, item.PrecoUnitario, item.Quantidade)
            ).ToList();

            var pedido = new Pedido(dto.ClienteId, dto.EnderecoEntrega, itensDominio);

            // 1. Salva no SQL Server
            await _pedidoRepository.AdicionarAsync(pedido);

            // 2. DISPARA O EVENTO PARA O RABBITMQ
            // O MassTransit vai serializar esse record em JSON e mandar para o broker
            var evento = new PedidoCriadoIntegrationEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal);
            await _publishEndpoint.Publish(evento);

            return MapearParaDto(pedido);
        }

        private static PedidoDto MapearParaDto(Pedido pedido)
        {
            return new PedidoDto
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                ValorTotal = pedido.ValorTotal,
                DataPedido = pedido.DataPedido,
                Status = pedido.Status.ToString(),
                EnderecoEntrega = pedido.EnderecoEntrega,
                Itens = pedido.Itens.Select(i => new PedidoItemDto
                {
                    Id = i.Id,
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.ProdutoNome,
                    PrecoUnitario = i.PrecoUnitario,
                    Quantidade = i.Quantidade
                }).ToList()
            };
        }
    }
}