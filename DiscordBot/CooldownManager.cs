using Discord;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace DiscordBot
{
    internal class CooldownManager
    {
        public virtual TimeSpan Cooldown { get; } = new TimeSpan(0, 0, 10);
        private readonly Dictionary<ulong, DateTime> _users = new();
        internal bool IsCooldown(IUser user, out double cooldownTime)
        {
            if (!_users.TryGetValue(user.Id, out DateTime time))
            {
                _users.Add(user.Id, DateTime.UtcNow);
                cooldownTime = -1;
                return false;
            }
            var diff = DateTime.UtcNow - time;
            if (diff < Cooldown)
            {
                cooldownTime = (Cooldown - diff).TotalSeconds;
                return true;
            }
            else
            {
                _users[user.Id] = DateTime.UtcNow;
                cooldownTime = -1;
                return false;
            }
        }
    }
}
