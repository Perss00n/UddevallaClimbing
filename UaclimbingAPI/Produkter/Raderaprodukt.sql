USE UddevallaKlätterDB;
GO

CREATE PROCEDURE Raderaprodukt
@ProduktId INT
AS
IF EXISTS (SELECT 1 FROM Produkter WHERE ProduktId = @ProduktId)
DELETE FROM Produkter
WHERE ProduktId = @ProduktId;
ELSE
RAISERROR ('Produkten med angivet ID finns inte!', 16, 1);