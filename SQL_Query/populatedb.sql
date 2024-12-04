MERGE INTO Airlines AS target
USING (VALUES
    ('Delta Airlines'),
    ('American Airlines'),
    ('United Airlines'),
    ('Southwest Airlines'),
    ('JetBlue Airways'),
    ('Alaska Airlines'),
    ('Spirit Airlines'),
    ('Hawaiian Airlines'),
    ('SkyWest Airlines'),
    ('Air Canada')
) AS source (AirlineName)
ON target.AirlineName = source.AirlineName
WHEN NOT MATCHED BY TARGET THEN
    INSERT (AirlineName) VALUES (source.AirlineName);

MERGE INTO Airports AS target
USING (VALUES
    ('Copenhagen Airport', 'Copenhagen', 'Capital Region of Denmark', 'CPH'),
    ('Aalborg Airport', 'Aalborg', 'North Jutland Region', 'AAL'),
    ('Billund Airport', 'Billund', 'South Denmark', 'BLL'),
    ('Esbjerg Airport', 'Esbjerg', 'South Denmark', 'EBJ'),
    ('Aarhus Airport', 'Aarhus', 'Central Denmark Region', 'AAR'),
    ('Odense Airport', 'Odense', 'Southern Denmark', 'ODE'),
    ('Bornholm Airport', 'Rønne', 'Capital Region of Denmark', 'RNN'),
    ('Karup Airport', 'Karup', 'Central Jutland Region', 'KRP'),
    ('Sonderborg Airport', 'Sønderborg', 'Southern Denmark', 'SGD'),
    ('Roskilde Airport', 'Roskilde', 'Zealand Region', 'RKE')
) AS source (AirportName, AirportCity, Municipality, AirportAbbreviation)
ON target.AirportName = source.AirportName
WHEN NOT MATCHED BY TARGET THEN
    INSERT (AirportName, AirportCity, Municipality, AirportAbbreviation)
    VALUES (source.AirportName, source.AirportCity, source.Municipality, source.AirportAbbreviation);

DECLARE @Customers TABLE (FirstName NVARCHAR(50), LastName NVARCHAR(50), PassportNumber INT);

INSERT INTO @Customers (FirstName, LastName, PassportNumber)
VALUES
    ('John', 'Doe', 123456789),
    ('Jane', 'Smith', 987654321),
    ('Michael', 'Johnson', 112233445),
    ('Emily', 'Davis', 556677889),
    ('James', 'Brown', 223344556),
    ('Olivia', 'Williams', 998877665),
    ('William', 'Jones', 334455667),
    ('Sophia', 'Miller', 776655443),
    ('David', 'Wilson', 445566778),
    ('Isabella', 'Moore', 889977665),
    ('Benjamin', 'Taylor', 123987654),
    ('Mia', 'Anderson', 667788990),
    ('Elijah', 'Thomas', 334411223),
    ('Amelia', 'Jackson', 775544332),
    ('Lucas', 'White', 998822233),
    ('Harper', 'Harris', 446688557),
    ('Alexander', 'Martin', 229988445),
    ('Evelyn', 'Garcia', 887766554),
    ('Jackson', 'Rodriguez', 223344888),
    ('Charlotte', 'Martinez', 998877224);

DECLARE @FirstName NVARCHAR(50);
DECLARE @LastName NVARCHAR(50);
DECLARE @PassportNumber INT;

DECLARE customer_cursor CURSOR FOR
    SELECT FirstName, LastName, PassportNumber
    FROM @Customers;

OPEN customer_cursor;

FETCH NEXT FROM customer_cursor INTO @FirstName, @LastName, @PassportNumber;

WHILE @@FETCH_STATUS = 0
    BEGIN
        IF NOT EXISTS (
            SELECT 1
            FROM Customers
            WHERE PassportNumber = @PassportNumber
        )
            BEGIN
                INSERT INTO Customers (FirstName, LastName, PassportNumber)
                VALUES (@FirstName, @LastName, @PassportNumber);
            END

        FETCH NEXT FROM customer_cursor INTO @FirstName, @LastName, @PassportNumber;
    END

CLOSE customer_cursor;
DEALLOCATE customer_cursor;

MERGE INTO Orders AS target
USING (VALUES
    ('111AAA'),
    ('222BBB'),
    ('333CCC'),
    ('444DDD'),
    ('555EEE'),
    ('666FFF'),
    ('777GGG'),
    ('888HHH'),
    ('999III'),
    ('101JJJ')
) AS source (AirlineConfirmationNumber)
ON target.AirlineConfirmationNumber = source.AirlineConfirmationNumber
WHEN NOT MATCHED BY TARGET THEN
    INSERT (AirlineConfirmationNumber)
    VALUES (source.AirlineConfirmationNumber);

MERGE INTO TicketTypes AS target
USING (VALUES
    ('Economy'),
    ('Business'),
    ('First Class')
) AS source (TicketTypeName)
ON target.TicketTypeName = source.TicketTypeName
WHEN NOT MATCHED BY TARGET THEN
    INSERT (TicketTypeName)
    VALUES (source.TicketTypeName);

