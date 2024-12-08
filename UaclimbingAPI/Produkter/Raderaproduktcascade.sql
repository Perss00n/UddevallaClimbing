USE UddevallaKlätterDB;
GO

CREATE PROCEDURE Raderaproduktcascade
@ProduktId INT
AS
BEGIN
IF EXISTS (SELECT 1 FROM Produkter WHERE ProduktId = @ProduktId)

BEGIN
DELETE FROM Ordrar
WHERE ProduktId = @ProduktId;

DELETE FROM Produkter
WHERE ProduktId = @ProduktId;
END

ELSE

BEGIN
RAISERROR ('Produkten med angivet ID finns inte!', 16, 1);
END

END;