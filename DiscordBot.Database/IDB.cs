using DiscordBot.Database.DataTypes;
using System.Collections.ObjectModel;

namespace DiscordBot.Database
{
    /// <summary>
    /// Provides interface for testing purpose.
    /// </summary>
    public interface IDB
    {
        public bool TryGetTable<T>(out ReadOnlyDictionary<string, T>? result) where T : IInfo;
    }
}
