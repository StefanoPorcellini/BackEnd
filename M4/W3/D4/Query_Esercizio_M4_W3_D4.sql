-- 1) Creazione tabelle

CREATE TABLE [dbo].[Impiegato] (
    [IDImpiegato]       INT           IDENTITY (1, 1) NOT NULL,
    [Cognome]           NVARCHAR (50) NOT NULL,
    [Nome]              NVARCHAR (50) NOT NULL,
    [CodiceFiscale]     CHAR (16)     NOT NULL,
    [Eta]               INT           NOT NULL,
    [RedditoMensile]    MONEY         DEFAULT ((800)) NULL,
    [DetrazioneFiscale] BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([IDImpiegato] ASC)
);

CREATE TABLE [dbo].[Impiego] (
    [IDImpiego]   INT           IDENTITY (1, 1) NOT NULL,
    [TipoImpiego] NVARCHAR (50)     NOT NULL,
    [Assunzione]  DATETIME2 (7) NOT NULL,
    [IDImpiegato] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([IDImpiego] ASC),
    FOREIGN KEY ([IDImpiegato]) REFERENCES [dbo].[IMPIEGATO] ([IDImpiegato])
);

-- 2) Popolare le tabelle con almeno 20 impiegati e i relativi impieghi

INSERT INTO Impiegato (Cognome, Nome, CodiceFiscale, Eta, RedditoMensile, DetrazioneFiscale) VALUES
('Rossi', 'Mario', 'RSSMRA80A01H501Z', 35, 1500.00, 1),
('Bianchi', 'Luigi', 'BNCLGU85B01H501Z', 30, 1200.00, 0),
('Verdi', 'Anna', 'VRDANN90C01H501Z', 28, 950.00, 1),
('Gialli', 'Paolo', 'GLLPLO75D01H501Z', 45, 2000.00, 0),
('Neri', 'Chiara', 'NRICHR78F01H501Z', 42, 1800.00, 1),
('Rossi', 'Alessandro', 'RSSALS85M01H501Z', 36, 1550.00, 0),
('Bianchi', 'Marta', 'BNCMRT80A01H501Z', 32, 1100.00, 1),
('Verdi', 'Giuseppe', 'VRDGPP92H01H501Z', 29, 850.00, 0),
('Gialli', 'Laura', 'GLLLRA85D01H501Z', 33, 1700.00, 1),
('Neri', 'Francesco', 'NRFNSC90A01H501Z', 26, 900.00, 0),
('Rossi', 'Sofia', 'RSSSFO85A01H501Z', 34, 1600.00, 1),
('Bianchi', 'Giorgio', 'BNCGRG80A01H501Z', 31, 1300.00, 0),
('Verdi', 'Elena', 'VRDENE88A01H501Z', 29, 950.00, 1),
('Gialli', 'Marco', 'GLLMRC85D01H501Z', 37, 1450.00, 0),
('Neri', 'Valeria', 'NRVLEI85A01H501Z', 39, 1650.00, 1),
('Rossi', 'Gabriele', 'RSSGBR85A01H501Z', 27, 1200.00, 0),
('Bianchi', 'Stefano', 'BNCSTF80A01H501Z', 44, 2000.00, 1),
('Verdi', 'Cristina', 'VRDCRN92H01H501Z', 24, 800.00, 0),
('Gialli', 'Simone', 'GLLSMN85D01H501Z', 35, 1500.00, 1),
('Neri', 'Federica', 'NRFDRC90A01H501Z', 28, 1400.00, 0);

INSERT INTO Impiego (IDImpiegato, TipoImpiego, Assunzione)
VALUES
(1, 'Amministrativo', '01/06/2005'),
(2, 'Tecnico', '15/03/2007'),
(3, 'HR', '23/08/2008'),
(4, 'Commerciale', '30/11/2004'),
(5, 'Manager', '19/05/2010'),
(6, 'Amministrativo', '01/06/2005'),
(7, 'Tecnico', '15/03/2007'),
(8, 'HR', '23/08/2009'),
(9, 'Commerciale', '30/11/2004'),
(10, 'Manager', '19/05/2010'),
(11, 'Amministrativo', '01/06/2005'),
(12, 'Tecnico', '15/03/2007'),
(13, 'HR', '23/08/2009'),
(14, 'Commerciale', '30/11/2004'),
(15, 'Manager', '19/05/2010'),
(16, 'Amministrativo', '01/06/2005'),
(17, 'Tecnico', '15/03/2007'),
(18, 'HR', '23/08/2009'),
(19, 'Commerciale', '30/11/2004'),
(20, 'Manager', '19/05/2010');

