// See https://aka.ms/new-console-template for more information
using Discord;
using DiscordBot;

await new Bot(new ConsoleLogger()).StartAsync();

public class ConsoleLogger : ILogger
{
    public Task Log(string message, LogSeverity severity)
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
