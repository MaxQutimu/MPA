<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Playlist.aspx.cs" Inherits="JukeBox.Playlists" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="Style.css" />
    <title>Playlist Songs</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="sidebar">
                <h3>Menu</h3>
                <ul>
                    <li><a href="Homepage.aspx">Home</a></li>
                    <li><a href="Playlist.aspx">Playlists</a></li>
                </ul>
                <div id="playlistsContainer" class="playlists-container" runat="server">
                 <!-- Playlists will be dynamically added here -->
                </div>  
            </div>
            <div class="content">
                <h3 id="playlistName" runat="server"></h3>
                <div id="songsContainer" class="songs-container" runat="server">
                    <!-- Songs will be dynamically added here -->
                </div>
            </div>
        </div>
        <div id="playlistModal" class="modal">
            <div class="modal-content">
                <span class="close">&times;</span>
                <h3>Remove from Playlist</h3>
                <p>Confirm that you want to remove this song from the playlist:</p>
                <asp:Button ID="btnRemoveFromPlaylist" runat="server" Text="Confirm" OnClick="btnRemoveFromPlaylist_Click" />
            </div>
        </div>
        <asp:HiddenField ID="hiddenSongId" runat="server" />
        <asp:HiddenField ID="hiddenPlaylistId" runat="server" />
    </form>
    <script>
        var modal = document.getElementById("playlistModal");
        var span = document.getElementsByClassName("close")[0];

        function openModal() {
            modal.style.display = "block";
        }

        span.onclick = function () {
            modal.style.display = "none";
        }

        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }

        function RemoveFromPlaylist(songId) {
            event.preventDefault();
            document.getElementById("<%= hiddenSongId.ClientID %>").value = songId;
            openModal();
        }
    </script>
</body>
</html>
