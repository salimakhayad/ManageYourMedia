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
        private readonly IUserStore<MediaUser> _userStore;
        private readonly IUserClaimsPrincipalFactory<MediaUser> _claimsPrincipalFactory;
        private readonly SignInManager<MediaUser> _signInManager;
        private readonly UserManager<MediaUser> _userManager;
        private MediaUser? _currentMediaUser;

        public EpisodesController(IMyMediaService mediaService,
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
        public IActionResult AddEpisode(int serieId)
        {
            var selectedSerie = _mediaService.GetAllSeries().First(serie => serie.Id == serieId);


            selectedSerie.Seasons = _mediaService.GetAllSeasons().Where(z => z.Serie.Id == serieId).ToList();
            var Seasonen = selectedSerie.Seasons == null ? new List<Season>() : selectedSerie.Seasons.ToList();

            EpisodeCreateViewModel Epimodel = new EpisodeCreateViewModel()
            {
                SerieName = selectedSerie.Name,
                PossibleSeasons = Seasonen,
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
                SerieName = serie.Name
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
                _currentMediaUser =_mediaService.GetAllMediaUsers().First(p => p.Id == currentUserId);
            }
            var episode = _mediaService.GetAllEpisodes().FirstOrDefault(epi => epi.Id == id);
            var isRated = false;
            var playLists = new List<PlayList>();
            var AveragePoints = 0.0;
            if (_currentMediaUser != null)
            {
                isRated = _mediaService.GetAllRatings().Where(epi => epi.Media.Id == epi.Id && epi.MediaUser.Id == _currentMediaUser.Id).Any();
                playLists = _currentMediaUser.Playlists.ToList();
            

            }
            var points = _mediaService.GetAllRatings().Where(epi => epi.Media.Id == epi.Id).Average(epi => epi.Points); 

            AveragePoints = _mediaService.GetAllRatings().Where(epi => epi.Media.Id == epi.Id).Average(epi => epi.Points);

            EpisodeDetailViewModel model = new EpisodeDetailViewModel()
            {
                MediaId = episode.Id,
                SeasonNr = episode.Season.SeasonNr,
                Description = episode.Description,
                Duration = episode.Duration,
                Photo = episode.Photo,
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
            var Seasonen = _mediaService.GetAllSeasons().Where(sz => sz.Serie.Id == id);
            if (!IsAanwezig(Seasonen))
            {
                CreateNewSeasonVoorSerie(foundSerie);
            }
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);

            EpisodeCreateViewModel vm = new EpisodeCreateViewModel()
            {
                SerieId = foundSerie.Id,
                SerieName = foundSerie.Name,
                PossibleSeasons = GetSeasonenInSerie(foundSerie),
                ReleaseDate = DateTime.Now,
                MediaUser = _currentMediaUser,
                MediaUserId = currentUserId
            };

            return View(vm);
        }
        private static bool IsAanwezig(IEnumerable<Season> Seasonen)
        {
            return Seasonen.Any();
        }
        private void CreateNewSeasonVoorSerie(Serie foundSerie)
        {
            var newSeason = new Season
            {
                SeasonNr = 1,
                Serie = foundSerie
            };
            _mediaService.InsertSeason(newSeason);
            _mediaService.SaveChanges();
        }
        private List<Season> GetSeasonenInSerie(Serie foundSerie)
        {
            return _mediaService.GetAllSeasons().Where(sz => sz.Serie.Id == foundSerie.Id).ToList();
        }
        

        [HttpPost]
        public IActionResult Create(EpisodeCreateViewModel model)
        {
            var serieId = model.SerieId;

            if (model.SeasonId == 0)
            {
                Season newSeason = new Season()
                {
                    Serie = _mediaService.GetAllSeries().FirstOrDefault(ser => ser.Id == serieId),
                    SeasonNr = _mediaService.GetAllSeasons().Where(Season => Season.Serie.Id == serieId).Count() + 1,
                    Episodes = new List<Episode>()
                };
                _mediaService.InsertSeason(newSeason);
                _mediaService.SaveChanges();
                var season = _mediaService.GetAllSeasons().First(sei => sei.Id == newSeason.Id);
                _mediaService.SaveChanges();

                season.Episodes.Add(new Episode()
                {
                    Description = model.Description,
                    Duration = model.Duration,
                    ReleaseDate = model.ReleaseDate,
                    Titel = model.Titel,
                    IMDBLink = model.IMDBLink,
                    Season = newSeason
                });
                _mediaService.SaveChanges();
            }
            else
            {
               var Season = _mediaService.GetAllSeasons().First(Season => Season.Serie.Id == serieId && Season.SeasonNr == model.SeasonId);
                Season.Episodes.Add(new Episode()
                {
                    Description = model.Description,
                    Duration = model.Duration,
                    ReleaseDate = model.ReleaseDate,
                    Titel = model.Titel,
                    IMDBLink = model.IMDBLink,
                    Season = Season
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
                _currentMediaUser = _mediaService.GetAllMediaUsers().First(p => p.Id == currentUserId);
            }

            var newRating = new Rating()
            {
                Media = episode,
                CreationDate = DateTime.Now,
                Points = model.Points,
                MediaUser = _currentMediaUser

            };

            _mediaService.InsertRating(newRating);
            _mediaService.SaveChanges();
            return RedirectToAction("Details", new {episode.Id });
        }

    }
}