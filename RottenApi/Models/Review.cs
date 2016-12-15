using System;
using System.Collections.Generic;

namespace RottenApi
{

    public class Review
    {
        public string Critic { get; set; }

        public string Freshness { get; set; }

        public string Publication { get; set; }

        public string Quote { get; set; }

        public Link Links { get; set; }
    }
    
}
