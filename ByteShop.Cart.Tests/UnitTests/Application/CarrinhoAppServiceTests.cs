using ByteShop.Cart.Application.Dtos;
using ByteShop.Cart.Application.Services;
using ByteShop.Cart.Domain.Entities;
using ByteShop.Cart.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ByteShop.Cart.Tests.UnitTests.Application;

public class CarrinhoAppServiceTests
{
    [Fact(DisplayName = "Deve adicionar um novo item ao carrinho corretamente")]
    [Trait("Categoria", "Unidade - Serviços de Carrinho")]
    public async Task AdicionarItem_CarrinhoValido_DeveSalvarERetornarDto()
    {
       
        var clienteId = "elias@teste.com";
        var carrinhoExistente = new CarrinhoCompra(clienteId); 

      
        var novoItemDto = new AdicionarItemCarrinhoDto
        {
            ProdutoId = Guid.NewGuid(),
            ProdutoNome = "Teclado Mecânico",
            PrecoUnitario = 150.00m,
            Quantidade = 2,
            ImagemUrl = "teclado.png"
        };

        var carrinhoRepositoryMock = new Mock<ICarrinhoRepository>();

    
        carrinhoRepositoryMock
            .Setup(repo => repo.ObterPorClienteIdAsync(clienteId))
            .ReturnsAsync(carrinhoExistente);

      
        carrinhoRepositoryMock
            .Setup(repo => repo.AtualizarAsync(It.IsAny<CarrinhoCompra>()))
            .ReturnsAsync((CarrinhoCompra c) => c);

        var carrinhoService = new CarrinhoAppService(carrinhoRepositoryMock.Object);

        var resultadoDto = await carrinhoService.AdicionarItemAsync(clienteId, novoItemDto);

        carrinhoRepositoryMock.Verify(repo =>
            repo.AtualizarAsync(It.IsAny<CarrinhoCompra>()), Times.Once);

   
        carrinhoExistente.Itens.Should().HaveCount(1);
        carrinhoExistente.Itens.First().ProdutoNome.Should().Be("Teclado Mecânico");

        resultadoDto.Should().NotBeNull();
    }
}