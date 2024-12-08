USE UddevallaKlätterDB;
GO

CREATE PROCEDURE Nyorder 
@KundId INT,
@ProduktId INT,
@Antal INT,
@OrderDatum DATETIME,
@OrderId INT OUTPUT
AS
INSERT INTO Ordrar (KundId, ProduktId, Antal, OrderDatum)
VALUES (@KundId, @ProduktId, @Antal, @OrderDatum);

SET @OrderId = SCOPE_IDENTITY();