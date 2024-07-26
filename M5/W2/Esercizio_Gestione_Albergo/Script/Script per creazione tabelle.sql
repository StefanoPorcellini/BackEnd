CREATE TABLE [dbo].[Camere] (
    [Numero]       INT             NOT NULL,
    [Descrizione]  NVARCHAR (255)  NOT NULL,
    [TipologiaId]  INT             NOT NULL,
    [Disponibile]  BIT             DEFAULT ((1)) NOT NULL,
    [Coefficiente] DECIMAL (18, 1) DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([Numero] ASC),
    FOREIGN KEY ([TipologiaId]) REFERENCES [dbo].[TipologieCamere] ([Id])
);

CREATE TABLE [dbo].[Clienti] (
    [CodiceFiscale] VARCHAR (16)   NOT NULL,
    [Cognome]       NVARCHAR (50)  NOT NULL,
    [Nome]          NVARCHAR (50)  NOT NULL,
    [Città]         NVARCHAR (100) NOT NULL,
    [Provincia]     CHAR (2)       NOT NULL,
    [Email]         NVARCHAR (100) NOT NULL,
    [Telefono]      NVARCHAR (20)  NULL,
    [Cellulare]     NVARCHAR (20)  NULL,
    PRIMARY KEY CLUSTERED ([CodiceFiscale] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC),
    CHECK ([Provincia]=upper([Provincia]) AND len([Provincia])=(2))
);

CREATE TABLE [dbo].[DettagliSoggiorno] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Descrizione] NVARCHAR (100)  NOT NULL,
    [Prezzo]      DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Descrizione] ASC)
);

CREATE TABLE [dbo].[Prenotazioni] (
    [ID]                   INT             IDENTITY (1, 1) NOT NULL,
    [ClienteCodiceFiscale] VARCHAR (16)    NOT NULL,
    [CameraNumero]         INT             NOT NULL,
    [DataPrenotazione]     DATETIME2 (0)   NOT NULL,
    [NumeroProgressivo]    VARCHAR (15)    NOT NULL,
    [Anno]                 INT             NOT NULL,
    [Dal]                  DATETIME2 (0)   NOT NULL,
    [Al]                   DATETIME2 (0)   NOT NULL,
    [CaparraConfirmatoria] DECIMAL (18, 2) NOT NULL,
    [Tariffa]              DECIMAL (18, 2) NOT NULL,
    [DettagliSoggiornoId]  INT             NOT NULL,
    [SaldoFinale]          DECIMAL (18, 2) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CameraNumero]) REFERENCES [dbo].[Camere] ([Numero]),
    FOREIGN KEY ([ClienteCodiceFiscale]) REFERENCES [dbo].[Clienti] ([CodiceFiscale]),
    FOREIGN KEY ([DettagliSoggiornoId]) REFERENCES [dbo].[DettagliSoggiorno] ([Id])
);

CREATE TABLE [dbo].[PrenotazioniServiziAggiuntivi] (
    [PrenotazioneID]       INT           NOT NULL,
    [ServizioAggiuntivoID] INT           NOT NULL,
    [Data]                 DATETIME2 (0) DEFAULT (getdate()) NOT NULL,
    [Quantità]             INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([PrenotazioneID] ASC, [ServizioAggiuntivoID] ASC),
    FOREIGN KEY ([PrenotazioneID]) REFERENCES [dbo].[Prenotazioni] ([ID]),
    FOREIGN KEY ([ServizioAggiuntivoID]) REFERENCES [dbo].[ServiziAggiuntivi] ([ID])
);

CREATE TABLE [dbo].[ServiziAggiuntivi] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [Descrizione] NVARCHAR (255)  NOT NULL,
    [prezzo]      DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    UNIQUE NONCLUSTERED ([Descrizione] ASC)
);

CREATE TABLE [dbo].[TipologieCamere] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Descrizione] NVARCHAR (50)   NOT NULL,
    [Prezzo]      DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Descrizione] ASC)
);


CREATE TABLE [dbo].[Utenti] (
    [UserId]   INT            IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (30)  NOT NULL,
    [Password] NVARCHAR (128) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC)
);

