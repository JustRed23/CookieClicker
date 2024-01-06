using CookieClicker.assets;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookieClicker
{
    /// <summary>
    /// Particle system that spawns cookies at random locations on the screen
    /// </summary>
    internal static class CookieSpawner
    {

        private static readonly List<Cookie> cookies = new List<Cookie>();

        private static readonly List<Cookie> pendingAdd = new List<Cookie>();
        private static readonly List<Cookie> pendingRemoval = new List<Cookie>();

        /// <summary>
        /// Adds a cookie to the list of cookies to spawn
        /// </summary>
        public static void Spawn()
        {
            if (cookies.Count >= Cookie.MAX_ON_SCREEN) return;
            pendingAdd.Add(new Cookie());
        }

        /// <summary>
        /// Ticks the particle system, spawning cookies and removing them when they exceed their lifetime
        /// </summary>
        public static void Tick()
        {
            if (pendingAdd.Count > 0)
            {
                cookies.AddRange(pendingAdd);
                pendingAdd.Clear();
            }

            //create a copy of the list to prevent concurrency issues
            List<Cookie> copy = new List<Cookie>(cookies);
            copy.ForEach(c => c.Tick());

            if (pendingRemoval.Count == 0) return;
            pendingRemoval.ForEach(c => cookies.Remove(c));
            pendingRemoval.Clear();
        }

        internal static void Remove(Cookie cookie)
        {
            pendingRemoval.Add(cookie);
        }
    }

    /// <summary>
    /// A cookie that is spawned by the particle system
    /// </summary>
    internal class Cookie
    {
        /// <summary>
        /// The maximum amount of cookies that can be on screen at once
        /// </summary>
        public static readonly int MAX_ON_SCREEN = 50;
        private readonly int LIFETIME = 200;

        private static readonly int WIDTH = 30;

        private int ticks = 0;

        private double xPosition;
        private double yPosition = -WIDTH;

        private Image image;

        internal Cookie()
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                this.xPosition = new Random().Next(0, (int)MainWindow.Instance.Width - WIDTH);

                image = new Image();
                image.Source = Assets.COOKIE;
                image.Width = WIDTH;
                image.Height = WIDTH;

                //Set random rotation
                image.RenderTransformOrigin = new Point(0.5, 0.5);
                image.RenderTransform = new RotateTransform(new Random().Next(0, 360));

                Redraw();

                References.PARTICLES.Children.Add(image);
            });
        }

        internal void Tick()
        {
            if (ticks++ >= LIFETIME)
            {
                Redraw(true);
                CookieSpawner.Remove(this);
                return;
            }

            yPosition += 0.5;

            Redraw();
        }

        private void Redraw(bool remove = false)
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                if (remove)
                {
                    References.PARTICLES.Children.Remove(image);
                    return;
                }

                image.Opacity = 1 - (ticks / (double) LIFETIME);

                Canvas.SetLeft(image, xPosition);
                Canvas.SetTop(image, yPosition);
            });
        }
    }
}
