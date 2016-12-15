using System;
using RestSharp;

namespace RottenApi
{
    public interface IServerApi
    {
        void GetBoxOfficeMovies(Action<MovieList> callback);

        void GetOpeningThisWeek(Action<MovieList> callback);

        void GetAlsoInTheaters(Action<MovieList> callback);

        void GetMovieInfo(string id, Action<MovieInfo> callback);

        void GetMovieCast(string id, Action<MovieCast> callback);

        void GetMovieReviews(string id, Action<ReviewList> callback);
    }
    
}