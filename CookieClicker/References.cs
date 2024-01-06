using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace CookieClicker
{
    /// <summary>
    /// A utility class that contains references to all the controls in the window.
    /// </summary>
    internal static class References
    {
        //Main window
        public static Grid       MAINWINDOW    = MainWindow.Instance.MainGrid;
        public static Canvas     GOLDENCOOKIE  = MainWindow.Instance.GoldenCookieSpawner;
        public static TextBlock  BAKERYNAME    = MainWindow.Instance.TxtBakery;
        public static TextBlock  COOKIES       = MainWindow.Instance.TxtCookies;
        public static TextBlock  CPS           = MainWindow.Instance.TxtCPS;
        public static Image      COOKIE_IMAGE  = MainWindow.Instance.ImgCookie;
        public static Button     SHOP_BUTTON   = MainWindow.Instance.BtnShop;
        public static Button     QUESTS_BUTTON = MainWindow.Instance.BtnQuests;

        //Shop
        public static Grid       SHOP          = MainWindow.Instance.ShopGrid;
        public static Button     CLOSE_SHOP    = MainWindow.Instance.BtnCloseShop;
        public static StackPanel INVESTMENTS   = MainWindow.Instance.Investments;
        public static StackPanel CATEGORIES    = MainWindow.Instance.Categories;

        //Quests
        public static Grid       QUESTS        = MainWindow.Instance.QuestGrid;
        public static Button     CLOSE_QUESTS  = MainWindow.Instance.BtnCloseQuests;
        public static StackPanel QUEST_LIST    = MainWindow.Instance.Quests;

        /// <summary>
        /// Grids, used for hiding/showing multiple controls at once
        /// </summary>
        public static List<Grid> ALL_GRIDS     = new Grid[]{ MAINWINDOW, SHOP, QUESTS }.ToList();
    }
}
