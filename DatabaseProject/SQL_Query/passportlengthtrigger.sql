CREATE TRIGGER trg_ValidatePassportNumber
ON Customers
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @PassportNumberLength INT;
    DECLARE @CustomerId INT;
    DECLARE @FirstName NVARCHAR(50);
    DECLARE @LastName NVARCHAR(50);
    DECLARE @PassportNumber INT;

    SELECT @CustomerId = CustomerId, @FirstName = FirstName, @LastName = LastName, @PassportNumber = PassportNumber
    FROM inserted;

    SET @PassportNumberLength = LEN(CAST(@PassportNumber AS NVARCHAR(50)));

    IF @PassportNumberLength != 9
    BEGIN
        RAISERROR ('Passport number must be exactly 9 characters long.', 16, 1);
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        IF @CustomerId IS NOT NULL AND @CustomerId != 0
        BEGIN
            SET IDENTITY_INSERT Customers ON;

            INSERT INTO Customers (CustomerId, FirstName, LastName, PassportNumber)
            VALUES (@CustomerId, @FirstName, @LastName, @PassportNumber);

            SET IDENTITY_INSERT Customers OFF;
        END
        ELSE
        BEGIN
            INSERT INTO Customers (FirstName, LastName, PassportNumber)
            VALUES (@FirstName, @LastName, @PassportNumber);
        END
    END
END;