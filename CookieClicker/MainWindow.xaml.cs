using CookieClicker.assets;
using CookieClicker.investment.items;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookieClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The main window instance. Used for accessing the window from other classes.
        /// </summary>
        public static MainWindow Instance;

        /// <summary>
        /// The scale transform used for the cookie image.
        /// </summary>
        private readonly ScaleTransform scale = new ScaleTransform();

        public MainWindow()
        {
            Instance = this;
            Assets.Load();
            InitializeComponent();
        }

        public void ShowWindow(Grid window)
        {
            References.ALL_GRIDS.ForEach(g => g.Visibility = Visibility.Hidden);
            window.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Called when the window is loaded. Starts main game loop.
        /// </summary>
        private void Setup(object sender, RoutedEventArgs e)
        {
            //Set up the cookie image
            References.COOKIE_IMAGE.Source = Assets.COOKIE;

            //When changing the image width, make it scale in the center
            References.COOKIE_IMAGE.RenderTransformOrigin = new Point(0.5, 0.5);

            //Set up the cookie click event and the scaling animation
            References.COOKIE_IMAGE.RenderTransform = scale;

            References.COOKIE_IMAGE.MouseDown += (s, ev) => Cookie_Pressed();
            References.COOKIE_IMAGE.MouseUp += (s, ev) => Cookie_Released();
            References.COOKIE_IMAGE.MouseLeave += (s, ev) => Cookie_Released();

            //Set up shop buttons
            References.SHOP_BUTTON.Click += (s, ev) => ShowWindow(References.SHOP);
            References.CLOSE_BUTTON.Click += (s, ev) => ShowWindow(References.MAINWINDOW);

            //Add all investments
            GameCore.AddInvestment(new Cursor());
            GameCore.AddInvestment(new Grandma());
            GameCore.AddInvestment(new Farm());
            GameCore.AddInvestment(new Mine());
            GameCore.AddInvestment(new Factory());

            //Set up the game
            GameCore.Init();
        }

        /// <summary>
        /// Called when the window is closed. Stops the main game loop.
        /// </summary>
        private void Shutdown(object sender, EventArgs e)
        {
            GameCore.Shutdown();
        }

        private void Cookie_Pressed()
        {
            scale.ScaleX = 0.8;
            scale.ScaleY = 0.8;

            GameCore.AddCookies(1);
        }

        private void Cookie_Released()
        {
            scale.ScaleX = 1;
            scale.ScaleY = 1;
        }
    }
}
