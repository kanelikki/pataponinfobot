using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands;
using DiscordBot.SlashCommands.Models;
using Moq;

namespace DiscordBot.Tests
{
    public class P2RareponCalculationTest
    {
        private readonly RareponSlashModule _module;
        System.Collections.ObjectModel.ReadOnlyDictionary<string, RareClassInfo> _rareClassData =
                new(
                new Dictionary<string, RareClassInfo>()
            {
                {
                    "Yumipon",
                    new RareClassInfo()
                    {
                        ClassName = P2Class.Yumipon,
                        Material1 = Material.Fang,
                        Material2 = Material.Bone,
                        Material3 = Material.Mineral,
                        Material4 = Material.Tree,
                        Kaching = 14
                    }
                },
                {
                    "Megapon",
                    new RareClassInfo()
                    {
                        ClassName = P2Class.Megapon,
                        Material1= Material.Tree,
                        Material2 = Material.Vegetable,
                        Material3 = Material.Liquid,
                        Material4 = Material.Alloy,
                        Kaching = 24
                    }
                }
            });
            System.Collections.ObjectModel.ReadOnlyDictionary<string, RareponCalcInfo> _rareData = new(
                new Dictionary<string, RareponCalcInfo>()
            {
                {
                    "Fumya",
                    new RareponCalcInfo()
                    {
                        RareponType = P2Rarepon.Fumya,
                        Kaching = 2,
                        Material1Tier = 3,
                        Material2Tier = 1,
                        Material3Tier = 1,
                        Material4Tier = 1
                    }
                },
                {
                    "Mogyu",
                    new RareponCalcInfo()
                    {
                        RareponType = P2Rarepon.Mogyu,
                        Kaching = 5.5,
                        Material1Tier = 4,
                        Material2Tier = 5,
                        Material3Tier = 3,
                        Material4Tier = 4
                    }
                }
            });
        public P2RareponCalculationTest()
        {
            _module = new RareponSlashModule(Mock.Of<IDB>());
        }
        [Theory]
        [InlineData(P2Class.Yumipon, P2Rarepon.Fumya, 0, 5, 420,
            new[] { 7, 15, 6, 0 }, new[] { Material.Fang, Material.Bone, Material.Mineral, Material.Tree })]
        [InlineData(P2Class.Megapon, P2Rarepon.Mogyu, 5, 6, 792, new[] { 2, 2, 2, 1 }, new[] { Material.Tree, Material.Vegetable, Material.Liquid, Material.Alloy })]
        public void GetRequirement_Returns_CorrectData(
            P2Class className, P2Rarepon rarepon, int fromLevel, int toLevel,
            int kachingExpected, int[] materialAmountExpected, Material[] materialExpected)
        {
            var classData = _rareClassData[className.ToString()];
            var rareData = _rareData[rarepon.ToString()];
            UpgradeRequirementModel result =
                _module.GetRequirement(rareData, classData, fromLevel, toLevel);
            Assert.NotNull(result);
            Assert.NotNull(classData);
            Assert.Equal(kachingExpected, result.Kaching);
            for (int i = 0; i < 4; i++)
            {
                Assert.Equal(materialAmountExpected[i], result.MaterialAmounts[i]);
                Assert.Equal(materialExpected[i], classData.GetMaterial(i));
            }
            Assert.Equal(0, result.MaterialAmounts[4]);
        }
        private delegate void GetTable(out System.Collections.ObjectModel.ReadOnlyDictionary<string, RareClassInfo> data);
        private delegate void GetTable2(out System.Collections.ObjectModel.ReadOnlyDictionary<string, RareponCalcInfo> data);
    }
}
