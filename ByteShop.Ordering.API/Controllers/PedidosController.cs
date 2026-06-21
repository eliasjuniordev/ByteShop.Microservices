using ByteShop.Ordering.Application.Dtos;
using ByteShop.Ordering.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByteShop.Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoAppService _pedidoAppService;

        public PedidosController(IPedidoAppService pedidoAppService)
        {
            _pedidoAppService = pedidoAppService;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var pedido = await _pedidoAppService.ObterPorIdAsync(id);
            if (pedido == null) return NotFound(new { mensagem = "Pedido não encontrado." });

            return Ok(pedido);
        }

        [HttpGet("cliente/{clienteId}")]
        [ProducesResponseType(typeof(IEnumerable<PedidoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorCliente(string clienteId)
        {
            var pedidos = await _pedidoAppService.ObterPorClienteIdAsync(clienteId);
            return Ok(pedidos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoDto criarPedidoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var pedido = await _pedidoAppService.CriarPedidoAsync(criarPedidoDto);
                return CreatedAtAction(nameof(ObterPorId), new { id = pedido.Id }, pedido);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}