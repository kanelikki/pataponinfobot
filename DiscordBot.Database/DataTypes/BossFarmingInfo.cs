using CsvHelper.Configuration.Attributes;

namespace DiscordBot.Database.DataTypes
{
    public record BossFarmingInfo : IInfo
    {
        public P2Boss Boss { get; set; }
        public Material Material { get; set; }
        public bool Thunder { get; set; }
        public bool Ice { get; set; }
        public bool Knockback { get; set; }
        public bool Sleep { get; set; }
        public StatusType Weakness { get; set; }
        [Optional]
        public string? NoStagger { get; set; }

        public string GetKey() => Boss.ToString();
    }
}
