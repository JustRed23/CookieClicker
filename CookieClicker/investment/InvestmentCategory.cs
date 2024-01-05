using CookieClicker.assets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookieClicker.investment
{
    internal class InvestmentCategory
    {
        private readonly Investment investment;

        private StackPanel panel;
        private Border border;

        private Image iconReference;

        public InvestmentCategory(Investment investment)
        {
            this.investment = investment;
        }

        public void OnBuy()
        {
            border.Visibility = Visibility.Visible;

            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.Orientation = Orientation.Vertical;
            wrapPanel.HorizontalAlignment = HorizontalAlignment.Center;
            wrapPanel.VerticalAlignment = VerticalAlignment.Center;

            if (iconReference == null)
            {
                //create a reference so we don't have to load the image every time
                iconReference = new Image();
                iconReference.Source = Assets.GetImage("investments/" + investment.name.ToLower() + ".png");
                iconReference.Width = panel.Height - 10;
                iconReference.Height = iconReference.Width;
                iconReference.Margin = new System.Windows.Thickness(2);
            }

            //create a copy of the reference
            Image icon = new Image();
            icon.Source = iconReference.Source;
            icon.Width = iconReference.Width;
            icon.Height = iconReference.Height;
            icon.Margin = iconReference.Margin;

            wrapPanel.Children.Add(icon);
            panel.Children.Add(wrapPanel);
        }

        public void Create(Panel parent)
        {
            if (panel != null) return;

            panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Height = 75;
            panel.Margin = new Thickness(0, 0, 0, 10);

            //set the panel background to a repeating image
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = Assets.GetImage("investments/" + investment.name.ToLower() + "-bg.png");
            brush.TileMode = TileMode.Tile;
            brush.Viewport = new Rect(0, 0, panel.Height, panel.Height);
            brush.ViewportUnits = BrushMappingMode.Absolute;
            panel.Background = brush;

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            scrollViewer.Content = panel;

            border = new Border();
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(3);
            border.Child = scrollViewer;
            border.Visibility = Visibility.Hidden;

            parent.Children.Add(border);
        }
    }
}
