// See https://aka.ms/new-console-template for more information
using Discord;
using DiscordBot;
using DiscordBot.Database;

await new Bot(new ConsoleLogger(), new ConsoleDBLogger())
    .StartAsync();

public class ConsoleLogger : ILogger
{
    public void Log(string message, LogSeverity severity)
    {
        Console.WriteLine($"<{severity}> {message}");
    }

    public Task LogAsync(string message, LogSeverity severity)
    {
        return Console.Out.WriteAsync($"[{severity}] {message}\n");
    }

    public void LogException(IReadOnlyCollection<DiscordJsonError> error)
    {
        foreach (var err in error)
        {
            Console.WriteLine($"[ {err.Path} ]");
            Console.WriteLine("** EXCEPTION ** : " + err.Errors);
        }
    }
}
public class ConsoleDBLogger : IDbLogger
{
    public void LogDBMessage(string message)
    {
        Console.WriteLine("**DB MESSAGE**\n"+message);
    }
}
