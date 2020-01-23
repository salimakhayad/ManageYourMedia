using Microsoft.AspNetCore.Authorization;
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
    
    public class PlaylistsController : Controller
    {
        private readonly IMyMediaService _mediaService;
        private readonly IUserStore<MediaUser> _userStore;
        private readonly IUserClaimsPrincipalFactory<MediaUser> _claimsPrincipalFactory;
        private readonly SignInManager<MediaUser> _signInManager;
        private readonly UserManager<MediaUser> _userManager;
        private MediaUser? _currentMediaUser;
        public PlaylistsController(IMyMediaService mediaService,
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
           var isSignedIn = this._signInManager.IsSignedIn(HttpContext.User);
           var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
           _currentMediaUser = _mediaService.GetAllMediaUsers().FirstOrDefault(p => p.Id == currentUserId);
           
        
            List<PlayListIndexViewModel> model = new List<PlayListIndexViewModel>();
            var playlistsFromDb = _mediaService.GetAllPlaylists().ToList();

            foreach (var playlist in playlistsFromDb)
            {
                model.Add(new PlayListIndexViewModel()
                {
                    Id = playlist.Id,
                    Name = playlist.Name,
                    IsSignedIn = true,  // edit
                    UserName = _currentMediaUser.UserName
                });
            }

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var playlist = _mediaService.GetAllPlaylists().First(playlst => playlst.Id == id);
            PlayListDetailViewModel model = new PlayListDetailViewModel()
            {
                Name = playlist.Name,
                MediaList = playlist.MediaList.ToList(),
                UserName = playlist.MediaUser.UserName,
            };
            return View(model);
        }
        [Authorize]
        public IActionResult Create()
        {
            PlayListCreateViewModel model = new PlayListCreateViewModel();
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            _currentMediaUser = _mediaService.GetAllMediaUsers().First(p => p.Id == currentUserId);
            model.UserName = _currentMediaUser.UserName;
            return View(model);

        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(PlayListCreateViewModel model)
        {
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            _currentMediaUser = _mediaService.GetAllMediaUsers().First(p => p.Id == currentUserId);
            var newPlayList = new PlayList()
            {
                Name = model.Name,
                MediaList = new List<Media>(),
                MediaUser =  _currentMediaUser,
                MediaUserId = currentUserId
            };
            _currentMediaUser.Playlists.Add(newPlayList);
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
