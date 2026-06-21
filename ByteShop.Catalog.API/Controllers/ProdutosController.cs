using ByteShop.Catalog.Application.Dtos;
using ByteShop.Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByteShop.Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;

        public ProdutosController(IProdutoAppService produtoAppService)
        {
            _produtoAppService = produtoAppService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProdutoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            var produtos = await _produtoAppService.ObterTodosAsync();
            return Ok(produtos);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var produto = await _produtoAppService.ObterPorIdAsync(id);

            if (produto == null)
                return NotFound(new { mensagem = "Produto não encontrado." });

            return Ok(produto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] CriarProdutoDto criarProdutoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _produtoAppService.AdicionarAsync(criarProdutoDto);

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}