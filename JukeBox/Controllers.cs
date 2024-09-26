using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using JukeBox.Data;
using JukeBox.Models;

namespace JukeBox.Controllers
{
    public class HomeController : Controller
    {
        private JukeBoxRepository _repository = new JukeBoxRepository();

        public ActionResult Index(string search)
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var songs = _repository.GetSongs(search);
            var playlists = _repository.GetPlaylists(Session["User_ID"].ToString());

            var viewModel = new HomePageViewModel
            {
                Songs = songs,
                Playlists = playlists
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddToPlaylist(int songId, int playlistId, string newPlaylistName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True"))
                {
                    conn.Open();
                    SqlCommand cmd;

                    if (!string.IsNullOrEmpty(newPlaylistName))
                    {
                        // Create a new playlist
                        cmd = new SqlCommand("INSERT INTO Playlists (Name, User_ID, Date_Created, Cover_Image_URL) OUTPUT INSERTED.Playlist_ID VALUES (@Name, @User_ID, @Date_Created, @PlaylistIMG)", conn);
                        cmd.Parameters.AddWithValue("@Name", newPlaylistName);
                        cmd.Parameters.AddWithValue("@User_ID", Session["User_ID"].ToString());
                        cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@PlaylistIMG", "https://i.scdn.co/image/ab67706c0000da8470d229cb865e8d81cdce0889");

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            playlistId = Convert.ToInt32(result);
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed to create the new playlist.";
                            return RedirectToAction("Index");
                        }
                    }

                    if (IsSongInPlaylist(playlistId, songId))
                    {
                        TempData["ErrorMessage"] = "This song is already in the selected playlist.";
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO Playlist_Songs (Playlist_ID, Song_ID) VALUES (@Playlist_ID, @Song_ID)", conn);
                        cmd.Parameters.AddWithValue("@Playlist_ID", playlistId);
                        cmd.Parameters.AddWithValue("@Song_ID", songId);
                        cmd.ExecuteNonQuery();

                        TempData["SuccessMessage"] = "Song added to the playlist successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            // Redirect to the Index action to show updated playlists and songs.
            return RedirectToAction("Index");
        }

        private bool IsSongInPlaylist(int playlistId, int songId)
        {
            using (SqlConnection conn = new SqlConnection("Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Playlist_Songs WHERE Playlist_ID = @Playlist_ID AND Song_ID = @Song_ID", conn);
                cmd.Parameters.AddWithValue("@Playlist_ID", playlistId);
                cmd.Parameters.AddWithValue("@Song_ID", songId);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

    }
}
