using CsvHelper.Configuration.Attributes;

namespace DiscordBot.Database.DataTypes
{
    public record CsGrindInfo : IInfo
    {
        [Name("Class")]
        public string ClassName { get; set; }
        public int Level { get; set; }
        [Optional]
        public string Group { get; set; } //valid for oohoroc only
        [Name("CS Name")]
        public string SkillName { get; set; }
        public string Prerequest { get; set; }
        public int Exp { get; set; }
        [Name("Training Time")]
        [Optional]
        public string TrainingTime { get; set; }
        [Name("Trains From")]
        public string TrainsFrom { get; set; }
        [Name("Training Method")]
        public string TrainingMethod { get; set; }
        [Name("Doi Dupe")]
        public bool Dupe { get; set; }
        [Optional]
        public string Note { get; set; }
        [Optional]
        public string Detail { get; set; } //from Mielikki's note

        public static string DBName => "CS";

        public string GetKey()
        {
            return ClassName + Level + Group;
        }
    }
}
