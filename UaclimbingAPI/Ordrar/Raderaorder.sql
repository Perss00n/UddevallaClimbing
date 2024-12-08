USE UddevallaKlätterDB;
GO

CREATE PROCEDURE Raderaorder
@OrderId INT
AS
IF EXISTS (SELECT 1 FROM Ordrar WHERE OrderId = @OrderId)
DELETE FROM Ordrar
WHERE OrderId = @OrderId;
ELSE
RAISERROR ('Ordern med angivet ID finns inte!', 16, 1);