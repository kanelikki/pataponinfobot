using CsvHelper.Configuration.Attributes;

namespace DiscordBot.Database.DataTypes
{
    public record SetSkillInfo : IInfo
    {
        public string Name { get; set; }
        [Name("Class")]
        public string ClassName { get; set; }
        [Name("Obtain Level")]
        public int Level { get; set; }
        public string Called { get; set; }
        [Optional]
        public string Detail { get; set; }

        public string GetKey() => Name;
    }
}
