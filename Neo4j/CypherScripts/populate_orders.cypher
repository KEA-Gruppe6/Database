// Create Orders
CREATE (o1:Order {OrderId: 1, AirlineConfirmationNumber: 'CONF12345'})
CREATE (o2:Order {OrderId: 2, AirlineConfirmationNumber: 'CONF67890'})

// Create Tickets for Order 1
CREATE (t1:Ticket {TicketId: 101, Price: 150.00, TicketTypeId: 1, CustomerId: 1, DepartureId: 1, OrderId: 1})
CREATE (t2:Ticket {TicketId: 102, Price: 200.00, TicketTypeId: 2, CustomerId: 2, DepartureId: 1, OrderId: 1})

// Create Tickets for Order 2
CREATE (t3:Ticket {TicketId: 103, Price: 250.00, TicketTypeId: 1, CustomerId: 3, DepartureId: 2, OrderId: 2})
CREATE (t4:Ticket {TicketId: 104, Price: 300.00, TicketTypeId: 2, CustomerId: 4, DepartureId: 2, OrderId: 2})

// Create relationships between Orders and Tickets
WITH o1, t1, t2, o2, t3, t4
MATCH (o1:Order {OrderId: 1}), (t1:Ticket {TicketId: 101})
CREATE (o1)-[:CONTAINS]->(t1)

WITH o1, t2, o2, t3, t4
MATCH (o1:Order {OrderId: 1}), (t2:Ticket {TicketId: 102})
CREATE (o1)-[:CONTAINS]->(t2)

WITH o2, t3, t4
MATCH (o2:Order {OrderId: 2}), (t3:Ticket {TicketId: 103})
CREATE (o2)-[:CONTAINS]->(t3)

WITH o2, t4
MATCH (o2:Order {OrderId: 2}), (t4:Ticket {TicketId: 104})
CREATE (o2)-[:CONTAINS]->(t4)
