using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.AutoCompleters;
using System.Collections.ObjectModel;

namespace DiscordBot.SlashCommands
{
    [HelpGroup("Patapon 3")]
    public class SetItemSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly ReadOnlyDictionary<string, SetItemInfo> _info;
        private readonly IEnumerable<SetItemInfo> _infoList;
        public SetItemSlashModule(IDB db)
        {
            if (db.TryGetTable<SetItemInfo>(out var info))
            {
                _info = info;
                _infoList = info.Select(x => x.Value);
            }
        }
        [SlashCommand("set","Gets set item information")]
        public async Task GetSetItemInfo(
            [Autocomplete(typeof(SetItemAutoCompleteHandler))]string item1,
            [Autocomplete(typeof(SetItemAutoCompleteHandler))]string item2="",
            [Autocomplete(typeof(SetItemAutoCompleteHandler))]string item3="",
            [Autocomplete(typeof(SetItemAutoCompleteHandler))]string item4="")
        {
            if (_infoList == null)
            {
                await RespondAsync("The database does not exist.");
                return;
            }
            var infoList = FindItemInfo(item1, item2, item3, item4);
            if (infoList.Count < 1)
            {
                await RespondAsync("The set item combination doesn't exist!");
                return;
            }
            var embed = new EmbedBuilder();
            if (infoList.Count > 1)
            {
                embed.WithTitle("More than one available set found:");
                embed.AddField("Available sets:", string.Join("\n", infoList.Select(d => d.ItemsRaw)));
            }
            else
            {
                var info = infoList[0];
                embed.WithTitle(string.Join(" / ", info.SetItems));
                embed.AddField("Stats", info.Description.Replace("/","\n"));
            }
            await RespondAsync(embed: embed.Build());
        }
        private List<SetItemInfo> FindItemInfo(string item1, string item2, string item3="", string item4="")
        {
            return _infoList.Where(s => EmptyOrContains(s, item1) && EmptyOrContains(s, item2)
                    && EmptyOrContains(s, item3) && EmptyOrContains(s, item4)).ToList();

            bool EmptyOrContains(SetItemInfo info, string item) =>
                (string.IsNullOrWhiteSpace(item) || info.SetItems.Contains(item));
        }
    }
}
