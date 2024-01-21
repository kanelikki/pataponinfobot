using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.Tests
{
    public class CSParserTests
    {
        private readonly Dictionary<string, CsGrindInfo> _grindInfo;
        public CSParserTests()
        {
            _grindInfo = new TsvParser().Parse<CsGrindInfo>("data/CS.tsv", StringComparer.OrdinalIgnoreCase);
        }
        [Fact]
        public void ParseString_WithoutGroup()
        {
            var record = _grindInfo["Traffikrab2"];
            Assert.Equal("Claw Attack", record.SkillName);
            Assert.Equal(999, record.Exp);
            Assert.True(record.Dupe);
        }
        [Fact]
        public void ParseString_WithGroup()
        {
            var record = _grindInfo["Samurai1."];
            Assert.True(string.IsNullOrEmpty(record.TrainingTime));
            Assert.True(string.IsNullOrEmpty(record.Note));
            Assert.False(string.IsNullOrEmpty(record.TrainingMethod));
            Assert.False(record.Dupe);
        }
    }
}