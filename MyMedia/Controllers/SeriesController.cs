using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using MyMedia.Data;
using MyMedia.Models.Series;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyMedia.Controllers
{

    public class SeriesController : Controller
    {
        private readonly IMyMediaService _mediaService;
        private readonly SignInManager<MediaUser> _signInManager;
        private readonly UserManager<MediaUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<MediaUser> _claimsPrincipalFactory;
        private readonly IUserStore<MediaUser> _userStore;
        private readonly MediaUser? _currentMediaUser;

        public SeriesController(IMyMediaService mediaService,
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
           //  var isSignedIn = this._signInManager.IsSignedIn(HttpContext.User);
           //  var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
           //  if (isSignedIn)
           //  {
           //      var MediaUser = service.GetAllMediaUseren().FirstOrDefault(p => p.Id == currentUserId);
           //      if (MediaUser == null)
           //      {
           //          var newMediaUser = new MediaUser
           //          {
           //              Id = currentUserId,
           //          };
           //          service.InsertMediaUser(newMediaUser);
           //          service.SaveChanges();
           //      }
           // 
           //      _currentMediaUser = service.GetAllMediaUseren().First(p => p.Id == currentUserId);
           // 
           //  }
            var series = _mediaService.GetAllSeries();
            if (series != null && series.Any())
            {
                var seriesViewModels = series.Select(MapToListViewModel).ToList();
                return View(seriesViewModels);
            }

            return View(new List<SerieListViewModel>());
        }

        private SerieListViewModel MapToListViewModel(Serie serie)
        {
            return new SerieListViewModel
            {
                Seasons = serie.Seasons?.Where(x => x.Serie.Id == serie.Id).ToList() == null ? new List<Season>() { new Season() { SeasonNr = 1 } } : serie.Seasons.Where(x => x.Serie.Id == serie.Id).ToList(),
                Id = serie.Id,
                Name = serie.Name,
                Photo = serie.Photo
            };
        }
       
        public IActionResult Delete(int id)
        {
            Serie selectedMovie = _mediaService.GetAllSeries().FirstOrDefault(x => x.Id == id);
            SerieDeleteViewModel model = new SerieDeleteViewModel()
            {
                Id = id,
                Name = selectedMovie.Name
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            _mediaService.DeleteSerieById(id);
            _mediaService.SaveChanges();
            return RedirectToAction("Index");
        }
       
        public IActionResult Create()
        {
            SerieCreateViewModel model = new SerieCreateViewModel();
            return View("Create", model);
        }

        [HttpPost]
        public IActionResult Create(SerieCreateViewModel model)
        {
            Serie newSerie = new Serie()
            {
                Name = model.Name,
                IsPublic = true
            };
            _mediaService.InsertSerie(newSerie);
            _mediaService.SaveChanges();
            var foundSerie = _mediaService.GetAllSeries().FirstOrDefault(x => x.Id == newSerie.Id);
          

            if (model.Photo != null)
            {
                using var memoryStream = new MemoryStream();
                model.Photo.CopyTo(memoryStream);
                foundSerie.Photo = memoryStream.ToArray();
            }

            _mediaService.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            Serie serie = _mediaService.GetAllSeries().FirstOrDefault(x => x.Id == id);
            var allseasons = _mediaService.GetAllSeasons();
            serie.Seasons = _mediaService.GetAllSeasons().Where(Season=>Season.Serie == serie).ToList();

            SerieDetailViewModel vm = MapToViewModel(serie);
            // SetSeasonsAndEpisodes(vm);
            return View(vm);
        }




        private SerieDetailViewModel MapToViewModel(Serie serie)
        {
            return new SerieDetailViewModel()
            {
                Id = serie.Id,
                Name = serie.Name,
                Photo = serie.Photo,
                Seasons = serie.Seasons?.Count > 0 ? serie.Seasons.ToList() : new List<Season>()
            };
        }
      
        public IActionResult Edit(int id)
        {
            Serie serie = _mediaService.GetAllSeries().FirstOrDefault(x => x.Id == id);
            SerieEditViewModel vm = new SerieEditViewModel()
            {
                Id = id,
                Name = serie.Name
            };
            vm.Seasons = _mediaService.GetAllSeasons().Where(z => z.Serie.Id == serie.Id).ToList();

            return View(vm);
        }

    }
}

