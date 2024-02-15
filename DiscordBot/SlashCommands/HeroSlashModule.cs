using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.AutoCompleters;

namespace DiscordBot.SlashCommands
{
    [HelpGroup("Patapon 3")]
    [Group("hero", "Provides information about a certain hero.")]
    public class HeroSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, HeroInfo> _info;
        private readonly Uri? _resourceUrl;
        public HeroSlashModule(IDB db, ISettingProvider settingProvider):base()
        {
            if (db.TryGetTable<HeroInfo>(out var info))
            {
                _info = info;
            }
            var url = settingProvider?.Setting?.ImageUrl;
            if (url == null) _resourceUrl = null;
            else _resourceUrl = new Uri(url);
        }
        [SlashCommand("info", "See detailed info about the hero.")]
        public async Task GetHeroInfo(
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
            if (_resourceUrl != null)
            {
                var file = $"./Heroes/{info.ClassName}.png";
                embed.WithThumbnailUrl(new Uri(_resourceUrl, file).ToString());
            }
            await RespondAsync(embed: embed.Build());
        }
        [SlashCommand("sound", "Get the sound of the hero.")]
        public async Task GetHeroSound(
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
            var file = $"./Resources/HeroSound/{info.ClassName}.wav";
            if (!File.Exists(file))
            {
                await RespondAsync("The heromode sound file does not exist!");
                return;
            }
            await RespondWithFileAsync(file, text:$"**{info.ClassName}** Heromode Sound");
        }
    }
}
