CREATE TABLE [dbo].[Training](
	[Id] [int] NOT NULL DEFAULT (NEXT VALUE FOR [dbo].[SportEventSeq]),
	[Season_Id] [int] NOT NULL,
	[SeasonCategory_Name] [varchar](10) NOT NULL,
	[TrainingType_Id] [int] NOT NULL,
	[TrainingPhase_Id] [int] NOT NULL,
	[TrainingState_Id] [int] NOT NULL,
	[TrainingPlan_Id] [int] NULL,
	[IceRink_Id] [int] NOT NULL,
	[TimeFrom] [time](0) NOT NULL,
	[TimeTo] [time](0) NOT NULL,
	[Date] [date] NOT NULL,
	[DurationMinutes]  AS (datediff(minute,[TimeFrom],[TimeTo])) PERSISTED,
	[Note] [varchar](50) NOT NULL,
	CONSTRAINT [PK_Training] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

ALTER TABLE [dbo].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_Season] FOREIGN KEY([Season_Id])
REFERENCES [dbo].[Season] ([Id])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Training] CHECK CONSTRAINT [FK_Training_Season]
GO

ALTER TABLE [dbo].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_SeasonCategory] FOREIGN KEY([Season_Id], [SeasonCategory_Name])
REFERENCES [dbo].[SeasonCategory] ([Season_Id], [Name])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Training] CHECK CONSTRAINT [FK_Training_SeasonCategory]
GO

ALTER TABLE [dbo].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_TrainingPhase] FOREIGN KEY([TrainingPhase_Id])
REFERENCES [dbo].[TrainingPhase] ([Id])
GO

ALTER TABLE [dbo].[Training] CHECK CONSTRAINT [FK_Training_TrainingPhase]
GO

ALTER TABLE [dbo].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_TrainingPlan] FOREIGN KEY([TrainingPlan_Id])
REFERENCES [dbo].[TrainingPlan] ([Id])
GO

ALTER TABLE [dbo].[Training] CHECK CONSTRAINT [FK_Training_TrainingPlan]
GO

ALTER TABLE [dbo].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_TrainingState] FOREIGN KEY([TrainingState_Id])
REFERENCES [dbo].[TrainingState] ([Id])
GO

ALTER TABLE [dbo].[Training] CHECK CONSTRAINT [FK_Training_TrainingState]
GO

ALTER TABLE [dbo].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_TrainingType] FOREIGN KEY([TrainingType_Id])
REFERENCES [dbo].[TrainingType] ([Id])
GO

ALTER TABLE [dbo].[Training] CHECK CONSTRAINT [FK_Training_TrainingType]
GO

ALTER TABLE [dbo].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_IceRink] FOREIGN KEY([IceRink_Id])
REFERENCES [dbo].[IceRink] ([Id])
GO

ALTER TABLE [dbo].[Training] CHECK CONSTRAINT [FK_Training_IceRink]
GO

ALTER TABLE [dbo].[Training]  WITH CHECK ADD  CONSTRAINT [CHK_Training_TimeRange] CHECK  (([TimeTo]>[TimeFrom]))
GO

ALTER TABLE [dbo].[Training] CHECK CONSTRAINT [CHK_Training_TimeRange]
GO


