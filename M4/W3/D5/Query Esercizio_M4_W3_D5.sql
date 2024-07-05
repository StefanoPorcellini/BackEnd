-- Creazione della tabella Anagrafiche

CREATE TABLE Anagrafiche (
	         IDAnagrafica INT PRIMARY KEY IDENTITY NOT NULL,
	         Cognome NVARCHAR(50) NOT NULL,
	         Nome NVARCHAR(50) NOT NULL,
	         Indirizzo NVARCHAR(100) NOT NULL,
             Città NVARCHAR(50) NOT NULL,
             CAP NCHAR(5) NOT NULL,
             CF NCHAR(16) NOT NULL
);

-- Creazione della tabella Violazioni

CREATE TABLE Violazioni (
             IDViolazione INT PRIMARY KEY IDENTITY NOT NULL,
             Descrizione NVARCHAR(100) NOT NULL,
             Importo DECIMAL(10, 2) NOT NULL,
             DecurtamentoPunti INT NOT NULL
);

-- Creazione della tabella Verbali

CREATE TABLE Verbali (
             IDVerbale INT PRIMARY KEY IDENTITY NOT NULL,
             DataViolazione DATETIME2 NOT NULL,
             IndirizzoViolazione NVARCHAR(100) NOT NULL,
             NominativoAgente NVARCHAR(50) NOT NULL,
             DataTrascrizioneVerbale DATETIME2 NOT NULL,
             Anagrafica_FK INT FOREIGN KEY REFERENCES Anagrafiche(IDAnagrafica) NOT NULL,
             -- ho messo "NOT NULL" perchè un verbale avrà sicuramente l'anagrafica e la violazione
             Violazione_FK INT FOREIGN KEY REFERENCES Violazioni(IDViolazione) NOT NULL
);

-- Popolamento della tabella Anagrafiche

INSERT INTO Anagrafiche 
            (Cognome, 
            Nome, 
            Indirizzo,
            Città, 
            CAP, 
            CF)
VALUES
            ('Rossi', 'Mario', 'Via Roma 1', 'Palermo', '90100', 'RSSMRI01A01H501W'),
            ('Verdi', 'Luigi', 'Viale dei Giardini 5', 'Palermo', '90100', 'VRDLGI02B02H502X'),
            ('Bianchi', 'Anna', 'Corso Italia 10', 'Catania', '95100', 'BNCHAN03C03H503Y'),
            ('Ferrari', 'Giuseppe', 'Via Garibaldi 25', 'Messina', '98100', 'FRRGPP04D04H504Z');

-- Popolamento della tabella Violazioni

INSERT INTO Violazioni 
            (Descrizione, 
            Importo, 
            DecurtamentoPunti)
VALUES
            ('Eccesso di velocità', 100.00, 2),
            ('Guida senza cintura di sicurezza', 150.00, 3),
            ('Superamento semaforo rosso', 200.00, 5),
            ('Guida in stato di ebbrezza', 500.00, 10),
            ('Sosta vietata', 50.00, 2),
            ('Superamento del limite di velocità', 150.00, 3),
            ('Manutenzione irregolare del veicolo', 100.00, 5),
            ('Utilizzo del telefono mentre si guida', 80.00, 2),
            ('Manovra pericolosa', 120.00, 4);

-- Popolamento della tabella Verbali

INSERT INTO Verbali
            (DataViolazione,
            IndirizzoViolazione,
            NominativoAgente,
            DataTrascrizioneVerbale,
            Anagrafica_FK,
            Violazione_FK)
