CREATE TABLE [sport].[MatchType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_MatchType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

INSERT INTO [sport].[MatchType] ([Name]) VALUES
	(N'Ligový zápas'),
	(N'Příprava'),
	(N'Turnaj')
GO
