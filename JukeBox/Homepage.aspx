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
                <div id="playlistsContainer" class="playlists-container" runat="server">
                    <!-- Playlists will be dynamically added here -->
                </div>
            </div>
            <div class="content">
                <h3>Przygotowane dla Ciebie</h3>
                <div id="songsContainer" class="songs-container" runat="server">
                    <!-- Songs will be dynamically added here -->
                </div>
            </div>
        </div>
        <!-- The Modal -->
        <div id="playlistModal" class="modal">
            <div class="modal-content">
                <span class="close">&times;</span>
                <h3>Add to Playlist</h3>
                <p>Select a playlist or create a new one:</p>
                <asp:DropDownList ID="ddlPlaylists" runat="server"></asp:DropDownList>
                <asp:TextBox ID="txtNewPlaylist" runat="server" Placeholder="New Playlist Name"></asp:TextBox>
                <asp:Button ID="btnAddToPlaylist" runat="server" Text="Add" OnClick="btnAddToPlaylist_Click" />
            </div>
        </div>
        <asp:HiddenField ID="hiddenSongId" runat="server" />
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

        function addToPlaylist(songId) {
            event.preventDefault();
            document.getElementById("<%= hiddenSongId.ClientID %>").value = songId;
            openModal();
        }

        function debounce(func, delay) {
            let debounceTimer;
            return function () {
                const context = this;
                const args = arguments;
                clearTimeout(debounceTimer);
                debounceTimer = setTimeout(() => func.apply(context, args), delay);
            };
        }

        function searchSongs() {
            var query = document.getElementById('searchBar').value;
            window.location.href = 'Homepage.aspx?search=' + query;
        }

        const debounceSearch = debounce(searchSongs, 5000);

        function handleEnter(event) {
            if (event.key === "Enter") {
                event.preventDefault();
                searchSongs();
            }
        }
    </script>
</body>
</html>

