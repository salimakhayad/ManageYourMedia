using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyMedia.Models.Media;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using Microsoft.AspNetCore.Authorization;

namespace MyMedia.Controllers
{
    public class MediaController : Controller
    {

        private readonly IMyMediaService _mediaService;
        private readonly IUserStore<Profiel> _userStore;
        private readonly IUserClaimsPrincipalFactory<Profiel> _claimsPrincipalFactory;
        private readonly SignInManager<Profiel> _signInManager;
        private readonly UserManager<Profiel> _userManager;
        private Profiel? _currentProfiel;
        public MediaController(IMyMediaService mediaService,
            SignInManager<Profiel> signInManager,
            IUserClaimsPrincipalFactory<Profiel> claimsPrincipalFactory,
            IUserStore<Profiel> userStore,
            UserManager<Profiel> userManager
            )
        {
            this._userManager = userManager;
            this._claimsPrincipalFactory = claimsPrincipalFactory;
            this._userStore = userStore;
            this._mediaService = mediaService;
            this._signInManager = signInManager;
        }
        [Authorize]
        public IActionResult Index()
        {
          
            // selects non approved media 
            var MediaList = _mediaService.GetAllMedia().Where(m => m.IsPubliek != true);

            // maps to listviewmodel 
            var mediaListViewModel = new List<MediaListViewModel>();
            foreach (var med in MediaList)
            {
                mediaListViewModel.Add(
                    new MediaListViewModel()
                    {
                        Foto = med.Foto,
                        Titel = med.Titel,
                        IsChecked = false
                    });
            }
            var MediaOverviewVM = new MediaOverviewViewModel()
            {
                NietPubliekeMediaLijst = mediaListViewModel
            };


            return View(MediaOverviewVM);
            
        }

        [HttpPost]
        public IActionResult ApproveMedia(ICollection<MediaListViewModel> NietPubliekeMediaLijst)
        {
            var unapprovedMedia =_mediaService.GetAllMedia().Where(m => m.IsPubliek != true);
            foreach (var media in unapprovedMedia)
            {
                media.IsPubliek = true;
            }
            _mediaService.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult Share(int Id)
        {
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
        
            _currentProfiel = _mediaService.GetAllProfielen().First(p => p.Id == currentUserId);

            var playlist = _mediaService.GetAllPlaylists().FirstOrDefault(playlist => playlist.Id == Id);
           
            var listUsers = _mediaService.GetAllProfielen();
             
            foreach(var user in listUsers)
            {
                if (user != _currentProfiel)
                {
                    user.Playlists.Add(
                        new PlayList()
                        {
                            MediaList = playlist.MediaList,
                            Name = playlist.Name,
                            Profiel = _currentProfiel
                        });
                }

            }
            
            // share with all
            _mediaService.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
      
    }
}
