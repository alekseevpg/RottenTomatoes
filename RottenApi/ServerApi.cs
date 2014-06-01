using System;
using System.Collections.Generic;
using RestSharp;
using System.Net;

namespace RottenApi
{
    public interface IServerApi
    {
        void GetBoxOfficeMovies(Action<MovieList> callback);

        void GetOpeningThisWeek(Action<MovieList> callback);

        void GetAlsoInTheaters(Action<MovieList> callback);
    }

    public class ServerApi : IServerApi
    {
        private readonly string RottenKey = "3b3ywrhy5rrsc4u7b32s5wfp";
        private RestClient _restClient;

        public ServerApi()
        {
            _restClient = new RestClient("http://api.rottentomatoes.com/api/public/v1.0/");
        }

        public void GetOpeningThisWeek(Action<MovieList> callback)
        {
            var request = new RestRequest("lists/movies/opening.json");
            request.AddParameter("limit", 2);
            ExecuteMovieRequestRequest(request, callback);
        }

        public void GetBoxOfficeMovies(Action<MovieList> callback)
        {
            var request = new RestRequest("lists/movies/box_office.json");
            request.AddParameter("limit", 10);
            ExecuteMovieRequestRequest(request, callback);
        }

        public void GetAlsoInTheaters(Action<MovieList> callback)
        {
            var request = new RestRequest("lists/movies/in_theaters.json");
            request.AddParameter("page_limit", 10);
            request.AddParameter("page", 2);
            ExecuteMovieRequestRequest(request, callback);
        }

        public void GetMovieInfo(string id, Action callback)
        {
            var request = new RestRequest(string.Format("lists/movies/{0}.json", id));

            request.AddParameter("apikey", RottenKey);
            _restClient.ExecuteAsync<MovieList>(request, response =>
            {
                if ((response.ErrorException == null || response.Content.Contains("error")) && callback != null)
                {
                    callback(response.Data);
                }
            });
        }

        private void ExecuteMovieRequestRequest(RestRequest request, Action<MovieList> callback)
        {
            request.AddParameter("apikey", RottenKey);
            _restClient.ExecuteAsync<MovieList>(request, response =>
            {
                if ((response.ErrorException == null || response.Content.Contains("error")) && callback != null)
                {
                    callback(response.Data);
                }
            });
        }
    }
}

