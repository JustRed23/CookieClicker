﻿using CookieClicker.assets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookieClicker.investment
{
    internal class InvestmentButton
    {
        private readonly Investment investment;

        private DockPanel panel;
        private TextBlock count;
        private TextBlock price;

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
        }

        public void Create(Panel parent)
        {
            if (IsCreated()) return;

            panel = new DockPanel();
            panel.Margin = new Thickness(0, 0, 0, 5);
            panel.Background = new SolidColorBrush(Color.FromRgb(101, 67, 33));
            panel.MouseLeftButtonDown += (s, e) =>
            {
                if (investment.Price <= GameCore.Cookies) investment.Buy();
            };

            ToolTip toolTip = new ToolTip();
            toolTip.Content = $"Each {investment.Name} produces {Formatter.FormatCookies(investment.CookiesPerSecond, null)} per second";
            panel.ToolTip = toolTip;

            Image image = new Image();
            image.Source = Assets.GetImage("investments/" + investment.Name.ToLower() + ".png");
            image.Width = 50;
            image.Height = 50;
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

            parent.Children.Add(panel);
        }

        public bool IsCreated()
        {
            return panel != null;
        }
    }
}
