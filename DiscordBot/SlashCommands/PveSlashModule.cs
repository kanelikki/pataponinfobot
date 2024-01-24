using Discord;
using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using DiscordBot.SlashCommands.AutoCompleters;

namespace DiscordBot.SlashCommands
{
    public class PveSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Dictionary<string, PveEnemyInfo> _info;
        public PveSlashModule(IDB db):base()
        {
            if (db.TryGetTable<PveEnemyInfo>("PVE", out var info))
            {
                _info = info;
            }
        }
        [SlashCommand("monster", "see detailed pve enemy strength and weakness.")]
        public async Task GetPveInfo([Autocomplete(typeof(EnemyDataAutoCompleteHandler))]string name)
        {
            if (_info == null)
            {
                await RespondAsync("This command is not functional, because the database is not loaded.");
                return;
            }
            if (!_info.TryGetValue(name, out var info) || info == null)
            {
                await RespondAsync("Incorrect parameter. Try again");
                return;
            }
            var embed = new EmbedBuilder
            {
                Title = info.Name,
                Url = "https://docs.google.com/spreadsheets/d/12nTfa2zWSFwjSnqWw9KE0WaSpNoucuurPieBx7S9oT4"
            };
            embed.AddField("Enemy Type", info.EnemyType);
            embed.AddField("Immune to", info.Immunities);
            embed.AddField("Strong Against", BuildStringFromWeakness(info.GetStrengthes()));
            embed.AddField("Weak Against", BuildStringFromWeakness(info.GetWeaknesses()));
            await RespondAsync(embed: embed.Build());
        }
        internal string BuildStringFromWeakness(KeyValuePair<AttackElement, float>[] weaknesses)
        {
            if (weaknesses.Length < 1) return "None";
            List<string> res = new List<string>();
            foreach (var weakness in weaknesses)
            {
                bool ifNotable = weakness.Value <= 0.7f || weakness.Value >= 1.5f;
                res.Add($"{(ifNotable?"**":"")}{weakness.Key} ({weakness.Value.ToString("P0")}){(ifNotable?"**":"")}");
            }
            return string.Join(" ,", res);
        }
    }
}
