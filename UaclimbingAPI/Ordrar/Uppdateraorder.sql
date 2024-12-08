USE UddevallaKlätterDB;
GO

CREATE PROCEDURE UppdateraOrder
@OrderId INT,
@KundId INT,
@ProduktId INT,
@Antal INT,
@OrderDatum DATETIME
AS
IF EXISTS (SELECT 1 FROM Ordrar WHERE OrderId = @OrderId)
-- Jag har valt att bygga mitt API på sättet att om man uppdaterar en kund och lämnar fältet tomt så ska nuvarande värde behållas
-- Därför använder jag isnull här vilket sätter värdet till det som redan existerar annars uppdaterar den det med det nya värdet
UPDATE Ordrar
SET KundId = ISNULL(@KundId, KundId),
    ProduktId = ISNULL(@ProduktId, ProduktId),
    Antal = ISNULL(@Antal, Antal),
    OrderDatum = ISNULL(@OrderDatum, OrderDatum)
    WHERE OrderId = @OrderId;
ELSE
RAISERROR ('Ordern med angivet ID finns inte!', 16, 1);
