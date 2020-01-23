using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using MyMedia.Data;
using MyMedia.Models.Music;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyMedia.Controllers
{

    public class SongsController : Controller
    {
        private readonly IMyMediaService _mediaService;
        private readonly IUserStore<MediaUser> _userStore;
        private readonly IUserClaimsPrincipalFactory<MediaUser> _claimsPrincipalFactory;
        private readonly SignInManager<MediaUser> _signInManager;
        private readonly UserManager<MediaUser> _userManager;


        private MediaUser? _currentMediaUser;
        public SongsController(IMyMediaService mediaService,
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
        public IActionResult Index()
        {
            // create list songs view
            List<MusicListViewModel> Songs = new List<MusicListViewModel>();
            IEnumerable<Music>? songListFromDb = _mediaService.GetAllMedia().OfType<Music>();
            if (songListFromDb.Count() > 0)
            {
                foreach (var song in songListFromDb)
                {
                    Songs.Add(new MusicListViewModel() { Id=song.Id,ZangersName = song.ZangersName, Titel = song.Titel,Photo=song.Photo });
                }
            }

            return View(Songs);

        }
        [Authorize]
        public IActionResult Details(int id)
        {
           
           var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
          
           _currentMediaUser = _mediaService.GetAllMediaUsers().First(p => p.Id == currentUserId);
        
            Music selectedMusic = _mediaService.GetAllMedia().OfType<Music>().Where(muz => muz.Id == id).FirstOrDefault();
            List<Rating> UserRatingList=new List<Rating>();
            bool isAlreadyRated = false;
            var playlists = new List<PlayList>();
            if (_currentMediaUser != null)
            {
                isAlreadyRated = _mediaService.GetAllRatings().Where(movie => movie.Media.Titel == selectedMusic.Titel).Where(user => user.MediaUser.Id == _currentMediaUser.Id).Any();
                playlists = _currentMediaUser.Playlists.ToList();
            }

            UserRatingList = _mediaService.GetAllRatings().Where(music => music.Media.Id == selectedMusic.Id).ToList();
            var detailViewModel = new MusicDetailViewModel()
            {
                MediaId = selectedMusic.Id,
                Lied = selectedMusic.Lied,
                Titel = selectedMusic.Titel,
                ZangersName = selectedMusic.ZangersName,
                Photo = selectedMusic.Photo,
                IsRated = isAlreadyRated,
                PlayLists = playlists,
                IsSignedIn = true
            };
            if (selectedMusic.Ratings.Count() > 0)
            {
                detailViewModel.AveragePoints = selectedMusic.Ratings.Where(med => med.Media.Id == selectedMusic.Id) != null ? selectedMusic.Ratings.Where(med => med.Media.Id == selectedMusic.Id).Average(r => r.Points) : 0;
            }
            if (_currentMediaUser != null)
            {
                detailViewModel.PlayLists = _currentMediaUser.Playlists.ToList();
            }
            return View(detailViewModel);

        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            Music selectedMusic = _mediaService.GetAllMedia().OfType<Music>().Where(x => x.Id == id).FirstOrDefault();
            var editViewModel = new MusicEditViewModel()
            {
                Id = selectedMusic.Id,
                Lied = selectedMusic.Lied,
                Titel = selectedMusic.Titel,
                ZangersName = selectedMusic.ZangersName,
                Photo = selectedMusic.Photo
            };


            return View(editViewModel);

        }
        [Authorize]
        [HttpPost]
        public IActionResult Edit(MusicEditViewModel model)
        {
            var music = _mediaService.GetAllMedia().OfType<Music>().FirstOrDefault(mus => mus.Id == model.Id);
            music.Lied = model.Lied;
            music.Titel = model.Titel;
            music.Photo = model.Photo;
            music.ZangersName = model.ZangersName;
            _mediaService.SaveChanges();
            return RedirectToAction("Details", new { music.Id });

        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            Music selectedMusic = _mediaService.GetAllMedia().OfType<Music>().FirstOrDefault(x => x.Id == id);
            MusicDeleteViewModel model = new MusicDeleteViewModel()
            {
                Id= selectedMusic.Id,
                Name = selectedMusic.Titel
              
            };
            return View(model);

        }
        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            // var musicToDelete = _mediaService.GetAllMedia().OfType<Music>().First(x => x.Id == id);
            _mediaService.DeleteMediaById(id);
            _mediaService.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Create()
        {
            MusicCreateViewModel Muz = new MusicCreateViewModel();

            return View(Muz);
        }
        [Authorize]

        [HttpPost]
        public IActionResult Create(MusicCreateViewModel model)
        {
            if (!TryValidateModel(model))
            {
                return View(model);
            }
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            Music newMusic = new Music()
            {
                Titel = model.Titel,
                ZangersName = model.ZangersName,
                Lied = model.Lied,
                MediaUser = _currentMediaUser,
                MediaUserId = currentUserId
            };
            _mediaService.InsertMedia(newMusic);
            _mediaService.SaveChanges();

            Music muzFrmDb = _mediaService.GetAllMedia().OfType<Music>().FirstOrDefault(x => x.Id == newMusic.Id);
            if (model.Photo != null)
            {
                using var memoryStream = new MemoryStream();
                model.Photo.CopyTo(memoryStream);
                muzFrmDb.Photo = memoryStream.ToArray();
            }
        
            _mediaService.SaveChanges();

            return RedirectToAction("Details", new { muzFrmDb.Id });

        }
        [Authorize]
        public IActionResult RateMusic(MusicRateViewModel model)
        {
            var music = _mediaService.GetAllMedia().OfType<Music>().First(muz => muz.Id == model.MediaId);
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);


            _currentMediaUser = _mediaService.GetAllMediaUsers().First(p => p.Id == currentUserId);


            var newRating = new Rating()
            {
                Media = music,
                CreationDate = DateTime.Now,
                Points = model.Points,
                MediaUser = _currentMediaUser

            };

            _mediaService.InsertRating(newRating);
            _mediaService.SaveChanges();

            return RedirectToAction("Details", new { music.Id });
        }
    }
}
