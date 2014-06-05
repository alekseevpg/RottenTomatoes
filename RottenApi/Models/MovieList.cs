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

        public string GetFormattedRuntime()
        {
            return string.Format("{0}hr. {1}min.", Runtime / 60, Runtime % 60);
        }

        public string GetFormattedCast()
        {
            if (AbridgedCast.Count != 0)
            {
                return AbridgedCast.Count == 1 ?
                    AbridgedCast[0].Name :
                    string.Format("{0}, {1}", AbridgedCast[0].Name, AbridgedCast[1].Name);
            }
            return string.Empty;
        }
    }

    public class MovieInfo : Movie
    {
        public List<string> Genres { get; set; }

        public string CriticsConsensus { get; set; }

        public string Synopsis { get; set; }

        public List<Human> AbridgedDirectors { get; set; }

        public string GetFormattedDirector()
        {
            if (AbridgedDirectors.Count != 0)
            {
                return AbridgedDirectors.Count == 1 ?
                    AbridgedDirectors[0].Name :
                    string.Format("{0}, {1}", AbridgedDirectors[0].Name, AbridgedDirectors[1].Name);
            }
            return string.Empty;
        }

        public string GetFormattedGenres()
        {
            return string.Join(", ", Genres.ToArray());
        }
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

    public class Human
    {
        public string Name { get; set; }
    }

    public class Actor : Human
    {
        public string Id { get; set; }

        public List<string> Characters { get; set; }

        public string GetFormattedChars()
        {
            return string.Join(", ", Characters.ToArray());
        }
    }

    public class MovieCast
    {
        public List<Actor> Cast { get; set; }
    }

    public class ReleaseDates
    {
        public DateTime Theater { get; set; }

        public DateTime Dvd { get; set; }

        public string GetFormattedTheaterDate()
        {
            if (Theater == DateTime.MinValue)
                return "Not available";
            else
                return Theater.ToString("MMM, yyyy");
        }

        public string GetFormattedDvdDate()
        {
            if (Dvd == DateTime.MinValue)
                return "Not available";
            else
                return Dvd.ToString("MMM, yyyy");
        }
    }

    public class ReviewList
    {
        public List<Review> Reviews { get; set; }
    }

    public class Review
    {
        public string Critic { get; set; }

        public string Freshness { get; set; }

        public string Publication { get; set; }

        public string Quote { get; set; }

        public Link Links { get; set; }
    }

    public class Link
    {
        public string Review { get; set; }
    }
}

