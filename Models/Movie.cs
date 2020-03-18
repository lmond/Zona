using System.Collections.Generic;

namespace Zona.Models
{
    public class Movies
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
        public double Star { get; set; }
        public string Link { get; set; }
        public string Year { get; set; }
    }
    public class Movie
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
        public double Star { get; set; }
        public string Link { get; set; }
        public string Year { get; set; }
        public string Duration { get; set; }
        public string MovieFile { get; set; } = null;
        public string Trailer { get; set; } = null;
        public string Description { get; set; }
        public List<ActorsList> ActorsList { get; set; }
        public string Genres { get; set; }
        public string Countries { get; set; }
        public string Directors { get; set; }
        public string Cover { get; set; }
        public List<string> Posters { get; set; } = null;
        public List<Season> Seasons { get; set; } = null;
        public List<Episode> EpisodeItem { get; set; } = null;
    }
    public class ActorsList {
        public string Image{ get; set; }
        public string Name { get; set; }
    }
    public class Video
    {
        public List<string> images { get; set; }
        public string url { get; set; }
        public string host { get; set; }
    }
    public class Season
    {
        public string Title { get; set; }
        public string Link { get; set; }
    }
    public class Episode
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
    }
}
