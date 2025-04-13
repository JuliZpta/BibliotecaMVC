using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaMVC.Models;
using MongoDB.Driver;

namespace BibliotecaMVC.Controllers
{
    public class BibliotecaController
    {
        private readonly MongoContext _context;

        public BibliotecaController()
        {
            _context = new MongoContext();
        }

        public void RegistrarMaterial()
        {
            Console.Write("Título del material: ");
            string titulo = Console.ReadLine();
            Console.Write("Cantidad a registrar: ");
            int cantidad = int.Parse(Console.ReadLine());

            var totalMateriales = _context.Materiales.CountDocuments(_ => true);
            var nuevoId = $"MAT{(totalMateriales + 1).ToString("D3")}";

            var material = new Material
            {
                Id = nuevoId,
                Titulo = titulo,
                FechaRegistro = DateTime.Now,
                CantidadRegistrada = cantidad,
                CantidadActual = cantidad
            };

            _context.Materiales.InsertOne(material);
            Console.WriteLine($"Material registrado con ID: {material.Id}");
        }

        public void RegistrarPersona()
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();
            Console.Write("Cédula: ");
            string cedula = Console.ReadLine();
            Console.Write("Rol (Estudiante, Profesor, Administrativo): ");
            string rol = Console.ReadLine();

            var existente = _context.Personas.Find(p => p.Cedula == cedula).FirstOrDefault();
            if (existente != null)
            {
                Console.WriteLine("Ya existe una persona con esa cédula.");
                return;
            }

            var persona = new Persona
            {
                Nombre = nombre,
                Cedula = cedula,
                Rol = rol
            };

            _context.Personas.InsertOne(persona);
            Console.WriteLine("Persona registrada correctamente.");
        }

        public void EliminarPersona()
        {
            Console.Write("Cédula de la persona a eliminar: ");
            string cedula = Console.ReadLine();

            var persona = _context.Personas.Find(p => p.Cedula == cedula).FirstOrDefault();
            if (persona == null)
            {
                Console.WriteLine("Persona no encontrada.");
                return;
            }

            if (persona.PrestamosActuales != null && persona.PrestamosActuales.Count > 0)
            {
                Console.WriteLine("No se puede eliminar. Tiene préstamos activos.");
                return;
            }

            _context.Personas.DeleteOne(p => p.Cedula == cedula);
            Console.WriteLine("Persona eliminada.");
        }

        public void RegistrarPrestamo()
        {
            Console.Write("Cédula de la persona: ");
            string cedula = Console.ReadLine();
            Console.Write("ID del material a prestar: ");
            string idMaterial = Console.ReadLine();

            var persona = _context.Personas.Find(p => p.Cedula == cedula).FirstOrDefault();
            if (persona == null)
            {
                Console.WriteLine("❌ Persona no registrada.");
                return;
            }

            if (persona.PrestamosActuales.Count >= persona.LimitePrestamos())
            {
                Console.WriteLine("Límite de préstamos alcanzado.");
                return;
            }

            var material = _context.Materiales.Find(m => m.Id == idMaterial).FirstOrDefault();
            if (material == null || material.CantidadActual <= 0)
            {
                Console.WriteLine("Material no disponible.");
                return;
            }

            material.CantidadActual--;
            persona.PrestamosActuales.Add(material.Id);

            _context.Materiales.ReplaceOne(m => m.Id == material.Id, material);
            _context.Personas.ReplaceOne(p => p.Cedula == persona.Cedula, persona);

            var movimiento = new Movimiento
            {
                Tipo = "Préstamo",
                MaterialId = material.Id,
                Fecha = DateTime.Now,
                Cantidad = 1,
                CedulaPersona = persona.Cedula,
                NombrePersona = persona.Nombre
            };
            _context.Movimientos.InsertOne(movimiento);

            Console.WriteLine("Préstamo registrado.");
        }

        public void RegistrarDevolucion()
        {
            Console.Write("Cédula de la persona: ");
            string cedula = Console.ReadLine();
            Console.Write("ID del material a devolver: ");
            string idMaterial = Console.ReadLine();

            var persona = _context.Personas.Find(p => p.Cedula == cedula).FirstOrDefault();
            if (persona == null)
            {
                Console.WriteLine("Persona no registrada.");
                return;
            }

            if (!persona.PrestamosActuales.Contains(idMaterial))
            {
                Console.WriteLine("La persona no tiene ese material prestado.");
                return;
            }

            var material = _context.Materiales.Find(m => m.Id == idMaterial).FirstOrDefault();
            if (material != null)
            {
                material.CantidadActual++;
                _context.Materiales.ReplaceOne(m => m.Id == material.Id, material);
            }

            persona.PrestamosActuales.Remove(idMaterial);
            _context.Personas.ReplaceOne(p => p.Cedula == persona.Cedula, persona);

            var movimiento = new Movimiento
            {
                Tipo = "Devolución",
                MaterialId = idMaterial,
                Fecha = DateTime.Now,
                Cantidad = 1,
                CedulaPersona = persona.Cedula,
                NombrePersona = persona.Nombre
            };
            _context.Movimientos.InsertOne(movimiento);

            Console.WriteLine("Devolución registrada.");
        }

        public void IncrementarCantidadMaterial()
        {
            Console.Write("ID del material a incrementar: ");
            string idMaterial = Console.ReadLine();
            Console.Write("Cantidad a agregar: ");
            int extra = int.Parse(Console.ReadLine());

            var material = _context.Materiales.Find(m => m.Id == idMaterial).FirstOrDefault();
            if (material == null)
            {
                Console.WriteLine("Material no encontrado.");
                return;
            }

            material.CantidadRegistrada += extra;
            material.CantidadActual += extra;

            _context.Materiales.ReplaceOne(m => m.Id == material.Id, material);
            Console.WriteLine("Cantidad actualizada.");
        }

        public void MostrarHistorial()
        {
            var movimientos = _context.Movimientos.Find(_ => true).ToList();

            Console.WriteLine("\nHistorial de movimientos:");
            foreach (var mov in movimientos)
            {
                Console.WriteLine($"{mov.Tipo} - ID Material: {mov.MaterialId} - Cantidad: {mov.Cantidad} - Fecha: {mov.Fecha} - Persona: {mov.NombrePersona} ({mov.CedulaPersona})");
            }
        }

        public void VerPrestamosDePersona()
        {
            Console.Write("Cédula: ");
            string cedula = Console.ReadLine();

            var persona = _context.Personas.Find(p => p.Cedula == cedula).FirstOrDefault();

            if (persona == null)
            {
                Console.WriteLine("Persona no encontrada.");
                return;
            }

            Console.WriteLine($"{persona.Nombre} ({persona.Cedula}) tiene prestados:");

            if (persona.PrestamosActuales.Count == 0)
            {
                Console.WriteLine("No tiene materiales prestados actualmente.");
                return;
            }

            foreach (var idMaterial in persona.PrestamosActuales)
            {
                var material = _context.Materiales.Find(m => m.Id == idMaterial).FirstOrDefault();
                if (material != null)
                {
                    Console.WriteLine($"{material.Titulo} (ID: {material.Id}) - Disponible: {material.CantidadActual}");
                }
                else
                {
                    Console.WriteLine($"Material con ID {idMaterial} no encontrado.");
                }
            }
        }
    }
}
