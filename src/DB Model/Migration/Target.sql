USE [SportSys]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coach]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coach](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[FullName]  AS (([FirstName]+N' ')+[LastName]) PERSISTED NOT NULL,
 CONSTRAINT [PK_Coach] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CoachRole]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CoachRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CoachRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[CoachTraining]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[CoachTraining](
	[Coach_Id] [int] NOT NULL,
	[Training_Id] [int] NOT NULL,
	[ParticipationType_Id] [int] NOT NULL,
	[Note] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CoachTraining] PRIMARY KEY CLUSTERED 
(
	[Coach_Id] ASC,
	[Training_Id] ASC,
	[ParticipationType_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[CoachTrainingEntitlement]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[CoachTrainingEntitlement](
	[Coach_Id] [int] NOT NULL,
	[TrainingEntitlement_Id] [int] NOT NULL,
	[CoachRole_Id] [int] NOT NULL,
 CONSTRAINT [PK_CoachTrainingEntitlement] PRIMARY KEY CLUSTERED 
(
	[Coach_Id] ASC,
	[TrainingEntitlement_Id] ASC,
	[CoachRole_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[CoachTrainingPlan]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[CoachTrainingPlan](
	[Coach_Id] [int] NOT NULL,
	[TrainingPlan_Id] [int] NOT NULL,
	[ValidFrom] [date] NOT NULL,
	[ValidTo] [date] NOT NULL,
 CONSTRAINT [PK_CoachTrainingPlan] PRIMARY KEY CLUSTERED 
(
	[Coach_Id] ASC,
	[TrainingPlan_Id] ASC,
	[ValidFrom] ASC,
	[ValidTo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[IceRink]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[IceRink](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[Location] [geography] NULL,
 CONSTRAINT [PK_IceRink] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [sport].[Match]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[Match](
	[Id] [int] NOT NULL,
	[SeasonCategory_Season_Id] [int] NOT NULL,
	[SeasonCategory_Name] [varchar](10) NOT NULL,
	[IceRink_Id] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[TimeFrom] [time](0) NOT NULL,
	[TimeTo] [time](0) NOT NULL,
	[DurationMinutes]  AS (datediff(minute,[TimeFrom],[TimeTo])) PERSISTED,
	[Note] [varchar](50) NOT NULL,
	[MatchCode] [varchar](10) NULL,
	[Opponent_Id] [int] NOT NULL,
	[IsHome] [bit] NOT NULL,
	[Home] [int] NOT NULL,
	[Away] [int] NOT NULL,
	[MatchType_Id] [int] NOT NULL,
 CONSTRAINT [PK_Match] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[MatchType]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[MatchType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_MatchType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[Opponent]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[Opponent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[HomeIceRink_Id] [int] NULL,
 CONSTRAINT [PK_Opponent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[ParticipationType]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[ParticipationType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ParticipationType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[Season]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[Season](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[From] [date] NOT NULL,
	[To] [date] NOT NULL,
 CONSTRAINT [PK_Season] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[SeasonCategory]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[SeasonCategory](
	[Season_Id] [int] NOT NULL,
	[Name] [varchar](10) NOT NULL,
	[Order] [int] NOT NULL,
	[BirthYears] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_SeasonCategory] PRIMARY KEY CLUSTERED 
(
	[Season_Id] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[Training]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[Training](
	[Id] [int] NOT NULL,
	[SeasonCategory_Season_Id] [int] NOT NULL,
	[SeasonCategory_Name] [varchar](10) NOT NULL,
	[TrainingType_Id] [int] NOT NULL,
	[TrainingPhase_Id] [int] NOT NULL,
	[TrainingState_Id] [int] NOT NULL,
	[TrainingPlan_Id] [int] NULL,
	[IceRink_Id] [int] NOT NULL,
	[Location] [varchar](100) NOT NULL,
	[TimeFrom] [time](0) NOT NULL,
	[TimeTo] [time](0) NOT NULL,
	[Date] [date] NOT NULL,
	[DurationMinutes]  AS (datediff(minute,[TimeFrom],[TimeTo])) PERSISTED,
	[Note] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Training] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[TrainingEntitlement]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[TrainingEntitlement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SeasonCategory_Season_Id] [int] NOT NULL,
	[SeasonCategory_Name] [varchar](10) NOT NULL,
	[TrainingType_Id] [int] NOT NULL,
	[TrainingPhase_Id] [int] NOT NULL,
	[From] [date] NOT NULL,
	[To] [date] NOT NULL,
	[DurationHours] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_TrainingEntitlement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[TrainingPhase]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[TrainingPhase](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TrainingPhase] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[TrainingPlan]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[TrainingPlan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SeasonCategory_Season_Id] [int] NOT NULL,
	[SeasonCategory_Name] [varchar](10) NOT NULL,
	[TrainingType_Id] [int] NOT NULL,
	[TrainingPhase_Id] [int] NOT NULL,
	[From] [date] NOT NULL,
	[To] [date] NOT NULL,
	[Location] [nvarchar](100) NOT NULL,
	[TimeFrom] [time](0) NOT NULL,
	[TimeTo] [time](0) NOT NULL,
	[DurationMinutes]  AS (datediff(minute,[TimeFrom],[TimeTo])) PERSISTED,
	[DayName] [varchar](10) NOT NULL,
 CONSTRAINT [PK_TrainingPlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[TrainingState]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[TrainingState](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TrainingState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sport].[TrainingType]    Script Date: 22.05.2026 9:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sport].[TrainingType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TrainingType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [sport].[CoachTraining] ADD  CONSTRAINT [DF_CoachTraining_Note]  DEFAULT ('') FOR [Note]
GO
ALTER TABLE [sport].[Match] ADD  CONSTRAINT [DF_Match_Id]  DEFAULT (NEXT VALUE FOR [sport].[SportEventSeq]) FOR [Id]
GO
ALTER TABLE [sport].[SeasonCategory] ADD  CONSTRAINT [DF_SeasonCategory_BirthYears]  DEFAULT (N'[]') FOR [BirthYears]
GO
ALTER TABLE [sport].[Training] ADD  CONSTRAINT [DF_Training_Id]  DEFAULT (NEXT VALUE FOR [sport].[SportEventSeq]) FOR [Id]
GO
ALTER TABLE [sport].[CoachTraining]  WITH CHECK ADD  CONSTRAINT [FK_CoachTraining_Coach_Coach_Id] FOREIGN KEY([Coach_Id])
REFERENCES [dbo].[Coach] ([Id])
GO
ALTER TABLE [sport].[CoachTraining] CHECK CONSTRAINT [FK_CoachTraining_Coach_Coach_Id]
GO
ALTER TABLE [sport].[CoachTraining]  WITH CHECK ADD  CONSTRAINT [FK_CoachTraining_ParticipationType_ParticipationType_Id] FOREIGN KEY([ParticipationType_Id])
REFERENCES [sport].[ParticipationType] ([Id])
GO
ALTER TABLE [sport].[CoachTraining] CHECK CONSTRAINT [FK_CoachTraining_ParticipationType_ParticipationType_Id]
GO
ALTER TABLE [sport].[CoachTraining]  WITH CHECK ADD  CONSTRAINT [FK_CoachTraining_Training_Training_Id] FOREIGN KEY([Training_Id])
REFERENCES [sport].[Training] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sport].[CoachTraining] CHECK CONSTRAINT [FK_CoachTraining_Training_Training_Id]
GO
ALTER TABLE [sport].[CoachTrainingEntitlement]  WITH CHECK ADD  CONSTRAINT [FK_CoachTrainingEntitlement_Coach_Coach_Id] FOREIGN KEY([Coach_Id])
REFERENCES [dbo].[Coach] ([Id])
GO
ALTER TABLE [sport].[CoachTrainingEntitlement] CHECK CONSTRAINT [FK_CoachTrainingEntitlement_Coach_Coach_Id]
GO
ALTER TABLE [sport].[CoachTrainingEntitlement]  WITH CHECK ADD  CONSTRAINT [FK_CoachTrainingEntitlement_CoachRole_CoachRole_Id] FOREIGN KEY([CoachRole_Id])
REFERENCES [dbo].[CoachRole] ([Id])
GO
ALTER TABLE [sport].[CoachTrainingEntitlement] CHECK CONSTRAINT [FK_CoachTrainingEntitlement_CoachRole_CoachRole_Id]
GO
ALTER TABLE [sport].[CoachTrainingEntitlement]  WITH CHECK ADD  CONSTRAINT [FK_CoachTrainingEntitlement_TrainingEntitlement_TrainingEntitlement_Id] FOREIGN KEY([TrainingEntitlement_Id])
REFERENCES [sport].[TrainingEntitlement] ([Id])
GO
ALTER TABLE [sport].[CoachTrainingEntitlement] CHECK CONSTRAINT [FK_CoachTrainingEntitlement_TrainingEntitlement_TrainingEntitlement_Id]
GO
ALTER TABLE [sport].[CoachTrainingPlan]  WITH CHECK ADD  CONSTRAINT [FK_CoachTrainingPlan_Coach_Coach_Id] FOREIGN KEY([Coach_Id])
REFERENCES [dbo].[Coach] ([Id])
GO
ALTER TABLE [sport].[CoachTrainingPlan] CHECK CONSTRAINT [FK_CoachTrainingPlan_Coach_Coach_Id]
GO
ALTER TABLE [sport].[CoachTrainingPlan]  WITH CHECK ADD  CONSTRAINT [FK_CoachTrainingPlan_TrainingPlan_TrainingPlan_Id] FOREIGN KEY([TrainingPlan_Id])
REFERENCES [sport].[TrainingPlan] ([Id])
GO
ALTER TABLE [sport].[CoachTrainingPlan] CHECK CONSTRAINT [FK_CoachTrainingPlan_TrainingPlan_TrainingPlan_Id]
GO
ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_IceRink_IceRink_Id] FOREIGN KEY([IceRink_Id])
REFERENCES [sport].[IceRink] ([Id])
GO
ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_IceRink_IceRink_Id]
GO
ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_MatchType_MatchType_Id] FOREIGN KEY([MatchType_Id])
REFERENCES [sport].[MatchType] ([Id])
GO
ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_MatchType_MatchType_Id]
GO
ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_Opponent_Opponent_Id] FOREIGN KEY([Opponent_Id])
REFERENCES [sport].[Opponent] ([Id])
GO
ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_Opponent_Opponent_Id]
GO
ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_Season_SeasonCategory_Season_Id] FOREIGN KEY([SeasonCategory_Season_Id])
REFERENCES [sport].[Season] ([Id])
GO
ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_Season_SeasonCategory_Season_Id]
GO
ALTER TABLE [sport].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name] FOREIGN KEY([SeasonCategory_Season_Id], [SeasonCategory_Name])
REFERENCES [sport].[SeasonCategory] ([Season_Id], [Name])
GO
ALTER TABLE [sport].[Match] CHECK CONSTRAINT [FK_Match_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name]
GO
ALTER TABLE [sport].[Opponent]  WITH CHECK ADD  CONSTRAINT [FK_Opponent_IceRink_HomeIceRink_Id] FOREIGN KEY([HomeIceRink_Id])
REFERENCES [sport].[IceRink] ([Id])
GO
ALTER TABLE [sport].[Opponent] CHECK CONSTRAINT [FK_Opponent_IceRink_HomeIceRink_Id]
GO
ALTER TABLE [sport].[SeasonCategory]  WITH CHECK ADD  CONSTRAINT [FK_SeasonCategory_Season_Season_Id] FOREIGN KEY([Season_Id])
REFERENCES [sport].[Season] ([Id])
GO
ALTER TABLE [sport].[SeasonCategory] CHECK CONSTRAINT [FK_SeasonCategory_Season_Season_Id]
GO
ALTER TABLE [sport].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_IceRink_IceRink_Id] FOREIGN KEY([IceRink_Id])
REFERENCES [sport].[IceRink] ([Id])
GO
ALTER TABLE [sport].[Training] CHECK CONSTRAINT [FK_Training_IceRink_IceRink_Id]
GO
ALTER TABLE [sport].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_Season_SeasonCategory_Season_Id] FOREIGN KEY([SeasonCategory_Season_Id])
REFERENCES [sport].[Season] ([Id])
GO
ALTER TABLE [sport].[Training] CHECK CONSTRAINT [FK_Training_Season_SeasonCategory_Season_Id]
GO
ALTER TABLE [sport].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name] FOREIGN KEY([SeasonCategory_Season_Id], [SeasonCategory_Name])
REFERENCES [sport].[SeasonCategory] ([Season_Id], [Name])
GO
ALTER TABLE [sport].[Training] CHECK CONSTRAINT [FK_Training_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name]
GO
ALTER TABLE [sport].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_TrainingPhase_TrainingPhase_Id] FOREIGN KEY([TrainingPhase_Id])
REFERENCES [sport].[TrainingPhase] ([Id])
GO
ALTER TABLE [sport].[Training] CHECK CONSTRAINT [FK_Training_TrainingPhase_TrainingPhase_Id]
GO
ALTER TABLE [sport].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_TrainingPlan_TrainingPlan_Id] FOREIGN KEY([TrainingPlan_Id])
REFERENCES [sport].[TrainingPlan] ([Id])
GO
ALTER TABLE [sport].[Training] CHECK CONSTRAINT [FK_Training_TrainingPlan_TrainingPlan_Id]
GO
ALTER TABLE [sport].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_TrainingState_TrainingState_Id] FOREIGN KEY([TrainingState_Id])
REFERENCES [sport].[TrainingState] ([Id])
GO
ALTER TABLE [sport].[Training] CHECK CONSTRAINT [FK_Training_TrainingState_TrainingState_Id]
GO
ALTER TABLE [sport].[Training]  WITH CHECK ADD  CONSTRAINT [FK_Training_TrainingType_TrainingType_Id] FOREIGN KEY([TrainingType_Id])
REFERENCES [sport].[TrainingType] ([Id])
GO
ALTER TABLE [sport].[Training] CHECK CONSTRAINT [FK_Training_TrainingType_TrainingType_Id]
GO
ALTER TABLE [sport].[TrainingEntitlement]  WITH CHECK ADD  CONSTRAINT [FK_TrainingEntitlement_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name] FOREIGN KEY([SeasonCategory_Season_Id], [SeasonCategory_Name])
REFERENCES [sport].[SeasonCategory] ([Season_Id], [Name])
GO
ALTER TABLE [sport].[TrainingEntitlement] CHECK CONSTRAINT [FK_TrainingEntitlement_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name]
GO
ALTER TABLE [sport].[TrainingEntitlement]  WITH CHECK ADD  CONSTRAINT [FK_TrainingEntitlement_TrainingPhase_TrainingPhase_Id] FOREIGN KEY([TrainingPhase_Id])
REFERENCES [sport].[TrainingPhase] ([Id])
GO
ALTER TABLE [sport].[TrainingEntitlement] CHECK CONSTRAINT [FK_TrainingEntitlement_TrainingPhase_TrainingPhase_Id]
GO
ALTER TABLE [sport].[TrainingEntitlement]  WITH CHECK ADD  CONSTRAINT [FK_TrainingEntitlement_TrainingType_TrainingType_Id] FOREIGN KEY([TrainingType_Id])
REFERENCES [sport].[TrainingType] ([Id])
GO
ALTER TABLE [sport].[TrainingEntitlement] CHECK CONSTRAINT [FK_TrainingEntitlement_TrainingType_TrainingType_Id]
GO
ALTER TABLE [sport].[TrainingPlan]  WITH CHECK ADD  CONSTRAINT [FK_TrainingPlan_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name] FOREIGN KEY([SeasonCategory_Season_Id], [SeasonCategory_Name])
REFERENCES [sport].[SeasonCategory] ([Season_Id], [Name])
GO
ALTER TABLE [sport].[TrainingPlan] CHECK CONSTRAINT [FK_TrainingPlan_SeasonCategory_SeasonCategory_Season_Id_SeasonCategory_Name]
GO
ALTER TABLE [sport].[TrainingPlan]  WITH CHECK ADD  CONSTRAINT [FK_TrainingPlan_TrainingPhase_TrainingPhase_Id] FOREIGN KEY([TrainingPhase_Id])
REFERENCES [sport].[TrainingPhase] ([Id])
GO
ALTER TABLE [sport].[TrainingPlan] CHECK CONSTRAINT [FK_TrainingPlan_TrainingPhase_TrainingPhase_Id]
GO
ALTER TABLE [sport].[TrainingPlan]  WITH CHECK ADD  CONSTRAINT [FK_TrainingPlan_TrainingType_TrainingType_Id] FOREIGN KEY([TrainingType_Id])
REFERENCES [sport].[TrainingType] ([Id])
GO
ALTER TABLE [sport].[TrainingPlan] CHECK CONSTRAINT [FK_TrainingPlan_TrainingType_TrainingType_Id]
GO
