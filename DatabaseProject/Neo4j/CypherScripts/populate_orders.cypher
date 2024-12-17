// Create Airlines
CREATE (a1:Airline {AirlineId: 1, AirlineName: 'Airline A'});
CREATE (a2:Airline {AirlineId: 2, AirlineName: 'Airline B'});

// Create Airports
CREATE (ap1:Airport {AirportId: 1, AirportName: 'Airport 1', AirportCity: 'City 1', Municipality: 'Municipality 1', AirportAbbreviation: 'AP1'});
CREATE (ap2:Airport {AirportId: 2, AirportName: 'Airport 2', AirportCity: 'City 2', Municipality: 'Municipality 2', AirportAbbreviation: 'AP2'});

// Create Customers
CREATE (c1:Customer {CustomerId: 1, FirstName: 'John', LastName: 'Doe', PassportNumber: 123456});
CREATE (c2:Customer {CustomerId: 2, FirstName: 'Jane', LastName: 'Smith', PassportNumber: 654321});
CREATE (c3:Customer {CustomerId: 3, FirstName: 'Alice', LastName: 'Johnson', PassportNumber: 111222});
CREATE (c4:Customer {CustomerId: 4, FirstName: 'Bob', LastName: 'Brown', PassportNumber: 333444});

// Create Planes
CREATE (p1:Plane {PlaneId: 1, PlaneDisplayName: 'Plane 1'});
CREATE (p2:Plane {PlaneId: 2, PlaneDisplayName: 'Plane 2'});

// Create Orders
CREATE (o1:Order {OrderId: 1, AirlineConfirmationNumber: 'CONF12345'});
CREATE (o2:Order {OrderId: 2, AirlineConfirmationNumber: 'CONF67890'});

// Create TicketTypes
CREATE (tt1:TicketType {TicketTypeId: 1, TicketTypeName: 'Economy'});
CREATE (tt2:TicketType {TicketTypeId: 2, TicketTypeName: 'Business'});

// Create Luggage
CREATE (l1:Luggage {LuggageId: 1, MaxWeight: 20.0, IsCarryOn: true});
CREATE (l2:Luggage {LuggageId: 2, MaxWeight: 30.0, IsCarryOn: false});

// Create Maintenance
CREATE (m1:Maintenance {MaintenanceId: 1, AirportId: 1, PlaneId: 1});
CREATE (m2:Maintenance {MaintenanceId: 2, AirportId: 2, PlaneId: 2});

// Create Flightroutes
CREATE (fr1:Flightroute {FlightrouteId: 1, DepartureTime: datetime('2023-10-01T10:00:00'), ArrivalTime: datetime('2023-10-01T12:00:00'), PlaneId: 1, DepartureAirportId: 1, ArrivalAirportId: 2});
CREATE (fr2:Flightroute {FlightrouteId: 2, DepartureTime: datetime('2023-10-02T14:00:00'), ArrivalTime: datetime('2023-10-02T16:00:00'), PlaneId: 2, DepartureAirportId: 2, ArrivalAirportId: 1});

// Create Tickets for Order 1
CREATE (t1:Ticket {TicketId: 101, Price: 150.00, TicketTypeId: 1, CustomerId: 1, FlightrouteId: 1, OrderId: 1});
CREATE (t2:Ticket {TicketId: 102, Price: 200.00, TicketTypeId: 2, CustomerId: 2, FlightrouteId: 1, OrderId: 1});

// Create Tickets for Order 2
CREATE (t3:Ticket {TicketId: 103, Price: 250.00, TicketTypeId: 1, CustomerId: 3, FlightrouteId: 2, OrderId: 2});
CREATE (t4:Ticket {TicketId: 104, Price: 300.00, TicketTypeId: 2, CustomerId: 4, FlightrouteId: 2, OrderId: 2});

// Create relationships between Orders and Tickets
MATCH (o1:Order {OrderId: 1}), (t1:Ticket {TicketId: 101})
CREATE (o1)-[:CONTAINS]->(t1);

MATCH (o1:Order {OrderId: 1}), (t2:Ticket {TicketId: 102})
CREATE (o1)-[:CONTAINS]->(t2);

MATCH (o2:Order {OrderId: 2}), (t3:Ticket {TicketId: 103})
CREATE (o2)-[:CONTAINS]->(t3);

MATCH (o2:Order {OrderId: 2}), (t4:Ticket {TicketId: 104})
CREATE (o2)-[:CONTAINS]->(t4);

