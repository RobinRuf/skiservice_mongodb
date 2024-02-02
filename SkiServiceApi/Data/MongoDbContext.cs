using skiservice.Common;
using skiservice.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace skiservice.Data
{
    /// <summary>
    /// Database context for the Jetstream API using MongoDB
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);

            // Initialisieren Sie hier die Seed-Methoden, falls erforderlich
        }

        public IMongoCollection<ServiceOrderModel> ServiceOrders => _database.GetCollection<ServiceOrderModel>("ServiceOrders");
        public IMongoCollection<UserModel> Users => _database.GetCollection<UserModel>("Users");
        public IMongoCollection<ServiceModel> Services => _database.GetCollection<ServiceModel>("Services");
        public IMongoCollection<StatusModel> Statuses => _database.GetCollection<StatusModel>("Statuses");
        public IMongoCollection<PriorityModel> Priorities => _database.GetCollection<PriorityModel>("Priorities");

        // Beispiel für eine Methode zum Erstellen eines Passwort-Hashes
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public void SeedDatabase()
        {
            var userCollection = _database.GetCollection<UserModel>("Users");

            // Eindeutigen Index für den Benutzernamen erstellen
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexKeys = Builders<UserModel>.IndexKeys.Ascending(u => u.UserName);
            userCollection.Indexes.CreateOne(new CreateIndexModel<UserModel>(indexKeys, indexOptions));

            // Überprüfen, ob Users bereits Daten enthält
            if (!userCollection.Find(_ => true).Any())
            {
                var users = new List<UserModel>();

                // Seed Data für Users
                CreatePasswordHash("Password", out byte[] adminPasswordHash, out byte[] adminPasswordSalt);
                users.Add(new UserModel
                {
                    UserName = "admin",
                    PasswordHash = adminPasswordHash,
                    PasswordSalt = adminPasswordSalt,
                    IsLocked = false,
                    Role = Roles.ADMIN
                });

                for (int i = 1; i <= 10; i++)
                {
                    CreatePasswordHash($"Password{i}", out byte[] userPasswordHash, out byte[] userPasswordSalt);
                    users.Add(new UserModel
                    {
                        UserName = $"user{i}",
                        PasswordHash = userPasswordHash,
                        PasswordSalt = userPasswordSalt,
                        IsLocked = false,
                        Role = Roles.USER
                    });
                }

                

                userCollection.InsertMany(users);

                var serviceIdMap = new Dictionary<string, string>();
                var statusIdMap = new Dictionary<string, string>();
                var priorityIdMap = new Dictionary<string, string>();

                // Seed Data für Services
                var serviceCollection = _database.GetCollection<ServiceModel>("Services");
                if (!serviceCollection.Find(_ => true).Any())
                {
                    var services = new List<ServiceModel>
        {
            new ServiceModel { ServiceName = "Kleiner Service", Price = 49 },
            new ServiceModel { ServiceName = "Grosser Service", Price = 69 },
            new ServiceModel { ServiceName = "Rennskiservice", Price = 99 },
            new ServiceModel { ServiceName = "Bindung montieren und einstellen", Price = 39 },
            new ServiceModel { ServiceName = "Fell zuschneiden", Price = 25 },
            new ServiceModel { ServiceName = "Heißwachsen", Price = 18 }
        };

                    serviceCollection.InsertMany(services);

                    // Abrufen der eingefügten Services, um deren ObjectId zu erhalten
                    var insertedServices = serviceCollection.Find(_ => true).ToList();

                    serviceIdMap = insertedServices.ToDictionary(service => service.ServiceName, service => service.Id.ToString());

                    
                }

                // Seed Data für Statuses
                var statusCollection = _database.GetCollection<StatusModel>("Statuses");
                if (!statusCollection.Find(_ => true).Any())
                {
                    var statuses = new List<StatusModel>
        {
            new StatusModel { StatusName = "Offen" },
            new StatusModel { StatusName = "In Arbeit" },
            new StatusModel { StatusName = "Abgeschlossen" },
            new StatusModel { StatusName = "Storniert" }
        };

                    statusCollection.InsertMany(statuses);

                    var insertedStatuses = statusCollection.Find(_ => true).ToList();
                    statusIdMap = insertedStatuses.ToDictionary(status => status.StatusName, status => status.Id.ToString());
                }

                // Seed Data für Priorities
                var priorityCollection = _database.GetCollection<PriorityModel>("Priorities");
                if (!priorityCollection.Find(_ => true).Any())
                {
                    var priorities = new List<PriorityModel>
        {
            new PriorityModel { PriorityName = "Tief", Price = 0 },
            new PriorityModel { PriorityName = "Standard", Price = 5 },
            new PriorityModel { PriorityName = "Hoch", Price = 10 }
        };

                    priorityCollection.InsertMany(priorities);

                    var insertedPriorities = priorityCollection.Find(_ => true).ToList();
                    priorityIdMap = insertedPriorities.ToDictionary(priority => priority.PriorityName, priority => priority.Id.ToString());

                    // Seed Data für ServiceOrders
                    var serviceOrderCollection = _database.GetCollection<ServiceOrderModel>("ServiceOrders");
                    if (!serviceOrderCollection.Find(_ => true).Any())
                    {
                        var serviceOrders = new List<ServiceOrderModel>
        {
            new ServiceOrderModel
            {
                Firstname = "Max",
                Lastname = "Mustermann",
                Email = "max.mustermann@example.com",
                Phone = "1234567890",
                PriorityId = priorityIdMap["Tief"],
                CreateDate = DateTime.Now,
                PickupDate = DateTime.Now.AddDays(1),
                ServiceId = serviceIdMap["Kleiner Service"],
                TotalPrice_CHF = 49,
                StatusId = statusIdMap["Offen"],
                Comment = "Erster Kommentar"
            },
            new ServiceOrderModel
            {
                Firstname = "Maria",
                Lastname = "Musterfrau",
                Email = "maria.musterfrau@example.com",
                Phone = "0987654321",
                PriorityId =priorityIdMap["Hoch"],
                CreateDate = DateTime.Now,
                PickupDate = DateTime.Now.AddDays(2),
                ServiceId = serviceIdMap["Grosser Service"],
                TotalPrice_CHF = 79, // 69 + 10
                StatusId = statusIdMap["In Arbeit"],
                Comment = "Zweiter Kommentar"
            },
            new ServiceOrderModel
            {
                Firstname = "Johannes",
                Lastname = "Doe",
                Email = "johannes.doe@example.com",
                Phone = "1122334455",
                PriorityId = priorityIdMap["Standard"],
                CreateDate = DateTime.Now,
                PickupDate = DateTime.Now.AddDays(3),
                ServiceId = serviceIdMap["Rennskiservice"],
                TotalPrice_CHF = 109, // 99 + 10
                StatusId = statusIdMap["Abgeschlossen"],
                Comment = "Dritter Kommentar"
            },
            new ServiceOrderModel
            {
                Firstname = "Anna",
                Lastname = "Beispiel",
                Email = "anna.beispiel@example.com",
                Phone = "2233445566",
                PriorityId = priorityIdMap["Standard"],
                CreateDate = DateTime.Now,
                PickupDate = DateTime.Now.AddDays(4),
                ServiceId = serviceIdMap ["Bindung montieren und einstellen"],
                TotalPrice_CHF = 49, // 39 + 10
                StatusId = statusIdMap["Storniert"],
                Comment = "Vierter Kommentar"
            },
            new ServiceOrderModel
            {
                Firstname = "Lukas",
                Lastname = "Muster",
                Email = "lukas.muster@example.com",
                Phone = "3344556677",
                PriorityId = priorityIdMap["Tief"],
                CreateDate = DateTime.Now,
                PickupDate = DateTime.Now.AddDays(5),
                ServiceId = serviceIdMap["Fell zuschneiden"],
                TotalPrice_CHF = 30, // 25 + 5
                StatusId = statusIdMap["Offen"],
                Comment = "Fünfter Kommentar"
            }
        };
                        serviceOrderCollection.InsertMany(serviceOrders);
                    }
                }
            }
        } 
    }
}
