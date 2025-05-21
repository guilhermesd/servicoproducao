using System.Net;
using System.Net.Http.Json;
using Crosscutting.DTOs;
using DotNet.Testcontainers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json;
using Testcontainers.MongoDb;
using TechTalk.SpecFlow;
using Domain.Interfaces.ServicosExternos;
using Infrastructure.ServicosExternos;

namespace Tests.BDD
{
    [Binding]
    public class RegistrarStatusPedidoProducaoSteps : IAsyncLifetime
    {
        private readonly ScenarioContext _scenarioContext;
        private WebApplicationFactory<Program> _factory = null!;
        private HttpClient _client = null!;
        private readonly MongoDbContainer _mongoContainer;
        private RegistraStatusPedidoProducaoDTO _request = null!;
        private HttpResponseMessage _response = null!;

        public RegistrarStatusPedidoProducaoSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _mongoContainer = new MongoDbBuilder()
                .WithImage("mongo:7.0")
                .WithCleanUp(true)
                .WithName($"bdd-producao-test-{Guid.NewGuid()}")
                .WithPortBinding(27017, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017))
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _mongoContainer.StartAsync();

            var connectionString = _mongoContainer.GetConnectionString();

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
                        services.RemoveAll<IPedidosService>();
                        services.AddScoped<IPedidosService, PedidosServiceMock>();
                    });
                });

            _client = _factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            await _mongoContainer.DisposeAsync();
        }

        [Given(@"que tenho um pedido válido com status ""(.*)""")]
        public void GivenQueTenhoUmPedidoValidoComStatus(string status)
        {
            _request = new RegistraStatusPedidoProducaoDTO
            {
                IdPedido = Guid.NewGuid(),
                StatusPedido = Enum.Parse<StatusPedidoProducaoDTO>(status)
            };
        }

        [When(@"envio a requisição para registrar o status de produção")]
        public async Task WhenEnvioARequisicaoParaRegistrarOStatusDeProducao()
        {
            _response = await _client.PostAsJsonAsync("/api/producao/registra-status-pedido", _request);
        }

        [Then(@"a resposta deve retornar status (.*) OK")]
        public void ThenARespostaDeveRetornarStatusOK(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            await InitializeAsync();
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            await DisposeAsync();
        }
    }
}