-- 3) Visualizzare tutti gli impiegati oltre i 29 anni

SELECT  IDImpiegato,
        Cognome, 
        Nome, 
        CodiceFiscale, 
        Eta, 
        RedditoMensile, 
        DetrazioneFiscale 
FROM    Impiegato 
WHERE   Eta < 30;

-- 4) Visualizzare tutti gli impiegati con un reddito di almeno 800 euro mensili

SELECT  IDImpiegato,
        Cognome, 
        Nome, 
        CodiceFiscale, 
        Eta, 
        RedditoMensile, 
        DetrazioneFiscale 
FROM    Impiegato 
WHERE   RedditoMensile >= 800.00;

-- 5) Visualizzare tutti gli impiegati che posseggono la detrazione fiscale

SELECT  IDImpiegato,
        Cognome, 
        Nome, 
        CodiceFiscale, 
        Eta, 
        RedditoMensile, 
        DetrazioneFiscale 
FROM    Impiegato 
WHERE   DetrazioneFiscale = 1;

-- 6) Visualizzare tutti gli impiegati che non posseggono la detrazione fiscale

SELECT  IDImpiegato,
        Cognome, 
        Nome, 
        CodiceFiscale, 
        Eta, 
        RedditoMensile, 
        DetrazioneFiscale 
FROM    Impiegato 
WHERE   DetrazioneFiscale = 0;

-- 7) Visualizzare tutti gli impiegati cui il cognome cominci con una lettera G e li visualizzi in ordine alfabetico

SELECT  IDImpiegato,
        Cognome, 
        Nome, 
        CodiceFiscale, 
        Eta, 
        RedditoMensile, 
        DetrazioneFiscale 
FROM    Impiegato 
WHERE   Cognome LIKE 'G%'
ORDER BY Cognome;

-- 8) Visualizzare il numero totale degli impiegati registrati nella base dati

SELECT COUNT(*) AS TotaleImpiegati FROM Impiegato;

-- 9) Visualizzare il totale dei redditi mensili di tutti gli impiegati

SELECT SUM(RedditoMensile) AS TotaleRedditiImpiegati FROM Impiegato;

-- 10) Visualizzare la media dei redditi mensili di tutti gli impiegati

SELECT AVG(RedditoMensile) AS MediaRedditiImpiegati FROM Impiegato;

-- 11) Visualizzare l’importo del reddito mensile maggiore

SELECT MAX(RedditoMensile) AS RedditoMax FROM Impiegato;

-- 12) Visualizzare l’importo del reddito mensile minore

SELECT MIN(RedditoMensile) AS RedditoMin FROM Impiegato;

-- 13) Visualizzare gli impiegati assunti dall’ 01/01/2007 all’ 01/01/2008

SELECT  Impiegato.IDImpiegato,
        Impiegato.Cognome, 
        Impiegato.Nome, 
        Impiegato.CodiceFiscale, 
        Impiegato.Eta, 
        Impiegato.RedditoMensile, 
        Impiegato.DetrazioneFiscale
FROM    Impiegato
INNER JOIN Impiego
ON      Impiegato.IDImpiegato = Impiego.IDImpiegato
WHERE   Assunzione BETWEEN '01/01/2007' AND '01/01/2008';

-- 14)  Tramite una query parametrica che identifichi il tipo di impiego,
--      visualizzare tutti gli impiegati che corrispondono a quel tipo di impiego

DECLARE @TipoImpiego  NVARCHAR (50) = 'Commerciale'

SELECT  Impiegato.IDImpiegato,
        Impiegato.Cognome, 
        Impiegato.Nome, 
        Impiegato.CodiceFiscale, 
        Impiegato.Eta, 
        Impiegato.RedditoMensile, 
        Impiegato.DetrazioneFiscale
FROM    Impiegato
INNER JOIN Impiego
ON      Impiegato.IDImpiegato = Impiego.IDImpiegato
WHERE   Impiego.TipoImpiego = @TipoImpiego;

-- 15)  Visualizzare l’età media dei lavoratori all’interno dell’azienda

SELECT  AVG(Eta) AS EtaMedia FROM Impiegato;