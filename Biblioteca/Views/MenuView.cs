namespace BibliotecaMVC.Views {
    public static class MenuView {
        public static void MostrarMenuPrincipal() {
            Console.WriteLine("\n=== Menú Biblioteca ===");
            Console.WriteLine("1. Registrar material");
            Console.WriteLine("2. Registrar persona");
            Console.WriteLine("3. Eliminar persona");
            Console.WriteLine("4. Registrar préstamo");
            Console.WriteLine("5. Registrar devolución");
            Console.WriteLine("6. Incrementar cantidad de material");
            Console.WriteLine("7. Consultar historial");
            Console.WriteLine("0. Salir");
        }
    }
}
