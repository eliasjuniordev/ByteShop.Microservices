using System.Net;
using FluentAssertions;
using Xunit;

namespace ByteShop.Cart.Tests.IntegrationTests.Controllers;

// IClassFixture injeta a nossa fábrica, garantindo que a API e o Redis só sobem 1x por classe de teste
public class CarrinhoControllerTests : IClassFixture<CartApiFactory>
{
    private readonly HttpClient _client;

    public CarrinhoControllerTests(CartApiFactory factory)
    {
       
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "Deve retornar carrinho vazio ao consultar cliente sem itens")]
    [Trait("Categoria", "Integração - Carrinho API")]
    public async Task ObterCarrinho_ClienteNovo_DeveRetornarCarrinhoVazio()
    {
     
        var clienteId = "cliente_teste_integracao@teste.com";

  
        var response = await _client.GetAsync($"/api/v1/Carrinho/{clienteId}");

     
        var jsonResult = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"💥 EXPLOSÃO INTERNA DA API (Status {response.StatusCode}):\n{jsonResult}");
        }

   
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        jsonResult.Should().Contain(clienteId);
        jsonResult.Should().Contain("\"itens\":[]");
    }
}