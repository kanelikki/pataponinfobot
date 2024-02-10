using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.AutoCompleters;
using System.Collections.ObjectModel;

namespace DiscordBot.SlashCommands
{
    [HelpGroup("Patapon 3")]
    [Group("theme", "Find DxD theme by ID or Name.")]
    public class ThemeSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private ReadOnlyDictionary<string, ThemeInfo> _info;
        private ReadOnlyDictionary<int, ThemeInfo> _infoAsId;
        public ThemeSlashModule(IDB db)
        {
            if (db.TryGetTable<ThemeInfo>(out var info) && info!=null)
            {
                _info = info;
                _infoAsId = info.ToDictionary(kv => kv.Value.Id, kv => kv.Value).AsReadOnly();
            }
        }
        [SlashCommand("id", "Find theme by ID.")]
        public async Task FindThemeById(int id)
        {
            if (_infoAsId == null)
            {
                await RespondAsync("Database is not initialised.");
            }
            else if (id < 1 || id > _infoAsId.Count)
            {
                await RespondAsync($"Incorrect ID. Expected to be range of 1 ~ {_infoAsId.Count}");
            }
            else if (_infoAsId.TryGetValue(id, out var info) && info != null)
            {
                await DisplayThemeInfo(info);
            }
            else await RespondAsync($"Cannot find the theme with {id}.");
        }
        [SlashCommand("name", "Find theme by theme name.")]
        public async Task FindThemeByName(
            [Autocomplete(typeof(ThemeAutoCompleteHandler))]string name)
        {
            if ( _info == null)
            {
                await RespondAsync("Database is not initialised.");
            }
            else if (_info.TryGetValue(name, out var info) && info != null)
            {
                await DisplayThemeInfo(info);
            }
            else await RespondAsync($"Cannot find the theme with the name.");
        }
        private async Task DisplayThemeInfo(ThemeInfo info)
        {
            var embed = new EmbedBuilder()
            {
                Title = info.Name
            };
            embed.AddField("BGM ID", string.Format("BGM_{0:D2}",info.Id));
            embed.AddField("Description", info.Description.Replace("\\n", "\n"));
            await RespondAsync(embed: embed.Build());
        }
    }
}
