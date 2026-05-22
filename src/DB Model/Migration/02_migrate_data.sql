-- ============================================================
-- Migrace dat: [Hockey] → [SportSys]
-- Spustit AŽ PO 01_schema_prep.sql
--
-- Pořadí respektuje FK závislosti:
--   Coach, Season → SeasonCategory → TrainingPlan, TrainingEntitlement
--   → Training → CoachTraining
--   Coach + TrainingPlan → CoachTrainingPlan
--
-- Skript je obalený v transakci – pro ostré spuštění změň ROLLBACK → COMMIT.
-- ============================================================

USE [SportSys];
GO

-- Bezpečnostní kontrola: cílové tabulky musí být prázdné
IF EXISTS (SELECT 1 FROM [dbo].[Coach])
    OR EXISTS (SELECT 1 FROM [sport].[Season])
    OR EXISTS (SELECT 1 FROM [sport].[Training])
BEGIN
    RAISERROR('Cílové tabulky nejsou prázdné. Migrace přerušena.', 16, 1);
    RETURN;
END;

BEGIN TRANSACTION;

-- --------------------------------------------------------
-- 1. dbo.Coach
-- --------------------------------------------------------
SET IDENTITY_INSERT [dbo].[Coach] ON;

INSERT INTO [dbo].[Coach] ([Id], [FirstName], [LastName])
SELECT [Id], [FirstName], [LastName]
FROM [Hockey].[dbo].[Coach];

SET IDENTITY_INSERT [dbo].[Coach] OFF;
PRINT 'OK: dbo.Coach – ' + CAST(@@ROWCOUNT AS varchar(10)) + ' řádků';

-- --------------------------------------------------------
-- 2. sport.Season
-- --------------------------------------------------------
SET IDENTITY_INSERT [sport].[Season] ON;

INSERT INTO [sport].[Season] ([Id], [Name], [From], [To])
SELECT [Id], [Name], [From], [To]
FROM [Hockey].[sport].[Season];

SET IDENTITY_INSERT [sport].[Season] OFF;
PRINT 'OK: sport.Season – ' + CAST(@@ROWCOUNT AS varchar(10)) + ' řádků';

-- --------------------------------------------------------
-- 3. sport.SeasonCategory  (composite PK, bez IDENTITY)
-- --------------------------------------------------------
INSERT INTO [sport].[SeasonCategory] ([Season_Id], [Name], [Order], [BirthYears])
SELECT [Season_Id], [Name], [Order], [BirthYears]
FROM [Hockey].[sport].[SeasonCategory];

PRINT 'OK: sport.SeasonCategory – ' + CAST(@@ROWCOUNT AS varchar(10)) + ' řádků';

-- --------------------------------------------------------
-- 4. sport.TrainingPlan  (Season_Id → SeasonCategory_Season_Id)
-- --------------------------------------------------------
SET IDENTITY_INSERT [sport].[TrainingPlan] ON;

INSERT INTO [sport].[TrainingPlan]
    ([Id], [SeasonCategory_Season_Id], [SeasonCategory_Name],
     [TrainingType_Id], [TrainingPhase_Id],
     [From], [To], [Location], [TimeFrom], [TimeTo], [DayName])
SELECT
     [Id],
     [Season_Id],          -- přejmenováno na SeasonCategory_Season_Id
     [SeasonCategory_Name],
     [TrainingType_Id],
     [TrainingPhase_Id],
     [From], [To], [Location], [TimeFrom], [TimeTo], [DayName]
FROM [Hockey].[sport].[TrainingPlan];

SET IDENTITY_INSERT [sport].[TrainingPlan] OFF;
PRINT 'OK: sport.TrainingPlan – ' + CAST(@@ROWCOUNT AS varchar(10)) + ' řádků';

-- --------------------------------------------------------
-- 5. sport.TrainingEntitlement  (Season_Id → SeasonCategory_Season_Id)
-- --------------------------------------------------------
SET IDENTITY_INSERT [sport].[TrainingEntitlement] ON;

