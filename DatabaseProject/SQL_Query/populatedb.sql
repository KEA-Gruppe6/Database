SET IDENTITY_INSERT Airlines ON;
MERGE INTO Airlines AS target
USING (VALUES
    (1, 'Delta Airlines'),
    (2, 'American Airlines'),
    (3, 'United Airlines'),
    (4, 'Southwest Airlines'),
    (5, 'JetBlue Airways'),
    (6, 'Alaska Airlines'),
    (7, 'Spirit Airlines'),
    (8, 'Hawaiian Airlines'),
    (9, 'SkyWest Airlines'),
    (10, 'Air Canada')
) AS source (AirlineId, AirlineName)
ON target.AirlineId = source.AirlineId
WHEN MATCHED THEN
    UPDATE SET AirlineName = source.AirlineName
WHEN NOT MATCHED BY TARGET THEN
    INSERT (AirlineId, AirlineName)
    VALUES (source.AirlineId, source.AirlineName);
SET IDENTITY_INSERT Airlines OFF;

SET IDENTITY_INSERT Airports ON;
MERGE INTO Airports AS target
USING (VALUES
    (1,'Copenhagen Airport', 'Copenhagen', 'Capital Region of Denmark', 'CPH'),
    (2,'Aalborg Airport', 'Aalborg', 'North Jutland Region', 'AAL'),
    (3,'Billund Airport', 'Billund', 'South Denmark', 'BLL'),
    (4,'Esbjerg Airport', 'Esbjerg', 'South Denmark', 'EBJ'),
    (5,'Aarhus Airport', 'Aarhus', 'Central Denmark Region', 'AAR'),
    (6,'Odense Airport', 'Odense', 'Southern Denmark', 'ODE'),
    (7,'Bornholm Airport', 'Rønne', 'Capital Region of Denmark', 'RNN'),
    (8,'Karup Airport', 'Karup', 'Central Jutland Region', 'KRP'),
    (9,'Sonderborg Airport', 'Sønderborg', 'Southern Denmark', 'SGD'),
    (10,'Roskilde Airport', 'Roskilde', 'Zealand Region', 'RKE')
) AS source (AirportId, AirportName, AirportCity, Municipality, AirportAbbreviation)
ON target.AirportId = source.AirportId
WHEN MATCHED THEN
    UPDATE SET AirportName = source.AirportName, AirportCity = source.AirportCity, Municipality = source.Municipality, AirportAbbreviation = source.AirportAbbreviation
WHEN NOT MATCHED BY TARGET THEN
    INSERT (AirportId, AirportName, AirportCity, Municipality, AirportAbbreviation)
    VALUES (source.AirportId, source.AirportName, source.AirportCity, source.Municipality, source.AirportAbbreviation);
SET IDENTITY_INSERT Airports OFF;

SET IDENTITY_INSERT Customers ON;
DECLARE @Customers TABLE (CustomerId BIGINT, FirstName NVARCHAR(50), LastName NVARCHAR(50), PassportNumber INT);

INSERT INTO @Customers (CustomerId, FirstName, LastName, PassportNumber)
VALUES
    (1,'John', 'Doe', 123456789),
    (2,'Jane', 'Smith', 987654321),
    (3,'Michael', 'Johnson', 112233445),
    (4,'Emily', 'Davis', 556677889),
    (5,'James', 'Brown', 223344556),
    (6,'Olivia', 'Williams', 998877665),
    (7,'William', 'Jones', 334455667),
    (8,'Sophia', 'Miller', 776655443),
    (9,'David', 'Wilson', 445566778),
    (10,'Isabella', 'Moore', 889977665),
    (11,'Benjamin', 'Taylor', 123987654),
    (12,'Mia', 'Anderson', 667788990),
    (13,'Elijah', 'Thomas', 334411223),
    (14,'Amelia', 'Jackson', 775544332),
    (15,'Lucas', 'White', 998822233),
    (16,'Harper', 'Harris', 446688557),
    (17,'Alexander', 'Martin', 229988445),
    (18,'Evelyn', 'Garcia', 887766554),
    (19,'Jackson', 'Rodriguez', 223344888),
    (20,'Charlotte', 'Martinez', 998877224);

DECLARE @CustomerId BIGINT;
DECLARE @FirstName NVARCHAR(50);
DECLARE @LastName NVARCHAR(50);
DECLARE @PassportNumber INT;

DECLARE customer_cursor CURSOR FOR
    SELECT CustomerId, FirstName, LastName, PassportNumber
    FROM @Customers;

OPEN customer_cursor;

FETCH NEXT FROM customer_cursor INTO @CustomerId, @FirstName, @LastName, @PassportNumber;

