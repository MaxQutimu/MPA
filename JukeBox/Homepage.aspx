<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="JukeBox.Homepage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="Style.css" />
    <title>Homepage</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="sidebar">
                <ul>
                    <li><a href="Homepage.aspx">Home</a></li>
                    <li>
                        <input class="searchBar" type="text" id="searchBar" placeholder="Search for songs..." onkeyup="debounceSearch()" onkeypress="handleEnter(event)" />
                    </li>
                    <li><a href="playlist.aspx">Playlists</a></li>
                </ul>
                <div id="playlistsContainer" class="playlists-container">
                    <asp:Repeater ID="PlaylistsRepeater" runat="server">
                        <ItemTemplate>
                            <div class="playlist" onclick="location.href='Playlist.aspx?playlistId=<%# Eval("Playlist_ID") %>'">
                                <img src="<%# Eval("Cover_Image_URL") %>" alt="<%# Eval("Name") %>" />
                                <div class="playlist-details">
                                    <p class="playlist-title"><%# Eval("Name") %></p>
                                    <p class="playlist-date">Created: <%# Eval("Date_Created", "{0:dd-MM-yyyy}") %></p>

                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div class="content">
                <h3>Prepared for you</h3>
                <div id="songsContainer" class="songs-container">
                    <asp:Repeater ID="SongsRepeater" runat="server">
                        <ItemTemplate>
                            <div class="song">
                                <img src="<%# Eval("PhotoUrl") %>" alt="<%# Eval("Title") %>" />
                                <div class="song-details">
                                    <p class="song-title">Title: <%# Eval("Title") %></p>
                                    <p class="song-artist">Artist: <%# Eval("Artist") %></p>
                                    <button type="button" onclick="addToPlaylist(<%# Eval("Song_ID") %>)">Add to playlist</button>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>

        <!-- Modal -->
        <div id="playlistModal" class="modal">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h3>Add to Playlist</h3>
                <p>Select a playlist or create a new one:</p>
                <asp:DropDownList ID="ddlPlaylists" runat="server"></asp:DropDownList>
                <asp:TextBox ID="txtNewPlaylist" runat="server" Placeholder="New Playlist Name"></asp:TextBox>
                <asp:Button ID="btnAddToPlaylist" runat="server" Text="Add" OnClick="btnAddToPlaylist_Click" />
                <asp:HiddenField ID="hiddenSongId" runat="server" />
            </div>
        </div>

        <script> function addToPlaylist(songId) {
                document.getElementById('hiddenSongId').value = songId;
                document.getElementById('playlistModal').style.display = 'block';
            }

            function closeModal() {
                document.getElementById('playlistModal').style.display = 'none';
            }

            function handleEnter(event) {
                if (event.key === 'Enter') {
                    searchSongs();
                }
            }

            function debounceSearch() {
                let timer;
                clearTimeout(timer);
                timer = setTimeout(searchSongs, 500);
            }

            function searchSongs() {
                let searchTerm = document.getElementById("searchBar").value;
                if (searchTerm) {
                    window.location.href = `Homepage.aspx?search=${searchTerm}`;
                } else {
                    window.location.href = "Homepage.aspx";
                }
            }
        </script>
    </form>
</body>
</html>
