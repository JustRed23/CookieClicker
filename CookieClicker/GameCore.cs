using CookieClicker.assets;
using CookieClicker.investment;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookieClicker
{
    /// <summary>
    /// The main game logic class.
    /// </summary>
    internal static class GameCore
    {
        public static double TotalCookies;
        public static double Cookies;
        public static double CPS;

        private static double previousCookies = 0;

        /// <summary>
        /// The list of all investments.
        /// </summary>
        private static readonly List<Investment> investments = new List<Investment>();

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

            TotalCookies = 0;
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
        public static void AddCookies(double amount, string sender = null)
        {
            if (timer == null) return;
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");

            Cookies += amount;
            TotalCookies += amount;

            UpdateComponents();

            if (sender != null) QuestManager.CheckProgress(ActionType.Generate, sender);
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
            
            //Try spawning a golden cookie every minute
            if (ticks != 0 && ticks % 6000 == 0)
            {
                if (new Random().Next(0, 100) < 30)
                {
                    References.MAINWINDOW.Dispatcher.Invoke(() =>
                    {
                        Random random = new Random();
                        Image goldenCookie = new Image();
                        goldenCookie.Source = Assets.GOLDEN_COOKIE;
                        goldenCookie.Width = 64;
                        goldenCookie.Height = 64;

                        //Add click event
                        goldenCookie.MouseLeftButtonDown += (s, e) =>
                        {
                            //Add 15m worth of CPS
                            AddCookies(CPS * 15 * 60);
                            References.GOLDENCOOKIE.Children.Remove(goldenCookie);
                        };

                        //Set random rotation
                        goldenCookie.RenderTransformOrigin = new Point(0.5, 0.5);
                        double rotation = random.Next(0, 360);
                        goldenCookie.RenderTransform = new RotateTransform(rotation);

                        //Spawn the cookie at a random location
                        double x = random.Next(0, (int)(MainWindow.Instance.Width - goldenCookie.Width));
                        double y = random.Next(0, (int)(MainWindow.Instance.Height - goldenCookie.Height));
                        Canvas.SetLeft(goldenCookie, x);
                        Canvas.SetTop(goldenCookie, y);

                        References.GOLDENCOOKIE.Children.Add(goldenCookie);
                    });
                }
            }

            //Tick particles
            CookieSpawner.Tick();

            //Generate cookies from investments
            investments.ForEach(i => i.Generate());

            //Calculate CPS every second
            if (ticks % 100 == 0)
            {
                CPS = Cookies - previousCookies;
                previousCookies = Cookies;

                UpdateComponents();

                QuestManager.CheckProgress(ActionType.CPS);
            }

            //Increment the tick counter, reset it if it's too high
            if (++ticks >= 100_000) ticks = 0;
        }

        public static void UpdateComponents()
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                double floor = Math.Floor(Cookies);
                References.COOKIES.Text = Formatter.FormatCookies(floor, null);
                References.CPS.Text = Formatter.FormatCookies(Math.Round(CPS, 2), "CPS");
                MainWindow.Instance.Title = "Cookie Clicker (" + Formatter.FormatCookies(floor, null) + ")";

                //Update the shop buttons
                investments.ForEach(i => i.Update());
            });
        }

        /// <summary>
        /// Get the current amount of a certain investment.
        /// </summary>
        /// <param name="investmentName">The investment</param>
        /// <returns>The amount of that investment or 0 if not found</returns>
        public static int GetAmount(string investmentName)
        {
            if (investmentName == "cookies") return (int)Cookies;

            foreach (Investment investment in investments)
            {
                if (investment.Name.ToLower() == investmentName.ToLower())
                {
                    return investment.Amount;
                }
            }

            return 0;
        }
    }
}
