-- ============================================================
-- Ověření shody číselníkových tabulek: [Hockey] vs [SportSys]
-- Spustit před migrací dat. Výsledky s Stav <> 'OK' vyžadují ruční kontrolu.
-- ============================================================

-- --------------------------------------------------------
-- dbo.CoachRole
-- --------------------------------------------------------
PRINT '=== dbo.CoachRole ==='
SELECT
    COALESCE(s.Id, t.Id)   AS Id,
    s.Name                  AS Source_Name,
    t.Name                  AS Target_Name,
    CASE
        WHEN s.Id IS NULL       THEN 'Pouze v Target'
        WHEN t.Id IS NULL       THEN 'Pouze v Source'
        WHEN s.Name <> t.Name   THEN 'Neshodné hodnoty'
        ELSE 'OK'
    END AS Stav
FROM [Hockey].[dbo].[CoachRole] s
FULL OUTER JOIN [SportSys].[dbo].[CoachRole] t ON s.Id = t.Id
ORDER BY COALESCE(s.Id, t.Id);

-- --------------------------------------------------------
-- sport.MatchType
-- --------------------------------------------------------
PRINT '=== sport.MatchType ==='
SELECT
    COALESCE(s.Id, t.Id)   AS Id,
    s.Name                  AS Source_Name,
    t.Name                  AS Target_Name,
    CASE
        WHEN s.Id IS NULL       THEN 'Pouze v Target'
        WHEN t.Id IS NULL       THEN 'Pouze v Source'
        WHEN s.Name <> t.Name   THEN 'Neshodné hodnoty'
        ELSE 'OK'
    END AS Stav
FROM [Hockey].[sport].[MatchType] s
FULL OUTER JOIN [SportSys].[sport].[MatchType] t ON s.Id = t.Id
ORDER BY COALESCE(s.Id, t.Id);

-- --------------------------------------------------------
-- sport.ParticipationType
-- --------------------------------------------------------
PRINT '=== sport.ParticipationType ==='
SELECT
    COALESCE(s.Id, t.Id)   AS Id,
    s.Name                  AS Source_Name,
    t.Name                  AS Target_Name,
    CASE
        WHEN s.Id IS NULL       THEN 'Pouze v Target'
        WHEN t.Id IS NULL       THEN 'Pouze v Source'
        WHEN s.Name <> t.Name   THEN 'Neshodné hodnoty'
        ELSE 'OK'
    END AS Stav
FROM [Hockey].[sport].[ParticipationType] s
FULL OUTER JOIN [SportSys].[sport].[ParticipationType] t ON s.Id = t.Id
ORDER BY COALESCE(s.Id, t.Id);

-- --------------------------------------------------------
-- sport.TrainingPhase
-- --------------------------------------------------------
PRINT '=== sport.TrainingPhase ==='
SELECT
    COALESCE(s.Id, t.Id)   AS Id,
    s.Name                  AS Source_Name,
    t.Name                  AS Target_Name,
    CASE
        WHEN s.Id IS NULL       THEN 'Pouze v Target'
        WHEN t.Id IS NULL       THEN 'Pouze v Source'
        WHEN s.Name <> t.Name   THEN 'Neshodné hodnoty'
        ELSE 'OK'
    END AS Stav
FROM [Hockey].[sport].[TrainingPhase] s
FULL OUTER JOIN [SportSys].[sport].[TrainingPhase] t ON s.Id = t.Id
ORDER BY COALESCE(s.Id, t.Id);

-- --------------------------------------------------------
-- sport.TrainingState
-- --------------------------------------------------------
PRINT '=== sport.TrainingState ==='
SELECT
    COALESCE(s.Id, t.Id)   AS Id,
    s.Name                  AS Source_Name,
    t.Name                  AS Target_Name,
    CASE
        WHEN s.Id IS NULL       THEN 'Pouze v Target'
        WHEN t.Id IS NULL       THEN 'Pouze v Source'
        WHEN s.Name <> t.Name   THEN 'Neshodné hodnoty'
        ELSE 'OK'
    END AS Stav
FROM [Hockey].[sport].[TrainingState] s
FULL OUTER JOIN [SportSys].[sport].[TrainingState] t ON s.Id = t.Id
ORDER BY COALESCE(s.Id, t.Id);

-- --------------------------------------------------------
-- sport.TrainingType
-- --------------------------------------------------------
PRINT '=== sport.TrainingType ==='
SELECT
    COALESCE(s.Id, t.Id)   AS Id,
    s.Name                  AS Source_Name,
    t.Name                  AS Target_Name,
    CASE
        WHEN s.Id IS NULL       THEN 'Pouze v Target'
        WHEN t.Id IS NULL       THEN 'Pouze v Source'
        WHEN s.Name <> t.Name   THEN 'Neshodné hodnoty'
        ELSE 'OK'
    END AS Stav
FROM [Hockey].[sport].[TrainingType] s
FULL OUTER JOIN [SportSys].[sport].[TrainingType] t ON s.Id = t.Id
ORDER BY COALESCE(s.Id, t.Id);
