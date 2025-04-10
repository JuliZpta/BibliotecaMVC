using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers {
    public class BibliotecaController {
        private List<Material> materiales = new();
        private List<Persona> personas = new();
        private List<Movimiento> historial = new();

        public void RegistrarMaterial() {
            Console.Write("ID del material: ");
            string id = Console.ReadLine();
            if (materiales.Any(m => m.Id == id)) {
                Console.WriteLine("Ya existe un material con ese ID.");
                return;
            }

            Console.Write("Título del material: ");
            string titulo = Console.ReadLine();

            Console.Write("Cantidad registrada: ");
            int cantidad = int.Parse(Console.ReadLine());

            Material nuevo = new() {
                Id = id,
                Titulo = titulo,
                FechaRegistro = DateTime.Now,
                CantidadRegistrada = cantidad,
                CantidadActual = cantidad
            };

            materiales.Add(nuevo);
            Console.WriteLine("Material registrado con éxito.");
        }

        public void RegistrarPersona() {
            Console.Write("Cédula: ");
            string cedula = Console.ReadLine();
            if (personas.Any(p => p.Cedula == cedula)) {
                Console.WriteLine("Ya existe una persona con esa cédula.");
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Rol (Estudiante, Profesor, Administrativo): ");
            string rol = Console.ReadLine();

            personas.Add(new Persona {
                Nombre = nombre,
                Cedula = cedula,
                Rol = rol
            });

            Console.WriteLine("Persona registrada con éxito.");
        }

        public void EliminarPersona() {
            Console.Write("Cédula de la persona a eliminar: ");
            string cedula = Console.ReadLine();

            var persona = personas.FirstOrDefault(p => p.Cedula == cedula);
            if (persona == null) {
                Console.WriteLine("Persona no encontrada.");
                return;
            }

            if (persona.PrestamosActuales.Count > 0) {
                Console.WriteLine("No se puede eliminar la persona porque tiene materiales prestados.");
                return;
            }

            personas.Remove(persona);
            Console.WriteLine("Persona eliminada con éxito.");
        }

        public void RegistrarPrestamo() {
            Console.Write("Cédula de la persona: ");
            string cedula = Console.ReadLine();

            var persona = personas.FirstOrDefault(p => p.Cedula == cedula);
            if (persona == null) {
                Console.WriteLine("Persona no registrada.");
                return;
            }

            if (persona.PrestamosActuales.Count >= persona.LimitePrestamos()) {
                Console.WriteLine("Límite de préstamos alcanzado.");
                return;
            }

            Console.Write("ID del material: ");
            string materialId = Console.ReadLine();

            var material = materiales.FirstOrDefault(m => m.Id == materialId);
            if (material == null || material.CantidadActual <= 0) {
                Console.WriteLine("Material no disponible.");
                return;
            }

            material.CantidadActual--;
            persona.PrestamosActuales.Add(material.Id);

            historial.Add(new Movimiento {
                Tipo = "Préstamo",
                MaterialId = material.Id,
                TituloMaterial = material.Titulo,
                CedulaPersona = persona.Cedula,
                NombrePersona = persona.Nombre,
                Fecha = DateTime.Now
            });

            Console.WriteLine("Préstamo registrado.");
            Console.WriteLine($"Cantidad disponible de '{material.Titulo}': {material.CantidadActual}");

        }

        public void RegistrarDevolucion() {
            Console.Write("Cédula de la persona: ");
            string cedula = Console.ReadLine();

            var persona = personas.FirstOrDefault(p => p.Cedula == cedula);
            if (persona == null) {
                Console.WriteLine("Persona no registrada.");
                return;
            }

            Console.Write("ID del material a devolver: ");
            string materialId = Console.ReadLine();

            if (!persona.PrestamosActuales.Contains(materialId)) {
                Console.WriteLine("La persona no tiene este material prestado.");
                return;
            }

            var material = materiales.FirstOrDefault(m => m.Id == materialId);
            if (material != null) {
                material.CantidadActual++;
            }

            persona.PrestamosActuales.Remove(materialId);

            historial.Add(new Movimiento {
                Tipo = "Devolución",
                MaterialId = materialId,
                TituloMaterial = material?.Titulo ?? "Desconocido",
                CedulaPersona = persona.Cedula,
                NombrePersona = persona.Nombre,
                Fecha = DateTime.Now
            });

            Console.WriteLine("Devolución registrada.");
            Console.WriteLine($"Cantidad disponible de '{material.Titulo}': {material.CantidadActual}");

        }

        public void IncrementarCantidadMaterial() {
            Console.Write("ID del material: ");
            string id = Console.ReadLine();

            var material = materiales.FirstOrDefault(m => m.Id == id);
            if (material == null) {
                Console.WriteLine("Material no encontrado.");
                return;
            }

            Console.Write("Cantidad a agregar: ");
            int cantidad = int.Parse(Console.ReadLine());

            material.CantidadRegistrada += cantidad;
            material.CantidadActual += cantidad;

            Console.WriteLine("Cantidad actualizada.");
        }

        public void MostrarHistorial() {
            Console.WriteLine("\n=== Historial de Movimientos ===");
            foreach (var m in historial) {
                Console.WriteLine($"{m.Fecha}: {m.Tipo} - {m.TituloMaterial} por {m.NombrePersona} ({m.CedulaPersona})");
            }
        }
    }
}
