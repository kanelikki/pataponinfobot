using Discord.Interactions;
using DiscordBot.Database;
using DiscordBot.Database.DataTypes;
using System.Collections.ObjectModel;

namespace DiscordBot.SlashCommands.Base
{
    public abstract class SlashModule  : InteractionModuleBase<SocketInteractionContext>
    {
        public SlashModule(IDB db)
        {
        }
        protected ReadOnlyDictionary<string, T>? LoadDB<T>(IDB db) where T:IInfo
        {
            if (db.TryGetTable<T>(out var info))
            {
                return info;
            }
            return null;
        }
    }
}
