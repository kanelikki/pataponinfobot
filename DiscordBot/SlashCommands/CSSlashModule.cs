using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands
{
    public class CSSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Dictionary<string, CsGrindInfo> _info;
        public CSSlashModule():base()
        {
            _info = new TsvParser().ParseCS();
        }
        [SlashCommand("cs", "command_description")]
        public async Task ExecuteCommand([Autocomplete(typeof(ClassAutoComplete))]string className, int level, CsOohoroc group = CsOohoroc.None)
        {
            bool isOohoroc = className == "Oohoroc";
            if (level>5 || level<0 || !ClassAutoComplete.IsValidClassName(className, out string finalClassName)
                || !_info.TryGetValue(finalClassName+level+(isOohoroc?group:""), out CsGrindInfo info))
            {
                await RespondAsync("Incorrect parameter. Try again");
                return;
            }
            var embed = new EmbedBuilder
            {
                Title = info.SkillName,
                Description = $"{info.ClassName}"+(isOohoroc?(" "+group):"")+$" CS {info.Level}",
                Url = "https://docs.google.com/spreadsheets/d/10-YThzwJuxNJlMT9Y8fF3bQYJP3HGV1doaZ2ERn4vhg"
            };
            embed.AddField("Prerequest", info.Prerequest);
            embed.AddField("Trains from", $"{info.TrainingMethod} ({info.TrainsFrom})");
            embed.AddField("Train time", $"{info.TrainingTime} ({info.Exp} / 100000 exp per performing)");
            if (!string.IsNullOrEmpty(info.Note)) embed.AddField("Note", info.Note);
            if (!string.IsNullOrEmpty(info.Detail)) embed.AddField("Detail", info.Detail);

            await RespondAsync(embed: embed.Build());
        }
        [SlashCommand("aaa", "command_description")]
        public async Task ExecuteCommand()
        {
            await RespondAsync("Ok. ");
        }
    }
}
