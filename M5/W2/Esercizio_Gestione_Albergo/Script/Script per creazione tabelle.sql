-- Script per la creazione del database per la gestione delle prenotazioni di un albergo
-- Creazione della tabella Clienti
CREATE TABLE Clienti (
    CodiceFiscale VARCHAR(16) PRIMARY KEY,
    Cognome NVARCHAR(50) NOT NULL,
    Nome NVARCHAR(50) NOT NULL,
    Città NVARCHAR(100) NOT NULL,
    Provincia CHAR(2) NOT NULL CHECK (Provincia = UPPER(Provincia) AND LEN(Provincia) = 2),
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Telefono NVARCHAR(20),
    Cellulare NVARCHAR(20)
);

-- Creazione della tabella TipologieCamere
CREATE TABLE TipologieCamere (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descrizione NVARCHAR(50) NOT NULL UNIQUE
);

-- Creazione della tabella Camere con riferimento alla tabella TipologieCamere
CREATE TABLE Camere (
    Numero INT PRIMARY KEY,
    Descrizione NVARCHAR(255) NOT NULL,
    TipologiaId INT NOT NULL,
    FOREIGN KEY (TipologiaId) REFERENCES TipologieCamere(Id)
);

-- Creazione della tabella Prenotazioni
CREATE TABLE Prenotazioni (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ClienteCodiceFiscale VARCHAR(16) NOT NULL,
    CameraNumero INT NOT NULL,
    DataPrenotazione DATETIME2(0) NOT NULL DEFAULT GETDATE(), -- Solo data e ora, senza frazioni di secondo
    NumeroProgressivo INT NOT NULL,
    Anno INT NOT NULL,
    Dal DATE NOT NULL, 
    Al DATE NOT NULL,
    CaparraConfirmatoria DECIMAL(18, 2) NOT NULL,
    Tariffa DECIMAL(18, 2) NOT NULL,
    DettagliSoggiorno NVARCHAR(100) NOT NULL, -- Mezza pensione, pensione completa, ecc.
    FOREIGN KEY (ClienteCodiceFiscale) REFERENCES Clienti(CodiceFiscale),
    FOREIGN KEY (CameraNumero) REFERENCES Camere(Numero)
);

-- Creazione della tabella ServiziAggiuntivi
CREATE TABLE ServiziAggiuntivi (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    PrenotazioneID INT NOT NULL,
    Data DATETIME2(0) NOT NULL DEFAULT GETDATE(), -- Solo data e ora, senza frazioni di secondo
    Quantità INT NOT NULL,
    Prezzo DECIMAL(18, 2) NOT NULL,
    Descrizione NVARCHAR(255) NOT NULL,
    FOREIGN KEY (PrenotazioneID) REFERENCES Prenotazioni(ID)
);
