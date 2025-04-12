using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BibliotecaMVC.Models
{
    public class Persona
    {
        [BsonId] // Cedula como _id
        [BsonRepresentation(BsonType.String)] // MongoDB la trata como string, no ObjectId
        public string Cedula { get; set; }

        public string Nombre { get; set; }
        public string Rol { get; set; }
        public List<string> PrestamosActuales { get; set; } = new();

        public int LimitePrestamos()
        {
            return Rol switch
            {
                "Estudiante" => 5,
                "Profesor" => 3,
                "Administrativo" => 1,
                _ => 0
            };
        }
    }
}
