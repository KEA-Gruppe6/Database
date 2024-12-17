use AirportDB;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CustomersAudit')
BEGIN
    CREATE TABLE CustomersAudit (
        AuditId BIGINT IDENTITY(1,1) PRIMARY KEY,
        CustomerId BIGINT NOT NULL,
        FirstName NVARCHAR(100),
        LastName NVARCHAR(100),
        PassportNumber INT,
        CreatedDate DATETIME NOT NULL,
        CreatedBy NVARCHAR(100) NOT NULL,
        ModifiedDate DATETIME NULL,
        ModifiedBy NVARCHAR(100) NULL,
        Operation NVARCHAR(10) NOT NULL
    );
END
GO

CREATE TRIGGER trgAuditCustomerInsert
ON Customers
AFTER INSERT
AS
BEGIN
    INSERT INTO CustomersAudit (CustomerId, FirstName, LastName, PassportNumber, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy, Operation)
    SELECT CustomerId, FirstName, LastName, PassportNumber, CreatedDate, CreatedBy, NULL, NULL, 'INSERT'
    FROM inserted;
END;
GO

CREATE TRIGGER trgAuditCustomerUpdate
ON Customers
AFTER UPDATE
AS
BEGIN
    INSERT INTO CustomersAudit (CustomerId, FirstName, LastName, PassportNumber, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy, Operation)
    SELECT CustomerId, FirstName, LastName, PassportNumber, CreatedDate, CreatedBy, GETDATE(), SYSTEM_USER, 'UPDATE'
    FROM inserted;
    
    UPDATE Customers
    SET ModifiedBy = SYSTEM_USER, ModifiedDate = GETDATE()
    FROM inserted i
    WHERE Customers.CustomerId = i.CustomerId;
END;
GO

CREATE TRIGGER trgAuditCustomerDelete
ON Customers
AFTER DELETE
AS
BEGIN
    INSERT INTO CustomersAudit (CustomerId, FirstName, LastName, PassportNumber, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy, Operation)
    SELECT CustomerId, FirstName, LastName, PassportNumber, CreatedDate, CreatedBy, GETDATE(), SYSTEM_USER, 'DELETE'
    FROM deleted;
END;
GO