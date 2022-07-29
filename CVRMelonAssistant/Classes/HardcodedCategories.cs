using System.Collections.Generic;

namespace CVRMelonAssistant
{
    public static class HardcodedCategories
    {
        private static readonly Dictionary<string, string> CategoryDescriptions = new()
        {
            {"Safety & Security", "Crash less, block annoyances"},
            {"Core mods and libraries", "Other mods might require these"},
            {"All-in-one mods", "It does a lot of stuff"},
            {"Camera mods", "For all your screenshot or streaming needs"},
            {"Performance & Fidelity", "Improve performance or make the game look better"},
            {"Utilities & Tweaks", "Small mods that address specific issues"},
            {"Hardware support", "For all exotic hardware out there"},
            {"Dynamic bones", "Mods that affect jiggly bits"},
            {"World tweaks", "Change aspects of the world you're in"},
            {"Fixes", "It's not a bug, it's a feature"},
            {"New features & Overhauls", "Mods that introduce new features or significantly change existing ones"},
            {"UI mods", "Modify the user interface or introduce new functionality to it"},
            {"Movement", "Move in new exciting ways"},
            {"Very Niche Mods", "Only use these if you're really sure you need them"}
        };

        public static string GetCategoryDescription(string category)
        {
            return CategoryDescriptions.TryGetValue(category, out var result) ? result : "";
        }

        private static List<(string Original, string Replace)> ourAuthorReplaces =
            new()
            {
                ("<@!170953680718266369>", "ImTiara"),
                ("<@!286669951987613706>", "Rafa"),
                ("<@!168795588366696450>", "Grummus"),
                ("<@!167335587488071682>", "KortyBoi/Lily"),
                ("<@!127978642981650432>", "tetra"),
                ("<@!155396491853168640>", "Dawn/arion")
            };

        public static string FixupAuthor(string authorName)
        {
            if (string.IsNullOrEmpty(authorName) || !authorName.Contains("@")) return authorName;

            foreach (var authorReplace in ourAuthorReplaces)
                authorName = authorName.Replace(authorReplace.Original, authorReplace.Replace);

            return authorName;
        }
    }
}
