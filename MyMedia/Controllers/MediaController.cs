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

            return View();
        }
    }
}
