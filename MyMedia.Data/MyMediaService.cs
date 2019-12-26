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
            return context.Media;
        }
        public IEnumerable<Rating> GetAllRatings()
        {
            return context.Ratings;
        }
        public IEnumerable<Profiel> GetAllProfielen()
        {
            return context.Profiel;
        }
        public IEnumerable<Serie> GetAllSeries()
        {
            return context.Series;
        }
        public IEnumerable<Seizoen> GetAllSeasons()
        {
            return context.Seizoenen;
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
            return context.Podcast;
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
            var podcastToDelete = context.Podcast.FirstOrDefault(m => m.Id == id);
            if (podcastToDelete != null)
            {
                context.Podcast.Remove(podcastToDelete);
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
        public void InsertProfiel(Profiel profiel)
        {
            context.Profiel.Add(profiel);
        }
        public void InsertPodcast(Podcast podcast)
        {
            context.Podcast.Add(podcast);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void InsertSeizoen(Seizoen seizoen)
        {
            context.Seizoenen.Add(seizoen);
        }
        public void InsertEpisode(Episode episode)
        {
            context.Episodes.Add(episode);
        }
    }
}