WHILE @@FETCH_STATUS = 0
    BEGIN
        IF NOT EXISTS (
            SELECT 1
            FROM Customers
            WHERE PassportNumber = @PassportNumber
        )
            BEGIN
                INSERT INTO Customers (CustomerId, FirstName, LastName, PassportNumber)
                VALUES (@CustomerId, @FirstName, @LastName, @PassportNumber);
            END

        FETCH NEXT FROM customer_cursor INTO @CustomerId, @FirstName, @LastName, @PassportNumber;
    END

CLOSE customer_cursor;
DEALLOCATE customer_cursor;
SET IDENTITY_INSERT Customers OFF;

SET IDENTITY_INSERT Orders ON;
MERGE INTO Orders AS target
USING (VALUES
    (1,'111AAA'),
    (2,'222BBB'),
    (3,'333CCC'),
    (4,'444DDD'),
    (5,'555EEE'),
    (6,'666FFF'),
    (7,'777GGG'),
    (8,'888HHH'),
    (9,'999III'),
    (10,'101JJJ')
) AS source (OrderId, AirlineConfirmationNumber)
ON target.OrderId = source.OrderID
WHEN MATCHED THEN
    UPDATE SET AirlineConfirmationNumber = source.AirlineConfirmationNumber
WHEN NOT MATCHED BY TARGET THEN
    INSERT (OrderId, AirlineConfirmationNumber)
    VALUES (source.OrderId, source.AirlineConfirmationNumber);
SET IDENTITY_INSERT Orders OFF;

SET IDENTITY_INSERT TicketTypes ON;
MERGE INTO TicketTypes AS target
USING (VALUES
    (1,'Economy'),
    (2,'Business'),
    (3,'First Class')
) AS source (TicketTypeId, TicketTypeName)
ON target.TicketTypeId = source.TicketTypeId
WHEN MATCHED THEN
    UPDATE SET TicketTypeName = source.TicketTypeName
WHEN NOT MATCHED BY TARGET THEN
    INSERT (TicketTypeId, TicketTypeName)
    VALUES (source.TicketTypeId, source.TicketTypeName);
SET IDENTITY_INSERT TicketTypes OFF;

SET IDENTITY_INSERT Planes ON;
MERGE INTO Planes AS target
USING (VALUES
    (1,'Boeing 737', 1),
    (2,'Airbus A320', 2),
    (3,'Boeing 777', 1),
    (4,'Airbus A350', 2),
    (5,'Boeing 787', 3),
    (6,'Airbus A380', 4),
    (7,'Boeing 767', 3),
    (8,'Airbus A321', 5),
    (9,'Boeing 737 MAX', 6),
    (10,'Airbus A220', 7) -- Changed AirlineId to 10
) AS source (PlaneId, PlaneDisplayName, AirlineId)
ON target.PlaneId = source.PlaneId
WHEN MATCHED THEN
    UPDATE SET PlaneDisplayName = source.PlaneDisplayName, AirlineId = source.AirlineId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (PlaneId, PlaneDisplayName, AirlineId)
    VALUES (source.PlaneId, source.PlaneDisplayName, source.AirlineId);
SET IDENTITY_INSERT Planes OFF;

SET IDENTITY_INSERT Flightroutes ON;
MERGE INTO Flightroutes AS target
USING (VALUES
    (1,'2024-11-15 08:00:00', '2024-11-15 10:30:00', 1, 2, 1),
    (2,'2024-11-15 09:30:00', '2024-11-15 12:00:00', 3, 4, 2),
    (3,'2024-11-15 10:00:00', '2024-11-15 13:00:00', 5, 6, 3),
    (4,'2024-11-15 11:00:00', '2024-11-15 13:30:00', 7, 8, 4),
    (5,'2024-11-15 12:00:00', '2024-11-15 14:30:00', 9, 10, 5),
    (6,'2024-11-15 13:00:00', '2024-11-15 15:30:00', 2, 1, 6),
    (7,'2024-11-15 14:00:00', '2024-11-15 16:30:00', 4, 3, 7),
    (8,'2024-11-15 15:00:00', '2024-11-15 17:30:00', 6, 5, 8),
    (9,'2024-11-15 16:00:00', '2024-11-15 18:30:00', 8, 7, 9),
    (10,'2024-11-15 17:00:00', '2024-11-15 19:30:00', 10, 9, 10)
) AS source (FlightrouteId, DepartureTime, ArrivalTime, DepartureAirportId, ArrivalAirportId, PlaneId)
ON target.FlightrouteId = source.FlightrouteId
WHEN MATCHED THEN
    UPDATE SET DepartureTime = source.DepartureTime, ArrivalTime = source.ArrivalTime, DepartureAirportId = source.DepartureAirportId, ArrivalAirportId = source.ArrivalAirportId, PlaneId = source.PlaneId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (FlightrouteId, DepartureTime, ArrivalTime, DepartureAirportId, ArrivalAirportId, PlaneId)
    VALUES (source.FlightrouteId, source.DepartureTime, source.ArrivalTime, source.DepartureAirportId, source.ArrivalAirportId, source.PlaneId);
