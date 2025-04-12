using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BibliotecaMVC.Models {
    public class Movimiento {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public string MaterialId { get; set; }

        public string Tipo { get; set; }
        public string TituloMaterial { get; set; }
        public string CedulaPersona { get; set; }
        public string NombrePersona { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}
