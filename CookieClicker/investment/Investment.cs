using System;

namespace CookieClicker.investment
{
    internal abstract class Investment
    {
        private readonly InvestmentButton button;
        private readonly InvestmentCategory category;

        public string Name;
        public int Amount = 0;
        public double Price = 0;

        public int Multiplier = 1;

        protected double initialPrice = 0;
        protected double output = 1;

        private long multiplierPriceMultiplier = 100;

        public double CookiesPerSecond = 0;

        public Investment(string name, double initialPrice, double output)
        {
            this.Name = name;
            this.button = new InvestmentButton(this);
            this.category = new InvestmentCategory(this);

            this.initialPrice = initialPrice;
            this.Price = initialPrice;
            this.output = output;

            this.CookiesPerSecond = 100 * output;
        }

        public void Generate()
        {
            if (Amount > 0) GameCore.AddCookies((Amount * output) * Multiplier, Name);
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

        public void Buy()
        {
            GameCore.RemoveCookies(Price);
            Amount++;
            Price = Math.Round(initialPrice * Math.Pow(1.15, Amount));
            category.OnBuy();

            QuestManager.CheckProgress(ActionType.Buy, Name, Amount);
        }

        public void BuyMultiplier()
        {
            GameCore.RemoveCookies(GetMultiplierPrice());
            Multiplier *= 2;

            if (multiplierPriceMultiplier == 100)
                multiplierPriceMultiplier = 500;
            else multiplierPriceMultiplier *= 10;

            GameCore.UpdateComponents();
        }

        public double GetMultiplierPrice()
        {
            return initialPrice * multiplierPriceMultiplier;
        }
    }
}
