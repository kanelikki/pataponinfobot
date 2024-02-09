using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.AutoCompleters;

namespace DiscordBot.SlashCommands
{
    public class ClassSkillSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, CsGrindInfo> _info;
        public ClassSkillSlashModule(IDB db):base()
        {
            if (db.TryGetTable<CsGrindInfo>(out var info))
            {
                _info = info;
            }
        }
        [SlashCommand("cs", "see detailed class skill grinding info.")]
        public async Task GetCsInfo(
            [Autocomplete(typeof(ClassDataAutoCompleteHandler))]string className,
            int level, CsOohoroc group = CsOohoroc.None)
        {
            if (_info == null)
            {
                await RespondAsync("This command is not functional, because the database is not loaded.");
                return;
            }
            bool isOohoroc = className.ToLower() == "oohoroc";
            if (level>5 || level<0 || !_info.TryGetValue(className+level+(isOohoroc?group:""), out CsGrindInfo info)
                || info ==null)
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
            if (!string.IsNullOrWhiteSpace(info.Note)) embed.AddField("Note", info.Note);
            if (!string.IsNullOrWhiteSpace(info.Detail)) embed.AddField("Detail", info.Detail);

            await RespondAsync(embed: embed.Build());
        }
    }
}
