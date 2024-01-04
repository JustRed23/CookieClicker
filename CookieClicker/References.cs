using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CookieClicker
{
    /// <summary>
    /// A utility class that contains references to all the controls in the window.
    /// </summary>
    internal static class References
    {
        public static TextBlock COOKIES      = MainWindow.Instance.TxtCookies;
        public static TextBlock CPS          = MainWindow.Instance.TxtCPS;
        public static Image     COOKIE_IMAGE = MainWindow.Instance.ImgCookie;
        public static Button    SHOP_BUTTON  = MainWindow.Instance.BtnShop;
    }
}
