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
}