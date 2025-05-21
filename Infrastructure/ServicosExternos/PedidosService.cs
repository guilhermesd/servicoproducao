using Crosscutting.DTOs;
using Domain.Interfaces.ServicosExternos;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Infrastructure.ServicosExternos
{
    public class PedidosService : IPedidosService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public PedidosService(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
        }

        public async Task<bool> AlterarStatusPedido(Guid idPedido, AlterarStatusPedidoDTO alterarStatusPedidoDTO)
        {
            var response = await _http.PutAsJsonAsync($"{_configuration["UrlPedido"]}/api/pedidos/{idPedido}/alterar/status-pedido", alterarStatusPedidoDTO);
            response.EnsureSuccessStatusCode();
            return true;
        }
    }

    public class PedidosServiceMock : IPedidosService
    {
        public async Task<bool> AlterarStatusPedido(Guid idPedido, AlterarStatusPedidoDTO alterarStatusPedidoDTO)
        {
            return true;
        }
    }
}