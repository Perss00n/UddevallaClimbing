USE UddevallaKlätterDB;
GO

CREATE PROCEDURE Uppdateraprodukt
@ProduktId INT,
@ProduktNamn NVARCHAR(100),
@Pris DECIMAL(10, 2)
AS
IF EXISTS (SELECT 1 FROM Produkter WHERE ProduktId = @ProduktId)
UPDATE Produkter
SET Produktnamn = ISNULL(@ProduktNamn, Produktnamn),
    Pris = ISNULL(@Pris, Pris)
WHERE ProduktId = @ProduktId;
ELSE
RAISERROR ('Produkten med angivet ID finns inte!', 16, 1);