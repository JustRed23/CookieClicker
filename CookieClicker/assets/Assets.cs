using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace CookieClicker.assets
{
    internal static class Assets
    {
        private static readonly string PACK_URL = "pack://application:,,,";
        private static string currentTexturePack = "default";

        public static BitmapImage COOKIE;
        public static BitmapImage SHOP;

        public static void Load()
        {
            Load(currentTexturePack);
        }

        public static void Load(string texturePack)
        {
            currentTexturePack = texturePack;

            COOKIE = LoadImageOrDefault("cookie.png");
            //SHOP = LoadImageOrDefault("shop.png");
        }

        public static BitmapImage GetImage(string image)
        {
            return LoadImageOrNull(image);
        }

        private static BitmapImage LoadImageOrDefault(string image)
        {
            BitmapImage bitmapImage = LoadImageOrNull(image);
            if (bitmapImage == null)
            {
                Debug.WriteLine($"Could not find image {image} in texture pack {currentTexturePack}. Loading default image.");
                bitmapImage = new BitmapImage(new Uri($"{PACK_URL}/assets/default/{image}"));
            }
            return bitmapImage;
        }

        private static BitmapImage LoadImageOrNull(string image)
        {
            try
            {
                return new BitmapImage(new Uri($"{PACK_URL}/assets/{currentTexturePack}/{image}"));
            } catch (IOException) {
                return null;
            }
        }
    }
}
