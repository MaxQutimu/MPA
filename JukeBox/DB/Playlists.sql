USE [Jukebox]
GO

ALTER TABLE [dbo].[Playlists] DROP CONSTRAINT [FK__Playlists__User___5070F446]
GO

ALTER TABLE [dbo].[Playlists] DROP CONSTRAINT [DF__Playlists__Is_Pu__4F7CD00D]
GO

ALTER TABLE [dbo].[Playlists] DROP CONSTRAINT [DF__Playlists__Date___4E88ABD4]
GO

/****** Object:  Table [dbo].[Playlists]    Script Date: 22.05.2024 00:33:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Playlists]') AND type in (N'U'))
DROP TABLE [dbo].[Playlists]
GO

/****** Object:  Table [dbo].[Playlists]    Script Date: 22.05.2024 00:33:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Playlists](
	[Playlist_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[User_ID] [bigint] NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Date_Created] [date] NULL,
	[Date_Modified] [date] NULL,
	[Is_Public] [bit] NULL,
	[Cover_Image_URL] [nvarchar](255) NULL,
	[Number_of_Songs] [int] NULL,
 CONSTRAINT [PK__Playlist__F5922DFFB523592E] PRIMARY KEY CLUSTERED 
(
	[Playlist_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Playlists] ADD  CONSTRAINT [DF__Playlists__Date___4E88ABD4]  DEFAULT (getdate()) FOR [Date_Created]
GO

ALTER TABLE [dbo].[Playlists] ADD  CONSTRAINT [DF__Playlists__Is_Pu__4F7CD00D]  DEFAULT ((1)) FOR [Is_Public]
GO

ALTER TABLE [dbo].[Playlists]  WITH CHECK ADD  CONSTRAINT [FK__Playlists__User___5070F446] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO

ALTER TABLE [dbo].[Playlists] CHECK CONSTRAINT [FK__Playlists__User___5070F446]
GO

