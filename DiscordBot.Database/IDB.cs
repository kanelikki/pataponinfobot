using DiscordBot.Database.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Database
{
    /// <summary>
    /// Provides interface for testing purpose.
    /// </summary>
    public interface IDB
    {
        public bool TryGetTable<T>(string tableName, out Dictionary<string, T>? result) where T : IInfo;
    }
}
