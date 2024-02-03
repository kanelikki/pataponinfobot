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

        public static string DBName => "Rarepon";

        public string GetKey() => RareponType.ToString();

        public int GetMaterial(int index) => index switch
        {
            0 => Material1,
            1 => Material2,
            2 => Material3,
            3 => Material4,
            _ => throw new IndexOutOfRangeException()
        };
    }
}
