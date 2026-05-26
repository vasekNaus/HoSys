CREATE TABLE [sport].[Team](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[HomeIceRink_Id] [int] NULL,
	CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

ALTER TABLE [sport].[Team]  WITH CHECK ADD  CONSTRAINT [FK_Team_IceRink] FOREIGN KEY([HomeIceRink_Id])
REFERENCES [sport].[IceRink] ([Id])
GO

ALTER TABLE [sport].[Team] CHECK CONSTRAINT [FK_Team_IceRink]
GO
