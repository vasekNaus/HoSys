-- VIEW zastupující abstraktní entitu SportEvent (TPC – Table Per Concrete type).
-- Tabulka SportEvent v databázi neexistuje; konkrétní entity jsou Training a Match.
-- VIEW slouží pro dotazy přes obě tabulky najednou.
-- POZOR: dbo.Training musí mít Id nastavené přes DEFAULT (NEXT VALUE FOR [dbo].[SportEventSeq]),
--        nikoliv přes IDENTITY, aby byly Id jedinečné napříč oběma tabulkami.

CREATE VIEW [dbo].[SportEvent]
AS
SELECT
	[Id],
	[Season_Id],
	[SeasonCategory_Name],
	[IceRink_Id],
	[Date],
	[TimeFrom],
	[TimeTo],
	[DurationMinutes],
	[Note],
	'Training' AS [EventType]
FROM [dbo].[Training]
UNION ALL
SELECT
	[Id],
	[Season_Id],
	[SeasonCategory_Name],
	[IceRink_Id],
	[Date],
	[TimeFrom],
	[TimeTo],
	[DurationMinutes],
	[Note],
	'Match' AS [EventType]
FROM [dbo].[Match]
GO
