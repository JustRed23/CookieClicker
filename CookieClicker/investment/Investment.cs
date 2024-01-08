using System;

namespace CookieClicker.investment
{
    /// <summary>
    /// Represents an investment that can be bought
    /// </summary>
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

        /// <summary>
        /// The total cookies produced by this investment
        /// </summary>
        public double TotalOfType = 0;

        /// <summary>
        /// The amount of which the multiplier price increases, starts at 100, then 500, then increases by a factor of 10
        /// </summary>
        private long multiplierPriceMultiplier = 100;

        public double InitialCookiesPerSecond = 0;
        public double CurrentCookiesPerSecond = 0;

        public Investment(string name, double initialPrice, double output)
        {
            this.Name = name;
            this.button = new InvestmentButton(this);
            this.category = new InvestmentCategory(this);

            this.initialPrice = initialPrice;
            this.Price = initialPrice;
            this.output = output;

            this.InitialCookiesPerSecond = 100 * output;
        }

        /// <summary>
        /// Generates 10ms worth of cookies
        /// </summary>
        public void Generate()
        {
            if (Amount > 0)
            {
                double generatedAmount = (Amount * output) * Multiplier;
                GameCore.AddCookies(generatedAmount, Name);
                TotalOfType += generatedAmount;
                CurrentCookiesPerSecond = 100 * (Amount * output);
            }
        }

        /// <summary>
        /// Updates UI elements
        /// </summary>
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
