using Microsoft.EntityFrameworkCore;
using MyMedia.Core;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using System.Collections.Generic;
using System.Linq;

namespace MyMedia.Data
{
    public class MyMediaService : IMyMediaService
    {
        private readonly MediaDbContext context;

        public MyMediaService(MediaDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Media> GetAllMedia()
        {
            return context.Media.Include("Ratings");
        }
        public IEnumerable<Rating> GetAllRatings()
        {
            return context.Ratings;
        }
        public IEnumerable<MediaUser> GetAllMediaUsers()
        {
            return context.MediaUsers;
        }
        public IEnumerable<Serie> GetAllSeries()
        {
            return context.Series;
        }
        public IEnumerable<Season> GetAllSeasons()
        {
            return context.Seasonen;
        }
        public IEnumerable<Episode> GetAllEpisodes()
        {
            return context.Episodes;
        }
        public IEnumerable<PlayList> GetAllPlaylists()
        {
            return context.Playlist;
        }
        public IEnumerable<Podcast> GetAllPodcasts()
        {
            return context.Podcasts;
        }


        public Media GetMediaById(int id)
        {
            return context.Media.FirstOrDefault(m => m.Id == id);
        }
        public void DeleteMediaById(int id)
        {
            var mediaToDelete = context.Media.FirstOrDefault(m => m.Id == id);
            if (mediaToDelete != null)
            {
                context.Media.Remove(mediaToDelete);
            }
        }
        public void DeleteSerieById(int id)
        {
            var serieToDelete = context.Series.FirstOrDefault(m => m.Id == id);
            if (serieToDelete != null)
            {
                context.Series.Remove(serieToDelete);
            }
        }
        public void DeletePodcastById(int id)
        {
            var podcastToDelete = context.Podcasts.FirstOrDefault(m => m.Id == id);
            if (podcastToDelete != null)
            {
                context.Podcasts.Remove(podcastToDelete);
            }
        }
        public void DeleteRatingsByMediaId(int id)
        {
            var ratings = context.Ratings.Where(m => m.Id == id);
            if (ratings != null)
            {
                foreach (var rating in ratings)
                {
                    context.Ratings.Remove(rating);
                }
            }
        }
        public void InsertRating(Rating rating)
        {
            context.Ratings.Add(rating);
        }
        public void InsertMedia(Media media)
        {
            context.Media.Add(media);
        }
        public void InsertSerie(Serie serie)
        {
            context.Series.Add(serie);
        }
        public void InsertMediaUser(MediaUser MediaUser)
        {
            context.MediaUsers.Add(MediaUser);
        }
        public void InsertPodcast(Podcast podcast)
        {
            context.Podcasts.Add(podcast);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void InsertSeason(Season Season)
        {
            context.Seasonen.Add(Season);
        }
        public void InsertEpisode(Episode episode)
        {
            context.Episodes.Add(episode);
        }
    }
}
