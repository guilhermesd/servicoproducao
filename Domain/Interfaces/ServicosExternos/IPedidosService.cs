using Crosscutting.DTOs;

namespace Domain.Interfaces.ServicosExternos
{
    public interface IPedidosService
    {
        public Task<bool> AlterarStatusPedido(Guid idPedido, AlterarStatusPedidoDTO alterarStatusPedidoDTO);
    }
}