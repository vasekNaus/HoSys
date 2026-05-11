CREATE TABLE [sport].[Match](
	[Id] [int] NOT NULL DEFAULT (NEXT VALUE FOR [sport].[SportEventSeq]),
	[Season_Id] [int] NOT NULL,
	[SeasonCategory_Name] [varchar](10) NOT NULL,
	[IceRink_Id] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[TimeFrom] [time](0) NOT NULL,
	[TimeTo] [time](0) NOT NULL,
	[DurationMinutes]  AS (datediff(minute,[TimeFrom],[TimeTo])) PERSISTED,
	[Note] [varchar](50) NOT NULL,
	[MatchCode] [varchar](10) NOT NULL,
	[Opponent_Id] [int] NOT NULL,
	[IsHome] [bit] NOT NULL,
	[GoalsScored] [tinyint] NULL,
	[GoalsConceded] [tinyint] NULL,
	[MatchState_Id] [int] NOT NULL,
	CONSTRAINT [PK_Match] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_Season] FOREIGN KEY([Season_Id])
REFERENCES [sport].[Season] ([Id])
ON UPDATE CASCADE
GO

ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_Season]
GO

ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_SeasonCategory] FOREIGN KEY([Season_Id], [SeasonCategory_Name])
REFERENCES [sport].[SeasonCategory] ([Season_Id], [Name])
ON UPDATE CASCADE
GO

ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_SeasonCategory]
GO

ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_MatchState] FOREIGN KEY([MatchState_Id])
REFERENCES [sport].[MatchState] ([Id])
GO

ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_MatchState]
GO

ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_IceRink] FOREIGN KEY([IceRink_Id])
REFERENCES [sport].[IceRink] ([Id])
GO

ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_IceRink]
GO

ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_Opponent] FOREIGN KEY([Opponent_Id])
REFERENCES [sport].[Opponent] ([Id])
GO

ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_Opponent]
GO

ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [CHK_Match_TimeRange] CHECK  (([TimeTo]>[TimeFrom]))
GO

ALTER TABLE [sport].[Match] CHECK CONSTRAINT [CHK_Match_TimeRange]
GO
