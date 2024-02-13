using CsvHelper.Configuration.Attributes;

namespace DiscordBot.Database.DataTypes
{
    public record EnchantInfo : IInfo
    {
        public Enchant Type { get; set; }
        [Name("Basic")]
        public string BasicInfo { get; set; }
        [Name("Bonus")]
        public string BonusInfo { get; set; }
        [Name("Status")]
        public string StatusInfo { get; set; }

        public string GetKey() => Type.ToString();
    }
}
