CREATE TABLE [dbo].[Opponent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[HomeIceRink_Id] [int] NULL,
	CONSTRAINT [PK_Opponent] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

ALTER TABLE [dbo].[Opponent]  WITH CHECK ADD  CONSTRAINT [FK_Opponent_IceRink] FOREIGN KEY([HomeIceRink_Id])
REFERENCES [dbo].[IceRink] ([Id])
GO

ALTER TABLE [dbo].[Opponent] CHECK CONSTRAINT [FK_Opponent_IceRink]
GO
