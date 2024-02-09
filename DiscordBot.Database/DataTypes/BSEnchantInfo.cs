namespace DiscordBot.Database.DataTypes
{
    public class BSEnchantInfo : IInfo
    {
        public Enchant Enchant { get; set; }
        public double Multiplier { get; set; }
        public string GetKey() => Enchant.ToString();
    }
}
