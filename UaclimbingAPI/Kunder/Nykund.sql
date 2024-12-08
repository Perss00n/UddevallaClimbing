/*
Skapar en lagrad procedur som används för att lägga till en ny kund i databasen.
Den tar emot kundens förnamn, efternamn och e-postadress som parametrar och lägger till dom i tabellen Kunder.
Proceduren returnerar också det nyss skapade KundId som en output-parameter som jag kommer fånga upp i mitt program sen.
*/

USE UddevallaKlätterDB;
GO

CREATE PROCEDURE Nykund 
@FörNamn NVARCHAR(50),
@EfterNamn NVARCHAR(50),
@Epost NVARCHAR(80),
@KundId INT OUTPUT
AS
INSERT INTO Kunder (Förnamn, Efternamn, Epost)
VALUES (@FörNamn, @EfterNamn, @Epost);
SET @KundId = SCOPE_IDENTITY();