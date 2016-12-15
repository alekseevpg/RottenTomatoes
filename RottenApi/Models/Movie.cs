using System;
using System.Collections.Generic;

namespace RottenApi
{

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
    
}
