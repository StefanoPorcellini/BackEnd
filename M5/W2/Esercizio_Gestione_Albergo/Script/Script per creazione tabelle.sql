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

-- Creazione della tabella TipologieCamere (singola, doppia)
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

-- Creazione della tabella DettagliSoggiorno (mezza pensione, pensione completa, pernottamento con prima colazione)
CREATE TABLE DettagliSoggiorno (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descrizione NVARCHAR(100) NOT NULL UNIQUE 
);

-- Creazione della tabella Prenotazioni con riferimento alla tabella DettagliSoggiorno
CREATE TABLE Prenotazioni (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ClienteCodiceFiscale VARCHAR(16) NOT NULL,
    CameraNumero INT NOT NULL,
    DataPrenotazione DATETIME2(0) NOT NULL DEFAULT GETDATE(),
    NumeroProgressivo INT NOT NULL,
    Anno INT NOT NULL,
    Dal DATE NOT NULL, 
    Al DATE NOT NULL,
    CaparraConfirmatoria DECIMAL(18, 2) NOT NULL,
    Tariffa DECIMAL(18, 2) NOT NULL,
    DettagliSoggiornoId INT NOT NULL, -- Riferimento ai dettagli del soggiorno
    FOREIGN KEY (ClienteCodiceFiscale) REFERENCES Clienti(CodiceFiscale),
    FOREIGN KEY (CameraNumero) REFERENCES Camere(Numero),
    FOREIGN KEY (DettagliSoggiornoId) REFERENCES DettagliSoggiorno(Id)
);

-- Creazione della tabella ServiziAggiuntivi (Colazione in camera, bevande e cibo nel mini bar, internet, letto aggiuntivo, culla))
CREATE TABLE ServiziAggiuntivi (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Descrizione NVARCHAR(255) NOT NULL UNIQUE 
);

-- Creazione della tabella associativa tra Prenotazioni e ServiziAggiuntivi
CREATE TABLE PrenotazioniServiziAggiuntivi (
    PrenotazioneID INT NOT NULL,
    ServizioAggiuntivoID INT NOT NULL,
    Data DATETIME2(0) NOT NULL DEFAULT GETDATE(), 
    Quantità INT NOT NULL,
    Prezzo DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (PrenotazioneID, ServizioAggiuntivoID, Data),
    FOREIGN KEY (PrenotazioneID) REFERENCES Prenotazioni(ID),
    FOREIGN KEY (ServizioAggiuntivoID) REFERENCES ServiziAggiuntivi(ID)
);
