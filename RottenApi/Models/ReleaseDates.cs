using System;
using System.Collections.Generic;

namespace RottenApi
{

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
    
}
