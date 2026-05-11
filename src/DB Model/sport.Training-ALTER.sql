-- Nahrazení IDENTITY na sloupci Id za DEFAULT ze sekvence SportEventSeq.
-- SQL Server neumožňuje odebrat IDENTITY přímo přes ALTER COLUMN,
-- proto je nutné provést přejmenování sloupce.

-- 1. Přidat nový sloupec bez IDENTITY (NULL, aby nebyly automaticky generovány hodnoty ze sekvence)
ALTER TABLE [sport].[Training] ADD [Id_new] [int] NULL;
GO

-- 2. Zkopírovat stávající hodnoty Id
UPDATE [sport].[Training] SET [Id_new] = [Id];
GO

-- 3. Nastavit NOT NULL
ALTER TABLE [sport].[Training] ALTER COLUMN [Id_new] [int] NOT NULL;
GO

-- 4. Odebrat primární klíč
ALTER TABLE [sport].[Training] DROP CONSTRAINT [PK_Training];
GO

-- 5. Odebrat původní sloupec s IDENTITY
ALTER TABLE [sport].[Training] DROP COLUMN [Id];
GO

-- 6. Přejmenovat nový sloupec na Id
EXEC sp_rename 'sport.Training.Id_new', 'Id', 'COLUMN';
GO

-- 7. Přidat DEFAULT ze sekvence
ALTER TABLE [sport].[Training] ADD CONSTRAINT [DF_Training_Id] DEFAULT (NEXT VALUE FOR [sport].[SportEventSeq]) FOR [Id];
GO

-- 8. Obnovit primární klíč
ALTER TABLE [sport].[Training] ADD CONSTRAINT [PK_Training] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- 9. Přenastavit sekvenci za nejvyšší stávající Id, aby nedocházelo ke kolizím
DECLARE @MaxId int;
SELECT @MaxId = ISNULL(MAX([Id]), 0) FROM [sport].[Training];
DECLARE @Sql nvarchar(200) = N'ALTER SEQUENCE [sport].[SportEventSeq] RESTART WITH ' + CAST(@MaxId + 1 AS nvarchar(20));
EXEC sp_executesql @Sql;
GO
