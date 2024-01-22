using Discord.Interactions;
using DiscordBot.SlashCommands.Enums;

namespace DiscordBot.SlashCommands
{
    
    public class RandomClassSlashModule:InteractionModuleBase<SocketInteractionContext>
    {
        private readonly string[] _uh = new string[]
        {
            "Yumiyacha", "Taterazay", "Yarida", "Kibadda", "Destrobo", "Piekron", "Wooyari", "Pyokorider", "Cannassault", "Charibasa",
            "Guardira", "Tondenga", "Myamsar", "Bowmunk", "Grenburr", "Alosson", "Wondabarappa", "Jamsch", "Oohoroc", "Pingrek", "Cannogabang"
        };
        private readonly string[] _dh = new string[]
        {
            "Ravenous", "Sonarchy", "Ragewolf", "Naughtyfins", "Slogturtle", "Covet-Hiss", "Buzzcrave"
        };
        private readonly Random _random = new Random();
        private readonly string[] _heroes;
        public RandomClassSlashModule()
        {
            _heroes = _uh.Concat(_dh).ToArray();
        }

        [SlashCommand("random", "Recommends Random class.")]
        public async Task GetRandomClass(ClassGroup type)
        {
            string result;
            switch (type)
            {
                case ClassGroup.Any:
                    result = GetRandom(_heroes);
                    break;
                case ClassGroup.Uberhero:
                    result = GetRandom(_uh);
                    break;
                case ClassGroup.Darkhero:
                    result = GetRandom(_dh);
                    break;
                default:
                    await RespondAsync("Wrong parameter.");
                    return;
            }
            await RespondAsync($"My{((type==ClassGroup.Any)?"":$" *{type.ToString()}*")} choice is **{result}**!");
        }
        private string GetRandom(string[] data) => data[_random.Next(0, data.Length)];
    }
}
