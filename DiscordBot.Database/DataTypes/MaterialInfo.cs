namespace DiscordBot.Database.DataTypes
{
    public class MaterialInfo : IInfo
    {
        public Material Group {  get; set; }
        public int Tier {  get; set; }
        public string Name {  get; set; }

        public static string DBName => "Material";

        public string GetKey() => Group.ToString() + Tier;
    }
}