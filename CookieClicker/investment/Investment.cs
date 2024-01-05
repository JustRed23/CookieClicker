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

        public int Multiplier = 1;

        protected double initialPrice = 0;
        protected double output = 1;

        public double cookiesPerSecond = 0;

        public Investment(string name, double initialPrice, double output)
        {
            this.name = name;
            this.button = new InvestmentButton(this);
            this.category = new InvestmentCategory(this);

            this.initialPrice = initialPrice;
            this.price = initialPrice;
            this.output = output;

            this.cookiesPerSecond = 100 * output;
        }

        public void Generate()
        {
            if (amount > 0) GameCore.AddCookies((amount * output) * Multiplier);
        }

        public void Update()
        {
            //Check if the button should be created
            if (!button.IsCreated() && initialPrice <= GameCore.TotalCookies)
            {
                button.Create(References.INVESTMENTS);
                category.Create(References.CATEGORIES);
            }

            button.Update();
        }

        //TODO
        public void Buy()
        {
            GameCore.RemoveCookies(price);
            amount++;
            price = Math.Round(initialPrice * Math.Pow(1.15, amount));
            category.OnBuy();
        }
    }
}
