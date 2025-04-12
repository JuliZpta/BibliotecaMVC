using BibliotecaMVC.Controllers;

class Program
{
    static void Main(string[] args)
    {
        var controller = new BibliotecaController();
        bool salir = false;

        while (!salir)
        {
            Console.WriteLine("\n===== Menú Biblioteca (MongoDB) =====");
            Console.WriteLine("1. Registrar material");
            Console.WriteLine("2. Registrar persona");
            Console.WriteLine("3. Eliminar persona");
            Console.WriteLine("4. Registrar préstamo");
            Console.WriteLine("5. Registrar devolución");
            Console.WriteLine("6. Incrementar cantidad de material");
            Console.WriteLine("7. Consultar historial de la biblioteca");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Título del material: ");
                    string titulo = Console.ReadLine();
                    Console.Write("Cantidad a registrar: ");
                    int cantidad = int.Parse(Console.ReadLine());
                    controller.RegistrarMaterial(titulo, cantidad);
                    break;

                case "2":
                    Console.Write("Nombre: ");
                    string nombre = Console.ReadLine();
                    Console.Write("Cédula: ");
                    string cedula = Console.ReadLine();
                    Console.Write("Rol (Estudiante, Profesor, Administrativo): ");
                    string rol = Console.ReadLine();
                    controller.RegistrarPersona(nombre, cedula, rol);
                    break;

                case "3":
                    Console.Write("Cédula de la persona a eliminar: ");
                    string cedEliminar = Console.ReadLine();
                    controller.EliminarPersona(cedEliminar);
                    break;

                case "4":
                    Console.Write("Cédula de la persona: ");
                    string cedPrestamo = Console.ReadLine();
                    Console.Write("ID del material a prestar: ");
                    string idPrestamo = Console.ReadLine();
                    controller.RegistrarPrestamo(cedPrestamo, idPrestamo);
                    break;

                case "5":
                    Console.Write("Cédula de la persona: ");
                    string cedDevolucion = Console.ReadLine();
                    Console.Write("ID del material a devolver: ");
                    string idDevolucion = Console.ReadLine();
                    controller.RegistrarDevolucion(cedDevolucion, idDevolucion);
                    break;

                case "6":
                    Console.Write("ID del material a incrementar: ");
                    string idIncremento = Console.ReadLine();
                    Console.Write("Cantidad a agregar: ");
                    int extra = int.Parse(Console.ReadLine());
                    controller.IncrementarCantidadMaterial(idIncremento, extra);
                    break;

                case "7":
                    controller.MostrarHistorial();
                    break;

                case "0":
                    salir = true;
                    Console.WriteLine("👋 Cerrando programa...");
                    break;

                default:
                    Console.WriteLine("❌ Opción inválida.");
                    break;
            }
        }
    }
}
