using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Windows;

namespace CookieClicker
{
    internal static class QuestManager
    {
        private static readonly List<Quest> quests = new List<Quest>();

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

                    if (cps >= goal) quest.Complete();
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

                    if (sender == split[0] && GameCore.GetAmount(sender) >= goal) quest.Complete();
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

                    if (sender == split[0] && count >= goal) quest.Complete();
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

    internal enum ActionType
    {
        Buy,
        CPS,
        Generate
    }

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

        public void Complete()
        {
            Completed = true;
            MessageBox.Show(Description, $"Quest completed: {Name}", MessageBoxButton.OK, MessageBoxImage.Information);

            //TODO: quest history
        }
    }
}
