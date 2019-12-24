using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMedia.Core.MediaClasses;
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
        private readonly SignInManager<IdentityUser> _signinManager;
        private Profiel? _currentProfiel;

        public PodcastsController(IMyMediaService service, SignInManager<IdentityUser> signinManager)
        {
            _mediaService = service;
            _signinManager = signinManager;
        }
        public IActionResult Index()
        {
            var podcasts = _mediaService.GetAllPodcasts().ToList();
            var podcastList = new List<PodcastListViewModel>();

            podcastList = podcasts.Select(pl =>
               new PodcastListViewModel
               {
                   Id = pl.Id,
                   Naam = pl.Naam,
                   Foto = pl.Foto
               }).ToList();

            return View(podcastList);
        }

        public IActionResult Details(int id)
        {
            var isSignedIn = this._signinManager.IsSignedIn(HttpContext.User);
            var currentUserId = this._signinManager.UserManager.GetUserId(HttpContext.User);
            if (isSignedIn)
            {
                _currentProfiel = _mediaService.GetAllProfielen().First(p => p.UserId == currentUserId);
            }
            Podcast selectedPodcast = _mediaService.GetAllPodcasts().FirstOrDefault(x => x.Id == id);

            var UserRatingList = _mediaService.GetAllRatings().Where(pod => pod.Media.Id == selectedPodcast.Id);
            Podcast podcast = _mediaService.GetAllPodcasts().First(x => x.Id == id);
            var media = _mediaService.GetAllMedia().First(med => med.Titel == podcast.Titel);
            PodcastDetailViewModel vm = new PodcastDetailViewModel()
            {
                MediaId = id,
                Naam = podcast.Naam,
                Foto = podcast.Foto,
                PodcastLink = podcast.ConversationMP3,
                IsRated = UserRatingList.Count()>0? true :false,
                IsSignedIn = currentUserId == null ? false : true,
                
            };
            vm.AveragePoints = selectedPodcast.Ratings.Count() > 0 ? selectedPodcast.Ratings.Average(r => r.Points) : 0;
            vm.PlayLists = new List<PlayList>();
            if (_currentProfiel != null)
            {
                vm.PlayLists = _currentProfiel.Playlists.ToList();
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

            Podcast newPodcast = new Podcast()
            {
                Naam = model.Naam,
                ConversationMP3 = model.PodcastLink,
                Titel = model.Titel
            };
            _mediaService.InsertPodcast(newPodcast);
            _mediaService.SaveChanges();
            Podcast podcastFromDb = _mediaService.GetAllPodcasts().FirstOrDefault(z => z.Id == newPodcast.Id);

            if (model.Foto != null)
            {
                using var memoryStream = new MemoryStream();
                model.Foto.CopyTo(memoryStream);
                podcastFromDb.Foto = memoryStream.ToArray();
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
                Naam = selectedPodcast.Naam,
                PodcastLink = selectedPodcast.ConversationMP3
            };
            return View(podcastEditModel);
        }

        [HttpPost]
        public IActionResult Edit(PodcastEditViewModel model)
        {
            var selectedPodcast = _mediaService.GetAllPodcasts().First(x => x.Id == model.Id);
            selectedPodcast.ConversationMP3 = model.PodcastLink;
            selectedPodcast.Naam = model.Naam;
            selectedPodcast.Titel = model.Naam;
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
            var isSignedIn = this._signinManager.IsSignedIn(HttpContext.User);
            var currentUserId = this._signinManager.UserManager.GetUserId(HttpContext.User);
            if (isSignedIn)
            {
                _currentProfiel = _mediaService.GetAllProfielen().First(p => p.UserId == currentUserId);
            }

            var newRating = new Rating()
            {
                Media = podcast,
                CreationDate = DateTime.Now,
                Points = model.Points,
                Profiel = _currentProfiel
            };

            _mediaService.InsertRating(newRating);
            _mediaService.SaveChanges();

            return RedirectToAction("Details", new {podcast.Id });
        }
       
    }

}
