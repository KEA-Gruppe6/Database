USE AirportDB;

-- Create roles if they don't exist
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'AdminRole')
BEGIN
    CREATE ROLE AdminRole;
END;

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'AirlineRole')
BEGIN
    CREATE ROLE AirlineRole;
END;

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'CustomerRole')
BEGIN
    CREATE ROLE CustomerRole;
END;

-- Grant permissions to AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.TicketTypes TO AdminRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Planes TO AdminRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Flightroutes TO AdminRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Maintenances TO AdminRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Tickets TO AdminRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Luggage TO AdminRole;

-- Grant permissions to AirlineRole
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Planes TO AirlineRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Flightroutes TO AirlineRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Maintenances TO AirlineRole;

-- Grant permissions to CustomerRole
GRANT SELECT ON dbo.TicketTypes TO CustomerRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Tickets TO CustomerRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Luggage TO CustomerRole;
GRANT SELECT ON dbo.Flightroutes TO CustomerRole;
GRANT SELECT ON dbo.Planes TO CustomerRole;