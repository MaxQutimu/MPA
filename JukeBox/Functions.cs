using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace JukeBox
{
    public class Functions
    {
    }
    public class UserAuthentication
    {
        public int? Login_In(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection("Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True"))
            {
                SqlCommand cmd = new SqlCommand("SELECT User_ID FROM Users WHERE username=@username AND password=@password", conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", HashPassword(password)); // Hashing the password before comparing

                conn.Open(); // Open the connection before executing the command
                object result = cmd.ExecuteScalar();
                conn.Close(); // Close the connection after executing the command

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return null;
                }
            }
        }
        private string HashPassword(string password)
            {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
    public class UserRegistration
    {
        public bool UserExistenceCheck(string username, string email)
        {
            using (SqlConnection conn = new SqlConnection("Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True"))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE username=@username or email=@email", conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@email", email);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                conn.Open(); // Open the connection before filling the dataset
                da.Fill(ds, "Users");
                conn.Close(); // Close the connection after filling the dataset

                return ds.Tables["Users"].Rows.Count != 0;

            }

        }
        public void RegisterUser(string username, string password, string email)
        {
            using (SqlConnection conn = new SqlConnection("Server=LAPTOP-JGV34OHE;Database=Jukebox;Trusted_Connection=True"))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Users (username,password,email) VALUES (@username,@password,@email)", conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", HashPassword(password)); // Hashing the password before storing
                cmd.Parameters.AddWithValue("@email", email);

                conn.Open(); // Open the connection
                cmd.ExecuteNonQuery(); // Execute the query
                conn.Close(); // Close the connection
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
    public class PlaylistManager
    {
        private string connectionString;

        public PlaylistManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddSongsToPlaylist(int playlistId, List<int> songIds)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (int songId in songIds)
                {
                    string query = "INSERT INTO PlaylistSongs (PlaylistID, SongID) VALUES (@PlaylistID, @SongID)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PlaylistID", playlistId);
                        command.Parameters.AddWithValue("@SongID", songId);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
