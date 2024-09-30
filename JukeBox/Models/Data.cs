using JukeBox.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace JukeBox.Data
{
    public class JukeBoxRepository
    {
        private readonly string _connectionString = "Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True";

        public List<Song> GetSongs(string searchQuery)
        {
            List<Song> songs = new List<Song>();
            

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = string.IsNullOrEmpty(searchQuery) ?
                    "SELECT * FROM Songs" :
                    "SELECT * FROM Songs WHERE Title LIKE @SearchQuery OR Artist LIKE @SearchQuery OR Genre LIKE @SearchQuery";

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    cmd.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                }

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    songs.Add(new Song
                    {
                        SongId = Convert.ToInt32(reader["Song_ID"]),
                        Title = reader["Title"].ToString(),
                        Artist = reader["Artist"].ToString(),
                        Url = reader["Url"].ToString(),
                        PhotoUrl = reader["PhotoUrl"].ToString(),
                    });
                }
            }
            return songs;
        }

        public List<Playlist> GetPlaylists(string userId)
        {
            List<Playlist> playlists = new List<Playlist>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Playlists WHERE User_ID = @User_ID", conn);
                cmd.Parameters.AddWithValue("@User_ID", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    playlists.Add(new Playlist
                    {
                        PlaylistId = Convert.ToInt32(reader["Playlist_ID"]),
                        Name = reader["Name"].ToString(),
                        DateCreated = (DateTime)reader["Date_Created"],
                        CoverImageUrl = reader["Cover_Image_URL"].ToString(),
                    });
                }
            }
            return playlists;
        }
        public int AddSongToPlaylist(int songId, int playlistId, string newPlaylistName, string userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd;

                // Create new playlist if needed
                if (!string.IsNullOrEmpty(newPlaylistName))
                {
                    Playlist playlist = new Playlist();
                    Playlist newPlaylist = playlist.CreatePlaylist(userId, newPlaylistName);

                    if (newPlaylist != null)
                    {
                        
                    }
                    else
                    {
                       
                    }

                }

                // Check if the song already exists in the playlist
                if (IsSongInPlaylist(playlistId, songId, conn))
                {
                    return 0; // Song already exists in playlist
                }

                // Add the song to the playlist
                cmd = new SqlCommand("INSERT INTO Playlist_Songs (Playlist_ID, Song_ID) VALUES (@Playlist_ID, @Song_ID)", conn);
                cmd.Parameters.AddWithValue("@Playlist_ID", playlistId);
                cmd.Parameters.AddWithValue("@Song_ID", songId);
                cmd.ExecuteNonQuery();

                return playlistId; // Song successfully added
            }
        }

        private bool IsSongInPlaylist(int playlistId, int songId, SqlConnection conn) //Check if song is already in playlist
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Playlist_Songs WHERE Playlist_ID = @Playlist_ID AND Song_ID = @Song_ID", conn);
            cmd.Parameters.AddWithValue("@Playlist_ID", playlistId);
            cmd.Parameters.AddWithValue("@Song_ID", songId);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

    }
}
