using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using MyMedia.Data;
using MyMedia.Models.Muziek;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyMedia.Controllers
{

    public class MuziekController : Controller
    {
        private readonly IMyMediaService _mediaService;
        private readonly IUserStore<Profiel> _userStore;
        private readonly IUserClaimsPrincipalFactory<Profiel> _claimsPrincipalFactory;
        private readonly SignInManager<Profiel> _signInManager;
        private readonly UserManager<Profiel> _userManager;


        private Profiel? _currentProfiel;
        public MuziekController(IMyMediaService mediaService,
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
        public IActionResult Index()
        {
            // create list songs view
            List<MuziekListViewModel> Songs = new List<MuziekListViewModel>();
            IEnumerable<Muziek>? songListFromDb = _mediaService.GetAllMedia().OfType<Muziek>();
            if (songListFromDb.Count() > 0)
            {
                foreach (var song in songListFromDb)
                {
                    Songs.Add(new MuziekListViewModel() { Id=song.Id,ZangersNaam = song.ZangersNaam, Titel = song.Titel,Foto=song.Foto });
                }
            }

            return View(Songs);

        }
        public IActionResult Details(int id)
        {
           // var isSignedIn = this._signinManager.IsSignedIn(HttpContext.User);
           // var currentUserId = this._signinManager.UserManager.GetUserId(HttpContext.User);
           // if (isSignedIn)
           // {
           //     _currentProfiel = _mediaService.GetAllProfielen().First(p => p.Id == currentUserId);
           // }
            Muziek selectedMusic = _mediaService.GetAllMedia().OfType<Muziek>().Where(muz => muz.Id == id).FirstOrDefault();
            List<Rating> UserRatingList=new List<Rating>();
            // if (currentUserId != null)
            // {
            //     UserRatingList = _mediaService.GetAllRatings().Where(music => music.Media.Titel == selectedMusic.Titel).Where(user => user.Profiel == _currentProfiel).ToList();
            // }
            UserRatingList = _mediaService.GetAllRatings().Where(music => music.Media.Id == selectedMusic.Id).ToList();
            var detailViewModel = new MuziekDetailViewModel()
            {
                MediaId = selectedMusic.Id,
                Lied = selectedMusic.Lied,
                Titel = selectedMusic.Titel,
                ZangersNaam = selectedMusic.ZangersNaam,
                Foto = selectedMusic.Foto,
                IsRated = UserRatingList.Any(),
                PlayLists = new List<PlayList>(),
                IsSignedIn = false/*isSignedIn*/
            };
            if (selectedMusic.Ratings.Count() > 0)
            {
                detailViewModel.AveragePoints = selectedMusic.Ratings.Where(med => med.Media.Id == selectedMusic.Id) != null ? selectedMusic.Ratings.Where(med => med.Media.Id == selectedMusic.Id).Average(r => r.Points) : 0;
            }
            if (_currentProfiel != null)
            {
                detailViewModel.PlayLists = _currentProfiel.Playlists.ToList();
            }
            return View(detailViewModel);

        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            Muziek selectedMusic = _mediaService.GetAllMedia().OfType<Muziek>().Where(x => x.Id == id).FirstOrDefault();
            var detailViewModel = new MuziekEditViewModel()
            {
                Id = selectedMusic.Id,
                Lied = selectedMusic.Lied,
                Titel = selectedMusic.Titel,
                ZangersNaam = selectedMusic.ZangersNaam,
                Foto = selectedMusic.Foto
            };


            return View(detailViewModel);

        }
        [HttpPost]
        public IActionResult Edit(MuziekEditViewModel model)
        {
            var music = _mediaService.GetAllMedia().OfType<Muziek>().FirstOrDefault(mus => mus.Id == model.Id);
            music.Lied = model.Lied;
            music.Titel = model.Titel;
            music.Foto = model.Foto;
            music.ZangersNaam = model.ZangersNaam;
            _mediaService.SaveChanges();
            return RedirectToAction("Details", new { music.Id });

        }

        
        public IActionResult Delete(int id)
        {
            Muziek selectedMusic = _mediaService.GetAllMedia().OfType<Muziek>().FirstOrDefault(x => x.Id == id);
            MuziekDeleteViewModel model = new MuziekDeleteViewModel()
            {
                Id= selectedMusic.Id,
                Naam = selectedMusic.Titel
              
            };
            return View(model);

        }
        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            // var muziekToDelete = _mediaService.GetAllMedia().OfType<Muziek>().First(x => x.Id == id);
            _mediaService.DeleteMediaById(id);
            _mediaService.SaveChanges();
            return RedirectToAction("Index");
        }

       
        public IActionResult Create()
        {
            MuziekCreateViewModel Muz = new MuziekCreateViewModel();

            return View(Muz);
        }

        [HttpPost]
        public IActionResult Create(MuziekCreateViewModel model)
        {
            if (!TryValidateModel(model))
            {
                return View(model);
            }
            Muziek newMusic = new Muziek()
            {
                Titel = model.Titel,
                ZangersNaam = model.ZangersNaam,
                Lied = model.Lied
                
               
            };
            _mediaService.InsertMedia(newMusic);
            _mediaService.SaveChanges();
       
            Muziek muzFrmDb = _mediaService.GetAllMedia().OfType<Muziek>().FirstOrDefault(x => x.Id == newMusic.Id);
            if (model.Foto != null)
            {
                using var memoryStream = new MemoryStream();
                model.Foto.CopyTo(memoryStream);
                muzFrmDb.Foto = memoryStream.ToArray();
            }
        
            _mediaService.SaveChanges();

            return RedirectToAction("Details", new { muzFrmDb.Id });

        }
        public IActionResult RateMusic(MusicRateViewModel model)
        {
            var music = _mediaService.GetAllMedia().OfType<Muziek>().First(muz => muz.Id == model.MediaId);
          // var isSignedIn = this._signinManager.IsSignedIn(HttpContext.User);
          // var currentUserId = this._signinManager.UserManager.GetUserId(HttpContext.User);
          // if (isSignedIn)
          // {
          //     _currentProfiel = _mediaService.GetAllProfielen().First(p => p.Id == currentUserId);
          // }

            var newRating = new Rating()
            {
                Media = music,
                CreationDate = DateTime.Now,
                Points = model.Points,
                Profiel = _currentProfiel

            };

            _mediaService.InsertRating(newRating);
            _mediaService.SaveChanges();

            return RedirectToAction("Details", new { music.Id });
        }
    }
}
