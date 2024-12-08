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
    private readonly FlightrouteService _flightrouteService;
    private readonly OrderService _orderService;
    private readonly LuggageService _luggageService;
    private readonly TicketService _ticketService;

    public MongoDBSeeder(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, AirlineService airlineService, AirportService airportService, PlaneService planeService, CustomerService customerService, FlightrouteService flightrouteService, OrderService orderService, LuggageService luggageService, TicketService ticketService)
    {
        _mongoClient = mongoClient;
        _settings = settings.Value;
        _airlineService = airlineService;
        _airportService = airportService;
        _planeService = planeService;
        _customerService = customerService;
        _flightrouteService = flightrouteService;
        _orderService = orderService;
        _luggageService = luggageService;
        _ticketService = ticketService;
    }

    public async Task SeederInitalization()
    {
        await FillMongoDBCustomers();
        await FillMongoDBAirline();
        await FillMongoDBAirport();
        await FillMongoDBPlane();
        await FillMongoDBFlightroute();
        await FillMongoDBMaintenance();
        await FillMongoDBTicket();
        await FillMongoDBLuggage();
        await UpdateTicketAndFlightrouteAutomatically();
        await UpdateTicketWithLuggageId();
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
    
    public async Task FillMongoDBFlightroute()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBFlightroute>("Flightroute");

        var airports = await _airportService.GetAirportAsync();
        
        var flightroutes = new List<MongoDBFlightroute>
        {
            new MongoDBFlightroute
            {
                DepartureTime = DateTime.UtcNow.AddHours(2),
                ArrivalTime = DateTime.UtcNow.AddHours(10),
                FlightrouteId = airports[0].AirportId, 
                ArrivalAirportId = airports[4].AirportId,  
                TicketIds = []
            },
            new MongoDBFlightroute
            {
                DepartureTime = DateTime.UtcNow.AddHours(3),
                ArrivalTime = DateTime.UtcNow.AddHours(7),
                FlightrouteId = airports[1].AirportId, 
                ArrivalAirportId = airports[3].AirportId,  
                TicketIds = []
            },
            new MongoDBFlightroute
            {
                DepartureTime = DateTime.UtcNow.AddHours(5),
                ArrivalTime = DateTime.UtcNow.AddHours(13),
                FlightrouteId = airports[2].AirportId, 
                ArrivalAirportId = airports[5].AirportId,  
                TicketIds = []
            }
        };

        if (await collection.CountDocumentsAsync(FilterDefinition<MongoDBFlightroute>.Empty) == 0)
        {
            await collection.InsertManyAsync(flightroutes);
            Console.WriteLine("Flightroute collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Flightroute collection already contains data. No seeding performed.");
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

    public async Task FillMongoDBTicket()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        
        var collection = database.GetCollection<MongoDBTicket>("Ticket");

        var customers = await _customerService.GetCustomersAsync();
        var flightroutes = await _flightrouteService.GetFlightrouteAsync();
        var orders = await _orderService.GetOrdersAsync();
        var luggage = await _luggageService.GetLuggageAsync();
        
        var count = await collection.CountDocumentsAsync(FilterDefinition<MongoDBTicket>.Empty);
        if (count == 0)
        {
            await _ticketService.CreateTicketAsync(new MongoDBTicket
            {
                Price = 199.99,
                TicketType = "Economy",
                CustomerId = customers[0].CustomerId,
                FlightrouteId = flightroutes[0].FlightrouteId,
                LuggageIds = new List<MongoDBLuggage>
                {
                    new MongoDBLuggage {  }
                }
            });
            await _ticketService.CreateTicketAsync(new MongoDBTicket
            {
                Price = 199.99,
                TicketType = "Economy", 
                CustomerId = customers[0].CustomerId, 
                FlightrouteId = flightroutes[0].FlightrouteId, 
                LuggageIds = new List<MongoDBLuggage>
                {
                    new MongoDBLuggage {  }
                }
            });
            await _ticketService.CreateTicketAsync(new MongoDBTicket
            {
                Price = 299.99,
                TicketType = "Economy",
                CustomerId = customers[1].CustomerId,
                FlightrouteId = flightroutes[1].FlightrouteId,
                LuggageIds = new List<MongoDBLuggage>
                {
                    new MongoDBLuggage {  }
                }
            });
            await _ticketService.CreateTicketAsync(new MongoDBTicket
            {
                Price = 99.99,
                TicketType = "Economy",
                CustomerId = customers[2].CustomerId,
                FlightrouteId = flightroutes[2].FlightrouteId,
                LuggageIds = new List<MongoDBLuggage>
                {
                    new MongoDBLuggage {  }
                }
            });
            Console.WriteLine("Ticket collection seeded successfully.");
        }
        else
        {
            Console.WriteLine("Ticket collection already contains data. No seeding performed.");
        }
    }
    
    public async Task FillMongoDBLuggage()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);
        var tickets = await _ticketService.GetTicketAsync();
        var collection = database.GetCollection<MongoDBLuggage>("Luggage");
        
        var luggage = new List<MongoDBLuggage>
        {
            new MongoDBLuggage { Weight = 23.5, IsCarryOn = true , TicketId = tickets[0].TicketId },
            new MongoDBLuggage { Weight = 18.2, IsCarryOn = true , TicketId = tickets[0].TicketId },
            new MongoDBLuggage { Weight = 30.0, IsCarryOn = true , TicketId = tickets[0].TicketId },
            new MongoDBLuggage { Weight = 15.7, IsCarryOn = true , TicketId = tickets[0].TicketId },
            new MongoDBLuggage { Weight = 25.3, IsCarryOn = true , TicketId = tickets[0].TicketId },
            new MongoDBLuggage { Weight = 19.9, IsCarryOn = true , TicketId = tickets[0].TicketId },
            new MongoDBLuggage { Weight = 22.1, IsCarryOn = true , TicketId = tickets[0].TicketId }
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

    public async Task UpdateTicketAndFlightrouteAutomatically()
    {
        var database = _mongoClient.GetDatabase(_settings.DatabaseName);

        var ticketCollection = database.GetCollection<MongoDBTicket>("Ticket");
        var tickets = await ticketCollection.Find(Builders<MongoDBTicket>.Filter.Empty)
                                             .Limit(3) 
                                             .ToListAsync();

        var flightrouteCollection = database.GetCollection<MongoDBFlightroute>("Flightroute");
        var flightroutes = await flightrouteCollection.Find(Builders<MongoDBFlightroute>.Filter.Empty)
                                                  .Limit(3) 
                                                  .ToListAsync();

        if (tickets.Count < 3 || flightroutes.Count < 3)
        {
            Console.WriteLine("Not enough tickets or flightroutes to update.");
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            var ticketId = tickets[i].TicketId;
            var flightrouteId = flightroutes[i].FlightrouteId;

            var ticketFilter = Builders<MongoDBTicket>.Filter.Eq(t => t.TicketId, ticketId);
            var ticketUpdate = Builders<MongoDBTicket>.Update.Set(t => t.FlightrouteId, flightrouteId);
            await ticketCollection.UpdateOneAsync(ticketFilter, ticketUpdate);

            var flightrouteFilter = Builders<MongoDBFlightroute>.Filter.Eq(d => d.FlightrouteId, flightrouteId);
            var flightrouteUpdate = Builders<MongoDBFlightroute>.Update.AddToSet(d => d.TicketIds, new MongoDBTicket { TicketId = ticketId });
            await flightrouteCollection.UpdateOneAsync(flightrouteFilter, flightrouteUpdate);
        }

        Console.WriteLine("First 3 tickets and flightroutes have been linked.");
    }
    public async Task UpdateTicketWithLuggageId()
    {
        var luggage = await _luggageService.GetLuggageAsync();
        var random = new Random();
        var tickets = await _ticketService.GetTicketAsync();

        foreach (var ticket in tickets)
        {
            int randomIndex = random.Next(luggage.Count);
            var randomLuggage = luggage[randomIndex];
            _ticketService.UpdateTicketAsync(ticket.TicketId, new MongoDBTicket
            {
                TicketType = ticket.TicketType,
                CustomerId = ticket.CustomerId,
                Price = ticket.Price,
                FlightrouteId = ticket.FlightrouteId,
                LuggageIds = new List<MongoDBLuggage>
                {
                    randomLuggage
                }
            });
        }
    }
}