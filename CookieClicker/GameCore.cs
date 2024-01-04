using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Threading;

namespace CookieClicker
{
    internal static class GameCore
    {
        public static int Cookies;
        public static double CPS;

        /// <summary>
        /// The main game loop timer.
        /// </summary>
        private static Timer timer;

        /// <summary>
        /// The amount of ticks that have passed since the game started.
        /// </summary>
        private static int ticks = 0;

        public static void Init()
        {
            if (timer != null) throw new InvalidOperationException("Game has already been initialized.");

            Cookies = 0;
            CPS = 0;

            References.COOKIES.Text = Cookies.ToString();
            References.CPS.Text = CPS.ToString();

            ticks = 0;
            timer = new Timer(10);
            timer.Elapsed += (s, e) => Update();
            timer.Start();
        }

        public static void Shutdown()
        {
            timer.Stop();
            timer.Dispose();
            timer = null;
        }

        public static void CookieClicked()
        {
            if (timer == null) return;

            Cookies++;
            UpdateComponents();
        }

        private static void Update()
        {
            //1 tick = 10ms

            //Update all components, we only do this every second to save performance
            if (ticks % 100 == 0) UpdateComponents();

            //Increment the tick counter, reset it if it's too high
            if (++ticks >= 100_000) ticks = 0;
        }

        private static void UpdateComponents()
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                References.COOKIES.Text = Cookies.ToString();
                References.CPS.Text = CPS.ToString();
            });
        }
    }
}