VALUES
            ('12/01/2009', 'Via Roma 10', 'Agente Rossi', '16/01/2009', 1, 1),
            ('20/02/2009', 'Corso Italia 20', 'Agente Verdi', '21/02/2009', 2, 2),
            ('10/03/2009', 'Via Garibaldi 30', 'Agente Bianchi', '11/03/2009', 3, 3),
            ('05/04/2009', 'Viale dei Giardini 15', 'Agente Ferrari', '06/04/2009', 4, 4),
            ('15/04/2009', 'Via Roma 123', 'Mario Rossi', '16/04/2009', 1, 4),
            ('16/05/2009', 'Via Napoli 456', 'Luigi Bianchi', '17/05/2009', 2, 2),
            ('22/06/2009', 'Corso Umberto 789', 'Giovanni Verdi', '24/06/2009', 3, 6),
            ('18/08/2009', 'Via Milano 321', 'Anna Neri', '19/08/2009', 4, 1),
            ('15/11/2009', 'Piazza Dante 567', 'Sara Gialli', '18/11/2009', 3, 5),
            ('20/12/2009', 'Corso Vittorio Emanuele 890', 'Paolo Blu', '22/12/2009', 1, 3);
 
 -- QUERY

 -- 1) Conteggio dei verbali trascritti

 SELECT     COUNT(*)
 AS         VerbaliTotali
 FROM       Verbali;

 -- 2) Conteggio dei verbali trascritti raggruppati per anagrafe

 SELECT     a.Cognome, a.Nome, COUNT(v.IDVerbale)
 AS         VerbaliTotaliPerAnagrafe
 FROM       Anagrafiche a
 LEFT JOIN  Verbali v
 ON         a.IDAnagrafica = v.Anagrafica_FK
 GROUP BY   a.Cognome, a.Nome;

 -- 3) Conteggio dei verbali trascritti raggruppati per tipo di violazione

 SELECT     vi.Descrizione, COUNT(v.IDVerbale) 
 AS         VerbaliTotaliPerViolazioni
 FROM       Violazioni vi
 LEFT JOIN  Verbali v
 ON         vi.IDViolazione = v.Violazione_FK
 GROUP BY   vi.Descrizione;

 -- 4) Totale dei punti decurtati per ogni anagrafe

 SELECT     a.Cognome, a.Nome, SUM(vi.DecurtamentoPunti) 
 AS         TotalePuntiDecurtati
 FROM       Anagrafiche a
 LEFT JOIN  Verbali v
 ON         a.IDAnagrafica = v.Anagrafica_FK
 LEFT JOIN  Violazioni vi 
 ON         v.Violazione_FK = vi.IDViolazione
 GROUP BY   a.Cognome, a.Nome;

 -- 5) Cognome, Nome, Data violazione, Indirizzo violazione, importo e punti decurtati per tutti gli anagrafici residenti a Palermo

 SELECT     a.Cognome,
            a.Nome,
            v.DataViolazione,
            v.IndirizzoViolazione,
            vi.Importo,
            vi.DecurtamentoPunti
 FROM       Anagrafiche a
 INNER JOIN Verbali v 
 ON         a.IDAnagrafica = v.Anagrafica_FK
 INNER JOIN Violazioni vi 
 ON         v.Violazione_FK = vi.IDViolazione
 WHERE      a.Città = 'Palermo'
 ORDER BY   a.Cognome, a.Nome;

 -- 6) Cognome, Nome, Indirizzo, Data violazione, importo e punti decurtati per le violazioni fatte tra il febbraio 2009 e luglio 2009

 SELECT     a.Cognome,
            a.Nome,
            v.DataViolazione,
            v.IndirizzoViolazione,
            vi.Importo,
            vi.DecurtamentoPunti
 FROM       Anagrafiche a
 INNER JOIN Verbali v 
 ON         a.IDAnagrafica = v.Anagrafica_FK
 INNER JOIN Violazioni vi 
 ON         v.Violazione_FK = vi.IDViolazione
 WHERE      v.DataViolazione BETWEEN '01/02/2009' AND '31/07/2009'
 ORDER BY   v.DataViolazione;

 -- 7) Totale degli importi per ogni anagrafico

 SELECT     a.Cognome, a.Nome, SUM(vi.Importo)
 AS         TotaleImporto
 FROM       Anagrafiche a
 INNER JOIN Verbali v 
 ON         a.IDAnagrafica = v.Anagrafica_FK
 INNER JOIN Violazioni vi 
 ON         v.Violazione_FK = vi.IDViolazione
 GROUP BY   a.Cognome, a.Nome;

 -- 8) Visualizzazione di tutti gli anagrafici residenti a Palermo

 SELECT     Cognome,
            Nome,
            Indirizzo,
            Città,
            CAP,
            CF
 FROM       Anagrafiche
 WHERE      Città = 'Palermo';

 -- 9) Query che visualizzi Data violazione, Importo e decurtamento punti relativi ad una certa data

 SELECT     v.DataViolazione, vi.Importo, vi.DecurtamentoPunti
 FROM       Verbali v
 INNER JOIN Violazioni vi
 ON         v.Violazione_FK = vi.IDViolazione
 WHERE      v.DataViolazione = '10/03/2009'

  -- 10) Conteggio delle violazioni contestate raggruppate per Nominativo dell’agente di Polizia

  SELECT    NominativoAgente, COUNT(*)
  AS        ConteggioViolazioniContestate
  FROM      Verbali
  GROUP BY  NominativoAgente;

  -- 11) Cognome, Nome, Indirizzo, Data violazione, Importo e punti decurtati
  --     per tutte le violazioni che superino il decurtamento di 5 punti

 SELECT     a.Cognome, 
            a.Nome, 
            a.Indirizzo, 
            v.DataViolazione,
            vi.Importo,
            vi.DecurtamentoPunti
 FROM       Anagrafiche a
 INNER JOIN Verbali v 
 ON         a.IDAnagrafica = v.Anagrafica_FK
 INNER JOIN Violazioni vi 
 ON         v.Violazione_FK = vi.IDViolazione
 WHERE      vi.DecurtamentoPunti > 5
 ORDER BY   a.Cognome, a.Nome;

 -- 12)     Cognome, Nome, Indirizzo, Data violazione, Importo e punti decurtati
 --         per tutte le violazioni che superino l’importo di 400 euro
  
 SELECT     a.Cognome, 
            a.Nome, 
            a.Indirizzo, 
            v.DataViolazione,
            vi.Importo,
            vi.DecurtamentoPunti
 FROM       Anagrafiche a
 INNER JOIN Verbali v 
 ON         a.IDAnagrafica = v.Anagrafica_FK
 INNER JOIN Violazioni vi 
 ON         v.Violazione_FK = vi.IDViolazione
 WHERE      vi.Importo > 400
 ORDER BY   a.Cognome, a.Nome;