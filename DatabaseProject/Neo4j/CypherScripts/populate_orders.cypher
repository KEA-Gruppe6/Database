// Create Airlines
CREATE (a1:Airline {AirlineId: 1, AirlineName: 'Airline A'});
CREATE (a2:Airline {AirlineId: 2, AirlineName: 'Airline B'});

// Create Airports
CREATE (ap1:Airport {AirportId: 1, AirportName: 'Airport 1', AirportCity: 'City 1', Municipality: 'Municipality 1', AirportAbbreviation: 'AP1'});
CREATE (ap2:Airport {AirportId: 2, AirportName: 'Airport 2', AirportCity: 'City 2', Municipality: 'Municipality 2', AirportAbbreviation: 'AP2'});
CREATE (ap3:Airport {AirportId: 3, AirportName: 'Airport 3', AirportCity: 'City 3', Municipality: 'Municipality 3', AirportAbbreviation: 'AP3'});
CREATE (ap4:Airport {AirportId: 4, AirportName: 'Airport 4', AirportCity: 'City 4', Municipality: 'Municipality 4', AirportAbbreviation: 'AP4'});

// Create Customers
CREATE (c1:Customer {CustomerId: 1, FirstName: 'John', LastName: 'Doe', PassportNumber: 12345675});
CREATE (c2:Customer {CustomerId: 2, FirstName: 'Jane', LastName: 'Smith', PassportNumber: 65432102});
CREATE (c3:Customer {CustomerId: 3, FirstName: 'Alice', LastName: 'Johnson', PassportNumber: 11122215});
CREATE (c4:Customer {CustomerId: 4, FirstName: 'Bob', LastName: 'Brown', PassportNumber: 33344445});
CREATE (c5:Customer {CustomerId: 5, FirstName: 'Charlie', LastName: 'Davis', PassportNumber: 55566678});
CREATE (c6:Customer {CustomerId: 6, FirstName: 'A.F. Asht', LastName: 'Kweery', PassportNumber: 98765457});

// Create Planes
CREATE (p1:Plane {PlaneId: 1, PlaneDisplayName: 'Plane 1'});
CREATE (p2:Plane {PlaneId: 2, PlaneDisplayName: 'Plane 2'});
CREATE (p3:Plane {PlaneId: 3, PlaneDisplayName: 'Plane 3'});
CREATE (p4:Plane {PlaneId: 4, PlaneDisplayName: 'Plane 4'});
CREATE (p5:Plane {PlaneId: 5, PlaneDisplayName: 'Plane 5'});

// Create Orders
CREATE (o1:Order {OrderId: 1, AirlineConfirmationNumber: 'CONF12345'});
CREATE (o2:Order {OrderId: 2, AirlineConfirmationNumber: 'CONF67890'});
CREATE (o3:Order {OrderId: 3, AirlineConfirmationNumber: 'CONF11111'});
CREATE (o4:Order {OrderId: 4, AirlineConfirmationNumber: 'CONF22222'});
CREATE (o5:Order {OrderId: 5, AirlineConfirmationNumber: 'CONF33333'});

// Create TicketTypes
CREATE (tt1:TicketType {TicketTypeId: 1, TicketTypeName: 'Economy'});
CREATE (tt2:TicketType {TicketTypeId: 2, TicketTypeName: 'Business'});
CREATE (tt3:TicketType {TicketTypeId: 3, TicketTypeName: 'First Class'});

// Create Luggage
CREATE (l1:Luggage {LuggageId: 1, MaxWeight: 20.0, IsCarryOn: true});
CREATE (l2:Luggage {LuggageId: 2, MaxWeight: 30.0, IsCarryOn: false});
CREATE (l3:Luggage {LuggageId: 3, MaxWeight: 25.0, IsCarryOn: true});
CREATE (l4:Luggage {LuggageId: 4, MaxWeight: 35.0, IsCarryOn: false});
CREATE (l5:Luggage {LuggageId: 5, MaxWeight: 22.0, IsCarryOn: true});
CREATE (l6:Luggage {LuggageId: 6, MaxWeight: 32.0, IsCarryOn: false});

