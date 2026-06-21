using ByteShop.Cart.Application.Dtos;
using ByteShop.Cart.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByteShop.Cart.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarrinhoController : ControllerBase
    {
        private readonly ICarrinhoAppService _carrinhoAppService;

        public CarrinhoController(ICarrinhoAppService carrinhoAppService)
        {
            _carrinhoAppService = carrinhoAppService;
        }

        [HttpGet("{clienteId}")]
        [ProducesResponseType(typeof(CarrinhoCompraDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorClienteId(string clienteId)
        {
            var carrinho = await _carrinhoAppService.ObterPorClienteIdAsync(clienteId);

 
            return Ok(carrinho ?? new CarrinhoCompraDto { ClienteId = clienteId });
        }

        [HttpPost("{clienteId}")]
        [ProducesResponseType(typeof(CarrinhoCompraDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> AdicionarItem(string clienteId, [FromBody] AdicionarItemCarrinhoDto itemDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var carrinho = await _carrinhoAppService.AdicionarItemAsync(clienteId, itemDto);
            return Ok(carrinho);
        }

        [HttpDelete("{clienteId}/produtos/{produtoId:guid}")]
        [ProducesResponseType(typeof(CarrinhoCompraDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoverItem(string clienteId, Guid produtoId)
        {
            var carrinho = await _carrinhoAppService.RemoverItemAsync(clienteId, produtoId);
            return Ok(carrinho);
        }

        [HttpDelete("{clienteId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Limpar(string clienteId)
        {
            await _carrinhoAppService.LimparCarrinhoAsync(clienteId);
            return NoContent();
        }
    }
}