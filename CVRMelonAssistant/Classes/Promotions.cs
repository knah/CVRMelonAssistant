namespace CVRMelonAssistant
{
    class Promotions
    {
        public static Promotion[] ActivePromotions =
        {
            new Promotion
            {
                ModName = "emmVRCLoader",
                Text = "Join our Discord!",
                Link = "https://discord.gg/emmvrc"
            }
        };
    }

    class Promotion
    {
        public string ModName;
        public string Text;
        public string Link;
    }
}
