-- Vytvoření schématu sport.
-- Spustit před ostatními sport.*.sql skripty.

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'sport')
    EXEC('CREATE SCHEMA [sport] AUTHORIZATION dbo');
GO
