using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BibliotecaMVC.Models {
    public class Material {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int CantidadRegistrada { get; set; }
        public int CantidadActual { get; set; }
    }
}
