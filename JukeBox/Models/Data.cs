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
    }
}
