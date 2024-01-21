using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.AutoCompleters
{
    internal class ClassDataAutoCompleteHandler : BigChoicesAutoCompleteHandler<CsGrindInfo>
    {
        public ClassDataAutoCompleteHandler(IDB db) : base(db)
        {
        }

        protected override int _resultAmount => 10;

        protected override string _tableName => "CS";

        protected override bool CompareForAutocompletion(string choice, string input) =>
            choice.ToLower().StartsWith(input.ToLower());

        protected override string GetBigChoicesName(CsGrindInfo input) => input.ClassName;
    }
}
