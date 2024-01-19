namespace DiscordBot
{
    public interface ILogger
    {
        public Task Log(string message, Discord.LogSeverity severity);
        /// <summary>
        /// Logs exception as json.
        /// </summary>
        /// <param name="message">Serialized log message.</param>
        /// <returns></returns>
        public void LogException(IReadOnlyCollection<Discord.DiscordJsonError> error);
    }
}