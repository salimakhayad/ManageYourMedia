using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyMedia.Models.Episodes;
using MyMedia.Models.Series.Episodes;
using MyMedia.Data;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;

namespace MyMedia.Controllers
{
    
    public class EpisodesController : Controller
    {
        private readonly IMyMediaService _mediaService;
        private readonly IUserStore<Profiel> _userStore;
        private readonly IUserClaimsPrincipalFactory<Profiel> _claimsPrincipalFactory;
        private readonly SignInManager<Profiel> _signInManager;
        private readonly UserManager<Profiel> _userManager;
        private Profiel? _currentProfiel;

        public EpisodesController(IMyMediaService mediaService,
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
        public IActionResult AddEpisode(int serieId)
        {
            var selectedSerie = _mediaService.GetAllSeries().First(serie => serie.Id == serieId);


            selectedSerie.Seizoenen = _mediaService.GetAllSeasons().Where(z => z.Serie.Id == serieId).ToList();
            var seizoenen = selectedSerie.Seizoenen == null ? new List<Seizoen>() : selectedSerie.Seizoenen.ToList();

            EpisodeCreateViewModel Epimodel = new EpisodeCreateViewModel()
            {
                SerieNaam = selectedSerie.Naam,
                MogelijkeSeizoenen = seizoenen,
                ReleaseDate = DateTime.Now
            };
            return View(Epimodel);
        }

        public IActionResult CreateForExistingSerie(int serieId)
        {
            var serie = _mediaService.GetAllSeries().First(x => x.Id == serieId);

            EpisodeCreateViewModel Epimodel = new EpisodeCreateViewModel()
            {
                SerieId = serie.Id,
                SerieNaam = serie.Naam
            };

            return View(Epimodel);
        }
        [Authorize]
        public IActionResult Details(int id)
        {
            var isSignedIn = this._signInManager.IsSignedIn(HttpContext.User);
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            if (isSignedIn)
            {
                _currentProfiel =_mediaService.GetAllProfielen().First(p => p.Id == currentUserId);
            }
            var episode = _mediaService.GetAllEpisodes().FirstOrDefault(epi => epi.Id == id);
            var isRated = false;
            var playLists = new List<PlayList>();
            var AveragePoints = 0.0;
            if (_currentProfiel != null)
            {
                isRated = _mediaService.GetAllRatings().Where(epi => epi.Media.Id == epi.Id && epi.Profiel.Id == _currentProfiel.Id).Any();
                playLists = _currentProfiel.Playlists.ToList();
            

            }
            AveragePoints = _mediaService.GetAllRatings().Where(epi => epi.Media.Id == epi.Id).Average(epi => epi.Points);

            EpisodeDetailViewModel model = new EpisodeDetailViewModel()
            {
                MediaId = episode.Id,
                SeizoenNr = episode.Seizoen.SeizoenNr,
                Beschrijving = episode.Beschrijving,
                Duration = episode.Duration,
                Foto = episode.Foto,
                ReleaseDate = episode.ReleaseDate,
                IMDBLink = episode.IMDBLink,
                Titel = episode.Titel,
                IsRated = isRated,
                IsSignedIn = isSignedIn,
                PlayLists = playLists,
                AveragePoints = AveragePoints

            };
            return View(model);
        }

        public IActionResult Create(int id)
        {
            var foundSerie = _mediaService.GetAllSeries().First(s => s.Id == id);
            var seizoenen = _mediaService.GetAllSeasons().Where(sz => sz.Serie.Id == id);
            if (!IsAanwezig(seizoenen))
            {
                CreateNewSeizoenVoorSerie(foundSerie);
            }
            EpisodeCreateViewModel vm = new EpisodeCreateViewModel()
            {
                SerieId = foundSerie.Id,
                SerieNaam = foundSerie.Naam,
                MogelijkeSeizoenen = GetSeizoenenVoorSerie(foundSerie),
                ReleaseDate = DateTime.Now
            };

            return View(vm);
        }
        private static bool IsAanwezig(IEnumerable<Seizoen> seizoenen)
        {
            return seizoenen.Any();
        }
        private void CreateNewSeizoenVoorSerie(Serie foundSerie)
        {
            var newSeizoen = new Seizoen
            {
                SeizoenNr = 1,
                Serie = foundSerie
            };
            _mediaService.InsertSeizoen(newSeizoen);
            _mediaService.SaveChanges();
        }
        private List<Seizoen> GetSeizoenenVoorSerie(Serie foundSerie)
        {
            return _mediaService.GetAllSeasons().Where(sz => sz.Serie.Id == foundSerie.Id).ToList();
        }
        

        [HttpPost]
        public IActionResult Create(EpisodeCreateViewModel model)
        {
            var serieId = model.SerieId;

            if (model.SeizoenId == 0)
            {
                Seizoen newSeason = new Seizoen()
                {
                    Serie = _mediaService.GetAllSeries().FirstOrDefault(ser => ser.Id == serieId),
                    SeizoenNr = _mediaService.GetAllSeasons().Where(seizoen => seizoen.Serie.Id == serieId).Count() + 1,
                    Episodes = new List<Episode>()
            };
                _mediaService.InsertSeizoen(newSeason);
                _mediaService.SaveChanges();
                var season = _mediaService.GetAllSeasons().First(sei => sei.Id == newSeason.Id);
                _mediaService.SaveChanges();

                season.Episodes.Add(new Episode()
                {
                    Beschrijving = model.Beschrijving,
                    Duration = model.Duration,
                    ReleaseDate = model.ReleaseDate,
                    Titel = model.Titel,
                    IMDBLink = model.IMDBLink,
                    Seizoen = newSeason
                });
                _mediaService.SaveChanges();
            }
            else
            {
               var seizoen = _mediaService.GetAllSeasons().First(seizoen => seizoen.Serie.Id == serieId && seizoen.SeizoenNr == model.SeizoenId);
                seizoen.Episodes.Add(new Episode()
                {
                    Beschrijving = model.Beschrijving,
                    Duration = model.Duration,
                    ReleaseDate = model.ReleaseDate,
                    Titel = model.Titel,
                    IMDBLink = model.IMDBLink,
                    Seizoen = seizoen
                });
                _mediaService.SaveChanges();
            }


            return RedirectToAction("Details", "Serie", new {Id = serieId });
     
        }

        public IActionResult RateEpisode(EpisodeRateViewModel model)
        {
            var episode = _mediaService.GetAllEpisodes().First(epi => epi.Id == model.MediaId);
            var isSignedIn = this._signInManager.IsSignedIn(HttpContext.User);
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            if (isSignedIn)
            {
                _currentProfiel = _mediaService.GetAllProfielen().First(p => p.Id == currentUserId);
            }

            var newRating = new Rating()
            {
                Media = episode,
                CreationDate = DateTime.Now,
                Points = model.Points,
                Profiel = _currentProfiel

            };

            _mediaService.InsertRating(newRating);
            _mediaService.SaveChanges();
            return RedirectToAction("Details", new {episode.Id });
        }

    }
}