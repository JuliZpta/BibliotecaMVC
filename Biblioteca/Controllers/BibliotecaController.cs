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

        public void RegistrarMaterial(string titulo, int cantidad)
        {
            // Obtener la cantidad de materiales existentes
            var totalMateriales = _context.Materiales.CountDocuments(_ => true);

            // Crear un ID personalizado: "MAT001", "MAT002", etc.
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

            Console.WriteLine($"✅ Material registrado con ID: {material.Id}");
        }


        public void RegistrarPersona(string nombre, string cedula, string rol)
        {
            var existente = _context.Personas.Find(p => p.Cedula == cedula).FirstOrDefault();
            if (existente != null)
            {
                Console.WriteLine("⚠️ Ya existe una persona con esa cédula.");
                return;
            }

            var persona = new Persona
            {
                Nombre = nombre,
                Cedula = cedula,
                Rol = rol
            };

            _context.Personas.InsertOne(persona);
            Console.WriteLine("✅ Persona registrada correctamente.");
        }

        public void EliminarPersona(string cedula)
        {
            var persona = _context.Personas.Find(p => p.Cedula == cedula).FirstOrDefault();
            if (persona == null)
            {
                Console.WriteLine("❌ Persona no encontrada.");
                return;
            }

            if (persona.PrestamosActuales != null && persona.PrestamosActuales.Count > 0)
            {
                Console.WriteLine("⚠️ No se puede eliminar. Tiene préstamos activos.");
                return;
            }

            _context.Personas.DeleteOne(p => p.Cedula == cedula);
            Console.WriteLine("✅ Persona eliminada.");
        }

        public void RegistrarPrestamo(string cedula, string idMaterial)
        {
            var persona = _context.Personas.Find(p => p.Cedula == cedula).FirstOrDefault();
            if (persona == null)
            {
                Console.WriteLine("❌ Persona no registrada.");
                return;
            }

            if (persona.PrestamosActuales.Count >= persona.LimitePrestamos())
            {
                Console.WriteLine("⚠️ Límite de préstamos alcanzado.");
                return;
            }

            var material = _context.Materiales.Find(m => m.Id == idMaterial).FirstOrDefault();
            if (material == null || material.CantidadActual <= 0)
            {
                Console.WriteLine("❌ Material no disponible.");
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
                Cantidad = 1
            };
            _context.Movimientos.InsertOne(movimiento);

            Console.WriteLine("✅ Préstamo registrado.");
        }

        public void RegistrarDevolucion(string cedula, string idMaterial)
        {
            var persona = _context.Personas.Find(p => p.Cedula == cedula).FirstOrDefault();
            if (persona == null)
            {
                Console.WriteLine("❌ Persona no registrada.");
                return;
            }

            if (!persona.PrestamosActuales.Contains(idMaterial))
            {
                Console.WriteLine("⚠️ La persona no tiene ese material prestado.");
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
                Cantidad = 1
            };
            _context.Movimientos.InsertOne(movimiento);

            Console.WriteLine("✅ Devolución registrada.");
        }

        public void IncrementarCantidadMaterial(string idMaterial, int extra)
        {
            var material = _context.Materiales.Find(m => m.Id == idMaterial).FirstOrDefault();
            if (material == null)
            {
                Console.WriteLine("❌ Material no encontrado.");
                return;
            }

            material.CantidadRegistrada += extra;
            material.CantidadActual += extra;

            _context.Materiales.ReplaceOne(m => m.Id == material.Id, material);
            Console.WriteLine("✅ Cantidad actualizada.");
        }

        public void MostrarHistorial()
        {
            var movimientos = _context.Movimientos.Find(_ => true).ToList();

            Console.WriteLine("\n📘 Historial de movimientos:");
            foreach (var mov in movimientos)
            {
                Console.WriteLine($"🔹 {mov.Tipo} - ID Material: {mov.MaterialId} - Cantidad: {mov.Cantidad} - Fecha: {mov.Fecha}");
            }
        }
    }
}
