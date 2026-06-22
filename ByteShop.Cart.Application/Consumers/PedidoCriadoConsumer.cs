using ByteShop.BuildingBlocks.EventBus.Events;
using ByteShop.Cart.Domain.Interfaces;
using MassTransit;

namespace ByteShop.Cart.Application.Consumers
{
  
    public class PedidoCriadoConsumer : IConsumer<PedidoCriadoIntegrationEvent>
    {
        private readonly ICarrinhoRepository _carrinhoRepository;

        public PedidoCriadoConsumer(ICarrinhoRepository carrinhoRepository)
        {
            _carrinhoRepository = carrinhoRepository;
        }

        public async Task Consume(ConsumeContext<PedidoCriadoIntegrationEvent> context)
        {
      
            var clienteId = context.Message.ClienteId;

            await _carrinhoRepository.LimparCarrinhoAsync(clienteId);
        }
    }
}