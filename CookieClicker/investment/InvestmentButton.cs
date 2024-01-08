using CookieClicker.assets;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookieClicker.investment
{
    internal class InvestmentButton
    {
        private readonly Investment investment;

        private DockPanel panel;
        private ToolTip toolTip;
        private TextBlock count;
        private TextBlock price;

        private TextBlock multiplierText;
        private TextBlock multiplierPrice;

        public InvestmentButton(Investment investment)
        {
            this.investment = investment;
        }

        public void Update()
        {
            if (!IsCreated()) return;

            count.Text = investment.Amount.ToString();
            price.Text = Formatter.FormatCookies(investment.Price, null);

            bool hasEnough = investment.Price <= GameCore.Cookies;
            price.Foreground = hasEnough ? count.Foreground : Brushes.Red;

            multiplierText.Text = "x" + investment.Multiplier;
            multiplierPrice.Text = Formatter.FormatCookies(investment.GetMultiplierPrice(), "");

            bool hasEnoughMultiplier = investment.GetMultiplierPrice() <= GameCore.Cookies;
            multiplierPrice.Foreground = hasEnoughMultiplier ? multiplierText.Foreground : Brushes.Red;

            UpdateTooltip();
        }

        private void UpdateTooltip()
        {
            StringBuilder tt = new StringBuilder();
            tt.AppendLine($"Each {investment.Name} produces {Formatter.FormatCookies(investment.InitialCookiesPerSecond, null)} per second");
            tt.AppendLine($"{investment.Amount} x {investment.Name} procudes {Formatter.FormatCookies(investment.CurrentCookiesPerSecond, null)} per second");
            tt.AppendLine($"With a multiplier of x{investment.Multiplier}, each {investment.Name} produces {Formatter.FormatCookies(investment.CurrentCookiesPerSecond * investment.Multiplier, null)} per second");
            tt.AppendLine($"This investment has generated a total of {Formatter.FormatCookies(Math.Round(investment.TotalOfType, 2), "cookies")}");

            toolTip.Content = tt.ToString();
        }

        public void Create(Panel parent)
        {
            if (IsCreated()) return;

            panel = new DockPanel();
            panel.Background = new SolidColorBrush(Color.FromRgb(101, 67, 33));
            panel.Height = 60;
            panel.MouseLeftButtonDown += (s, e) =>
            {
                if (investment.Price <= GameCore.Cookies) investment.Buy();
            };

            toolTip = new ToolTip();
            UpdateTooltip();
            panel.ToolTip = toolTip;

            Image image = new Image();
            image.Source = Assets.GetImage("investments/" + investment.Name.ToLower() + ".png");
            image.Width = panel.Height - 5;
            image.Height = panel.Height - 5;
            image.Margin = new Thickness(2);
            DockPanel.SetDock(image, Dock.Left);
            panel.Children.Add(image);

            //info start
            StackPanel info = new StackPanel();
            info.Orientation = Orientation.Vertical;
            info.Margin = new Thickness(5);

            TextBlock name = new TextBlock();
            name.Text = investment.Name;
            name.FontSize = 20;
            info.Children.Add(name);

            price = new TextBlock();
            price.Text = Formatter.FormatCookies(investment.Price, null);
            price.FontSize = 15;
            info.Children.Add(price);

            panel.Children.Add(info);
            //info end

            count = new TextBlock();
            count.Text = "0";
            count.FontSize = 32;
            count.Margin = new Thickness(5);
            count.HorizontalAlignment = HorizontalAlignment.Right;
            count.VerticalAlignment = VerticalAlignment.Center;
            DockPanel.SetDock(count, Dock.Right);
            panel.Children.Add(count);

            //multiplier start
            StackPanel multiplier = new StackPanel();
            multiplier.Orientation = Orientation.Vertical;
            multiplier.Margin = new Thickness(5, 0, 0, 0);
            multiplier.MinWidth = panel.Height;
            multiplier.Background = panel.Background;

            multiplier.MouseLeftButtonDown += (s, e) =>
            {
                if (investment.GetMultiplierPrice() <= GameCore.Cookies) investment.BuyMultiplier();
            };

            multiplierText = new TextBlock();
            multiplierText.Text = "x" + investment.Multiplier;
            multiplierText.FontSize = 22;
            multiplierText.HorizontalAlignment = HorizontalAlignment.Center;
            multiplierText.VerticalAlignment = VerticalAlignment.Center;
            multiplier.Children.Add(multiplierText);

            multiplierPrice = new TextBlock();
            multiplierPrice.Text = Formatter.FormatCookies(investment.GetMultiplierPrice(), "");
            multiplierPrice.FontSize = 12;
            multiplierPrice.HorizontalAlignment = HorizontalAlignment.Center;
            multiplierPrice.VerticalAlignment = VerticalAlignment.Center;
            multiplier.Children.Add(multiplierPrice);
            //multiplier end

            DockPanel combined = new DockPanel();
            combined.Margin = new Thickness(0, 0, 0, 5);
            DockPanel.SetDock(multiplier, Dock.Right);
            combined.Children.Add(multiplier);
            combined.Children.Add(panel);

            parent.Children.Add(combined);
        }

        public bool IsCreated()
        {
            return panel != null;
        }
    }
}
