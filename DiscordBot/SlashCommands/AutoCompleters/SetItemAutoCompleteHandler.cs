using DiscordBot.Database;
using DiscordBot.Database.DataTypes;

namespace DiscordBot.SlashCommands.AutoCompleters
{
    internal class SetItemAutoCompleteHandler : BigChoicesAutoCompleteHandler<SetItemInfo>
    {
        internal SetItemAutoCompleteHandler(IDB db) : base(db)
        {
        }
        protected override int _resultAmount => 8;

        protected override bool CompareForAutocompletion(string choice, string input) =>
            choice.Contains(input, StringComparison.InvariantCultureIgnoreCase);
        protected override IEnumerable<string> InitBigChoices() =>
            GetData().SelectMany(x => x.Value.SetItems).Distinct();

        protected override string GetBigChoicesName(SetItemInfo input) => input.Name;

    }
}
