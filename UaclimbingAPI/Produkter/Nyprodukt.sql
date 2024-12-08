USE UddevallaKlätterDB;
GO

CREATE PROCEDURE Nyprodukt 
@ProduktNamn NVARCHAR(100),
@Pris DECIMAL(10, 2),
@ProduktId INT OUTPUT
AS
INSERT INTO Produkter (Produktnamn, Pris)
VALUES (@ProduktNamn, @Pris);
SET @ProduktId = SCOPE_IDENTITY();