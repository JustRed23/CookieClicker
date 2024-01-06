using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace CookieClicker.assets
{
    /// <summary>
    /// A basic texture pack system, loads textures from the assets folder in their respective texture pack folder
    /// </summary>
    internal static class Assets
    {
        private static readonly string PACK_URL = "pack://application:,,,";
        private static string currentTexturePack = "default";

        public static BitmapImage BACKGROUND;
        public static BitmapImage COOKIE;
        public static BitmapImage GOLDEN_COOKIE;

        public static BitmapImage CLOSE_ICON;

        /// <summary>
        /// Loads the default texture pack
        /// </summary>
        public static void Load()
        {
            Load(currentTexturePack);
        }

        /// <summary>
        /// Loads a texture pack
        /// </summary>
        /// <param name="texturePack">The name of the texture pack, THIS SHOULD BE THE SAME AS THE FOLDER NAME UNDER THE ASSETS DIRECTORY!</param>
        public static void Load(string texturePack)
        {
            currentTexturePack = texturePack;

            BACKGROUND = LoadImageOrDefault("bg.png");
            COOKIE = LoadImageOrDefault("cookie.png");
            GOLDEN_COOKIE = LoadImageOrDefault("golden_cookie.png");

            CLOSE_ICON = LoadImageOrDefault("close.png");
        }

        /// <summary>
        /// Gets an image from the current texture pack
        /// </summary>
        /// <param name="image">The full image name including it's extension</param>
        /// <returns>The image if found, or null if not</returns>
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
                Debug.WriteLine($"Could not find image {image} in texture pack {currentTexturePack}. Returning null.");
                return null;
            }
        }
    }
}
