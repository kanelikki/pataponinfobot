using Discord;
using Discord.WebSocket;
using DiscordBot.Database;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly InteractionSender _interactionSender;
        public Bot(ILogger logger, IDbLogger dbLogger = null)
        {
            _logger = logger;
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.None
            };
            var collection = new ServiceCollection();
            if (dbLogger != null) collection.AddSingleton<IDB, DB>(_ => new DB(dbLogger));
            else collection.AddSingleton<IDB, DB>();
            _client = new DiscordSocketClient(config);
            _serviceProvider = collection.BuildServiceProvider();
            _interactionSender = new InteractionSender(_client, _serviceProvider, _logger);
        }
        public async Task StartAsync()
        {
            _client.Ready += _interactionSender.InitAsync;
            _client.Log += Log;

            try
            {
                var token = await new TokenLoader().GetTokenAsync(_logger);
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
            }
            catch(Exception ex)
            {
                await _logger.LogAsync(ex.Message, LogSeverity.Critical);
                throw;
            }

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg) => _logger.LogAsync(msg.ToString(), msg.Severity);
    }
}