// Create Maintenance
CREATE (m1:Maintenance {MaintenanceId: 1, AirportId: 1, PlaneId: 1});
CREATE (m2:Maintenance {MaintenanceId: 2, AirportId: 2, PlaneId: 2});

// Create Flightroutes
CREATE (fr1:Flightroute {FlightrouteId: 1, DepartureTime: datetime('2023-10-01T10:00:00'), ArrivalTime: datetime('2023-10-01T12:00:00'), PlaneId: 1, DepartureAirportId: 1, ArrivalAirportId: 2});
CREATE (fr2:Flightroute {FlightrouteId: 2, DepartureTime: datetime('2023-10-02T14:00:00'), ArrivalTime: datetime('2023-10-02T16:00:00'), PlaneId: 2, DepartureAirportId: 2, ArrivalAirportId: 1});
CREATE (fr3:Flightroute {FlightrouteId: 3, DepartureTime: datetime('2023-10-03T10:00:00'), ArrivalTime: datetime('2023-10-03T12:00:00'), PlaneId: 3, DepartureAirportId: 3, ArrivalAirportId: 4});
CREATE (fr4:Flightroute {FlightrouteId: 4, DepartureTime: datetime('2023-10-04T14:00:00'), ArrivalTime: datetime('2023-10-04T16:00:00'), PlaneId: 4, DepartureAirportId: 4, ArrivalAirportId: 3});
CREATE (fr5:Flightroute {FlightrouteId: 5, DepartureTime: datetime('2023-10-05T10:00:00'), ArrivalTime: datetime('2023-10-05T12:00:00'), PlaneId: 5, DepartureAirportId: 5, ArrivalAirportId: 6});

// Create Tickets for Order 1
CREATE (t1:Ticket {TicketId: 101, Price: 150.00, TicketTypeId: 1, CustomerId: 1, FlightrouteId: 1, OrderId: 1});
CREATE (t2:Ticket {TicketId: 102, Price: 200.00, TicketTypeId: 2, CustomerId: 2, FlightrouteId: 1, OrderId: 1});

// Create Tickets for Order 2
CREATE (t3:Ticket {TicketId: 103, Price: 250.00, TicketTypeId: 1, CustomerId: 3, FlightrouteId: 2, OrderId: 2});
CREATE (t4:Ticket {TicketId: 104, Price: 300.00, TicketTypeId: 2, CustomerId: 4, FlightrouteId: 2, OrderId: 2});

// Create Tickets for Order 3
CREATE (t5:Ticket {TicketId: 105, Price: 350.00, TicketTypeId: 3, CustomerId: 5, FlightrouteId: 3, OrderId: 3});
CREATE (t6:Ticket {TicketId: 106, Price: 400.00, TicketTypeId: 1, CustomerId: 6, FlightrouteId: 3, OrderId: 3});

// Create Tickets for Order 4
CREATE (t7:Ticket {TicketId: 107, Price: 450.00, TicketTypeId: 2, CustomerId: 1, FlightrouteId: 4, OrderId: 4});
CREATE (t8:Ticket {TicketId: 108, Price: 500.00, TicketTypeId: 3, CustomerId: 3, FlightrouteId: 4, OrderId: 4});

// Create Tickets for Order 5
CREATE (t9:Ticket {TicketId: 109, Price: 550.00, TicketTypeId: 1, CustomerId: 3, FlightrouteId: 5, OrderId: 5});
CREATE (t10:Ticket {TicketId: 110, Price: 600.00, TicketTypeId: 2, CustomerId: 6, FlightrouteId: 5, OrderId: 5});

// Create relationships between Orders and Tickets
MATCH (o3:Order {OrderId: 3}), (t5:Ticket {TicketId: 105})
CREATE (o3)-[:CONTAINS]->(t5);

MATCH (o3:Order {OrderId: 3}), (t6:Ticket {TicketId: 106})
CREATE (o3)-[:CONTAINS]->(t6);

