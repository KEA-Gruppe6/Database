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

IF EXISTS (SELECT * FROM sys.views WHERE name = 'CustomersDeletedIsNull')
BEGIN
    DROP VIEW CustomersDeletedIsNull;
END
GO

CREATE VIEW CustomersDeletedIsNull AS
SELECT 
    c.CustomerId,
    CASE 
        WHEN ca.Operation = 'DELETE' THEN NULL
        ELSE c.FirstName
    END AS FirstName,
    CASE 
        WHEN ca.Operation = 'DELETE' THEN NULL
        ELSE c.LastName
    END AS LastName,
    CASE 
        WHEN ca.Operation = 'DELETE' THEN NULL
        ELSE c.PassportNumber
    END AS PassportNumber,
    CASE 
        WHEN ca.Operation = 'DELETE' THEN NULL
        ELSE c.CreatedDate
    END AS CreatedDate,
    CASE 
        WHEN ca.Operation = 'DELETE' THEN NULL
        ELSE c.CreatedBy
    END AS CreatedBy,
    CASE 
        WHEN ca.Operation = 'DELETE' THEN NULL
        ELSE c.ModifiedDate
    END AS ModifiedDate,
    CASE 
        WHEN ca.Operation = 'DELETE' THEN NULL
        ELSE c.ModifiedBy
    END AS ModifiedBy
FROM 
    Customers c
LEFT JOIN 
    CustomersAudit ca ON c.CustomerId = ca.CustomerId AND ca.Operation = 'DELETE';
GO

CREATE TRIGGER trg_AuditCustomerInsert
ON Customers
AFTER INSERT
AS
BEGIN
    INSERT INTO CustomersAudit (CustomerId, FirstName, LastName, PassportNumber, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy, Operation)
    SELECT CustomerId, FirstName, LastName, PassportNumber, CreatedDate, CreatedBy, NULL, NULL, 'INSERT'
    FROM inserted;
END;
GO

CREATE TRIGGER trg_AuditCustomerUpdate
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

CREATE TRIGGER trg_AuditCustomerDelete
ON Customers
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM CustomersAudit WHERE CustomerId IN (SELECT CustomerId FROM deleted) AND Operation = 'DELETE')
    BEGIN
        RAISERROR ('Customer is already marked as deleted.', 16, 1);
        RETURN;
    END

    INSERT INTO CustomersAudit (CustomerId, FirstName, LastName, PassportNumber, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy, Operation)
    SELECT CustomerId, FirstName, LastName, PassportNumber, CreatedDate, CreatedBy, GETDATE(), SYSTEM_USER, 'DELETE'
    FROM deleted;
END;
GO