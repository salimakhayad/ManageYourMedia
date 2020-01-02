using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using MyMedia.Data;
using MyMedia.Models.Series;
using System.Collections.Generic;
using System.Linq;

namespace MyMedia.Controllers
{

    public class SerieController : Controller
    {
        private readonly IMyMediaService _mediaService;
        private readonly SignInManager<Profiel> _signInManager;
        private readonly UserManager<Profiel> _userManager;
        private readonly IUserClaimsPrincipalFactory<Profiel> _claimsPrincipalFactory;
        private readonly IUserStore<Profiel> _userStore;
        private Profiel? _currentProfiel;

        public SerieController(IMyMediaService mediaService,
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
           //  var isSignedIn = this._signInManager.IsSignedIn(HttpContext.User);
           //  var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
           //  if (isSignedIn)
           //  {
           //      var profiel = service.GetAllProfielen().FirstOrDefault(p => p.Id == currentUserId);
           //      if (profiel == null)
           //      {
           //          var newProfiel = new Profiel
           //          {
           //              Id = currentUserId,
           //          };
           //          service.InsertProfiel(newProfiel);
           //          service.SaveChanges();
           //      }
           // 
           //      _currentProfiel = service.GetAllProfielen().First(p => p.Id == currentUserId);
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
                Seizoenen = serie.Seizoenen?.Where(x => x.Serie.Id == serie.Id).ToList() == null ? new List<Seizoen>() { new Seizoen() { SeizoenNr = 1 } } : serie.Seizoenen.Where(x => x.Serie.Id == serie.Id).ToList(),
                Id = serie.Id,
                Naam = serie.Naam,
                Foto = serie.Foto
            };
        }
       
        public IActionResult Delete(int id)
        {
            Serie selectedMovie = _mediaService.GetAllSeries().FirstOrDefault(x => x.Id == id);
            SerieDeleteViewModel model = new SerieDeleteViewModel()
            {
                Id = id,
                Naam = selectedMovie.Naam
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
                Naam = model.Naam
            };
            _mediaService.InsertSerie(newSerie);
            _mediaService.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            Serie serie = _mediaService.GetAllSeries().FirstOrDefault(x => x.Id == id);
            var allseasons = _mediaService.GetAllSeasons();
            serie.Seizoenen = _mediaService.GetAllSeasons().Where(seizoen=>seizoen.Serie == serie).ToList();

            SerieDetailViewModel vm = MapToViewModel(serie);
            // SetSeasonsAndEpisodes(vm);
            return View(vm);
        }




        private SerieDetailViewModel MapToViewModel(Serie serie)
        {
            return new SerieDetailViewModel()
            {
                Id = serie.Id,
                Naam = serie.Naam,
                Foto = serie.Foto,
                Seizoenen = serie.Seizoenen?.Count > 0 ? serie.Seizoenen.ToList() : new List<Seizoen>()
            };
        }
      
        public IActionResult Edit(int id)
        {
            Serie serie = _mediaService.GetAllSeries().FirstOrDefault(x => x.Id == id);
            SerieEditViewModel vm = new SerieEditViewModel()
            {
                Id = id,
                Naam = serie.Naam
            };
            vm.Seizoenen = _mediaService.GetAllSeasons().Where(z => z.Serie.Id == serie.Id).ToList();

            return View(vm);
        }

    }
}

