using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.AutoCompleters;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Text;

namespace DiscordBot.SlashCommands
{
    [HelpGroup("Patapon 2")]
    [Group("mask", "Gets mask stat, or gets mask drop info from a boss")]
    public class MaskSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private ReadOnlyDictionary<string, MaskInfo> _info;
        private ReadOnlyDictionary<string, IEnumerable<MaskInfo>> _infoByBoss;
        public MaskSlashModule(IDB db)
        {
            if (db.TryGetTable<MaskInfo>(out var info) && info!=null)
            {
                _info = info;
                _infoByBoss = info
                    .GroupBy(b => b.Value.Boss, b => b.Value)
                    .ToDictionary(b => b.Key, b => b.AsEnumerable())
                    .AsReadOnly();
            }
        }
        [SlashCommand("info", "Gets mask stats.")]
        public async Task GetMaskInfo([Autocomplete(typeof(MaskAutoCompleteHandler))]string mask)
        {
            if (_info == null)
            {
                await RespondAsync("Database is not ready.");
                return;
            }
            if (!_info.TryGetValue(mask, out var info) || info == null)
            {
                await RespondAsync("The info doesn't exist.");
                return;
            }
            var embed = new EmbedBuilder
            {
                Title = $"info.Name",
            };
            embed.AddField("Obtain from", info.Boss);
          SetBasicInfo(embed, info);
            FillFloatSection(
                embed,
                "Status Chances",
                    new[]
                    {
                        ("Crit", info.Critical),
                        ("KB", info.Knockback),
                        ("Cnc", info.Cnc),
                        ("Ignite", info.Ignite),
                        ("Sleep", info.Sleep),
                        ("Freeze", info.Freeze),
                    }
                );
            FillFloatSection(
                embed,
                "Status Effect Resists",
                    new[]
                    {
                        ("Crit Res", info.CriticalResist),
                        ("KB Res", info.KnockbackResist),
                        ("Cnc Res", info.CncResist),
                        ("Ignite Res", info.IgniteResist),
                        ("Sleep Res", info.SleepResist),
                        ("Freeze Res", info.FreezeResist),
                    }
                );
            FillFloatSection(
                embed,
                "Damage Taken",
                new[]
                {
                    ("vs Sword/Axe/Hammer", info.MeleeNormalDmgTaken),
                    ("vs Kiba Lance", info.MeleeLanceDmgTaken),
                    ("vs Spear/Javelin", info.SpearDmgTaken),
                    ("vs Bow", info.BowDmgTaken),
                    ("vs (Unknown)", info.UnknownDmgTaken),
                    ("vs Crush/Sound", info.CrushMagicDmgTaken),
                    ("vs Fire", info.FireDmgTaken),
                    ("vs Ice", info.IceDmgTaken),
                    ("vs Lightning", info.LightningDmgTaken),
                }, 1, "**{0}** : {1:P0}"
                );
            await RespondAsync(embed:embed.Build());
        }
        [SlashCommand("egg", "Gets mask info from the boss name (egg).")]
        public async Task GetMaskByBoss([Autocomplete(typeof(MaskBossAutoCompleteHandler))]string egg)
        {
            if (_infoByBoss == null)
            {
                await RespondAsync("Database is not ready.");
                return;
            }
            if(!_infoByBoss.TryGetValue(egg, out var info) || info == null)
            {
                await RespondAsync("Data not found.");
                return;
            }
            var embed = new EmbedBuilder()
            {
                Title = $"Mask from {egg} Egg"
            };
            var text = new StringBuilder();
            bool first = true;
            foreach (var line in info)
            {
                if (!first) text.Append(", ");
                text.Append(line.Name);
                first = false;
            }
            var lastData = info.Last();
            List<string> affectedStats = new List<string>();
            AddInfo(affectedStats,nameof(lastData.Stamina), lastData.Stamina, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats,nameof(lastData.MovementSpeed), lastData.MovementSpeed-1, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats,nameof(lastData.AttackSpeed), lastData.AttackSpeed-1, 0, "{0} ({1:+;-})");

            AddInfo(affectedStats, nameof(lastData.Critical), lastData.Critical, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, nameof(lastData.Knockback), lastData.Knockback, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, nameof(lastData.Cnc), lastData.Cnc, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, nameof(lastData.Ignite), lastData.Ignite, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, nameof(lastData.Sleep), lastData.Sleep, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, nameof(lastData.Freeze), lastData.Freeze, 0, "{0} ({1:+;-})");

            AddInfo(affectedStats, nameof(lastData.CriticalResist), lastData.CriticalResist, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, nameof(lastData.KnockbackResist), lastData.KnockbackResist, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, nameof(lastData.CncResist), lastData.CncResist, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, nameof(lastData.IgniteResist), lastData.IgniteResist, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, nameof(lastData.SleepResist), lastData.SleepResist, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, nameof(lastData.FreezeResist), lastData.FreezeResist, 0, "{0} ({1:+;-})");

            AddInfo(affectedStats, "vs Sword/Axe/Hammer", lastData.MeleeNormalDmgTaken - 1, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, "vs Kiba Lance", lastData.MeleeLanceDmgTaken - 1, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, "vs Spear/Javelin", lastData.SpearDmgTaken - 1, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, "vs Bow", lastData.BowDmgTaken - 1, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, "vs Unknown", lastData.UnknownDmgTaken - 1, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, "vs Crush/Sound", lastData.CrushMagicDmgTaken - 1, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, "vs Fire", lastData.FireDmgTaken - 1, 0, "{0} ({1:+;-})");
            AddInfo(affectedStats, "vs Ice", lastData.IceDmgTaken - 1, 0, "{0} ({1:+;-})");

            embed.AddField("Affected stats", string.Join(", ", affectedStats));
            await RespondAsync(embed:embed.Build());
        }
        private void SetBasicInfo(EmbedBuilder embed, MaskInfo info)
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
                | AddInfo(items, "Attk Speed", info.AttackSpeed, 1, "**{0}** : x{1}");
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
