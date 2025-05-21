using Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public interface IProducaoContext
    {
        IMongoCollection<PedidoProducao> PedidosProducao { get; }
    }

    public class ProducaoContext : IProducaoContext
    {
        private readonly IMongoDatabase _database;

        public ProducaoContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        }

        public IMongoCollection<PedidoProducao> PedidosProducao => _database.GetCollection<PedidoProducao>("PedidosProducao");
    }
}