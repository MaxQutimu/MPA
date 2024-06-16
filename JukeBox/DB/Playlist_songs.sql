USE [Jukebox]
GO

ALTER TABLE [dbo].[Playlist_Songs] DROP CONSTRAINT [FK__Playlist___Song___5441852A]
GO

ALTER TABLE [dbo].[Playlist_Songs] DROP CONSTRAINT [FK__Playlist___Playl__534D60F1]
GO

/****** Object:  Table [dbo].[Playlist_Songs]    Script Date: 22.05.2024 00:33:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Playlist_Songs]') AND type in (N'U'))
DROP TABLE [dbo].[Playlist_Songs]
GO

/****** Object:  Table [dbo].[Playlist_Songs]    Script Date: 22.05.2024 00:33:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Playlist_Songs](
	[Playlist_ID] [bigint] NOT NULL,
	[Song_ID] [bigint] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Playlist_Songs]  WITH CHECK ADD  CONSTRAINT [FK__Playlist___Playl__534D60F1] FOREIGN KEY([Playlist_ID])
REFERENCES [dbo].[Playlists] ([Playlist_ID])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Playlist_Songs] CHECK CONSTRAINT [FK__Playlist___Playl__534D60F1]
GO

ALTER TABLE [dbo].[Playlist_Songs]  WITH CHECK ADD  CONSTRAINT [FK__Playlist___Song___5441852A] FOREIGN KEY([Song_ID])
REFERENCES [dbo].[Songs] ([Song_ID])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Playlist_Songs] CHECK CONSTRAINT [FK__Playlist___Song___5441852A]
GO

