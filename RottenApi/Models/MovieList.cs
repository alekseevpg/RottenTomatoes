using System;
using System.Collections.Generic;

namespace RottenApi
{
    public class MovieList
    {
        public MovieList()
        {
            Movies = new List<Movie>();
        }

        public List<Movie> Movies { get; set; }
    }

    public class Movie
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Year { get; set; }

        public Ratings Ratings { get; set; }

        public int Runtime { get; set; }

        public string MpaaRating { get; set; }

        public Posters Posters { get; set; }

        public List<Actor> AbridgedCast { get; set; }

        public ReleaseDates ReleaseDates { get; set; }
    }

    public class Posters
    {
        public string Thumbnail { get; set; }

        public string Profile { get; set; }
    }

    public class Ratings
    {
        public string CriticsRating { get; set; }

        public string CriticsScore { get; set; }
    }

    public class Actor
    {
        public string Name { get; set; }

        public List<string> Characters { get; set; }
    }

    public class ReleaseDates
    {
        public DateTime Theater { get; set; }
    }
}

