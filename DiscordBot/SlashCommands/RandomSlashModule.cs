using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands
{ 
    [HelpGroup("Patapon 3")]
    [Group("random", "Randomizes Patapon class.")]
    public class RandomSlashModule:InteractionModuleBase<SocketInteractionContext>
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
        private readonly string[] _elements;
        private readonly IEnumerable<MissionInfo> _missions;
        public RandomSlashModule(IDB db)
        {
            _heroes = _uh.Concat(_dh).ToArray();
            _elements = Enum.GetNames(typeof(AttackElement));
            if (db.TryGetTable<MissionInfo>(out var info) && info!=null) _missions = info.Select(v => v.Value);
        }

        [SlashCommand("class", "Suggests random class.")]
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
            await RespondAsync($"My{((type == ClassGroup.Any) ? "" : $" *{type}*")} choice is **{result}**!");
        }
        [SlashCommand("elemental", "Suggests random elemental type.")]
        public async Task GetRandomElementalType() =>
            await RespondAsync($"My *elemental type* choice is **{GetRandom(_elements)}**!");
        private string GetRandom(string[] data) => data[_random.Next(0, data.Length)];
        private MissionInfo GetRandom(MissionInfo[] data) => data[_random.Next(0, data.Length)];
        [SlashCommand("mission", "Suggests random mission.")]
        public async Task GetRandomMission(MissionType missionTypes)
        {
            if (_missions == null)
            {
                await RespondAsync("Cannot find DB.");
                return;
            }
            var pool = _missions.Where(m => missionTypes.HasFlag(m.Type)).ToArray();
            var mission = GetRandom(pool);
            await RespondAsync($"My Mission choice is **{mission.Name}** (*{mission.Type}{(mission.Type==MissionType.DLC?"":$", Group ID: {mission.GroupId}")}*)");
        }
    }
}
