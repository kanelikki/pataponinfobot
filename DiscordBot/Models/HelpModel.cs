namespace DiscordBot.Models
{
    public record HelpModel
    {
        public string Group { get; init; }
        public string GroupDescription { get; init; }
        public IEnumerable<(string command, string description)> Commands { get; init; }
    }
}
