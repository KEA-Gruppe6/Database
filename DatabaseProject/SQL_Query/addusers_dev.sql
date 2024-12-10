-- Check if the database exists and create it if it doesn't
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AirportDB')
BEGIN
    CREATE DATABASE AirportDB;
END
GO

USE AirportDB;
GO

-- Create a login for the admin user if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'adminUser')
BEGIN
    CREATE LOGIN adminUser WITH PASSWORD = 'exampleAdmin0';
END
GO

-- Create a user for the admin login if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'adminUser')
BEGIN
    CREATE USER adminUser FOR LOGIN adminUser;
    ALTER ROLE db_datareader ADD MEMBER adminUser;
    ALTER ROLE db_datawriter ADD MEMBER adminUser;
    ALTER ROLE db_owner ADD MEMBER adminUser;
END
GO

-- Create a login for the app user if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'appUser')
BEGIN
    CREATE LOGIN appUser WITH PASSWORD = 'exampleUser0';
END
GO

-- Create a user for the app login if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'appUser')
BEGIN
    CREATE USER appUser FOR LOGIN appUser;
    ALTER ROLE db_datareader ADD MEMBER appUser;
    ALTER ROLE db_datawriter ADD MEMBER appUser;
END
GO

-- Create a login for the read only user if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'readOnlyUser')
BEGIN
    CREATE LOGIN readOnlyUser WITH PASSWORD = 'exampleReadOnly0';
END
GO

-- Create a user for the read only login if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'readOnlyUser')
BEGIN
    CREATE USER readOnlyUser FOR LOGIN readOnlyUser;
    ALTER ROLE db_datareader ADD MEMBER readOnlyUser;
END
GO

-- Ensure the users have the appropriate permissions
GRANT SELECT ON SCHEMA::dbo TO readOnlyUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO appUser;
GRANT CONTROL ON SCHEMA::dbo TO adminUser;
GO