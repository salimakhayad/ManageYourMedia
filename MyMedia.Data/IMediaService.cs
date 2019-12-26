using MyMedia.Core;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using System.Collections.Generic;

namespace MyMedia.Data
{
    public interface IMyMediaService
    {
         Media GetMediaById(int id);
        IEnumerable<Media> GetAllMedia();
        void DeleteMediaById(int id);
        void InsertMedia(Media media);
        void SaveChanges();
        IEnumerable<Profiel> GetAllProfielen();
        void InsertProfiel(Profiel profiel);
        IEnumerable<Rating> GetAllRatings();
        IEnumerable<Serie> GetAllSeries();
        IEnumerable<Seizoen> GetAllSeasons();
        IEnumerable<Episode> GetAllEpisodes();

        IEnumerable<PlayList> GetAllPlaylists();
        IEnumerable<Podcast> GetAllPodcasts();
        void DeleteSerieById(int id);
        void DeletePodcastById(int id);
        void DeleteRatingsByMediaId(int id);
        void InsertRating(Rating rating);
        void InsertSerie(Serie serie);
        void InsertPodcast(Podcast podcast);
        void InsertSeizoen(Seizoen seizoen);
        void InsertEpisode(Episode episode);













    }
}
