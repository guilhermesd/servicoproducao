using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IProducaoRepository
    {
        Task AddAsync(PedidoProducao pedidoProducao);
    }
}
