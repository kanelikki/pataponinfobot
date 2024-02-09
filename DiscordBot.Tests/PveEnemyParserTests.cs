
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands;
using Moq;

namespace DiscordBot.Tests
{
    public class PveEnemyParserTests
    {
        private readonly Dictionary<string, PveEnemyInfo> _enemyInfo;
        public PveEnemyParserTests()
        {
            _enemyInfo = new DB().Parse("data/PveEnemyInfo.tsv", typeof(PveEnemyInfo))
                .ToDictionary(v => v.Key, v => (PveEnemyInfo)v.Value);
        }
        [Fact]
        public void PveParseString_EnemyTypeTest()
        {
            var record = _enemyInfo["Chicken"];
            Assert.Equal("Dragon", record.EnemyType);
        }
        [Fact]
        public void PveParseString_EnemyResistanceTest()
        {
            var record = _enemyInfo["Fish"];
            Assert.Equal(1.2f, record.LightTaken, 0.001);
        }
        [Fact]
        public void PveParseString_WeaknessOrderTest()
        {
            Assert.Single(_enemyInfo["Fish"].GetWeaknesses());
            Assert.Equal(3, _enemyInfo["Fish"].GetStrengthes().Length);
            Assert.Equal(1.5f, _enemyInfo["Bear"].GetWeaknesses()[0].Value, 0.001);
            Assert.Equal(AttackElement.Sound, _enemyInfo["Cat"].GetStrengthes()[0].Key);
            Assert.Equal(0.75, _enemyInfo["Cat"].GetStrengthes()[1].Value);
        }
        [Fact]
        public void PveSlashModule_EmptyWeakness_ReturnsNone()
        {
            var module = new EnemyInfoSlashModule(Mock.Of<IDB>());
            var result = module.BuildStringFromWeakness(Array.Empty<KeyValuePair<AttackElement, float>>());
            Assert.Equal("None", result);
        }
    }
}
