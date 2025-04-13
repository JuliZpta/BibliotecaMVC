using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;
using BibliotecaMVC.Models;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext()
    {
        var client = new MongoClient("mongodb://localhost:27017"); 
        _database = client.GetDatabase("BibliotecaDB");
    }

    public IMongoCollection<Material> Materiales => _database.GetCollection<Material>("Materiales");
    public IMongoCollection<Persona> Personas => _database.GetCollection<Persona>("Personas");
    public IMongoCollection<Movimiento> Movimientos => _database.GetCollection<Movimiento>("Movimientos");
}

