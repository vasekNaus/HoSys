-- ============================================================
-- Příprava schématu Target před migrací dat
-- Spustit PŘED 02_migrate_data.sql
--
-- Důvod: sport.Training v Target obsahuje sloupec IceRink_Id NOT NULL
-- (nový sloupec bez ekvivalentu v Source). Tabulka sport.IceRink se
-- nemigruje, proto se sloupec odebere.
-- ============================================================

USE [SportSys];
GO

BEGIN TRANSACTION;

ALTER TABLE [sport].[Training] DROP CONSTRAINT [FK_Training_IceRink_IceRink_Id];
ALTER TABLE [sport].[Training] DROP COLUMN [IceRink_Id];

PRINT 'Schema prep OK – IceRink_Id odebran z sport.Training';

-- Změnit ROLLBACK na COMMIT po ověření
ROLLBACK;
-- COMMIT;
