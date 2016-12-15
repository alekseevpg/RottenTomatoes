using System.Collections.Generic;
using UIKit;

namespace RottenTomatoes.Helpers.Helpers
{
    public static class Images
    {
        private static readonly Dictionary<string, UIImage> images = new Dictionary<string, UIImage>();

        public static UIImage Get(string path)
        {
            if (images.ContainsKey(path))
                return images[path];
            var image = UIImage.FromFile(path);
            images.Add(path, image);
            return image;
        }
    }
}