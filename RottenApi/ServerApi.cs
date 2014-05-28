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
    }

    public class ServerApi : IServerApi
    {
        private RestClient _restClient;
        private readonly string RottenKey = "3b3ywrhy5rrsc4u7b32s5wfp";

        public ServerApi()
        {
            _restClient = new RestClient("http://api.rottentomatoes.com/api/public/v1.0/");
        }

        public void GetBoxOfficeMovies(Action<MovieList> callback)
        {
            var request = new RestRequest(string.Format("lists/movies/box_office.json?apikey={0}", RottenKey));
            _restClient.ExecuteAsync<MovieList>(request, response =>
            {
                if (response.ErrorException == null && callback != null)
                {
                    callback(response.Data);
                }
            });
        }

        public void GetOpeningThisWeek(Action<MovieList> callback)
        {
            var request = new RestRequest(string.Format("lists/movies/opening.json?apikey={0}", RottenKey));
            _restClient.ExecuteAsync<MovieList>(request, response =>
            {
                if (response.ErrorException == null && callback != null)
                {
                    callback(response.Data);
                }
            });
        }
    }
}

