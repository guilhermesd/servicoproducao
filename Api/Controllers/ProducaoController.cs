using Application.UseCases;
using Crosscutting.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/producao")]
    public class ProducaoController : ControllerBase
    {
        private readonly IPedidoProducaoUseCase _pedidoProducaoUseCase;

        public ProducaoController(IPedidoProducaoUseCase pedidoProducaoUseCase)
        {
            _pedidoProducaoUseCase = pedidoProducaoUseCase;
        }

        /// <summary>
        ///  Registra status do pedido
        /// </summary>
        [HttpPost("registra-status-pedido")]
        public async Task<IActionResult> RegistrarStatusPedidoProducao([FromBody] RegistraStatusPedidoProducaoDTO registraStatus)
        {
            await _pedidoProducaoUseCase.RegistrarStatusPedidoProducao(registraStatus);
            return Ok();
        }
    }
}
