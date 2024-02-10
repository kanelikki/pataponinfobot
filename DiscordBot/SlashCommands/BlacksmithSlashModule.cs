using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.AutoCompleters;
using DiscordBot.SlashCommands.Models;
using System.Text;

namespace DiscordBot.SlashCommands
{
    [HelpGroup("Patapon 3")]
    public class BlacksmithSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, BSEnchantInfo> _enchantInfo;
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, BSInfo> _info;
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, MaterialInfo> _materialInfo;
        private BSBaseRequirementModel[] _preCalculated;
        public BlacksmithSlashModule(IDB db):base()
        {
            if (db.TryGetTable<BSInfo>(out var info))
            {
                _info = info;
            }
            if (db.TryGetTable<BSEnchantInfo>(out var enchantInfo))
            {
                _enchantInfo = enchantInfo;
            }
            if (db.TryGetTable<MaterialInfo>(out var materialInfo))
            {
                _materialInfo = materialInfo;
            }
            _preCalculated = LoadBSRequirements();
        }
        [SlashCommand("bs", "Calculates material and Ka-Ching fo blacksmith.")]
        public async Task GetBsInfo(
            [Autocomplete(typeof(EquipDataAutoCompleteHandler))] string equipment, Enchant enchant, int fromLevel, int toLevel)
        {
            if (_info == null || _enchantInfo == null)
            {
                await RespondAsync("The database does not exist. You cannot use this command :(");
                return;
            }
            if (fromLevel < 0 || fromLevel > 39 || toLevel < 1 || toLevel > 40 || fromLevel >= toLevel)
            {
                await RespondAsync($"Invalid level range {fromLevel} - {toLevel}. `fromLevel` is CURRENT equipment level and `toLevel` is TARGET equipment level.");
                return;
            }
            if (!_info.TryGetValue(equipment, out var info))
            {
                await RespondAsync("Cannot find the equipment. Try using the suggestion from slash command.");
                return;
            }
            if ((info.IsWeapon && (int)enchant > 7) || (!info.IsWeapon && (int)enchant < 8 && enchant !=0))
            {
                await RespondAsync($"**{info.Equipment} [{enchant}]** does not exist LMFAO. The item is {(info.IsWeapon?"WEAPON":"ARMOUR")}!");
                return;
            }
            var result = UpgradeRequirementModel.GetEmpty();
            for (int i = fromLevel; i < toLevel; i++)
            {
                if (!_enchantInfo.TryGetValue(enchant.ToString(), out BSEnchantInfo enchantInfo))
                {
                    await RespondAsync("Internal error: Enchant data not found.");
                    return;
                }
                var requirement = CalculateRequirement(i+1, info.Kaching, enchantInfo.Multiplier);
                if (requirement == null)
                {
                    await RespondAsync("Failed to calculate. Maybe wrong enchant name? Enchant name is case sensitive.");
                    return;
                }
                result.Add(requirement);
            }

            var embed = new EmbedBuilder
            {
                Title = info.Equipment + ((enchant == Enchant.None) ? "" : $" [{enchant}]"),
                Description = $"From {fromLevel} To {toLevel}",
                Url = "https://rnielikki.github.io/pata/blacksmith/"
            };
            embed.AddField("Ka-Ching", result.Kaching);
            StringBuilder materialResultBuilder = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                if (result.MaterialAmounts[i] < 1) continue;
                string materialName = info.Material.ToString() + (i+1);
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
        internal UpgradeRequirementModel? CalculateRequirement(int itemLevel, int baseKaching, double multiplier)
        {
            if (itemLevel > 0 && itemLevel <= 40)
            {
                var preData = _preCalculated[itemLevel - 1];
                int kaching = (int)Math.Round(baseKaching * multiplier * preData.Kaching);
                return new UpgradeRequirementModel()
                {
                    Kaching = kaching,
                    MaterialAmounts = preData.MaterialAmounts
                };
            }
            else return null;
        }
        private BSBaseRequirementModel[] LoadBSRequirements()
        {
            var preCalculated = new BSBaseRequirementModel[40];
            for (int i = 0; i< 40; i++) {
                preCalculated[i] = GetOneRequirement(i);
            }
            return preCalculated;
        }

        private BSBaseRequirementModel GetOneRequirement(int level)
        {
            //material
            var material = GetMaterialRequirements(level);
            //ka-ching
            return new BSBaseRequirementModel()
            {
                MaterialAmounts = material,
                Kaching = 0.375 * level * level + 1.125 * level + 1
            };
        }
        private int[] GetMaterialRequirements(int level)
        {
            var res = new int[5];
            for (int tier = 1; tier < 6; tier++)
            {
                res[tier - 1] = GetMaterialRequirementByTier(level, tier);
            }
            return res;
        }
        private int GetMaterialRequirementByTier(int level, int tier)
        {
            if (tier == 1) return level;
            else if (level <= 20)
            {
                return Math.Max((int)Math.Ceiling((double)(level + 1) / 5) - tier, 0);
            }
            else
            {
                return (int)Math.Ceiling((double)(level + 1) / 10) - tier + 2;
            }
        }
    }
}
