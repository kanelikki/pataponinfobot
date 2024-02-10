using Discord;
using Discord.Interactions;
using DiscordBot.Models;
using System.Collections.ObjectModel;
using System.Text;

namespace DiscordBot.SlashCommands
{
    public class HelpSlashModule: InteractionModuleBase<SocketInteractionContext>
    {
        private ReadOnlyDictionary<string, IEnumerable<HelpModel>> _helpModels;
        public HelpSlashModule(ISettingProvider settingProvider)
        {
            _helpModels = new HelpGenerator()
                .GenerateHelp(
                    settingProvider.Setting.GenerateNoHelpGroup,
                    settingProvider.Setting.OtherCommandsLabel);
        }
        [NoHelp]
        [SlashCommand("help", "Shows  help")]
        public async Task ShowHelp()
        {
            if (!_helpModels.Any())
            {
                await RespondAsync("No Help Found!");
                return;
            }
            var embed = new EmbedBuilder()
            {
                Title = "Command Help",
            };
            embed.WithFooter("This help is automatically generated.");
            foreach(var modelGroup in _helpModels) {
                var text = new StringBuilder();
                bool first = true;
                foreach(var model in modelGroup.Value)
                {
                    bool hasGroup = !string.IsNullOrWhiteSpace(model.Group);
                    foreach (var command in model.Commands)
                    {
                        text.Append($"{(first?"":"\n")}`/{(hasGroup ? model.Group+" ":"")}{command.command}` : {command.description}");
                        first = false;
                    }
                }
                if (modelGroup.Value.Any())
                {
                    embed.AddField(modelGroup.Key, text.ToString());
                }
            }
            await RespondAsync(embed: embed.Build());
        }
    }
}
