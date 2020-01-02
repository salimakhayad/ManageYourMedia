using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using MyMedia.Data;
using MyMedia.Models.Movie;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyMedia.Controllers
{
   
    public class MovieController : Controller
    {
        
        private readonly IMyMediaService _mediaService;
        private readonly SignInManager<Profiel> _signInManager;
        private readonly IUserStore<Profiel> _userStore;
        private readonly UserManager<Profiel> _userManager;
        private readonly IUserClaimsPrincipalFactory<Profiel> _claimsPrincipalFactory;
        private Profiel? _currentProfiel;

        public MovieController(IMyMediaService mediaService,
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
        //[HttpGet("Movie")]
        public IActionResult Index()
        {
            List<MovieListViewModel> Movies = new List<MovieListViewModel>();
            IEnumerable<Movie>? movieListFromDb = _mediaService.GetAllMedia().OfType<Movie>();
            if (movieListFromDb.Any() && movieListFromDb != null)
            {
                foreach (var movie in movieListFromDb)
                {
                    Movies.Add(new MovieListViewModel()
                    {
                        Id = movie.Id,
                        Titel = movie.Titel,
                        Duration = movie.Duration,
                        IMDBLink = movie.IMDBLink,
                        Foto = movie.Foto,
                        ReleaseDate = movie.ReleaseDate,
                    }); ;

                }
            }

            return View(Movies);
        }
        [Authorize]
        public IActionResult Create()
        {

            MovieCreateViewModel mov = new MovieCreateViewModel();

            return View(mov);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create(MovieCreateViewModel model)
        {
            if (!TryValidateModel(model))
            {
                return View(model);
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
            }
            Movie newMovie = new Movie()
            {
                Titel = model.Titel,
                Duration = (model.Duration),
                IMDBLink = model.IMDBLink,
                ReleaseDate = model.ReleaseDate,
                Status = model.Status
            };
            _mediaService.InsertMedia(newMovie);
            _mediaService.SaveChanges();

            var foundMovie = _mediaService.GetAllMedia().OfType<Movie>().FirstOrDefault(x => x.Id == newMovie.Id);
            _mediaService.SaveChanges();

            if (model.Foto != null)
            {
                using var memoryStream = new MemoryStream();
                model.Foto.CopyTo(memoryStream);
                foundMovie.Foto = memoryStream.ToArray();
            }

            _mediaService.SaveChanges();

            return RedirectToAction("Details", new { foundMovie.Id });

        }

        public IActionResult Details(int id)
        {
           // var isSignedIn = this._signinManager.IsSignedIn(HttpContext.User);
           // var currentUserId = this._signinManager.UserManager.GetUserId(HttpContext.User);
           // if (isSignedIn)
           // {
           //     _currentProfiel = _mediaService.GetAllProfielen().First(p => p.Id == currentUserId);
           // }
            Movie selectedMovie = _mediaService.GetAllMedia().OfType<Movie>().FirstOrDefault(x => x.Id == id);
            bool isAlreadyRated = false;
            var playlists = new List<PlayList>();
            if (_currentProfiel != null)
            {
                isAlreadyRated = _mediaService.GetAllRatings().Where(movie => movie.Media.Titel == selectedMovie.Titel).Where(user => user.Profiel.Id == _currentProfiel.Id).Any();
                playlists = _currentProfiel.Playlists.ToList();
            }

            MovieDetailViewModel model = new MovieDetailViewModel()
            {
                MediaId = selectedMovie.Id,
                Titel = selectedMovie.Titel,
                Foto = selectedMovie.Foto,
                Duration = selectedMovie.Duration,
                IMDBLink = selectedMovie.IMDBLink,
                Status = selectedMovie.Status,
                PlayLists = playlists,
                AveragePoints = selectedMovie.Ratings.Count()>0 ? selectedMovie.Ratings.Average(r => r.Points) : 0,
                IsRated = isAlreadyRated
            };

            return View(model);
        }
        public IActionResult Delete(int id)
        {
            Movie selectedMovie = _mediaService.GetAllMedia().OfType<Movie>().FirstOrDefault(mov => mov.Id == id);
            MovieDeleteViewModel model = new MovieDeleteViewModel()
            {
                Id = id,
                Titel = selectedMovie.Titel,
            };
            return View(model);
        }
        [Authorize]
        public IActionResult ConfirmDelete(int Id)
        {
          
            _mediaService.GetAllRatings().Where(rat => rat.Media.Id == Id);
            _mediaService.SaveChanges();
            _mediaService.DeleteMediaById(Id);
            _mediaService.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult CreateReview()
        {
            MovieCreateViewModel model = new MovieCreateViewModel();
            return View(model);
        }
        [Authorize]
        public IActionResult Edit(int id)
        {
            Movie mvFromDb = _mediaService.GetAllMedia().OfType<Movie>().First(z => z.Id == id);
            MovieEditViewModel model = new MovieEditViewModel()
            {
                Duration = mvFromDb.Duration,
                Id = mvFromDb.Id,
                IMDBLink = mvFromDb.IMDBLink,
                ReleaseDate = mvFromDb.ReleaseDate,
                Titel = mvFromDb.Titel
            };
            return View(model);
       
        }
        [HttpPost]
        public IActionResult Edit(MovieEditViewModel model)
        {
            Movie mvFromDb = _mediaService.GetAllMedia().OfType<Movie>().First(z => z.Id == model.Id);
            mvFromDb.IMDBLink = model.IMDBLink;
            mvFromDb.ReleaseDate = model.ReleaseDate;
            mvFromDb.Status = model.Status;
            mvFromDb.Titel = model.Titel;
            mvFromDb.Duration = model.Duration;
            using (var memoryStream = new MemoryStream())
            {
                if (model.Foto != null)
                {
                    model.Foto.CopyTo(memoryStream);
                    mvFromDb.Foto = memoryStream.ToArray();
                }
            }
            _mediaService.SaveChanges();

            return RedirectToAction("Details", new { mvFromDb.Id });
        }
        [Authorize]
        public IActionResult RateMovie(MovieRateViewModel model)
        {
            var movie = _mediaService.GetAllMedia().OfType<Movie>().First(mov => mov.Id == model.MediaId);
            //var isSignedIn = this._signinManager.IsSignedIn(HttpContext.User);
            //var currentUserId = this._signinManager.UserManager.GetUserId(HttpContext.User);
            //if (isSignedIn)
            //{
            //    _currentProfiel = _mediaService.GetAllProfielen().First(p => p.Id == currentUserId);
            //}

            var newRating = new Rating()
            {
                Media = movie,
                CreationDate = DateTime.Now,
                Points = model.Points,
                Profiel = _currentProfiel
            };

             _mediaService.InsertRating(newRating);
            _mediaService.SaveChanges();
            return RedirectToAction("Details", new { movie.Id });
        }
    }
}
