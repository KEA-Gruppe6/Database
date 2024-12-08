using Database_project.Core;
using Database_project.Core.MongoDB.Entities;
using Database_project.Core.MongoDB.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database_project.MongoDB_Query;

public class MongoDBSeeder
{
    private readonly IMongoClient _mongoClient;
    private readonly MongoDbSettings _settings;
    private readonly AirlineService _airlineService;
    private readonly AirportService _airportService;
    private readonly PlaneService _planeService;
    private readonly CustomerService _customerService;
    private readonly DepartureService _departureService;
    private readonly OrderService _orderService;
    private readonly LuggageService _luggageService;

    public MongoDBSeeder(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, AirlineService airlineService, AirportService airportService, PlaneService planeService, CustomerService customerService, DepartureService departureService, OrderService orderService, LuggageService luggageService)
    {
        _mongoClient = mongoClient;
        _settings = settings.Value;
        _airlineService = airlineService;
        _airportService = airportService;
        _planeService = planeService;
        _customerService = customerService;
        _departureService = departureService;
        _orderService = orderService;
        _luggageService = luggageService;
    }

    public async Task SeederInitalization()
    {
        await FillMongoDBCustomers();
        await FillMongoDBAirline();
        await FillMongoDBAirport();
        await FillMongoDBLuggage();
        await FillMongoDBPlane();
        await FillMongoDBDeparture();
        await FillMongoDBMaintenance();
        await FillMongoDBOrder();
        await FillMongoDBTicket();
        await UpdateTicketAndDepartureAutomatically();
    }
    
    
    
    public async Task FillMongoDBCustomers()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBCustomer>("Customers");

        var customers = new List<MongoDBCustomer>
        {
            new MongoDBCustomer { FirstName = "John", LastName = "Doe", PassportNumber = 123456789 },
            new MongoDBCustomer { FirstName = "Jane", LastName = "Smith", PassportNumber = 987654321 },
            new MongoDBCustomer { FirstName = "Michael", LastName = "Johnson", PassportNumber = 112233445 },
            new MongoDBCustomer { FirstName = "Emily", LastName = "Davis", PassportNumber = 556677889 },
            new MongoDBCustomer { FirstName = "James", LastName = "Brown", PassportNumber = 223344556 },
            new MongoDBCustomer { FirstName = "Olivia", LastName = "Williams", PassportNumber = 998877665 },
            new MongoDBCustomer { FirstName = "William", LastName = "Jones", PassportNumber = 334455667 },
            new MongoDBCustomer { FirstName = "Sophia", LastName = "Miller", PassportNumber = 776655443 },
            new MongoDBCustomer { FirstName = "David", LastName = "Wilson", PassportNumber = 445566778 },
            new MongoDBCustomer { FirstName = "Isabella", LastName = "Moore", PassportNumber = 889977665 },
            new MongoDBCustomer { FirstName = "Benjamin", LastName = "Taylor", PassportNumber = 123987654 },
            new MongoDBCustomer { FirstName = "Mia", LastName = "Anderson", PassportNumber = 667788990 },
            new MongoDBCustomer { FirstName = "Elijah", LastName = "Thomas", PassportNumber = 334411223 },
            new MongoDBCustomer { FirstName = "Amelia", LastName = "Jackson", PassportNumber = 775544332 },
            new MongoDBCustomer { FirstName = "Lucas", LastName = "White", PassportNumber = 998822233 },
            new MongoDBCustomer { FirstName = "Harper", LastName = "Harris", PassportNumber = 446688557 },
            new MongoDBCustomer { FirstName = "Alexander", LastName = "Martin", PassportNumber = 229988445 },
            new MongoDBCustomer { FirstName = "Evelyn", LastName = "Garcia", PassportNumber = 887766554 },
            new MongoDBCustomer { FirstName = "Jackson", LastName = "Rodriguez", PassportNumber = 223344888 },
            new MongoDBCustomer { FirstName = "Charlotte", LastName = "Martinez", PassportNumber = 998877224 }
        };

