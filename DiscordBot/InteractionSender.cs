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
        private readonly Setting _setting;
        internal InteractionSender(DiscordSocketClient client, IServiceProvider serviceProvider, ILogger logger)
        {
            _client = client;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _interactionService = new InteractionService(_client.Rest);
            _setting = serviceProvider.GetService<ISettingProvider>().Setting;
            var cooldownTime = _setting.Cooldown;
            _cooldownManager = new CooldownManager(cooldownTime);
        }
        internal async Task InitAsync()
        {
            await _interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);

            //GUILD to register command
            await _interactionService.RegisterCommandsToGuildAsync(_setting.GuildID);
            _client.InteractionCreated += SendInteraction;
            _interactionService.InteractionExecuted += _interactionService_InteractionExecuted;
            await _logger.LogAsync("*** INTERACTION POWER LAUNCHED! *** :: Now you can use your command :)", LogSeverity.Info);
        }

        private async Task _interactionService_InteractionExecuted(ICommandInfo commandInfo, IInteractionContext interactionContext, IResult result)
        {
            if (!result.IsSuccess)
            {
                await interactionContext.Interaction.RespondAsync("> *Oh no! An internal error occurred. Check the log, admins!*");
                await _logger.LogAsync($"Interaction failure issue ({result.Error}) : {result.ErrorReason}", LogSeverity.Error);
            }
        }

        private async Task SendInteraction(SocketInteraction interaction)
        {
            try
            {
                var scope = _serviceProvider.CreateScope();
                var ctx = new SocketInteractionContext(_client, interaction);
                if (interaction.Type == InteractionType.ApplicationCommand
                    && _cooldownManager.IsCooldown(interaction.User, out var cooldown))
                {
                    await interaction.RespondAsync(
                        $"Please calm down! Wait for {cooldown.ToString("F2", CultureInfo.InvariantCulture)} seconds.",
                        ephemeral: true);
                    return;
                }
                await _interactionService.ExecuteCommandAsync(ctx, scope.ServiceProvider);

            }
            catch (Exception ex)
            {
                await _logger.LogAsync("Interaction failed: " + ex.Message, LogSeverity.Error);
            }
        }
    }
}