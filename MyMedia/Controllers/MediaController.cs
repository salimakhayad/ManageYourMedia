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
        private readonly IUserStore<MediaUser> _userStore;
        private readonly IUserClaimsPrincipalFactory<MediaUser> _claimsPrincipalFactory;
        private readonly SignInManager<MediaUser> _signInManager;
        private readonly UserManager<MediaUser> _userManager;
        private MediaUser? _currentMediaUser;
        public MediaController(IMyMediaService mediaService,
            SignInManager<MediaUser> signInManager,
            IUserClaimsPrincipalFactory<MediaUser> claimsPrincipalFactory,
            IUserStore<MediaUser> userStore,
            UserManager<MediaUser> userManager
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
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            // selects non approved media 
            var MediaList = _mediaService.GetAllMedia().Where(m => m.IsPublic != true);

            // maps to listviewmodel 
            var mediaListViewModel = new List<MediaListViewModel>();
            foreach (var med in MediaList)
            {
                var model =
                    new MediaListViewModel()
                    {
                        Id = med.Id,
                        Photo = med.Photo,
                        Titel = med.Titel,
                        IsChecked = false,
                        AddedByMediaUserName = med.MediaUser.UserName
                    };

                mediaListViewModel.Add(model);

            }
            var MediaOverviewVM = new MediaOverviewViewModel()
            {
                UnApprovedMediaList = mediaListViewModel
            };


            return View(MediaOverviewVM);
            
        }

        [HttpPost]
        public IActionResult ApproveMedia(MediaOverviewViewModel Model)
        {
            if (Model.UnApprovedMediaList != null)
            {
                foreach (var media in Model.UnApprovedMediaList)
                {
                    if (media.IsChecked == true)
                    {
                        var mediaFromDb = _mediaService.GetAllMedia().Where(m => m.Id == media.Id).FirstOrDefault();
                        mediaFromDb.IsPublic = true;
                        _mediaService.SaveChanges();

                    }
                }
            }
           
           

            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult Share(int Id)
        {
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
        
            _currentMediaUser = _mediaService.GetAllMediaUsers().First(p => p.Id == currentUserId);

            var playlist = _mediaService.GetAllPlaylists().FirstOrDefault(playlist => playlist.Id == Id);
           
            var listUsers = _mediaService.GetAllMediaUsers();
             
            foreach(var user in listUsers)
            {
                if (user != _currentMediaUser)
                {
                    user.Playlists.Add(
                        new PlayList()
                        {
                            MediaList = playlist.MediaList,
                            Name = playlist.Name,
                            MediaUser = _currentMediaUser
                        });
                }

            }
            
            // share with all
            _mediaService.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
      
    }
}
