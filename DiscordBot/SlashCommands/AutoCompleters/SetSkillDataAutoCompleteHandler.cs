using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.AutoCompleters
{
    internal class SetSkillDataAutoCompleteHandler : BigChoicesAutoCompleteHandler<SetSkillInfo>
    {
        public SetSkillDataAutoCompleteHandler(IDB db) : base(db)
        {
        }

        protected override int _resultAmount => 8;

        protected override string _tableName => "SS";

        protected override bool CompareForAutocompletion(string choice, string input) =>
            choice.Contains(input, StringComparison.InvariantCultureIgnoreCase);

        protected override string GetBigChoicesName(SetSkillInfo input) => input.Name;
    }
}