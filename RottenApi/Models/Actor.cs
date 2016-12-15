using System;
using System.Collections.Generic;

namespace RottenApi
{

    public class Actor : Human
    {
        public string Id { get; set; }

        public List<string> Characters { get; set; }

        public string GetFormattedChars()
        {
            return string.Join(", ", Characters.ToArray());
        }
    }
    
}
