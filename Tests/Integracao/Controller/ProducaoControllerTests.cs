using Crosscutting.DTOs;
using Domain.Interfaces.ServicosExternos;
using DotNet.Testcontainers.Builders;
using FluentAssertions;
using Infrastructure.ServicosExternos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Json;
using Testcontainers.MongoDb;

namespace Tests.Integracao.Controller
{
    public class ProducaoControllerTests : IAsyncLifetime
    {
        private readonly MongoDbContainer _mongoContainer;
        private WebApplicationFactory<Program> _factory = null!;
        private HttpClient _client = null!;

        public ProducaoControllerTests()
        {
            _mongoContainer = new MongoDbBuilder()
                .WithImage("mongo:7.0")
                .WithCleanUp(true)
                .WithName($"mongo-pedidos-test{Guid.NewGuid()}")
                .WithPortBinding(27017, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017))
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _mongoContainer.StartAsync();

            var connectionString = _mongoContainer.GetConnectionString(); // exemplo: "mongodb://localhost:32776"

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        var inMemorySettings = new Dictionary<string, string>
                        {
                            ["MongoDB:ConnectionString"] = connectionString,
                            ["MongoDB:DatabaseName"] = "TestDb"
                        };

                        configBuilder.AddInMemoryCollection(inMemorySettings);
                    });

                    builder.ConfigureServices(services =>
                    {
                        // Se necessário, substitua repositórios ou services
                        services.RemoveAll<IPedidosService>(); // Garante que o real não está registrado
                        services.AddScoped<IPedidosService, PedidosServiceMock>();

                    });
                });

            _client = _factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            await _mongoContainer.DisposeAsync();
        }

        [Fact]
        public async Task Post_RegistrarStatusPedidoProducao_DeveCriarPedidoProducao()
        {
            RegistraStatusPedidoProducaoDTO request = new()
            {
                IdPedido = Guid.NewGuid(),
                StatusPedido = StatusPedidoProducaoDTO.EmPreparacao
            };
            var response = await _client.PostAsJsonAsync("/api/producao/registra-status-pedido", request);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

    }
}
