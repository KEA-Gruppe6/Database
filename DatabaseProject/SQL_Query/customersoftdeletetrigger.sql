CREATE TRIGGER trg_SoftDeleteCustomer
ON Customers
INSTEAD OF DELETE
AS
BEGIN
    UPDATE Customers
    SET IsDeleted = 1
    FROM Customers
    INNER JOIN deleted ON Customers.CustomerId = deleted.CustomerId;
END;