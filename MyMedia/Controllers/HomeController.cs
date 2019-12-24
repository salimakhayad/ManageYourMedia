using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using MyMedia.Data;
using MyMedia.Models;
using MyMedia.Models.Home;

namespace MyMedia.Controllers
{
    public class HomeController : Controller
    {
        // private readonly MediaDbContext _context;
        private readonly IMyMediaService _mediaService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private Profiel? _currentProfiel;

        public HomeController(IMyMediaService mediaService, SignInManager<IdentityUser> signInManager)
        {
            // _context = context;
            _signInManager = signInManager;
            _mediaService = mediaService;
        }

        [Route("")]
        public IActionResult Index()
        {
            var isSignedIn = this._signInManager.IsSignedIn(HttpContext.User);
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            if (isSignedIn)
            {
                var profiel = _mediaService.GetAllProfielen().FirstOrDefault(p => p.UserId == currentUserId);
                if (profiel == null)
                {
                    var newProfiel = new Profiel
                    {
                        UserId = currentUserId,
                    };
                    _mediaService.InsertProfiel(newProfiel);
                    _mediaService.SaveChanges();
                }

                _currentProfiel = _mediaService.GetAllProfielen().FirstOrDefault(p => p.UserId == currentUserId);

            }
            var topMovies = _mediaService.GetAllMedia().OfType<Movie>().Take(10);//.Where(m=>m.IsPubliek==true) .OrderBy(r => r.Rating.Points);
            var topSeries = _mediaService.GetAllSeries().Take(10);//.Where(m=>m.IsPubliek==true);
            var topMusic = _mediaService.GetAllMedia().OfType<Muziek>().Take(10);//.Where(m=>m.IsPubliek==true);
            var topPodcasts = _mediaService.GetAllPodcasts().Take(10);//.Where(p=>p.IsPubliek==true);
            var topPlaylists = _mediaService.GetAllPlaylists().Take(10);//.Where(pl=>pl.IsPubliek==true);

            var vm = new HomeOverviewViewModel
            {
                Movies = topMovies,
                Series = topSeries,
                Musics = topMusic,
                Podcasts = topPodcasts,
                PlayLists = topPlaylists,
                IsSignedIn = isSignedIn,
                Profiel = _currentProfiel
            };

            return View(vm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
