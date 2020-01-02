﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using MyMedia.Data;
using MyMedia.Models.Playlist;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMedia.Controllers
{
    
    public class PlayListController : Controller
    {
        private readonly IMyMediaService _mediaService;
        private readonly IUserStore<Profiel> _userStore;
        private readonly IUserClaimsPrincipalFactory<Profiel> _claimsPrincipalFactory;
        private readonly SignInManager<Profiel> _signInManager;
        private readonly UserManager<Profiel> _userManager;
        private Profiel? _currentProfiel;
        public PlayListController(IMyMediaService mediaService,
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
           var isSignedIn = this._signInManager.IsSignedIn(HttpContext.User);
           var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
           _currentProfiel = _mediaService.GetAllProfielen().FirstOrDefault(p => p.Id == currentUserId);
           
        
            List<PlayListIndexViewModel> model = new List<PlayListIndexViewModel>();
            var playlistsFromDb = _mediaService.GetAllPlaylists().ToList();

            foreach (var playlist in playlistsFromDb)
            {
                model.Add(new PlayListIndexViewModel()
                {
                    Id = playlist.Id,
                    Naam = playlist.Name,
                    IsSignedIn = true,  // edit
                    UserNaam = _currentProfiel.UserName
                });
            }

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var playlist = _mediaService.GetAllPlaylists().First(playlst => playlst.Id == id);
            PlayListDetailViewModel model = new PlayListDetailViewModel()
            {
                Naam = playlist.Name,
                MediaList = playlist.MediaList.ToList(),
                UserNaam = playlist.Profiel.UserName,
            };
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Create()
        {
            
            PlayListCreateViewModel model = new PlayListCreateViewModel();
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            _currentProfiel = _mediaService.GetAllProfielen().First(p => p.Id == currentUserId);
            model.UserName = _currentProfiel.UserName;
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(PlayListCreateViewModel model)
        {
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            _currentProfiel = _mediaService.GetAllProfielen().First(p => p.Id == currentUserId);
            var newPlayList = new PlayList()
            {
                Name = model.Naam,
                MediaList = new List<Media>(),
                Profiel =  _currentProfiel
            };
            _currentProfiel.Playlists.Add(newPlayList);
            _mediaService.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddToPlaylist(PlayListAddMediaViewModel playListAddMediaViewModel)
        {
            if (playListAddMediaViewModel.PlayListId == 0)
            {
                return View("Index");
            }
            var playList = _mediaService.GetAllPlaylists().First(pl => pl.Id == playListAddMediaViewModel.PlayListId);
            var media = _mediaService.GetAllMedia().First(m => m.Id == playListAddMediaViewModel.MediaId);
            playList.MediaList.Add(media);
            _mediaService.SaveChanges();

            return RedirectToAction("Details", new { id = playList.Id });

        }
    }

}
