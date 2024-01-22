using CsvHelper.Configuration.Attributes;
using DiscordBot.SlashCommands.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Database.DataTypes
{
    public class SetSkillInfo : IInfo
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
