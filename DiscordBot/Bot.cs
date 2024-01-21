using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.Database;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiscordBot
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private InteractionService _interactionService;
        public Bot(ILogger logger)
        {
            _logger = logger;
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.None
            };
            var collection = new ServiceCollection();
            collection.AddSingleton<IDB, DB>();
            _client = new DiscordSocketClient(config);
            _serviceProvider = collection.BuildServiceProvider();
        }

        public async Task StartAsync()
        {

            _client.Ready += InitAsync;
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, CoreSettings.BotInfo.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task InitAsync()
        {
            _interactionService = new InteractionService(_client.Rest);
            await _interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
            await _interactionService.RegisterCommandsToGuildAsync(1055494833021661296);

            _client.InteractionCreated += async (x) =>
            {
                var scope = _serviceProvider.CreateScope();
                var ctx = new SocketInteractionContext(_client, x);
                try
                {
                    await _interactionService.ExecuteCommandAsync(ctx, scope.ServiceProvider);
                }
                catch(Exception ex)
                {
                    await _logger.Log("Interaction failed: " + ex.Message,LogSeverity.Error);
                }
            };
        }

        private Task Log(LogMessage msg) => _logger.Log(msg.ToString(), msg.Severity);
    }
}