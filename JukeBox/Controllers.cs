using System;
using System.Web.Mvc;
using JukeBox.Data;
using JukeBox.Models;

namespace JukeBox.Controllers
{
    public class HomeController : Controller
    {
        private readonly JukeBoxRepository _repository;

        // Injecting repository through the constructor
        public HomeController(JukeBoxRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index(string search)
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = Session["User_ID"].ToString();
            var songs = _repository.GetSongs(search);
            var playlists = _repository.GetPlaylists(userId);

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
            if (Session["User_ID"] == null)
            {
                Response.Redirect("LoginSite.aspx");
            }

            try
            {
                var userId = Session["User_ID"].ToString();
                playlistId = _repository.AddSongToPlaylist(songId, playlistId, newPlaylistName, userId);
                if (playlistId > 0)
                {
                    TempData["SuccessMessage"] = "Song added to the playlist successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
