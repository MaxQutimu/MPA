using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string CoverImageUrl { get; set; }
    }
    public class HomePageViewModel
    {
        public List<Song> Songs { get; set; }
        public List<Playlist> Playlists { get; set; }
    }
}
