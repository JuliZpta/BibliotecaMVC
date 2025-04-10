namespace BibliotecaMVC.Models {
    public class Persona {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Rol { get; set; }
        public List<string> PrestamosActuales { get; set; } = new();

        public int LimitePrestamos() {
            return Rol switch {
                "Estudiante" => 5,
                "Profesor" => 3,
                "Administrativo" => 1,
                _ => 0
            };
        }
    }
}
