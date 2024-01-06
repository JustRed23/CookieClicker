using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookieClicker
{
    /// <summary>
    /// Handles everything related to quests.
    /// </summary>
    internal static class QuestManager
    {
        private static readonly List<Quest> quests = new List<Quest>();
        private static int completeCount = 0;

        /// <summary>
        /// Loads the quests from the embedded quests.json file.
        /// </summary>
        public static void Load()
        {
            quests.Clear();

            string json = ReadEmbeddedJson("CookieClicker.assets.quests.json");

            JsonNode jsonNode = JsonNode.Parse(json);
            JsonArray questsArray = (JsonArray)jsonNode["quests"];

            foreach (JsonObject questObject in questsArray)
            {
                string name = (string)questObject["name"];
                string description = (string)questObject["description"];
                string goal = (string)questObject["goal"];

                quests.Add(new Quest(name, description, goal));
            }

            Debug.WriteLine($"Loaded {quests.Count} quests.");
        }

        /// <summary>
        /// Checks if the given action has completed any quests.
        /// </summary>
        /// <param name="type">The action type</param>
        /// <param name="sender">(Optional) the sender, this would represent the investment on the Buy/Generate action</param>
        /// <param name="count">(Optional) the current amount of the investment</param>
        public static void CheckProgress(ActionType type, string sender = null, int count = -1)
        {
            if (sender != null) sender = sender.ToLower();

            switch (type)
            {
                case ActionType.CPS:
                    CheckCPSProgress();
                    break;
                case ActionType.Generate:
                    CheckGenerateProgress(sender);
                    break;
                case ActionType.Buy:
                    CheckBuyProgress(sender, count);
                    break;
            }   
        }

        private static void CheckCPSProgress()
        {
            double cps = GameCore.CPS;

            foreach (Quest quest in quests)
            {
                if (quest.Completed) continue;
                if (quest.Goal.StartsWith("cps"))
                {
                    string[] split = quest.Goal.Split('>');
                    double goal = double.Parse(split[1]);

                    if (cps >= goal) quest.Complete(++completeCount);
                }
            }
        }

        private static void CheckGenerateProgress(string sender)
        {
            foreach (Quest quest in quests)
            {
                if (quest.Completed) continue;
                if (quest.Goal.StartsWith(sender))
                {
                    string[] split = quest.Goal.Split('>');
                    int goal = int.Parse(split[1]);

                    if (sender == split[0] && GameCore.GetAmount(sender) >= goal) quest.Complete(++completeCount);
                }
            }
        }

        private static void CheckBuyProgress(string sender, int count)
        {
            foreach (Quest quest in quests)
            {
                if (quest.Completed) continue;
                if (quest.Goal.StartsWith(sender))
                {
                    string[] split = quest.Goal.Split('>');
                    int goal = int.Parse(split[1]);

                    if (sender == split[0] && count >= goal) quest.Complete(++completeCount);
                }
            }
        }

        private static string ReadEmbeddedJson(string file)
        {
            Debug.WriteLine($"Reading embedded json file: {file}");

            string result = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }

    /// <summary>
    /// The type of action that was performed.
    /// </summary>
    internal enum ActionType
    {
        Buy,
        CPS,
        Generate
    }

    /// <summary>
    /// Represents a quest.
    /// </summary>
    internal class Quest
    {
        public string Name { get; }
        public string Description { get; }
        public string Goal { get; }

        public bool Completed { get; private set; }

        public Quest(string name, string description, string goal)
        {
            Name = name;
            Description = description;
            Goal = goal;
        }

        public void Complete(int completeCount)
        {
            Completed = true;
            MessageBox.Show(Description, $"Quest completed: {Name}", MessageBoxButton.OK, MessageBoxImage.Information);

            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                WrapPanel combined = new WrapPanel();
                combined.Orientation = Orientation.Horizontal;
                combined.Margin = new Thickness(0, 0, 0, 5);
                combined.Background = new SolidColorBrush(Color.FromRgb(101, 67, 33));
                combined.HorizontalAlignment = HorizontalAlignment.Stretch;
                combined.VerticalAlignment = VerticalAlignment.Center;

                TextBlock number = new TextBlock() { Text = completeCount.ToString(), FontSize = 28, FontWeight = FontWeights.Bold, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(10, 0, 10, 0) };

                Border border = new Border();
                border.BorderThickness = new Thickness(0, 0, 2, 0);
                border.CornerRadius = new CornerRadius(0, 0, 10, 0);
                border.BorderBrush = MainWindow.Instance.Foreground;
                border.Child = number;
                combined.Children.Add(border);

                WrapPanel panel = new WrapPanel();
                panel.Margin = new Thickness(5, 0, 0, 0);
                panel.VerticalAlignment = VerticalAlignment.Center;
                panel.Orientation = Orientation.Vertical;
                panel.Children.Add(new TextBlock() { Text = Name, FontSize = 20, FontWeight = FontWeights.Bold });
                panel.Children.Add(new TextBlock() { Text = Description, FontSize = 14, TextWrapping = TextWrapping.WrapWithOverflow });
                combined.Children.Add(panel);

                References.QUEST_LIST.Children.Add(combined);
            });
        }
    }
}
