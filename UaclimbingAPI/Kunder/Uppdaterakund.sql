/*
Den här proceduren används för att uppdatera informationen om en befintlig kund
Proceduren tar emot kundens KundId, förnamn, efternamn och e-postadress som parametrar.
Om kunden med det angivna KundId finns i databasen så uppdateras deras information, annars genereras ett
sql felmeddelande som jag fångar upp i mitt program.
*/

USE UddevallaKlätterDB;
GO

CREATE PROCEDURE UppdateraKund
@KundId INT,
@FörNamn NVARCHAR(50),
@EfterNamn NVARCHAR(50),
@Epost NVARCHAR(80)
AS
IF EXISTS (SELECT 1 FROM Kunder WHERE KundId = @KundId)
UPDATE Kunder
SET Förnamn = ISNULL(@FörNamn, Förnamn),
    Efternamn = ISNULL(@EfterNamn, Efternamn),
    Epost = ISNULL(@Epost, Epost)
WHERE KundId = @KundId;
ELSE
RAISERROR ('Kunden med angivet ID finns inte!', 16, 1);