using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyMedia.Models.Media;
using MyMedia.Core.MediaClasses;

namespace MyMedia.Controllers
{
    public class MediaController : Controller
    {
        
        private readonly IMyMediaService _mediaService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private Profiel? _currentProfiel;
        public MediaController(IMyMediaService mediaService, SignInManager<IdentityUser> signInManager)
        {
            _mediaService = mediaService;
            _signInManager = signInManager;
        }
        
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

                _currentProfiel = _mediaService.GetAllProfielen().First(p => p.UserId == currentUserId);

            }
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
            return View();
        }
    }
}
