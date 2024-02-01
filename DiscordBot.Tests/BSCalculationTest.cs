using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands;
using Moq;

namespace DiscordBot.Tests
{
    //This tests with real Patapon 3 Blacksmith value.
    public class BSCalculationTest
    {
        private BSSlashModule _module;
        Dictionary<Enchant, double> _enchantInfo = new Dictionary<Enchant, double>();
        public BSCalculationTest()
        {
            double[] _baseEnc = new[]
            {
                1, 1.2, 1.3, 1.4, 1.6, 2, 3, 4
            };
            _enchantInfo = new Dictionary<Enchant, double>();
            for (int i = 0; i < 8; i++)
            {
                var enchant = (Enchant)i;
                _enchantInfo.Add(enchant, _baseEnc[i]);
                if (i > 0)
                {
                    enchant = (Enchant)(i+7);
                    _enchantInfo.Add(enchant, _baseEnc[i]);
                }
            }
            _module = new BSSlashModule(Mock.Of<IDB>());
        }
        //testing test
        [Fact]
        public void BSCalculationTest_CorrectInitTest()
        {
            Assert.Equal(15, _enchantInfo.Count);
            Assert.Equal(4, _enchantInfo[Enchant.G]);
            Assert.Equal(1.2, _enchantInfo[Enchant.Hp]);
            Assert.Equal(4, _enchantInfo[Enchant.Me]);
        }
        [Theory]
        [InlineData(40, 80, Enchant.H, 78752, 39, 4, 3, 2, 1)]
        [InlineData(21, 90, Enchant.W, 21861, 20, 3, 2, 1, 0)]
        [InlineData(20, 110, Enchant.G, 69410, 19, 2, 1, 0, 0)]
        public void BSSlashModel_Requires_CorrectValue(int itemLevel, int kaching, Enchant enchant,
            int kachingExpected, int t1Expected, int t2Expected, int t3Expected, int t4Expected, int t5Expected)
        {
            var result = _module.CalculateRequirement(itemLevel, kaching, _enchantInfo[enchant]);
            Assert.Equal(kachingExpected, result.Kaching);
            Assert.Equal(t1Expected, result.MaterialAmounts[0]);
            Assert.Equal(t2Expected, result.MaterialAmounts[1]);
            Assert.Equal(t3Expected, result.MaterialAmounts[2]);
            Assert.Equal(t4Expected, result.MaterialAmounts[3]);
            Assert.Equal(t5Expected, result.MaterialAmounts[4]);
        }
    }
}
