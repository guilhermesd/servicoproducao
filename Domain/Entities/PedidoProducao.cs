using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum StatusPedido
    {
        EmPreparacao = 1,
        Pronto = 2
    }

    public class PedidoProducao
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid IdProducao { get; private set; }

        [BsonRepresentation(BsonType.String)]
        public Guid IdPedido { get; private set; }

        public StatusPedido StatusPedidoProducao { get; private set; }

        public DateTime DataStatusPedidoProducao { get; private set; }

        public PedidoProducao(Guid idPedido, StatusPedido statusPedido)
        {
            IdPedido = idPedido;
            StatusPedidoProducao = statusPedido;
            DataStatusPedidoProducao = DateTime.Now;
        }
    }
}
