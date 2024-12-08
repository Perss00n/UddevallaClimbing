/*
Skapar en lagrad procedur som används för att radera en kund ifrån databasen baserat på KundId.
Proceduren kontrollerar först om kunden med det angivna KundIdt finns i databasen och om kunden finns
tas kunden bort. Om kunden inte finns genereras ett felmeddelande som jag fångar upp i mitt program.
*/

USE UddevallaKlätterDB;
GO

CREATE PROCEDURE Raderakund
@KundId INT
AS
IF EXISTS (SELECT 1 FROM Kunder WHERE KundId = @KundId)
DELETE FROM Kunder
WHERE KundId = @KundId;
ELSE
RAISERROR ('Kunden med angivet ID finns inte!', 16, 1);