using BibliotecaMVC.Controllers;
using BibliotecaMVC.Views;

class Program
{
    static void Main(string[] args)
    {
        var controller = new BibliotecaController();

        while (true)
        {
            MenuView.MostrarMenuPrincipal();
            Console.Write("Seleccione una opción: ");
            var opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1": controller.RegistrarMaterial(); break;
                case "2": controller.RegistrarPersona(); break;
                case "3": controller.EliminarPersona(); break;
                case "4": controller.RegistrarPrestamo(); break;
                case "5": controller.RegistrarDevolucion(); break;
                case "6": controller.IncrementarCantidadMaterial(); break;
                case "7": controller.MostrarHistorial(); break;
                case "0": return;
                default: Console.WriteLine("Opción inválida."); break;
            }
        }
    }
}
