using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace CookieClicker.investment
{
    internal abstract class Investment
    {
        private string name;

        protected int amount = 0;
        protected double initialPrice = 0;
        protected double price = 0;

        protected double multiplier = 1;

        public Investment(string name, double initialPrice, double multiplier)
        {
            this.name = name;
            this.initialPrice = initialPrice;
            this.price = initialPrice;
            this.multiplier = multiplier;
        }

        public void Generate()
        {
            GameCore.AddCookies(amount * multiplier);
        }

        //TODO
        private void Buy()
        {
            GameCore.RemoveCookies(price);
            amount++;
            price = Math.Round(initialPrice * Math.Pow(1.15, amount));
        }
    }
}