        if (await collection.CountDocumentsAsync(FilterDefinition<MongoDBCustomer>.Empty) == 0)
        {
            await collection.InsertManyAsync(customers);
            Console.WriteLine("Customer collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Customer collection already contains data. No seeding performed.");
        }
    }

    public async Task FillMongoDBAirline()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBAirline>("Airline");

        var airlines = new List<MongoDBAirline>
        {
            new MongoDBAirline { AirlineName = "Delta Airlines", Planes = new List<MongoDBPlane>() },
            new MongoDBAirline { AirlineName = "American Airlines", Planes = new List<MongoDBPlane>() },
            new MongoDBAirline { AirlineName = "United Airlines", Planes = new List<MongoDBPlane>() },
            new MongoDBAirline { AirlineName = "Southwest Airlines", Planes = new List<MongoDBPlane>() },
            new MongoDBAirline { AirlineName = "JetBlue Airways", Planes = new List<MongoDBPlane>() },
            new MongoDBAirline { AirlineName = "Alaska Airlines", Planes = new List<MongoDBPlane>() },
            new MongoDBAirline { AirlineName = "Spirit Airlines", Planes = new List<MongoDBPlane>() },
            new MongoDBAirline { AirlineName = "Hawaiian Airlines", Planes = new List<MongoDBPlane>() },
            new MongoDBAirline { AirlineName = "SkyWest Airlines", Planes = new List<MongoDBPlane>() },
            new MongoDBAirline { AirlineName = "Air Canada", Planes = new List<MongoDBPlane>() }
        };
        
        if (await collection.CountDocumentsAsync(FilterDefinition<MongoDBAirline>.Empty) == 0)
        {
            await collection.InsertManyAsync(airlines);
            Console.WriteLine("Airline collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Airline collection already contains data. No seeding performed.");
        }
    }

    public async Task FillMongoDBAirport()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBAirport>("Airport");
        
        var airports = new List<MongoDBAirport>
        {
            new MongoDBAirport { AirportName = "Copenhagen Airport", AirportCity = "Copenhagen", Municipality = "Capital Region of Denmark", AirportAbbreviation = "CPH" },
            new MongoDBAirport { AirportName = "Aalborg Airport", AirportCity = "Aalborg", Municipality = "North Jutland Region", AirportAbbreviation = "AAL" },
            new MongoDBAirport { AirportName = "Billund Airport", AirportCity = "Billund", Municipality = "South Denmark", AirportAbbreviation = "BLL" },
            new MongoDBAirport { AirportName = "Esbjerg Airport", AirportCity = "Esbjerg", Municipality = "South Denmark", AirportAbbreviation = "EBJ" },
            new MongoDBAirport { AirportName = "Aarhus Airport", AirportCity = "Aarhus", Municipality = "Central Denmark Region", AirportAbbreviation = "AAR" },
            new MongoDBAirport { AirportName = "Odense Airport", AirportCity = "Odense", Municipality = "Southern Denmark", AirportAbbreviation = "ODE" },
            new MongoDBAirport { AirportName = "Bornholm Airport", AirportCity = "Rønne", Municipality = "Capital Region of Denmark", AirportAbbreviation = "RNN" },
            new MongoDBAirport { AirportName = "Karup Airport", AirportCity = "Karup", Municipality = "Central Jutland Region", AirportAbbreviation = "KRP" },
            new MongoDBAirport { AirportName = "Sonderborg Airport", AirportCity = "Sønderborg", Municipality = "Southern Denmark", AirportAbbreviation = "SGD" },
            new MongoDBAirport { AirportName = "Roskilde Airport", AirportCity = "Roskilde", Municipality = "Zealand Region", AirportAbbreviation = "RKE" }
        };

        if (await collection.CountDocumentsAsync(FilterDefinition<MongoDBAirport>.Empty) == 0)
        {
            await collection.InsertManyAsync(airports);
            Console.WriteLine("Airport collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Airport collection already contains data. No seeding performed.");
        }
    }
    
    public async Task FillMongoDBLuggage()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBLuggage>("Luggage");
        
        var luggage = new List<MongoDBLuggage>
        {
            new MongoDBLuggage { MaxWeight = 23.5, IsCarryOn = true },
            new MongoDBLuggage { MaxWeight = 18.2, IsCarryOn = true },
            new MongoDBLuggage { MaxWeight = 30.0, IsCarryOn = true },
            new MongoDBLuggage { MaxWeight = 15.7, IsCarryOn = true },
            new MongoDBLuggage { MaxWeight = 25.3, IsCarryOn = true },
            new MongoDBLuggage { MaxWeight = 19.9, IsCarryOn = true },
            new MongoDBLuggage { MaxWeight = 22.1, IsCarryOn = true }
        };

        if (await collection.CountDocumentsAsync(FilterDefinition<MongoDBLuggage>.Empty) == 0)
        {
            await collection.InsertManyAsync(luggage);
            Console.WriteLine("Luggage collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Luggage collection already contains data. No seeding performed.");
        }
    }
    
    public async Task FillMongoDBPlane()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBPlane>("Plane");

        var airlines = await _airlineService.GetAirlineAsync();
        
        var planes = new List<MongoDBPlane>
        {
            new MongoDBPlane { PlaneDisplayName = "Boeing 737", AirlineId = airlines[0].AirlineId },
            new MongoDBPlane { PlaneDisplayName = "Airbus A320", AirlineId = airlines[1].AirlineId },
            new MongoDBPlane { PlaneDisplayName = "Boeing 777", AirlineId = airlines[2].AirlineId },
            new MongoDBPlane { PlaneDisplayName = "Airbus A321", AirlineId = airlines[3].AirlineId },
            new MongoDBPlane { PlaneDisplayName = "Boeing 747", AirlineId = airlines[4].AirlineId },
            new MongoDBPlane { PlaneDisplayName = "Embraer 190", AirlineId = airlines[5].AirlineId },
            new MongoDBPlane { PlaneDisplayName = "Bombardier CRJ900", AirlineId = airlines[6].AirlineId },
            new MongoDBPlane { PlaneDisplayName = "Airbus A330", AirlineId = airlines[7].AirlineId },
            new MongoDBPlane { PlaneDisplayName = "Boeing 767", AirlineId = airlines[8].AirlineId },
            new MongoDBPlane { PlaneDisplayName = "Airbus A380", AirlineId = airlines[9].AirlineId }
        };

        if (await collection.CountDocumentsAsync(FilterDefinition<MongoDBPlane>.Empty) == 0)
        {
            await collection.InsertManyAsync(planes);
            Console.WriteLine("Plane collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Plane collection already contains data. No seeding performed.");
        }
    }
    
    public async Task FillMongoDBDeparture()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBDeparture>("Departure");

        var airports = await _airportService.GetAirportAsync();
        
        var departures = new List<MongoDBDeparture>
        {
            new MongoDBDeparture
            {
                DepartureTime = DateTime.UtcNow.AddHours(2),
                ArrivalTime = DateTime.UtcNow.AddHours(10),
                DepartureAirportId = airports[0].AirportId, 
                ArrivalAirportId = airports[4].AirportId,  
                TicketIds = []
            },
            new MongoDBDeparture
            {
                DepartureTime = DateTime.UtcNow.AddHours(3),
                ArrivalTime = DateTime.UtcNow.AddHours(7),
                DepartureAirportId = airports[1].AirportId, 
                ArrivalAirportId = airports[3].AirportId,  
                TicketIds = []
            },
            new MongoDBDeparture
            {
                DepartureTime = DateTime.UtcNow.AddHours(5),
                ArrivalTime = DateTime.UtcNow.AddHours(13),
                DepartureAirportId = airports[2].AirportId, 
                ArrivalAirportId = airports[5].AirportId,  
                TicketIds = []
            }
        };

        if (await collection.CountDocumentsAsync(FilterDefinition<MongoDBDeparture>.Empty) == 0)
        {
            await collection.InsertManyAsync(departures);
            Console.WriteLine("Departure collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Departure collection already contains data. No seeding performed.");
        }
    }

    public async Task FillMongoDBMaintenance()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBMaintenance>("Maintenance");

        var airports = await _airportService.GetAirportAsync();
        var planes = await _planeService.GetPlaneAsync();
        
        var maintenances = new List<MongoDBMaintenance>
        {
            new MongoDBMaintenance
            {
                AirportId = airports[0].AirportId,
                PlaneId = planes[0].PlaneId
            },
            new MongoDBMaintenance
            {
                AirportId = airports[1].AirportId,
                PlaneId = planes[1].PlaneId
            }
        };
        var count = await collection.CountDocumentsAsync(FilterDefinition<MongoDBMaintenance>.Empty);
        if (count == 0)
        {
            await collection.InsertManyAsync(maintenances);
            Console.WriteLine("Maintenance collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Maintenance collection already contains data. No seeding performed.");
        }
    }

    public async Task FillMongoDBOrder()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBOrder>("Order");
        
        var orders = new List<MongoDBOrder>
        {
            new MongoDBOrder { AirlineConfirmationNumber = "111AAA" },
            new MongoDBOrder { AirlineConfirmationNumber = "222BBB" },
            new MongoDBOrder { AirlineConfirmationNumber = "333CCC" },
            new MongoDBOrder { AirlineConfirmationNumber = "444DDD" },
            new MongoDBOrder { AirlineConfirmationNumber = "555EEE" },
            new MongoDBOrder { AirlineConfirmationNumber = "666FFF" },
            new MongoDBOrder { AirlineConfirmationNumber = "777GGG" },
            new MongoDBOrder { AirlineConfirmationNumber = "888HHH" },
            new MongoDBOrder { AirlineConfirmationNumber = "999III" },
            new MongoDBOrder { AirlineConfirmationNumber = "101JJJ" }
        };

        if (await collection.CountDocumentsAsync(FilterDefinition<MongoDBOrder>.Empty) == 0)
        {
            await collection.InsertManyAsync(orders);
            Console.WriteLine("Order collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Order collection already contains data. No seeding performed.");
        }
    }

    

    public async Task FillMongoDBTicket()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBTicket>("Ticket");

        var customers = await _customerService.GetCustomersAsync();
        var departures = await _departureService.GetDepartureAsync();
        var orders = await _orderService.GetOrdersAsync();
        var luggage = await _luggageService.GetLuggageAsync();
        
        var tickets = new List<MongoDBTicket>
        {
            new MongoDBTicket
            {
                Price = 199.99,
                TicketType = "Economy", 
                CustomerId = customers[0].CustomerId, 
                DepartureId = departures[0].DepartureId, 
                OrderId = orders[0].OrderId, 
                LuggageIds = new List<MongoDBLuggage>
                {
                    new MongoDBLuggage { LuggageId = luggage[0].LuggageId }
                }
            },
            new MongoDBTicket
            {
                Price = 299.99,
                TicketType = "Economy",
                CustomerId = customers[1].CustomerId,
                DepartureId = departures[1].DepartureId,
                OrderId = orders[1].OrderId,
                LuggageIds = new List<MongoDBLuggage>
                {
                    new MongoDBLuggage { LuggageId = luggage[1].LuggageId }
                }
            },
            new MongoDBTicket
            {
                Price = 99.99,
                TicketType = "Economy",
                CustomerId = customers[2].CustomerId,
                DepartureId = departures[2].DepartureId,
                OrderId = orders[2].OrderId,
                LuggageIds = new List<MongoDBLuggage>
                {
                    new MongoDBLuggage { LuggageId = luggage[2].LuggageId }
                }
            }
        };
        var count = await collection.CountDocumentsAsync(FilterDefinition<MongoDBTicket>.Empty);
        if (count == 0)
        {
            await collection.InsertManyAsync(tickets);
            Console.WriteLine("Ticket collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Ticket collection already contains data. No seeding performed.");
        }

    }

    public async Task UpdateTicketAndDepartureAutomatically()
{
    var database = _mongoClient.GetDatabase(_settings.DatabaseName);

    var ticketCollection = database.GetCollection<MongoDBTicket>("Ticket");
    var tickets = await ticketCollection.Find(Builders<MongoDBTicket>.Filter.Empty)
                                         .Limit(3) 
                                         .ToListAsync();

    var departureCollection = database.GetCollection<MongoDBDeparture>("Departure");
    var departures = await departureCollection.Find(Builders<MongoDBDeparture>.Filter.Empty)
                                              .Limit(3) 
                                              .ToListAsync();

    if (tickets.Count < 3 || departures.Count < 3)
    {
        Console.WriteLine("Not enough tickets or departures to update.");
        return;
    }

    for (int i = 0; i < 3; i++)
    {
        var ticketId = tickets[i].TicketId;
        var departureId = departures[i].DepartureId;

        var ticketFilter = Builders<MongoDBTicket>.Filter.Eq(t => t.TicketId, ticketId);
        var ticketUpdate = Builders<MongoDBTicket>.Update.Set(t => t.DepartureId, departureId);
        await ticketCollection.UpdateOneAsync(ticketFilter, ticketUpdate);

        var departureFilter = Builders<MongoDBDeparture>.Filter.Eq(d => d.DepartureId, departureId);
        var departureUpdate = Builders<MongoDBDeparture>.Update.AddToSet(d => d.TicketIds, new MongoDBTicket { TicketId = ticketId });
        await departureCollection.UpdateOneAsync(departureFilter, departureUpdate);
    }

    Console.WriteLine("First 3 tickets and departures have been linked.");
}
}
