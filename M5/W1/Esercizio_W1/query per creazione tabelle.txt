-- creazione tabella clienti

CREATE TABLE [dbo].[Clienti] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Nome]          NVARCHAR (100) NOT NULL,
    [Tipo]          NVARCHAR (7)   NOT NULL,
    [CodiceFiscale] NVARCHAR (16)  NULL,
    [PartitaIva]    NVARCHAR (11)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CHK_Tipo] CHECK ([Tipo]='Azienda' OR [Tipo]='Privato')
);

-- creazione tabella spedizioni

CREATE TABLE [dbo].[Spedizioni] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,
    [Id_Cliente]             INT             NOT NULL,
    [NumeroIdentificativo]   NVARCHAR (50)   NOT NULL,
    [DataSpedizione]         DATETIME2 (7)   NOT NULL,
    [Peso]                   FLOAT (53)      NOT NULL,
    [CittaDestinataria]      NVARCHAR (100)  NOT NULL,
    [IndirizzoDestinatario]  NVARCHAR (200)  NOT NULL,
    [NominativoDestinatario] NVARCHAR (100)  NOT NULL,
    [Costo]                  DECIMAL (18, 2) NOT NULL,
    [DataConsegnaPrevista]   DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ID_Cliente]) REFERENCES [dbo].[Clienti] ([Id])
);

-- creazione tabella Aggiornamento Spedizioni

CREATE TABLE [dbo].[AggiornamentiSpedizione] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Id_Spedizione] INT            NOT NULL,
    [Stato]         NVARCHAR (14)  NOT NULL,
    [Luogo]         NVARCHAR (100) NOT NULL,
    [Descrizione]   NVARCHAR (500) NULL,
    [DataOra]       DATETIME2 (7)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Id_Spedizione]) REFERENCES [dbo].[Spedizioni] ([Id]),
	CONSTRAINT [CHK_Stato] CHECK ([Stato]='In transito' OR
							      [Stato]='In consegna' OR
								  [Stato]='Consegnato' OR
								  [Stato]='Non consegnato')
);