MATCH (o4:Order {OrderId: 4}), (t7:Ticket {TicketId: 107})
CREATE (o4)-[:CONTAINS]->(t7);

MATCH (o4:Order {OrderId: 4}), (t8:Ticket {TicketId: 108})
CREATE (o4)-[:CONTAINS]->(t8);

MATCH (o5:Order {OrderId: 5}), (t9:Ticket {TicketId: 109})
CREATE (o5)-[:CONTAINS]->(t9);

MATCH (o5:Order {OrderId: 5}), (t10:Ticket {TicketId: 110})
CREATE (o5)-[:CONTAINS]->(t10);

// Create relationships between Tickets and Customers
MATCH (t5:Ticket {TicketId: 105}), (c5:Customer {CustomerId: 5})
CREATE (t5)-[:ASSIGNED_TO]->(c5);

MATCH (t6:Ticket {TicketId: 106}), (c6:Customer {CustomerId: 6})
CREATE (t6)-[:ASSIGNED_TO]->(c6);

MATCH (t7:Ticket {TicketId: 107}), (c1:Customer {CustomerId: 1})
CREATE (t7)-[:ASSIGNED_TO]->(c1);

MATCH (t8:Ticket {TicketId: 108}), (c3:Customer {CustomerId: 3})
CREATE (t8)-[:ASSIGNED_TO]->(c3);

MATCH (t9:Ticket {TicketId: 109}), (c3:Customer {CustomerId: 3})
CREATE (t9)-[:ASSIGNED_TO]->(c3);

MATCH (t10:Ticket {TicketId: 110}), (c6:Customer {CustomerId: 6})
CREATE (t10)-[:ASSIGNED_TO]->(c6);

// Create relationships between Tickets and Flightroutes
MATCH (t5:Ticket {TicketId: 105}), (fr3:Flightroute {FlightrouteId: 3})
CREATE (t5)-[:FOR_FLIGHTROUTE]->(fr3);

MATCH (t6:Ticket {TicketId: 106}), (fr3:Flightroute {FlightrouteId: 3})
CREATE (t6)-[:FOR_FLIGHTROUTE]->(fr3);

MATCH (t7:Ticket {TicketId: 107}), (fr4:Flightroute {FlightrouteId: 4})
CREATE (t7)-[:FOR_FLIGHTROUTE]->(fr4);

MATCH (t8:Ticket {TicketId: 108}), (fr4:Flightroute {FlightrouteId: 4})
CREATE (t8)-[:FOR_FLIGHTROUTE]->(fr4);

MATCH (t9:Ticket {TicketId: 109}), (fr5:Flightroute {FlightrouteId: 5})
CREATE (t9)-[:FOR_FLIGHTROUTE]->(fr5);

MATCH (t10:Ticket {TicketId: 110}), (fr5:Flightroute {FlightrouteId: 5})
CREATE (t10)-[:FOR_FLIGHTROUTE]->(fr5);

// Create relationships between Tickets and TicketTypes
MATCH (t1:Ticket {TicketId: 101}), (tt1:TicketType {TicketTypeId: 1})
CREATE (t1)-[:HAS_TICKET_TYPE]->(tt1);

MATCH (t2:Ticket {TicketId: 102}), (tt2:TicketType {TicketTypeId: 2})
CREATE (t2)-[:HAS_TICKET_TYPE]->(tt2);

MATCH (t3:Ticket {TicketId: 103}), (tt1:TicketType {TicketTypeId: 1})
CREATE (t3)-[:HAS_TICKET_TYPE]->(tt1);

MATCH (t4:Ticket {TicketId: 104}), (tt2:TicketType {TicketTypeId: 2})
CREATE (t4)-[:HAS_TICKET_TYPE]->(tt2);

MATCH (t5:Ticket {TicketId: 105}), (tt3:TicketType {TicketTypeId: 3})
CREATE (t5)-[:HAS_TICKET_TYPE]->(tt3);

MATCH (t6:Ticket {TicketId: 106}), (tt1:TicketType {TicketTypeId: 1})
CREATE (t6)-[:HAS_TICKET_TYPE]->(tt1);


