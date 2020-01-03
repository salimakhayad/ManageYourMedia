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
        private readonly IUserStore<Profiel> _userStore;
        private readonly IUserClaimsPrincipalFactory<Profiel> _claimsPrincipalFactory;
        private readonly SignInManager<Profiel> _signInManager;
        private readonly UserManager<Profiel> _userManager;

        private Profiel? _currentProfiel;

        public HomeController(IMyMediaService mediaService, IUserStore<Profiel> userStore,UserManager<Profiel> userManager,IUserClaimsPrincipalFactory<Profiel> claimsPrincipalFactory, SignInManager<Profiel> signInManager)
        {
            this._userManager = userManager;
            this._claimsPrincipalFactory = claimsPrincipalFactory;
            this._userStore = userStore;
            this._mediaService = mediaService;
            this._signInManager = signInManager;
        }

        [Route("")]
        [Authorize]
        public IActionResult Index()
        {

            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            var user = _mediaService.GetAllProfielen().Where(prof => prof.Id == currentUserId).FirstOrDefault();

            var topMovies = _mediaService.GetAllMedia().OfType<Movie>().Take(10).Where(m=>m.IsPubliek==true) .OrderBy(r => r.Ratings);
            var topSeries = _mediaService.GetAllSeries().Take(10).Where(m => m.IsPubliek == true);
            var topMusic = _mediaService.GetAllMedia().OfType<Muziek>().Take(10).Where(m => m.IsPubliek == true).OrderBy(r => r.Ratings);
            var topPodcasts = _mediaService.GetAllPodcasts().Take(10).Where(m => m.IsPubliek == true).OrderBy(r => r.Ratings);
            var topPlaylists = _mediaService.GetAllPlaylists().Take(10).Where(m => m.Profiel==_currentProfiel);

            var vm = new HomeOverviewViewModel
            {
                Movies = topMovies,
                Series = topSeries,
                Musics = topMusic,
                Podcasts = topPodcasts,
                PlayLists = topPlaylists,
                IsSignedIn = true,//isSignedIn,
                Profiel = user//_currentProfiel
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
                        UserName = model.UserName,
                        FavorieteKleur = "Dark Orange"
                    };
                    var identityResult = await _userManager.CreateAsync(user,model.Password);
                   if (identityResult.Succeeded)
                    {
                        return View("Success");
                    }
                    return View();
                }
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
                   var principal = await _claimsPrincipalFactory.CreateAsync(user);
               
                   await HttpContext.SignInAsync("Identity.Application", principal);
                    _currentProfiel = _mediaService.GetAllProfielen().FirstOrDefault(usr => usr.Id == user.Id);
                   return RedirectToAction("Index");
               }
                ModelState.AddModelError("", "Invalid Username or Password");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
          await this._signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


    }
}
