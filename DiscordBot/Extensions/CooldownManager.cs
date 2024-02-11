using Discord;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace DiscordBot
{
    internal class CooldownManager
    {
        public virtual TimeSpan Cooldown { get; }
        private readonly Dictionary<ulong, DateTime> _users = new();
        private readonly bool _enabled;
        internal CooldownManager(int cooldown){
            _enabled = cooldown > 0;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }
        internal bool IsCooldown(IUser user, out double cooldownTime)
        {
            if (!_enabled)
            {
                cooldownTime = -1;
                return false;
            }
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
