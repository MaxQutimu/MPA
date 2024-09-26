using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace JukeBox
{
    public partial class Playlists : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User_ID"] != null)
                {
                    if (Request.QueryString["playlistId"] != null)
                    {
                        int playlistId = Convert.ToInt32(Request.QueryString["playlistId"]);
                        hiddenPlaylistId.Value = playlistId.ToString();
                        LoadPlaylistSongs(playlistId);
                    }
                    else
                    {
                        int User_ID = Convert.ToInt32(Request.QueryString["User_ID"]);
                        LoadPlaylists();
                    }
                }
                else
                {
                    Response.Redirect("LoginSite.aspx");
                }
            }
        }

        private void LoadPlaylists()
        {
            if (Session["User_ID"] != null)
            {
                string userId = Session["User_ID"].ToString();
                DataTable playlistsData = GetPlaylistsForUser(userId);


                foreach (DataRow playlistRow in playlistsData.Rows)
                {
                    string playlistName = playlistRow["Name"].ToString();
                    DateTime dateCreated = (DateTime)playlistRow["Date_Created"];
                    string playlistDate = dateCreated.ToString("dd-MM-yyyy");
                    int playlistId = Convert.ToInt32(playlistRow["Playlist_ID"]);
                    string Is_Public = playlistRow["Is_Public"].ToString();
                    string playlistIMG = playlistRow["Cover_Image_URL"].ToString();


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
                    dateParagraph.InnerText = playlistDate;

                    HtmlGenericControl IsPublic = new HtmlGenericControl("p");
                    IsPublic.Attributes["class"] = "playlist-date";
                    IsPublic.InnerText = Is_Public == "1" ? "Public" : "Private";

                    detailsDiv.Controls.Add(titleParagraph);
                    detailsDiv.Controls.Add(dateParagraph);
                    detailsDiv.Controls.Add(IsPublic);

                    playlistDiv.Controls.Add(imgElement);
                    playlistDiv.Controls.Add(detailsDiv);

                    songsContainer.Controls.Add(playlistDiv);
                }
            }
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

        private void LoadPlaylistSongs(int playlistId)
        {
            DataTable songsData = GetSongsForPlaylist(playlistId);

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
                playlistButton.InnerText = "Remove from playlist";
                playlistButton.Attributes["onclick"] = $"RemoveFromPlaylist({songId})";

                detailsDiv.Controls.Add(titleParagraph);
                detailsDiv.Controls.Add(artistParagraph);

                songDiv.Controls.Add(imgElement);
                songDiv.Controls.Add(detailsDiv);
                songDiv.Controls.Add(playlistButton);

                playlistsContainer.Controls.Add(songDiv);
                songsContainer.Controls.Add(songDiv);
            }
        }

        private DataTable GetSongsForPlaylist(int playlistId)
        {
            DataTable songsData = new DataTable();
            using (SqlConnection conn = new SqlConnection("Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True"))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT s.Song_ID, s.Title, s.Artist, s.Url, s.PhotoUrl
                    FROM Songs s
                    INNER JOIN Playlist_Songs ps ON s.Song_ID = ps.Song_ID
                    WHERE ps.Playlist_ID = @Playlist_ID", conn);
                cmd.Parameters.AddWithValue("@Playlist_ID", playlistId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(songsData);
            }
            return songsData;
        }

        protected void btnRemoveFromPlaylist_Click(object sender, EventArgs e)
        {
            long songId;
            long playlistId;

            // Adding debug statements
            System.Diagnostics.Debug.WriteLine("hiddenSongId Value: " + hiddenSongId.Value);
            System.Diagnostics.Debug.WriteLine("hiddenPlaylistId Value: " + hiddenPlaylistId.Value);

            if (long.TryParse(hiddenSongId.Value, out songId) && long.TryParse(hiddenPlaylistId.Value, out playlistId))
            {
                using (SqlConnection conn = new SqlConnection("Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Playlist_Songs WHERE Song_ID = @Song_ID AND Playlist_ID = @Playlist_ID", conn);
                    cmd.Parameters.AddWithValue("@Song_ID", songId);
                    cmd.Parameters.AddWithValue("@Playlist_ID", playlistId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Debugging rows affected
                    System.Diagnostics.Debug.WriteLine("Rows affected: " + rowsAffected);
                }

                // Refresh the page to update the playlist songs
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                // Handle the error: invalid songId or playlistId
                System.Diagnostics.Debug.WriteLine("Error: Invalid songId or playlistId");
            }
        }
    }
}
