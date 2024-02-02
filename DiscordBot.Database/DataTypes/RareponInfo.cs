using CsvHelper.Configuration.Attributes;

namespace DiscordBot.Database.DataTypes
{
    public class RareponInfo : IInfo
    {
        [Name("Type")]
        public P2Rarepon RareponType { get; set; }
        public double Kaching { get; set; }
        public int Material1 { get; set; }
        public int Material2 { get; set; }
        public int Material3 { get; set; }
        public int Material4 { get; set; }
        public string GetKey() => RareponType.ToString();
    }
}
