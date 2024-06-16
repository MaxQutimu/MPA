
using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;

public partial class AddToPlaylistHandler : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            // Read the JSON data sent from the client
            string jsonData;
            using (var reader = new StreamReader(Request.InputStream))
            {
                jsonData = reader.ReadToEnd();
            }

            // Deserialize the JSON data into a C# object
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData);

            // Extract the data from the object
            string songId = data.songId;
            string selectedPlaylistId = data.selectedPlaylistId;
            string newPlaylistName = data.newPlaylistName;

            // Perform database operations or any other necessary actions based on the received data
            // Example: Inserting data into the database
            InsertIntoPlaylist(songId, selectedPlaylistId, newPlaylistName);

            // Send a response back to the client
            Response.Write("Data received successfully");
            Response.End();
        }
        else
        {
            // Handle other types of requests (if necessary)
            Response.StatusCode = 405; // Method Not Allowed
            Response.End();
        }
    }

    // Example method to insert data into the database
    private void InsertIntoPlaylist(string songId, string selectedPlaylistId, string newPlaylistName)
    {
        using (SqlConnection conn = new SqlConnection("Server=DESKTOP-7QDMPI0\\SQLEXPRESS;Database=Jukebox;Trusted_Connection=True"))
        {
            conn.Open();
            SqlCommand cmd;

            if (!string.IsNullOrEmpty(newPlaylistName))
            {
                cmd = new SqlCommand("INSERT INTO Playlists (Name, User_ID) OUTPUT INSERTED.Playlist_ID VALUES (@Name, @User_ID)", conn);
                cmd.Parameters.AddWithValue("@Name", newPlaylistName);
                // Assuming you have a user ID available here
                cmd.Parameters.AddWithValue("@User_ID", HttpContext.Current.Session["User_ID"].ToString()); // Replace YourUserID with the actual user ID
                selectedPlaylistId = cmd.ExecuteScalar().ToString();
            }

            cmd = new SqlCommand("INSERT INTO Playlist_Songs (Playlist_ID, Song_ID) VALUES (@Playlist_ID, @Song_ID)", conn);
            cmd.Parameters.AddWithValue("@Playlist_ID", selectedPlaylistId);
            cmd.Parameters.AddWithValue("@Song_ID", songId);
            cmd.ExecuteNonQuery();
        }
    }
}
