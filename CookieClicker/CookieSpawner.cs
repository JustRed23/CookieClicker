using CookieClicker.assets;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookieClicker
{
    internal static class CookieSpawner
    {

        private static readonly List<Cookie> cookies = new List<Cookie>();

        private static readonly List<Cookie> pendingAdd = new List<Cookie>();
        private static readonly List<Cookie> pendingRemoval = new List<Cookie>();

        private static bool stopping = false;

        public static void Spawn()
        {
            if (cookies.Count >= Cookie.MAX_ON_SCREEN) return;
            pendingAdd.Add(new Cookie());
        }

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

        public static void Stop()
        {
            stopping = true;
        }

        internal static void Remove(Cookie cookie)
        {
            pendingRemoval.Add(cookie);
        }
    }

    internal class Cookie
    {
        public static readonly int MAX_ON_SCREEN = 50;
        private readonly int LIFETIME = 200;

        private static readonly int WIDTH = 30;

        private int ticks = 0;

        private double xPosition;
        private double yPosition = -WIDTH;

        private Image image;

        public Cookie()
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

        public void Tick()
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
