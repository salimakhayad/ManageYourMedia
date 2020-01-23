using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using MyMedia.Data;
using MyMedia.Models.Podcast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyMedia.Controllers
{
   
    public class PodcastsController : Controller
    {
        private readonly IMyMediaService _mediaService;
        private readonly IUserStore<MediaUser> _userStore;
        private readonly IUserClaimsPrincipalFactory<MediaUser> _claimsPrincipalFactory;
        private readonly SignInManager<MediaUser> _signInManager;
        private readonly UserManager<MediaUser> _userManager;
        private MediaUser? _currentMediaUser;

        public PodcastsController(IMyMediaService mediaService,
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
            var podcasts = _mediaService.GetAllPodcasts().ToList();
            var podcastList = new List<PodcastListViewModel>();

            podcastList = podcasts.Select(pl =>
               new PodcastListViewModel
               {
                   Id = pl.Id,
                   Name = pl.Name,
                   Photo = pl.Photo
               }).ToList();

            return View(podcastList);
        }

        public IActionResult Details(int id)
        {
           //var isSignedIn = this._signinManager.IsSignedIn(HttpContext.User);
           //var currentUserId = this._signinManager.UserManager.GetUserId(HttpContext.User);
           //if (isSignedIn)
           //{
           //    _currentMediaUser = _mediaService.GetAllMediaUseren().First(p => p.Id == currentUserId);
           //}
            Podcast selectedPodcast = _mediaService.GetAllPodcasts().FirstOrDefault(x => x.Id == id);

            var UserRatingList = _mediaService.GetAllRatings().Where(pod => pod.Media.Id == selectedPodcast.Id);
            Podcast podcast = _mediaService.GetAllPodcasts().First(x => x.Id == id);
            var media = _mediaService.GetAllMedia().First(med => med.Titel == podcast.Titel);
            PodcastDetailViewModel vm = new PodcastDetailViewModel()
            {
                MediaId = id,
                Name = podcast.Name,
                Photo = podcast.Photo,
                PodcastLink = podcast.ConversationMP3,
                IsRated = UserRatingList.Count()>0? true :false,
                IsSignedIn = false//currentUserId == null ? false : true,
                
            };
            vm.AveragePoints = selectedPodcast.Ratings.Count() > 0 ? selectedPodcast.Ratings.Average(r => r.Points) : 0;
            vm.PlayLists = new List<PlayList>();
            if (_currentMediaUser != null)
            {
                vm.PlayLists = _currentMediaUser.Playlists.ToList();
            }
            return View(vm);
        }

        public IActionResult Create()
        {
            PodcastCreateViewModel model = new PodcastCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PodcastCreateViewModel model)
        {
            if (!TryValidateModel(model))
            {
                return View(model);
            }

            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);

            Podcast newPodcast = new Podcast()
            {
                Name = model.Name,
                ConversationMP3 = model.PodcastLink,
                Titel = model.Titel,
                MediaUser = _currentMediaUser,
                MediaUserId = currentUserId
            };
            _mediaService.InsertPodcast(newPodcast);
            _mediaService.SaveChanges();
            Podcast podcastFromDb = _mediaService.GetAllPodcasts().FirstOrDefault(z => z.Id == newPodcast.Id);

            if (model.Photo != null)
            {
                using var memoryStream = new MemoryStream();
                model.Photo.CopyTo(memoryStream);
                podcastFromDb.Photo = memoryStream.ToArray();
            }

            _mediaService.SaveChanges();
            return RedirectToAction("Details", new { podcastFromDb.Id });

        }

        public IActionResult Edit(int podcastID)
        {
            var selectedPodcast = _mediaService.GetAllPodcasts().First(x => x.Id == podcastID);
            PodcastEditViewModel podcastEditModel = new PodcastEditViewModel()
            {
                Id = selectedPodcast.Id,
                Name = selectedPodcast.Name,
                PodcastLink = selectedPodcast.ConversationMP3
            };
            return View(podcastEditModel);
        }

        [HttpPost]
        public IActionResult Edit(PodcastEditViewModel model)
        {
            var selectedPodcast = _mediaService.GetAllPodcasts().First(x => x.Id == model.Id);
            selectedPodcast.ConversationMP3 = model.PodcastLink;
            selectedPodcast.Name = model.Name;
            selectedPodcast.Titel = model.Name;
            _mediaService.SaveChanges();
            return RedirectToAction("Details", new {selectedPodcast.Id });
        }


        public IActionResult Delete(int id)
        {
            //var selectedPodcast = _mediaService.GetAllPodcasts().First(zx => zx.Id == id);
            _mediaService.DeletePodcastById(id);
            _mediaService.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult RatePodcast(PodcastRateViewModel model)
        {
            var podcast = _mediaService.GetAllPodcasts().First(pod => pod.Id == model.MediaId);
            var currentUserId = this._signInManager.UserManager.GetUserId(HttpContext.User);


            _currentMediaUser = _mediaService.GetAllMediaUsers().First(p => p.Id == currentUserId);

            var newRating = new Rating()
            {
                Media = podcast,
                CreationDate = DateTime.Now,
                Points = model.Points,
                MediaUser = _currentMediaUser
            };

            _mediaService.InsertRating(newRating);
            _mediaService.SaveChanges();

            return RedirectToAction("Details", new {podcast.Id });
        }
       
    }

}
