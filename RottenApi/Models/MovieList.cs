using System;
using System.Collections.Generic;

namespace RottenApi
{
    public class MovieList
    {
        public List<Movie> Movies { get; set; }
    }

    public class Movie
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Year { get; set; }

        public string Rating { get; set; }

        public string Runtime { get; set; }

        public string Thumbnail { get; set; }

        public string MpaaRating { get; set; }

        public Posters Posters { get; set; }
    }

    public class Posters
    {
        public string Thumbnail { get; set; }

        public string Profile { get; set; }
    }
}