SET IDENTITY_INSERT Flightroutes OFF;

SET IDENTITY_INSERT Tickets ON;
MERGE INTO Tickets AS target
USING (VALUES
    (1, 150.00, 1, 1, 1, 1),
    (2, 200.00, 2, 2, 2, 2),
    (3, 100.00, 3, 3, 3, 3),
    (4, 250.00, 3, 4, 4, 4),
    (5, 180.00, 1, 5, 5, 5),
    (6, 220.00, 2, 6, 6, 6),
    (7, 120.00, 3, 7, 7, 7),
    (8, 300.00, 2, 8, 8, 8),
    (9, 160.00, 1, 9, 9, 9),
    (10, 210.00, 2, 10, 10, 10)
) AS source (TicketId, Price, TicketTypeId, CustomerId, FlightrouteId, OrderId)
ON target.TicketId = source.TicketId
WHEN MATCHED THEN
    UPDATE SET Price = source.Price, TicketTypeId = source.TicketTypeId, CustomerId = source.CustomerId, FlightrouteId = source.FlightrouteId, OrderId = source.OrderId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (TicketId, Price, TicketTypeId, CustomerId, FlightrouteId, OrderId)
    VALUES (source.TicketId, source.Price, source.TicketTypeId, source.CustomerId, source.FlightrouteId, source.OrderId);
SET IDENTITY_INSERT Tickets OFF;

SET IDENTITY_INSERT Luggage ON;
MERGE INTO Luggage AS target
USING (VALUES
    (1, 10.5, 1, 1),
    (2, 15.0, 0, 1),
    (3, 20.0, 1, 2),
    (4, 25.0, 0, 2),
    (5, 18.0, 1, 3),
    (6, 22.5, 0, 3),
    (7, 12.0, 1, 3),
    (8, 30.0, 0, 4),
    (9, 8.0, 1, 5),
    (10, 19.5, 0, 5),
    (11, 23.0, 1, 6),
    (12, 28.0, 0, 6),
    (13, 14.0, 1, 6),
    (14, 26.0, 0, 6),
    (15, 17.0, 1, 7),
    (16, 21.0, 0, 7),
    (17, 11.0, 1, 8),
    (18, 24.0, 0, 8),
    (19, 16.5, 1, 9),
    (20, 20.5, 0, 9),
    (21, 29.0, 1, 10),
    (22, 27.0, 0, 10)
) AS source (LuggageId, Weight, IsCarryOn, TicketId)
ON target.LuggageId = source.LuggageId
WHEN MATCHED THEN
    UPDATE SET Weight = source.Weight, IsCarryOn = source.IsCarryOn, TicketId = source.TicketId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (LuggageId, Weight, IsCarryOn, TicketId)
    VALUES (source.LuggageId, source.Weight, source.IsCarryOn, source.TicketId);
SET IDENTITY_INSERT Luggage OFF;

SET IDENTITY_INSERT Maintenances ON;
MERGE INTO Maintenances AS target
USING (VALUES
    (1,1,1, '2024-11-15 08:00:00', '2024-11-15 10:30:00'),
    (2,2,2, '2024-11-15 09:30:00', '2024-11-15 12:00:00'),
    (3,3,3, '2024-11-15 10:00:00', '2024-11-15 13:00:00'),
    (5,4,4, '2024-11-15 11:00:00', '2024-11-15 13:30:00'),
    (6,5,5, '2024-11-15 12:00:00', '2024-11-15 14:30:00'),
    (7,6,6, '2024-11-15 13:00:00', '2024-11-15 15:30:00'),
    (8,7,7, '2024-11-15 14:00:00', '2024-11-15 16:30:00'),
    (9,8,8, '2024-11-15 15:00:00', '2024-11-15 17:30:00'),
    (0,9,9, '2024-11-15 16:00:00', '2024-11-15 18:30:00'),
    (10,10,10, '2024-11-15 17:00:00', '2024-11-15 19:30:00')
) AS source (MaintenanceId, AirportId, PlaneId, StartDate, EndDate)
ON target.MaintenanceId = source.MaintenanceId
WHEN MATCHED THEN
    UPDATE SET AirportId = source.AirportId, PlaneId = source.PlaneId, StartDate = source.StartDate, EndDate = source.EndDate
WHEN NOT MATCHED BY TARGET THEN
    INSERT (MaintenanceId, AirportId, PlaneId, StartDate, EndDate)
    VALUES (source.MaintenanceId, source.AirportId, source.PlaneId, SOURCE.StartDate, SOURCE.EndDate);
SET IDENTITY_INSERT Maintenances OFF;