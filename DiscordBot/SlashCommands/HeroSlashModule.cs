using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.AutoCompleters;

namespace DiscordBot.SlashCommands
{
    [HelpGroup("Patapon 3")]
    public class HeroSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, HeroInfo> _info;
        public HeroSlashModule(IDB db):base()
        {
            if (db.TryGetTable<HeroInfo>(out var info))
            {
                _info = info;
            }
        }
        [SlashCommand("hero", "see detailed info about the hero.")]
        public async Task GetCsInfo(
            [Autocomplete(typeof(ClassDataAutoCompleteHandler))]string className)
        {
            if (_info == null)
            {
                await RespondAsync("This command is not functional, because the database is not loaded.");
                return;
            }
            if (!_info.TryGetValue(className, out HeroInfo info) || info ==null)
            {
                await RespondAsync("Incorrect parameter. Try again");
                return;
            }
            var embed = new EmbedBuilder
            {
                Title = info.ClassName
            };
            embed.AddField("Can Equip", info.Equipment);
            embed.AddField("Obtain with", info.Acquistition);
            embed.AddField("Class Skills", info.CS);
            embed.AddField("Set Skills", info.SS);
            embed.AddField("CS Inherit From", info.InheritFrom);
            embed.AddField("CS Inherit To", info.InheritTo);
            await RespondAsync(embed: embed.Build());
        }
    }
}
