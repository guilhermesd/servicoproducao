using Crosscutting.DTOs;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.ServicosExternos;

namespace Application.UseCases
{
    public interface IPedidoProducaoUseCase
    {
        Task<bool> RegistrarStatusPedidoProducao(RegistraStatusPedidoProducaoDTO registraStatus);
    }

    public class PedidoProducaoUseCase : IPedidoProducaoUseCase
    {
        private IProducaoRepository _producaoRepository;
        private IPedidosService _pedidosService;

        public PedidoProducaoUseCase(IProducaoRepository producaoRepository, IPedidosService pedidosService)
        {
            _producaoRepository = producaoRepository;
            _pedidosService = pedidosService;
        }

        public async Task<bool> RegistrarStatusPedidoProducao(RegistraStatusPedidoProducaoDTO registraStatus)
        {
            var pedidoProducao = new PedidoProducao(registraStatus.IdPedido, (StatusPedido)registraStatus.StatusPedido);

            await _pedidosService.AlterarStatusPedido(registraStatus.IdPedido, new AlterarStatusPedidoDTO
            {
                Status = registraStatus.StatusPedido
            });

            await _producaoRepository.AddAsync(pedidoProducao);

            return true;
        }
    }
}