// Create relationships between Planes and Airlines
MATCH (p1:Plane {PlaneId: 1}), (a1:Airline {AirlineId: 1})
CREATE (p1)-[:BELONGS_TO]->(a1);

MATCH (p2:Plane {PlaneId: 2}), (a2:Airline {AirlineId: 2})
CREATE (p2)-[:BELONGS_TO]->(a2);

// Create relationships between Tickets and Customers
MATCH (t1:Ticket {TicketId: 101}), (c1:Customer {CustomerId: 1})
CREATE (t1)-[:ASSIGNED_TO]->(c1);

MATCH (t2:Ticket {TicketId: 102}), (c2:Customer {CustomerId: 2})
CREATE (t2)-[:ASSIGNED_TO]->(c2);

MATCH (t3:Ticket {TicketId: 103}), (c3:Customer {CustomerId: 3})
CREATE (t3)-[:ASSIGNED_TO]->(c3);

MATCH (t4:Ticket {TicketId: 104}), (c4:Customer {CustomerId: 4})
CREATE (t4)-[:ASSIGNED_TO]->(c4);

// Create relationships between Tickets and Flightroutes
MATCH (t1:Ticket {TicketId: 101}), (fr1:Flightroute {FlightrouteId: 1})
CREATE (t1)-[:FOR_FLIGHTROUTE]->(fr1);

MATCH (t2:Ticket {TicketId: 102}), (fr1:Flightroute {FlightrouteId: 1})
CREATE (t2)-[:FOR_FLIGHTROUTE]->(fr1);

MATCH (t3:Ticket {TicketId: 103}), (fr2:Flightroute {FlightrouteId: 2})
CREATE (t3)-[:FOR_FLIGHTROUTE]->(fr2);

MATCH (t4:Ticket {TicketId: 104}), (fr2:Flightroute {FlightrouteId: 2})
CREATE (t4)-[:FOR_FLIGHTROUTE]->(fr2);

// Create relationships between Maintenance and Airports/Planes
MATCH (m1:Maintenance {MaintenanceId: 1}), (ap1:Airport {AirportId: 1}), (p1:Plane {PlaneId: 1})
CREATE (m1)-[:AT_AIRPORT]->(ap1)
CREATE (m1)-[:FOR_PLANE]->(p1);

MATCH (m2:Maintenance {MaintenanceId: 2}), (ap2:Airport {AirportId: 2}), (p2:Plane {PlaneId: 2})
CREATE (m2)-[:AT_AIRPORT]->(ap2)
CREATE (m2)-[:FOR_PLANE]->(p2);

// Create relationships for Flightroutes
MATCH (fr1:Flightroute {FlightrouteId: 1}), (p1:Plane {PlaneId: 1}), (ap1:Airport {AirportId: 1}), (ap2:Airport {AirportId: 2})
CREATE (fr1)-[:USES_PLANE]->(p1)
CREATE (fr1)-[:DEPARTS_FROM]->(ap1)
CREATE (fr1)-[:ARRIVES_AT]->(ap2);

MATCH (fr2:Flightroute {FlightrouteId: 2}), (p2:Plane {PlaneId: 2}), (ap2:Airport {AirportId: 2}), (ap1:Airport {AirportId: 1})
CREATE (fr2)-[:USES_PLANE]->(p2)
CREATE (fr2)-[:DEPARTS_FROM]->(ap2)
CREATE (fr2)-[:ARRIVES_AT]->(ap1);

// Create relationships between Luggage and Tickets
MATCH (l1:Luggage {LuggageId: 1}), (t1:Ticket {TicketId: 101})
CREATE (l1)-[:BELONGS_TO]->(t1);

MATCH (l2:Luggage {LuggageId: 2}), (t2:Ticket {TicketId: 102})
CREATE (l2)-[:BELONGS_TO]->(t2);

// Create relationships between Tickets and TicketTypes
MATCH (t1:Ticket {TicketId: 101}), (tt1:TicketType {TicketTypeId: 1})
CREATE (t1)-[:HAS_TICKET_TYPE]->(tt1);

MATCH (t2:Ticket {TicketId: 102}), (tt2:TicketType {TicketTypeId: 2})
CREATE (t2)-[:HAS_TICKET_TYPE]->(tt2);

MATCH (t3:Ticket {TicketId: 103}), (tt1:TicketType {TicketTypeId: 1})
CREATE (t3)-[:HAS_TICKET_TYPE]->(tt1);

MATCH (t4:Ticket {TicketId: 104}), (tt2:TicketType {TicketTypeId: 2})
CREATE (t4)-[:HAS_TICKET_TYPE]->(tt2);
