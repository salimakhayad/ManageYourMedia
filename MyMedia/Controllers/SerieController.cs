using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using MyMedia.Data;
using MyMedia.Models.Series;
using System.Collections.Generic;
using System.Linq;

namespace MyMedia.Controllers
{

    public class SerieController : Controller
    {
        private readonly IMyMediaService service;
        private readonly SignInManager<IdentityUser> _signInManager;
        private Profiel? _currentProfiel;

        public SerieController(IMyMediaService context, SignInManager<IdentityUser> signinManager)
        {
            service = context;
            _signInManager = signinManager;
        }
        public IActionResult Index()
        {
            var isSignedIn = this._signInManager.IsSignedIn(HttpContext.User);
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            if (isSignedIn)
            {
                var profiel = service.GetAllProfielen().FirstOrDefault(p => p.UserId == currentUserId);
                if (profiel == null)
                {
                    var newProfiel = new Profiel
                    {
                        UserId = currentUserId,
                    };
                    service.InsertProfiel(newProfiel);
                    service.SaveChanges();
                }

                _currentProfiel = service.GetAllProfielen().First(p => p.UserId == currentUserId);

            }
            var series = service.GetAllSeries();
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
            Serie selectedMovie = service.GetAllSeries().FirstOrDefault(x => x.Id == id);
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
            service.DeleteSerieById(id);
            service.SaveChanges();
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
            service.InsertSerie(newSerie);
            service.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            Serie serie = service.GetAllSeries().FirstOrDefault(x => x.Id == id);
            var allseasons = service.GetAllSeasons();
            serie.Seizoenen = service.GetAllSeasons().Where(seizoen=>seizoen.Serie == serie).ToList();

            SerieDetailViewModel vm = MapToViewModel(serie);
            // SetSeasonsAndEpisodes(vm);
            return View(vm);
        }


        private void SetSeasonsAndEpisodes(SerieDetailViewModel vm)
        {
            foreach (var seizoen in vm.Seizoenen)
            {
                var listEpisodes = service.GetAllEpisodes().Where(x => x.Seizoen.Id == seizoen.Id);
                foreach (var episode in listEpisodes)
                {
                    vm.Seizoenen.First(x => x.Id == seizoen.Id).Episodes.Add(episode);
                }
            }
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
            Serie serie = service.GetAllSeries().FirstOrDefault(x => x.Id == id);
            SerieEditViewModel vm = new SerieEditViewModel()
            {
                Id = id,
                Naam = serie.Naam
            };
            vm.Seizoenen = service.GetAllSeasons().Where(z => z.Serie.Id == serie.Id).ToList();

            return View(vm);
        }

    }
}

