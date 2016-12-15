using System;
using System.Collections.Generic;

namespace RottenApi
{

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
    
}
