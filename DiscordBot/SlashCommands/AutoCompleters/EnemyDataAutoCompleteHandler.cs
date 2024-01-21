using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.AutoCompleters
{
    internal class EnemyDataAutoCompleteHandler : BigChoicesAutoCompleteHandler<PveEnemyInfo>
    {
        public EnemyDataAutoCompleteHandler(IDB db) : base(db)
        {
        }

        protected override int _resultAmount => 8;

        protected override string _tableName => "PVE";

        protected override bool CompareForAutocompletion(string choice, string input) =>
            choice.Contains(input, StringComparison.InvariantCultureIgnoreCase);

        protected override string GetBigChoicesName(PveEnemyInfo input) => input.Name;
    }
}
