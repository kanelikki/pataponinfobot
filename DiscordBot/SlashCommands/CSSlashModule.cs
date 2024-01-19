using Discord;
using Discord.Interactions;

namespace DiscordBot.SlashCommands
{
    public class CSSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        public CSSlashModule():base()
        {
        }
        [SlashCommand("cs", "command_description")]
        public async Task ExecuteCommand([Autocomplete(typeof(ClassAutoComplete))]string className, int level)
        {
            if (level>5 || level<0)
            {
                await RespondAsync("CS value is incorrect.");
                return;
            }
            await RespondAsync("Ok. "+className.ToString());
        }
        [SlashCommand("aaa", "command_description")]
        public async Task ExecuteCommand()
        {
            await RespondAsync("Ok. ");
        }
    }
}
