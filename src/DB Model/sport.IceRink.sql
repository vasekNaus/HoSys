CREATE TABLE [sport].[IceRink](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[Location] [geography] NULL,
	CONSTRAINT [PK_IceRink] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO
