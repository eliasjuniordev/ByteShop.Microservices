using ByteShop.Catalog.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ByteShop.Catalog.Infrastructure.Context
{
    public class CatalogContext
    {
        private readonly IMongoDatabase _database;

        public CatalogContext(IConfiguration configuration)
        {
       
            var connectionString = configuration.GetConnectionString("MongoConnection");
            var databaseName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Produto> Produtos =>
            _database.GetCollection<Produto>("Produtos");
    }
}