MERGE INTO Departures AS target
USING (VALUES
    ('2024-11-15 08:00:00', '2024-11-15 10:30:00', 1, 2),
    ('2024-11-15 09:30:00', '2024-11-15 12:00:00', 3, 4),
    ('2024-11-15 10:00:00', '2024-11-15 13:00:00', 5, 6),
    ('2024-11-15 11:00:00', '2024-11-15 13:30:00', 7, 8),
    ('2024-11-15 12:00:00', '2024-11-15 14:30:00', 9, 10),
    ('2024-11-15 13:00:00', '2024-11-15 15:30:00', 2, 1),
    ('2024-11-15 14:00:00', '2024-11-15 16:30:00', 4, 3),
    ('2024-11-15 15:00:00', '2024-11-15 17:30:00', 6, 5),
    ('2024-11-15 16:00:00', '2024-11-15 18:30:00', 8, 7),
    ('2024-11-15 17:00:00', '2024-11-15 19:30:00', 10, 9)
) AS source (DepartureTime, ArrivalTime, DepartureAirportId, ArrivalAirportId)
ON target.DepartureTime = source.DepartureTime AND target.ArrivalTime = source.ArrivalTime AND target.DepartureAirportId = source.DepartureAirportId AND target.ArrivalAirportId = source.ArrivalAirportId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (DepartureTime, ArrivalTime, DepartureAirportId, ArrivalAirportId)
    VALUES (source.DepartureTime, source.ArrivalTime, source.DepartureAirportId, source.ArrivalAirportId);

MERGE INTO Planes AS target
USING (VALUES
    ('Boeing 737', 1),
    ('Airbus A320', 2),
    ('Boeing 777', 1),
    ('Airbus A350', 2),
    ('Boeing 787', 3),
    ('Airbus A380', 4),
    ('Boeing 767', 3),
    ('Airbus A321', 5),
    ('Boeing 737 MAX', 6),
    ('Airbus A220', 7)
) AS source (PlaneDisplayName, AirlineId)
ON target.PlaneDisplayName = source.PlaneDisplayName AND target.AirlineId = source.AirlineId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (PlaneDisplayName, AirlineId)
    VALUES (source.PlaneDisplayName, source.AirlineId);

MERGE INTO Tickets AS target
USING (VALUES
    (150.00, 1, 1, 1, 1),
    (200.00, 2, 2, 2, 2),
    (100.00, 3, 3, 3, 3),
    (250.00, 3, 4, 4, 4),
    (180.00, 1, 5, 5, 5),
    (220.00, 2, 6, 6, 6),
    (120.00, 3, 7, 7, 7),
    (300.00, 2, 8, 8, 8),
    (160.00, 1, 9, 9, 9),
    (210.00, 2, 10, 10, 10)
) AS source (Price, TicketTypeId, CustomerId, DepartureId, OrderId)
ON target.Price = source.Price AND target.TicketTypeId = source.TicketTypeId AND target.CustomerId = source.CustomerId AND target.DepartureId = source.DepartureId AND target.OrderId = source.OrderId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Price, TicketTypeId, CustomerId, DepartureId, OrderId)
    VALUES (source.Price, source.TicketTypeId, source.CustomerId, source.DepartureId, source.OrderId);

MERGE INTO Luggage AS target
USING (VALUES
    (10.5, 1, 1),
    (15.0, 0, 1),
    (20.0, 1, 2),
    (25.0, 0, 2),
    (18.0, 1, 3),
    (22.5, 0, 3),
    (12.0, 1, 3),
    (30.0, 0, 4),
    (8.0, 1, 5),
    (19.5, 0, 5),
    (23.0, 1, 6),
    (28.0, 0, 6),
    (14.0, 1, 6),
    (26.0, 0, 6),
    (17.0, 1, 7),
    (21.0, 0, 7),
    (11.0, 1, 8),
    (24.0, 0, 8),
    (16.5, 1, 9),
    (20.5, 0, 9),
    (29.0, 1, 10),
    (27.0, 0, 10)
) AS source (MaxWeight, IsCarryOn, TicketId)
ON target.MaxWeight = source.MaxWeight AND target.IsCarryOn = source.IsCarryOn AND target.TicketId = source.TicketId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (MaxWeight, IsCarryOn, TicketId)
    VALUES (source.MaxWeight, source.IsCarryOn, source.TicketId);

MERGE INTO Maintenances AS target
USING (VALUES
    (1,1),
    (2,2),
    (3,3),
    (4,4),
    (5,5),
    (6,6),
    (7,7),
    (8,8),
    (9,9),
    (10,10)
) AS source (AirportId, PlaneId)
ON target.AirportId = source.AirportId AND target.PlaneId = source.PlaneId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (AirportId, PlaneId)
    VALUES (source.AirportId, source.PlaneId);