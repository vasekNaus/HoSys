-- Úprava tabulky sport.Match pro potřeby importu:
-- 1. Odebrání sloupce MatchState_Id (a jeho FK)
-- 2. Změna MatchCode na NULL

-- 1a. Odebrat FK constraint MatchState
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Match_MatchState')
    ALTER TABLE [sport].[Match] DROP CONSTRAINT [FK_Match_MatchState];
GO

-- 1b. Odebrat sloupec MatchState_Id
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('sport.Match') AND name = 'MatchState_Id')
    ALTER TABLE [sport].[Match] DROP COLUMN [MatchState_Id];
GO

-- 2. Změnit MatchCode na nullable
ALTER TABLE [sport].[Match] ALTER COLUMN [MatchCode] [varchar](10) NULL;
GO
