using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
                    LoadSongs();
                    LoadPlaylists();
                }
                else
                {
                    Response.Redirect("LoginSite.aspx");
                }
            }
            else { 
            }
        }

        private void LoadSongs()
        {
            string searchQuery = Request.QueryString["search"];
            DataTable songsData = GetSongsFromDatabase(searchQuery);

            foreach (DataRow songRow in songsData.Rows)
            {
                string songTitle = songRow["Title"].ToString();
                string songArtist = songRow["Artist"].ToString();
                string songUrl = songRow["Url"].ToString();
                string photoUrl = songRow["PhotoUrl"].ToString();
                int songId = Convert.ToInt32(songRow["Song_ID"]);

                HtmlGenericControl songDiv = new HtmlGenericControl("div");
                songDiv.Attributes["class"] = "song";

                HtmlGenericControl imgElement = new HtmlGenericControl("img");
                imgElement.Attributes["src"] = photoUrl;
                imgElement.Attributes["alt"] = songTitle;

                HtmlGenericControl detailsDiv = new HtmlGenericControl("div");
                detailsDiv.Attributes["class"] = "song-details";

                HtmlGenericControl titleParagraph = new HtmlGenericControl("p");
                titleParagraph.Attributes["class"] = "song-title";
                titleParagraph.InnerText = "Title: " + songTitle;

                HtmlGenericControl artistParagraph = new HtmlGenericControl("p");
                artistParagraph.Attributes["class"] = "song-artist";
                artistParagraph.InnerText = "Artist: " + songArtist;

                HtmlGenericControl playlistButton = new HtmlGenericControl("button");
                playlistButton.InnerText = "Add to playlist";
                playlistButton.Attributes["onclick"] = $"addToPlaylist({songId})";

                detailsDiv.Controls.Add(titleParagraph);
                detailsDiv.Controls.Add(artistParagraph);

                songDiv.Controls.Add(imgElement);
                songDiv.Controls.Add(detailsDiv);
                songDiv.Controls.Add(playlistButton);

                songsContainer.Controls.Add(songDiv);
            }
        }

        private DataTable GetSongsFromDatabase(string searchQuery)
        {
            DataTable songsData = new DataTable();
            using (SqlConnection conn = new SqlConnection("Server=DESKTOP-7QDMPI0\\SQLEXPRESS;Database=Jukebox;Trusted_Connection=True"))
            {
                SqlCommand cmd;
                if (string.IsNullOrEmpty(searchQuery))
                {
                    cmd = new SqlCommand("SELECT * FROM Songs", conn);
                }
                else
                {
                    cmd = new SqlCommand("SELECT * FROM Songs WHERE Title LIKE @SearchQuery OR Artist LIKE @SearchQuery OR Genre LIKE @SearchQuery", conn);
                    cmd.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(songsData);
            }
            return songsData;
        }

        private void LoadPlaylists()
        {
            if (Session["User_ID"] != null)
            {
                string userId = Session["User_ID"].ToString();
                DataTable playlistsData = GetPlaylistsForUser(userId);
                ddlPlaylists.Items.Clear();

                foreach (DataRow playlistRow in playlistsData.Rows)
                {
                    string playlistName = playlistRow["Name"].ToString();
                    DateTime dateCreated = (DateTime)playlistRow["Date_Created"];
                    string playlistDate = dateCreated.ToString("dd-MM-yyyy");
                    string playlistIMG = playlistRow["Cover_Image_URL"].ToString();


                    int playlistId = Convert.ToInt32(playlistRow["Playlist_ID"]);
                    ddlPlaylists.Items.Add(new ListItem(playlistName, playlistId.ToString()));
                    HtmlGenericControl playlistDiv = new HtmlGenericControl("div");
                    playlistDiv.Attributes["class"] = "playlist";
                    playlistDiv.Attributes["onclick"] = $"window.location.href='Playlist.aspx?playlistId={playlistId}';";

                    HtmlGenericControl imgElement = new HtmlGenericControl("img");
                    imgElement.Attributes["src"] = playlistIMG;
                    imgElement.Attributes["alt"] = playlistName;

                    HtmlGenericControl detailsDiv = new HtmlGenericControl("div");
                    detailsDiv.Attributes["class"] = "playlist-details";

                    HtmlGenericControl titleParagraph = new HtmlGenericControl("p");
                    titleParagraph.Attributes["class"] = "playlist-title";
                    titleParagraph.InnerText = playlistName;

                    HtmlGenericControl dateParagraph = new HtmlGenericControl("p");
                    dateParagraph.Attributes["class"] = "playlist-date";
                    dateParagraph.InnerText = "Created: " + playlistDate;

                    

                    detailsDiv.Controls.Add(titleParagraph);
                    detailsDiv.Controls.Add(dateParagraph);
                    

                    playlistDiv.Controls.Add(imgElement);
                    playlistDiv.Controls.Add(detailsDiv);

                    playlistsContainer.Controls.Add(playlistDiv);
                }
            }
        }

        private DataTable GetPlaylistsForUser(string userId)
        {
            DataTable playlistsData = new DataTable();
            using (SqlConnection conn = new SqlConnection("Server=DESKTOP-7QDMPI0\\SQLEXPRESS;Database=Jukebox;Trusted_Connection=True"))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Playlists WHERE User_ID = @User_ID", conn);
                cmd.Parameters.AddWithValue("@User_ID", userId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(playlistsData);
            }
            return playlistsData;
        }

        protected void btnAddToPlaylist_Click(object sender, EventArgs e)
        {
            string selectedPlaylistId = ddlPlaylists.SelectedValue;
            string newPlaylistName = txtNewPlaylist.Text;
            int songId = int.Parse(hiddenSongId.Value);
            int playlistId;

            using (SqlConnection conn = new SqlConnection("Server=DESKTOP-7QDMPI0\\SQLEXPRESS;Database=Jukebox;Trusted_Connection=True"))
            {
                conn.Open();
                SqlCommand cmd;

                if (!string.IsNullOrEmpty(newPlaylistName))
                {
                    // Create a new playlist
                    cmd = new SqlCommand("INSERT INTO Playlists (Name, User_ID, Date_Created,Cover_Image_URL) OUTPUT INSERTED.Playlist_ID VALUES (@Name, @User_ID, @Date_Created,@PlaylistIMG)", conn);
                    cmd.Parameters.AddWithValue("@Name", newPlaylistName);
                    cmd.Parameters.AddWithValue("@User_ID", Session["User_ID"].ToString());
                    cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PlaylistIMG", "https://i.scdn.co/image/ab67706c0000da8470d229cb865e8d81cdce0889");

                    // Execute the command and get the new playlist ID
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        playlistId = Convert.ToInt32(result);
                    }
                    else
                    {
                        // Handle the case where playlist creation failed
                        ClientScript.RegisterStartupScript(this.GetType(), "PlaylistCreationFailed", "alert('Failed to create the new playlist.');", true);
                        return;
                    }
                }
                else
                {
                    // Use existing playlist
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
            using (SqlConnection conn = new SqlConnection("Server=DESKTOP-7QDMPI0\\SQLEXPRESS;Database=Jukebox;Trusted_Connection=True"))
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
