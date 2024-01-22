using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Database.DataTypes
{
    public record HeroInfo : IInfo
    {
        [Name("Class")]
        public string ClassName { get; set; }
        public string Equipment { get; set; }
        public string Acquistition { get; set; }
        [Name("Class Skills")]
        public string CS { get; set; }
        [Name("Set Skills")]
        public string SS { get; set; }
        [Name("CS inherit from")]
        public string InheritFrom { get; set; }
        [Name("CS inherit to")]
        public string InheritTo { get; set; }

        public string GetKey() => ClassName;
    }
}
