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
        IEnumerable<MediaUser> GetAllMediaUsers();
        void InsertMediaUser(MediaUser MediaUser);
        IEnumerable<Rating> GetAllRatings();
        IEnumerable<Serie> GetAllSeries();
        IEnumerable<Season> GetAllSeasons();
        IEnumerable<Episode> GetAllEpisodes();

        IEnumerable<PlayList> GetAllPlaylists();
        IEnumerable<Podcast> GetAllPodcasts();
        void DeleteSerieById(int id);
        void DeletePodcastById(int id);
        void DeleteRatingsByMediaId(int id);
        void InsertRating(Rating rating);
        void InsertSerie(Serie serie);
        void InsertPodcast(Podcast podcast);
        void InsertSeason(Season Season);
        void InsertEpisode(Episode episode);













    }
}
