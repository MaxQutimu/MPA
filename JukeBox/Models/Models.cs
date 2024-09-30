using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace JukeBox.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Url { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class Playlist
    {
        public int PlaylistId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string CoverImageUrl { get; set; }

        private string _connectionString = "Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True";

        public Playlist CreatePlaylist(string userId, string playlistName)
        {
            Playlist newPlaylist = new Playlist
            {
                UserId = userId,
                Name = playlistName,
                DateCreated = DateTime.Now,
                CoverImageUrl = "https://i.scdn.co/image/ab67706c0000da8470d229cb865e8d81cdce0889"
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Playlists (Name, User_ID, Date_Created, Cover_Image_URL) OUTPUT INSERTED.Playlist_ID VALUES (@Name, @User_ID, @Date_Created, @PlaylistIMG)",
                    conn
                );
                cmd.Parameters.AddWithValue("@Name", newPlaylist.Name);
                cmd.Parameters.AddWithValue("@User_ID", newPlaylist.UserId);
                cmd.Parameters.AddWithValue("@Date_Created", newPlaylist.DateCreated);
                cmd.Parameters.AddWithValue("@PlaylistIMG", newPlaylist.CoverImageUrl);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    newPlaylist.PlaylistId = Convert.ToInt32(result); // Set the generated PlaylistId
                    return newPlaylist; // Return the created playlist
                }
                else
                {
                    return null; // Playlist creation failed
                }
            }
        }
    }

    public class HomePageViewModel
    {
        public List<Song> Songs { get; set; }
        public List<Playlist> Playlists { get; set; }
    }
}
