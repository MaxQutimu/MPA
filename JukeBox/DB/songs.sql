USE [Jukebox]
GO

/****** Object:  Table [dbo].[Songs]    Script Date: 22.05.2024 00:33:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Songs]') AND type in (N'U'))
DROP TABLE [dbo].[Songs]
GO

/****** Object:  Table [dbo].[Songs]    Script Date: 22.05.2024 00:33:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Songs](
	[Song_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](250) NULL,
	[Artist] [varchar](250) NULL,
	[Genre] [varchar](50) NULL,
	[PhotoUrl] [varchar](250) NULL,
	[Duration] [time](7) NULL,
	[SongDescription] [varchar](2000) NULL,
	[PlayedAmount] [varchar](16) NULL,
	[Url] [nchar](250) NULL,
 CONSTRAINT [PK_Songs] PRIMARY KEY CLUSTERED 
(
	[Song_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

