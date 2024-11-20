INSERT INTO Airlines (AirlineName)
VALUES
    ('Delta Airlines'),
    ('American Airlines'),
    ('United Airlines'),
    ('Southwest Airlines'),
    ('JetBlue Airways'),
    ('Alaska Airlines'),
    ('Spirit Airlines'),
    ('Hawaiian Airlines'),
    ('SkyWest Airlines'),
    ('Air Canada');

INSERT INTO Airports (AirportName, AirportCity, Municipality, AirportAbbreviation)
VALUES
    ('Copenhagen Airport', 'Copenhagen', 'Capital Region of Denmark', 'CPH'),
    ('Aalborg Airport', 'Aalborg', 'North Jutland Region', 'AAL'),
    ('Billund Airport', 'Billund', 'South Denmark', 'BLL'),
    ('Esbjerg Airport', 'Esbjerg', 'South Denmark', 'EBJ'),
    ('Aarhus Airport', 'Aarhus', 'Central Denmark Region', 'AAR'),
    ('Odense Airport', 'Odense', 'Southern Denmark', 'ODE'),
    ('Bornholm Airport', 'Rønne', 'Capital Region of Denmark', 'RNN'),
    ('Karup Airport', 'Karup', 'Central Jutland Region', 'KRP'),
    ('Sonderborg Airport', 'Sønderborg', 'Southern Denmark', 'SGD'),
    ('Roskilde Airport', 'Roskilde', 'Zealand Region', 'RKE');

INSERT INTO Customers (FirstName, LastName, PassportNumber)
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

INSERT INTO Luggage (MaxWeight, IsCarryOn)
VALUES
    (10.5, 1),
    (15.0, 0),
    (20.0, 1),
    (25.0, 0),
    (18.0, 1),
    (22.5, 0),
    (12.0, 1),
    (30.0, 0),
    (8.0, 1),
    (19.5, 0),
    (23.0, 1),
    (28.0, 0),
    (14.0, 1),
    (26.0, 0),
    (17.0, 1),
    (21.0, 0),
    (11.0, 1),
    (24.0, 0),
    (16.5, 1),
    (27.0, 0);

INSERT INTO TicketTypes (TicketTypeName)
VALUES
    ('Economy'),
    ('Business'),
    ('First Class');

INSERT INTO Departures (DepartureTime, ArrivalTime, DepartureAirportId, ArrivalAirportId)
VALUES
    ('2024-11-15 08:00:00', '2024-11-15 10:30:00', 1, 2),
    ('2024-11-15 09:30:00', '2024-11-15 12:00:00', 3, 4),
    ('2024-11-15 10:00:00', '2024-11-15 13:00:00', 5, 6),
    ('2024-11-15 11:00:00', '2024-11-15 13:30:00', 7, 8),
    ('2024-11-15 12:00:00', '2024-11-15 14:30:00', 9, 10),
    ('2024-11-15 13:00:00', '2024-11-15 15:30:00', 2, 1),
    ('2024-11-15 14:00:00', '2024-11-15 16:30:00', 4, 3),
    ('2024-11-15 15:00:00', '2024-11-15 17:30:00', 6, 5),
    ('2024-11-15 16:00:00', '2024-11-15 18:30:00', 8, 7),
    ('2024-11-15 17:00:00', '2024-11-15 19:30:00', 10, 9);

INSERT INTO Planes (PlaneDisplayName, AirlineId)
VALUES
    ('Boeing 737', 1),
    ('Airbus A320', 2),
    ('Boeing 777', 1),
    ('Airbus A350', 2),
    ('Boeing 787', 3),
    ('Airbus A380', 4),
    ('Boeing 767', 3),
    ('Airbus A321', 5),
    ('Boeing 737 MAX', 6),
    ('Airbus A220', 7);

INSERT INTO Tickets (Price, TicketTypeId, CustomerId, DepartureId)
VALUES
    (150.00, 1, 1, 1),
    (200.00, 2, 2, 2),
    (100.00, 3, 3, 3),
    (250.00, 3, 4, 4),
    (180.00, 1, 5, 5),
    (220.00, 2, 6, 6),
    (120.00, 3, 7, 7),
    (300.00, 2, 8, 8),
    (160.00, 1, 9, 9),
    (210.00, 2, 10, 10);


Select * From Airlines

SELECT * From Planes

SELECT p.PlaneDisplayName, a.AirlineName
FROM Planes p
         JOIN Airlines a ON p.AirlineId = a.AirlineId
WHERE a.AirlineName = 'Delta Airlines';