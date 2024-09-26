using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JukeBox
{
    public partial class Homepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User_ID"] != null)
                {
                    string searchQuery = Request.QueryString["search"];

                    // Fetch and bind songs and playlists for the user
                    BindSongsAndPlaylists(searchQuery);
                }
                else
                {
                    Response.Redirect("LoginSite.aspx");
                }
            }
        }

        private void BindSongsAndPlaylists(string searchQuery)
        {
            string userId = Session["User_ID"].ToString();

            // Get the user's playlists
            DataTable playlists = GetPlaylistsForUser(userId);
            ddlPlaylists.DataSource = playlists;
            ddlPlaylists.DataTextField = "Name";
            ddlPlaylists.DataValueField = "Playlist_ID";
            ddlPlaylists.DataBind();


            PlaylistsRepeater.DataSource = playlists;
            PlaylistsRepeater.DataBind();
            System.Diagnostics.Debug.WriteLine("testing that itch ddl" + playlists);


            // Get the songs for the user based on the search query
            DataTable songs = GetSongsForUser(userId, searchQuery);
            SongsRepeater.DataSource = songs;
            SongsRepeater.DataBind();
        }

        private DataTable GetPlaylistsForUser(string userId)
        {
            DataTable playlistsData = new DataTable();
            using (SqlConnection conn = new SqlConnection("Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True"))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Playlists WHERE User_ID = @User_ID", conn);
                cmd.Parameters.AddWithValue("@User_ID", userId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(playlistsData);

            }
            return playlistsData;
        }

        private DataTable GetSongsForUser(string userId, string searchQuery)
        {
            DataTable songsData = new DataTable();
            using (SqlConnection conn = new SqlConnection("Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True"))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                // Base query to get all songs for the user
                cmd.CommandText = "SELECT * FROM Songs";

                // Add search condition if search query is provided
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    cmd.CommandText += " AND Title LIKE @SearchQuery";
                    cmd.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                }

                cmd.Parameters.AddWithValue("@User_ID", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(songsData);
            }
            return songsData;
        }

        protected void btnAddToPlaylist_Click(object sender, EventArgs e)
        {
            string selectedPlaylistId = ddlPlaylists.SelectedValue;
            string newPlaylistName = txtNewPlaylist.Text;
            int songId = int.Parse(hiddenSongId.Value);
            int playlistId;

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
                        ClientScript.RegisterStartupScript(this.GetType(), "PlaylistCreationFailed", "alert('Failed to create the new playlist.');", true);
                        return;
                    }
                }
                else
                {
                    playlistId = int.Parse(selectedPlaylistId);
                }

                if (IsSongInPlaylist(playlistId, songId))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "SongAlreadyInPlaylist", "alert('This song is already in the selected playlist.');", true);
                }
                else
                {
                    cmd = new SqlCommand("INSERT INTO Playlist_Songs (Playlist_ID, Song_ID) VALUES (@Playlist_ID, @Song_ID)", conn);
                    cmd.Parameters.AddWithValue("@Playlist_ID", playlistId);
                    cmd.Parameters.AddWithValue("@Song_ID", songId);
                    cmd.ExecuteNonQuery();
                    ClientScript.RegisterStartupScript(this.GetType(), "SongAddedToPlaylist", "alert('Song added to the playlist successfully.');", true);
                }
            }

            // Hide modal after adding to playlist
            ClientScript.RegisterStartupScript(this.GetType(), "HideModalScript", "document.getElementById('playlistModal').style.display = 'none';", true);
            Response.Redirect("Homepage.aspx");
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