MATCH (t7:Ticket {TicketId: 107}), (tt2:TicketType {TicketTypeId: 2})
CREATE (t7)-[:HAS_TICKET_TYPE]->(tt2);

MATCH (t8:Ticket {TicketId: 108}), (tt3:TicketType {TicketTypeId: 3})
CREATE (t8)-[:HAS_TICKET_TYPE]->(tt3);

MATCH (t9:Ticket {TicketId: 109}), (tt1:TicketType {TicketTypeId: 1})
CREATE (t9)-[:HAS_TICKET_TYPE]->(tt1);

MATCH (t10:Ticket {TicketId: 110}), (tt2:TicketType {TicketTypeId: 2})
CREATE (t10)-[:HAS_TICKET_TYPE]->(tt2);

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

MATCH (p3:Plane {PlaneId: 3}), (a1:Airline {AirlineId: 1})
CREATE (p3)-[:BELONGS_TO]->(a1);

MATCH (p4:Plane {PlaneId: 4}), (a1:Airline {AirlineId: 1})
CREATE (p4)-[:BELONGS_TO]->(a1);

MATCH (p5:Plane {PlaneId: 5}), (a1:Airline {AirlineId: 1})
CREATE (p5)-[:BELONGS_TO]->(a1);

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

MATCH (fr3:Flightroute {FlightrouteId: 3}), (p3:Plane {PlaneId: 3}), (ap3:Airport {AirportId: 3}), (ap4:Airport {AirportId: 4})
CREATE (fr3)-[:USES_PLANE]->(p3)
CREATE (fr3)-[:DEPARTS_FROM]->(ap3)
CREATE (fr3)-[:ARRIVES_AT]->(ap4);

MATCH (fr4:Flightroute {FlightrouteId: 4}), (p4:Plane {PlaneId: 4}), (ap4:Airport {AirportId: 4}), (ap3:Airport {AirportId: 3})
CREATE (fr4)-[:USES_PLANE]->(p4)
CREATE (fr4)-[:DEPARTS_FROM]->(ap4)
CREATE (fr4)-[:ARRIVES_AT]->(ap3);

MATCH (fr5:Flightroute {FlightrouteId: 5}), (p5:Plane {PlaneId: 5}), (ap4:Airport {AirportId: 4}), (ap1:Airport {AirportId: 1})
CREATE (fr5)-[:USES_PLANE]->(p5)
CREATE (fr5)-[:DEPARTS_FROM]->(ap4)
CREATE (fr5)-[:ARRIVES_AT]->(ap1);

MATCH (fr6:Flightroute {FlightrouteId: 6}), (p6:Plane {PlaneId: 6}), (ap6:Airport {AirportId: 6}), (ap5:Airport {AirportId: 5})
CREATE (fr6)-[:USES_PLANE]->(p6)
CREATE (fr6)-[:DEPARTS_FROM]->(ap6)
CREATE (fr6)-[:ARRIVES_AT]->(ap5);

// Create relationships between Luggage and Tickets
MATCH (l1:Luggage {LuggageId: 1}), (t1:Ticket {TicketId: 101})
CREATE (l1)-[:BELONGS_TO]->(t1);

MATCH (l2:Luggage {LuggageId: 2}), (t2:Ticket {TicketId: 102})
CREATE (l2)-[:BELONGS_TO]->(t2);

MATCH (l3:Luggage {LuggageId: 3}), (t3:Ticket {TicketId: 103})
CREATE (l3)-[:BELONGS_TO]->(t3);

MATCH (l4:Luggage {LuggageId: 4}), (t4:Ticket {TicketId: 104})
CREATE (l4)-[:BELONGS_TO]->(t4);

MATCH (l5:Luggage {LuggageId: 5}), (t5:Ticket {TicketId: 105})
CREATE (l5)-[:BELONGS_TO]->(t5);

MATCH (l6:Luggage {LuggageId: 6}), (t6:Ticket {TicketId: 106})
CREATE (l6)-[:BELONGS_TO]->(t6);


