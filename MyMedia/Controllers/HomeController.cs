using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using MyMedia.Data;
using MyMedia.Models;
using MyMedia.Models.Home;
using MyMedia.Models.Profiel;

namespace MyMedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMyMediaService _mediaService;
        //private readonly SignInManager<Profiel> _signInManager;
        private readonly UserManager<Profiel> _userManager;

        private readonly IUserStore<Profiel> _userStore;
        private Profiel? _currentProfiel;

        public HomeController(IMyMediaService mediaService, IUserStore<Profiel> userStore,UserManager<Profiel> userManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _mediaService = mediaService;
        }

        [Route("")]
        public IActionResult Index()
        {
            // var isSignedIn = this._signInManager.IsSignedIn(HttpContext.User);
            // var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            // if (isSignedIn)
            // {
            //     var profiel = _mediaService.GetAllProfielen().FirstOrDefault(p => p.Id == currentUserId);
            //     if (profiel == null)
            //     {
            //         var newProfiel = new Profiel
            //         {
            //             Id = currentUserId,
            //         };
            //         _mediaService.InsertProfiel(newProfiel);
            //         _mediaService.SaveChanges();
            //     }
            // 
            //     _currentProfiel = _mediaService.GetAllProfielen().FirstOrDefault(p => p.Id == currentUserId);
            // 
            // }
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
                IsSignedIn = false,//isSignedIn,
                Profiel = null//_currentProfiel
            };

            return View(vm);
        }
        [Authorize]
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
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // protection for other websites using this post endpoints
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                
                if (user == null)
                {
                    user = new Profiel
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                }
                return View("Success");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user!=null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var identity = new ClaimsIdentity("Cookies");
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                    await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Invalid Username or Password");
            }
            return View();
        }


    }
}
