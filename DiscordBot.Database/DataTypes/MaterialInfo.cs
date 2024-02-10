namespace DiscordBot.Database.DataTypes
{
    public record MaterialInfo : IInfo
    {
        public Material Group {  get; set; }
        public int Tier {  get; set; }
        public string Name {  get; set; }
        public string GetKey() => Group.ToString() + Tier;
    }
}