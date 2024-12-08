/*
Min tanke med det här projektet är att skapa en klätterutrustningswebbshop som med hjälp utav
ett API ska kunna hantera kundregister, produktutbud och orderhistorik.
Den inkluderar tre tabeller: Kunder, Produkter och Ordrar samt fyller dessa tabeller med exempeldata.

Min grundtanke är att:

Kunder: Innehåller kundinformation där varje kund har ett unikt ID och en unik e-postadress.
Produkter: Representerar produkter till försäljning med namn, pris samt ett unikt ID.
Ordrar: Kopplar kunder till produkter med hjälp av KundId och ProduktId samt lagrar antalet köpta produkter och beställningsdatumet.

Istället för att ange ON DELETE CASCADE på dom främmande nycklarna i tabellen Ordrar så har jag skapat separata lagrade procedurer
och metoder som ska kunna hantera detta snyggt i mitt API.

Alla sql filer finns sparade i respektive mapp och behöver köras innan APIet kan användas.

Jag har bara lagt till kommentarer i mina SQL-filer som ligger i mappen Kunder. Där förklarar jag mina tankar kring hur jag har
byggt procedurerna och vad dom gör. Procedurerna för Ordrar och Produkter följer typ samma struktur och logik, enda skillnaden att
de är anpassade för respektive tabeller och fält.

Mycket Nöje! =)
*/

CREATE DATABASE UddevallaKlätterDB;
GO

USE UddevallaKlätterDB;
GO

CREATE TABLE Kunder (
    KundId INT IDENTITY(1,1) PRIMARY KEY,
    Förnamn NVARCHAR(50) NOT NULL,
    Efternamn NVARCHAR(50) NOT NULL,
    Epost NVARCHAR(80) UNIQUE NOT NULL
);

CREATE TABLE Produkter (
    ProduktId INT IDENTITY(1,1) PRIMARY KEY,
    Produktnamn NVARCHAR(100) NOT NULL, 
    Pris DECIMAL(10, 2) NOT NULL
);

CREATE TABLE Ordrar (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    KundId INT NOT NULL REFERENCES Kunder(KundId),
    ProduktId INT NOT NULL REFERENCES Produkter(ProduktId),
    Antal INT NOT NULL,
    OrderDatum DATETIME NOT NULL DEFAULT GETDATE()
);

INSERT INTO Kunder
VALUES ('Daniel', 'Svensson', 'Dannebanan@fakemail.com'),
       ('Eva-Lena', 'Abrahamsson', 'kotten77@fakemail.com'),
       ('Urban', 'Dickmen', 'pickendicken@fakemail.com'),
       ('Sarah', 'Niklasson', 'sniklasson@fakemail.com'),
       ('Tore', 'Trygg', 'storastyggatore@fakemail.com');

INSERT INTO Produkter
VALUES ('Klätterskor - Beginner Plus', 899.99),
       ('Rep - Dynamic 9.8mm 60m', 1799.00),
       ('Klättersele - Allround Pro', 749.50),
       ('Karbinhake - Screw Lock', 129.99),
       ('Quickdraws - Set om 6', 649.00),
       ('Krita - 250g', 99.00),
       ('Kritpåse - Ergonomisk Design', 249.00),
       ('Hjälm - Lightweight Climb', 499.99),
       ('Repbroms - Auto-Locking', 999.00),
       ('Klätterhandskar - Full Finger', 349.50);

INSERT INTO Ordrar (KundId, ProduktId, Antal)
VALUES (1, 1, 2), -- Daniel Svensson köper 2 st Klätterskor
       (2, 4, 5), -- Eva-Lena Abrahamsson köper 5 st Karbinhakar
       (3, 7, 1), -- Urban Dickmen köper 1 st Kritpåse
       (4, 9, 3), -- Sarah Niklasson köper 3 st Repbromsar
       (5, 3, 1); -- Tore Trygg köper 1 st Klättersele