-- Přidání sloupce MatchType_Id do tabulky sport.Match.
-- Předpoklad: tabulka sport.MatchType musí existovat a obsahovat data.
-- Sloupec je přidán jako NULL, naplněn výchozí hodnotou a poté změněn na NOT NULL.

-- 1. Přidat sloupec jako NULL (tabulka může již obsahovat záznamy)
ALTER TABLE [sport].[Match] ADD [MatchType_Id] [int] NULL;
GO

-- 2. Naplnit stávající záznamy výchozí hodnotou (1 = Ligový zápas)
UPDATE [sport].[Match] SET [MatchType_Id] = 1;
GO

-- 3. Změnit na NOT NULL
ALTER TABLE [sport].[Match] ALTER COLUMN [MatchType_Id] [int] NOT NULL;
GO

-- 4. Přidat FK constraint
ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_MatchType] FOREIGN KEY([MatchType_Id])
REFERENCES [sport].[MatchType] ([Id])
GO

ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_MatchType]
GO
