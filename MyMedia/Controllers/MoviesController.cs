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
using System.Threading.Tasks;

namespace MyMedia.Controllers
{
   
    public class MoviesController : Controller
    {
        
        private readonly IMyMediaService _mediaService;
        private readonly SignInManager<MediaUser> _signInManager;
        private readonly IUserStore<MediaUser> _userStore;
        private readonly UserManager<MediaUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<MediaUser> _claimsPrincipalFactory;
        private MediaUser? _currentMediaUser;

        public MoviesController(IMyMediaService mediaService,
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
        //[HttpGet("Movies")]
        [Authorize]
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
                        Photo = movie.Photo,
                        ReleaseDate = movie.ReleaseDate,
                    }); ;

                }
            }

            return View(Movies);
        }
        [Authorize]
        public IActionResult Create()
        {

            MovieCreateViewModel mov = new MovieCreateViewModel()
            {
                ReleaseDate = DateTime.Today
            };


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
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
            _currentMediaUser = _mediaService.GetAllMediaUsers().Where(u => u.Id == currentUserId).First();
            Movie newMovie = new Movie()
            {
                Titel = model.Titel,
                Duration = (model.Duration),
                IMDBLink = model.IMDBLink,
                ReleaseDate = model.ReleaseDate,
                Status = model.Status,
                MediaUser = this._currentMediaUser,
                MediaUserId = currentUserId
        };
            _mediaService.InsertMedia(newMovie);
            _mediaService.SaveChanges();

            var foundMovie = _mediaService.GetAllMedia().OfType<Movie>().FirstOrDefault(x => x.Id == newMovie.Id);
            _mediaService.SaveChanges();

            if (model.Photo != null)
            {
                using var memoryStream = new MemoryStream();
                model.Photo.CopyTo(memoryStream);
                foundMovie.Photo = memoryStream.ToArray();
            }

            _mediaService.SaveChanges();

            return RedirectToAction("Details", new { foundMovie.Id });

        }

        public IActionResult Details(int Id)
        {
           // var isSignedIn = this._signinManager.IsSignedIn(HttpContext.User);
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
           
           
            _currentMediaUser = _mediaService.GetAllMediaUsers().First(p => p.Id == currentUserId);
          
            Movie selectedMovie = _mediaService.GetAllMedia().OfType<Movie>().FirstOrDefault(mov => mov.Id == Id);
            bool isAlreadyRatedByCurrentUser = false;
            var playlists = new List<PlayList>();
            if (_currentMediaUser != null)
            {
                var allratings = _mediaService.GetAllRatings().Where(media => media.Id == selectedMovie.Id);

                isAlreadyRatedByCurrentUser = allratings.Where(media => media.MediaUser.Id == _currentMediaUser.Id).Any();
                playlists = _currentMediaUser.Playlists.ToList();
            }

            MovieDetailViewModel model = new MovieDetailViewModel()
            {
                MediaId = selectedMovie.Id,
                Titel = selectedMovie.Titel,
                Photo = selectedMovie.Photo,
                Duration = selectedMovie.Duration,
                IMDBLink = selectedMovie.IMDBLink,
                Status = selectedMovie.Status,
                PlayLists = playlists,
                AveragePoints = selectedMovie.Ratings.Count()>0 ? selectedMovie.Ratings.Average(r => r.Points) : 0,
                IsRated = isAlreadyRatedByCurrentUser,
                AddedByUserName = selectedMovie.MediaUser.UserName,
                Ratings = selectedMovie.Ratings.ToList(),
                Reviews = selectedMovie.Ratings.Select(r => r.Review).ToList()
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
                if (model.Photo != null)
                {
                    model.Photo.CopyTo(memoryStream);
                    mvFromDb.Photo = memoryStream.ToArray();
                }
            }
            _mediaService.SaveChanges();

            return RedirectToAction("Details", new { mvFromDb.Id });
        }
        [Authorize]
        public IActionResult RateMovie(MovieRateViewModel model)
        {
           var movie = _mediaService.GetAllMedia().OfType<Movie>().First(mov => mov.Id == model.MediaId);
           var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);
           var user = _mediaService.GetAllMediaUsers().Where(prof => prof.Id == currentUserId).FirstOrDefault();
            
            var newRating = new Rating()
            {
                Media = movie,
                CreationDate = DateTime.Now,
                Points = model.Points,
                Review = model.Review,
                MediaUser = user
            };

             _mediaService.InsertRating(newRating);
            _mediaService.SaveChanges();
            return RedirectToAction("Details", new { movie.Id });
        }
    }
}
