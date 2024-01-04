using System;

namespace CookieClicker.investment
{
    internal abstract class Investment
    {
        private readonly InvestmentButton button;
        private readonly InvestmentCategory category;

        public string name;
        public int amount = 0;
        public double price = 0;

        protected double initialPrice = 0;
        protected double multiplier = 1;

        public double cookiesPerSecond = 0;

        public Investment(string name, double initialPrice, double multiplier)
        {
            this.name = name;
            this.button = new InvestmentButton(this);
            this.category = new InvestmentCategory(this);

            this.initialPrice = initialPrice;
            this.price = initialPrice;
            this.multiplier = multiplier;

            this.cookiesPerSecond = 100 * multiplier;
        }

        public void Create()
        {
            button.Create(References.INVESTMENTS);
            category.Create(References.CATEGORIES);
        }

        public void Generate()
        {
            if (amount > 0) GameCore.AddCookies(amount * multiplier);
        }

        public void Update()
        {
            button.Update();
            category.Update();
        }

        //TODO
        public void Buy()
        {
            GameCore.RemoveCookies(price);
            amount++;
            price = Math.Round(initialPrice * Math.Pow(1.15, amount));
        }
    }
}
