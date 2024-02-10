using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using System.Text;

namespace DiscordBot.SlashCommands
{
    [HelpGroup("Patapon 2")]
    public class BossFarmingInfoSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly System.Collections.ObjectModel.ReadOnlyDictionary<string, BossFarmingInfo> _farmingInfo;
        private readonly Uri? _imageUrl;
        public BossFarmingInfoSlashModule(IDB db, ISettingProvider settingProvider)
        {
            if (db.TryGetTable<BossFarmingInfo>(out var farmingInfo))
            {
                _farmingInfo = farmingInfo;
            }
            var url = settingProvider?.Setting?.ImageUrl;
            if (url == null) _imageUrl = null;
            else _imageUrl = new Uri(url);
        }
        [SlashCommand("boss", "Get Patapon 2 Boss information for farming.")]
        public async Task BossFarmingInfo(P2Boss boss)
        {
            if (_farmingInfo == null)
            {
                await RespondAsync("The database doesn't exist :(");
                return;
            }
            var bossName = boss.ToString();
            if (!_farmingInfo.TryGetValue(bossName, out var info) || info == null)
            {
                await RespondAsync("No information found.");
                return;
            }
            var builder = new EmbedBuilder()
            {
                Title = bossName,
                Url = "https://rnielikki.github.io/pata/p2/material/"
            };
            builder.AddField("Material", info.Material);
            builder.AddField("Status effect for tier 3-5 materials", BuildStatusAvailability(info));
            if (!string.IsNullOrWhiteSpace(info.NoStagger))
            {
                builder.AddField("Attack that cannot be staggered", info.NoStagger);
                if (_imageUrl != null)
                {
                    var file = $"./Bosses/{info.Boss}.gif";
                    builder.WithImageUrl(new Uri(_imageUrl, file).ToString());
                }
            }
            await RespondAsync(embed:builder.Build());
        }
        private string BuildStatusAvailability(BossFarmingInfo info)
        {
            var result = new StringBuilder();
            bool isFirst = true;
            SetAvailability(info.Ice, StatusType.Ice);
            SetAvailability(info.Knockback, StatusType.Knockback);
            SetAvailability(info.Sleep, StatusType.Sleep);

            if (isFirst) return "*None*";
            else return result.ToString();

            void SetAvailability(bool statusPossibility, StatusType statusType)
            {
                if (statusPossibility)
                {
                    if (!isFirst)
                    {
                        result.Append(", ");
                    }
                    if (info.Weakness == statusType)
                    {
                        result.Append($"**{statusType.ToString()}**");
                    }
                    else result.Append(statusType.ToString());
                    isFirst = false;
                }
            }
        }
    }
}
