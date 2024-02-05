using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.Models;
using System.Reflection.Emit;
using System.Text;

namespace DiscordBot.SlashCommands
{
    [Group("rarepon", "contains some Patapon 2 Rarepon information.")]
    public class RareponSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, RareponCalcInfo> _rareMatInfo;
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, RareponInfo> _rareponInfo;
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, RareClassInfo> _classInfo;
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, MaterialInfo> _materialInfo;
        public RareponSlashModule(IDB db) : base()
        {
            if (db.TryGetTable<RareponInfo>(out var rareponInfo))
            {
                _rareponInfo = rareponInfo;
            }
            if (db.TryGetTable<RareponCalcInfo>(out var rareMatInfo))
            {
                _rareMatInfo = rareMatInfo;
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
        [SlashCommand("upgrade", "Calculates material and Ka-Ching fo Rarepon.")]
        public async Task GetRareponUpgradeInfo(P2Rarepon rarepon, P2Class className, int fromLevel, int toLevel)
        {
            if (_rareMatInfo == null || _classInfo == null)
            {
                await RespondAsync("The database does not exist. You cannot use this command :(");
                return;
            }
            if (fromLevel < 0 || fromLevel > 9 || toLevel < 1 || toLevel > 10 || fromLevel >= toLevel)
            {
                await RespondAsync($"Invalid level range {fromLevel} - {toLevel}. `fromLevel` is CURRENT Rarepon level and `toLevel` is TARGET Rarepon level.");
                return;
            }
            if (!_rareMatInfo.TryGetValue(rarepon.ToString(), out var rareponData)
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
        internal UpgradeRequirementModel GetRequirement(RareponCalcInfo rareponInfo, RareClassInfo classInfo, int fromLevel, int toLevel)
        {
            UpgradeRequirementModel result = UpgradeRequirementModel.GetEmpty();
            for (var i = fromLevel; i < toLevel; i++)
            {
                var level = i + 1;
                //rarepon level : i
                //material level: rareponMaterialData.MaterialX
                var amountData = new UpgradeRequirementModel()
                {
                    Kaching = (int)Math.Round(level * classInfo.Kaching * rareponInfo.Kaching),
                    MaterialAmounts = new[] {
                        CalculateAmount(level, rareponInfo.Material1Tier),
                        CalculateAmount(level, rareponInfo.Material2Tier),
                        CalculateAmount(level, rareponInfo.Material3Tier, 2),
                        CalculateAmount(level, rareponInfo.Material4Tier, 5),
                        0
                    }
                };
                result.Add(amountData);
            }
            return result;
        }
        //startLevel starts from 0, not 1 
        private int CalculateAmount(int rareponLevel, int materialLevel, int startLevel = 0)
        {
            if (materialLevel == 0) return 0;
            var res = (int)Math.Ceiling((double)(rareponLevel - startLevel) / materialLevel);
            return (res < 0) ? 0 : res;
        }
        [SlashCommand("info", "Shows information of the Rarepon. If the level is 0, this doesn't calculate stat.")]
        public async Task GetRareponInfo(P2Rarepon rarepon, int level)
        {
            if (level < 1 || level > 10)
            {
                await RespondAsync($"The level value {level} is invalid.");
                return;
            }
            if (_rareponInfo == null)
            {
                await RespondAsync("The database does not exist. You cannot use this command :(");
                return;
            }
            if (!_rareponInfo.TryGetValue(rarepon.ToString() + level, out var data))
            {
                await RespondAsync("Invalid Rarepon name.");
                return;
            }
            var embed = new EmbedBuilder
            {
                Title = $"{data.Name} ({data.Rarepon} Lv. {data.Level})",
            };
            SetBasicInfo(embed, data);
            FillFloatSection(
                embed,
                "Status Chances",
                    new[]
                    {
                        ("Crit", data.Critical),
                        ("KB", data.Knockback),
                        ("Cnc", data.Cnc),
                        ("Ignite", data.Ignite),
                        ("Sleep", data.Sleep),
                        ("Freeze", data.Freeze),
                    }
                );
            FillFloatSection(
                embed,
                "Status Effect Resists",
                    new[]
                    {
                        ("Crit Res", data.CriticalResist),
                        ("KB Res", data.KnockbackResist),
                        ("Cnc Res", data.CncResist),
                        ("Ignite Res", data.IgniteResist),
                        ("Sleep Res", data.SleepResist),
                        ("Freeze Res", data.FreezeResist),
                    }
                );
            FillBoolSection(
                embed,
                "Status Effect Immunity",
                    new[]
                    {
                        ("Critical", data.CritImmunity),
                        ("Knockback", data.KBImmunity),
                        ("Cnc", data.CncImmunity),
                        ("Ignite", data.BurnImmunity),
                        ("Sleep", data.SleepImmunity),
                        ("Freeze", data.FreezeImmunity),
                    }
                );
            FillFloatSection(
                embed,
                "Damage Taken",
                new[]
                {
                    ("vs Sword/Axe/Hammer", data.MeleeNormalDmgTaken),
                    ("vs Kiba Lance", data.MeleeLanceDmgTaken),
                    ("vs Spear/Javelin", data.SpearDmgTaken),
                    ("vs Bow", data.BowDmgTaken),
                    ("vs (Unknown)", data.UnknownDmgTaken),
                    ("vs Crush/Sound", data.CrushMagicDmgTaken),
                    ("vs Fire", data.FireDmgTaken),
                    ("vs Ice", data.IceDmgTaken),
                    ("vs Lightning", data.LightningDmgTaken),
                }, 1, "**{0}** : {1:P0}"
                );
            embed.WithFooter("Elemental weapon has pure elemental type! For example, Ice sword DOESN'T apply vs sword.");
            await RespondAsync(embed: embed.Build());
        }
        private void SetBasicInfo(EmbedBuilder embed, RareponInfo info)
        {
            var isAdded = false;
            List<string> items = new List<string>();
            isAdded |= AddInfo(items,nameof(info.Stamina), info.Stamina, 0);
            if (info.MinDmg != 0 || info.MaxDmg != 0)
            {
                items.Add($"**Damage**: {info.MinDmg.ToString("+#;-#;+0")} ~ {info.MaxDmg.ToString("+#;-#;+0")}");
                isAdded = true;
            }
            isAdded |= AddInfo(items, "Mov. Speed", info.MovementSpeed, 1, "**{0}** : x{1}")
                | AddInfo(items, "Attk Speed", info.AttackSpeed, 1, "**{0}** : x{1}")
                | AddInfo(items, "KB Power", info.KBPower, 0)
                | AddInfo(items, "Weight", info.Weight, 0);
            if (isAdded)
            {
                embed.AddField("Basic Info",string.Join('\n', items));
            }
        }
        private void FillFloatSection(EmbedBuilder embed, string globalLabel, (string dataLabel, float dataValue)[] data,
            float ignoreValue = 0, string format = "**{0}** : {1:+# %;-# %;+0 %}")
        {
            var isAdded = false;
            List<string> items = new List<string>();
            foreach ((var label, float value) in data)
            {
                isAdded |= AddInfo(items, label, value, ignoreValue, format);
            }
            if (isAdded)
            {
                embed.AddField(globalLabel, string.Join('\n', items));
            }
        }
        private void FillBoolSection(EmbedBuilder embed, string globalLabel, (string dataLabel, bool dataValue)[] data)
        {
            var isAdded = false;
            List<string> items = new List<string>();
            foreach ((var label, bool value) in data)
            {
                isAdded |= AddInfo(items, label, value, false, "{0}");
            }
            if (isAdded)
            {
                embed.AddField(globalLabel, string.Join(", ", items));
            }
        }
        private bool AddInfo<T>(List<string> list, string label, T value, T ignoreValue, string format = "**{0}** : {1:+#;-#;}") where T : IEquatable<T>
        {
            if (!value.Equals(ignoreValue))
            {
                list.Add(string.Format(System.Globalization.CultureInfo.InvariantCulture, format, label, value));
                return true;
            }
            return false;
        }
    }
}
