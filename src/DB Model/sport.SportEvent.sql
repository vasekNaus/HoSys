-- VIEW zastupující abstraktní entitu SportEvent (TPC – Table Per Concrete type).
-- Tabulka SportEvent v databázi neexistuje; konkrétní entity jsou Training a Match.
-- VIEW slouží pro dotazy přes obě tabulky najednou.
-- POZOR: sport.Training musí mít Id nastavené přes DEFAULT (NEXT VALUE FOR [sport].[SportEventSeq]),
--        nikoliv přes IDENTITY, aby byly Id jedinečné napříč oběma tabulkami.

CREATE VIEW [sport].[SportEvent]
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
FROM [sport].[Training]
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
FROM [sport].[Match]
GO
