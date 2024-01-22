using Discord;
using Discord.Interactions;

namespace DiscordBot.SlashCommands
{
    public class VsTeamSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Random _random = new Random();
        [SlashCommand("vsteam", "Arranges vs team. Available for at least 2v")]
        public async Task SetVsTeam(IUser user1, IUser user2, IUser user3, IUser user4,
            IUser? user5 = null, IUser? user6 = null, IUser? user7 = null, IUser? user8 = null)
        {

            var users = new IUser[] { user1, user2, user3, user4, user5, user6, user7, user8 }
                .Where(u => u != null);
            var repeat = users.GroupBy(u => u.Id).Where(g => g.Count() > 1).SelectMany(g => g).Select(u => u.Mention).Distinct();
            if (repeat.Any())
            {
                await RespondAsync($"Duplicated user found : {string.Join(",",repeat)}. This command will not be performed.");
                return;
            }
            await Shuffle(users.ToArray());
        }
        private async Task Shuffle(IUser[] users)
        {
            if (users.Length % 2 != 0)
            {
                await RespondAsync($"The player amount must be even number. (Current count: {users.Length})");
                return;
            }
            IUser temp;
            int halfLength = users.Length/2;
            for (int i = 0; i < users.Length; i++)
            {
                int rand = _random.Next(0,i);
                if (rand != i)
                {
                    temp = users[i];
                    users[i] = users[rand];
                    users[rand] = temp;
                }
            }
            var team1 = string.Join(",", users.Take(halfLength).Select(u => u.Mention));
            var team2 = string.Join(",", users.Skip(halfLength).Select(u => u.Mention));

            var embed = new EmbedBuilder
            {
                Title = $"{halfLength} vs {halfLength} team shuffled",
                Description = $"With Team {_random.Next(1,3)}'s rule"
            };
            embed.AddField("Team 1", team1);
            embed.AddField("Team 2", team2);
            await RespondAsync(embed: embed.Build());
        }
    }
}
