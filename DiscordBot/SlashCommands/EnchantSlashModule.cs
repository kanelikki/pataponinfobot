using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands
{
    [HelpGroup("Patapon 3")]
    public class EnchantSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IReadOnlyDictionary<string, EnchantInfo> _info;
        public EnchantSlashModule(IDB db)
        {
            if (db.TryGetTable<EnchantInfo>(out var info) && info != null) _info = info;
        }
        [SlashCommand("enchant", "Gets stat of equipment enchant.")]
        public async Task GetEnchant(Enchant enchant)
        {
            if (_info == null)
            {
                await RespondAsync("No database found.");
                return;
            }
            if (!_info.TryGetValue(enchant.ToString(), out var info) || info == null)
            {
                await RespondAsync("The enchantment doesn't exist.");
                return;
            }
            var embed = new EmbedBuilder()
            {
                Title = info.Type.ToString()
            };
            AddIfExist("Basic Info", info.BasicInfo);
            AddIfExist("Attack/Defensive Bonus", info.BonusInfo);
            AddIfExist("Status Chance/Resistance Info", info.StatusInfo);
            await RespondAsync(embed: embed.Build());
            void AddIfExist(string label, string text)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    embed.AddField(label, text.Replace("\\n","\n"));
                }
            }
        }

    }
}
