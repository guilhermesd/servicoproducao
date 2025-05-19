using Domain.Entities;
using FluentAssertions;

namespace Tests.Unidade.Entities
{
    public class PedidoProducaoTests
    {
        [Fact(DisplayName = "Deve criar um PedidoProducao com status e data corretamente")]
        public void DeveCriarPedidoProducao_Corretamente()
        {
            // Arrange
            var idPedido = Guid.NewGuid();
            var status = StatusPedido.EmPreparacao;
            var antesCriacao = DateTime.Now;

            // Act
            var pedido = new PedidoProducao(idPedido, status);

            // Assert
            pedido.IdPedido.Should().Be(idPedido);
            pedido.StatusPedidoProducao.Should().Be(status);
            pedido.DataStatusPedidoProducao.Should().BeAfter(antesCriacao).And.BeBefore(DateTime.Now.AddSeconds(1));
        }
    }
}
