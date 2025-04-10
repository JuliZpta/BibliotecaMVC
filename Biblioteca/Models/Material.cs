namespace BibliotecaMVC.Models {
    public class Material {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int CantidadRegistrada { get; set; }
        public int CantidadActual { get; set; }
    }
}
