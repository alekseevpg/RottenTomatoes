using System.Collections.Generic;
using MonoTouch.UIKit;

namespace RottenTomatoes
{
    public static class Images
    {
        private static Dictionary<string, UIImage> images = new Dictionary<string, UIImage>();

        public static UIImage Get(string path)
        {
            if (images.ContainsKey(path))
                return images[path];
            else
            {
                var image = UIImage.FromFile(path);
                images.Add(path, image);
                return image;
            }
        }
    }
}

