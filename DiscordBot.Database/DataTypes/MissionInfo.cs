namespace DiscordBot.Database.DataTypes
{
    public record MissionInfo : IInfo
    {
        public string GroupId { get; set; }
        public string Name { get; set; }
        public MissionType Type{get;set;}
        public string GetKey() => Name;
    }
}
