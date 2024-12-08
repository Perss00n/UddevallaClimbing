/*
Skapar en lagrad procedur som används för att radera en kund och deras tillhörande ordrar ifrån databasen.
Om kunden finns i databasen, raderas både kundens OCH alla ordrar som är kopplade till den kunden.
Om kunden inte finns så genereras ett felmeddelande som jag fångar upp i mitt program.

Tanken med denna proceduren är att den ska finnas som ett alternativ i mitt API där man kan välja om man vill
ta bort enbart kunden med förutsättningen att kunden INTE har några pågående ordrar, eller välja att radera kunden samt
ALLT som är kopplat till kunden.
*/

USE UddevallaKlätterDB;
GO

CREATE PROCEDURE Raderakundcascade
@KundId INT
AS
BEGIN
IF EXISTS (SELECT 1 FROM Kunder WHERE KundId = @KundId)

BEGIN
DELETE FROM Ordrar
WHERE KundId = @KundId;

DELETE FROM Kunder
WHERE KundId = @KundId;
END

ELSE

BEGIN
RAISERROR ('Kunden med angivet ID finns inte!', 16, 1);
END

END;