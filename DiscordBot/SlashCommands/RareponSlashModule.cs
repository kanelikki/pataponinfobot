using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.Models;
using System.Text;

namespace DiscordBot.SlashCommands
{
    public class RareponSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, RareponInfo> _rareInfo;
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, RareClassInfo> _classInfo;
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, MaterialInfo> _materialInfo;
        public RareponSlashModule(IDB db):base()
        {
            if (db.TryGetTable<RareponInfo>(out var rareInfo))
            {
                _rareInfo = rareInfo;
            }
            if (db.TryGetTable<RareClassInfo>(out var classInfo))
            {
                _classInfo = classInfo;
            }
            if (db.TryGetTable<MaterialInfo>(out var materialInfo))
            {
                _materialInfo = materialInfo;
            }
        }
        [SlashCommand("rarepon", "Calculates material and Ka-Ching fo Rarepon (Patapon 2).")]
        public async Task GetRareponInfo(P2Rarepon rarepon, P2Class className, int fromLevel, int toLevel)
        {
            if (_rareInfo == null || _classInfo == null)
            {
                await RespondAsync("The database does not exist. You cannot use this command :(");
                return;
            }
            if (fromLevel < 0 || fromLevel > 9 || toLevel < 1 || toLevel > 10 || fromLevel >= toLevel)
            {
                await RespondAsync($"Invalid level range {fromLevel} - {toLevel}. `fromLevel` is CURRENT Rarepon level and `toLevel` is TARGET Rarepon level.");
                return;
            }
            if (!_rareInfo.TryGetValue(rarepon.ToString(), out var rareponData)
                || !_classInfo.TryGetValue(className.ToString(), out var classData) || rareponData == null || classData == null)
            {
                await RespondAsync("The Rarepon or Class could not found.");
                return;
            }
            UpgradeRequirementModel result = GetRequirement(rareponData, classData, fromLevel, toLevel);

            var embed = new EmbedBuilder
            {
                Title = $"{rarepon} {className}",
                Description = $"From {fromLevel} To {toLevel}",
                Url = "https://rnielikki.github.io/pata/p2/rarepon"
            };
            embed.AddField("Ka-Ching", result.Kaching);
            StringBuilder materialResultBuilder = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                if (result.MaterialAmounts[i] < 1) continue;
                string materialName = classData.GetMaterial(i).ToString() + rareponData.GetMaterial(i);
                if (_materialInfo != null
                     && _materialInfo.TryGetValue(materialName, out var materialInfo))
                {
                    materialName = materialInfo.Name;
                }
                materialResultBuilder.Append($"**{materialName}**: {result.MaterialAmounts[i]}\n");
            }
            if (materialResultBuilder.Length > 0)
            {
                embed.AddField("Materials", materialResultBuilder.ToString());
            }
            await RespondAsync(embed: embed.Build());
        }
        internal UpgradeRequirementModel GetRequirement(RareponInfo rareponInfo, RareClassInfo classInfo, int fromLevel, int toLevel)
        {
            UpgradeRequirementModel result = UpgradeRequirementModel.GetEmpty();
            for(var i = fromLevel; i < toLevel; i++)
            {
                var level = i + 1;
                //rarepon level : i
                //material level: rareponMaterialData.MaterialX
                var amountData = new UpgradeRequirementModel() {
                    Kaching = (int)Math.Round(level * classInfo.Kaching * rareponInfo.Kaching),
                    MaterialAmounts = new[] {
                        CalculateAmount(level, rareponInfo.Material1),
                        CalculateAmount(level, rareponInfo.Material2),
                        CalculateAmount(level, rareponInfo.Material3, 2),
                        CalculateAmount(level, rareponInfo.Material4, 5),
                        0
                    }
                };
                result.Add(amountData);
            }
            return result;
        }
        //startLevel starts from 0, not 1 
        private int CalculateAmount(int rareponLevel, int materialLevel, int startLevel = 0) {
            if (materialLevel == 0) return 0;
            var res = (int)Math.Ceiling((double)(rareponLevel - startLevel)/materialLevel);
            return (res < 0)?0:res;
        }
    }
}
