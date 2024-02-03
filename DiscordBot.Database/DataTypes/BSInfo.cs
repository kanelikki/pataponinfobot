namespace DiscordBot.Database.DataTypes
{
    public class BSInfo : IInfo
    {
        public string Equipment { get; set; }
        public bool IsWeapon { get; set; }
        public Material Material { get; set; }
        public int Kaching { get; set; }

        public static string DBName => "BSData";

        public string GetKey() => Equipment;
    }
}
