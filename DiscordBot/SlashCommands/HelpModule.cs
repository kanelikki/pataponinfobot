using Discord;
using Discord.Interactions;
using Discord.Rest;
using DiscordBot.Extensions;
using DiscordBot.Models;
using System.Text;

namespace DiscordBot.SlashCommands
{
    public class HelpModule: InteractionModuleBase<SocketInteractionContext>
    {
        private IEnumerable<HelpGeneratorModel> _helpModels;
        public HelpModule()
        {
            _helpModels = new HelpGenerator().GenerateHelp();
        }
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
            embed.WithFooter("Note: This help is automatically generated.");
            var text = new StringBuilder();
            bool first = true;
            foreach(var model in _helpModels)
            {
                bool hasGroup = !string.IsNullOrWhiteSpace(model.Group);
                //var text = new StringBuilder();
                //bool first = true;
                foreach (var command in model.Commands)
                {
                    text.Append($"{(first?"":"\n")}`/{(hasGroup ? model.Group+" ":"")}{command.command}` : {command.description}");
                    first = false;
                }
                /* for shorter line we don't use...
                if (!first) //has any description
                {
                    embed.AddField(model.Name, text.ToString());
                }
                */
            }
            embed.AddField("Here are the available commands:", text.ToString());
            await RespondAsync(embed: embed.Build());
        }
    }
}
