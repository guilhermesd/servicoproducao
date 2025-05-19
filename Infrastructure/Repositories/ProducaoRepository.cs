using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class ProducaoRepository : IProducaoRepository
    {
        private readonly IProducaoContext _context;

        public ProducaoRepository(IProducaoContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PedidoProducao pedidoProducao)
        {
            await _context.PedidosProducao.InsertOneAsync(pedidoProducao);
        }

    }
}