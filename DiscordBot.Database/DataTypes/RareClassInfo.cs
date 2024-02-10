using CsvHelper.Configuration.Attributes;

namespace DiscordBot.Database.DataTypes
{
    public record RareClassInfo : IInfo
    {
        [Name("Class")]
        public P2Class ClassName { get; set; }
        public Material Material1 { get; set; }
        public Material Material2 { get; set; }
        public Material Material3 { get; set; }
        public Material Material4 { get; set; }
        public int Kaching { get; set; }

        public string GetKey() => ClassName.ToString();
        public Material GetMaterial(int index) => index switch
        {
            0 => Material1,
            1 => Material2,
            2 => Material3,
            3 => Material4,
            _ => throw new IndexOutOfRangeException()
        };
    }
}
