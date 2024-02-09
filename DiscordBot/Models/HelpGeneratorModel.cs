namespace DiscordBot.Models
{
    public record HelpGeneratorModel
    {
        public string Name { get; init; }
        public string Group { get; init; }
        public string GroupDescription { get; init; }
        public IEnumerable<(string command, string description)> Commands { get; init; }
    }
}
