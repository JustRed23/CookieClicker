using CookieClicker.investment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Threading;

namespace CookieClicker
{
    /// <summary>
    /// The main game logic class.
    /// </summary>
    internal static class GameCore
    {
        public static double Cookies;
        public static double CPS;

        private static double previousCookies = 0;

        /// <summary>
        /// The list of all investments.
        /// </summary>
        private static List<Investment> investments = new List<Investment>();

        /// <summary>
        /// The main game loop timer.
        /// </summary>
        private static Timer timer;

        /// <summary>
        /// The amount of ticks that have passed since the game started.
        /// </summary>
        private static int ticks = 0;

        /// <summary>
        /// Initializes the game.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the game has already been initialized.</exception>
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

        /// <summary>
        /// Shuts down the game.
        /// </summary>
        public static void Shutdown()
        {
            if (timer == null) return;

            timer.Stop();
            timer.Dispose();
            timer = null;
        }

        public static void AddInvestment(Investment investment)
        {
            investments.Add(investment);
        }

        /// <summary>
        /// Adds cookies to the total amount of cookies.
        /// </summary>
        /// <param name="amount">The amount of cookies to add</param>
        public static void AddCookies(double amount)
        {
            if (timer == null) return;
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");

            Cookies += amount;
            UpdateComponents();
        }

        /// <summary>
        /// Removes cookies to the total amount of cookies.
        /// </summary>
        /// <param name="amount">The amount of cookies to remove</param>
        public static void RemoveCookies(double amount)
        {
            if (timer == null) return;
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");

            Cookies -= amount;
            UpdateComponents();
        }

        private static void Update()
        {
            //1 tick = 10ms

            //Generate cookies from investments
            investments.ForEach(i => i.Generate());

            //Calculate CPS every second
            if (ticks % 100 == 0)
            {
                CPS = Cookies - previousCookies;
                previousCookies = Cookies;

                UpdateComponents();
            }

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
