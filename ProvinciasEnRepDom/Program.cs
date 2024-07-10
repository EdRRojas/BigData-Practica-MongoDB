using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

class Program
{
    private static IMongoCollection<BsonDocument> _collection;

    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var mongoSetting = configuration.GetSection("MongoDB").Get<MongoSettings>();

        var client = new MongoClient(mongoSetting.ConnectionString);

        var database = client.GetDatabase(mongoSetting.DatabaseName);

        _collection = database.GetCollection<BsonDocument>(mongoSetting.CollectionName);

        List<string> provincias = new List<string>
        {
            "Azua",
            "Bahoruco",
            "Barahona",
            "Dajabón",
            "Duarte",
            "El Seibo",
            "Elias Piña",
            "Espaillat",
            "Hato Mayor",
            "Hermanas",
            "Hermanas Mirabal",
            "Independencia",
            "La Altagracia",
            "La Romana",
            "La Vega",
            "Maria Trinidad Sánchez",
            "Monseñor Nouel",
            "Monte Cristi",
            "Monte Plata",
            "Pedernales",
            "Peravia",
            "Puerto Plata",
            "Samaná",
            "San Cristóbal",
            "San José de Ocoa",
            "San Juan",
            "San Pedro de Macoris",
            "Sánchez Ramirez",
            "Santiago",
            "Siango Rodriguez",
            "Santo Domingo",
            "Valverde",
            "Distrito Nacional"
        };

        await InsertItemsAsync(provincias);

        await PrintAllItemsAsync();
    }

    private static async Task InsertItemsAsync(List<string> items)
    {
        var documents = new List<BsonDocument>();

        foreach (var item in items)
        {
            var document = new BsonDocument { {"name", item} };
            documents.Add(document);
        }

        await _collection.InsertManyAsync(documents);
        Console.WriteLine("Elementos insertados correctamente en la coleccion."); ;
    }

    private static async Task PrintAllItemsAsync()
    {
        var alldocuments = await _collection.Find(new BsonDocument()).ToListAsync();

        Console.WriteLine("Elementos en la coleccion: ");
        foreach (var document in alldocuments)
        {
            Console.WriteLine(document["name"]);
        }
    }
}

class MongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName {  get; set; }
    public string CollectionName {  get; set; }
}
