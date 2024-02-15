using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.AutoCompleters;

namespace DiscordBot.SlashCommands
{
    [HelpGroup("Patapon 3")]
    public class SetSkillSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, SetSkillInfo> _info;
        public SetSkillSlashModule(IDB db):base()
        {
            if (db.TryGetTable<SetSkillInfo>(out var info))
            {
                _info = info;
            }
        }
        [SlashCommand("ss", "See detailed set skill info.")]
        public async Task GetSsInfo(
            [Autocomplete(typeof(SetSkillDataAutoCompleteHandler))] string name)
        {
            if (_info == null)
            {
                await RespondAsync("This command is not functional, because the database is not loaded.");
                return;
            }
                if (!_info.TryGetValue(name, out SetSkillInfo info)
                    || info ==null)
                {
                    await RespondAsync("Incorrect parameter. Try again");
                    return;
                }
                var embed = new EmbedBuilder
                {
                    Title = info.Name,
                    Url = "https://rnielikki.github.io/pata/resources/skill.html"
                };
                embed.AddField("Obtained", $"{info.ClassName} level {info.Level}");
                embed.AddField("Called in", info.Called);
                if(!string.IsNullOrWhiteSpace(info.Detail)) embed.AddField("Detail", info.Detail);
                await RespondAsync(embed: embed.Build());
        }
    }
}