INSERT INTO [sport].[TrainingEntitlement]
    ([Id], [SeasonCategory_Season_Id], [SeasonCategory_Name],
     [TrainingType_Id], [TrainingPhase_Id],
     [From], [To], [DurationHours])
SELECT
     [Id],
     [Season_Id],          -- přejmenováno na SeasonCategory_Season_Id
     [SeasonCategory_Name],
     [TrainingType_Id],
     [TrainingPhase_Id],
     [From], [To], [DurationHours]
FROM [Hockey].[sport].[TrainingEntitlement];

SET IDENTITY_INSERT [sport].[TrainingEntitlement] OFF;
PRINT 'OK: sport.TrainingEntitlement – ' + CAST(@@ROWCOUNT AS varchar(10)) + ' řádků';

-- --------------------------------------------------------
-- 6. sport.Training  (bez IDENTITY – Id přiděluje sekvence)
--    Season_Id → SeasonCategory_Season_Id
--    Location: nvarchar(100) → varchar(100)
--    IceRink_Id odebran v 01_schema_prep.sql
-- --------------------------------------------------------
INSERT INTO [sport].[Training]
    ([Id], [SeasonCategory_Season_Id], [SeasonCategory_Name],
     [TrainingType_Id], [TrainingPhase_Id], [TrainingState_Id], [TrainingPlan_Id],
     [Location], [TimeFrom], [TimeTo], [Date], [Note])
SELECT
     [Id],
     [Season_Id],          -- přejmenováno na SeasonCategory_Season_Id
     [SeasonCategory_Name],
     [TrainingType_Id],
     [TrainingPhase_Id],
     [TrainingState_Id],
     [TrainingPlan_Id],
     CAST([Location] AS varchar(100)),
     [TimeFrom], [TimeTo], [Date], [Note]
FROM [Hockey].[sport].[Training];

PRINT 'OK: sport.Training – ' + CAST(@@ROWCOUNT AS varchar(10)) + ' řádků';

-- Posun sekvence nad maximum importovaných Id, aby nové záznamy nekolidovaly
DECLARE @MaxTrainingId INT = ISNULL((SELECT MAX(Id) FROM [sport].[Training]), 0);
DECLARE @SeqSql NVARCHAR(200) = N'ALTER SEQUENCE [sport].[SportEventSeq] RESTART WITH '
    + CAST(@MaxTrainingId + 1 AS NVARCHAR(10));
EXEC sp_executesql @SeqSql;
PRINT 'OK: sport.SportEventSeq restartována na ' + CAST(@MaxTrainingId + 1 AS varchar(10));

-- --------------------------------------------------------
-- 7. sport.CoachTraining  (composite PK, bez IDENTITY)
-- --------------------------------------------------------
INSERT INTO [sport].[CoachTraining] ([Coach_Id], [Training_Id], [ParticipationType_Id], [Note])
SELECT [Coach_Id], [Training_Id], [ParticipationType_Id], [Note]
FROM [Hockey].[sport].[CoachTraining];

PRINT 'OK: sport.CoachTraining – ' + CAST(@@ROWCOUNT AS varchar(10)) + ' řádků';

-- --------------------------------------------------------
-- 8. sport.CoachTrainingPlan  (composite PK, bez IDENTITY)
-- --------------------------------------------------------
INSERT INTO [sport].[CoachTrainingPlan] ([Coach_Id], [TrainingPlan_Id], [ValidFrom], [ValidTo])
SELECT [Coach_Id], [TrainingPlan_Id], [ValidFrom], [ValidTo]
FROM [Hockey].[sport].[CoachTrainingPlan];

PRINT 'OK: sport.CoachTrainingPlan – ' + CAST(@@ROWCOUNT AS varchar(10)) + ' řádků';

-- --------------------------------------------------------
-- Výsledek
-- --------------------------------------------------------
PRINT '--- Migrace dokončena (transakce čeká na COMMIT/ROLLBACK) ---';

-- Pro ostré spuštění změň ROLLBACK na COMMIT:
--ROLLBACK;
 COMMIT;
