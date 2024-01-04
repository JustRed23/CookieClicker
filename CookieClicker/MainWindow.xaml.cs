using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        private ScaleTransform scale = new ScaleTransform();

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
        }

        /// <summary>
        /// Called when the window is loaded. Starts main game loop.
        /// </summary>
        private void Setup(object sender, RoutedEventArgs e)
        {
            //When changing the image width, make it scale in the center
            References.COOKIE_IMAGE.RenderTransformOrigin = new Point(0.5, 0.5);

            //Set up the cookie click event and the scaling animation
            References.COOKIE_IMAGE.RenderTransform = scale;

            References.COOKIE_IMAGE.MouseDown += (s, ev) => Cookie_Pressed();
            References.COOKIE_IMAGE.MouseUp += (s, ev) => Cookie_Released();
            References.COOKIE_IMAGE.MouseLeave += (s, ev) => Cookie_Released();

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

            GameCore.CookieClicked();
        }

        private void Cookie_Released()
        {
            scale.ScaleX = 1;
            scale.ScaleY = 1;
        }
    }
}
