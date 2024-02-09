using CsvHelper.Configuration.Attributes;

namespace DiscordBot.Database.DataTypes
{
    public class RareponCalcInfo : IInfo
    {
        [Name("Type")]
        public P2Rarepon RareponType { get; set; }
        public double Kaching { get; set; }
        [Name("Material1")]
        public int Material1Tier { get; set; }
        [Name("Material2")]
        public int Material2Tier { get; set; }
        [Name("Material3")]
        public int Material3Tier { get; set; }
        [Name("Material4")]
        public int Material4Tier { get; set; }

        public string GetKey() => RareponType.ToString();

        public int GetMaterial(int index) => index switch
        {
            0 => Material1Tier,
            1 => Material2Tier,
            2 => Material3Tier,
            3 => Material4Tier,
            _ => throw new IndexOutOfRangeException()
        };
    }
}
