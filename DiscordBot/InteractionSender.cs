using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace DiscordBot
{
    internal class InteractionSender
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _serviceProvider;
        private readonly InteractionService _interactionService;
        private readonly ILogger _logger;
        private readonly CooldownManager _cooldownManager;
        internal InteractionSender(DiscordSocketClient client, IServiceProvider serviceProvider, ILogger logger)
        {
            _client = client;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _interactionService = new InteractionService(_client.Rest);
            _cooldownManager = new CooldownManager();
        }
        internal async Task InitAsync()
        {
            await _interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);

            //GUILD to register command
            await _interactionService.RegisterCommandsToGuildAsync(1055494833021661296);
            _client.InteractionCreated += SendInteraction;
            await _logger.Log("*** INTERACTION POWER LAUNCHED! ***", LogSeverity.Info);
        }
        private async Task SendInteraction(SocketInteraction interaction)
        {
            var scope = _serviceProvider.CreateScope();
            var ctx = new SocketInteractionContext(_client, interaction);
            try
            {
                if (interaction.Type == InteractionType.ApplicationCommand
                    && _cooldownManager.IsCooldown(interaction.User, out var cooldown))
                {
                    await interaction.RespondAsync(
                        $"Please calm down! Wait for {cooldown.ToString("F2", CultureInfo.InvariantCulture)} seconds.",
                        ephemeral:true);
                    return;
                }
                await _interactionService.ExecuteCommandAsync(ctx, scope.ServiceProvider);
            }
            catch(Exception ex)
            {
                await _logger.Log("Interaction failed: " + ex.Message,LogSeverity.Error);
            }
        }
    }
}
