using Discord;

namespace DiscordBot
{
    public interface ILogger
    {
        /// <summary>
        /// Writes log Synchronously. Mainly used outside Discord command, which works synchronously.
        /// </summary>
        /// <param name="message">Message to be displayed in the log.</param>
        /// <param name="severity">How serious the status/error is.</param>
        void Log(string message, LogSeverity severity);
        /// <summary>
        /// Writes log Asynchronously. Mainly used with Discord internal status and command.
        /// </summary>
        /// <param name="message">Message to be displayed in the log.</param>
        public Task LogAsync(string message, Discord.LogSeverity severity);
        /// <summary>
        /// Logs exception as json.
        /// </summary>
        /// <param name="message">Serialized log message.</param>
        /// <returns></returns>
        public void LogException(IReadOnlyCollection<DiscordJsonError> error);
    }
}