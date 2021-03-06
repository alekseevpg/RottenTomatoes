﻿using System;
using RestSharp;

namespace RottenApi
{
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

        public void GetMovieInfo(string id, Action<MovieInfo> callback)
        {
            var request = new RestRequest(string.Format("movies/{0}.json", id));

            request.AddParameter("apikey", RottenKey);
            _restClient.ExecuteAsync<MovieInfo>(request, response =>
            {
                if ((response.ErrorException == null || response.Content.Contains("error")) && callback != null)
                {
                    callback(response.Data);
                }
            });
        }

        public void GetMovieCast(string id, Action<MovieCast> callback)
        {
            var request = new RestRequest(string.Format("movies/{0}/cast.json", id));

            request.AddParameter("apikey", RottenKey);
            _restClient.ExecuteAsync<MovieCast>(request, response =>
            {
                if ((response.ErrorException == null || response.Content.Contains("error")) && callback != null)
                {
                    callback(response.Data);
                }
            });
        }

        public void GetMovieReviews(string id, Action<ReviewList> callback)
        {
            var request = new RestRequest(string.Format("movies/{0}/reviews.json", id));

            request.AddParameter("apikey", RottenKey);
            _restClient.ExecuteAsync<ReviewList>(request, response =>
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