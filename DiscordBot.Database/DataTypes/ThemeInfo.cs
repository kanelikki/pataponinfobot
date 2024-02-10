namespace DiscordBot.Database.DataTypes
{
    public record ThemeInfo : IInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GetKey() => Name;
    }
}
